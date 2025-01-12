// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Bots;

/// <summary> Bot entity </summary>
[DebuggerDisplay("{ToDebugString()}")]
[Index(nameof(Uid), IsUnique = true)]
[Index(nameof(BotToken), IsUnique = true)]
public sealed class TgEfBotEntity : ITgDbEntity, ITgDbFillEntity<TgEfBotEntity>
{
	#region Public and private fields, properties, constructor

	[DefaultValue("00000000-0000-0000-0000-000000000000")]
	[Key]
	[Required]
	[Column(TgEfConstants.ColumnUid, TypeName = "CHAR(36)")]
	[SQLite.Collation("NOCASE")]
	public Guid Uid { get; set; }

	[Timestamp]
	[Column(TgEfConstants.ColumnRowVersion)]
	public byte[]? RowVersion { get; set; }

	[DefaultValue("")]
	[ConcurrencyCheck]
	[MaxLength(50)]
	[Column(TgEfConstants.ColumnBotToken, TypeName = "NVARCHAR(50)")]
	public string BotToken { get; set; } = default!;

	public TgEfBotEntity() : base()
    {
	    Default();
    }

	#endregion

	#region Public and private methods

	public string ToDebugString() => TgObjectUtils.ToDebugString(this);

	public void Default()
    {
		Uid = this.GetDefaultPropertyGuid(nameof(Uid));
		BotToken = this.GetDefaultPropertyString(nameof(BotToken));
    }

	public TgEfBotEntity Fill(TgEfBotEntity item, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = item.Uid;
		BotToken = item.BotToken;
		return this;
    }

	#endregion
}