﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Net.Sockets;

namespace TgDownloader.Helpers;

public class TgClientHelper : ITgHelper
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
	public bool IsClientReady => Client is { Disconnected: false };
	public TgExceptionModel ClientException { get; private set; }
	public string TgQuery { get; set; }
	public TgExceptionModel ProxyException { get; private set; }
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
				(TgAppSettings.AppXml.IsUseProxy && ContextManager.ContextTableProxies.GetItem(ContextManager.ContextTableApps.GetCurrentProxyUid).IsExists))) return false;
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
	public Action<string> UpdateException { get; set; }
	public Action<string> UpdateState { get; set; }
	public Action<string> UpdateStateProgress { get; set; }
	public Action<long, int, string> UpdateStateSource { get; set; }
	public bool IsUpdateStatus { get; set; }
	private object ChannelUpdateLocker { get; }
	public TgEnumAppType AppType { get; set; }
	private List<TgSqlTableFilterModel> Filters { get; set; }
	private object Locker { get; }

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
		UpdateException = _ => { };
		UpdateState = _ => { };
		UpdateStateProgress = _ => { };
		UpdateStateSource = (_, _, _) => { };
		ChannelUpdateLocker = new();
		TgQuery = string.Empty;
		AppType = TgEnumAppType.Default;
		Filters = new();
		Locker = new();

		// TgLog to VS Output debugging pane in addition.
		//WTelegram.Helpers.Log += (i, str) => Debug.WriteLine($"{i} | {str}");
		// Disable logging in Console.
		WTelegram.Helpers.Log = (_, _) => { };
	}

	#endregion

	#region Public and private methods

	public void ConnectSessionConsole(Func<string, string?>? config, TgSqlTableProxyModel proxy, Action? afterConnect = null)
	{
		if (IsReady) return;
		Disconnect();

		AppType = TgEnumAppType.Console;
		Client = new(config);
		ConnectThroughProxy(proxy);
		Client.OnUpdate += Client_OnUpdateAsync;

		LoginUserConsole(true, afterConnect);
	}

	public void ConnectSessionDesktop(Func<string, string?>? config, TgSqlTableProxyModel proxy, Action? afterConnect = null)
	{
		if (IsReady) return;
		Disconnect();

		AppType = TgEnumAppType.Desktop;
		Client = new(config);
		ConnectThroughProxy(proxy);
		Client.OnUpdate += Client_OnUpdateAsync;

		LoginUserDesktop(true, afterConnect);
	}

	public void ConnectThroughProxy(TgSqlTableProxyModel proxy)
	{
		if (!IsClientReady) return;
		if (Client is null) return;
		if (!TgAppSettings.AppXml.IsUseProxy) return;
		if (Equals(proxy.Type, TgEnumProxyType.None)) return;
		if (!ContextManager.ContextTableProxies.GetValidXpLite(proxy).IsValid) return;

		try
		{
			ProxyException = new();
			switch (proxy.Type)
			{
				case TgEnumProxyType.Http:
				case TgEnumProxyType.Socks:
					Client.TcpHandler = (address, port) =>
					{
						Socks5ProxyClient proxyClient = string.IsNullOrEmpty(proxy.UserName) && string.IsNullOrEmpty(proxy.Password)
							? new(proxy.HostName, proxy.Port) : new(proxy.HostName, proxy.Port, proxy.UserName, proxy.Password);
						return Task.FromResult(proxyClient.CreateConnection(address, port));
					};
					break;
				case TgEnumProxyType.MtProto:
					Client.MTProxyUrl = string.IsNullOrEmpty(proxy.Secret)
						? $"https://t.me/proxy?server={proxy.HostName}&port={proxy.Port}"
						: $"https://t.me/proxy?server={proxy.HostName}&port={proxy.Port}&secret={proxy.Secret}";
					break;
			}
		}
		catch (Exception ex)
		{
			SetProxyException(ex);
		}
	}

	public long ReduceChatId(long chatId) => !$"{chatId}".StartsWith("-100") ? chatId : Convert.ToInt64($"{chatId}"[4..]);

	//public long FixChatId(long chatId) => $"{chatId}".StartsWith("-100") ? chatId : Convert.ToInt64($"-100{chatId}");

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

		Messages_Chats? messagesChats = null;
		if (Me is not null)
			messagesChats = GetTaskResult<Messages_Chats?>(() => 
		Client.Channels_GetChannels(new InputChannel(tgDownloadSettings.SourceId, Me.access_hash)));
		if (messagesChats is not null)
		{
			foreach (KeyValuePair<long, ChatBase> chat in messagesChats.chats)
			{
				if (chat.Value is Channel channel && Equals(channel.ID, tgDownloadSettings.SourceId))
					return channel;
			}
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
				if (chat.Value is { } chatBase && Equals(chatBase.ID, tgDownloadSettings.SourceId))
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

		if (tgDownloadSettings.SourceId is 0)
			tgDownloadSettings.SourceId = GetPeerId(tgDownloadSettings.SourceUserName);

		Messages_Chats? messagesChats = null;
		if (Me is not null)
			messagesChats = GetTaskResult(() => Client.Channels_GetGroupsForDiscussion());

		if (messagesChats is not null)
			foreach (KeyValuePair<long, ChatBase> chat in messagesChats.chats)
			{
				if (chat.Value is { } chatBase && Equals(chatBase.ID, tgDownloadSettings.SourceId))
					return chatBase;
			}

		return null;
	}

	public Bots_BotInfo? GetBotInfo(TgDownloadSettingsModel tgDownloadSettings)
	{
		if (tgDownloadSettings.SourceId is 0)
			tgDownloadSettings.SourceId = GetPeerId(tgDownloadSettings.SourceUserName);
		if (!tgDownloadSettings.IsReadySourceId)
			tgDownloadSettings.SourceId = ReduceChatId(tgDownloadSettings.SourceId);
		if (!tgDownloadSettings.IsReadySourceId) return null;
		Bots_BotInfo? botInfo = null;
			if (Me is not null)
				botInfo = GetTaskResult(() => 
					Client.Bots_GetBotInfo("en", new InputUser(tgDownloadSettings.SourceId, 0)));
		return botInfo;
	}

	public string GetChatUpdatedName(long id) => DicChatsUpdated.TryGetValue(ReduceChatId(id), out ChatBase? chat) ? chat.ToString() : string.Empty;

	public string GetPeerUpdatedName(Peer peer) => peer is PeerUser user ? GetUserUpdatedName(user.user_id)
		: peer is PeerChat or PeerChannel ? GetChatUpdatedName(peer.ID) : $"Peer {peer.ID}";

	public Dictionary<long, ChatBase> CollectAllChatsConsole()
	{
		switch (IsReady)
		{
			case true when Client is not null:
				{
					Messages_Chats messages = GetTaskResult(() => Client.Messages_GetAllChats());
					DicChatsAll = messages.chats;
					FillListChats(DicChatsAll);
					return DicChatsAll;
				}
		}
		return new();
	}

