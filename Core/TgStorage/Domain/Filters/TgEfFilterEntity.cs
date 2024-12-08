// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Filters;

[DebuggerDisplay("{ToDebugString()}")]
[Index(nameof(Uid), IsUnique = true)]
[Index(nameof(IsEnabled))]
[Index(nameof(FilterType))]
[Index(nameof(Name))]
[Index(nameof(Mask))]
[Index(nameof(Size))]
[Index(nameof(SizeType))]
public sealed class TgEfFilterEntity : ITgDbEntity, ITgDbFillEntity<TgEfFilterEntity>
{
	#region Public and private fields, properties, constructor

	[DefaultValue("00000000-0000-0000-0000-000000000000")]
	[Key]
	[Required]
	[Column(TgEfConstants.ColumnUid, TypeName = "CHAR(36)")]
	[SQLite.Collation("NOCASE")]
	public Guid Uid { get; set; }

	[DefaultValue(true)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnIsEnabled, TypeName = "BIT")]
    public bool IsEnabled { get; set; }

    [DefaultValue(TgEnumFilterType.SingleName)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnFilterType, TypeName = "INT")]
    public TgEnumFilterType FilterType { get; set; }

    [DefaultValue("Any")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgEfConstants.ColumnName, TypeName = "NVARCHAR(128)")]
    public string Name { get; set; } = default!;

    [DefaultValue("*")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgEfConstants.ColumnMask, TypeName = "NVARCHAR(128)")]
    public string Mask { get; set; } = default!;

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnSize, TypeName = "LONG(20)")]
    public long Size { get; set; }

    [NotMapped]
    public long SizeAtBytes => SizeType switch
    {
	    TgEnumFileSizeType.KBytes => Size * 1024,
	    TgEnumFileSizeType.MBytes => Size * 1024 * 1024,
	    TgEnumFileSizeType.GBytes => Size * 1024 * 1024 * 1024,
	    TgEnumFileSizeType.TBytes => Size * 1024 * 1024 * 1024 * 1024,
	    _ => Size,
    };
    
    [DefaultValue(TgEnumFileSizeType.Bytes)]
    [ConcurrencyCheck]
    [Column(TgEfConstants.ColumnSizeType, TypeName = "INT")]
    public TgEnumFileSizeType SizeType { get; set; }

    public TgEfFilterEntity() : base()
    {
        Default();
    }

    #endregion

    #region Public and private methods

    public string ToDebugString() => FilterType switch
    {
        TgEnumFilterType.MinSize or TgEnumFilterType.MaxSize =>
			$"{TgEfConstants.TableFilters} | {Uid} | {TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {Size} | {SizeType}",
        _ => $"{TgEfConstants.TableFilters} | {Uid} | {TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {(string.IsNullOrEmpty(Mask) ? $"<{nameof(string.Empty)}>" : Mask)}",
    };

    public void Default()
    {
		Uid = this.GetDefaultPropertyGuid(nameof(Uid));
		IsEnabled = this.GetDefaultPropertyBool(nameof(IsEnabled));
	    FilterType = this.GetDefaultPropertyGeneric<TgEnumFilterType>(nameof(FilterType));
	    Name = this.GetDefaultPropertyString(nameof(Name));
	    Mask = this.GetDefaultPropertyString(nameof(Mask));
	    Size = this.GetDefaultPropertyUint(nameof(Size));
	    SizeType = this.GetDefaultPropertyGeneric<TgEnumFileSizeType>(nameof(SizeType));
	}

    public TgEfFilterEntity Fill(TgEfFilterEntity item, bool isUidCopy)
	{
		if (isUidCopy)
			Uid = item.Uid;
		IsEnabled = item.IsEnabled;
		FilterType = item.FilterType;
		Name = item.Name;
		//_mask = filter.Mask;
		Mask = string.IsNullOrEmpty(item.Mask) &&
		        (Equals(item.FilterType, TgEnumFilterType.MinSize) ||
		         Equals(item.FilterType, TgEnumFilterType.MaxSize)) ? "*" : item.Mask;
		Size = item.Size;
		SizeType = item.SizeType;
        return this;
	}

	private string GetStringForFilterType() => FilterType switch
    {
        TgEnumFilterType.SingleName => TgLocaleHelper.Instance.MenuFiltersSetSingleName,
        TgEnumFilterType.SingleExtension => TgLocaleHelper.Instance.MenuFiltersSetSingleExtension,
        TgEnumFilterType.MultiName => TgLocaleHelper.Instance.MenuFiltersSetMultiName,
        TgEnumFilterType.MultiExtension => TgLocaleHelper.Instance.MenuFiltersSetMultiExtension,
        TgEnumFilterType.MinSize => TgLocaleHelper.Instance.MenuFiltersSetMinSize,
        TgEnumFilterType.MaxSize => TgLocaleHelper.Instance.MenuFiltersSetMaxSize,
        _ => $"<{TgLocaleHelper.Instance.MenuFiltersError}>",
    };

    #endregion
}