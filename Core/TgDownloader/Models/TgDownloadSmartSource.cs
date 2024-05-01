// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloader.Models;

[DebuggerDisplay("{ToDebugString()}")]
public class TgDownloadSmartSource
{
	#region Public and private fields, properties, constructor

	private ChatBase? _chatBase;
	public ChatBase? ChatBase
	{
		get => _chatBase;
		set
		{
			_chatBase = value;
			SourceType = _chatBase is not null ? TgEnumSourceType.ChatBase : TgEnumSourceType.Default;
		}
	}
	private Channel? _channel;
	public Channel? Channel
	{
		get => _channel;
		set
		{
			_channel = value;
			SourceType = _channel is not null ? TgEnumSourceType.Channel : TgEnumSourceType.Default;
		}
	}
	public TgEnumSourceType SourceType { get; set; }

	#endregion

	#region Public and private methods

	public string ToDebugString() => $"{ChatBase.ID} | {SourceType} | {GetUserName()}";

	public string GetUserName()
	{
		if (ChatBase is not null)
			return !string.IsNullOrEmpty(ChatBase.MainUsername) ? ChatBase.MainUsername : ChatBase.Title;
		if (Channel is not null)
			return !string.IsNullOrEmpty(Channel.MainUsername) ? Channel.MainUsername : Channel.Title;
		return string.Empty;
	}

	#endregion
}