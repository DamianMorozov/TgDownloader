// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Filters;

/// <summary>
/// SQL table FILTERS.
/// Do not make base class!
/// </summary>
[Persistent(TgSqlConstants.TableFilters)]
[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgSqlTableFilterModel : XPLiteObject, ITgSqlTable
{
	#region Public and private fields, properties, constructor

	private Guid _uid;
	[Key(true)]
	[DefaultValue("00000000-0000-0000-0000-000000000000")]
	[Persistent(TgSqlConstants.ColumnUid)]
	[Indexed]
	public Guid Uid { get => _uid; set => SetPropertyValue(nameof(_uid), ref _uid, value); }
	public bool IsNotExists => Equals(Uid, Guid.Empty);
	public bool IsExists => !IsNotExists;

	private bool _isEnabled;
	[DefaultValue(false)]
	[Persistent(TgSqlConstants.ColumnIsEnabled)]
	[Indexed]
	public bool IsEnabled { get => _isEnabled; set => SetPropertyValue(nameof(_isEnabled), ref _isEnabled, value); }

	private TgEnumFilterType _filterType;
	[DefaultValue(TgEnumFilterType.None)]
	[Persistent(TgSqlConstants.ColumnFilterType)]
	[Indexed]
	public TgEnumFilterType FilterType { get => _filterType; set => SetPropertyValue(nameof(_filterType), ref _filterType, value); }

	private string _name;
	[DefaultValue("Any")]
	[Persistent(TgSqlConstants.ColumnName)]
	[Size(128)]
	[Indexed]
	public string Name { get => _name; set => SetPropertyValue(nameof(_name), ref _name, value); }

	private string _mask;
	[DefaultValue("*")]
	[Persistent(TgSqlConstants.ColumnMask)]
	[Size(128)]
	[Indexed]
	public string Mask { get => _mask; set => SetPropertyValue(nameof(_mask), ref _mask, value); }

	private long _size;
	[DefaultValue(0)]
	[Persistent(TgSqlConstants.ColumnSize)]
	[Size(128)]
	[Indexed]
	public long Size { get => _size; set => SetPropertyValue(nameof(_size), ref _size, value); }
	public long SizeAtBytes => SizeType switch
	{
		TgEnumFileSizeType.KBytes => Size * 1024,
		TgEnumFileSizeType.MBytes => Size * 1024 * 1024,
		TgEnumFileSizeType.GBytes => Size * 1024 * 1024 * 1024,
		TgEnumFileSizeType.TBytes => Size * 1024 * 1024 * 1024 * 1024,
		_ => Size,
	};

	[DefaultValue(TgEnumFileSizeType.Bytes)]
	private TgEnumFileSizeType _sizeType;
	[Persistent(TgSqlConstants.ColumnSizeType)]
	[Indexed]
	public TgEnumFileSizeType SizeType { get => _sizeType; set => SetPropertyValue(nameof(_sizeType), ref _sizeType, value); }

	/// <summary>
	/// Default constructor.
	/// </summary>
	public TgSqlTableFilterModel()
	{
		_uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
		_isEnabled = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsEnabled));
		_filterType = this.GetPropertyDefaultValueAsGeneric<TgEnumFilterType>(nameof(FilterType));
		_name = this.GetPropertyDefaultValue(nameof(Name));
		_mask = this.GetPropertyDefaultValue(nameof(Mask));
		_size = this.GetPropertyDefaultValueAsGeneric<uint>(nameof(Size));
		_sizeType = this.GetPropertyDefaultValueAsGeneric<TgEnumFileSizeType>(nameof(SizeType));
	}

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    /// <param name="session"></param>
    public TgSqlTableFilterModel(Session session) : base(session)
    {
		_uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
		_isEnabled = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsEnabled));
		_filterType = this.GetPropertyDefaultValueAsGeneric<TgEnumFilterType>(nameof(FilterType));
		_name = this.GetPropertyDefaultValue(nameof(Name));
		_mask = this.GetPropertyDefaultValue(nameof(Mask));
		_size = this.GetPropertyDefaultValueAsGeneric<uint>(nameof(Size));
		_sizeType = this.GetPropertyDefaultValueAsGeneric<TgEnumFileSizeType>(nameof(SizeType));
	}

    public void Fill(TgSqlTableFilterModel item, Guid? uid = null)
	{
		_uid = uid ?? this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
        if (item is { } filter)
        {
		    _isEnabled = filter.IsEnabled;
		    _filterType = filter.FilterType;
		    _name = filter.Name;
            //_mask = filter.Mask;
            _mask = string.IsNullOrEmpty(filter.Mask) && 
                    (Equals(filter.FilterType, TgEnumFilterType.MinSize) ||
                    Equals(filter.FilterType, TgEnumFilterType.MaxSize)) ? "*" : filter.Mask;
		    _size = filter.Size;
		    _sizeType = filter.SizeType;
        }
        else
        {
            _isEnabled = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsEnabled));
            _filterType = this.GetPropertyDefaultValueAsGeneric<TgEnumFilterType>(nameof(FilterType));
            _name = this.GetPropertyDefaultValue(nameof(Name));
            _mask = this.GetPropertyDefaultValue(nameof(Mask));
            _size = this.GetPropertyDefaultValueAsGeneric<uint>(nameof(Size));
            _sizeType = this.GetPropertyDefaultValueAsGeneric<TgEnumFileSizeType>(nameof(SizeType));
        }
    }

	#endregion

	#region Public and private methods

	public override string ToString() => FilterType switch
	{
		TgEnumFilterType.MinSize or TgEnumFilterType.MaxSize =>
		$"{TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {Size} | {SizeType}",
		_ => $"{TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {(string.IsNullOrEmpty(Mask) ? $"<{nameof(string.Empty)}>" : Mask)}",
	};

	public string ToDebugString() => FilterType switch
	{
		TgEnumFilterType.MinSize or TgEnumFilterType.MaxSize =>
		$"{TgCommonUtils.GetIsExists(IsExists)} | {Uid} | {TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {Size} | {SizeType}",
		_ => $"{TgCommonUtils.GetIsExists(IsExists)} | {Uid} | {TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {(string.IsNullOrEmpty(Mask) ? $"<{nameof(string.Empty)}>" : Mask)}",
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

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Uid.GetHashCode();
            hashCode = (hashCode * 397) ^ IsEnabled.GetHashCode();
            hashCode = (hashCode * 397) ^ FilterType.GetHashCode();
            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(Name) ? 0 : Name.GetHashCode());
            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(Mask) ? 0 : Mask.GetHashCode());
            hashCode = (hashCode * 397) ^ Size.GetHashCode();
            hashCode = (hashCode * 397) ^ SizeType.GetHashCode();
            return hashCode;
        }
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        if (obj is not TgSqlTableFilterModel item)
            return false;
        return Equals(Uid, item.Uid) && Equals(IsEnabled, item.IsEnabled) && Equals(FilterType, item.FilterType) &&
               Equals(Name, item.Name) && Equals(Mask, item.Mask) &&
               Equals(Size, item.Size) && Equals(SizeType, item.SizeType);
    }

    #endregion
}