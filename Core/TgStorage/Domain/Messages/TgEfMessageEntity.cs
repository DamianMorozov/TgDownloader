// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Messages;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgEfConstants.TableMessages)]
[Index(nameof(SourceId))]
[Index(nameof(Id))]
[Index(nameof(SourceId), nameof(Id), IsUnique = true)]
[Index(nameof(DtCreated))]
[Index(nameof(Type))]
[Index(nameof(Size))]
[Index(nameof(Message))]
public sealed class TgEfMessageEntity : TgEfEntityBase
{
    #region Public and private fields, properties, constructor

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnSourceId)]
    public long SourceId { get; set; }
	
    public TgEfSourceEntity? Source { get; set; }

	[DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnId)]
    public long Id { get; set; }

	[DefaultValue("0001-01-01 00:00:00")]
	[ConcurrencyCheck]
    [Column(TgEfConstants.ColumnDtCreated)]
    public DateTime DtCreated { get; set; }

    [DefaultValue(TgEnumMessageType.Message)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnType)]
    public TgEnumMessageType Type { get; set; }

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnSize)]
    public long Size { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnMessage)]
    public string Message { get; set; } = default!;

    public TgEfMessageEntity() : base()
    {
        Default();
	}

    #endregion

    #region Public and private methods

    public override string ToDebugString() =>
        $"{TgEfConstants.TableMessages} | {base.ToDebugString()} | {Uid} | {SourceId} | {Id} | {Type} | {Size} | {Message}";

    public override void Default()
    {
	    base.Default();
	    SourceId = this.GetDefaultPropertyLong(nameof(SourceId));
	    Id = this.GetDefaultPropertyLong(nameof(Id));
	    DtCreated = this.GetDefaultPropertyDateTime(nameof(DtCreated));
		Type = this.GetDefaultPropertyGeneric<TgEnumMessageType>(nameof(Type));
	    Size = this.GetDefaultPropertyLong(nameof(Size));
	    Message = this.GetDefaultPropertyString(nameof(Message));
	}

    public override void Fill(object item)
    {
		if (item is not TgEfMessageEntity message)
			throw new ArgumentException($"The {nameof(item)} is not {nameof(TgEfMessageEntity)}!");

		SourceId = message.SourceId;
		Id = message.Id;
		DtCreated = message.DtCreated > DateTime.MinValue ? message.DtCreated : DateTime.Now;
		Type = message.Type;
		Size = message.Size;
		Message = message.Message;
	}

    public override void Backup(object item)
    {
	    Fill(item);
	    base.Backup(item);
    }

	#endregion
}