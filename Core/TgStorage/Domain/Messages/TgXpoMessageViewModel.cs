// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Messages;

/// <summary>
/// View for TgSqlTableSourceModel.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgXpoMessageViewModel : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	public TgXpoMessageEntity Message { get; set; }

	public Action<TgXpoMessageViewModel> UpdateAction { get; set; }

	public TgXpoMessageViewModel()
	{
		Message = new();
		UpdateAction = _ => { };
	}

	public TgXpoMessageViewModel(TgXpoMessageEntity message, Action<TgXpoMessageViewModel> updateAction)
	{
		Message = message;
		UpdateAction = updateAction;
	}

	#endregion

	#region Public and private methods

	public override string ToString() => $"{Message}";

	#endregion
}