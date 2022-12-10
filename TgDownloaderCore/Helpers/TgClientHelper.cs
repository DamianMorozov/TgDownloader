// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgDownloaderCore.Locales;
using TgDownloaderCore.Models;

namespace TgDownloaderCore.Helpers;

public class TgClientHelper
{
    #region Design pattern "Lazy Singleton"

    private static TgClientHelper _instance;
    public static TgClientHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    private LocaleHelper Locale => LocaleHelper.Instance;
    private LogHelper Log => LogHelper.Instance;
    public TgSettingsModel TgSettings { get; } = new();
    private Client _client;
    private Client WClient { get => _client; set { _client = value; IsExists = value is not null; } }
    public bool IsExists { get; private set; }
    public bool IsConnected => IsExists && !WClient.Disconnected;
    private User _mySelfUser;
    public User MySelfUser
    {
        get
        {
            if (_mySelfUser is not null)
                return _mySelfUser;
            _mySelfUser = IsConnected? WClient.LoginUserIfNeeded().ConfigureAwait(true).GetAwaiter().GetResult() : new();
            return _mySelfUser;
        }
    }

    private Dictionary<long, ChatBase> _dicChatsAll;
    public Dictionary<long, ChatBase> DicChatsAll
    {
        get => _dicChatsAll;
        private set
        {
            _dicChatsAll = value;
            DtDicChatsAll = DateTime.Now;
        }
    }
    public DateTime DtDicChatsAll { get; private set; }
    public Dictionary<long, ChatBase> DicChatsUpdated { get; }
    public Dictionary<long, ChatBase> DicDialogsAll { get; }
    public Dictionary<long, User> DicUsersUpdated { get; }
    public List<Channel> ListChannels { get; }
    public List<Channel> ListGroups { get; }
    public List<ChatBase> ListSmallGroups { get; }
    public List<KeyValuePair<long, long>> HashesChannels { get; private set; }
    public List<KeyValuePair<long, long>> HashesUsers { get; private set; }
    
    public TgClientHelper()
    {
        DicChatsAll = new();
        DicChatsUpdated = new();
        DicDialogsAll = new();
        DicUsersUpdated = new();
        HashesChannels = new();
        HashesUsers = new();
        ListChannels = new();
        ListGroups = new();
        ListSmallGroups = new();

        // Log to VS Output debugging pane in addition.
        WTelegram.Helpers.Log += (_, str) => Debug.WriteLine(str);
        // Disable logging.
        WTelegram.Helpers.Log = (_, _) => { };
    }

    #endregion

    #region Public and private methods

    public string GetConfig(string what)
    {
        return what switch
        {
            "api_id" => Log.AskString(Locale.Info.TgSetupAppId),
            "api_hash" => Log.AskString(Locale.Info.TgSetupApiHash),
            "phone_number" => Log.AskString(Locale.Info.TgSetupPhone),
            "verification_code" => Log.AskString(Locale.Info.TgSetupCode),
            "notifications" => Log.AskBool(Locale.Info.TgSetupNotifications).ToString(),
            "first_name" => Log.AskString(Locale.Info.TgSetupFirstName),
            "last_name" => Log.AskString(Locale.Info.TgSetupLastName),
            "session_pathname" => "TgDownloader.session",
            "password" => Log.AskString(Locale.Info.TgSetupPassword),
            _ => null
        };
        // if enabled 2FA
        //case "password":
        //    BeginInvoke(new Action(() => CodeNeeded(what.Replace('_', ' '))));
        //    _codeReady.Reset();
        //    _codeReady.Wait();
        //    return textBoxCode.Text;
    }

    public void Connect()
    {
        if (WClient is not null && IsConnected) return;
        if (WClient is not null)
        {
            WClient.OnUpdate -= Client_OnUpdate;
            WClient.Dispose();
        }

        WClient = new(GetConfig);
        WClient.OnUpdate += Client_OnUpdate;
        Log.MarkupLineStamp("Setup the TG Client was complete");

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
        if (IsConnected)
        {
            Messages_Chats messages = await WClient.Messages_GetAllChats().ConfigureAwait(true);
            DicChatsAll = messages.chats;
        }
        FillListChats(DicChatsAll);
    }

    private void FillListChats(Dictionary<long, ChatBase> dic)
    {
        ListSmallGroups.Clear();
        ListGroups.Clear();
        ListChannels.Clear();

        foreach (KeyValuePair<long, ChatBase> item in dic)
        {
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

    private void FillListDialogs(Dictionary<long, ChatBase> dic)
    {
        foreach (KeyValuePair<long, ChatBase> item in dic)
        {
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

        try
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
                        await Client_DisplayMessage(null);
                        break; // there are much more update types than the above example cases
                }
            }
        }
        catch (Exception ex)
        {
            Log.MarkupLineStamp(ex.Message);
            if (ex.InnerException is not null)
                Log.MarkupLineStamp(ex.InnerException.Message);
        }
    }

