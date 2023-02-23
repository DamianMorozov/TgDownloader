﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Messages;

[DebuggerDisplay("{nameof(TableMessageModel)} | {Id} | {SourceId} | {Message}")]
[SQLite.Table("MESSAGES")]
public class SqlTableMessageModel : SqlTableBase
{
    #region Public and private fields, properties, constructor

    [SQLite.Indexed]
    [SQLite.Column("ID")]
    [DefaultValue(0)]
    public long Id { get; set; }
    
    [SQLite.Indexed]
    [SQLite.Column("SOURCE_ID")]
    [DefaultValue(0)]
    public long SourceId { get; set; }
    
    [SQLite.Column("DT_CREATE")]
    [DefaultValue(null)]
    public DateTime DtCreate { get; set; }
    
    [SQLite.Column("TYPE")]
    [DefaultValue("")]
    public string Type { get; set; }

    [SQLite.Column("SIZE")]
    [DefaultValue(0)]
    public long Size { get; set; }
    
    [SQLite.Column("MESSAGE")]
    [DefaultValue("")]
    public string Message { get; set; }

    public SqlTableMessageModel()
    {
        Id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
        SourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
        DtCreate = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(DtCreate));
        Message = this.GetPropertyDefaultValue(nameof(Message));
        Type = this.GetPropertyDefaultValue(nameof(Type));
        Size = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Size));
    }

    public SqlTableMessageModel(long id, long sourceId, DateTime dtCreate, string message, string type, long size)
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
    protected SqlTableMessageModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Id = info.GetInt64(nameof(Id));
        SourceId = info.GetInt64(nameof(SourceId));
        DtCreate = info.GetDateTime(nameof(DtCreate));
        Message = info.GetString(nameof(Message)) ?? this.GetPropertyDefaultValue(nameof(Message));
        Type = info.GetString(nameof(Type)) ?? this.GetPropertyDefaultValue(nameof(Type));
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