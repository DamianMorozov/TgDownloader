// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloader.Helpers;

/// <summary>
/// Client helper.
/// </summary>
public sealed partial class TgClientHelper : ObservableObject, ITgHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgClientHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgClientHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    private static TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
    private static TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
    private static TgSqlContextManagerHelper ContextManager => TgSqlContextManagerHelper.Instance;
    private static TgLogHelper TgLog => TgLogHelper.Instance;
    public Client? Client { get; set; }
    public TgExceptionModel ClientException { get; private set; }
    public TgExceptionModel ProxyException { get; private set; }
    public bool IsReady { get; private set; }
    public bool IsNotReady => !IsReady;
    public bool IsProxyUsage { get; private set; }
    public User? Me { get; set; }
    public Dictionary<long, ChatBase> DicChatsAll { get; private set; }
    public Dictionary<long, ChatBase> DicChatsUpdated { get; }
    public Dictionary<long, User> DicUsersUpdated { get; }
    public IEnumerable<Channel> EnumerableChannels { get; set; }
    public IEnumerable<Channel> EnumerableGroups { get; set; }
    public IEnumerable<ChatBase> EnumerableChats { get; set; }
    public IEnumerable<ChatBase> EnumerableSmallGroups { get; set; }
    public bool IsUpdateStatus { get; set; }
    //private object ChannelUpdateLocker { get; }
    private IEnumerable<TgSqlTableFilterModel> Filters { get; set; }
    private object Locker { get; }

    public Func<string, Task> UpdateTitleAsync { get; private set; }
    public Func<string, Task> UpdateStateConnectAsync { get; private set; }
    public Func<string, Task> UpdateStateProxyAsync { get; private set; }
    public Func<string, Task> UpdateStateMessageAsync { get; private set; }
    public Func<long, int, string, Task> UpdateStateSourceAsync { get; private set; }
    public Func<string, int, string, string, Task> UpdateStateExceptionAsync { get; private set; }
    public Func<Task> AfterClientConnectAsync { get; private set; }
    public Func<string, string?> GetClientDesktopConfigAsync { get; private set; }

    public TgClientHelper()
    {
        DicChatsAll = new();
        DicChatsUpdated = new();
        DicUsersUpdated = new();
        EnumerableChannels = Enumerable.Empty<Channel>();
        EnumerableChats = Enumerable.Empty<ChatBase>();
        EnumerableGroups = Enumerable.Empty<Channel>();
        EnumerableSmallGroups = Enumerable.Empty<ChatBase>();
        ClientException = new();
        ProxyException = new();
        //ChannelUpdateLocker = new();
        Filters = Enumerable.Empty<TgSqlTableFilterModel>();
        Locker = new();

        UpdateTitleAsync = async _ => await Task.CompletedTask;
        UpdateStateConnectAsync = async _ => await Task.CompletedTask;
        UpdateStateProxyAsync = async _ => await Task.CompletedTask;
        UpdateStateMessageAsync = async _ => await Task.CompletedTask;
        UpdateStateExceptionAsync = async (_, _, _, _) => await Task.CompletedTask;
        UpdateStateSourceAsync = async (_, _, _) => await Task.CompletedTask;
        AfterClientConnectAsync = async () => await Task.CompletedTask;
        GetClientDesktopConfigAsync = what => null;

#if DEBUG
        // TgLog to VS Output debugging pane in addition.
        WTelegram.Helpers.Log = (i, str) => Debug.WriteLine($"{i} | {str}");
#else
        // Disable logging in Console.
        WTelegram.Helpers.Log = (_, _) => { };
#endif
    }

    #endregion

    #region Public and private methods

    public string ToDebugString() => $"{TgCommonUtils.GetIsReady(IsReady)} | {Me}";

    public void SetupActions(Func<string, Task> updateStateConnect, Func<string, Task> updateStateProxy,
        Func<string, Task> updateStateMessage, Func<long, int, string, Task> updateStateSource,
        Func<string, int, string, string, Task> updateStateException, 
        Func<Task> afterClientConnect, Func<string, string?> getClientDesktopConfig)
    {
        UpdateStateConnectAsync = updateStateConnect;
        UpdateStateProxyAsync = updateStateProxy;
        UpdateStateMessageAsync = updateStateMessage;
        UpdateStateSourceAsync = updateStateSource;
        UpdateStateExceptionAsync = updateStateException;
        AfterClientConnectAsync = afterClientConnect;
        GetClientDesktopConfigAsync = getClientDesktopConfig;
    }

    public void SetupActions(Func<string, Task> updateTitle, Func<string, Task> updateStateMessage, Func<long, int, string, Task> updateStateSource)
    {
        UpdateTitleAsync = updateTitle;
        UpdateStateConnectAsync = UpdateStateMessageAsync;
        UpdateStateProxyAsync = UpdateStateMessageAsync;
        UpdateStateMessageAsync = updateStateMessage;
        UpdateStateSourceAsync = updateStateSource;
    }

    public bool CheckClientIsReady()
    {
        bool ResultDisconnected()
        {
            UpdateStateSourceAsync(0, 0, string.Empty);
            UpdateStateMessageAsync(string.Empty);
            UpdateStateProxyAsync(TgLocale.ProxyIsDisconnect);
            UpdateStateConnectAsync(TgLocale.MenuClientIsDisconnected);
            return IsReady = false;
        }
        bool ResultConnected()
        {
            UpdateStateConnectAsync(TgLocale.MenuClientIsConnected);
            return IsReady = true;
        }
        bool result = Client is { Disconnected: false };
        if (!result) 
            return ResultDisconnected();
        if (!TgAppSettings.AppXml.IsExistsFileSession)
            return ResultDisconnected();

        //if (!(!TgAppSettings.AppXml.IsUseProxy ||
        //      (TgAppSettings.AppXml.IsUseProxy &&
        //       (ContextManager.ProxyRepository.Get(ContextManager.AppRepository.GetFirstProxyUid) ??
        //        ContextManager.ProxyRepository.GetNew()).IsExists)))
        Guid proxyUid = ContextManager.AppRepository.GetFirstProxyUidAsync().Result;
        if (!(!TgAppSettings.AppXml.IsUseProxy ||
              (TgAppSettings.AppXml.IsUseProxy &&
               (ContextManager.ProxyRepository.GetAsync(proxyUid) ??
                ContextManager.ProxyRepository.GetNewAsync()).Result.IsExists)))
            return ResultDisconnected();
        if (ProxyException.IsExists || ClientException.IsExists)
            return ResultDisconnected();
        return ResultConnected();
    }

    public void ConnectSessionConsole(Func<string, string?>? config, TgSqlTableProxyModel proxy)
    {
        if (IsReady)
            return;
        Disconnect();

        Client = new(config);
        ConnectThroughProxyAsync(proxy, false).GetAwaiter().GetResult();
        Client.OnUpdate += OnUpdateClientAsync;
        Client.OnOther += OnClientOtherAsync;

        LoginUserConsole(true);
    }

    public async Task ConnectSessionAsync(TgSqlTableProxyViewModel proxyVm)
    {
        if (IsReady) return;
        Disconnect();

        Client = new(GetClientDesktopConfigAsync);
        await ConnectThroughProxyAsync(proxyVm.Proxy, true);
        Client.OnUpdate += OnUpdateClientAsync;
        Client.OnOther += OnClientOtherAsync;

        await LoginUserDesktopAsync(true);
    }

    public async Task ConnectThroughProxyAsync(TgSqlTableProxyModel proxy, bool isDesktop)
    {
        IsProxyUsage = false;
        if (!CheckClientIsReady()) return;
        if (Client is null) return;
        if (proxy.IsNotExists) return;
        if (!isDesktop && !TgAppSettings.AppXml.IsUseProxy) return;
        if (Equals(proxy.Type, TgEnumProxyType.None)) return;
        if (!TgSqlUtils.GetValidXpLite(proxy).IsValid) return;

        try
        {
            ProxyException = new();
            IsProxyUsage = true;
            switch (proxy.Type)
            {
                case TgEnumProxyType.Http:
                case TgEnumProxyType.Socks:
                    Client.TcpHandler = (address, port) =>
                    {
                        Socks5ProxyClient proxyClient = string.IsNullOrEmpty(proxy.UserName) && string.IsNullOrEmpty(proxy.Password)
                            ? new(proxy.HostName, proxy.Port) : new(proxy.HostName, proxy.Port, proxy.UserName, proxy.Password);
                        UpdateStateProxyAsync(TgLocale.ProxyIsConnected);
                        return Task.FromResult(proxyClient.CreateConnection(address, port));
                    };
                    break;
                case TgEnumProxyType.MtProto:
                    Client.MTProxyUrl = string.IsNullOrEmpty(proxy.Secret)
                        ? $"https://t.me/proxy?server={proxy.HostName}&port={proxy.Port}"
                        : $"https://t.me/proxy?server={proxy.HostName}&port={proxy.Port}&secret={proxy.Secret}";
                    await UpdateStateProxyAsync(TgLocale.ProxyIsConnected);
                    break;
            }
        }
        catch (Exception ex)
        {
            IsProxyUsage = false;
            await SetProxyExceptionAsync(ex);
        }
    }

    public long ReduceChatId(long chatId) => !$"{chatId}".StartsWith("-100") ? chatId : Convert.ToInt64($"{chatId}"[4..]);

    //public long FixChatId(long chatId) => $"{chatId}".StartsWith("-100") ? chatId : Convert.ToInt64($"-100{chatId}");

    public string GetUserUpdatedName(long id) => DicUsersUpdated.TryGetValue(ReduceChatId(id), out User? user) ? user.username : string.Empty;

    public async Task<Channel?> GetChannelAsync(TgDownloadSettingsModel tgDownloadSettings)
    {
        if (tgDownloadSettings.SourceVm.IsReadySourceId)
        {
            tgDownloadSettings.SourceVm.SourceId = ReduceChatId(tgDownloadSettings.SourceVm.SourceId);
            foreach (KeyValuePair<long, ChatBase> chat in DicChatsAll)
            {
                if (chat.Value is Channel channel && Equals(channel.id, tgDownloadSettings.SourceVm.SourceId) && await IsChannelAccessAsync(channel)) return channel;
            }
        }
        else
        {
            foreach (KeyValuePair<long, ChatBase> chat in DicChatsAll)
            {
                if (chat.Value is Channel channel && Equals(channel.username, tgDownloadSettings.SourceVm.SourceUserName) && await IsChannelAccessAsync(channel)) return channel;
            }
        }

        if (tgDownloadSettings.SourceVm.SourceId is 0 or 1)
            tgDownloadSettings.SourceVm.SourceId = await GetPeerIdAsync(tgDownloadSettings.SourceVm.SourceUserName);

        Messages_Chats? messagesChats = null;
        if (Me is not null)
            messagesChats = await Client.Channels_GetChannels(new InputChannel(tgDownloadSettings.SourceVm.SourceId, Me.access_hash));
        if (messagesChats is not null)
        {
            foreach (KeyValuePair<long, ChatBase> chat in messagesChats.chats)
            {
                if (chat.Value is Channel channel && Equals(channel.ID, tgDownloadSettings.SourceVm.SourceId))
                    return channel;
            }
        }
        return null;
    }

    public async Task<ChatBase?> GetChatBaseAsync(TgDownloadSettingsModel tgDownloadSettings)
    {
        if (tgDownloadSettings.SourceVm.IsReadySourceId)
        {
            tgDownloadSettings.SourceVm.SourceId = ReduceChatId(tgDownloadSettings.SourceVm.SourceId);
            foreach (KeyValuePair<long, ChatBase> chat in DicChatsAll)
            {
                if (chat.Value is { } chatBase && Equals(chatBase.ID, tgDownloadSettings.SourceVm.SourceId))
                    return chatBase;
            }
        }
        else
        {
            foreach (KeyValuePair<long, ChatBase> chat in DicChatsAll)
            {
                if (chat.Value is { } chatBase)
                    return chatBase;
            }
        }

        if (tgDownloadSettings.SourceVm.SourceId is 0)
            tgDownloadSettings.SourceVm.SourceId = await GetPeerIdAsync(tgDownloadSettings.SourceVm.SourceUserName);

        Messages_Chats? messagesChats = null;
        if (Me is not null)
            messagesChats = await Client.Channels_GetGroupsForDiscussion();

        if (messagesChats is not null)
            foreach (KeyValuePair<long, ChatBase> chat in messagesChats.chats)
            {
                if (chat.Value is { } chatBase && Equals(chatBase.ID, tgDownloadSettings.SourceVm.SourceId))
                    return chatBase;
            }

        return null;
    }

    public async Task<Bots_BotInfo?> GetBotInfoAsync(TgDownloadSettingsModel tgDownloadSettings)
    {
        if (tgDownloadSettings.SourceVm.SourceId is 0)
            tgDownloadSettings.SourceVm.SourceId = await GetPeerIdAsync(tgDownloadSettings.SourceVm.SourceUserName);
        if (!tgDownloadSettings.SourceVm.IsReadySourceId)
            tgDownloadSettings.SourceVm.SourceId = ReduceChatId(tgDownloadSettings.SourceVm.SourceId);
        if (!tgDownloadSettings.SourceVm.IsReadySourceId)
            return null;
        Bots_BotInfo? botInfo = null;
        if (Me is not null)
            botInfo = await Client.Bots_GetBotInfo("en", new InputUser(tgDownloadSettings.SourceVm.SourceId, 0));
        return botInfo;
    }

    public string GetChatUpdatedName(long id)
    {
        bool isGetValue = DicChatsUpdated.TryGetValue(ReduceChatId(id), out ChatBase? chat);
        if (!isGetValue || chat is null)
            return string.Empty;
        return chat.ToString() ?? string.Empty;
    }

    public string GetPeerUpdatedName(Peer peer) => peer is PeerUser user ? GetUserUpdatedName(user.user_id)
        : peer is PeerChat or PeerChannel ? GetChatUpdatedName(peer.ID) : $"Peer {peer.ID}";

    public async Task<Dictionary<long, ChatBase>> CollectAllChatsAsync()
    {
        switch (IsReady)
        {
            case true when Client is not null:
            {
                Messages_Chats messages = await Client.Messages_GetAllChats();
                DicChatsAll = messages.chats;
                FillEnumerableChats(DicChatsAll);
                return DicChatsAll;
            }
        }
        return new();
    }

    public async Task<Dictionary<long, ChatBase>> CollectAllDialogsAsync()
    {
        switch (IsReady)
        {
            case true when Client is { }:
            {
                Messages_Dialogs messages = await Client.Messages_GetAllDialogs();
                DicChatsAll = messages.chats;
                FillEnumerableChats(DicChatsAll);
                return DicChatsAll;
            }
        }
        return new();
    }

    private void FillEnumerableChats(Dictionary<long, ChatBase> dic)
    {
        List<Channel> listChannels = EnumerableChannels.OrderBy(i => i.username).ThenBy(i => i.id).ToList();
        listChannels.Clear();
        List<ChatBase> listChats = EnumerableChats.OrderBy(i => i.MainUsername).ThenBy(i => i.ID).ToList();
        listChats.Clear();
        List<Channel> listGroups = EnumerableGroups.OrderBy(i => i.username).ThenBy(i => i.id).ToList();
        listGroups.Clear();
        List<ChatBase> listSmallGroups = EnumerableSmallGroups.OrderBy(i => i.MainUsername).ThenBy(i => i.ID).ToList();
        listSmallGroups.Clear();

        foreach (KeyValuePair<long, ChatBase> item in dic)
        {
            listChats.Add(item.Value);
            switch (item.Value)
            {
                case Chat smallGroup when (smallGroup.flags & Chat.Flags.deactivated) is 0:
                    listSmallGroups.Add(item.Value);
                    break;
                case Channel { IsGroup: true } group:
                    //case Channel group: // no broadcast flag => it's a big group, also called superGroup or megaGroup
                    listGroups.Add(group);
                    break;
                case Channel channel:
                    //case Channel channel when (channel.flags & Channel.Flags.broadcast) is not 0:
                    listChannels.Add(channel);
                    break;
            }
        }

        EnumerableChannels = listChannels;
        EnumerableChats = listChats;
        EnumerableGroups = listGroups;
        EnumerableSmallGroups = listSmallGroups;
    }

    public async Task OnUpdateClientAsync(IObject arg)
    {
        if (!IsUpdateStatus)
            return;
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        
        if (arg is UpdateShort updateShort)
            await OnUpdateShortClientAsync(updateShort);
        if (arg is UpdatesBase updates)
            await OnUpdateClientUpdatesAsync(updates);
    }

    private async Task OnUpdateShortClientAsync(UpdateShort updateShort)
    {
        try
        {
            updateShort.CollectUsersChats(DicUsersUpdated, DicChatsUpdated);
            if (updateShort.UpdateList.Any())
            {
                foreach (Update update in updateShort.UpdateList)
                {
                    try
                    {
                        if (updateShort.Chats.Any())
                        {
                            foreach (KeyValuePair<long, ChatBase> chatBase in updateShort.Chats)
                            {
                                if (chatBase.Value is Channel { IsActive: true } channel)
                                {
                                    await SwitchUpdateTypeAsync(update, channel);
                                }
                            }
                        }
                        else
                            await SwitchUpdateTypeAsync(update);
                    }
                    catch (Exception ex)
                    {
                        await SetClientExceptionAsync(ex);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await SetClientExceptionAsync(ex);
        }
    }

    private async Task OnUpdateClientUpdatesAsync(UpdatesBase updates)
    {
        try
        {
            updates.CollectUsersChats(DicUsersUpdated, DicChatsUpdated);
            if (updates.UpdateList.Any())
            {
                foreach (Update update in updates.UpdateList)
                {
                    try
                    {
                        if (updates.Chats.Any())
                        {
                            foreach (KeyValuePair<long, ChatBase> chatBase in updates.Chats)
                            {
                                if (chatBase.Value is Channel { IsActive: true } channel)
                                {
                                    await SwitchUpdateTypeAsync(update, channel);
                                }
                            }
                        }
                        else
                            await SwitchUpdateTypeAsync(update);
                    }
                    catch (Exception ex)
                    {
                        await SetClientExceptionAsync(ex);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await SetClientExceptionAsync(ex);
        }
    }

    // https://corefork.telegram.org/type/Update
    private async Task SwitchUpdateTypeAsync(Update update, Channel? channel = null)
    {
        await UpdateTitleAsync(TgDataFormatUtils.GetTimeFormat(DateTime.Now));
        string channelLabel = channel is null ? string.Empty :
            string.IsNullOrEmpty(channel.MainUsername) ? channel.ID.ToString() : $"{channel.ID} | {channel.MainUsername}";
        if (!string.IsNullOrEmpty(channelLabel))
            channelLabel = $" for channel [{channelLabel}]";
        switch (update)
        {
            case UpdateNewChannelMessage updateNewChannelMessage:
                //if (channel is not null && updateNewChannelMessage.message.Peer.ID.Equals(channel.ID))
                await UpdateStateMessageAsync($"New channel message [{updateNewChannelMessage}]{channelLabel}");
                break;
            case UpdateNewMessage updateNewMessage:
                await UpdateStateMessageAsync($"New message [{updateNewMessage}]{channelLabel}");
                break;
            case UpdateMessageID updateMessageId:
                await UpdateStateMessageAsync($"Message ID [{updateMessageId}]{channelLabel}");
                break;
            case UpdateDeleteChannelMessages updateDeleteChannelMessages:
                await UpdateStateMessageAsync($"Delete channel messages [{string.Join(", ", updateDeleteChannelMessages.messages)}]{channelLabel}");
                break;
            case UpdateDeleteMessages updateDeleteMessages:
                await UpdateStateMessageAsync($"Delete messages [{string.Join(", ", updateDeleteMessages.messages)}]{channelLabel}");
                break;
            case UpdateChatUserTyping updateChatUserTyping:
                await UpdateStateMessageAsync($"Chat user typing [{updateChatUserTyping}]{channelLabel}");
                break;
            case UpdateChatParticipants { participants: ChatParticipants chatParticipants }:
                await UpdateStateMessageAsync($"Chat participants [{chatParticipants.ChatId} | {string.Join(", ", chatParticipants.Participants.Length)}]{channelLabel}");
                break;
            case UpdateUserStatus updateUserStatus:
                await UpdateStateMessageAsync($"User status [{updateUserStatus.user_id} | {updateUserStatus}]{channelLabel}");
                break;
            case UpdateUserName updateUserName:
                await UpdateStateMessageAsync($"User name [{updateUserName.user_id} | {string.Join(", ", updateUserName.usernames.Select(item => item.username))}]{channelLabel}");
                break;
            case UpdateNewEncryptedMessage updateNewEncryptedMessage:
                await UpdateStateMessageAsync($"New encrypted message [{updateNewEncryptedMessage}]{channelLabel}");
                break;
            case UpdateEncryptedChatTyping updateEncryptedChatTyping:
                await UpdateStateMessageAsync($"Encrypted chat typing [{updateEncryptedChatTyping}]{channelLabel}");
                break;
            case UpdateEncryption updateEncryption:
                await UpdateStateMessageAsync($"Encryption [{updateEncryption}]{channelLabel}");
                break;
            case UpdateEncryptedMessagesRead updateEncryptedMessagesRead:
                await UpdateStateMessageAsync($"Encrypted message read [{updateEncryptedMessagesRead}]{channelLabel}");
                break;
            case UpdateChatParticipantAdd updateChatParticipantAdd:
                await UpdateStateMessageAsync($"Chat participant add [{updateChatParticipantAdd}]{channelLabel}");
                break;
            case UpdateChatParticipantDelete updateChatParticipantDelete:
                await UpdateStateMessageAsync($"Chat participant delete [{updateChatParticipantDelete}]{channelLabel}");
                break;
            case UpdateDcOptions updateDcOptions:
                await UpdateStateMessageAsync($"Dc options [{string.Join(", ", updateDcOptions.dc_options.Select(item => item.id))}]{channelLabel}");
                break;
            case UpdateNotifySettings updateNotifySettings:
                await UpdateStateMessageAsync($"Notify settings [{updateNotifySettings}]{channelLabel}");
                break;
            case UpdateServiceNotification updateServiceNotification:
                await UpdateStateMessageAsync($"Service notification [{updateServiceNotification}]{channelLabel}");
                break;
            case UpdatePrivacy updatePrivacy:
                await UpdateStateMessageAsync($"Privacy [{updatePrivacy}]{channelLabel}");
                break;
            case UpdateUserPhone updateUserPhone:
                await UpdateStateMessageAsync($"User phone [{updateUserPhone}]{channelLabel}");
                break;
            case UpdateReadHistoryInbox updateReadHistoryInbox:
                await UpdateStateMessageAsync($"Read history inbox [{updateReadHistoryInbox}]{channelLabel}");
                break;
            case UpdateReadHistoryOutbox updateReadHistoryOutbox:
                await UpdateStateMessageAsync($"Read history outbox [{updateReadHistoryOutbox}]{channelLabel}");
                break;
            case UpdateWebPage updateWebPage:
                await UpdateStateMessageAsync($"Web page [{updateWebPage}]{channelLabel}");
                break;
            case UpdateReadMessagesContents updateReadMessagesContents:
                await UpdateStateMessageAsync($"Read messages contents [{string.Join(", ", updateReadMessagesContents.messages.Select(item => item.ToString()))}]{channelLabel}");
                break;
            case UpdateEditChannelMessage updateEditChannelMessage:
                await UpdateStateMessageAsync($"Edit channel message [{updateEditChannelMessage}]{channelLabel}");
                break;
            case UpdateEditMessage updateEditMessage:
                await UpdateStateMessageAsync($"Edit message [{updateEditMessage}]{channelLabel}");
                break;
            case UpdateUserTyping updateUserTyping:
                await UpdateStateMessageAsync($"User typing [{updateUserTyping}]{channelLabel}");
                break;
            case UpdateChannelMessageViews updateChannelMessageViews:
                await UpdateStateMessageAsync($"Channel message views [{updateChannelMessageViews}]{channelLabel}");
                break;
            case UpdateChannel updateChannel:
                await UpdateStateMessageAsync($"Channel [{updateChannel}]");
                break;
            case UpdateChannelReadMessagesContents updateChannelReadMessages:
                await UpdateStateMessageAsync($"Channel read messages [{string.Join(", ", updateChannelReadMessages.messages)}]{channelLabel}");
                break;
            case UpdateChannelUserTyping updateChannelUserTyping:
                await UpdateStateMessageAsync($"Channel user typing [{updateChannelUserTyping}]{channelLabel}");
                break;
            case UpdateMessagePoll updateMessagePoll:
                await UpdateStateMessageAsync($"Message poll [{updateMessagePoll}]{channelLabel}");
                break;
            case UpdateChannelTooLong updateChannelTooLong:
                await UpdateStateMessageAsync($"Channel too long [{updateChannelTooLong}]{channelLabel}");
                break;
            case UpdateReadChannelInbox updateReadChannelInbox:
                await UpdateStateMessageAsync($"Channel inbox [{updateReadChannelInbox}]{channelLabel}");
                break;
            case UpdateChatParticipantAdmin updateChatParticipantAdmin:
                await UpdateStateMessageAsync($"Chat participant admin[{updateChatParticipantAdmin}]{channelLabel}");
                break;
            case UpdateNewStickerSet updateNewStickerSet:
                await UpdateStateMessageAsync($"New sticker set [{updateNewStickerSet}]{channelLabel}");
                break;
            case UpdateStickerSetsOrder updateStickerSetsOrder:
                await UpdateStateMessageAsync($"Sticker sets order [{updateStickerSetsOrder}]{channelLabel}");
                break;
            case UpdateStickerSets updateStickerSets:
                await UpdateStateMessageAsync($"Sticker sets [{updateStickerSets}]{channelLabel}");
                break;
            case UpdateSavedGifs updateSavedGifs:
                await UpdateStateMessageAsync($"SavedGifs [{updateSavedGifs}]{channelLabel}");
                break;
            case UpdateBotInlineQuery updateBotInlineQuery:
                await UpdateStateMessageAsync($"Bot inline query [{updateBotInlineQuery}]{channelLabel}");
                break;
            case UpdateBotInlineSend updateBotInlineSend:
                await UpdateStateMessageAsync($"Bot inline send [{updateBotInlineSend}]{channelLabel}");
                break;
            case UpdateBotCallbackQuery updateBotCallbackQuery:
                await UpdateStateMessageAsync($"Bot cCallback query [{updateBotCallbackQuery}]{channelLabel}");
                break;
            case UpdateInlineBotCallbackQuery updateInlineBotCallbackQuery:
                await UpdateStateMessageAsync($"Inline bot callback query [{updateInlineBotCallbackQuery}]{channelLabel}");
                break;
        }
    }

    private async Task OnClientOtherAsync(IObject arg)
    {
        if (!IsUpdateStatus)
            return;
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        
        if (arg is Auth_SentCodeBase authSentCode)
            await OnClientOtherAuthSentCodeAsync(authSentCode);
    }

    private async Task OnClientOtherAuthSentCodeAsync(Auth_SentCodeBase authSentCode)
    {
        try
        {
            //
        }
        catch (Exception ex)
        {
            await SetClientExceptionAsync(ex);
        }
    }


    //private void Client_DisplayMessage(MessageBase messageBase, bool edit = false)
    //{
    //	if (edit) Console.Write("(Edit): ");
    //	switch (messageBase)
    //	{
    //		case TL.Message message:
    //			TgLog.MarkupLine($"{GetPeerUpdatedName(message.from_id)} in {GetPeerUpdatedName(message.peer_id)}> {message.message}");
    //			break;
    //		case MessageService messageService:
    //			TgLog.MarkupLine($"{GetPeerUpdatedName(messageService.from_id)} in {GetPeerUpdatedName(messageService.peer_id)} [{messageService.action.GetType().Name[13..]}]");
    //			break;
    //	}
    //}

    //public async Task PrintSendAsync(Messages_Chats messagesChats)
    //{
    //	await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
    //	Console.Write("Type a chat ID to send a message: ");
    //	string? input = Console.ReadLine();
    //	if (!string.IsNullOrEmpty(input))
    //	{
    //		long chatId = long.Parse(input);
    //		ChatBase target = messagesChats.chats[chatId];
    //		TgLog.MarkupLine($"Type the message into the chat: {target.Title} | {chatId}");
    //		input = Console.ReadLine();
    //		if (Client is { })
    //			await Client.SendMessageAsync(target, input);
    //	}
    //}

    public IEnumerable<ChatBase> SortListChats(IList<ChatBase> chats)
    {
        if (!chats.Any())
            return chats;
        List<ChatBase> result = new();
        List<ChatBase> chatsOrders = chats.OrderBy(x => x.Title).ToList();
        foreach (ChatBase chatOrder in chatsOrders)
        {
            ChatBase chatNew = chats.First(x => Equals(x.Title, chatOrder.Title));
            if (chatNew.ID is not 0)
                result.Add(chatNew);
        }
        return result;
    }

    public IEnumerable<Channel> SortListChannels(IList<Channel> channels)
    {
        if (!channels.Any())
            return channels;
        List<Channel> result = new();
        List<Channel> channelsOrders = channels.OrderBy(x => x.username).ToList();
        foreach (Channel chatOrder in channelsOrders)
        {
            Channel chatNew = channels.First(x => Equals(x.Title, chatOrder.Title));
            if (chatNew.ID is not 0)
                result.Add(chatNew);
        }
        return result;
    }

    public async Task PrintChatsInfoAsync(Dictionary<long, ChatBase> dicChats, string name, bool isSave)
    {
        TgLog.MarkupInfo($"Found {name}: {dicChats.Count}");
        TgLog.MarkupInfo(TgLocale.TgGetDialogsInfo);
        foreach (KeyValuePair<long, ChatBase> dicChat in dicChats)
        {
            await TryCatchFuncAsync(async () =>
            {
                switch (dicChat.Value)
                {
                    case Channel channel:
                        await PrintChatsInfoChannelAsync(channel, false, false, isSave);
                        break;
                    default:
                        TgLog.MarkupLine(GetChatInfo(dicChat.Value));
                        break;
                }
            });
        }
    }

    private async Task<Messages_ChatFull?> PrintChatsInfoChannelAsync(Channel channel, bool isFull, bool isSilent, bool isSave)
    {
        Messages_ChatFull? chatFull = null;
        try
        {
            chatFull = await Client.Channels_GetFullChannel(channel);
            if (isSave)
                await ContextManager.SourceRepository.SaveAsync(new() { Id = channel.id, UserName = channel.username, Title = channel.title });
            if (!isSilent)
            {
                if (chatFull is not null)
                {
                    if (chatFull.full_chat is ChannelFull channelFull)
                        TgLog.MarkupLine(GetChannelFullInfo(channelFull, channel, isFull));
                    else
                        TgLog.MarkupLine(GetChatFullBaseInfo(chatFull.full_chat, channel, isFull));
                }
            }
        }
        catch (Exception ex)
        {
            await SetClientExceptionAsync(ex);
        }
        return chatFull;
    }

    private async Task<Messages_ChatFull?> PrintChatsInfoChatBaseAsync(ChatBase chatBase, bool isFull, bool isSilent)
    {
        Messages_ChatFull? chatFull = null;
        if (Client is null)
            return chatFull;
        try
        {
            chatFull = await Client.GetFullChat(chatBase);
            if (!isSilent)
            {
                if (chatFull.full_chat is ChannelFull channelFull)
                    TgLog.MarkupLine(GetChannelFullInfo(channelFull, chatBase, isFull));
                else
                    TgLog.MarkupLine(GetChatFullBaseInfo(chatFull.full_chat, chatBase, isFull));
            }
        }
        catch (Exception ex)
        {
            await SetClientExceptionAsync(ex);
        }
        return chatFull;
    }

    public string GetChatInfo(ChatBase chatBase) => $"{chatBase.ID} | {chatBase.Title}";

    public string GetChannelFullInfo(ChannelFull channelFull, ChatBase chatBase, bool isFull)
    {
        string result = GetChatInfo(chatBase);
        if (isFull)
            result += " | " + Environment.NewLine + channelFull.About;
        return result;
    }

    public string GetChatFullBaseInfo(ChatFullBase chatFull, ChatBase chatBase, bool isFull)
    {
        string result = GetChatInfo(chatBase);
        if (isFull)
            result += " | " + Environment.NewLine + chatFull.About;
        return result;
    }

    public async Task<bool> IsChannelAccessAsync(ChatBase chatBase)
    {
        if (chatBase.ID is 0 || chatBase is not Channel channel)
            return false;
        
        return await Client.Channels_ReadMessageContents(channel);
    }

    public async Task<int> GetChannelMessageIdWithLockAsync(TgDownloadSettingsModel? tgDownloadSettings, ChatBase chatBase, TgEnumPosition position) => 
        await GetChannelMessageIdCoreAsync(tgDownloadSettings, chatBase, position);

    public async Task<int> GetChannelMessageIdWithLockAsync(ChatBase chatBase, TgEnumPosition position) => 
        await GetChannelMessageIdCoreAsync(null, chatBase, position);

    public async Task<int> GetChannelMessageIdAsync(TgDownloadSettingsModel? tgDownloadSettings, ChatBase chatBase,
        TgEnumPosition position) =>
        await GetChannelMessageIdCoreAsync(tgDownloadSettings, chatBase, position);

    private async Task<int> GetChannelMessageIdCoreAsync(TgDownloadSettingsModel? tgDownloadSettings, ChatBase chatBase, TgEnumPosition position)
    {
        if (Client is null)
            return 0;
        if (chatBase.ID is 0)
            return 0;

        if (chatBase is Channel channel)
        {
            Messages_ChatFull fullChannel = await Client.Channels_GetFullChannel(channel);
            if (fullChannel.full_chat is not ChannelFull channelFull)
                return 0;
            bool isAccessToMessages = await Client.Channels_ReadMessageContents(channel);
            if (isAccessToMessages)
            {
                switch (position)
                {
                    case TgEnumPosition.First:
                        if (tgDownloadSettings is not null)
                            return await SetChannelMessageIdFirstCoreAsync(tgDownloadSettings, chatBase, channelFull);
                        break;
                    case TgEnumPosition.Last:
                        return GetChannelMessageIdLastCore(channelFull);
                }
            }
        }
        else
        {
            Messages_ChatFull fullChannel = await Client.GetFullChat(chatBase);
            switch (position)
            {
                case TgEnumPosition.First:
                    if (tgDownloadSettings is not null)
                        return await SetChannelMessageIdFirstCoreAsync(tgDownloadSettings, chatBase, fullChannel.full_chat);
                    break;
                case TgEnumPosition.Last:
                    return GetChannelMessageIdLastCore(fullChannel.full_chat);
            }
        }
        return 0;
    }

    public async Task<int> GetChannelMessageIdLastAsync(TgDownloadSettingsModel? tgDownloadSettings, ChatBase chatBase) =>
        await GetChannelMessageIdWithLockAsync(tgDownloadSettings, chatBase, TgEnumPosition.Last);

    private int GetChannelMessageIdLastCore(ChatFullBase chatFullBase) => 
        chatFullBase is ChannelFull channelFull ? channelFull.read_inbox_max_id : 0;

    public async Task SetChannelMessageIdFirstAsync(TgDownloadSettingsModel tgDownloadSettings, ChatBase chatBase) =>
        await GetChannelMessageIdAsync(tgDownloadSettings, chatBase, TgEnumPosition.First);

    private async Task<int> SetChannelMessageIdFirstCoreAsync(TgDownloadSettingsModel tgDownloadSettings, ChatBase chatBase,
        ChatFullBase chatFullBase)
    {
        int max = chatFullBase is ChannelFull channelFull ? channelFull.read_inbox_max_id : 0;
        int result = max;
        int partition = 200;
        InputMessage[] inputMessages = new InputMessage[partition];
        int offset = 0;
        bool isSkipChannelCreate = false;
        // While.
        while (offset < max)
        {
            for (int i = 0; i < partition; i++)
            {
                inputMessages[i] = offset + i + 1;
            }
            tgDownloadSettings.SourceVm.SourceFirstId = offset;
            await UpdateStateMessageAsync($"Read from {offset} to {offset + partition} messages");
            Messages_MessagesBase? messages = await Client.Channels_GetMessages(chatBase as Channel, inputMessages);
            for (int i = messages.Offset; i < messages.Count; i++)
            {
                // Skip first message.
                if (!isSkipChannelCreate)
                {
                    string? msg = messages.Messages[i].ToString();
                    // Magic String. It needs refactoring.
                    if (Equals(msg, $"{chatBase.ID} [ChannelCreate]"))
                    {
                        isSkipChannelCreate = true;
                    }
                }
                // Check message exists.
                else if (messages.Messages[i].Date > DateTime.MinValue)
                {
                    result = offset + i + 1;
                    break;
                }
            }
            if (result < max)
                break;
            offset += partition;
        }
        // Finally.
        if (result >= max)
            result = 1;
        tgDownloadSettings.SourceVm.SourceFirstId = result;
        await UpdateStateMessageAsync($"Get the first ID message '{result}' is complete.");
        return result;
    }

    public async Task<Channel?> PrepareChannelDownloadMessagesAsync(TgDownloadSettingsModel tgDownloadSettings, bool isSilent)
    {
        Channel? channel = null;
        await TryCatchFuncAsync(async () =>
        {
            channel = await GetChannelAsync(tgDownloadSettings);
            if (channel is null)
                return;
            if (! await IsChannelAccessAsync(channel))
                return;
            tgDownloadSettings.SourceVm.SourceUserName = !string.IsNullOrEmpty(channel.username) ? channel.username : string.Empty;
            Messages_ChatFull? chatFull = await PrintChatsInfoChannelAsync(channel, true, isSilent, false);
            tgDownloadSettings.SourceVm.SourceLastId = await GetChannelMessageIdLastAsync(tgDownloadSettings, channel);
            if (chatFull?.full_chat is ChannelFull channelFull)
                tgDownloadSettings.SourceVm.SetSource(channelFull.ID, channel.Title, channelFull.About);

            // Save.
            TgSqlTableSourceModel source = await ContextManager.SourceRepository.GetAsync(tgDownloadSettings.SourceVm.SourceId);
            source.UserName = tgDownloadSettings.SourceVm.SourceUserName;
            source.Title = tgDownloadSettings.SourceVm.SourceTitle;
            source.About = tgDownloadSettings.SourceVm.SourceAbout;
            source.Count = tgDownloadSettings.SourceVm.SourceLastId;
            await ContextManager.SourceRepository.SaveAsync(source);
        });
        return channel;
    }

    public async Task<ChatBase?> PrepareChatBaseDownloadMessagesAsync(TgDownloadSettingsModel tgDownloadSettings, bool isSilent)
    {
        ChatBase? chatBase = null;
        await TryCatchFuncAsync(async () =>
        {
            chatBase = await GetChatBaseAsync(tgDownloadSettings);
            if (chatBase is null)
                return;
            Messages_ChatFull? chatFull = await PrintChatsInfoChatBaseAsync(chatBase, true, isSilent);
            tgDownloadSettings.SourceVm.SourceLastId = await GetChannelMessageIdLastAsync(tgDownloadSettings, chatBase);
            if (chatFull?.full_chat is ChannelFull channelFull)
                tgDownloadSettings.SourceVm.SetSource(channelFull.id, chatBase.Title, channelFull.About);
        });
        return chatBase;
    }

    /// <summary>
    /// Update source from Telegram.
    /// </summary>
    /// <param name="sourceVm"></param>
    /// <param name="tgDownloadSettings"></param>
    public async Task UpdateSourceDbAsync(TgSqlTableSourceViewModel sourceVm, TgDownloadSettingsModel tgDownloadSettings)
    {
        Channel? channel = await PrepareChannelDownloadMessagesAsync(tgDownloadSettings, true);
        if (channel is not null)
        {
            sourceVm.Source.UserName = tgDownloadSettings.SourceVm.SourceUserName;
            sourceVm.Source.Count = tgDownloadSettings.SourceVm.SourceLastId;
            sourceVm.Source.Title = tgDownloadSettings.SourceVm.SourceTitle;
            sourceVm.Source.About = tgDownloadSettings.SourceVm.SourceAbout;
            return;
        }

        ChatBase? chat = await PrepareChatBaseDownloadMessagesAsync(tgDownloadSettings, true);
        if (chat is not null)
        {
            sourceVm.Source.UserName = tgDownloadSettings.SourceVm.SourceUserName;
            sourceVm.Source.Count = tgDownloadSettings.SourceVm.SourceLastId;
            sourceVm.Source.Title = tgDownloadSettings.SourceVm.SourceTitle;
            sourceVm.Source.About = tgDownloadSettings.SourceVm.SourceAbout;
        }
    }

    private async Task UpdateSourceTgAsync(Channel channel, string about, int count)
    {
        TgSqlTableSourceModel itemFind = await ContextManager.SourceRepository.GetAsync(channel.id);
        if (itemFind.IsExists)
        {
            itemFind.UserName = channel.username;
            if (!string.IsNullOrEmpty(channel.title))
                itemFind.Title = channel.title;
            if (!string.IsNullOrEmpty(about))
                itemFind.About = about;
            itemFind.Count = count;
        }
        await ContextManager.SourceRepository.SaveAsync(itemFind);
    }

    private async Task UpdateSourceTgAsync(Channel channel, int count) => 
        await UpdateSourceTgAsync(channel, string.Empty, count);

    public async Task ScanSourcesTgConsoleAsync(TgDownloadSettingsModel tgDownloadSettings, TgEnumSourceType sourceType)
    {
        await TryCatchFuncAsync(async () =>
        {
            LoginUserConsole();
            switch (sourceType)
            {
                case TgEnumSourceType.Chat:
                    await UpdateStateMessageAsync(TgLocale.CollectChats);
                    await CollectAllChatsAsync();
                    break;
                case TgEnumSourceType.Dialog:
                    await UpdateStateMessageAsync(TgLocale.CollectDialogs);
                    await CollectAllDialogsAsync();
                    break;
            }
            tgDownloadSettings.SourceVm.SourceScanCount = DicChatsAll.Count;
            tgDownloadSettings.SourceVm.SourceScanCurrent = 0;
            // ListChannels.
            foreach (Channel channel in EnumerableChannels)
            {
                tgDownloadSettings.SourceVm.SourceScanCurrent++;
                if (channel.IsActive)
                {
                    await TryCatchFuncAsync(async () =>
                    {
                        int messagesCount = await GetChannelMessageIdLastAsync(tgDownloadSettings, channel);
                        if (channel.IsChannel)
                        {
                            Messages_ChatFull? chatFull = await Client.Channels_GetFullChannel(channel);
                            if (chatFull?.full_chat is ChannelFull channelFull)
                            {
                                await UpdateSourceTgAsync(channel, channelFull.about, messagesCount);
                                await UpdateStateMessageAsync($"{channel} | {messagesCount} | {TgDataFormatUtils.TrimStringEnd(channelFull.about)}");
                            }
                        }
                        else
                        {
                            await UpdateSourceTgAsync(channel, messagesCount);
                            await UpdateStateMessageAsync($"{channel} | {messagesCount}");
                        }
                    });
                }
                await UpdateTitleAsync($"{TgCommonUtils.CalcSourceProgress(tgDownloadSettings.SourceVm.SourceScanCount, 
                    tgDownloadSettings.SourceVm.SourceScanCurrent):#00.00} %");
            }
            // ListGroups.
            foreach (Channel group in EnumerableGroups)
            {
                tgDownloadSettings.SourceVm.SourceScanCurrent++;
                if (group.IsActive)
                {
                    await TryCatchFuncAsync(async () =>
                    {
                        int messagesCount = await GetChannelMessageIdLastAsync(tgDownloadSettings, group);
                        await UpdateSourceTgAsync(group, messagesCount);
                        await UpdateStateMessageAsync($"{group} | {messagesCount}");
                    });
                }
            }
        });
        await UpdateTitleAsync(string.Empty);
    }

    public async Task ScanSourcesTgDesktopAsync(TgEnumSourceType sourceType, Func<TgSqlTableSourceViewModel, Task> afterScanAsync)
    {
        await TryCatchFuncAsync(async () =>
        {
            switch (sourceType)
            {
                case TgEnumSourceType.Chat:
                    await UpdateStateMessageAsync(TgLocale.CollectChats);
                    switch (IsReady)
                    {
                        case true when Client is not null:
                            Messages_Chats messages = await Client.Messages_GetAllChats().ConfigureAwait(false);
                            DicChatsAll = messages.chats;
                            FillEnumerableChats(DicChatsAll);
                            break;
                    }
                    await AfterCollectSourcesAsync(afterScanAsync);
                    break;
                case TgEnumSourceType.Dialog:
                    await UpdateStateMessageAsync(TgLocale.CollectDialogs);
                    switch (IsReady)
                    {
                        case true when Client is { }:
                        {
                            Messages_Dialogs messages = await Client.Messages_GetAllDialogs().ConfigureAwait(false);
                            FillEnumerableChats(messages.chats);
                            break;
                        }
                    }
                    break;
            }
        });
    }

    private async Task AfterCollectSourcesAsync(Func<TgSqlTableSourceViewModel, Task> afterScanAsync)
    {
        // ListChannels.
        int i = 0;
        int count = EnumerableChannels.Count();
        foreach (Channel channel in EnumerableChannels)
        {
            if (channel.IsActive)
            {
                await TryCatchFuncAsync(async () =>
                {
                    TgSqlTableSourceModel source = new() { Id = channel.ID };
                    int messagesCount = await GetChannelMessageIdLastAsync(new(), channel);
                    source.Count = messagesCount;
                    if (channel.IsChannel)
                    {
                        Messages_ChatFull chatFull = await Client.Channels_GetFullChannel(channel);
                        if (chatFull.full_chat is ChannelFull channelFull)
                        {
                            source.About = channelFull.about;
                            source.UserName = channel.username;
                            source.Title = channel.title;
                        }
                    }
                    await afterScanAsync(new(source));
                });
            }
            i++;
            await UpdateStateMessageAsync($"Read channel '{channel.ID}' | {channel.IsActive} | {i} from {count}");
            await UpdateStateSourceAsync(channel.ID, 0, $"Read channel '{channel.ID}' | {channel.IsActive} | {i} from {count}");
        }

        // ListGroups.
        i = 0;
        count = EnumerableGroups.Count();
        foreach (Channel group in EnumerableGroups)
        {
            await TryCatchFuncAsync(async () =>
            {
                TgSqlTableSourceModel source = new() { Id = group.ID };
                if (group.IsActive)
                {
                    int messagesCount = await GetChannelMessageIdLastAsync(new(), group);
                    source.Count = messagesCount;
                }
                await afterScanAsync(new(source));
            });
            i++;
            await UpdateStateMessageAsync($"Read group '{group.ID}' | {group.IsActive} | {i} from {count}");
            await UpdateStateSourceAsync(group.ID, 0, $"Read channel '{group.ID}' | {group.IsActive} | {i} from {count}");
        }
    }

    public async Task DownloadAllDataAsync(TgDownloadSettingsModel tgDownloadSettings)
    {
        Channel? channel = await PrepareChannelDownloadMessagesAsync(tgDownloadSettings, false);
        ChatBase? chatBase = null;
        if (channel is null || channel.id is 0)
            chatBase = await PrepareChatBaseDownloadMessagesAsync(tgDownloadSettings, true);
        await TryCatchFuncAsync(async () =>
        {
            // Set filters.
            Filters = ContextManager.FilterRepository.GetEnumerableEnabled();

            switch (TgAsyncUtils.AppType)
            {
                case TgEnumAppType.Console:
                    LoginUserConsole();
                    break;
                case TgEnumAppType.Desktop:
                    await LoginUserDesktopAsync();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //int backupId = tgDownloadSettings.SourceFirstId;
            await CreateDestDirectoryIfNotExistsAsync(tgDownloadSettings);
            while (tgDownloadSettings.SourceVm.SourceFirstId <= tgDownloadSettings.SourceVm.SourceLastId)
            {
                await TryCatchFuncAsync(async () =>
                {
                    bool isAccessToMessages = false;
                    if (channel is not null)
                        isAccessToMessages = await Client.Channels_ReadMessageContents(channel);
                    else if (chatBase is not null)
                        isAccessToMessages = true;
                    if (isAccessToMessages && Client is not null)
                    {
                        Messages_MessagesBase messages = channel is not null
                            ? await Client.Channels_GetMessages(channel, tgDownloadSettings.SourceVm.SourceFirstId)
                            : await Client.GetMessages(chatBase, tgDownloadSettings.SourceVm.SourceFirstId);
                        await UpdateTitleAsync($"{TgCommonUtils.CalcSourceProgress(tgDownloadSettings.SourceVm.SourceLastId, tgDownloadSettings.SourceVm.SourceFirstId):#00.00} %");
                        foreach (MessageBase message in messages.Messages)
                        {
                            // Check message exists.
                            if (message.Date > DateTime.MinValue)
                            {
                                DownloadDataAsync(tgDownloadSettings, message);
                            }
                            else
                            {
                                await UpdateStateMessageAsync("Message is not exists!");
                                await UpdateStateMessageAsync($"Message {message.ID} is not exists in {tgDownloadSettings.SourceVm.SourceId}!");
                                await UpdateStateSourceAsync(tgDownloadSettings.SourceVm.SourceId, message.ID,
                                    $"Message {message.ID} is not exists in {tgDownloadSettings.SourceVm.SourceId}!");
                            }
                        }
                    }
                });
                tgDownloadSettings.SourceVm.SourceFirstId++;
            }
            tgDownloadSettings.SourceVm.SourceFirstId = tgDownloadSettings.SourceVm.SourceLastId;
        });
        await UpdateTitleAsync(string.Empty);
    }

    private async Task CreateDestDirectoryIfNotExistsAsync(TgDownloadSettingsModel tgDownloadSettings)
    {
        try
        {
            Directory.CreateDirectory(tgDownloadSettings.SourceVm.SourceDirectory);
        }
        catch (Exception ex)
        {
            await SetClientExceptionAsync(ex);
        }
    }

    private async Task DownloadDataAsync(TgDownloadSettingsModel tgDownloadSettings, MessageBase messageBase)
    {
        if (messageBase is not Message message)
        {
            await UpdateStateMessageAsync("Empty message");
            await UpdateStateSourceAsync(tgDownloadSettings.SourceVm.SourceId, messageBase.ID, "Empty message");
            return;
        }

        await TryCatchFuncAsync(async () =>
        {
            //lock (Locker)
            {
                await tgDownloadSettings.UpdateSourceWithSettingsAsync();
                // Store message.
                bool isExistsMessage = await ContextManager.MessageRepository.GetExistsAsync(
                    tgDownloadSettings.SourceVm.SourceFirstId, tgDownloadSettings.SourceVm.SourceId);
                if ((isExistsMessage && tgDownloadSettings.IsRewriteMessages) || !isExistsMessage)
                    await ContextManager.MessageRepository.SaveAsync(
                        message.ID, tgDownloadSettings.SourceVm.SourceId, message.Date, TgEnumMessageType.Message, 0, message.message);
                // Parse documents and photos.
                if ((message.flags & Message.Flags.has_media) is not 0)
                {
                    if (message.media is MessageMediaDocument mediaDocument)
                    {
                        if ((mediaDocument.flags & MessageMediaDocument.Flags.has_document) is not 0)
                        {
                            if (mediaDocument.document is Document document)
                            {
                                await DownloadDataCoreAsync(tgDownloadSettings, messageBase, document, null);
                            }
                        }
                    }
                    else if (message.media is MessageMediaPhoto { photo: Photo photo })
                    {
                        await DownloadDataCoreAsync(tgDownloadSettings, messageBase, null, photo);
                    }
                }
                await UpdateStateMessageAsync("Read the message");
                await UpdateStateSourceAsync(tgDownloadSettings.SourceVm.SourceId, message.ID, "Read the message");
            }
        });
    }

    private (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] GetFiles(Document? document, Photo? photo)
    {
        string extensionName = string.Empty;
        if (document is { })
        {
            if (!string.IsNullOrEmpty(document.Filename) && (Path.GetExtension(document.Filename).TrimStart('.') is { } str))
                extensionName = str;
            if (!string.IsNullOrEmpty(document.Filename) && CheckFileAtFilter(document.Filename, extensionName, document.size))
                return new (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] { (document.Filename, document.size,
                    document.date, string.Empty, string.Empty) };
            if (document.attributes.Length > 0)
            {
                if (document.attributes.Any(x => x is DocumentAttributeVideo))
                {
                    extensionName = "mp4";
                    if (CheckFileAtFilter(string.Empty, extensionName, document.size))
                        return new (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] { ($"{document.ID}.{extensionName}", document.size,
                            document.date, string.Empty, string.Empty) };
                }
                if (document.attributes.Any(x => x is DocumentAttributeAudio))
                {
                    extensionName = "mp3";
                    if (CheckFileAtFilter(string.Empty, extensionName, document.size))
                        return new (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] { ($"{document.ID}.{extensionName}", document.size,
                            document.date, string.Empty, string.Empty) };
                }
            }
            if (string.IsNullOrEmpty(document.Filename))
                if (CheckFileAtFilter(string.Empty, extensionName, document.size))
                    return new (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] { ($"{document.ID}.{extensionName}", document.size,
                        document.date, string.Empty, string.Empty) };
        }

        if (photo is { })
        {
            extensionName = "jpg";
            //return photo.sizes.Select(x => ($"{photo.ID} {x.Width}x{x.Height}.{GetPhotoExt(x.Type)}", Convert.ToInt64(x.FileSize), photo.date, string.Empty, string.Empty)).ToArray();
            if (CheckFileAtFilter(string.Empty, extensionName, photo.sizes.Last().FileSize))
                return new (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] { ($"{photo.ID}.{extensionName}", photo.sizes.Last().FileSize,
                    photo.date, string.Empty, string.Empty) };
        }

        return Array.Empty<(string Remote, long Size, DateTime DtCreate, string Local, string Join)>();
    }

    public bool CheckFileAtFilter(string fileName, string extensionName, long size)
    {
        foreach (TgSqlTableFilterModel filter in Filters)
        {
            if (!filter.IsEnabled)
                continue;
            switch (filter.FilterType)
            {
                case TgEnumFilterType.SingleName:
                    if (string.IsNullOrEmpty(fileName))
                        continue;
                    if (!TgDataFormatUtils.CheckFileAtMask(fileName, filter.Mask))
                        return false;
                    break;
                case TgEnumFilterType.SingleExtension:
                    if (string.IsNullOrEmpty(extensionName))
                        continue;
                    if (!TgDataFormatUtils.CheckFileAtMask(extensionName, filter.Mask))
                        return false;
                    break;
                case TgEnumFilterType.MultiName:
                    if (string.IsNullOrEmpty(fileName))
                        continue;
                    bool isMultiName = filter.Mask.Split(',')
                        .Any(mask => TgDataFormatUtils.CheckFileAtMask(fileName, mask.Trim()));
                    if (!isMultiName)
                        return false;
                    break;
                case TgEnumFilterType.MultiExtension:
                    if (string.IsNullOrEmpty(extensionName))
                        continue;
                    bool isMultiExtension = filter.Mask.Split(',')
                        .Any(mask => TgDataFormatUtils.CheckFileAtMask(extensionName, mask.Trim()));
                    if (!isMultiExtension)
                        return false;
                    break;
                case TgEnumFilterType.MinSize:
                    if (size < filter.SizeAtBytes)
                        return false;
                    break;
                case TgEnumFilterType.MaxSize:
                    if (size > filter.SizeAtBytes)
                        return false;
                    break;
            }
        }
        return true;
    }

    private void SetFilesLocalNames(TgDownloadSettingsModel tgDownloadSettings, MessageBase messageBase,
        ref (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] files)
    {
        // Join ID.
        for (int i = 0; i < files.Length; i++)
        {
            files[i].Join = tgDownloadSettings.SourceVm.SourceLastId switch
            {
                < 1000 => tgDownloadSettings.IsJoinFileNameWithMessageId ? $"{messageBase.ID:000} {files[i].Remote}" : files[i].Remote,
                < 10000 => tgDownloadSettings.IsJoinFileNameWithMessageId ? $"{messageBase.ID:0000} {files[i].Remote}" : files[i].Remote,
                < 100000 => tgDownloadSettings.IsJoinFileNameWithMessageId ? $"{messageBase.ID:00000} {files[i].Remote}" : files[i].Remote,
                < 1000000 => tgDownloadSettings.IsJoinFileNameWithMessageId ? $"{messageBase.ID:000000} {files[i].Remote}" : files[i].Remote,
                < 10000000 => tgDownloadSettings.IsJoinFileNameWithMessageId ? $"{messageBase.ID:0000000} {files[i].Remote}" : files[i].Remote,
                < 100000000 => tgDownloadSettings.IsJoinFileNameWithMessageId ? $"{messageBase.ID:00000000} {files[i].Remote}" : files[i].Remote,
                < 1000000000 => tgDownloadSettings.IsJoinFileNameWithMessageId ? $"{messageBase.ID:000000000} {files[i].Remote}" : files[i].Remote,
                _ => tgDownloadSettings.IsJoinFileNameWithMessageId ? $"{messageBase.ID} {files[i].Remote}" : files[i].Remote
            };
        }
        // Join tgDownloadSettings.DestDirectory.
        for (int i = 0; i < files.Length; i++)
        {
            files[i].Local = Path.Combine(tgDownloadSettings.SourceVm.SourceDirectory, files[i].Remote);
            files[i].Join = Path.Combine(tgDownloadSettings.SourceVm.SourceDirectory, files[i].Join);
        }
    }

    private async Task DeleteExistsFilesAsync(TgDownloadSettingsModel tgDownloadSettings,
          (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] files)
    {
        await TryCatchFuncAsync(async () =>
        {
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = tgDownloadSettings.IsJoinFileNameWithMessageId ? files[i].Join : files[i].Local;
                if (File.Exists(fileName))
                {
                    long fileSize = TgFileUtils.CalculateFileSize(fileName);
                    if (tgDownloadSettings.IsRewriteFiles && fileSize < files[i].Size || fileSize == 0)
                    {
                        File.Delete(fileName);
                    }
                }
            }
        });
    }

    private async Task DownloadDataCoreAsync(TgDownloadSettingsModel tgDownloadSettings,
        MessageBase messageBase, Document? document, Photo? photo)
    {
        (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] files = GetFiles(document, photo);
        if (Equals(files, Array.Empty<(string Remote, long Size, DateTime DtCreate, string Local, string Join)>()))
            return;
        SetFilesLocalNames(tgDownloadSettings, messageBase, ref files);
        long accessHash = document?.access_hash ?? photo?.access_hash ?? 0;

        // Delete files.
        await DeleteExistsFilesAsync(tgDownloadSettings, files);

        // Download file.
        for (int i = 0; i < files.Length; i++)
        {
            // Move exists file.
            if (tgDownloadSettings.IsJoinFileNameWithMessageId)
            {
                if (File.Exists(files[i].Local) && !File.Exists(files[i].Join))
                    File.Move(files[i].Local, files[i].Join);
            }
            // FileName.
            string fileName = tgDownloadSettings.IsJoinFileNameWithMessageId ? files[i].Join : files[i].Local;
            // Scanning subdirectories for downloaded files to move them to the root directory.
            foreach (string directory in Directory.GetDirectories(tgDownloadSettings.SourceVm.SourceDirectory))
            {
                string fileSubDir = Path.Combine(directory, files[i].Remote);
                if (File.Exists(fileSubDir) && !File.Exists(fileName))
                    File.Move(fileSubDir, fileName);
            }
            // Download new file.
            if (!File.Exists(fileName))
            {
                await using FileStream localFileStream = File.Create(fileName);
                if (Client is { })
                {
                    if (document is { })
                    {
                        await Client.DownloadFileAsync(document, localFileStream);
                        await ContextManager.DocumentRepository.SaveAsync(document.ID, tgDownloadSettings.SourceVm.SourceId, messageBase.ID, fileName, files[i].Size, accessHash);
                    }
                    else if (photo is { })
                    {
                        //WClient.DownloadFileAsync(photo, localFileStream, new PhotoSize
                        //    { h = photo.sizes[i].Height, w = photo.sizes[i].Width, 
                        //        size = photo.sizes[i].FileSize, type = photo.sizes[i].Type })
                        await Client.DownloadFileAsync(photo, localFileStream);
                    }
                }
                localFileStream.Close();
            }
            // Store message.
            bool isExistsMessage = await ContextManager.MessageRepository.GetExistsAsync(tgDownloadSettings.SourceVm.SourceFirstId, tgDownloadSettings.SourceVm.SourceId);
            if (document is not null && ((isExistsMessage && tgDownloadSettings.IsRewriteMessages) || !isExistsMessage))
            {
                await ContextManager.MessageRepository.SaveAsync(messageBase.ID, tgDownloadSettings.SourceVm.SourceId, files[i].DtCreate,
                    TgEnumMessageType.Document, files[i].Size, files[i].Remote);
            }
            else if (photo is not null && ((isExistsMessage && tgDownloadSettings.IsRewriteMessages) || !isExistsMessage))
            {
                await ContextManager.MessageRepository.SaveAsync(messageBase.ID, tgDownloadSettings.SourceVm.SourceId, files[i].DtCreate,
                    TgEnumMessageType.Photo, files[i].Size, files[i].Remote);
            }
            // Set file date time.
            if (File.Exists(fileName))
            {
                //File.SetCreationTime(fileName, files[i].DtCreate);
                FileInfo fileInfo = new(fileName)
                {
                    CreationTimeUtc = files[i].DtCreate,
                    LastAccessTimeUtc = files[i].DtCreate,
                    LastWriteTimeUtc = files[i].DtCreate
                };
            }
        }
    }

    //private long GetAccessHash(long channelId) => Client?.GetAccessHashFor<Channel>(channelId) ?? 0;

    //private long GetAccessHash(string userName)
    //{
    //	Contacts_ResolvedPeer contactsResolved = Client.Contacts_ResolveUsername(userName).ConfigureAwait(true).GetAwaiter().GetResult();
    //	//WClient.Channels_JoinChannel(new InputChannel(channelId, accessHash)).ConfigureAwait(true).GetAwaiter().GetResult();
    //	return GetAccessHash(contactsResolved.peer.ID);
    //}

    private async Task<long> GetPeerIdAsync(string userName) => (await Client.Contacts_ResolveUsername(userName)).peer.ID;

    // AUTH_KEY_DUPLICATED  | rpcException.Code, 406
    // "Could not read payload length : Connection shut down"
    //public string GetClientExceptionMessage() =>
    //    ClientException switch
    //    {
    //        RpcException rpcException => rpcException.InnerException is null
    //            ? $"{rpcException.Code} | {rpcException.Message}"
    //            : $"{rpcException.Code} | {rpcException.Message} | {rpcException.InnerException.Message}",
    //        { } ex => ex.InnerException is null ? ex.Message : $"{ex.Message} | {ex.InnerException.Message}",
    //        _ => string.Empty
    //    };

    public void LoginUserConsole(bool isProxyUpdate = false)
    {
        ClientException = new();
        if (Client is null)
            return;

        try
        {
            Me = Client.LoginUserIfNeeded().Result;
            UpdateStateMessageAsync(string.Empty);
        }
        catch (Exception ex)
        {
            SetClientExceptionAsync(ex).GetAwaiter().GetResult();
            Me = null;
        }
        finally
        {
            CheckClientIsReady();
            if (isProxyUpdate && IsReady)
            {
                TgSqlTableAppModel app = ContextManager.AppRepository.GetFirstAsync().Result;
                app.ProxyUid = ContextManager.AppRepository.GetCurrentProxyAsync().Result.Uid;
                ContextManager.AppRepository.SaveAsync(app).GetAwaiter().GetResult();
            }
        }
    }

    public async Task LoginUserDesktopAsync(bool isProxyUpdate = false)
    {
        ClientException = new();
        if (Client is null)
            return;

        try
        {
            Me = await Client.LoginUserIfNeeded();
            await UpdateStateMessageAsync(string.Empty);
        }
        catch (Exception ex)
        {
            await SetClientExceptionAsync(ex);
            Me = null;
        }
        finally
        {
            CheckClientIsReady();
            if (isProxyUpdate && IsReady)
            {
                TgSqlTableAppModel app = await ContextManager.AppRepository.GetFirstAsync();
                app.ProxyUid = (await ContextManager.AppRepository.GetCurrentProxyAsync()).Uid;
                await ContextManager.AppRepository.SaveAsync(app);
            }
            await AfterClientConnectAsync();
        }
    }

    public void Disconnect()
    {
        IsProxyUsage = false;
        UpdateStateSourceAsync(0, 0, string.Empty);
        UpdateStateMessageAsync(string.Empty);
        UpdateStateProxyAsync(TgLocale.ProxyIsDisconnect);
        UpdateStateConnectAsync(TgLocale.MenuClientIsDisconnected);
        if (Client is null) return;
        Client.OnUpdate -= OnUpdateClientAsync;
        Client.OnOther -= OnClientOtherAsync;
        Client.Dispose();
        Client = null;
        ClientException = new();
        Me = null;
        CheckClientIsReady();
    }

    private async Task SetClientExceptionAsync(Exception ex,
        [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
    {
        ClientException.Set(ex);
        await UpdateStateExceptionAsync(filePath, lineNumber, memberName, ClientException.Message);
    }

    private async Task SetProxyExceptionAsync(Exception ex,
        [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
    {
        ProxyException.Set(ex);
        await UpdateStateExceptionAsync(filePath, lineNumber, memberName, ProxyException.Message);
    }

    #endregion

    #region Public and private methods

    //private void TryCatchAction(Action action)
    //{
    //    try
    //    {
    //        action();
    //    }
    //    catch (Exception ex)
    //    {
    //        SetClientExceptionAsync(ex).GetAwaiter().GetResult();
    //        if (ClientException.Message.Contains("You must connect to Telegram first"))
    //        {
    //            LoginUserConsole();
    //            UpdateStateMessageAsync("Reconnect client ...");
    //        }
    //    }
    //}

    private async Task TryCatchFuncAsync(Func<Task> asyncAction)
    {
        try
        {
            await asyncAction();
        }
        catch (Exception ex)
        {
            await SetClientExceptionAsync(ex);
            // CHANNEL_INVALID | BadMsgNotification 48
            if (ClientException.Message.Contains("You must connect to Telegram first"))
            {
                await UpdateStateMessageAsync("Reconnect client ...");
                await LoginUserDesktopAsync();
            }
        }
    }

    #endregion
}