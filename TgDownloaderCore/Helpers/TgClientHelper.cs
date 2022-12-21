// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Channel = TL.Channel;
using Document = TL.Document;

namespace TgDownloaderCore.Helpers;

public partial class TgClientHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgClientHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgClientHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    private TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
    private TgLogHelper TgLog => TgLogHelper.Instance;
    public TgDownloadHelper TgDownload => TgDownloadHelper.Instance;
    private Client? _client;
    public Client? WClient { get => _client; set { _client = value; IsExists = value is not null; } }
    public bool IsExists { get; private set; }
    public bool IsReady => WClient is { } && IsExists && !WClient.Disconnected;
    private User? _mySelfUser;
    public User MySelfUser
    {
        get
        {
            if (_mySelfUser is not null)
                return _mySelfUser;
            _mySelfUser = IsReady ? WClient?.LoginUserIfNeeded().ConfigureAwait(true).GetAwaiter().GetResult() : new();
            return _mySelfUser ?? new();
        }
    }

    public Dictionary<long, ChatBase> DicChatsAll { get; private set; }
    public Dictionary<long, ChatBase> DicChatsUpdated { get; }
    public Dictionary<long, User> DicUsersUpdated { get; }
    public List<Channel> ListChannels { get; }
    public List<Channel> ListGroups { get; }
    public List<ChatBase> ListChats { get; }
    public List<ChatBase> ListSmallGroups { get; }
    public List<KeyValuePair<long, long>> HashesChannels { get; private set; }
    public List<KeyValuePair<long, long>> HashesUsers { get; private set; }
    public string ApiId { get; private set; }
    public string ApiHash { get; private set; }
    public string PhoneNumber { get; private set; }

    public TgClientHelper()
    {
        DicChatsAll = new();
        DicChatsUpdated = new();
        DicUsersUpdated = new();
        HashesChannels = new();
        HashesUsers = new();
        ListChats = new();
        ListChannels = new();
        ListGroups = new();
        ListSmallGroups = new();
        ApiId = string.Empty;
        ApiHash = string.Empty;
        PhoneNumber = string.Empty;

        // TgLog to VS Output debugging pane in addition.
        //WTelegram.Helpers.Log += (_, str) => Debug.WriteLine(str);
        // Disable logging.
        WTelegram.Helpers.Log = (_, _) => { };

        //Database.InitialiseConnection();
    }

    #endregion

    #region Public and private methods

    public string? GetConfig(string what) =>
        what switch
        {
            "api_id" => ApiId = TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TgSetupAppId)),
            "api_hash" => ApiHash = TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TgSetupApiHash)),
            "phone_number" => PhoneNumber = TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TgSetupPhone)),
            "verification_code" => TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TgSetupCode)),
            "notifications" => TgLog.AskBool(TgLog.GetLineStampInfo(TgLocale.TgSetupNotifications)).ToString(),
            "first_name" => TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TgSetupFirstName)),
            "last_name" => TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TgSetupLastName)),
            "session_pathname" => "TgDownloader.session",
            "password" => TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TgSetupPassword)),
            _ => null
        };

    public string? GetExistsConfig(string what) =>
        what switch
        {
            "api_id" => ApiId = TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TgSetupAppId)),
            "api_hash" => ApiHash,
            "phone_number" => PhoneNumber,
            "verification_code" => TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TgSetupCode)),
            "notifications" => TgLog.AskBool(TgLog.GetLineStampInfo(TgLocale.TgSetupNotifications)).ToString(),
            "first_name" => TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TgSetupFirstName)),
            "last_name" => TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TgSetupLastName)),
            "session_pathname" => "TgDownloader.session",
            "password" => TgLog.AskString(TgLog.GetLineStampInfo(TgLocale.TgSetupPassword)),
            _ => null
        };

    public void Connect(string apiHash, string phoneNumber)
    {
        if (WClient is not null && IsReady) return;
        if (WClient is not null)
        {
            WClient.OnUpdate -= Client_OnUpdate;
            WClient.Dispose();
        }

        if (!string.IsNullOrEmpty(apiHash) && !string.IsNullOrEmpty(phoneNumber))
        {
            ApiHash = apiHash;
            PhoneNumber = phoneNumber;
        }
        WClient = !string.IsNullOrEmpty(apiHash) && !string.IsNullOrEmpty(phoneNumber)
            ? new(GetExistsConfig)
            : new(GetConfig);
        WClient.OnUpdate += Client_OnUpdate;

        _ = MySelfUser;
    }

    public long ReduceChatId(long chatId) =>
        !$"{chatId}".StartsWith("-100") ? chatId : Convert.ToInt64($"{chatId}"[4..]);

    public long FixChatId(long chatId) =>
        $"{chatId}".StartsWith("-100") ? chatId : Convert.ToInt64($"-100{chatId}");

    public User GetUserUpdated(long id) => DicUsersUpdated.TryGetValue(ReduceChatId(id), out User user) ? user : new();

    public string GetUserUpdatedName(long id) => DicUsersUpdated.TryGetValue(ReduceChatId(id), out User user) ? user.username : string.Empty;

    public ChatBase GetChat(long id) => DicChatsAll.TryGetValue(ReduceChatId(id), out ChatBase chat) ? chat : new Chat();

    public ChatBase GetChatUpdated(long id) => DicChatsUpdated.TryGetValue(ReduceChatId(id), out ChatBase chat) ? chat : new Chat();

    public Channel GetChannel(long id) => DicChatsAll.TryGetValue(ReduceChatId(id), out ChatBase chat) ? chat as Channel ?? new() : new();

    public Channel GetChannel(string userName)
    {
        foreach (KeyValuePair<long, ChatBase> chatBase in DicChatsAll)
        {
            if (chatBase.Value is Channel channel)
            {
                if (Equals(channel.username, userName))
                    return channel;
            }
        }
        return new();
    }

    public Channel GetChannelUpdated(long id) => DicChatsUpdated.TryGetValue(ReduceChatId(id), out ChatBase chat) ? chat as Channel ?? new() : new();

    public string GetChatName(long id) => DicChatsAll.TryGetValue(ReduceChatId(id), out ChatBase chat) ? chat.ToString() : string.Empty;

    public string GetChatUpdatedName(long id) => DicChatsUpdated.TryGetValue(ReduceChatId(id), out ChatBase chat) ? chat.ToString() : string.Empty;

    public string GetPeerUpdatedName(Peer peer) => peer is null ? null : peer is PeerUser user ? GetUserUpdatedName(user.user_id)
        : peer is PeerChat or PeerChannel ? GetChatUpdatedName(peer.ID) : $"Peer {peer.ID}";

    public async Task CollectAllChats()
    {
        DicChatsAll = new();
        if (IsReady)
        {
            Messages_Chats messages = await WClient.Messages_GetAllChats().ConfigureAwait(true);
            DicChatsAll = messages.chats;
        }
        FillListChats(DicChatsAll);
    }

    public Dictionary<long, ChatBase> CollectAllDialogs()
    {
        switch (IsReady)
        {
            case true when WClient is { }:
            {
                Messages_Dialogs messages = WClient.Messages_GetAllDialogs()
                    .ConfigureAwait(true).GetAwaiter().GetResult();
                return messages.chats;
            }
            default:
                return new();
        }
    }

    private void FillListChats(Dictionary<long, ChatBase> dic)
    {
        ListChannels.Clear();
        ListChats.Clear();
        ListGroups.Clear();
        ListSmallGroups.Clear();

        foreach (KeyValuePair<long, ChatBase> item in dic)
        {
            ListChats.Add(item.Value);
            switch (item.Value)
            {
                case Chat smallGroup when (smallGroup.flags & Chat.Flags.deactivated) is 0:
                    ListSmallGroups.Add(item.Value);
                    break;
                case Channel { IsGroup: true } group:
                    //case Channel group: // no broadcast flag => it's a big group, also called superGroup or megaGroup
                    ListGroups.Add(group);
                    break;
                case Channel channel:
                    //case Channel channel when (channel.flags & Channel.Flags.broadcast) is not 0:
                    ListChannels.Add(channel);
                    break;
            }
        }
    }

    private async Task Client_OnUpdate(IObject arg)
    {
        if (arg is not UpdatesBase updates) return;
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

        TryCatchAction(() =>
        {
            updates.CollectUsersChats(DicUsersUpdated, DicChatsUpdated);
            foreach (Update update in updates.UpdateList)
            {
                switch (update)
                {
                    //    case UpdateNewMessage updateNewMessage:
                    //        //ConsoleUtils.MarkupLine($"Set {nameof(LastMessageId)}: {updateNewMessage.message.ID}");
                    //        //await Client_DisplayMessage(unm.message);
                    //        break;
                    //    case UpdateEditMessage uem:
                    //        //await Client_DisplayMessage(uem.message, true); 
                    //        break;
                    //    // Note: UpdateNewChannelMessage and UpdateEditChannelMessage are also handled by above cases
                    //    case UpdateDeleteChannelMessages updateDeleteChannelMessages:
                    //        ConsoleUtils.MarkupLine(
                    //            $"{updateDeleteChannelMessages.messages.Length} message(s) deleted in {GetChatUpdatedName(updateDeleteChannelMessages.channel_id)}");
                    //        break;
                    //    case UpdateDeleteMessages udm:
                    //        ConsoleUtils.MarkupLine($"{udm.messages.Length} message(s) deleted");
                    //        break;
                    //    case UpdateUserTyping uut:
                    //        ConsoleUtils.MarkupLine($"{GetUserUpdated(uut.user_id)} is {uut.action}");
                    //        break;
                    //    case UpdateChatUserTyping ucut:
                    //        ConsoleUtils.MarkupLine($"{GetPeerUpdatedName(ucut.from_id)} is {ucut.action} in {GetChatUpdatedName(ucut.chat_id)}");
                    //        break;
                    //    case UpdateChannel updateChannel:
                    //        ConsoleUtils.MarkupLine($"{GetChatUpdatedName(updateChannel.channel_id)}");
                    //        break;
                    //    case UpdateChannelReadMessagesContents updateChannelReadMessagesContents:
                    //        ConsoleUtils.MarkupLine($"{GetChatUpdatedName(updateChannelReadMessagesContents.channel_id)}");
                    //        ConsoleUtils.MarkupLine($"{updateChannelReadMessagesContents.channel_id}");
                    //        //LastMessageId = updateChannelReadMessagesContents.messages.Length;
                    //        ConsoleUtils.MarkupLine($"{nameof(updateChannelReadMessagesContents)}: {updateChannelReadMessagesContents.messages.Length}");
                    //        break;
                    //    case UpdateChannelUserTyping ucut2:
                    //        ConsoleUtils.MarkupLine($"{GetPeerUpdatedName(ucut2.from_id)} is {ucut2.action} in {GetChatUpdatedName(ucut2.channel_id)}");
                    //        break;
                    //    case UpdateChatParticipants { participants: ChatParticipants cp }:
                    //        ConsoleUtils.MarkupLine($"{cp.participants.Length} participants in {GetChatUpdatedName(cp.chat_id)}");
                    //        break;
                    //    case UpdateUserStatus uus:
                    //        ConsoleUtils.MarkupLine($"{GetUserUpdatedName(uus.user_id)} is now {uus.status.GetType().Name[10..]}");
                    //        break;
                    //    case UpdateUserName uun:
                    //        ConsoleUtils.MarkupLine($"{GetUserUpdatedName(uun.user_id)} has changed profile name: {uun.first_name} {uun.last_name}");
                    //        break;
                    //    case UpdateUserPhoto uup:
                    //        ConsoleUtils.MarkupLine($"{GetUserUpdatedName(uup.user_id)} has changed profile photo");
                    //        break;
                    //    case UpdateMessagePoll ump:
                    //        //ConsoleUtils.MarkupLine(update.GetType().Name);
                    //        break;
                    default:
                        //ConsoleUtils.MarkupLine(update.GetType().Name);
                        Client_DisplayMessage(null);
                        break; // there are much more update types than the above example cases
                }
            }
        });
    }

    private void Client_DisplayMessage(MessageBase messageBase, bool edit = false)
    {
        if (messageBase is null) return;
        if (edit) Console.Write("(Edit): ");
        switch (messageBase)
        {
            case Message message:
                TgLog.Line($"{GetPeerUpdatedName(message.from_id) ?? message.post_author} in {GetPeerUpdatedName(message.peer_id)}> {message.message}");
                break;
            case MessageService messageService:
                TgLog.Line($"{GetPeerUpdatedName(messageService.from_id)} in {GetPeerUpdatedName(messageService.peer_id)} [{messageService.action.GetType().Name[13..]}]");
                break;
        }
    }

    public async Task PrintSendAsync(Messages_Chats messagesChats)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        Console.Write("Type a chat ID to send a message: ");
        string? input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
        {
            long chatId = long.Parse(input);
            ChatBase target = messagesChats.chats[chatId];
            TgLog.Line($"Type the message into the chat: {target.Title} | {chatId}");
            input = Console.ReadLine();
            if (WClient is { })
                await WClient.SendMessageAsync(target, input);
        }
    }

    public List<ChatBase> SortListChats(List<ChatBase> chats)
    {
        List<ChatBase> result = new();
        if (chats.Count is 0) return chats;
        List<ChatBase> chatsOrders = chats.OrderBy(x => x.Title).ToList();
        foreach (ChatBase chatOrder in chatsOrders)
        {
            ChatBase chatNew = chats.First(x =>
                Equals(x.Title, chatOrder.Title));
            if (chatNew.ID is not 0)
                result.Add(chatNew);
        }
        return result;
    }

    public List<Channel> SortListChannels(List<Channel> channels)
    {
        List<Channel> result = new();
        if (channels.Count is 0) return channels;
        List<Channel> channelsOrders = channels.OrderBy(x => x.username).ToList();
        foreach (Channel chatOrder in channelsOrders)
        {
            Channel chatNew = channels.First(x => Equals(x.Title, chatOrder.Title));
            if (chatNew.ID is not 0)
                result.Add(chatNew);
        }
        return result;
    }

    public void PrintChatsInfo(Dictionary<long, ChatBase> dicChats, string name)
    {
        TgLog.Info($"Found {name}: {dicChats.Count}");
        TgLog.Info(TgLocale.TgGetDialogsInfo);
        foreach (KeyValuePair<long, ChatBase> dicChat in dicChats)
        {
            TryCatchAction(() =>
            {
                switch (dicChat.Value)
                {
                    case Channel channel:
                        PrintChatsInfoChannel(channel, false);
                        break;
                    default:
                        TgLog.Line(GetChatInfo(dicChat.Value));
                        break;
                }
            });
        }
    }

    private void PrintChatsInfoChannel(Channel channel, bool isFull)
    {
        Messages_ChatFull fullChannel = WClient.Channels_GetFullChannel(channel).ConfigureAwait(true).GetAwaiter().GetResult();
        if (fullChannel.full_chat is ChannelFull channelFull)
            TgLog.Line(GetChannelFullInfo(channelFull, channel, isFull));
        else
            TgLog.Line(GetChatFullBaseInfo(fullChannel.full_chat, channel, isFull));
    }

    public string GetChatInfo(ChatBase chat)
    {
        if (chat is null) return string.Empty;
        return
            $"{nameof(chat.ID)}: {chat.ID} | " +
            $"{nameof(chat.IsActive)}: {chat.IsActive} | " +
            $"{nameof(chat.IsBanned)}: {chat.IsBanned()} | " +
            $"{nameof(chat.Title)}: {chat.Title} | " +
            $"{nameof(chat.GetType)}: {chat.GetType()}";
    }

    public string GetChannelFullInfo(ChannelFull channelFull, ChatBase chat, bool isFull)
    {
        if (channelFull is null) return string.Empty;
        string result = GetChatInfo(chat);
        if (isFull)
            result += " | " + Environment.NewLine + channelFull.About;
        return result;
    }

    public string GetChatFullBaseInfo(ChatFullBase chatFull, ChatBase chat, bool isFull)
    {
        if (chatFull is null) return string.Empty;
        string result = GetChatInfo(chat);
        if (isFull)
            result += " | " + Environment.NewLine + chatFull.About;
        return result;
    }

    public int GetChannelMessagesCount(Channel channel)
    {
        Messages_ChatFull fullChannel = WClient.Channels_GetFullChannel(channel).ConfigureAwait(true).GetAwaiter().GetResult();
        return fullChannel.full_chat is ChannelFull channelFull ? channelFull.read_inbox_max_id : 0;
    }

    public Channel? PrepareDownloadMessages(bool isSilent)
    {
        Channel? channel = null;
        TryCatchAction(() =>
        {
            channel = GetChannel(TgDownload.SourceUserName);
            if (channel.id is 0) return;
            if (!isSilent)
                PrintChatsInfoChannel(channel, true);
            TgDownload.SetMessageCount(GetChannelMessagesCount(channel));
        });
        TgDownload.SetSourceId(channel?.id);
        return channel;
    }

    public void DownloadMessages(Action<string> refreshStatus,
        Action<long?, long?, string, string, long, long> storeMessage, 
        Func<long?, long?, bool> findExistsMessage)
    {
        Channel? channel = PrepareDownloadMessages(false);
        if (channel?.id is 0) return;
        TryCatchAction(() =>
        {
            _ = MySelfUser;
            while (TgDownload.MessageCurrentId <= TgDownload.MessageCount)
            {
                if (findExistsMessage(TgDownload.MessageCurrentId, TgDownload.SourceId))
                {
                    refreshStatus($"Read the message {TgDownload.MessageCurrentId} | was skipped by storage");
                }
                else
                {
                    Messages_MessagesBase messages =
                        WClient.Channels_GetMessages(channel, TgDownload.MessageCurrentId).ConfigureAwait(true).GetAwaiter().GetResult();
                    foreach (MessageBase message in messages.Messages)
                    {
                        // It could be: "(no message)"
                        DownloadFile(message, refreshStatus, storeMessage);
                    }
                }
                TgDownload.AddMessageCurrentId();
            }
            TgDownload.SetMessageCurrentIdDefault();
        }, refreshStatus);
    }

    private void DownloadFile(MessageBase messageBase, Action<string> refreshStatus,
        Action<long?, long?, string, string, long, long> storeMessage)
    {
        TryCatchAction(() =>
        {
            refreshStatus($"Read the message {messageBase.ID}");

            if (messageBase is not Message { media: MessageMediaDocument { document: Document document } })
            {
                storeMessage(messageBase.ID, TgDownload.SourceId, 
                    messageBase.ToString() ?? string.Empty, string.Empty, 0, 0);
                refreshStatus($"Read the message {messageBase.ID} | was complete");
                return;
            }

            string fileName = Path.Combine(TgDownload.DestDirectory, document.Filename);
            // Delete file.
            if (TgDownload.IsRewriteFiles)
            {
                (bool IsNeed, long Size) fileToDelete = IsFileNeedDelete(fileName, document, refreshStatus);
                if (fileToDelete.IsNeed)
                    DeleteFile(fileName, fileToDelete.Size, refreshStatus);
            }
            // Create & download file.
            refreshStatus($"Read the message {messageBase.ID} | " +
                $"{document.Filename} | size {FileUtils.GetFileSizeString(document.size)}");
            // Download file.
            if (TgDownload.IsRewriteFiles || !File.Exists(fileName))
            {
                using FileStream fileStream = File.Create(fileName);
                WClient?.DownloadFileAsync(document, fileStream).ConfigureAwait(true).GetAwaiter().GetResult();
                fileStream.Close();
            }
            // Callback.
            refreshStatus($"Read the message {messageBase.ID} | {fileName} | was complete");
            storeMessage(messageBase.ID, TgDownload.SourceId, 
                messageBase.ToString() ?? string.Empty, 
                document.Filename, document.size, document.access_hash);
        }, refreshStatus);
    }

    private (bool, long) IsFileNeedDelete(string fileName, Document document, Action<string> refreshStatus)
    {
        bool result = false;
        long size = 0;
        TryCatchAction(() =>
        {
            if (File.Exists(fileName))
            {
                using FileStream fileStream = File.OpenRead(fileName);
                size = fileStream.Length;
                result = fileStream.Length < document.size;
                fileStream.Close();
            }
        }, refreshStatus);
        return (result, size);
    }

    private void DeleteFile(string fileName, long size, Action<string> refreshStatus)
    {
        if (!File.Exists(fileName)) return;
        TryCatchAction(() =>
        {
            refreshStatus($"Delete | {fileName} | size {FileUtils.GetFileSizeString(size)} | in progress");
            File.Delete(fileName);
            refreshStatus($"Delete | {fileName} | {FileUtils.GetFileSizeString(size)} | was complete");
        }, refreshStatus);
    }

    private void PrintAccessHashInfo(Channel channel, string userName)
    {
        if (WClient is not { }) return;
        long accessHash = WClient.GetAccessHashFor<Channel>(channel.ID);
        if (accessHash is not 0)
        {
            TgLog.Line(
                $"Channel has ID {channel.ID} and access hash was already collected: {accessHash:X}");
        }
        else
        {
            // Zero means the access hash for Channel was not collected yet.
            // So we need to obtain it through Client API calls whose results contains the access_hash field, such as:
            // - Messages_GetAllChats   (see Program_GetAllChats.cs   for an example on how to use it)
            // - Messages_GetAllDialogs (see Program_ListenUpdates.cs for an example on how to use it)
            // - Contacts_ResolveUsername                (see below for an example on how to use it)
            // and many more API methods...
            // The access_hash fields can be found inside instance of User, Channel, Photo, Document, etc..
            // usually listed through their base class UserBase, ChatBase, PhotoBase, DocumentBase, etc...
            TgLog.Line("Resolving channel to get its ID, access hash and other infos...");
            Contacts_ResolvedPeer contactsResolved =
                WClient.Contacts_ResolveUsername(userName).ConfigureAwait(true).GetAwaiter()
                    .GetResult();
            if (contactsResolved.peer.ID != channel.ID)
                throw new("has changed channel ID ?!");
            // should have been collected from the previous API result
            accessHash = WClient.GetAccessHashFor<Channel>(channel.ID);
            if (accessHash is 0)
                throw new("No access hash was automatically collected !? (shouldn't happen)");
            TgLog.Line(
                $"Channel has ID {channel.ID} and access hash was automatically collected: {accessHash:X}");
        }

        TgLog.Line("With the access hash, we can now join the channel for example.");
        WClient.Channels_JoinChannel(new InputChannel(channel.ID, accessHash)).ConfigureAwait(true).GetAwaiter().GetResult();

        TgLog.Line("Channel joined. Press any key to save and exit");
        Console.ReadKey(true);

        TgLog.Line("Saving all collected access hashes to disk for next run...");
        HashesChannels = WClient.AllAccessHashesFor<Channel>().ToList();
        HashesUsers = WClient.AllAccessHashesFor<User>().ToList();
    }

    #endregion
}
