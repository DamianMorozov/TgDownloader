// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Apps;

/// <summary> View-model for TgSqlTableSourceModel </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgEfAppViewModel(TgEfAppEntity app) : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private TgEfAppEntity _app = app;

	#endregion

	#region Public and private methods

	public override string ToString() => $"{App}";
	public override string ToDebugString() => $"{base.ToDebugString()} | {App}";

	#endregion
}