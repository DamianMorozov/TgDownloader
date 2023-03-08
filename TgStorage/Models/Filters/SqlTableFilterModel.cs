// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Utils;
using TgCore.Enums;
using TgCore.Localization;

namespace TgStorage.Models.Filters;

[DebuggerDisplay("{nameof(SqlTableFilterModel)} | {IsActive} | {FilterType} |{Name} | {Mask} | {Size}{SizeType}")]
[Persistent("FILTERS")]
public class SqlTableFilterModel : SqlTableXpLiteBase
{
 #region Public and private fields, properties, constructor

    private bool _isActive;
    [DefaultValue(false)]
    [Persistent("IS_ACTIVE")]
    public bool IsActive { get => _isActive; set => SetPropertyValue(nameof(_isActive), ref _isActive, value); }

    private FilterType _filterType;
    [DefaultValue(FilterType.None)]
    [Persistent("FILTER_TYPE")]
    public FilterType FilterType { get => _filterType; set => SetPropertyValue(nameof(_filterType), ref _filterType, value); }

    [DefaultValue("Any")]
    private string _name;
    [Size(128)]
    [Persistent("NAME")]
    public string Name { get => _name; set => SetPropertyValue(nameof(_name), ref _name, value); }

    [DefaultValue("*")]
    private string _mask;
    [Size(128)]
    [Persistent("MASK")]
    public string Mask { get => _mask; set => SetPropertyValue(nameof(_mask), ref _mask, value); }

    [DefaultValue(0)]
    private long _size;
    [Size(128)]
    [Persistent("SIZE")]
    public long Size { get => _size; set => SetPropertyValue(nameof(_size), ref _size, value); }
    public long SizeAtBytes => SizeType switch
	{
		FileSizeType.KBytes => Size * 1024,
		FileSizeType.MBytes => Size * 1024 * 1024,
		FileSizeType.GBytes => Size * 1024 * 1024 * 1024,
		FileSizeType.TBytes => Size * 1024 * 1024 * 1024 * 1024,
		_ => Size,
	};

    private FileSizeType _sizeType;
	[DefaultValue(FileSizeType.Bytes)]
	[Persistent("SIZE_TYPE")]
	public FileSizeType SizeType { get => _sizeType; set => SetPropertyValue(nameof(_sizeType), ref _sizeType, value); }
	
    /// <summary>
	/// Default constructor.
	/// </summary>
	public SqlTableFilterModel()
    {
        _isActive = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(_isActive));
        _filterType = this.GetPropertyDefaultValueAsGeneric<FilterType>(nameof(_filterType));
        _name = this.GetPropertyDefaultValue(nameof(_name));
        _mask = this.GetPropertyDefaultValue(nameof(_mask));
        _size = this.GetPropertyDefaultValueAsGeneric<uint>(nameof(_size));
		_sizeType = this.GetPropertyDefaultValueAsGeneric<FileSizeType>(nameof(_sizeType));
    }

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    /// <param name="session"></param>
    public SqlTableFilterModel(Session session) : base(session)
    {
		_isActive = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(_isActive));
		_filterType = this.GetPropertyDefaultValueAsGeneric<FilterType>(nameof(_filterType));
        _name = this.GetPropertyDefaultValue(nameof(_name));
        _mask = this.GetPropertyDefaultValue(nameof(_mask));
        _size = this.GetPropertyDefaultValueAsGeneric<uint>(nameof(_size));
		_sizeType = this.GetPropertyDefaultValueAsGeneric<FileSizeType>(nameof(_sizeType));
	}

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	protected SqlTableFilterModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
		_isActive = info.GetBoolean(nameof(_isActive));
        object? type = info.GetValue(nameof(_filterType), typeof(FilterType));
        _filterType = type is FilterType proxyTypeCast ? proxyTypeCast : FilterType.None;
        _name = info.GetString(nameof(_name)) ?? string.Empty;
        _mask = info.GetString(nameof(_mask)) ?? string.Empty;
        _size = info.GetInt64(nameof(_size));
        object? sizeType = info.GetValue(nameof(_sizeType), typeof(FileSizeType));
        _sizeType = sizeType is FileSizeType sizeTypeCast ? sizeTypeCast : FileSizeType.Bytes;
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public new void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(_isActive), _isActive);
        info.AddValue(nameof(_filterType), _filterType);
        info.AddValue(nameof(_name), _name);
        info.AddValue(nameof(_mask), _mask);
        info.AddValue(nameof(_size), _size);
        info.AddValue(nameof(_sizeType), _sizeType);
	}

	#endregion

	#region Public and private methods

	public override string ToString() => FilterType switch
	{
		FilterType.MinSize or FilterType.MaxSize => $" {GetStringForIsActive()} | {GetStringForFilterType()} | {Name} | {Size} | {SizeType}",
		_ => $" {GetStringForIsActive()} | {GetStringForFilterType()} | {Name} | {(string.IsNullOrEmpty(Mask) ? $"<{nameof(string.Empty)}>" : Mask)}",
	};

    private string GetStringForIsActive() => IsActive ? "Is active" : "Isn't active";

    private string GetStringForFilterType() => FilterType switch
    {
		FilterType.SingleName => TgLocaleHelper.Instance.MenuFiltersSetSingleName,
		FilterType.SingleExtension => TgLocaleHelper.Instance.MenuFiltersSetSingleExtension,
		FilterType.MultiName => TgLocaleHelper.Instance.MenuFiltersSetMultiName,
		FilterType.MultiExtension => TgLocaleHelper.Instance.MenuFiltersSetMultiExtension,
		FilterType.MinSize => TgLocaleHelper.Instance.MenuFiltersSetMinSize,
		FilterType.MaxSize => TgLocaleHelper.Instance.MenuFiltersSetMaxSize,
		_ => $"<{TgLocaleHelper.Instance.MenuFiltersError}>",
	};

	#endregion
}