// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using ProxyLib.Proxy;
using System.Runtime.Serialization;
using TgCore.Enums;
using TgCore.Helpers;
using TgCore.Interfaces;
using TgCore.Localization;
using TgCore.Models;
using TgCore.Utils;
using TgStorage.Helpers;
using TgStorage.Models.Apps;
using TgStorage.Models.Filters;
using TgStorage.Models.Proxies;
using TgStorage.Models.Sources;
using TL;
using Channel = TL.Channel;
using Document = TL.Document;
using TgProxyType = TgCore.Enums.TgProxyType;

namespace TgDownloader.Helpers;

public partial class TgClientHelper : ITgHelper, ITgSerializable
{
	#region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	private static TgClientHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public static TgClientHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

	#endregion

	#region Public and private fields, properties, constructor

	private TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
	private TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
	private TgSqlContextManagerHelper ContextManager => TgSqlContextManagerHelper.Instance;
	private TgLogHelper TgLog => TgLogHelper.Instance;
	public Client? Client { get; set; }
	public bool IsClientReady => Client is { } && !Client.Disconnected;
	public ExceptionModel ClientException { get; private set; }
	public ExceptionModel ProxyException { get; private set; }
	public bool IsReady
	{
		get
		{
			//return TgAppSettings.AppXml.IsExistsFileSession && Client is { } && IsClientReady && (!TgAppSettings.AppXml.IsUseProxy ||
			//	(TgAppSettings.AppXml.IsUseProxy && TgStorage.Proxies.GetItem(TgStorage.Apps.GetItem().ProxyUid).IsExists)) &&
			//	!ProxyException.IsExists && !ClientException.IsExists;
			if (!TgAppSettings.AppXml.IsExistsFileSession) return false;
			if (!IsClientReady) return false;
			if (!(!TgAppSettings.AppXml.IsUseProxy ||
				(TgAppSettings.AppXml.IsUseProxy && ContextManager.Proxies.GetItem(ContextManager.Apps.GetCurrentProxyUid).IsExists))) return false;
			if (ProxyException.IsExists || ClientException.IsExists) return false;
			return true;
		}
	}
	public User? Me { get; set; }
	public Dictionary<long, ChatBase> DicChatsAll { get; private set; }
	public Dictionary<long, ChatBase> DicChatsUpdated { get; }
	public Dictionary<long, User> DicUsersUpdated { get; }
	public List<Channel> ListChannels { get; }
	public List<Channel> ListGroups { get; }
	public List<ChatBase> ListChats { get; }
	public List<ChatBase> ListSmallGroups { get; }
	public TgClientHelper()
	{
		DicChatsAll = new();
		DicChatsUpdated = new();
		DicUsersUpdated = new();
		ListChannels = new();
		ListChats = new();
		ListGroups = new();
		ListSmallGroups = new();
		ClientException = new();
		ProxyException = new();

		// TgLog to VS Output debugging pane in addition.
		//WTelegram.Helpers.Log += (_, str) => Debug.WriteLine(str);
		// Disable logging.
		WTelegram.Helpers.Log = (_, _) => { };
	}

	#endregion

	#region Public and private methods

	public void Connect(Func<string, string?>? config, TgSqlTableProxyModel proxy)
	{
		if (IsReady) return;
		UnLoginUser();

		Client = new(config);
		ConnectThroughProxy(proxy);
		Client.OnUpdate += Client_OnUpdate;

		LoginUser(true);
	}

	public void ConnectThroughProxy(TgSqlTableProxyModel proxy)
	{
		if (!IsClientReady) return;
		if (Client is null) return;
		if (!TgAppSettings.AppXml.IsUseProxy) return;
		if (Equals(proxy.Type, TgProxyType.None)) return;
		if (!ContextManager.Proxies.IsValidXpLite(proxy)) return;

		try
		{
			ProxyException = new();
			switch (proxy.Type)
			{
				case TgProxyType.Http:
				case TgProxyType.Socks:
					Client.TcpHandler = (address, port) =>
					{
						Socks5ProxyClient proxyClient = string.IsNullOrEmpty(proxy.UserName) && string.IsNullOrEmpty(proxy.Password)
							? new(proxy.HostName, proxy.Port) : new(proxy.HostName, proxy.Port, proxy.UserName, proxy.Password);
						return Task.FromResult(proxyClient.CreateConnection(address, port));
					};
					break;
				case TgProxyType.MtProto:
					Client.MTProxyUrl = string.IsNullOrEmpty(proxy.Secret)
						? $"https://t.me/proxy?server={proxy.HostName}&port={proxy.Port}"
						: $"https://t.me/proxy?server={proxy.HostName}&port={proxy.Port}&secret={proxy.Secret}";
					break;
			}
		}
		catch (Exception ex)
		{
			ProxyException.Set(ex);
		}
	}

