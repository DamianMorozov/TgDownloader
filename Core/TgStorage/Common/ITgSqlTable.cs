﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

/// <summary>
/// SQL table interface.
/// </summary>
public interface ITgSqlTable : ITgCommon
{
	#region Public and private fields, properties, constructor

	public Guid Uid { get; set; }
	public bool IsExists { get; }
	public bool IsNotExists { get; }

    #endregion
}