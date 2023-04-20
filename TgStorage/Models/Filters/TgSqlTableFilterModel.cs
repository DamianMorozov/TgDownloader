// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Filters;

[DebuggerDisplay("{ToString()}")]
[Persistent(TgSqlConstants.TableFilters)]
public sealed class TgSqlTableFilterModel : TgSqlTableBase
{
	#region Public and private fields, properties, constructor

	private bool _isEnabled;
	[DefaultValue(false)]
	[Persistent(TgSqlConstants.ColumnIsEnabled)]
	[Indexed]
	public bool IsEnabled { get => _isEnabled; set => SetPropertyValue(nameof(_isEnabled), ref _isEnabled, value); }

	private TgFilterType _filterType;
	[DefaultValue(TgFilterType.None)]
	[Persistent(TgSqlConstants.ColumnFilterType)]
	[Indexed]
	public TgFilterType FilterType { get => _filterType; set => SetPropertyValue(nameof(_filterType), ref _filterType, value); }

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
		TgFileSizeType.KBytes => Size * 1024,
		TgFileSizeType.MBytes => Size * 1024 * 1024,
		TgFileSizeType.GBytes => Size * 1024 * 1024 * 1024,
		TgFileSizeType.TBytes => Size * 1024 * 1024 * 1024 * 1024,
		_ => Size,
	};

	[DefaultValue(TgFileSizeType.Bytes)]
	private TgFileSizeType _sizeType;
	[Persistent(TgSqlConstants.ColumnSizeType)]
	[Indexed]
	public TgFileSizeType SizeType { get => _sizeType; set => SetPropertyValue(nameof(_sizeType), ref _sizeType, value); }

	/// <summary>
	/// Default constructor.
	/// </summary>
	public TgSqlTableFilterModel() : base()
	{
		_isEnabled = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsEnabled));
		_filterType = this.GetPropertyDefaultValueAsGeneric<TgFilterType>(nameof(FilterType));
		_name = this.GetPropertyDefaultValue(nameof(Name));
		_mask = this.GetPropertyDefaultValue(nameof(Mask));
		_size = this.GetPropertyDefaultValueAsGeneric<uint>(nameof(Size));
		_sizeType = this.GetPropertyDefaultValueAsGeneric<TgFileSizeType>(nameof(SizeType));
	}

	/// <summary>
	/// Default constructor with session.
	/// </summary>
	/// <param name="session"></param>
	public TgSqlTableFilterModel(Session session) : base(session)
	{
		_isEnabled = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsEnabled));
		_filterType = this.GetPropertyDefaultValueAsGeneric<TgFilterType>(nameof(FilterType));
		_name = this.GetPropertyDefaultValue(nameof(Name));
		_mask = this.GetPropertyDefaultValue(nameof(Mask));
		_size = this.GetPropertyDefaultValueAsGeneric<uint>(nameof(Size));
		_sizeType = this.GetPropertyDefaultValueAsGeneric<TgFileSizeType>(nameof(SizeType));
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
		_filterType = type is TgFilterType proxyTypeCast ? proxyTypeCast : TgFilterType.None;
		_name = info.GetString(nameof(Name)) ?? string.Empty;
		_mask = info.GetString(nameof(Mask)) ?? string.Empty;
		_size = info.GetInt64(nameof(Size));
		object? sizeType = info.GetValue(nameof(SizeType), typeof(TgFileSizeType));
		_sizeType = sizeType is TgFileSizeType sizeTypeCast ? sizeTypeCast : TgFileSizeType.Bytes;
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
		TgFilterType.MinSize or TgFilterType.MaxSize =>
		$" {GetStringForIsEnabled()} | {GetStringForFilterType()} | {Name} | {Size} | {SizeType}",
		_ => $" {GetStringForIsEnabled()} | {GetStringForFilterType()} | {Name} | {(string.IsNullOrEmpty(Mask) ? $"<{nameof(string.Empty)}>" : Mask)}",
	};

	private string GetStringForIsEnabled() => IsEnabled ? "Enabled" : "Disabled";

	private string GetStringForFilterType() => FilterType switch
	{
		TgFilterType.SingleName => TgConstants.MenuFiltersSetSingleName,
		TgFilterType.SingleExtension => TgConstants.MenuFiltersSetSingleExtension,
		TgFilterType.MultiName => TgConstants.MenuFiltersSetMultiName,
		TgFilterType.MultiExtension => TgConstants.MenuFiltersSetMultiExtension,
		TgFilterType.MinSize => TgConstants.MenuFiltersSetMinSize,
		TgFilterType.MaxSize => TgConstants.MenuFiltersSetMaxSize,
		_ => $"<{TgConstants.MenuFiltersError}>",
	};

	#endregion
}