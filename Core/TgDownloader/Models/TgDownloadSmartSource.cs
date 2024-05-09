// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloader.Models;

[DebuggerDisplay("{ToDebugString()}")]
public class TgDownloadSmartSource
{
	#region Public and private fields, properties, constructor

	public TlChatBase? ChatBase { get; set; }

	#endregion

	#region Public and private methods

	public string ToDebugString() => $"{(ChatBase is not null ? ChatBase.ID : string.Empty)} | {GetUserName()}";

	public string GetUserName()
	{
		if (ChatBase is not null)
			return !string.IsNullOrEmpty(ChatBase.MainUsername) ? ChatBase.MainUsername : ChatBase.Title;
		return string.Empty;
	}

	#endregion
}