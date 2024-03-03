// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Messages;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgSqlConstants.TableMessages)]
public sealed class TgEfMessageEntity : TgEfEntityBase
{
    #region Public and private fields, properties, constructor

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnSourceId)]
    [SQLite.Indexed]
    public long SourceId { get; set; }

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnId)]
    [SQLite.Indexed]
    public long Id { get; set; }

	[DefaultValue("0001-01-01 00:00:00")]
	[ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnDtCreated)]
    [SQLite.Indexed]
    public DateTime DtCreated { get; set; }

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnType)]
    [SQLite.Indexed]
    public TgEnumMessageType Type { get; set; }

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnSize)]
    [SQLite.Indexed]
    public long Size { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnMessage)]
    [SQLite.Indexed]
    public string Message { get; set; }

    public TgEfMessageEntity() : base()
    {
	    SourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
	    Id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
	    DtCreated = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(DtCreated));
	    Type = this.GetPropertyDefaultValueAsGeneric<TgEnumMessageType>(nameof(Type));
	    Size = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Size));
	    Message = this.GetPropertyDefaultValue(nameof(Message));
	}

    #endregion

    #region Public and private methods

    public override string ToDebugString() =>
        $"{base.ToDebugString()} | {TgCommonUtils.GetIsExists(IsExists)} | {Uid} | {SourceId} | {Id} | {Type} | {Size} | {Message}";

    #endregion
}