    private Task Client_DisplayMessage(MessageBase messageBase, bool edit = false)
    {
        if (messageBase is null) return Task.CompletedTask;
        if (edit) Console.Write("(Edit): ");
        switch (messageBase)
        {
            case Message message:
                Log.MarkupLineStamp($"{GetPeerUpdatedName(message.from_id) ?? message.post_author} in {GetPeerUpdatedName(message.peer_id)}> {message.message}");
                break;
            case MessageService messageService:
                Log.MarkupLineStamp($"{GetPeerUpdatedName(messageService.from_id)} in {GetPeerUpdatedName(messageService.peer_id)} [{messageService.action.GetType().Name[13..]}]");
                break;
        }
        return Task.CompletedTask;
    }

    public async Task PrintSendAsync(Messages_Chats messagesChats)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        Console.Write("Type a chat ID to send a message: ");
        string input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
        {
            long chatId = long.Parse(input);
            ChatBase target = messagesChats.chats[chatId];
            Log.MarkupLineStamp($"Type the message into the chat: {target.Title} | {chatId}");
            input = Console.ReadLine();
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

    public void PrintChatsInfo(List<ChatBase> chats, string name, bool isSort)
    {
        if (isSort)
            chats = SortListChats(chats);
        Log.MarkupLineStamp($"Found {name}: {chats.Count}");
    }

    public void PrintChannelsInfo(List<Channel> channels, string name, bool isSort)
    {
        if (isSort)
            channels = SortListChannels(channels);
        Log.MarkupLineStamp($"Found {name}: {channels.Count}");
    }

    public void PrintChatFullBaseInfo(ChatFullBase chatFull)
    {
        if (chatFull is null) return;

        if (chatFull.BotInfo is not null)
            foreach (BotInfo botInfo in chatFull.BotInfo)
            {
                //ConsoleUtils.MarkupLine(botInfo.commands);
                //ConsoleUtils.MarkupLine(botInfo.description);
                //ConsoleUtils.MarkupLine(botInfo.menu_button);
            }

        if (chatFull.RecentRequesters is not null)
            foreach (long requester in chatFull.RecentRequesters)
            {
                Log.MarkupLineStamp($"{nameof(requester)}: {requester}");
            }

        Log.MarkupLineStamp($"{nameof(chatFull.RequestsPending)}: {chatFull.RequestsPending}");

        Log.MarkupLineStamp(
            $"{nameof(chatFull.About)}: {chatFull.About} | " +
            $"{nameof(chatFull.ID)}: {chatFull.ID} | " +
            $"{nameof(chatFull.GetType)}: {chatFull.GetType()} | " +
            $"{nameof(chatFull.TtlPeriod)}: {chatFull.TtlPeriod} | ");
    }

    public void PrintChannelFullInfo(ChannelFull channelFull)
    {
        if (channelFull is null) return;

        if (channelFull.BotInfo is not null)
            foreach (BotInfo botInfo in channelFull.BotInfo)
            {
                //ConsoleUtils.MarkupLine(botInfo.commands);
                //ConsoleUtils.MarkupLine(botInfo.description);
                //ConsoleUtils.MarkupLine(botInfo.menu_button);
            }

        if (channelFull.RecentRequesters is not null)
            foreach (long requester in channelFull.RecentRequesters)
            {
                Log.MarkupLineStamp($"{nameof(requester)}: {requester}");
            }

        Log.MarkupLineStamp($"{nameof(channelFull.RequestsPending)}: {channelFull.RequestsPending}");
        Log.MarkupLineStamp($"{nameof(channelFull.read_inbox_max_id)}: {channelFull.read_inbox_max_id}");

        Log.MarkupLineStamp(
            $"{nameof(channelFull.About)}: {channelFull.About} | " +
            $"{nameof(channelFull.ID)}: {channelFull.ID} | " +
            $"{nameof(channelFull.GetType)}: {channelFull.GetType()} | " +
            $"{nameof(channelFull.TtlPeriod)}: {channelFull.TtlPeriod} | ");
    }

    public int GetChannelMessagesCount(ChatFullBase chatFullBase) => 
        chatFullBase is not ChannelFull channelFull ? 0 : channelFull.read_inbox_max_id;

    public Channel PrepareCollectMessages()
    {
        Channel channel = GetChannel(TgSettings.SourceUserName);
        if (channel.id is 0) return channel;

        Messages_ChatFull fullChannel = WClient.Channels_GetFullChannel(channel).ConfigureAwait(true).GetAwaiter().GetResult();
        if (fullChannel.full_chat is ChannelFull channelFull)
            PrintChannelFullInfo(channelFull);
        else
            PrintChatFullBaseInfo(fullChannel.full_chat);
        TgSettings.SetMessageMaxCount(GetChannelMessagesCount(fullChannel.full_chat));
        
        return channel;
    }

    public async Task CollectMessages(Channel channel)
    {
        if (channel.id is 0) return;
        try
        {
            int lastId = TgSettings.MessageCount < 1 ? TgSettings.MessageStartId + 1 : TgSettings.MessageStartId + TgSettings.MessageCount;

            while (TgSettings.MessageStartId < lastId && TgSettings.MessageStartId <= TgSettings.MessageMaxCount)
            {
                _ = MySelfUser;
                Messages_MessagesBase messages =
                    await WClient.Channels_GetMessages(channel, TgSettings.MessageStartId).ConfigureAwait(true);

                foreach (MessageBase message in messages.Messages)
                {
                    // It could be: "(no message)".
                    Log.MarkupLineStamp($"Read the message {message.ID} | {message}");
                    //string fromId = message.From is not null ? message.From.ID.ToString() : string.Empty;
                    //string replyToId = message.ReplyTo is not null ? message.ReplyTo.reply_to_msg_id.ToString() : string.Empty;
                    await DownloadFile(message, TgSettings.DestDirectory);
                }
                TgSettings.AddMessageStartId();
                if (TgSettings.MessageCount < 1)
                    lastId = TgSettings.MessageStartId + 1;
            }
        }
        catch (Exception ex)
        {
            Log.MarkupLineStamp(ex.Message);
            if (ex.InnerException is not null)
                Log.MarkupLineStamp(ex.InnerException.Message);
        }
    }

    private async Task DownloadFile(MessageBase messageBase, string folder)
    {
        try
        {
            if (messageBase is null) return;
            if (messageBase is not Message { media: MessageMediaDocument { document: Document document } }) return;

            string fileName = Path.Combine(folder, document.Filename);
            if (!CheckDownloadFileProcess(fileName, document)) return;
            DeleteFileIfExists(fileName);

            // DownloadFile v1.
            //await File.WriteAllTextAsync(path, content);

            // DownloadFile v2.
            //await using MemoryStream memoryStream = new();
            //await WClient.DownloadFileAsync(document, memoryStream);
            //byte[] bytes = memoryStream.ToArray();
            //await using FileStream fileStream = File.Create(fileName);
            //await fileStream.WriteAsync(bytes);
            //fileStream.Close();
            //memoryStream.Close();

            // DownloadFile v3.
            await using FileStream fileStream = File.Create(fileName);
            await WClient.DownloadFileAsync(document, fileStream);
            fileStream.Close();
        }
        catch (Exception ex)
        {
            string messageId = messageBase is not null ? messageBase.ID.ToString() : string.Empty;
            Log.MarkupLineStamp($"Exception at the message ID: {messageId}");
            Log.MarkupLineStamp(ex.Message);
            if (ex.InnerException is not null)
                Log.MarkupLineStamp(ex.InnerException.Message);
        }
    }

    private bool CheckDownloadFileProcess(string fileName, Document document)
    {
        bool result = true;
        if (File.Exists(fileName))
        {
            Log.MarkupLineStamp($"File {Path.GetFileName(fileName)} is exists");
            result = false;
            using FileStream fileStream = File.OpenRead(fileName);
            if (fileStream.Length < document.size)
            {
                Log.MarkupLineStamp($"File {Path.GetFileName(fileName)} have {fileStream.Length} size, but document have {document.size} size. It will be re-download");
                result = true;
            }
            fileStream.Close();
        }
        return result;
    }

    private void DeleteFileIfExists(string fileName)
    {
        if (!File.Exists(fileName)) return;
        File.Delete(fileName);
        Log.MarkupLineStamp($"File {Path.GetFileName(fileName)} was deleted");
    }

    private void PrintAccessHashInfo(Channel channel, string userName)
    {
        long accessHash = WClient.GetAccessHashFor<Channel>(channel.ID);
        if (accessHash is not 0)
        {
            Log.MarkupLineStamp(
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
            Log.MarkupLineStamp("Resolving channel to get its ID, access hash and other infos...");
            Contacts_ResolvedPeer contactsResolved =
                WClient.Contacts_ResolveUsername(userName).ConfigureAwait(true).GetAwaiter()
                    .GetResult();
            if (contactsResolved.peer.ID != channel.ID)
                throw new("has changed channel ID ?!");
            // should have been collected from the previous API result
            accessHash = WClient.GetAccessHashFor<Channel>(channel.ID);
            if (accessHash is 0)
                throw new("No access hash was automatically collected !? (shouldn't happen)");
            Log.MarkupLineStamp(
                $"Channel has ID {channel.ID} and access hash was automatically collected: {accessHash:X}");
        }

        Log.MarkupLineStamp("With the access hash, we can now join the channel for example.");
        WClient.Channels_JoinChannel(new InputChannel(channel.ID, accessHash)).ConfigureAwait(true).GetAwaiter().GetResult();

        Log.MarkupLineStamp("Channel joined. Press any key to save and exit");
        Console.ReadKey(true);

        Log.MarkupLineStamp("Saving all collected access hashes to disk for next run...");
        HashesChannels = WClient.AllAccessHashesFor<Channel>().ToList();
        HashesUsers = WClient.AllAccessHashesFor<User>().ToList();
        
        //string StateFilename = "SavedState.json";
        //using FileStream stateStream = File.Create(StateFilename);
        //JsonSerializer.Serialize(stateStream, this);
    }

    #endregion
}
