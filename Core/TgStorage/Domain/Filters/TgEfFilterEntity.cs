// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Microsoft.EntityFrameworkCore;

namespace TgStorage.Domain.Filters;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgStorageConstants.TableFilters)]
[Index(nameof(IsEnabled))]
[Index(nameof(FilterType))]
[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Mask))]
[Index(nameof(Size))]
[Index(nameof(SizeType))]
public sealed class TgEfFilterEntity : TgEfEntityBase
{
    #region Public and private fields, properties, constructor

    [DefaultValue(true)]
    [ConcurrencyCheck]
    [Column(TgStorageConstants.ColumnIsEnabled)]
    public bool IsEnabled { get; set; }

    [DefaultValue(TgEnumFilterType.SingleName)]
    [ConcurrencyCheck]
    [Column(TgStorageConstants.ColumnFilterType)]
    public TgEnumFilterType FilterType { get; set; }

    [DefaultValue("Any")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgStorageConstants.ColumnName)]
    public string Name { get; set; } = default!;

    [DefaultValue("*")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgStorageConstants.ColumnMask)]
    public string Mask { get; set; } = default!;

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgStorageConstants.ColumnSize)]
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
    [Column(TgStorageConstants.ColumnSizeType)]
    public TgEnumFileSizeType SizeType { get; set; }

    public TgEfFilterEntity() : base()
    {
        Default();
    }

    #endregion

    #region Public and private methods

    public override string ToDebugString() => FilterType switch
    {
        TgEnumFilterType.MinSize or TgEnumFilterType.MaxSize =>
			$"{TgStorageConstants.TableFilters} | {base.ToDebugString()} | {TgCommonUtils.GetIsExists(IsExist)} | {Uid} | {TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {Size} | {SizeType}",
        _ => $"{TgStorageConstants.TableFilters} | {base.ToDebugString()} | {TgCommonUtils.GetIsExists(IsExist)} | {Uid} | {TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {(string.IsNullOrEmpty(Mask) ? $"<{nameof(string.Empty)}>" : Mask)}",
    };

    public override void Default()
    {
	    base.Default();
	    IsEnabled = this.GetDefaultPropertyBool(nameof(IsEnabled));
	    FilterType = this.GetDefaultPropertyGeneric<TgEnumFilterType>(nameof(FilterType));
	    Name = this.GetDefaultPropertyString(nameof(Name));
	    Mask = this.GetDefaultPropertyString(nameof(Mask));
	    Size = this.GetDefaultPropertyUint(nameof(Size));
	    SizeType = this.GetDefaultPropertyGeneric<TgEnumFileSizeType>(nameof(SizeType));
	}

    public override void Fill(object item)
    {
		if (item is not TgEfFilterEntity filter)
			throw new ArgumentException($"The {nameof(item)} is not {nameof(TgEfFilterEntity)}!");

		IsEnabled = filter.IsEnabled;
		FilterType = filter.FilterType;
		Name = filter.Name;
		//_mask = filter.Mask;
		Mask = string.IsNullOrEmpty(filter.Mask) &&
		        (Equals(filter.FilterType, TgEnumFilterType.MinSize) ||
		         Equals(filter.FilterType, TgEnumFilterType.MaxSize)) ? "*" : filter.Mask;
		Size = filter.Size;
		SizeType = filter.SizeType;
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