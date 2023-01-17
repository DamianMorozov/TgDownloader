// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgCore.Utils;

namespace TgStorageCore.Models.Documents;

[DebuggerDisplay("{nameof(TableDocumentModel)} | {Id} | {SourceId} | {MessageId} | {FileName} | {FileSize} | {AccessHash}")]
[Table("DOCUMENTS")]
public class SqlTableDocumentModel : SqlTableBase
{
    #region Public and private fields, properties, constructor

    //[PrimaryKey]
    [Indexed]
    [Column("ID")]
    [DefaultValue(0)]
    public long Id { get; set; }
    //[PrimaryKey]
    [Indexed]
    [Column("SOURCE_ID")]
    [DefaultValue(0)]
    public long SourceId { get; set; }
    //[PrimaryKey]
    [Indexed]
    [Column("MESSAGE_ID")]
    [DefaultValue("")]
    public long MessageId { get; set; }
    [Indexed]
    [Column("FILE_NAME")]
    [DefaultValue("")]
    public string FileName { get; set; }
    [Column("FILE_SIZE")]
    [DefaultValue(0)]
    public long FileSize { get; set; }
    [Column("ACCESS_HASH")]
    [DefaultValue(0)]
    public long AccessHash { get; set; }

    public SqlTableDocumentModel()
    {
        Id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
        SourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
        MessageId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(MessageId));
        FileName = this.GetPropertyDefaultValueAsString(nameof(FileName));
        FileSize = this.GetPropertyDefaultValueAsGeneric<long>(nameof(FileSize));
        AccessHash = this.GetPropertyDefaultValueAsGeneric<long>(nameof(AccessHash));
    }

    public SqlTableDocumentModel(long id, long sourceId, long messageId, string fileName, long fileSize, long accessHash)
    {
        Id = id;
        SourceId = sourceId;
        MessageId = messageId;
        FileName = fileName;
        FileSize = fileSize;
        AccessHash = accessHash;
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected SqlTableDocumentModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Id = info.GetInt64(nameof(Id));
        SourceId = info.GetInt64(nameof(SourceId));
        MessageId = info.GetInt64(nameof(MessageId));
        FileName = info.GetString(nameof(FileName)) ?? this.GetPropertyDefaultValueAsString(nameof(FileName));
        FileSize = info.GetInt64(nameof(FileSize));
        AccessHash = info.GetInt64(nameof(AccessHash));
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public new void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Id), Id);
        info.AddValue(nameof(SourceId), SourceId);
        info.AddValue(nameof(MessageId), MessageId);
        info.AddValue(nameof(FileName), FileName);
        info.AddValue(nameof(FileSize), FileSize);
        info.AddValue(nameof(AccessHash), AccessHash);
    }

    #endregion
}