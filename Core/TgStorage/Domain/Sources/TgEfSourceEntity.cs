// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Sources;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgEfConstants.TableSources)]
[Index(nameof(Uid), IsUnique = true)]
[Index(nameof(Id), IsUnique = true)]
[Index(nameof(UserName))]
[Index(nameof(Title))]
[Index(nameof(Count))]
[Index(nameof(Directory))]
[Index(nameof(FirstId))]
[Index(nameof(IsAutoUpdate))]
[Index(nameof(DtChanged))]
public sealed class TgEfSourceEntity : ITgDbEntity, ITgDbFillEntity<TgEfSourceEntity>
{
	#region Public and private fields, properties, constructor

	[DefaultValue("00000000-0000-0000-0000-000000000000")]
	[Key]
	//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Required]
	[Column(TgEfConstants.ColumnUid, TypeName = "CHAR(36)")]
	[SQLite.Collation("NOCASE")]
	public Guid Uid { get; set; }

	[DefaultValue(1)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnId, TypeName = "INT(20)")]
	public long Id { get; set; }

	[DefaultValue("UserName")]
    [ConcurrencyCheck]
    [MaxLength(256)]
    [Column(TgEfConstants.ColumnUserName, TypeName = "NVARCHAR(128)")]
    public string? UserName { get; set; }

    [DefaultValue("Title")]
    [ConcurrencyCheck]
    [MaxLength(1024)]
    [Column(TgEfConstants.ColumnTitle, TypeName = "NVARCHAR(256)")]
    public string? Title { get; set; }

    [DefaultValue("About")]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnAbout, TypeName = "NVARCHAR(1024)")]
    public string? About { get; set; }

    [DefaultValue(1)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnCount, TypeName = "INT")]
    public int Count { get; set; }

    [DefaultValue("")]
    [ConcurrencyCheck]
    [MaxLength(1024)]
    [Column(TgEfConstants.ColumnDirectory, TypeName = "NVARCHAR(256)")]
    public string? Directory { get; set; }

    [DefaultValue(1)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnFirstId, TypeName = "INT")]
    public int FirstId { get; set; }

    [DefaultValue(false)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnIsAutoUpdate, TypeName = "BIT")]
    public bool IsAutoUpdate { get; set; }

    [DefaultValue("0001-01-01 00:00:00")]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnDtChanged, TypeName = "DATETIME")]
	public DateTime DtChanged { get; set; }

    public ICollection<TgEfDocumentEntity> Documents { get; set; } = default!;

	public ICollection<TgEfMessageEntity> Messages { get; set; } = default!;

    public TgEfSourceEntity() : base()
    {
        Default();
    }

    #endregion

    #region Public and private methods

    public string ToDebugString() =>
        $"{TgEfConstants.TableSources} | {Uid} | {Id} | {(IsAutoUpdate ? "a" : " ")} | {(FirstId == Count ? "v" : "x")} | {UserName} | " +
        $"{(string.IsNullOrEmpty(Title) ? string.Empty : TgDataFormatUtils.TrimStringEnd(Title))} | {FirstId} {TgLocaleHelper.Instance.From} {Count} {TgLocaleHelper.Instance.Messages}";

    public void Default()
    {
		Uid = this.GetDefaultPropertyGuid(nameof(Uid));
		Id = this.GetDefaultPropertyLong(nameof(Id));
	    UserName = this.GetDefaultPropertyString(nameof(UserName));
	    Title = this.GetDefaultPropertyString(nameof(Title));
	    About = this.GetDefaultPropertyString(nameof(About));
	    Count = this.GetDefaultPropertyInt(nameof(Count));
	    Directory = this.GetDefaultPropertyString(nameof(Directory));
	    FirstId = this.GetDefaultPropertyInt(nameof(FirstId));
	    IsAutoUpdate = this.GetDefaultPropertyBool(nameof(IsAutoUpdate));
	    DtChanged = this.GetDefaultPropertyDateTime(nameof(DtChanged));
	    Documents = new List<TgEfDocumentEntity>();
        Messages = new List<TgEfMessageEntity>();
    }

    public TgEfSourceEntity Fill(TgEfSourceEntity item, bool isUidCopy)
    {
		if (isUidCopy)
			Uid = item.Uid;
		DtChanged = item.DtChanged > DateTime.MinValue ? item.DtChanged : DateTime.Now;
		Id = item.Id;
		FirstId = item.FirstId;
		IsAutoUpdate = item.IsAutoUpdate;
		UserName = string.IsNullOrEmpty(item.UserName) ? "" : item.UserName;
		Title = item.Title;
		About = item.About;
		Count = item.Count;
		Directory = item.Directory;
        return this;
	}

	public string ToConsoleStringShort() =>
	    $"{GetPercentCountString()} | {(IsAutoUpdate ? "a | " : "")} | {Id} | " +
	    $"{(string.IsNullOrEmpty(UserName) ? "" : TgDataFormatUtils.GetFormatString(UserName, 30))} | " +
	    $"{(string.IsNullOrEmpty(Title) ? "" : TgDataFormatUtils.GetFormatString(Title, 30))} | " +
	    $"{FirstId} {TgLocaleHelper.Instance.From} {Count} {TgLocaleHelper.Instance.Messages}";

    public string ToConsoleString() =>
	    $"{GetPercentCountString()} | {(IsAutoUpdate ? "a" : " ")} | {Id} | " +
	    $"{(UserName is not null ? TgDataFormatUtils.GetFormatString(UserName, 30) : string.Empty)} | " +
	    $"{(Title is not null ? TgDataFormatUtils.GetFormatString(Title, 30) : string.Empty)} | " +
	    $"{FirstId} {TgLocaleHelper.Instance.From} {Count} {TgLocaleHelper.Instance.Messages}";

	public string GetPercentCountString()
    {
	    float percent = Count <= FirstId ? 100 : FirstId > 1 ? (float)FirstId * 100 / Count : 0;
	    if (IsPercentCountAll())
		    return "100.00 %";
	    return percent > 9 ? $" {percent:00.00} %" : $"  {percent:0.00} %";
    }

    public bool IsPercentCountAll() => Count <= FirstId;

	#endregion
}