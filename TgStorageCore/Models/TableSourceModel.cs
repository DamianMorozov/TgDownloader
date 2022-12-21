// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Diagnostics;

namespace TgStorageCore.Models;

[DebuggerDisplay("Type = {nameof(TableSourcesModel)}")]
[Table("SOURCES")]
public class TableSourceModel
{
    #region Public and private fields, properties, constructor

    [PrimaryKey]
    [Column("ID")]
    public long Id { get; set; }
    [Indexed]
    [Column("USER_NAME")]
    public string UserName { get; set; }

    public TableSourceModel()
    {
        Id = 0;
        UserName = string.Empty;
    }

    public TableSourceModel(long id, string userName)
    {
        Id = id;
        UserName = userName;
    }

    #endregion
}