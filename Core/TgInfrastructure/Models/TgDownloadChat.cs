// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgInfrastructure.Models;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgDownloadChat : ObservableRecipient
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private TL.ChatBase? _base;

	#endregion

	#region Public and private methods

	public string ToDebugString() => $"{(Base is not null ? Base.ID : string.Empty)} | {GetUserName()}";

	public string GetUserName()
	{
		if (Base is not null)
			return !string.IsNullOrEmpty(Base.MainUsername) ? Base.MainUsername : Base.Title;
		return string.Empty;
	}

	#endregion
}