public void CollectAllChatsDesktopAsync(Action<Action<TgMvvmSourceModel>> afterCollect, 
	Action<TgMvvmSourceModel> afterScan)
	{
		switch (IsReady)
		{
			case true when Client is not null:
				Messages_Chats messages = GetTaskResult(Client.Messages_GetAllChats);
				DicChatsAll = messages.chats;
				FillListChats(DicChatsAll);
				break;
		}
		afterCollect(afterScan);
	}

	public Dictionary<long, ChatBase> CollectAllDialogs()
	{
		switch (IsReady)
		{
			case true when Client is { }:
				{
					Messages_Dialogs messages = GetTaskResult(() => Client.Messages_GetAllDialogs());
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

	public async Task Client_OnUpdateAsync(IObject arg)
	{
		if (arg is not UpdatesBase updates) return;
		if (!IsUpdateStatus) return;
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		try
		{
			lock (ChannelUpdateLocker)
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
										SwitchUpdateType(update, channel);
									}
								}
							}
							else
								SwitchUpdateType(update);
						}
						catch (Exception ex)
						{
							SetClientException(ex);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			SetClientException(ex);
		}
	}

	// https://corefork.telegram.org/type/Update
	private void SwitchUpdateType(Update update, Channel? channel = null)
	{
		string channelLabel = channel is null ? string.Empty :
			string.IsNullOrEmpty(channel.MainUsername) ? channel.ID.ToString() : $"{channel.ID} | {channel.MainUsername}";
		if (!string.IsNullOrEmpty(channelLabel))
			channelLabel = $" for channel [{channelLabel}]";
		switch (update)
		{
			case UpdateNewChannelMessage updateNewChannelMessage:
				if (channel is not null && updateNewChannelMessage.message.Peer.ID.Equals(channel.ID))
				{
					//UpdateSource(channel, updateNewChannelMessage.message.ID);
					UpdateState($" {TgLog.GetDtStamp()} | New channel message [{updateNewChannelMessage.message.ID}]{channelLabel}");
				}
				break;
			case UpdateNewMessage updateNewMessage:
				UpdateState($" {TgLog.GetDtStamp()} | New message [{updateNewMessage.message.ID}]{channelLabel}");
				break;
			case UpdateMessageID updateMessageId:
				UpdateState($" {TgLog.GetDtStamp()} | Message ID [{updateMessageId.id}]{channelLabel}");
				break;
			case UpdateDeleteChannelMessages deleteChannelMessages:
				UpdateState($" {TgLog.GetDtStamp()} | Delete channel messages [{string.Join(", ", deleteChannelMessages.messages)}]{channelLabel}");
				break;
			case UpdateDeleteMessages updateDeleteMessages:
				UpdateState($" {TgLog.GetDtStamp()} | Delete messages [{string.Join(", ", updateDeleteMessages.messages)}]{channelLabel}");
				break;
			case UpdateChatUserTyping updateChatUserTyping:
				UpdateState($" {TgLog.GetDtStamp()} | Chat user typing [{updateChatUserTyping.from_id}]{channelLabel}");
				break;
			case UpdateChatParticipants { participants: ChatParticipants chatParticipants }:
				UpdateState($" {TgLog.GetDtStamp()} | Chat participants [{chatParticipants.ChatId} | {string.Join(", ", chatParticipants.Participants.Length)}]{channelLabel}");
				break;
			case UpdateUserStatus updateUserStatus:
				UpdateState($" {TgLog.GetDtStamp()} | User status [{updateUserStatus.user_id} | {updateUserStatus.status}]{channelLabel}");
				break;
			case UpdateUserName updateUserName:
				UpdateState($" {TgLog.GetDtStamp()} | User name [{updateUserName.user_id} | {string.Join(", ", updateUserName.usernames.Select(item => item.username))}]{channelLabel}");
				break;
			case UpdateNewEncryptedMessage updateNewEncryptedMessage:
				UpdateState($" {TgLog.GetDtStamp()} | New encrypted message [{updateNewEncryptedMessage.message.ChatId}]{channelLabel}");
				break;
			case UpdateEncryptedChatTyping updateEncryptedChatTyping:
				UpdateState($" {TgLog.GetDtStamp()} | Encrypted chat typing [{updateEncryptedChatTyping.chat_id}]{channelLabel}");
				break;
			case UpdateEncryption updateEncryption:
				UpdateState($" {TgLog.GetDtStamp()} | Encryption [{updateEncryption.chat.ID}]{channelLabel}");
				break;
			case UpdateEncryptedMessagesRead updateEncryptedMessagesRead:
				UpdateState($" {TgLog.GetDtStamp()} | Encrypted message read [{updateEncryptedMessagesRead.chat_id}]{channelLabel}");
				break;
			case UpdateChatParticipantAdd updateChatParticipantAdd:
				UpdateState($" {TgLog.GetDtStamp()} | Chat participant add [{updateChatParticipantAdd.user_id}]{channelLabel}");
				break;
			case UpdateChatParticipantDelete updateChatParticipantDelete:
				UpdateState($" {TgLog.GetDtStamp()} | Chat participant delete [{updateChatParticipantDelete.user_id}]{channelLabel}");
				break;
			case UpdateDcOptions updateDcOptions:
				UpdateState($" {TgLog.GetDtStamp()} | Dc options [{string.Join(", ", updateDcOptions.dc_options.Select(item => item.id))}]{channelLabel}");
				break;
			case UpdateNotifySettings updateNotifySettings:
				UpdateState($" {TgLog.GetDtStamp()} | Notify settings [{updateNotifySettings.notify_settings}]{channelLabel}");
				break;
			case UpdateServiceNotification updateServiceNotification:
				UpdateState($" {TgLog.GetDtStamp()} | Service notification [{updateServiceNotification.message}]{channelLabel}");
				break;
			case UpdatePrivacy updatePrivacy:
				UpdateState($" {TgLog.GetDtStamp()} | Privacy [{updatePrivacy.key}]{channelLabel}");
				break;
			case UpdateUserPhone updateUserPhone:
				UpdateState($" {TgLog.GetDtStamp()} | User phone [{updateUserPhone.phone}]{channelLabel}");
				break;
			case UpdateReadHistoryInbox updateReadHistoryInbox:
				UpdateState($" {TgLog.GetDtStamp()} | Read history inbox [{updateReadHistoryInbox.flags}]{channelLabel}");
				break;
			case UpdateReadHistoryOutbox updateReadHistoryOutbox:
				UpdateState($" {TgLog.GetDtStamp()} | Read history outbox [{updateReadHistoryOutbox.peer}]{channelLabel}");
				break;
			case UpdateWebPage updateWebPage:
				UpdateState($" {TgLog.GetDtStamp()} | Web page [{updateWebPage.webpage.ID}]{channelLabel}");
				break;
			case UpdateReadMessagesContents updateReadMessagesContents:
				UpdateState($" {TgLog.GetDtStamp()} | Read messages contents [{string.Join(", ", updateReadMessagesContents.messages.Select(item => item.ToString()))}]{channelLabel}");
				break;

			case UpdateEditMessage updateEditMessage:
				UpdateState($" {TgLog.GetDtStamp()} | Edit message [{updateEditMessage.message.ID}]{channelLabel}");
				break;
			case UpdateUserTyping updateUserTyping:
				UpdateState($" {TgLog.GetDtStamp()} | User typing [{updateUserTyping.user_id}]{channelLabel}");
				break;
			case UpdateChannel updateChannel:
				UpdateState($" {TgLog.GetDtStamp()} | Channel [{updateChannel.channel_id}]");
				break;
			case UpdateChannelReadMessagesContents updateChannelReadMessages:
				UpdateState($" {TgLog.GetDtStamp()} | Channel read messages [{string.Join(", ", updateChannelReadMessages.messages)}]{channelLabel}");
				break;
			case UpdateChannelUserTyping updateChannelUserTyping:
				UpdateState($" {TgLog.GetDtStamp()} | Channel user typing [{updateChannelUserTyping.from_id}]{channelLabel}");
				break;
			case UpdateMessagePoll updateMessagePoll:
				UpdateState($" {TgLog.GetDtStamp()} | Message poll [{updateMessagePoll.poll_id}]{channelLabel}");
				break;
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
			chatFull = GetTaskResult(() => Client.Channels_GetFullChannel(channel));
			if (isSave)
				ContextManager.ContextTableSources.AddOrUpdateItem(new() { Id = channel.id, UserName = channel.username, Title = channel.title });
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
			SetClientException(ex);
		}
		return chatFull;
	}

	private Messages_ChatFull? PrintChatsInfoChatBase(ChatBase chatBase, bool isFull, bool isSilent)
	{
		Messages_ChatFull? chatFull = null;
		if (Client is null) return chatFull;
		try
		{
			chatFull = GetTaskResult(() => Client.GetFullChat(chatBase));
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
			SetClientException(ex);
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

	public bool IsChannelAccess(ChatBase chatBase)
	{
		if (chatBase.ID is 0 || chatBase is not Channel channel) return false;
		return GetTaskResult(() => Client.Channels_ReadMessageContents(channel));
	}

	public T GetTaskResult<T>(Func<Task<T>> taskFunc)
	{
		switch (AppType)
		{
			case TgEnumAppType.Desktop:
				Task<T> task = Task.Run(async () =>
				{
					await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
					return await taskFunc().ConfigureAwait(false);
				});
				task.ConfigureAwait(false);
				return task.GetAwaiter().GetResult();
			case TgEnumAppType.Console:
				return taskFunc().ConfigureAwait(true).GetAwaiter().GetResult();
			default:
				throw new ArgumentException(nameof(AppType));
		}
	}

	public void ExecTask(Func<Task> taskFunc, bool isWait)
	{
		switch (AppType)
		{
			case TgEnumAppType.Desktop:
				Task task = Task.Run(async () =>
				{
					await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
					await taskFunc().ConfigureAwait(false);
				});
				task.ConfigureAwait(false);
				if (isWait)
					task.GetAwaiter().GetResult();
				break;
			case TgEnumAppType.Console:
				taskFunc().ConfigureAwait(true);
				//if (isWait)
				//	taskFunc.GetAwaiter().GetResult();
				break;
			default:
				throw new ArgumentException(nameof(AppType));
		}
	}

	//public void ExecTask(Task taskFunc, bool isWait)
	//{
	//	switch (AppType)
	//	{
	//		case TgEnumAppType.Desktop:
	//			Task task = Task.Run(async () =>
	//			{
	//				await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
	//				await taskFunc().ConfigureAwait(false);
	//			});
	//			task.ConfigureAwait(false);
	//			if (isWait)
	//				task.GetAwaiter().GetResult();
	//			break;
	//		case TgEnumAppType.Console:
	//			taskFunc().ConfigureAwait(true);
	//			if (isWait)
	//				taskFunc.GetAwaiter().GetResult();
	//			break;
	//		default:
	//			throw new ArgumentException(nameof(AppType));
	//	}
	//}

	public void ExecAction(Action action)
	{
		switch (AppType)
		{
			case TgEnumAppType.Desktop:
				Task task = Task.Run(async () =>
				{
					await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
					action();
				});
				task.ConfigureAwait(false);
				break;
			case TgEnumAppType.Console:
				action();
				break;
			default:
				throw new ArgumentException(nameof(AppType));
		}
	}

		public int GetChannelMessageIdWithLock(TgDownloadSettingsModel? tgDownloadSettings, ChatBase chatBase,
		TgEnumPosition position)
	{
		lock (ChannelUpdateLocker)
		{
			return GetChannelMessageIdCore(tgDownloadSettings, chatBase, position);
		}
	}

	public int GetChannelMessageIdWithLock(ChatBase chatBase, TgEnumPosition position)
	{
		lock (ChannelUpdateLocker)
		{
			return GetChannelMessageIdCore(null, chatBase, position);
		}
	}

	public int GetChannelMessageIdWithoutLock(TgDownloadSettingsModel? tgDownloadSettings, ChatBase chatBase,
		TgEnumPosition position) => 
		GetChannelMessageIdCore(tgDownloadSettings, chatBase, position);

	private int GetChannelMessageIdCore(TgDownloadSettingsModel? tgDownloadSettings, ChatBase chatBase,
		TgEnumPosition position)
	{
		if (Client is null) return 0;
		if (chatBase.ID is 0) return 0;

		if (chatBase is Channel channel)
		{
			Messages_ChatFull fullChannel = GetTaskResult(() => Client.Channels_GetFullChannel(channel));
			if (fullChannel.full_chat is not ChannelFull channelFull) return 0;
			bool isAccessToMessages = GetTaskResult(() => Client.Channels_ReadMessageContents(channel));
			if (isAccessToMessages)
			{
				switch (position)
				{
					case TgEnumPosition.First:
						if (tgDownloadSettings is not null)
							return SetChannelMessageIdFirstCore(tgDownloadSettings, chatBase, channelFull);
						break;
					case TgEnumPosition.Last:
						return GetChannelMessageIdLastCore(channelFull);
				}
			}
		}
		else
		{
			Messages_ChatFull fullChannel = GetTaskResult(() => Client.GetFullChat(chatBase));
			switch (position)
			{
				case TgEnumPosition.First:
					if (tgDownloadSettings is not null)
						return SetChannelMessageIdFirstCore(tgDownloadSettings, chatBase, fullChannel.full_chat);
					break;
				case TgEnumPosition.Last:
					return GetChannelMessageIdLastCore(fullChannel.full_chat);
			}
		}
		return 0;
	}

	public int GetChannelMessageIdLastWithLock(TgDownloadSettingsModel? tgDownloadSettings, ChatBase chatBase) =>
		GetChannelMessageIdWithLock(tgDownloadSettings, chatBase, TgEnumPosition.Last);

	public int GetChannelMessageIdLastWithoutLock(TgDownloadSettingsModel? tgDownloadSettings, ChatBase chatBase) =>
		GetChannelMessageIdWithoutLock(tgDownloadSettings, chatBase, TgEnumPosition.Last);

	private int GetChannelMessageIdLastCore(ChatFullBase chatFullBase) =>
		chatFullBase is ChannelFull channelFull ? channelFull.read_inbox_max_id : 0;

	public void SetChannelMessageIdFirstWithLock(TgDownloadSettingsModel tgDownloadSettings, ChatBase chatBase) =>
		GetChannelMessageIdWithLock(tgDownloadSettings, chatBase, TgEnumPosition.First);

	public void SetChannelMessageIdFirstWithoutLock(TgDownloadSettingsModel tgDownloadSettings, ChatBase chatBase) =>
		GetChannelMessageIdWithoutLock(tgDownloadSettings, chatBase, TgEnumPosition.First);

	private int SetChannelMessageIdFirstCore(TgDownloadSettingsModel tgDownloadSettings, ChatBase chatBase,
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
			tgDownloadSettings.SourceFirstId = offset;
			UpdateState($"Read from {offset} to {offset + partition} messages.");
			Messages_MessagesBase? messages = GetTaskResult(() => 
				Client.Channels_GetMessages(chatBase as Channel, inputMessages));
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
			if (result < max) break;
			offset += partition;
		}
		// Finally.
		if (result >= max)
			result = 1;
		tgDownloadSettings.SourceFirstId = result;
		UpdateState($"Get the first ID message '{result}' is complete.");
		return result;
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
			tgDownloadSettings.SourceLastId = GetChannelMessageIdLastWithLock(tgDownloadSettings, channel);
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
			tgDownloadSettings.SourceLastId = GetChannelMessageIdLastWithLock(tgDownloadSettings, chatBase);
			if (chatFull?.full_chat is ChannelFull channelFull)
				tgDownloadSettings.SetSource(channelFull.id, chatBase.Title, channelFull.About);
		});
		return chatBase;
	}

	private void UpdateSource(Channel channel, int count) => UpdateSource(channel, string.Empty, count);

	private void UpdateSource(Channel channel, string about, int count) =>
		ContextManager.ContextTableSources.AddOrUpdateItem(new()
		{
			Id = channel.id,
			UserName = channel.username,
			Title = channel.title,
			About = about,
			Count = count
		});

	public void ScanSourceConsole(TgDownloadSettingsModel tgDownloadSettings, TgEnumSourceType sourceType)
	{
		TryCatchAction(() =>
		{
			LoginUserConsole(false);
			switch (sourceType)
			{
				case TgEnumSourceType.Default:
					return;
				case TgEnumSourceType.Chat:
					UpdateStateProgress(TgLocale.CollectChats);
					CollectAllChatsConsole();
					break;
				case TgEnumSourceType.Dialog:
					UpdateStateProgress(TgLocale.CollectDialogs);
					CollectAllDialogs();
					break;
			}
			tgDownloadSettings.SourceScanCount = DicChatsAll.Count;
			tgDownloadSettings.SourceScanCurrent = 0;
			// ListChannels.
			foreach (Channel channel in ListChannels)
			{
				tgDownloadSettings.SourceScanCurrent++;
				TryCatchAction(() =>
				{
					if (channel.IsActive)
					{
						int messagesCount = GetChannelMessageIdLastWithoutLock(tgDownloadSettings, channel);
						if (channel.IsChannel)
						{
							Messages_ChatFull? chatFull = GetTaskResult(() => 
								Client.Channels_GetFullChannel(channel));
							if (chatFull?.full_chat is ChannelFull channelFull)
							{
								UpdateSource(channel, channelFull.about, messagesCount);
								UpdateStateProgress($"{channel} | {messagesCount} | {TgDataFormatUtils.TrimStringEnd(channelFull.about)}");
							}
						}
						else
						{
							UpdateSource(channel, messagesCount);
							UpdateStateProgress($"{channel} | {messagesCount}");
						}
					}
				});
			}
			// ListGroups.
			foreach (Channel group in ListGroups)
			{
				TryCatchAction(() =>
				{
					if (group.IsActive)
					{
						int messagesCount = GetChannelMessageIdLastWithoutLock(tgDownloadSettings, group);
						UpdateSource(group, messagesCount);
						UpdateStateProgress($"{group} | {messagesCount}");
					}
				});
			}
		});
	}

	public void ScanSourceDesktop(TgEnumSourceType sourceType, Action<TgMvvmSourceModel> afterScan)
	{
		TryCatchActionDesktop(() =>
		{
			//LoginUserDesktop(false);
			switch (sourceType)
			{
				case TgEnumSourceType.Chat:
					UpdateStateProgress(TgLocale.CollectChats);
					CollectAllChatsDesktopAsync(AfterCollectSources, afterScan);
					break;
				case TgEnumSourceType.Dialog:
					UpdateStateProgress(TgLocale.CollectDialogs);
					CollectAllDialogs();
					break;
			}
			;
		});
	}

	private void AfterCollectSources(Action<TgMvvmSourceModel> afterScan)
	{
		// ListChannels.
		int i = 0;
		int count = ListChannels.Count;
		foreach (Channel channel in ListChannels)
		{
			TryCatchActionDesktop(() =>
			{
				TgSqlTableSourceModel source = new() { Id = channel.ID };
				if (channel.IsActive)
				{
					int messagesCount = GetChannelMessageIdLastWithoutLock(new(), channel);
					source.Count = messagesCount;
					if (channel.IsChannel)
					{
						Messages_ChatFull chatFull = GetTaskResult(() => Client.Channels_GetFullChannel(channel));
						if (chatFull.full_chat is ChannelFull channelFull)
						{
							source.About = channelFull.about;
							source.UserName = channel.username;
								source.Title = channel.title;
						}
					}
					afterScan(new(source));
				}
			});
				i++;
				UpdateState( $"Read channel '{channel.ID}' | {channel.IsActive} | {i} from {count}");
				UpdateStateSource(channel.ID, 0, $"Read channel '{channel.ID}' | {channel.IsActive} | {i} from {count}");
		}

		// ListGroups.
		i = 0;
		count = ListGroups.Count;
		foreach (Channel group in ListGroups)
		{
			TryCatchActionDesktop(() =>
			{
				TgSqlTableSourceModel source = new() { Id = group.ID };
				if (group.IsActive)
				{
					int messagesCount = GetChannelMessageIdLastWithoutLock(new(), group);
					source.Count = messagesCount;
				}
				afterScan(new(source));
			});
				i++;
				UpdateState($"Read group '{group.ID}' | {group.IsActive} | {i} from {count}");
				UpdateStateSource(group.ID, 0, $"Read channel '{group.ID}' | {group.IsActive} | {i} from {count}");
		}
	}

	public void DownloadAllData(TgDownloadSettingsModel tgDownloadSettings,
		Action<int, long, DateTime, TgEnumMessageType, long, string> storeMessage,
		Action<long, long, long, string, long, long> storeDocument, Func<long, long, bool> findExistsMessage)
	{
		Channel? channel = PrepareChannelDownloadMessages(tgDownloadSettings, false);
		ChatBase? chatBase = null;
		if (channel is null || channel.id is 0)
			chatBase = PrepareChatBaseDownloadMessages(tgDownloadSettings, true);
		TryCatchAction(() =>
		{
			// Set filters.
			Filters = ContextManager.ContextTableFilters.GetListEnabled();

			if (AppType.Equals(TgEnumAppType.Console))
				LoginUserConsole(false);
			else if (AppType.Equals(TgEnumAppType.Desktop))
				LoginUserDesktop(false);
			//int backupId = tgDownloadSettings.SourceFirstId;
			CreateDestDirectoryIfNotExists(tgDownloadSettings);
			while (tgDownloadSettings.SourceFirstId <= tgDownloadSettings.SourceLastId)
			{
				TryCatchAction(() =>
				{
					bool isAccessToMessages = false;
					if (channel is not null)
						isAccessToMessages = GetTaskResult(() => Client.Channels_ReadMessageContents(channel));
					else if (chatBase is not null)
						isAccessToMessages = true;
					if (isAccessToMessages && Client is not null)
					{
						Messages_MessagesBase messages = channel is not null
							? GetTaskResult(() => 
								Client.Channels_GetMessages(channel, tgDownloadSettings.SourceFirstId))
							: GetTaskResult(() => 
								Client.GetMessages(chatBase, tgDownloadSettings.SourceFirstId));
						foreach (MessageBase message in messages.Messages)
						{
							// Check message exists.
							if (message.Date > DateTime.MinValue)
							{
								DownloadData(tgDownloadSettings, message, storeMessage, storeDocument, findExistsMessage);
							}
							else
							{
								UpdateStateProgress("Message is not exists!");
								UpdateState($"Message {message.ID} is not exists in {tgDownloadSettings.SourceId}!");
								UpdateStateSource(tgDownloadSettings.SourceId, message.ID, 
									$"Message {message.ID} is not exists in {tgDownloadSettings.SourceId}!");
							}
						}
					}
				});
				tgDownloadSettings.SourceFirstId++;
			}
			tgDownloadSettings.SourceFirstId = tgDownloadSettings.SourceLastId;
		});
	}

	private void CreateDestDirectoryIfNotExists(TgDownloadSettingsModel tgDownloadSettings)
	{
		try
		{
			Directory.CreateDirectory(tgDownloadSettings.DestDirectory);
		}
		catch (Exception ex)
		{
			SetClientException(ex);
		}
	}

	private void DownloadData(TgDownloadSettingsModel tgDownloadSettings, MessageBase messageBase,
		Action<int, long, DateTime, TgEnumMessageType, long, string> storeMessage,
		Action<long, long, long, string, long, long> storeDocument, Func<long, long, bool> findExistsMessage)
	{
		if (messageBase is not TL.Message message)
		{
			UpdateStateProgress("Empty message");
			UpdateStateSource(tgDownloadSettings.SourceId, messageBase.ID, "Empty message");
			return;
		}

		TryCatchAction(() =>
		{
			lock (Locker)
			{
				// Store message.
				bool isExistsMessage = findExistsMessage(tgDownloadSettings.SourceFirstId, tgDownloadSettings.SourceId);
				if ((isExistsMessage && tgDownloadSettings.IsRewriteMessages) || !isExistsMessage)
					storeMessage(message.ID, tgDownloadSettings.SourceId, message.Date, TgEnumMessageType.Message, 0, message.message);
				// Parse documents and photos.
				if ((message.flags & TL.Message.Flags.has_media) is not 0)
				{
					if (message.media is MessageMediaDocument mediaDocument)
					{
						if ((mediaDocument.flags & MessageMediaDocument.Flags.has_document) is not 0)
						{
							if (mediaDocument.document is Document document)
							{
								DownloadDataCore(tgDownloadSettings, messageBase, document, null,
									storeMessage, storeDocument, findExistsMessage);
							}
						}
					}
					else if (message.media is MessageMediaPhoto { photo: Photo photo })
					{
						DownloadDataCore(tgDownloadSettings, messageBase, null, photo,
							storeMessage, storeDocument, findExistsMessage);
					}
				}
				UpdateStateProgress("Read the message");
				UpdateStateSource(tgDownloadSettings.SourceId, message.ID, "Read the message");
			}
		});
	}

	private (string Remote, long Size, DateTime DtCreate, string Local, string Join)[] 
		GetFiles(Document? document, Photo? photo)
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
			if (!filter.IsEnabled) continue;
			switch (filter.FilterType)
			{
				case TgEnumFilterType.SingleName:
					if (string.IsNullOrEmpty(fileName)) continue;
					if (!TgDataFormatUtils.CheckFileAtMask(fileName, filter.Mask)) return false;
					break;
				case TgEnumFilterType.SingleExtension:
					if (string.IsNullOrEmpty(extensionName)) continue;
					if (!TgDataFormatUtils.CheckFileAtMask(extensionName, filter.Mask)) return false;
					break;
				case TgEnumFilterType.MultiName:
					if (string.IsNullOrEmpty(fileName)) continue;
					bool isMultiName = false;
					foreach (string mask in filter.Mask.Split(','))
						if (TgDataFormatUtils.CheckFileAtMask(fileName, mask.TrimStart().TrimEnd())) isMultiName = true;
					if (!isMultiName) return false;
					break;
				case TgEnumFilterType.MultiExtension:
					if (string.IsNullOrEmpty(extensionName)) continue;
					bool isMultiExtension = false;
					foreach (string mask in filter.Mask.Split(','))
						if (TgDataFormatUtils.CheckFileAtMask(extensionName, mask.TrimStart().TrimEnd())) isMultiExtension = true;
					if (!isMultiExtension) return false;
					break;
				case TgEnumFilterType.MinSize:
					if (size < filter.SizeAtBytes) return false;
					break;
				case TgEnumFilterType.MaxSize:
					if (size > filter.SizeAtBytes) return false;
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

	private void DeleteExistsFiles(TgDownloadSettingsModel tgDownloadSettings,
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
		});
	}

	private void DownloadDataCore(TgDownloadSettingsModel tgDownloadSettings,
		MessageBase messageBase, Document? document, Photo? photo,
		Action<int, long, DateTime, TgEnumMessageType, long, string> storeMessage,
		Action<long, long, long, string, long, long> storeDocument,
		Func<long, long, bool> findExistsMessage)
	{
		(string Remote, long Size, DateTime DtCreate, string Local, string Join)[] files = GetFiles(document, photo);
		if (Equals(files, Array.Empty<(string Remote, long Size, DateTime DtCreate, string Local, string Join)>())) return;
		SetFilesLocalNames(tgDownloadSettings, messageBase, ref files);
		long accessHash = document?.access_hash ?? photo?.access_hash ?? 0;

		// Delete files.
		DeleteExistsFiles(tgDownloadSettings, files);

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
						GetTaskResult(() => Client.DownloadFileAsync(document, localFileStream));
						storeDocument(document.ID, tgDownloadSettings.SourceId, messageBase.ID, fileName, files[i].Size, accessHash);
					}
					else if (photo is { })
					{
						//WClient.DownloadFileAsync(photo, localFileStream, new PhotoSize
						//    { h = photo.sizes[i].Height, w = photo.sizes[i].Width, 
						//        size = photo.sizes[i].FileSize, type = photo.sizes[i].Type })
						GetTaskResult(() => Client.DownloadFileAsync(photo, localFileStream));
					}
				}
				localFileStream.Close();
			}
			// Store message.
			bool isExistsMessage = findExistsMessage(tgDownloadSettings.SourceFirstId, tgDownloadSettings.SourceId);
			if (document is not null && ((isExistsMessage && tgDownloadSettings.IsRewriteMessages) || !isExistsMessage))
			{
				storeMessage(messageBase.ID, tgDownloadSettings.SourceId, files[i].DtCreate,
					TgEnumMessageType.Document, files[i].Size, files[i].Remote);
			}
			else if (photo is not null && ((isExistsMessage && tgDownloadSettings.IsRewriteMessages) || !isExistsMessage))
			{
				storeMessage(messageBase.ID, tgDownloadSettings.SourceId, files[i].DtCreate,
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

	private long GetPeerId(string userName) => GetTaskResult(() => Client.Contacts_ResolveUsername(userName)).peer.ID;

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

	public void LoginUserConsole(bool isProxyUpdate, Action? afterConnect = null)
	{
		ClientException = new();
		if (Client is null) return;

		try
		{
			Me = Client.LoginUserIfNeeded().GetAwaiter().GetResult();
			TgQuery = string.Empty;
		}
		catch (Exception ex)
		{
			SetClientException(ex);
			Me = null;
		}
		finally
		{
			if (isProxyUpdate && IsReady)
			{
				TgSqlTableAppModel app = ContextManager.ContextTableApps.GetCurrentItem();
				app.ProxyUid = ContextManager.ContextTableApps.GetCurrentProxy().Uid;
				ContextManager.ContextTableApps.AddOrUpdateItem(app);
			}
			afterConnect?.Invoke();
		};
	}

	public void LoginUserDesktop(bool isProxyUpdate, Action? afterConnect = null)
	{
		ClientException = new();
		if (Client is null) return;

		try
		{
			Me = GetTaskResult(() => Client.LoginUserIfNeeded());
			TgQuery = string.Empty;
		}
		catch (Exception ex)
		{
			SetClientException(ex);
			Me = null;
		}
		finally
		{
			if (isProxyUpdate && IsReady)
			{
				TgSqlTableAppModel app = ContextManager.ContextTableApps.GetCurrentItem();
				app.ProxyUid = ContextManager.ContextTableApps.GetCurrentProxy().Uid;
				ContextManager.ContextTableApps.AddOrUpdateItem(app);
			}

			afterConnect?.Invoke();
		}
	}

	public void Disconnect()
	{
		if (Client is null) return;
		Client.OnUpdate -= Client_OnUpdateAsync;
		Client.Dispose();
		ClientException = new();
		Me = null;
	}

	private void SetClientException(Exception ex)
	{
		ClientException.Set(ex);
		UpdateException(ClientException.Message);
	}

	private void SetProxyException(Exception ex)
	{
		ProxyException.Set(ex);
		UpdateException(ProxyException.Message);
	}

	#endregion

	#region Public and private methods

	private void TryCatchAction(Action action)
	{
		try
		{
			action();
		}
		catch (Exception ex)
		{
			SetClientException(ex);
			if (ClientException.Message.Contains("You must connect to Telegram first"))
			{
				LoginUserConsole(false);
				UpdateState("Reconnect client ...");
			}
		}
	}

	private void TryCatchActionDesktop(Action action)
	{
		try
		{
			action();
		}
		catch (Exception ex)
		{
			SetClientException(ex);
			// CHANNEL_INVALID | BadMsgNotification 48
			if (ClientException.Message.Contains("You must connect to Telegram first"))
			{
				UpdateState("Reconnect client ...");
				LoginUserDesktop(false);
			}
		}
	}

	#endregion
}