	public long ReduceChatId(long chatId) => !$"{chatId}".StartsWith("-100") ? chatId : Convert.ToInt64($"{chatId}"[4..]);

	public long FixChatId(long chatId) => $"{chatId}".StartsWith("-100") ? chatId : Convert.ToInt64($"-100{chatId}");

	public string GetUserUpdatedName(long id) => DicUsersUpdated.TryGetValue(ReduceChatId(id), out User? user) ? user.username : string.Empty;

	public Channel? GetChannel(TgDownloadSettingsModel tgDownloadSettings)
	{
		if (tgDownloadSettings.IsReadySourceId)
		{
			tgDownloadSettings.SourceId = ReduceChatId(tgDownloadSettings.SourceId);
			foreach (KeyValuePair<long, ChatBase> chat in DicChatsAll)
			{
				if (chat.Value is Channel channel && Equals(channel.id, tgDownloadSettings.SourceId))
					if (IsChannelAccess(channel)) return channel;
			}
		}
		else
		{
			foreach (KeyValuePair<long, ChatBase> chat in DicChatsAll)
			{
				if (chat.Value is Channel channel && Equals(channel.username, tgDownloadSettings.SourceUserName))
					if (IsChannelAccess(channel)) return channel;
			}
		}

		if (tgDownloadSettings.SourceId is 0)
			tgDownloadSettings.SourceId = GetPeerId(tgDownloadSettings.SourceUserName);

		Messages_Chats? messagesChats = Me is null ? null : Client.Channels_GetChannels(new InputChannel(tgDownloadSettings.SourceId, Me.access_hash))
			.ConfigureAwait(true).GetAwaiter().GetResult();

		if (messagesChats is not null)
			foreach (KeyValuePair<long, ChatBase> chat in messagesChats.chats)
			{
				if (chat.Value is Channel channel && Equals(channel.ID, tgDownloadSettings.SourceId))
					return channel;
			}

		return null;
	}

	public ChatBase? GetChatBase(TgDownloadSettingsModel tgDownloadSettings)
	{
		if (tgDownloadSettings.IsReadySourceId)
		{
			tgDownloadSettings.SourceId = ReduceChatId(tgDownloadSettings.SourceId);
			foreach (KeyValuePair<long, ChatBase> chat in DicChatsAll)
			{
				if (chat.Value is ChatBase chatBase && Equals(chatBase.ID, tgDownloadSettings.SourceId))
					return chatBase;
			}
		}
		else
		{
			foreach (KeyValuePair<long, ChatBase> chat in DicChatsAll)
			{
				if (chat.Value is ChatBase chatBase)
					return chatBase;
			}
		}

		if (tgDownloadSettings.SourceId is 0)
			tgDownloadSettings.SourceId = GetPeerId(tgDownloadSettings.SourceUserName);

		Messages_Chats? messagesChats = Me is null ? null : Client.Channels_GetGroupsForDiscussion()
			.ConfigureAwait(true).GetAwaiter().GetResult();

		if (messagesChats is not null)
			foreach (KeyValuePair<long, ChatBase> chat in messagesChats.chats)
			{
				if (chat.Value is ChatBase chatBase && Equals(chatBase.ID, tgDownloadSettings.SourceId))
					return chatBase;
			}

		return null;
	}

	public string GetChatUpdatedName(long id) => DicChatsUpdated.TryGetValue(ReduceChatId(id), out ChatBase? chat) ? chat.ToString() : string.Empty;

	public string GetPeerUpdatedName(Peer peer) => peer is PeerUser user ? GetUserUpdatedName(user.user_id)
		: peer is PeerChat or PeerChannel ? GetChatUpdatedName(peer.ID) : $"Peer {peer.ID}";

	public Dictionary<long, ChatBase> CollectAllChats()
	{
		switch (IsReady)
		{
			case true when Client is { }:
			{
					Messages_Chats messages = Client.Messages_GetAllChats()
					.ConfigureAwait(true).GetAwaiter().GetResult();
				DicChatsAll = messages.chats;
					FillListChats(DicChatsAll);
				return DicChatsAll;
				}
		}
		return new();
	}

