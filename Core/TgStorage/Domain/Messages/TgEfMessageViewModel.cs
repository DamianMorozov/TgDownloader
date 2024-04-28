// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Messages;

/// <summary> View for TgSqlTableSourceModel </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgEfMessageViewModel(TgEfMessageEntity message, Action<TgEfMessageViewModel> updateAction) : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	public TgEfMessageEntity Message { get; set; } = message;

	public Action<TgEfMessageViewModel> UpdateAction { get; set; } = updateAction;

	public TgEfMessageViewModel() : this(new(), _ => { }) { }

	#endregion

	#region Public and private methods

	public override string ToString() => $"{Message}";

	#endregion
}