// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Diagnostics;

namespace TgStorageCore.Models;

[DebuggerDisplay("Type = {nameof(TableMessageModel)}")]
[Table("MESSAGES")]
public class TableMessageModel
{
    #region Public and private fields, properties, constructor

    //[PrimaryKey]
    [Indexed]
    [Column("ID")]
    public long Id { get; set; }
    //[PrimaryKey]
    [Indexed]
    [Column("SOURCE_ID")]
    public long SourceId { get; set; }
    [Column("MESSAGE")]
    public string Message { get; set; }
    [Indexed]
    [Column("FILE_NAME")]
    public string FileName { get; set; }
    [Column("FILE_SIZE")]
    public long FileSize { get; set; }
    [Column("ACCESS_HASH")]
    public long AccessHash { get; set; }

    public TableMessageModel()
    {
        Id = 0;
        SourceId = 0;
        Message = string.Empty;
        FileName = string.Empty;
        FileSize = 0;
        AccessHash = 0;
    }

    public TableMessageModel(long id, long sourceId, string message, string fileName, long fileSize, long accessHash)
    {
        Id = id;
        SourceId = sourceId;
        Message = message;
        FileName = fileName;
        FileSize = fileSize;
        AccessHash = accessHash;
    }

    #endregion
}