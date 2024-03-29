﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Apps;

/// <summary>
/// View for TgSqlTableSourceModel.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSqlTableAppViewModel : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	public TgSqlTableAppModel App { get; set; }

	public TgSqlTableAppViewModel(TgSqlTableAppModel app)
	{
		App = app;
	}

	#endregion

	#region Public and private methods

	public override string ToString() => $"{App}";
	public override string ToDebugString() => $"{base.ToDebugString()} | {App}";

	#endregion
}