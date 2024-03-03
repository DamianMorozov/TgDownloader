// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Documents;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgSqlConstants.TableDocuments)]
public sealed class TgEfDocumentEntity : TgEfEntityBase
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

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnMessageId)]
    [SQLite.Indexed]
    public long MessageId { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(256)]
    [Column(TgSqlConstants.ColumnFileName)]
    [SQLite.Indexed]
    public string FileName { get; set; }

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnFileSize)]
    [SQLite.Indexed]
    public long FileSize { get; set; }

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnAccessHash)]
    [SQLite.Indexed]
    public long AccessHash { get; set; }

    public TgEfDocumentEntity() : base()
    {
	    SourceId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(SourceId));
		Id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
		MessageId = this.GetPropertyDefaultValueAsGeneric<long>(nameof(MessageId));
		FileName = this.GetPropertyDefaultValue(nameof(FileName));
		FileSize = this.GetPropertyDefaultValueAsGeneric<long>(nameof(FileSize));
		AccessHash = this.GetPropertyDefaultValueAsGeneric<long>(nameof(AccessHash));
	}

    #endregion

    #region Public and private methods

    public override string ToDebugString() =>
        $"{base.ToDebugString()} | {TgCommonUtils.GetIsExists(IsExists)} | {Uid} | {SourceId} | {Id} | {MessageId} | {FileName} | {FileSize} | {AccessHash}";

    #endregion
}