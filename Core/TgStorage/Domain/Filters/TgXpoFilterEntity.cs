// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;

namespace TgStorage.Domain.Filters;

/// <summary>
/// SQL table FILTERS.
/// Do not make base class!
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
[Persistent(TgStorageConstants.TableFilters)]
[DoNotNotify]
public sealed class TgXpoFilterEntity : XPLiteObject, ITgDbEntity
{
	#region Public and private fields, properties, constructor

	private Guid _uid;
	[DevExpress.Xpo.Key(true)]
	[DefaultValue("00000000-0000-0000-0000-000000000000")]
	[Persistent(TgStorageConstants.ColumnUid)]
	[Indexed]
	public Guid Uid { get => _uid; set => SetPropertyValue(nameof(_uid), ref _uid, value); }
	[NonPersistent]
	public string UidString
	{
		get => Uid.ToString();
		set => Uid = Guid.TryParse(value, out Guid uid) ? uid : Guid.Empty;
	}
	[NonPersistent]
	public bool IsExist => !Equals(Uid, Guid.Empty);
	[NonPersistent]
	public bool NotExist => !IsExist;
	[NonPersistent]
	public TgEnumLetterCase LetterCase { get; set; }

	private bool _isEnabled;
	[DefaultValue(true)]
	[Persistent(TgStorageConstants.ColumnIsEnabled)]
	[Indexed]
	public bool IsEnabled { get => _isEnabled; set => SetPropertyValue(nameof(_isEnabled), ref _isEnabled, value); }

	private TgEnumFilterType _filterType;
	[DefaultValue(TgEnumFilterType.SingleName)]
	[Persistent(TgStorageConstants.ColumnFilterType)]
	[Indexed]
	public TgEnumFilterType FilterType { get => _filterType; set => SetPropertyValue(nameof(_filterType), ref _filterType, value); }

	private string _name = default!;
	[DefaultValue("Any")]
	[Persistent(TgStorageConstants.ColumnName)]
	[Size(128)]
	[Indexed]
	public string Name { get => _name; set => SetPropertyValue(nameof(_name), ref _name, value); }

	private string _mask = default!;
	[DefaultValue("*")]
	[Persistent(TgStorageConstants.ColumnMask)]
	[Size(128)]
	[Indexed]
	public string Mask { get => _mask; set => SetPropertyValue(nameof(_mask), ref _mask, value); }

	private long _size;
	[DefaultValue(0)]
	[Persistent(TgStorageConstants.ColumnSize)]
	[Indexed]
	public long Size { get => _size; set => SetPropertyValue(nameof(_size), ref _size, value); }
	
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
	private TgEnumFileSizeType _sizeType;
	[Persistent(TgStorageConstants.ColumnSizeType)]
	[Indexed]
	public TgEnumFileSizeType SizeType { get => _sizeType; set => SetPropertyValue(nameof(_sizeType), ref _sizeType, value); }

	/// <summary>
	/// Default constructor.
	/// </summary>
	public TgXpoFilterEntity()
	{
		Default();
	}

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    /// <param name="session"></param>
    public TgXpoFilterEntity(Session session) : base(session)
    {
	    Default();
	}

    #endregion

    #region Public and private methods

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

	public void Fill(object item)
	{
		if (item is not TgXpoFilterEntity filter)
			throw new ArgumentException($"The {nameof(item)} is not {nameof(TgXpoFilterEntity)}!");
        
	    IsEnabled = filter.IsEnabled;
	    FilterType = filter.FilterType;
	    Name = filter.Name;
        Mask = string.IsNullOrEmpty(filter.Mask) && 
                (Equals(filter.FilterType, TgEnumFilterType.MinSize) ||
                Equals(filter.FilterType, TgEnumFilterType.MaxSize)) ? "*" : filter.Mask;
	    Size = filter.Size;
	    SizeType = filter.SizeType;
    }

	public void Backup(object item)
	{
		Fill(item);
		Uid = (item as TgXpoFilterEntity)!.Uid;
	}

	public override string ToString() => FilterType switch
	{
		TgEnumFilterType.MinSize or TgEnumFilterType.MaxSize =>
		$"{TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {Size} | {SizeType}",
		_ => $"{TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {(string.IsNullOrEmpty(Mask) ? $"<{nameof(string.Empty)}>" : Mask)}",
	};

	public string ToDebugString() => FilterType switch
	{
		TgEnumFilterType.MinSize or TgEnumFilterType.MaxSize =>
			$"{TgStorageConstants.TableFilters} | {TgCommonUtils.GetIsExists(IsExist)} | {Uid} | {TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {Size} | {SizeType}",
		_ => $"{TgStorageConstants.TableFilters} | {TgCommonUtils.GetIsExists(IsExist)} | {Uid} | {TgCommonUtils.GetIsEnabled(IsEnabled)} | {GetStringForFilterType()} | {Name} | {(string.IsNullOrEmpty(Mask) ? $"<{nameof(string.Empty)}>" : Mask)}",
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
        if (obj is not TgXpoFilterEntity item)
            return false;
        return Equals(Uid, item.Uid) && Equals(IsEnabled, item.IsEnabled) && Equals(FilterType, item.FilterType) &&
               Equals(Name, item.Name) && Equals(Mask, item.Mask) &&
               Equals(Size, item.Size) && Equals(SizeType, item.SizeType);
    }

    #endregion
}