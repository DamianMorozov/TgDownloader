// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgLocalization.Helpers;

namespace TgStorage.Models.Filters;

[DebuggerDisplay("{ToString()}")]
[Persistent(TgSqlConstants.TableFilters)]
[DoNotNotify]
public sealed class TgSqlTableFilterModel : TgSqlTableBase
{
	#region Public and private fields, properties, constructor

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
	public TgSqlTableFilterModel() : base()
	{
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
		_isEnabled = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsEnabled));
		_filterType = this.GetPropertyDefaultValueAsGeneric<TgEnumFilterType>(nameof(FilterType));
		_name = this.GetPropertyDefaultValue(nameof(Name));
		_mask = this.GetPropertyDefaultValue(nameof(Mask));
		_size = this.GetPropertyDefaultValueAsGeneric<uint>(nameof(Size));
		_sizeType = this.GetPropertyDefaultValueAsGeneric<TgEnumFileSizeType>(nameof(SizeType));
	}

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public TgSqlTableFilterModel(SerializationInfo info, StreamingContext context) : base(info, context)
	{
		_isEnabled = info.GetBoolean(nameof(IsEnabled));
		object? type = info.GetValue(nameof(FilterType), typeof(Type));
		_filterType = type is TgEnumFilterType proxyTypeCast ? proxyTypeCast : TgEnumFilterType.None;
		_name = info.GetString(nameof(Name)) ?? string.Empty;
		_mask = info.GetString(nameof(Mask)) ?? string.Empty;
		_size = info.GetInt64(nameof(Size));
		object? sizeType = info.GetValue(nameof(SizeType), typeof(TgEnumFileSizeType));
		_sizeType = sizeType is TgEnumFileSizeType sizeTypeCast ? sizeTypeCast : TgEnumFileSizeType.Bytes;
	}

	/// <summary>
	/// Get object data for serialization info.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public new void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue(nameof(IsEnabled), IsEnabled);
		info.AddValue(nameof(FilterType), FilterType);
		info.AddValue(nameof(Name), Name);
		info.AddValue(nameof(Mask), Mask);
		info.AddValue(nameof(Size), Size);
		info.AddValue(nameof(SizeType), SizeType);
	}

	#endregion

	#region Public and private methods

	public override string ToString() => FilterType switch
	{
		TgEnumFilterType.MinSize or TgEnumFilterType.MaxSize =>
		$" {GetStringForIsEnabled()} | {GetStringForFilterType()} | {Name} | {Size} | {SizeType}",
		_ => $" {GetStringForIsEnabled()} | {GetStringForFilterType()} | {Name} | {(string.IsNullOrEmpty(Mask) ? $"<{nameof(string.Empty)}>" : Mask)}",
	};

	private string GetStringForIsEnabled() => IsEnabled ? "Enabled" : "Disabled";

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