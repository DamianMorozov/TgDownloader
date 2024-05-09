// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloader.Helpers;

/// <summary>
/// Client helper.
/// </summary>
public sealed class TgClientHelper : ObservableObject, ITgHelper
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
	private TgEfSourceRepository SourceRepository { get; } = new(TgEfUtils.CreateEfContext());
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
    private IEnumerable<TgEfFilterEntity> Filters { get; set; }
    public Func<string, Task> UpdateTitleAsync { get; private set; }
    public Func<string, Task> UpdateStateConnectAsync { get; private set; }
    public Func<string, Task> UpdateStateProxyAsync { get; private set; }
    public Func<long, int, string, Task> UpdateStateSourceAsync { get; private set; }
    public Func<string, int, string, string, Task> UpdateStateExceptionAsync { get; private set; }
    public Func<string, Task> UpdateStateExceptionShortAsync { get; private set; }
    public Func<Task> AfterClientConnectAsync { get; private set; }
    public Func<string, string?> ConfigClientDesktop { get; private set; }
    public Func<long, Task> UpdateStateItemSourceAsync { get; private set; }
    public Func<long, int, string, long, long, long, bool, Task> UpdateStateFileAsync { get; private set; }
    public Func<string, Task> UpdateStateMessageAsync { get; private set; }
    private TgEfAppRepository AppRepository { get; } = new(TgEfUtils.EfContext);
    private TgEfProxyRepository ProxyRepository { get; } = new(TgEfUtils.EfContext);
    private TgEfDocumentRepository DocumentRepository { get; } = new(TgEfUtils.EfContext);
    private TgEfFilterRepository FilterRepository { get; } = new(TgEfUtils.EfContext);
    private TgEfMessageRepository MessageRepository { get; } = new(TgEfUtils.EfContext);

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
        Filters = Enumerable.Empty<TgEfFilterEntity>();

        UpdateTitleAsync = _ => Task.CompletedTask;
        UpdateStateConnectAsync = _ => Task.CompletedTask;
        UpdateStateProxyAsync = _ => Task.CompletedTask;
        UpdateStateExceptionAsync = (_, _, _, _) => Task.CompletedTask;
        UpdateStateExceptionShortAsync = _ => Task.CompletedTask;
		UpdateStateSourceAsync = (_, _, _) => Task.CompletedTask;
        AfterClientConnectAsync = () => Task.CompletedTask;
        ConfigClientDesktop = what => null;
        UpdateStateItemSourceAsync = _ => Task.CompletedTask;
        UpdateStateFileAsync = (_, _, _, _, _, _, _) => Task.CompletedTask;
        UpdateStateMessageAsync = _ => Task.CompletedTask;

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

    public void SetupUpdateStateConnect(Func<string, Task> updateStateConnectAsync) => 
	    UpdateStateConnectAsync = updateStateConnectAsync;

    public void SetupUpdateStateProxy(Func<string, Task> updateStateProxyAsync) => 
	    UpdateStateProxyAsync = updateStateProxyAsync;

    public void SetupUpdateStateSource(Func<long, int, string, Task> updateStateSourceAsync) => 
	    UpdateStateSourceAsync = updateStateSourceAsync;

    public void SetupUpdateStateException(Func<string, int, string, string, Task> updateStateExceptionAsync) => 
	    UpdateStateExceptionAsync = updateStateExceptionAsync;

    public void SetupUpdateStateExceptionShort(Func<string, Task> updateStateExceptionShortAsync) =>
	    UpdateStateExceptionShortAsync = updateStateExceptionShortAsync;

    public void SetupAfterClientConnect(Func<Task> afterClientConnectAsync) => 
	    AfterClientConnectAsync = afterClientConnectAsync;

    public void SetupGetClientDesktopConfig(Func<string, string?> getClientDesktopConfig) => 
	    ConfigClientDesktop = getClientDesktopConfig;

    public void SetupUpdateTitle(Func<string, Task> updateTitleAsync) => 
	    UpdateTitleAsync = updateTitleAsync;

    public void SetupUpdateStateItemSource(Func<long, Task> updateStateItemSourceAsync) => 
	    UpdateStateItemSourceAsync = updateStateItemSourceAsync;

    public void SetupUpdateStateFile(Func<long, int, string, long, long, long, bool, Task> updateStateFileAsync) => 
	    UpdateStateFileAsync = updateStateFileAsync;

    public void SetupUpdateStateMessage(Func<string, Task> updateStateMessageAsync) =>
		UpdateStateMessageAsync = updateStateMessageAsync;

    public bool CheckClientIsReady()
    {
        bool result = Client is { Disconnected: false };
        if (!result) 
            return ClientResultDisconnected();
        if (!TgAppSettings.AppXml.IsExistsFileSession)
            return ClientResultDisconnected();
        //if (!(!TgAppSettings.IsUseProxy ||
        //      (TgAppSettings.IsUseProxy &&
        //       (ContextManager.ProxyRepository.Get(AppRepository.GetFirstProxyUid) ??
        //        ContextManager.ProxyRepository.GetNew()).IsExist)))
        TgEfOperResult<TgEfProxyEntity> operResult = ProxyRepository.GetCurrentProxy(AppRepository.GetCurrentApp());
		//if (!(!TgAppSettings.AppXml.IsUseProxy || (TgAppSettings.AppXml.IsUseProxy && operResult.IsExists)))
		//    return ClientResultDisconnected();
		if (TgAppSettings.IsUseProxy && !operResult.IsExists)
			return ClientResultDisconnected();
		if (ProxyException.IsExist || ClientException.IsExist)
			return ClientResultDisconnected();
		return ClientResultConnected();
    }

    private bool ClientResultDisconnected()
    {
	    UpdateStateSourceAsync(0, 0, string.Empty);
	    UpdateStateProxyAsync(TgLocale.ProxyIsDisconnect);
	    UpdateStateConnectAsync(TgLocale.MenuClientIsDisconnected);
	    return IsReady = false;
    }

	private bool ClientResultConnected()
    {
	    UpdateStateConnectAsync(TgLocale.MenuClientIsConnected);
	    return IsReady = true;
    }
    
    public void ConnectSessionConsole(Func<string, string?>? config, TgEfProxyEntity proxy)
    {
        if (IsReady) return;
        Disconnect();

        Client = new(config);
        ConnectThroughProxyAsync(proxy, false).GetAwaiter().GetResult();
        Client.OnUpdates += OnUpdatesClientAsync;
        Client.OnOther += OnClientOtherAsync;

        LoginUserConsole(true);
    }

    public async Task ConnectSessionConsoleAsync(Func<string, string?>? config, TgEfProxyEntity proxy)
    {
        if (IsReady) return;
        Disconnect();

        Client = new(config);
        await ConnectThroughProxyAsync(proxy, false);
        Client.OnUpdates += OnUpdatesClientAsync;
        Client.OnOther += OnClientOtherAsync;

        LoginUserConsole(true);
    }

    public async Task ConnectSessionAsync(ITgDbProxy proxy)
    {
        if (IsReady) return;
        Disconnect();

        Client = new(ConfigClientDesktop);
        await ConnectThroughProxyAsync(proxy, true);
        Client.OnUpdates += OnUpdatesClientAsync;
        Client.OnOther += OnClientOtherAsync;

        await LoginUserDesktopAsync(true);
    }

    public async Task ConnectThroughProxyAsync(ITgDbProxy proxy, bool isDesktop)
    {
        IsProxyUsage = false;
        if (!CheckClientIsReady()) return;
        if (Client is null) return;
        if (proxy.Uid == Guid.Empty) return;
        if (!isDesktop && !TgAppSettings.IsUseProxy) return;
        if (Equals(proxy.Type, TgEnumProxyType.None)) return;
        if (proxy is TgEfProxyEntity efProxy)
	        if (!TgEfUtils.GetEfValid<TgEfProxyEntity>(efProxy).IsValid) return;

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

    public string GetUserUpdatedName(long id) => DicUsersUpdated.TryGetValue(ReduceChatId(id), out User? user) ? user.username : string.Empty;

    public async Task<Channel?> GetChannelAsync(TgDownloadSettingsViewModel tgDownloadSettings)
    {
        // Collect chats from Telegram.
        if (!DicChatsAll.Any())
            await CollectAllChatsAsync();

        if (tgDownloadSettings.SourceVm.IsReadySourceId)
        {
            tgDownloadSettings.SourceVm.SourceId = ReduceChatId(tgDownloadSettings.SourceVm.SourceId);
            foreach (KeyValuePair<long, ChatBase> chat in DicChatsAll)
            {
                if (chat.Value is Channel channel && Equals(channel.id, tgDownloadSettings.SourceVm.SourceId) && 
                    await IsChatBaseAccessAsync(channel)) return channel;
            }
        }
        else
        {
            foreach (KeyValuePair<long, ChatBase> chat in DicChatsAll)
            {
                if (chat.Value is Channel channel && Equals(channel.username, tgDownloadSettings.SourceVm.SourceUserName) && 
                    await IsChatBaseAccessAsync(channel)) return channel;
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

    public async Task<ChatBase?> GetChatBaseAsync(TgDownloadSettingsViewModel tgDownloadSettings)
    {
        // Collect chats from Telegram.
        if (!DicChatsAll.Any())
            await CollectAllChatsAsync();
        
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

    public async Task<TgDownloadSmartSource> CreateSmartSourceCoreAsync(TgDownloadSettingsViewModel tgDownloadSettings)
    {
        TgDownloadSmartSource smartSource = new();
        if (!DicChatsAll.Any())
            await CollectAllChatsAsync();
		if (tgDownloadSettings.SourceVm.IsReadySourceId)
        {
            tgDownloadSettings.SourceVm.SourceId = ReduceChatId(tgDownloadSettings.SourceVm.SourceId);
            ChatBase? chatBase = DicChatsAll.FirstOrDefault(x => x.Key.Equals(tgDownloadSettings.SourceVm.SourceId)).Value;
            if (chatBase is { }) smartSource.ChatBase = chatBase;
        }
        else
        {
	        ChatBase? chatBase = DicChatsAll.FirstOrDefault(x => x.Value.MainUsername.Equals(tgDownloadSettings.SourceVm.SourceUserName)).Value;
	        if (chatBase is { }) smartSource.ChatBase = chatBase;
		}
		return smartSource;
    }

    public async Task<Bots_BotInfo?> GetBotInfoAsync(TgDownloadSettingsViewModel tgDownloadSettings)
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
				FillEnumerableChats(messages.chats);
                return messages.chats;
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
                FillEnumerableChats(messages.chats);
                return messages.chats;
            }
        }
        return new();
    }

    private void FillEnumerableChats(Dictionary<long, ChatBase> chats)
    {
	    DicChatsAll = chats;

		List<Channel> listChannels = EnumerableChannels.OrderBy(i => i.username).ThenBy(i => i.id).ToList();
        listChannels.Clear();
        List<ChatBase> listChats = EnumerableChats.OrderBy(i => i.MainUsername).ThenBy(i => i.ID).ToList();
        listChats.Clear();
        List<Channel> listGroups = EnumerableGroups.OrderBy(i => i.username).ThenBy(i => i.id).ToList();
        listGroups.Clear();
        List<ChatBase> listSmallGroups = EnumerableSmallGroups.OrderBy(i => i.MainUsername).ThenBy(i => i.ID).ToList();
        listSmallGroups.Clear();

        foreach (KeyValuePair<long, ChatBase> chat in chats)
        {
            listChats.Add(chat.Value);
            switch (chat.Value)
            {
                case Chat smallGroup when (smallGroup.flags & Chat.Flags.deactivated) is 0:
                    listSmallGroups.Add(chat.Value);
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

    public async Task OnUpdatesClientAsync(IObject arg)
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
        long sourceId = channel?.ID ?? 0;
		switch (update)
        {
            case UpdateNewChannelMessage updateNewChannelMessage:
                //if (channel is not null && updateNewChannelMessage.message.Peer.ID.Equals(channel.ID))
                await UpdateStateSourceAsync(sourceId, 0, $"New channel message [{updateNewChannelMessage}]{channelLabel}");
                break;
            case UpdateNewMessage updateNewMessage:
                await UpdateStateSourceAsync(sourceId, 0, $"New message [{updateNewMessage}]{channelLabel}");
                break;
            case UpdateMessageID updateMessageId:
                await UpdateStateSourceAsync(sourceId, 0, $"Message ID [{updateMessageId}]{channelLabel}");
                break;
            case UpdateDeleteChannelMessages updateDeleteChannelMessages:
                await UpdateStateSourceAsync(sourceId, 0, $"Delete channel messages [{string.Join(", ", updateDeleteChannelMessages.messages)}]{channelLabel}");
                break;
            case UpdateDeleteMessages updateDeleteMessages:
                await UpdateStateSourceAsync(sourceId, 0, $"Delete messages [{string.Join(", ", updateDeleteMessages.messages)}]{channelLabel}");
                break;
            case UpdateChatUserTyping updateChatUserTyping:
                await UpdateStateSourceAsync(sourceId, 0, $"Chat user typing [{updateChatUserTyping}]{channelLabel}");
                break;
            case UpdateChatParticipants { participants: ChatParticipants chatParticipants }:
                await UpdateStateSourceAsync(sourceId, 0, $"Chat participants [{chatParticipants.ChatId} | {string.Join(", ", chatParticipants.Participants.Length)}]{channelLabel}");
                break;
            case UpdateUserStatus updateUserStatus:
                await UpdateStateSourceAsync(sourceId, 0, $"User status [{updateUserStatus.user_id} | {updateUserStatus}]{channelLabel}");
                break;
            case UpdateUserName updateUserName:
                await UpdateStateSourceAsync(sourceId, 0, $"User name [{updateUserName.user_id} | {string.Join(", ", updateUserName.usernames.Select(item => item.username))}]{channelLabel}");
                break;
            case UpdateNewEncryptedMessage updateNewEncryptedMessage:
                await UpdateStateSourceAsync(sourceId, 0, $"New encrypted message [{updateNewEncryptedMessage}]{channelLabel}");
                break;
            case UpdateEncryptedChatTyping updateEncryptedChatTyping:
                await UpdateStateSourceAsync(sourceId, 0, $"Encrypted chat typing [{updateEncryptedChatTyping}]{channelLabel}");
                break;
            case UpdateEncryption updateEncryption:
                await UpdateStateSourceAsync(sourceId, 0, $"Encryption [{updateEncryption}]{channelLabel}");
                break;
            case UpdateEncryptedMessagesRead updateEncryptedMessagesRead:
                await UpdateStateSourceAsync(sourceId, 0, $"Encrypted message read [{updateEncryptedMessagesRead}]{channelLabel}");
                break;
            case UpdateChatParticipantAdd updateChatParticipantAdd:
                await UpdateStateSourceAsync(sourceId, 0, $"Chat participant add [{updateChatParticipantAdd}]{channelLabel}");
                break;
            case UpdateChatParticipantDelete updateChatParticipantDelete:
                await UpdateStateSourceAsync(sourceId, 0, $"Chat participant delete [{updateChatParticipantDelete}]{channelLabel}");
                break;
            case UpdateDcOptions updateDcOptions:
                await UpdateStateSourceAsync(sourceId, 0, $"Dc options [{string.Join(", ", updateDcOptions.dc_options.Select(item => item.id))}]{channelLabel}");
                break;
            case UpdateNotifySettings updateNotifySettings:
                await UpdateStateSourceAsync(sourceId, 0, $"Notify settings [{updateNotifySettings}]{channelLabel}");
                break;
            case UpdateServiceNotification updateServiceNotification:
                await UpdateStateSourceAsync(sourceId, 0, $"Service notification [{updateServiceNotification}]{channelLabel}");
                break;
            case UpdatePrivacy updatePrivacy:
                await UpdateStateSourceAsync(sourceId, 0, $"Privacy [{updatePrivacy}]{channelLabel}");
                break;
            case UpdateUserPhone updateUserPhone:
                await UpdateStateSourceAsync(sourceId, 0, $"User phone [{updateUserPhone}]{channelLabel}");
                break;
            case UpdateReadHistoryInbox updateReadHistoryInbox:
                await UpdateStateSourceAsync(sourceId, 0, $"Read history inbox [{updateReadHistoryInbox}]{channelLabel}");
                break;
            case UpdateReadHistoryOutbox updateReadHistoryOutbox:
                await UpdateStateSourceAsync(sourceId, 0, $"Read history outbox [{updateReadHistoryOutbox}]{channelLabel}");
                break;
            case UpdateWebPage updateWebPage:
                await UpdateStateSourceAsync(sourceId, 0, $"Web page [{updateWebPage}]{channelLabel}");
                break;
            case UpdateReadMessagesContents updateReadMessagesContents:
                await UpdateStateSourceAsync(sourceId, 0, $"Read messages contents [{string.Join(", ", updateReadMessagesContents.messages.Select(item => item.ToString()))}]{channelLabel}");
                break;
            case UpdateEditChannelMessage updateEditChannelMessage:
                await UpdateStateSourceAsync(sourceId, 0, $"Edit channel message [{updateEditChannelMessage}]{channelLabel}");
                break;
            case UpdateEditMessage updateEditMessage:
                await UpdateStateSourceAsync(sourceId, 0, $"Edit message [{updateEditMessage}]{channelLabel}");
                break;
            case UpdateUserTyping updateUserTyping:
                await UpdateStateSourceAsync(sourceId, 0, $"User typing [{updateUserTyping}]{channelLabel}");
                break;
            case UpdateChannelMessageViews updateChannelMessageViews:
                await UpdateStateSourceAsync(sourceId, 0, $"Channel message views [{updateChannelMessageViews}]{channelLabel}");
                break;
            case UpdateChannel updateChannel:
                await UpdateStateSourceAsync(sourceId, 0, $"Channel [{updateChannel}]");
                break;
            case UpdateChannelReadMessagesContents updateChannelReadMessages:
                await UpdateStateSourceAsync(sourceId, 0, $"Channel read messages [{string.Join(", ", updateChannelReadMessages.messages)}]{channelLabel}");
                break;
            case UpdateChannelUserTyping updateChannelUserTyping:
                await UpdateStateSourceAsync(sourceId, 0, $"Channel user typing [{updateChannelUserTyping}]{channelLabel}");
                break;
            case UpdateMessagePoll updateMessagePoll:
                await UpdateStateSourceAsync(sourceId, 0, $"Message poll [{updateMessagePoll}]{channelLabel}");
                break;
            case UpdateChannelTooLong updateChannelTooLong:
                await UpdateStateSourceAsync(sourceId, 0, $"Channel too long [{updateChannelTooLong}]{channelLabel}");
                break;
            case UpdateReadChannelInbox updateReadChannelInbox:
                await UpdateStateSourceAsync(sourceId, 0, $"Channel inbox [{updateReadChannelInbox}]{channelLabel}");
                break;
            case UpdateChatParticipantAdmin updateChatParticipantAdmin:
                await UpdateStateSourceAsync(sourceId, 0, $"Chat participant admin[{updateChatParticipantAdmin}]{channelLabel}");
                break;
            case UpdateNewStickerSet updateNewStickerSet:
                await UpdateStateSourceAsync(sourceId, 0, $"New sticker set [{updateNewStickerSet}]{channelLabel}");
                break;
            case UpdateStickerSetsOrder updateStickerSetsOrder:
                await UpdateStateSourceAsync(sourceId, 0, $"Sticker sets order [{updateStickerSetsOrder}]{channelLabel}");
                break;
            case UpdateStickerSets updateStickerSets:
                await UpdateStateSourceAsync(sourceId, 0, $"Sticker sets [{updateStickerSets}]{channelLabel}");
                break;
            case UpdateSavedGifs updateSavedGifs:
                await UpdateStateSourceAsync(sourceId, 0, $"SavedGifs [{updateSavedGifs}]{channelLabel}");
                break;
            case UpdateBotInlineQuery updateBotInlineQuery:
                await UpdateStateSourceAsync(sourceId, 0, $"Bot inline query [{updateBotInlineQuery}]{channelLabel}");
                break;
            case UpdateBotInlineSend updateBotInlineSend:
                await UpdateStateSourceAsync(sourceId, 0, $"Bot inline send [{updateBotInlineSend}]{channelLabel}");
                break;
            case UpdateBotCallbackQuery updateBotCallbackQuery:
                await UpdateStateSourceAsync(sourceId, 0, $"Bot cCallback query [{updateBotCallbackQuery}]{channelLabel}");
                break;
            case UpdateInlineBotCallbackQuery updateInlineBotCallbackQuery:
                await UpdateStateSourceAsync(sourceId, 0, $"Inline bot callback query [{updateInlineBotCallbackQuery}]{channelLabel}");
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
#if DEBUG
	        Debug.WriteLine($"{nameof(authSentCode)}: {authSentCode}");
#endif
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
            }, isLoginConsole: true);
        }
    }

    private async Task<Messages_ChatFull?> PrintChatsInfoChannelAsync(Channel channel, bool isFull, bool isSilent, bool isSave)
    {
        Messages_ChatFull? chatFull = null;
        try
        {
            chatFull = await Client.Channels_GetFullChannel(channel);
            if (isSave)
                await SourceRepository.SaveAsync(new() { Id = channel.id, UserName = channel.username, Title = channel.title });
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

    public async Task<bool> IsChatBaseAccessAsync(ChatBase chatBase)
    {
        if (Client is null || chatBase.ID is 0)
            return false;

        bool result = false;
        await TryCatchFuncAsync(async () =>
        {
	        await Client.ReadHistory(chatBase);
            result = true;
		}, isLoginConsole: true);
		
        return result;
    }

    public async Task<int> GetChannelMessageIdWithLockAsync(TgDownloadSettingsViewModel? tgDownloadSettings, ChatBase chatBase, TgEnumPosition position) => 
        await GetChannelMessageIdCoreAsync(tgDownloadSettings, chatBase, position);

    public async Task<int> GetChannelMessageIdWithLockAsync(ChatBase chatBase, TgEnumPosition position) => 
        await GetChannelMessageIdCoreAsync(null, chatBase, position);

    public async Task<int> GetChannelMessageIdAsync(TgDownloadSettingsViewModel? tgDownloadSettings, ChatBase chatBase,
        TgEnumPosition position) =>
        await GetChannelMessageIdCoreAsync(tgDownloadSettings, chatBase, position);

    private async Task<int> GetChannelMessageIdCoreAsync(TgDownloadSettingsViewModel? tgDownloadSettings, ChatBase chatBase, TgEnumPosition position)
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

    public async Task<int> GetChannelMessageIdLastAsync(TgDownloadSettingsViewModel? tgDownloadSettings, ChatBase chatBase) =>
        await GetChannelMessageIdWithLockAsync(tgDownloadSettings, chatBase, TgEnumPosition.Last);

    private int GetChannelMessageIdLastCore(ChatFullBase chatFullBase) => 
        chatFullBase is ChannelFull channelFull ? channelFull.read_inbox_max_id : 0;

    public async Task SetChannelMessageIdFirstAsync(TgDownloadSettingsViewModel tgDownloadSettings, ChatBase chatBase) =>
        await GetChannelMessageIdAsync(tgDownloadSettings, chatBase, TgEnumPosition.First);

    private async Task<int> SetChannelMessageIdFirstCoreAsync(TgDownloadSettingsViewModel tgDownloadSettings, ChatBase chatBase,
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
            await UpdateStateSourceAsync(chatBase.ID, 0, $"Read from {offset} to {offset + partition} messages");
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
        await UpdateStateSourceAsync(chatBase.ID, 0, $"Get the first ID message '{result}' is complete.");
        return result;
    }

    public TgDownloadSmartSource CreateSmartSource(TgDownloadSettingsViewModel tgDownloadSettings, bool isSilent)
    {
	    TgEfOperResult<TgEfSourceEntity> operResult = SourceRepository
			    .Get(new TgEfSourceEntity { Id = tgDownloadSettings.SourceVm.SourceId }, isNoTracking: false);

	    TgDownloadSmartSource smartSource = CreateSmartSourceCoreAsync(tgDownloadSettings).GetAwaiter().GetResult();
	    if (smartSource.ChatBase is not null && IsChatBaseAccessAsync(smartSource.ChatBase).GetAwaiter().GetResult())
	    {
		    tgDownloadSettings.SourceVm.SourceUserName = !string.IsNullOrEmpty(smartSource.ChatBase.MainUsername)
			    ? smartSource.ChatBase.MainUsername
			    : string.Empty;
		    tgDownloadSettings.SourceVm.SourceLastId =
			    GetChannelMessageIdLastAsync(tgDownloadSettings, smartSource.ChatBase).GetAwaiter().GetResult();
		    Messages_ChatFull? chatFull =
			    PrintChatsInfoChatBaseAsync(smartSource.ChatBase, isFull: true, isSilent).GetAwaiter().GetResult();
		    if (chatFull?.full_chat is ChannelFull chatBaseFull)
			    tgDownloadSettings.SourceVm.SetSource(chatBaseFull.ID, smartSource.ChatBase.Title, chatBaseFull.About);
	    }

	    // Save.
	    operResult.Item.UserName = tgDownloadSettings.SourceVm.SourceUserName;
	    operResult.Item.Count = tgDownloadSettings.SourceVm.SourceLastId;
	    operResult.Item.Id = tgDownloadSettings.SourceVm.SourceId;
	    operResult.Item.Title = tgDownloadSettings.SourceVm.SourceTitle;
	    operResult.Item.About = tgDownloadSettings.SourceVm.SourceAbout;
	    SourceRepository.Save(operResult.Item);
	    return smartSource;
    }

    /// <summary>
    /// Update source from Telegram.
    /// </summary>
    /// <param name="sourceVm"></param>
    /// <param name="tgDownloadSettings"></param>
    public void UpdateSourceDb(TgEfSourceViewModel sourceVm, TgDownloadSettingsViewModel tgDownloadSettings)
    {
        TgDownloadSmartSource smartSource = CreateSmartSource(tgDownloadSettings, true);
        if (smartSource.ChatBase is not null)
        {
            sourceVm.Item.Backup(tgDownloadSettings.SourceVm.Item);
        }
    }

    private async Task UpdateSourceTgAsync(Channel channel, string about, int count)
    {
        TgEfSourceEntity itemFind = (await SourceRepository.GetAsync(new TgEfSourceEntity
	        { Id = channel.id }, isNoTracking: false)).Item;

        itemFind.Id = channel.id;
        itemFind.UserName = channel.username;
        if (!string.IsNullOrEmpty(channel.title))
            itemFind.Title = channel.title;
        if (!string.IsNullOrEmpty(about))
            itemFind.About = about;
        itemFind.Count = count;
        
        await SourceRepository.SaveAsync(itemFind);
    }

    private async Task UpdateSourceTgAsync(Channel channel, int count) => 
        await UpdateSourceTgAsync(channel, string.Empty, count);

    public async Task ScanSourcesTgConsoleAsync(TgDownloadSettingsViewModel tgDownloadSettings, TgEnumSourceType sourceType)
    {
        await TryCatchFuncAsync(async () =>
        {
            LoginUserConsole();
            switch (sourceType)
            {
                case TgEnumSourceType.Chat:
                    await UpdateStateSourceAsync(tgDownloadSettings.SourceVm.SourceId, 0, TgLocale.CollectChats);
                    await CollectAllChatsAsync();
                    break;
                case TgEnumSourceType.Dialog:
                    await UpdateStateSourceAsync(tgDownloadSettings.SourceVm.SourceId, 0, TgLocale.CollectDialogs);
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
                                await UpdateStateSourceAsync(tgDownloadSettings.SourceVm.SourceId, 0, $"{channel} | {messagesCount} | {TgDataFormatUtils.TrimStringEnd(channelFull.about)}");
                            }
                        }
                        else
                        {
                            await UpdateSourceTgAsync(channel, messagesCount);
                            await UpdateStateSourceAsync(tgDownloadSettings.SourceVm.SourceId, 0, $"{channel} | {messagesCount}");
                        }
                    }, isLoginConsole: true);
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
                        await UpdateStateSourceAsync(tgDownloadSettings.SourceVm.SourceId, 0, $"{group} | {messagesCount}");
                    }, isLoginConsole: true);
                }
            }
        }, isLoginConsole: true);
        await UpdateTitleAsync(string.Empty);
    }

    public async Task ScanSourcesTgDesktopAsync(TgEnumSourceType sourceType, Func<TgEfSourceViewModel, Task> afterScanAsync)
    {
        await TryCatchFuncAsync(async () =>
        {
            switch (sourceType)
            {
                case TgEnumSourceType.Chat:
                    await UpdateStateSourceAsync(0, 0, TgLocale.CollectChats);
                    switch (IsReady)
                    {
                        case true when Client is not null:
                            Messages_Chats messages = await Client.Messages_GetAllChats().ConfigureAwait(false);
                            FillEnumerableChats(messages.chats);
                            break;
                    }
                    await AfterCollectSourcesAsync(afterScanAsync);
                    break;
                case TgEnumSourceType.Dialog:
                    await UpdateStateSourceAsync(0, 0, TgLocale.CollectDialogs);
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
        }, isLoginConsole: false);
    }

    private async Task AfterCollectSourcesAsync(Func<TgEfSourceViewModel, Task> afterScanAsync)
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
                    TgEfSourceEntity source = new() { Id = channel.ID };
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
                }, isLoginConsole: true);
            }
            i++;
            await UpdateStateSourceAsync(channel.ID, 0, $"Read channel '{channel.ID}' | {channel.IsActive} | {i} from {count}");
        }

        // ListGroups.
        i = 0;
        count = EnumerableGroups.Count();
        foreach (Channel group in EnumerableGroups)
        {
            await TryCatchFuncAsync(async () =>
            {
                TgEfSourceEntity source = new() { Id = group.ID };
                if (group.IsActive)
                {
                    int messagesCount = await GetChannelMessageIdLastAsync(new(), group);
                    source.Count = messagesCount;
                }
                await afterScanAsync(new(source));
            }, isLoginConsole: true);
            i++;
            await UpdateStateSourceAsync(group.ID, 0, $"Read channel '{group.ID}' | {group.IsActive} | {i} from {count}");
        }
    }

    public async Task DownloadAllDataAsync(TgDownloadSettingsViewModel tgDownloadSettings)
    {
        TgDownloadSmartSource smartSource = CreateSmartSource(tgDownloadSettings, false);
        await TryCatchFuncAsync(async () =>
        {
            // Set filters.
            Filters = (await FilterRepository.GetListAsync(TgEnumTableTopRecords.All, isNoTracking: false))
	            .Items.Where(x => x.IsEnabled);

            switch (TgAsyncUtils.AppType)
            {
                case TgEnumAppType.Console:
                    LoginUserConsole();
                    break;
                case TgEnumAppType.Desktop:
                    await LoginUserDesktopAsync();
                    break;
                default:
                    throw new InvalidEnumArgumentException(TgLocale.MenuDownloadException, (int)TgAsyncUtils.AppType, typeof(TgEnumAppType));
            }

            bool dirExists = await CreateDestDirectoryIfNotExistsAsync(tgDownloadSettings);
            if (!dirExists) return;

            tgDownloadSettings.SourceVm.SetIsDownload(true);
            bool isAccessToMessages = await Client.Channels_ReadMessageContents(smartSource.ChatBase as TlChannel);
            if (isAccessToMessages)
            {
	            while (tgDownloadSettings.SourceVm.SourceFirstId <= tgDownloadSettings.SourceVm.SourceLastId)
	            {
		            if (Client is null || (Client is not null && Client.Disconnected) || !tgDownloadSettings.SourceVm.IsDownload)
			            break;
		            await TryCatchFuncAsync(async () =>
		            {
			            Messages_MessagesBase messages = smartSource.ChatBase is not null
					            ? await Client.Channels_GetMessages(smartSource.ChatBase as TlChannel, tgDownloadSettings.SourceVm.SourceFirstId)
					            : await Client.GetMessages(smartSource.ChatBase, tgDownloadSettings.SourceVm.SourceFirstId);
			            await UpdateTitleAsync($"{TgCommonUtils.CalcSourceProgress(tgDownloadSettings.SourceVm.SourceLastId, tgDownloadSettings.SourceVm.SourceFirstId):#00.00} %");
			            foreach (MessageBase message in messages.Messages)
			            {
				            // Check message exists.
				            if (message.Date > DateTime.MinValue)
				            {
					            await DownloadDataAsync(tgDownloadSettings, message);
				            }
				            else
				            {
					            await UpdateStateSourceAsync(tgDownloadSettings.SourceVm.SourceId, message.ID,
						            $"Message {message.ID} is not exists in {tgDownloadSettings.SourceVm.SourceId}!");
				            }
			            }
			            tgDownloadSettings.SourceVm.SourceFirstId++;
		            }, isLoginConsole: true);
	            }
			}
            tgDownloadSettings.SourceVm.SourceFirstId = tgDownloadSettings.SourceVm.SourceLastId;
            tgDownloadSettings.SourceVm.SourceFirstId = tgDownloadSettings.SourceVm.SourceLastId;
            tgDownloadSettings.SourceVm.SetIsDownload(false);
		}, isLoginConsole: true);
		await UpdateTitleAsync(string.Empty);
    }

    public async Task MarkHistoryReadAsync()
    {
        await TryCatchFuncAsync(async () =>
        {
            switch (TgAsyncUtils.AppType)
            {
                case TgEnumAppType.Console:
                    LoginUserConsole();
                    break;
                case TgEnumAppType.Desktop:
                    await LoginUserDesktopAsync();
                    break;
                default:
	                throw new InvalidEnumArgumentException(TgLocale.MenuException, (int)TgAsyncUtils.AppType, typeof(TgEnumSourceType));
            }
        }, isLoginConsole: true);
        
        await CollectAllChatsAsync();
        await UpdateStateMessageAsync("Mark as read all message in the channels: in the progress");
        await TryCatchFuncAsync(async () =>
        {
	        if (Client is not null)
	        {
	            foreach (ChatBase chatBase in EnumerableChats)
	            {
		            await TryCatchFuncAsync(async () =>
		            {
			            bool isSuccess = await Client.ReadHistory(chatBase);
						await UpdateStateMessageAsync(
				            $"Mark as read the source | {chatBase.ID} | " +
				            $"{(string.IsNullOrEmpty(chatBase.MainUsername) ? chatBase.Title : chatBase.MainUsername)}]: {(isSuccess ? "success" : "was already readed")}");
		            }, isLoginConsole: true);
				}
	        }
	        else
	        {
		        await UpdateStateMessageAsync("Mark as read all messages: Client is not connected!");
	        }
        }, isLoginConsole: true);
        await UpdateStateMessageAsync("Mark as read all message in the channels: complete");
    }

    private async Task<bool> CreateDestDirectoryIfNotExistsAsync(TgDownloadSettingsViewModel tgDownloadSettings)
    {
        try
        {
            Directory.CreateDirectory(tgDownloadSettings.SourceVm.SourceDirectory);
        }
        catch (Exception ex)
        {
            await SetClientExceptionAsync(ex);
            return false;
        }
        return true;
    }

    private async Task DownloadDataAsync(TgDownloadSettingsViewModel tgDownloadSettings, MessageBase messageBase)
    {
        if (messageBase is not Message message)
        {
            await UpdateStateSourceAsync(tgDownloadSettings.SourceVm.SourceId, messageBase.ID, "Empty message");
            return;
        }

        await TryCatchFuncAsync(async () =>
        {
            tgDownloadSettings.UpdateSourceWithSettings();
			// Store message.
			await MessageSaveAsync(tgDownloadSettings, message.ID, message.Date, 0, message.message, TgEnumMessageType.Message);
            // Parse documents and photos.
            if ((message.flags & Message.Flags.has_media) is not 0)
            {
	            await DownloadDataCoreAsync(tgDownloadSettings, messageBase, message.media);
            }
            await UpdateStateSourceAsync(tgDownloadSettings.SourceVm.SourceId, message.ID, "Read the message");
            await UpdateStateItemSourceAsync(tgDownloadSettings.SourceVm.SourceId);
        }, isLoginConsole: true);
    }

    private (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] GetFiles(MessageMedia messageMedia)
    {
        string extensionName = string.Empty;
        switch (messageMedia)
        {
            case MessageMediaDocument mediaDocument:
	            if ((mediaDocument.flags & MessageMediaDocument.Flags.has_document) is not 0 && mediaDocument.document is Document document)
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
		            if (string.IsNullOrEmpty(document.Filename) && CheckFileAtFilter(string.Empty, extensionName, document.size))
			            return new (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] { ($"{document.ID}.{extensionName}", document.size,
				            document.date, string.Empty, string.Empty) };
	            }
				break;
            case MessageMediaPhoto mediaPhoto:
	            if (mediaPhoto is { photo: Photo photo })
	            {
		            extensionName = "jpg";
		            //return photo.sizes.Select(x => ($"{photo.ID} {x.Width}x{x.Height}.{GetPhotoExt(x.Type)}", Convert.ToInt64(x.FileSize), photo.date, string.Empty, string.Empty)).ToArray();
		            if (CheckFileAtFilter(string.Empty, extensionName, photo.sizes.Last().FileSize))
			            return new (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] { ($"{photo.ID}.{extensionName}", photo.sizes.Last().FileSize,
				            photo.date, string.Empty, string.Empty) };
	            }
				break;
        }
        return Array.Empty<(string Remote, long Size, DateTime DtCreate, string Local, string Join)>();
    }

    public bool CheckFileAtFilter(string fileName, string extensionName, long size)
    {
        foreach (TgEfFilterEntity filter in Filters)
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

    private void SetFilesLocalNames(TgDownloadSettingsViewModel tgDownloadSettings, MessageBase messageBase,
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

    private async Task DeleteExistsFilesAsync(TgDownloadSettingsViewModel tgDownloadSettings,
          (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] files)
    {
        await TryCatchFuncAsync(async() =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
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
        }, isLoginConsole: true);
    }

    private async Task DownloadDataCoreAsync(TgDownloadSettingsViewModel tgDownloadSettings, MessageBase messageBase, MessageMedia messageMedia)
    {
        (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] files = GetFiles(messageMedia);
        if (Equals(files, Array.Empty<(string Remote, long Size, DateTime DtCreate, string Local, string Join)>()))
            return;
        SetFilesLocalNames(tgDownloadSettings, messageBase, ref files);
		long accessHash = messageMedia switch
        {
	        MessageMediaDocument mediaDocument => 
		        (mediaDocument.flags & MessageMediaDocument.Flags.has_document) is not 0 && mediaDocument.document is Document document ? document.access_hash : 0,
	        MessageMediaPhoto mediaPhoto => mediaPhoto is { photo: Photo photo } ? photo.access_hash : 0,
	        _ => 0
        };

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
	                switch (messageMedia)
	                {
                        case MessageMediaDocument mediaDocument:
	                        if ((mediaDocument.flags & MessageMediaDocument.Flags.has_document) is not 0 && mediaDocument.document is Document document)
	                        {
		                        await Client.DownloadFileAsync(document, localFileStream, null,
			                        CreateClientProgressCallback(tgDownloadSettings.SourceVm.SourceId, messageBase.ID, files[i].Join));
		                        TgEfDocumentEntity doc = new()
		                        {
			                        Id = document.ID,
			                        SourceId = tgDownloadSettings.SourceVm.SourceId,
			                        MessageId = messageBase.ID,
			                        FileName = fileName,
			                        FileSize = files[i].Size,
			                        AccessHash = accessHash
								};
		                        await DocumentRepository.SaveAsync(doc);
	                        }
							break;
                        case MessageMediaPhoto mediaPhoto:
	                        if (mediaPhoto is { photo: Photo photo })
	                        {
		                        //WClient.DownloadFileAsync(photo, localFileStream, new PhotoSize
		                        //    { h = photo.sizes[i].Height, w = photo.sizes[i].Width, size = photo.sizes[i].FileSize, type = photo.sizes[i].Type })
		                        await Client.DownloadFileAsync(photo, localFileStream, null,
			                        CreateClientProgressCallback(tgDownloadSettings.SourceVm.SourceId, messageBase.ID, string.Empty));
	                        }
							break;
	                }
				}
                localFileStream.Close();
            }
			// Store message.
			await MessageSaveAsync(tgDownloadSettings, messageBase.ID, files[i].DtCreate, files[i].Size, files[i].Remote, TgEnumMessageType.Document);
			//switch (messageMedia)
   //         {
			//	case MessageMediaDocument:
			//		await MessageRepository.SaveAsync(messageBase.ID, tgDownloadSettings.SourceVm.SourceId, files[i].DtCreate,
			//			TgEnumMessageType.Document, files[i].Size, files[i].Remote);
			//		break;
			//	case MessageMediaPhoto:
			//		if ((isExistsMessage && tgDownloadSettings.IsRewriteMessages) || !isExistsMessage)
			//		{
			//			await MessageRepository.SaveAsync(messageBase.ID, tgDownloadSettings.SourceVm.SourceId, files[i].DtCreate,
			//				TgEnumMessageType.Photo, files[i].Size, files[i].Remote);
			//		}
			//		break;
			//}
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

    private async Task MessageSaveAsync(TgDownloadSettingsViewModel tgDownloadSettings, int messageId, DateTime dtCreated, long size, string message,
	    TgEnumMessageType messageType)
    {
		//await UpdateStateFileAsync(tgDownloadSettings.SourceVm.SourceId, messageBase.ID, string.Empty, 0, 0, 0, false);
		//TgEfOperResult<TgEfMessageEntity> operResult = await MessageRepository.GetAsync(new TgEfMessageEntity
		// { Id = tgDownloadSettings.SourceVm.SourceFirstId, SourceId = tgDownloadSettings.SourceVm.SourceId }, isNoTracking: false);
		//if ((operResult.IsExists && tgDownloadSettings.IsRewriteMessages) || !operResult.IsExists)
		//{
		// await MessageRepository.SaveAsync(new TgEfMessageEntity
		// {
		//  Id = messageBase.ID,
		//  SourceId = tgDownloadSettings.SourceVm.SourceId,
		//  DtCreated = files[i].DtCreate,
		//  Type = TgEnumMessageType.Document,
		//  Size = files[i].Size,
		//  Message = files[i].Remote,
		// });
		//}
	    TgEfOperResult<TgEfMessageEntity> operResult = await MessageRepository.GetAsync(
		    new TgEfMessageEntity { SourceId = tgDownloadSettings.SourceVm.SourceId, Id = tgDownloadSettings.SourceVm.SourceFirstId }, isNoTracking: false);
	    if (!operResult.IsExists || (operResult.IsExists && tgDownloadSettings.IsRewriteMessages))
	    {
		    await MessageRepository.SaveAsync(new TgEfMessageEntity
		    {
			    Id = messageId,
			    SourceId = tgDownloadSettings.SourceVm.SourceId,
			    DtCreated = dtCreated,
			    Type = messageType,
			    Size = size,
			    Message = message,
		    });
	    }
	    if (messageType == TgEnumMessageType.Document)
		    await UpdateStateFileAsync(tgDownloadSettings.SourceVm.SourceId, messageId, string.Empty, 0, 0, 0, false);
	}


	private bool IsFileLocked(string filePath)
    {
	    bool isLocked = false;
	    FileStream? fileStream = null;
	    try
	    {
		    fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
	    }
	    catch (IOException)
	    {
		    isLocked = true;
	    }
	    finally
	    {
		    fileStream?.Close();
	    }
	    return isLocked;
    }

 //   private async Task CreateFileWatcher(long sourceId, int messageId, string fileName, long fileSize)
 //   {
	//    await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
	//	long previousSize = 0;
	//    double milliseconds = 1_000;
	//    bool isFileNewDownload = true;

	//	while (IsFileLocked(fileName))
	//    {
	//		await Task.Delay(TimeSpan.FromMilliseconds(milliseconds)).ConfigureAwait(false);
	//		long transmitted = new FileInfo(fileName).Length;
	//		long sizeDiff = transmitted - previousSize;
	//		long fileSpeed = sizeDiff > 0 ? (long)(sizeDiff / milliseconds * 1000) : 0;
	//		previousSize = transmitted;
	//		await UpdateStateFileAsync(sourceId, messageId, fileName, fileSize, transmitted, fileSpeed, isFileNewDownload);
	//		isFileNewDownload = false;
	//	}
	//}

    private Client.ProgressCallback CreateClientProgressCallback(long sourceId, int messageId, string fileName)
    {
        Stopwatch sw = Stopwatch.StartNew();
		bool isFileNewDownload = true;
		return (transmitted, size) =>
		{
			if (string.IsNullOrEmpty(fileName))
			{
				//transmitted = 0;
				//size = 0;
				isFileNewDownload = false;
			}
			else
			{
				long fileSpeed = transmitted <= 0 || sw.Elapsed.Seconds <= 0 ? 0 : transmitted / sw.Elapsed.Seconds;
				UpdateStateFileAsync(sourceId, messageId, Path.GetFileName(fileName), size, transmitted, fileSpeed, isFileNewDownload);
				isFileNewDownload = false;
			}
		};
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
            if (Me is null || !Me.IsActive)
				Me = Client.LoginUserIfNeeded().GetAwaiter().GetResult();
            UpdateStateSourceAsync(0, 0, string.Empty);
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
                TgEfAppEntity app = AppRepository.GetFirst(isNoTracking: false).Item;
                if (ProxyRepository.GetCurrentProxyUid(AppRepository.GetCurrentApp()) != app.ProxyUid)
                {
	                app.ProxyUid = ProxyRepository.GetCurrentProxyUid(AppRepository.GetCurrentApp());
	                AppRepository.Save(app);
                }
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
            await UpdateStateSourceAsync(0, 0, string.Empty);
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
                TgEfAppEntity app = (await AppRepository.GetFirstAsync(isNoTracking: false)).Item;
					app.ProxyUid = await ProxyRepository.GetCurrentProxyUidAsync(await AppRepository.GetCurrentAppAsync());
                await AppRepository.SaveAsync(app);
            }
            await AfterClientConnectAsync();
        }
    }

    public void Disconnect()
    {
        IsProxyUsage = false;
        UpdateStateSourceAsync(0, 0, string.Empty);
        UpdateStateProxyAsync(TgLocale.ProxyIsDisconnect);
        UpdateStateConnectAsync(TgLocale.MenuClientIsDisconnected);
        if (Client is null) return;
        Client.OnUpdates -= OnUpdatesClientAsync;
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

    private void SetClientExceptionShort(Exception ex)
    {
        ClientException.Set(ex);
        UpdateStateExceptionShortAsync(ClientException.Message).GetAwaiter().GetResult();
    }

    private async Task SetClientExceptionShortAsync(Exception ex)
    {
        ClientException.Set(ex);
        await UpdateStateExceptionShortAsync(ClientException.Message);
    }

    private async Task SetProxyExceptionAsync(Exception ex,
        [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
    {
        ProxyException.Set(ex);
        await UpdateStateExceptionAsync(filePath, lineNumber, memberName, ProxyException.Message);
    }

    #endregion

    #region Public and private methods

    private async Task TryCatchFuncAsync(Func<Task> actionAsync, bool isLoginConsole)
    {
        try
        {
            await actionAsync();
        }
        catch (Exception ex)
        {
            // CHANNEL_INVALID | BadMsgNotification 48
            if (ClientException.Message.Contains("You must connect to Telegram first"))
            {
	            await SetClientExceptionShortAsync(ex);
	            await UpdateStateMessageAsync("Reconnect client ...");
	            if (isLoginConsole)
		            LoginUserConsole(isProxyUpdate: false);
                else
					await LoginUserDesktopAsync(isProxyUpdate: false);
            }
            else
            {
	            if (!string.IsNullOrEmpty(ex.Source) && ex.Source.Equals("WTelegramClient"))
	            {
		            switch (ex.Message)
		            {
                        case "PEER_ID_INVALID":
	                        await UpdateStateExceptionShortAsync("The source is invalid!");
	                        break;
                        case "CHANNEL_PRIVATE":
	                        await UpdateStateExceptionShortAsync("The channel is private!");
	                        break;
                        case "CHANNEL_INVALID":
	                        await UpdateStateExceptionShortAsync("The channel is invalid!");
	                        break;
                        default:
	                        await SetClientExceptionShortAsync(ex);
							break;
					}
				}
                else
		            await SetClientExceptionAsync(ex);
            }
		}
    }

    private async Task<T> TryCatchFuncTAsync<T>(Func<Task<T>> actionAsync) where T : new()
    {
        try
        {
            return await actionAsync();
        }
        catch (Exception ex)
        {
            // CHANNEL_INVALID | BadMsgNotification 48
            if (ClientException.Message.Contains("You must connect to Telegram first"))
            {
	            await SetClientExceptionShortAsync(ex);
	            await UpdateStateMessageAsync("Reconnect client ...");
	            await LoginUserDesktopAsync();
            }
            else
            {
	            if (!string.IsNullOrEmpty(ex.Source) && ex.Source.Equals("WTelegramClient"))
	            {
		            switch (ex.Message)
		            {
                        case "PEER_ID_INVALID":
	                        await UpdateStateExceptionShortAsync("The source is invalid!");
	                        break;
                        case "CHANNEL_PRIVATE":
	                        await UpdateStateExceptionShortAsync("The channel is private!");
	                        break;
                        case "CHANNEL_INVALID":
	                        await UpdateStateExceptionShortAsync("The channel is invalid!");
	                        break;
                        default:
	                        await SetClientExceptionShortAsync(ex);
							break;
					}
				}
                else
		            await SetClientExceptionAsync(ex);
            }
            return new();
        }
    }

    private void TryCatchAction(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            // CHANNEL_INVALID | BadMsgNotification 48
            if (ClientException.Message.Contains("You must connect to Telegram first"))
            {
	            SetClientExceptionShortAsync(ex).GetAwaiter().GetResult();
	            UpdateStateMessageAsync("Reconnect client ...");
	            LoginUserDesktopAsync().GetAwaiter().GetResult();
            }
            else
            {
	            if (!string.IsNullOrEmpty(ex.Source) && ex.Source.Equals("WTelegramClient"))
	            {
		            switch (ex.Message)
		            {
                        case "PEER_ID_INVALID":
	                        UpdateStateExceptionShortAsync("The source is invalid!").GetAwaiter().GetResult();
							break;
                        case "CHANNEL_PRIVATE":
	                        UpdateStateExceptionShortAsync("The channel is private!").GetAwaiter().GetResult();
							break;
                        case "CHANNEL_INVALID":
	                        UpdateStateExceptionShortAsync("The channel is invalid!").GetAwaiter().GetResult();
							break;
                        default:
	                        SetClientExceptionShortAsync(ex).GetAwaiter().GetResult();
							break;
					}
				}
                else
		            SetClientExceptionAsync(ex).GetAwaiter().GetResult();
			}
		}
    }

    #endregion
}