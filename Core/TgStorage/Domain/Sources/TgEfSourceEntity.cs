// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Sources;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgEfConstants.TableSources)]
[Index(nameof(Id), IsUnique = true)]
[Index(nameof(UserName))]
[Index(nameof(Title))]
[Index(nameof(Count))]
[Index(nameof(Directory))]
[Index(nameof(FirstId))]
[Index(nameof(IsAutoUpdate))]
[Index(nameof(DtChanged))]
public sealed partial class TgEfSourceEntity : TgEfEntityBase
{
    #region Public and private fields, properties, constructor

    [DefaultValue(1)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnId, TypeName = "INT(20)")]
    public long Id{ get; set; }

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

    public override string ToDebugString() =>
        $"{TgEfConstants.TableSources} | {base.ToDebugString()} | {Uid} | {Id} | {(IsAutoUpdate ? "a" : " ")} | {(FirstId == Count ? "v" : "x")} | {UserName} | " +
        $"{(string.IsNullOrEmpty(Title) ? string.Empty : TgDataFormatUtils.TrimStringEnd(Title))} | {FirstId} {TgLocaleHelper.Instance.From} {Count} {TgLocaleHelper.Instance.Messages}";

    public override void Default()
    {
	    base.Default();
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

    public override void Fill(object item)
    {
		if (item is not TgEfSourceEntity source)
			throw new ArgumentException($"The {nameof(item)} is not {nameof(TgEfSourceEntity)}!");

		DtChanged = source.DtChanged > DateTime.MinValue ? source.DtChanged : DateTime.Now;
		Id = source.Id;
		FirstId = source.FirstId;
		IsAutoUpdate = source.IsAutoUpdate;
		UserName = string.IsNullOrEmpty(source.UserName) ? "" : source.UserName;
		Title = source.Title;
		About = source.About;
		Count = source.Count;
		Directory = source.Directory;
	}

    public override void Backup(object item)
    {
	    Fill(item);
	    base.Backup(item);
    }

    public string ToConsoleStringShort() =>
	    $"{GetPercentCountString()} | {(IsAutoUpdate ? "a | " : "")} | {Id} | " +
	    $"{(string.IsNullOrEmpty(UserName) ? "" : TgDataFormatUtils.GetFormatString(UserName, 30))} | " +
	    $"{(string.IsNullOrEmpty(Title) ? "" : TgDataFormatUtils.GetFormatString(Title, 30))} | " +
	    $"{FirstId} {TgLocaleHelper.Instance.From} {Count} {TgLocaleHelper.Instance.Messages}";

    public string ToConsoleString() =>
	    $"{GetPercentCountString()} | {(IsAutoUpdate ? "a" : " ")} | {Id} | " +
	    $"{TgDataFormatUtils.GetFormatString(UserName, 30)} | " +
	    $"{TgDataFormatUtils.GetFormatString(Title, 30)} | " +
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