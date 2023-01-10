// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageCore.Models.Messages;

[DebuggerDisplay("{nameof(TableMessageModel)} | {Id} | {SourceId} | {Message}")]
[Table("MESSAGES")]
public class TableMessageModel : TableBase
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
    [Column("DT_CREATE")]
    [DefaultValue(null)]
    public DateTime DtCreate { get; set; }
    //private long DtCreateLong { get; set; }
    //public DateTime DtCreate { get => DtCreateLong.CastAsDt(); set => DtCreateLong = value.CastAsLong(); }
    [Column("TYPE")]
    [DefaultValue("")]
    public string Type { get; set; }
    [Column("SIZE")]
    [DefaultValue(0)]
    public long Size { get; set; }
    [Column("MESSAGE")]
    [DefaultValue("")]
    public string Message { get; set; }

    public TableMessageModel()
    {
        Id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
        SourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
        DtCreate = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(DtCreate));
        Message = this.GetPropertyDefaultValueAsString(nameof(Message));
        Type = this.GetPropertyDefaultValueAsString(nameof(Type));
        Size = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Size));
    }

    public TableMessageModel(long id, long sourceId, DateTime dtCreate, string message, string type, long size)
    {
        Id = id;
        SourceId = sourceId;
        DtCreate = dtCreate;
        Message = message;
        Type = type;
        Size = size;
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected TableMessageModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Id = info.GetInt64(nameof(Id));
        SourceId = info.GetInt64(nameof(SourceId));
        DtCreate = info.GetDateTime(nameof(DtCreate));
        Message = info.GetString(nameof(Message)) ?? this.GetPropertyDefaultValueAsString(nameof(Message));
        Type = info.GetString(nameof(Type)) ?? this.GetPropertyDefaultValueAsString(nameof(Type));
        Size = info.GetInt64(nameof(Size));
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
        info.AddValue(nameof(DtCreate), DtCreate);
        info.AddValue(nameof(Message), Message);
        info.AddValue(nameof(Type), Type);
        info.AddValue(nameof(Size), Size);
    }

    #endregion
}