	public Dictionary<long, ChatBase> CollectAllDialogs()
	{
		switch (IsReady)
		{
			case true when Client is { }:
				{
					Messages_Dialogs messages = Client.Messages_GetAllDialogs()
						.ConfigureAwait(true).GetAwaiter().GetResult();
					DicChatsAll = messages.chats;
					FillListChats(DicChatsAll);
					return messages.chats;
				}
		}
		return new();
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

	public async Task Client_OnUpdate(IObject arg)
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
						//Client_DisplayMessage(null);
						break; // there are much more update types than the above example cases
				}
			}
		});
	}

	private void Client_DisplayMessage(MessageBase messageBase, bool edit = false)
	{
		if (edit) Console.Write("(Edit): ");
		switch (messageBase)
		{
			case Message message:
				TgLog.MarkupLine($"{GetPeerUpdatedName(message.from_id)} in {GetPeerUpdatedName(message.peer_id)}> {message.message}");
				break;
			case MessageService messageService:
				TgLog.MarkupLine($"{GetPeerUpdatedName(messageService.from_id)} in {GetPeerUpdatedName(messageService.peer_id)} [{messageService.action.GetType().Name[13..]}]");
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
			TgLog.MarkupLine($"Type the message into the chat: {target.Title} | {chatId}");
			input = Console.ReadLine();
			if (Client is { })
				await Client.SendMessageAsync(target, input);
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

	public void PrintChatsInfo(Dictionary<long, ChatBase> dicChats, string name, bool isSave)
	{
		TgLog.MarkupInfo($"Found {name}: {dicChats.Count}");
		TgLog.MarkupInfo(TgLocale.TgGetDialogsInfo);
		foreach (KeyValuePair<long, ChatBase> dicChat in dicChats)
		{
			TryCatchAction(() =>
			{
				switch (dicChat.Value)
				{
					case Channel channel:
						PrintChatsInfoChannel(channel, false, false, isSave);
						break;
					default:
						TgLog.MarkupLine(GetChatInfo(dicChat.Value));
						break;
				}
			});
		}
	}

	private Messages_ChatFull? PrintChatsInfoChannel(Channel channel, bool isFull, bool isSilent, bool isSave)
	{
		Messages_ChatFull? chatFull = null;
		try
		{
			chatFull = Client.Channels_GetFullChannel(channel).ConfigureAwait(true).GetAwaiter().GetResult();
			if (isSave)
				ContextManager.Sources.AddOrUpdateItem(new() { Id = channel.id, UserName = channel.username, Title = channel.title });
			if (!isSilent)
			{
				if (chatFull.full_chat is ChannelFull channelFull)
					TgLog.MarkupLine(GetChannelFullInfo(channelFull, channel, isFull));
				else
					TgLog.MarkupLine(GetChatFullBaseInfo(chatFull.full_chat, channel, isFull));
			}
		}
		catch (Exception ex)
		{
			//if (Equals(ex.Message, "CHANNEL_INVALID"))
			TgLog.MarkupLine($"{channel.id} exception: {ex.Message}");
		}
		return chatFull;
	}

	private Messages_ChatFull? PrintChatsInfoChatBase(ChatBase chatBase, bool isFull, bool isSilent)
	{
		Messages_ChatFull? chatFull = null;
		if (Client is null) return chatFull;
		try
		{
			chatFull = Client.GetFullChat(chatBase).ConfigureAwait(true).GetAwaiter().GetResult();
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
			//if (Equals(ex.Message, "CHANNEL_INVALID"))
			TgLog.MarkupLine($"{chatBase.ID} exception: {ex.Message}");
		}
		return chatFull;
	}

	public string GetChatInfo(ChatBase chat) => $"{chat.ID} | {chat.Title}";

	public string GetChannelFullInfo(ChannelFull channelFull, ChatBase chat, bool isFull)
	{
		string result = GetChatInfo(chat);
		if (isFull)
			result += " | " + Environment.NewLine + channelFull.About;
		return result;
	}

	public string GetChatFullBaseInfo(ChatFullBase chatFull, ChatBase chat, bool isFull)
	{
		string result = GetChatInfo(chat);
		if (isFull)
			result += " | " + Environment.NewLine + chatFull.About;
		return result;
	}

	public bool IsChannelAccess(ChatBase chat)
	{
		if (chat.ID is 0 || chat is not Channel channel) return false;
		return Client.Channels_ReadMessageContents(channel).ConfigureAwait(true).GetAwaiter().GetResult();
	}

	public void ActionChannelAccess(ChatBase chat, Action<ChatFullBase> action)
	{
		if (Client is null) return;
		if (chat.ID is 0 || chat is not ChatBase chatBase) return;
		
		if (chatBase is Channel channel)
		{
			Messages_ChatFull fullChannel = Client.Channels_GetFullChannel(channel).ConfigureAwait(true).GetAwaiter().GetResult();
			if (fullChannel.full_chat is not ChannelFull channelFull) return;
			bool isAccessToMessages = Client.Channels_ReadMessageContents(channel).ConfigureAwait(true).GetAwaiter().GetResult();
			if (isAccessToMessages)
			{
				action(channelFull);
			}
		}
		else
		{
			Messages_ChatFull fullChannel = Client.GetFullChat(chatBase).ConfigureAwait(true).GetAwaiter().GetResult();
			action(fullChannel.full_chat);
		}
	}

	public int GetChannelMessageIdLast(ChatBase chat)
	{
		int max = 0;
		ActionChannelAccess(chat, chatFullBase =>
		{
			if (chatFullBase is ChannelFull channelFull)
				max = channelFull.read_inbox_max_id;
		});
		return max;
	}

	public void SetChannelMessageIdFirst(TgDownloadSettingsModel tgDownloadSettings, ChatBase chat, Action<string, bool> refreshStatus)
	{
		refreshStatus("Get the first ID message is run.", false);
		ActionChannelAccess(chat, chatFullBase =>
		{
			int max = chatFullBase is ChannelFull channelFull ? channelFull.read_inbox_max_id : 0;
			int min = max;
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
				tgDownloadSettings.SourceFirstId = offset;
				refreshStatus($"Read from {offset} to {offset + partition} messages.", false);
				Messages_MessagesBase? messages = Client.Channels_GetMessages(chat as Channel, inputMessages).ConfigureAwait(true).GetAwaiter().GetResult();
				for (int i = messages.Offset; i < messages.Count; i++)
				{
					// Skip first message.
					if (!isSkipChannelCreate)
					{
						string? msg = messages.Messages[i].ToString();
						// Magic String. It needs refactoring.
						if (Equals(msg, $"{chat.ID} [ChannelCreate]"))
						{
							isSkipChannelCreate = true;
						}
					}
					// Check message exists.
					else if (messages.Messages[i].Date > DateTime.MinValue)
					{
						min = offset + i + 1;
						break;
					}
				}
				if (min < max) break;
				offset += partition;
			}
			// Finally.
			if (min >= max)
				min = 1;
			tgDownloadSettings.SourceFirstId = min;
			refreshStatus($"Get the first ID message '{min}' is complete.", false);
		});
	}

	public Channel? PrepareChannelDownloadMessages(TgDownloadSettingsModel tgDownloadSettings, bool isSilent)
	{
		Channel? channel = null;
		TryCatchAction(() =>
		{
			channel = GetChannel(tgDownloadSettings);
			if (channel is null) return;
			if (!IsChannelAccess(channel)) return;
			tgDownloadSettings.SourceUserName = !string.IsNullOrEmpty(channel.username) ? channel.username : string.Empty;
			Messages_ChatFull? chatFull = PrintChatsInfoChannel(channel, true, isSilent, false);
			tgDownloadSettings.SourceLastId = GetChannelMessageIdLast(channel);
			if (chatFull?.full_chat is ChannelFull channelFull)
				tgDownloadSettings.SetSource(channelFull.ID, channel.Title, channelFull.About);
		});
		return channel;
	}

	public ChatBase? PrepareChatBaseDownloadMessages(TgDownloadSettingsModel tgDownloadSettings, bool isSilent)
	{
		ChatBase? chatBase = null;
		TryCatchAction(() =>
		{
			chatBase = GetChatBase(tgDownloadSettings);
			if (chatBase is null) return;
			Messages_ChatFull? chatFull = PrintChatsInfoChatBase(chatBase, true, isSilent);
			tgDownloadSettings.SourceLastId = GetChannelMessageIdLast(chatBase);
			if (chatFull?.full_chat is ChannelFull channelFull)
				tgDownloadSettings.SetSource(channelFull.id, chatBase.Title, channelFull.About);
		});
		return chatBase;
	}

	public void ScanSource(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus, TgSourceType sourceType, 
		Action<ChatBase, string, int> storeSource)
	{
		TryCatchAction(() =>
		{
			LoginUser(false);
			switch (sourceType)
			{
				case TgSourceType.None:
					return;
				case TgSourceType.Chat:
					refreshStatus(TgConstants.CollectChats, true);
					CollectAllChats();
					break;
				case TgSourceType.Dialog:
					refreshStatus(TgConstants.CollectDialogs, true);
					CollectAllDialogs();
					break;
			}
			tgDownloadSettings.SourceScanCount = DicChatsAll.Count;
			tgDownloadSettings.SourceScanCurrent = 0;
			foreach (Channel myChannel in ListChannels)
			{
				tgDownloadSettings.SourceScanCurrent++;
				TryCatchAction(() =>
				{
					if (myChannel.IsActive)
					{
						int messagesCount = GetChannelMessageIdLast(myChannel);
						if (myChannel.IsChannel)
						{
							Messages_ChatFull? chatFull = Client.Channels_GetFullChannel(myChannel)
								.ConfigureAwait(true).GetAwaiter().GetResult();
							if (chatFull?.full_chat is ChannelFull channelFull)
							{
								storeSource(myChannel, channelFull.about, messagesCount);
								if (channelFull.about.Length <= 20)
									refreshStatus($"{myChannel} | {messagesCount} | {channelFull.about}", true);
								else
									refreshStatus($"{myChannel} | {messagesCount} | {channelFull.about.Substring(0, 20)}...", true);
							}
						}
						else
						{
							storeSource(myChannel, string.Empty, messagesCount);
							refreshStatus($"{myChannel} | {messagesCount}", true);
						}
					}
				});
			}

			foreach (Channel myGroup in ListGroups)
			{
				TryCatchAction(() =>
				{
					if (myGroup.IsActive)
					{
						int messagesCount = GetChannelMessageIdLast(myGroup);
						storeSource(myGroup, string.Empty, messagesCount);
						refreshStatus($"{myGroup} | {messagesCount}", true);
					}
				});
			}
		});
	}

	public void DownloadAllData(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus,
		Action<int, long, DateTime, TgMessageType, long, string> storeMessage,
		Action<long, long, long, string, long, long> storeDocument, Func<long, long, bool> findExistsMessage)
	{
		Channel? channel = PrepareChannelDownloadMessages(tgDownloadSettings, false);
		ChatBase? chatBase = null;
		if (channel is null || channel.id is 0)
			chatBase = PrepareChatBaseDownloadMessages(tgDownloadSettings, true);
		TryCatchAction(() =>
		{
			LoginUser(false);
			//int backupId = tgDownloadSettings.SourceFirstId;
			CreateDestDirectoryIfNotExists(tgDownloadSettings, refreshStatus);
			while (tgDownloadSettings.SourceFirstId <= tgDownloadSettings.SourceLastId)
			{
				TryCatchAction(() =>
				{
					bool isAccessToMessages = false;
					if (channel is not null)
						isAccessToMessages = Client.Channels_ReadMessageContents(channel).ConfigureAwait(true).GetAwaiter().GetResult();
					else if (chatBase is not null)
						isAccessToMessages = true;
					if (isAccessToMessages)
					{
						Messages_MessagesBase messages = channel is not null 
							? Client.Channels_GetMessages(channel, tgDownloadSettings.SourceFirstId).ConfigureAwait(true).GetAwaiter().GetResult()
							: Client.GetMessages(chatBase, tgDownloadSettings.SourceFirstId).ConfigureAwait(true).GetAwaiter().GetResult();
						foreach (MessageBase message in messages.Messages)
						{
							// Check message exists.
							if (message.Date > DateTime.MinValue)
							{
								DownloadData(tgDownloadSettings, refreshStatus, message, storeMessage, storeDocument, findExistsMessage);
							}
							else
							{
								refreshStatus("Message is not exists!", true);
							}
						}
					}
				}, refreshStatus);
				tgDownloadSettings.SourceFirstId++;
			}
			tgDownloadSettings.SourceFirstId = tgDownloadSettings.SourceLastId;
		}, refreshStatus);
	}

	private void CreateDestDirectoryIfNotExists(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
	{
		try
		{
			Directory.CreateDirectory(tgDownloadSettings.DestDirectory);
		}
		catch (Exception ex)
		{
			refreshStatus(TgLocale.DirectoryCreateIsException(ex), true);
			TgLog.MarkupWarning(TgLocale.DirectoryCreateIsException(ex));
		}
	}

	private void DownloadData(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus, MessageBase messageBase,
		Action<int, long, DateTime, TgMessageType, long, string> storeMessage,
		Action<long, long, long, string, long, long> storeDocument, Func<long, long, bool> findExistsMessage)
	{
		if (messageBase is not Message message)
		{
			refreshStatus("Empty message", true);
			return;
		}

		TryCatchAction(() =>
		{
			// Get filters.
			List<TgSqlTableFilterModel> filters = ContextManager.Filters.GetListEnabled();
			// Store message.
			bool isExistsMessage = findExistsMessage(tgDownloadSettings.SourceFirstId, tgDownloadSettings.SourceId);
			if ((isExistsMessage && tgDownloadSettings.IsRewriteMessages) || !isExistsMessage)
				storeMessage(message.ID, tgDownloadSettings.SourceId, message.Date, TgMessageType.Message, 0, message.message);
			// Parse documents and photos.
			if ((message.flags & Message.Flags.has_media) is not 0)
			{
				if (message.media is MessageMediaDocument mediaDocument)
				{
					if ((mediaDocument.flags & MessageMediaDocument.Flags.has_document) is not 0)
					{
						if (mediaDocument.document is Document document)
						{
							DownloadDataCore(tgDownloadSettings, refreshStatus, messageBase, document, null, 
								storeMessage, storeDocument, findExistsMessage, filters);
						}
					}
				}
				else if (message.media is MessageMediaPhoto { photo: Photo photo })
				{
					DownloadDataCore(tgDownloadSettings, refreshStatus, messageBase, null, photo, 
						storeMessage, storeDocument, findExistsMessage, filters);
				}
			}
			refreshStatus("Read the message", true);
		}, refreshStatus);
	}

	private (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] GetFiles(Document? document, Photo? photo, List<TgSqlTableFilterModel> filters)
	{
		string extensionName = string.Empty;
		if (document is { })
		{
			if (!string.IsNullOrEmpty(document.Filename) && (Path.GetExtension(document.Filename).TrimStart('.') is { } str))
				extensionName = str;
			if (!string.IsNullOrEmpty(document.Filename) && CheckFileAtFilter(filters, document.Filename, extensionName, document.size))
				return new (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] { (document.Filename, document.size, 
					document.date, string.Empty, string.Empty) };
			if (document.attributes.Length > 0)
			{
				if (document.attributes.Any(x => x is DocumentAttributeVideo))
				{
					extensionName = "mp4";
					if (CheckFileAtFilter(filters, string.Empty, extensionName, document.size))
						return new (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] { ($"{document.ID}.{extensionName}", document.size, 
							document.date, string.Empty, string.Empty) };
				}
				if (document.attributes.Any(x => x is DocumentAttributeAudio))
				{
					extensionName = "mp3";
					if (CheckFileAtFilter(filters, string.Empty, extensionName, document.size))
						return new (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] { ($"{document.ID}.{extensionName}", document.size, 
							document.date, string.Empty, string.Empty) };
				}
			}
			if (string.IsNullOrEmpty(document.Filename))
				if (CheckFileAtFilter(filters, string.Empty, extensionName, document.size))
					return new (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] { ($"{document.ID}.{extensionName}", document.size, 
						document.date, string.Empty, string.Empty) };
		}

		if (photo is { })
		{
			extensionName = "jpg";
			//return photo.sizes.Select(x => ($"{photo.ID} {x.Width}x{x.Height}.{GetPhotoExt(x.Type)}", Convert.ToInt64(x.FileSize), photo.date, string.Empty, string.Empty)).ToArray();
			if (CheckFileAtFilter(filters, string.Empty, extensionName, photo.sizes.Last().FileSize))
				return new (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] { ($"{photo.ID}.{extensionName}", photo.sizes.Last().FileSize, 
					photo.date, string.Empty, string.Empty) };
		}

		return Array.Empty<(string Remote, long Size, DateTime DtCreate, string Local, string Join)>();
	}

	public bool CheckFileAtFilter(List<TgSqlTableFilterModel> filters, string fileName, string extensionName, long size)
	{
		foreach (TgSqlTableFilterModel filter in filters)
		{
			if (!filter.IsEnabled) continue;
			switch (filter.FilterType)
			{
				case TgFilterType.SingleName:
					if (string.IsNullOrEmpty(fileName)) continue;
					if (!TgDataFormatUtils.CheckFileAtMask(fileName, filter.Mask)) return false;
					break;
				case TgFilterType.SingleExtension:
					if (string.IsNullOrEmpty(extensionName)) continue;
					if (!TgDataFormatUtils.CheckFileAtMask(extensionName, filter.Mask)) return false;
					break;
				case TgFilterType.MultiName:
					if (string.IsNullOrEmpty(fileName)) continue;
					bool isMultiName = false;
					foreach (string mask in filter.Mask.Split(','))
						if (TgDataFormatUtils.CheckFileAtMask(fileName, mask.TrimStart().TrimEnd())) isMultiName = true;
					if (!isMultiName) return false;
					break;
				case TgFilterType.MultiExtension:
					if (string.IsNullOrEmpty(extensionName)) continue;
					bool isMultiExtension = false;
					foreach (string mask in filter.Mask.Split(','))
						if (TgDataFormatUtils.CheckFileAtMask(extensionName, mask.TrimStart().TrimEnd())) isMultiExtension = true;
					if (!isMultiExtension) return false;
					break;
				case TgFilterType.MinSize:
					if (size < filter.SizeAtBytes) return false;
					break;
				case TgFilterType.MaxSize:
					if (size > filter.SizeAtBytes) return false;
					break;
			}
		}
		return true;
	}

	private void SetFilesLocalNames(TgDownloadSettingsModel tgDownloadSettings, MessageBase messageBase, ref (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] files)
	{
		// Join ID.
		for (int i = 0; i < files.Length; i++)
		{
			files[i].Join = tgDownloadSettings.SourceLastId switch
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
			files[i].Local = Path.Combine(tgDownloadSettings.DestDirectory, files[i].Remote);
			files[i].Join = Path.Combine(tgDownloadSettings.DestDirectory, files[i].Join);
		}
	}

	private void DeleteExistsFiles(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus,
		  (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] files)
	{
		TryCatchAction(() =>
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
		}, refreshStatus);
	}

	private void DownloadDataCore(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus,
		MessageBase messageBase, Document? document, Photo? photo,
		Action<int, long, DateTime, TgMessageType, long, string> storeMessage,
		Action<long, long, long, string, long, long> storeDocument, 
		Func<long, long, bool> findExistsMessage, List<TgSqlTableFilterModel> filters)
	{
		(string Remote, long Size, DateTime DtCreate, string Local, string Join)[] files = GetFiles(document, photo, filters);
		if (Equals(files, Array.Empty<(string Remote, long Size, DateTime DtCreate, string Local, string Join)>())) return;
		SetFilesLocalNames(tgDownloadSettings, messageBase, ref files);
		long accessHash = document?.access_hash ?? photo?.access_hash ?? 0;

		// Delete files.
		DeleteExistsFiles(tgDownloadSettings, refreshStatus, files);

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
			foreach (string directory in Directory.GetDirectories(tgDownloadSettings.DestDirectory))
			{
				string fileSubDir = Path.Combine(directory, files[i].Remote);
				if (File.Exists(fileSubDir) && !File.Exists(fileName))
					File.Move(fileSubDir, fileName);
			}
			// Download new file.
			if (!File.Exists(fileName))
			{
				using FileStream localFileStream = File.Create(fileName);
				if (Client is { })
				{
					if (document is { })
					{
						Client.DownloadFileAsync(document, localFileStream).ConfigureAwait(true).GetAwaiter().GetResult();
						storeDocument(document.ID, tgDownloadSettings.SourceId, messageBase.ID, fileName, files[i].Size, accessHash);
					}
					else if (photo is { })
					{
						//WClient.DownloadFileAsync(photo, localFileStream, new PhotoSize
						//    { h = photo.sizes[i].Height, w = photo.sizes[i].Width, 
						//        size = photo.sizes[i].FileSize, type = photo.sizes[i].Type })
						Client.DownloadFileAsync(photo, localFileStream)
							.ConfigureAwait(true).GetAwaiter().GetResult();
					}
				}
				localFileStream.Close();
			}
			// Store message.
			bool isExistsMessage = findExistsMessage(tgDownloadSettings.SourceFirstId, tgDownloadSettings.SourceId);
			if (document is not null && ((isExistsMessage && tgDownloadSettings.IsRewriteMessages) || !isExistsMessage))
			{
				storeMessage(messageBase.ID, tgDownloadSettings.SourceId, files[i].DtCreate, 
					TgMessageType.Document, files[i].Size, files[i].Remote);
			}
			else if (photo is not null && ((isExistsMessage && tgDownloadSettings.IsRewriteMessages) || !isExistsMessage))
			{
				storeMessage(messageBase.ID, tgDownloadSettings.SourceId, files[i].DtCreate, 
					TgMessageType.Photo, files[i].Size, files[i].Remote);
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

	private long GetPeerId(string userName) =>
		Client.Contacts_ResolveUsername(userName).ConfigureAwait(true).GetAwaiter().GetResult().peer.ID;

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

	public void LoginUser(bool isProxyUpdate)
	{
		ClientException = new();
		try
		{
			Me = Client?.LoginUserIfNeeded().ConfigureAwait(true).GetAwaiter().GetResult();
		}
		catch (Exception ex)
		{
			ClientException.Set(ex);
			Me = null;
		}

		if (isProxyUpdate && IsReady)
		{
			TgSqlTableAppModel app = ContextManager.Apps.GetCurrentItem();
			app.ProxyUid = ContextManager.Apps.GetCurrentProxy.Uid;
			ContextManager.Apps.AddOrUpdateItem(app);
			//CollectAllChats();
		}
	}

	public void UnLoginUser()
	{
		if (Client is not null)
		{
			Client.OnUpdate -= Client_OnUpdate;
			Client.Dispose();
			Client = null;
			ClientException = new();
			Me = null;
		}
	}

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	protected TgClientHelper(SerializationInfo info, StreamingContext context)
	{
		DicChatsAll = info.GetValue(nameof(DicChatsAll), typeof(Dictionary<long, ChatBase>)) as Dictionary<long, ChatBase> ?? new();
		DicChatsUpdated = info.GetValue(nameof(DicChatsUpdated), typeof(Dictionary<long, ChatBase>)) as Dictionary<long, ChatBase> ?? new();
		DicUsersUpdated = info.GetValue(nameof(DicUsersUpdated), typeof(Dictionary<long, User>)) as Dictionary<long, User> ?? new();
		ListChannels = info.GetValue(nameof(ListChannels), typeof(List<Channel>)) as List<Channel> ?? new();
		ListChats = info.GetValue(nameof(ListChats), typeof(List<ChatBase>)) as List<ChatBase> ?? new();
		ListGroups = info.GetValue(nameof(ListGroups), typeof(List<Channel>)) as List<Channel> ?? new();
		ListSmallGroups = info.GetValue(nameof(ListSmallGroups), typeof(List<ChatBase>)) as List<ChatBase> ?? new();
		object? clientException = info.GetValue(nameof(ClientException), typeof(ExceptionModel));
		ClientException = clientException is not null ? (ExceptionModel)clientException : new();
		object? proxyException = info.GetValue(nameof(ProxyException), typeof(ExceptionModel));
		ProxyException = proxyException is not null ? (ExceptionModel)proxyException : new();
	}

	/// <summary>
	/// Get object data for serialization info.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(DicChatsAll), DicChatsAll);
		info.AddValue(nameof(DicChatsUpdated), DicChatsUpdated);
		info.AddValue(nameof(DicUsersUpdated), DicUsersUpdated);
		info.AddValue(nameof(ListChannels), ListChannels);
		info.AddValue(nameof(ListChats), ListChats);
		info.AddValue(nameof(ListGroups), ListGroups);
		info.AddValue(nameof(ListSmallGroups), ListSmallGroups);
		info.AddValue(nameof(ClientException), ClientException);
		info.AddValue(nameof(ProxyException), ProxyException);
	}

	#endregion

	#region Public and private methods

	private void TryCatchAction(Action action, Action<string, bool>? refreshStatus = null)
	{
		try
		{
			action();
		}
		catch (Exception ex)
		{
			// It should be saved and asked to be sent to the developer.
			if (refreshStatus is not null)
			{
				refreshStatus(ex.Message, false);
				if (ex.InnerException is not null)
					refreshStatus(ex.InnerException.Message, false);
			}
		}
	}

	#endregion
}