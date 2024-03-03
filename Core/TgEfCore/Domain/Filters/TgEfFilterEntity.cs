// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Filters;

[DebuggerDisplay("{ToDebugString()}")]
[Table(TgSqlConstants.TableFilters)]
public sealed class TgEfFilterEntity : TgEfEntityBase
{
    #region Public and private fields, properties, constructor

    [DefaultValue(false)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnIsEnabled)]
    [SQLite.Indexed]
    public bool IsEnabled { get; set; }

    [DefaultValue(TgEnumFilterType.None)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnFilterType)]
    [SQLite.Indexed]
    public TgEnumFilterType FilterType { get; set; }

    [DefaultValue("Any")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgSqlConstants.ColumnName)]
    [SQLite.Indexed]
    public string Name { get; set; }

    [DefaultValue("*")]
    [ConcurrencyCheck]
    [MaxLength(128)]
    [Column(TgSqlConstants.ColumnMask)]
    [SQLite.Indexed]
    public string Mask { get; set; }

    [DefaultValue(0)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnSize)]
    [SQLite.Indexed]
    public long Size { get; set; }

    [DefaultValue(TgEnumFileSizeType.Bytes)]
    [ConcurrencyCheck]
    [Column(TgSqlConstants.ColumnSizeType)]
    [SQLite.Indexed]
    public TgEnumFileSizeType SizeType { get; set; }

    public TgEfFilterEntity() : base()
    {
        IsEnabled = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsEnabled));
        FilterType = this.GetPropertyDefaultValueAsGeneric<TgEnumFilterType>(nameof(FilterType));
        Name = this.GetPropertyDefaultValue(nameof(Name));
        Mask = this.GetPropertyDefaultValue(nameof(Mask));
        Size = this.GetPropertyDefaultValueAsGeneric<uint>(nameof(Size));
        SizeType = this.GetPropertyDefaultValueAsGeneric<TgEnumFileSizeType>(nameof(SizeType));
    }

    #endregion

    #region Public and private methods

    public override string ToDebugString() => FilterType switch
    {
        TgEnumFilterType.MinSize or TgEnumFilterType.MaxSize =>
        $"{base.ToDebugString()} | {TgCommonUtils.GetIsExists(IsExists)} | {Uid} | {TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {Size} | {SizeType}",
        _ => $"{base.ToDebugString()} | {TgCommonUtils.GetIsExists(IsExists)} | {Uid} | {TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {(string.IsNullOrEmpty(Mask) ? $"<{nameof(string.Empty)}>" : Mask)}",
    };

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