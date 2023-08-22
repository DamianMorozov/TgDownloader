// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Messages;

/// <summary>
/// View for TgSqlTableSourceModel.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSqlTableMessageViewModel : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	public TgSqlTableMessageModel Message { get; set; }

	public Action<TgSqlTableMessageViewModel> UpdateAction { get; set; }

	public TgSqlTableMessageViewModel()
	{
		Message = new();
		UpdateAction = _ => { };
	}

	public TgSqlTableMessageViewModel(TgSqlTableMessageModel message, Action<TgSqlTableMessageViewModel> updateAction)
	{
		Message = message;
		UpdateAction = updateAction;
	}

	#endregion

	#region Public and private methods

	public override string ToString() => $"{Message}";

	#endregion
}