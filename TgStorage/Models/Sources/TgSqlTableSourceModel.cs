// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Sources;

[DebuggerDisplay("{ToString()}")]
[Persistent(TgSqlConstants.TableSources)]
public sealed class TgSqlTableSourceModel : TgSqlTableBase
{
	#region Public and private fields, properties, constructor

	private long _id;
	[DefaultValue(0)]
	[Persistent(TgSqlConstants.ColumnId)]
	[Indexed]
	public long Id { get => _id; set => SetPropertyValue(nameof(_id), ref _id, value); }

	private DateTime _dtChanged;
	[DefaultValue("0001-01-01 00:00:00")]
	[Persistent(TgSqlConstants.ColumnDtChanged)]
	[Indexed]
	public DateTime DtChanged { get => _dtChanged; set => SetPropertyValue(nameof(_dtChanged), ref _dtChanged, value); }

	private string _userName;
	[DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnUserName)]
	[Indexed]
	public string UserName { get => _userName; set => SetPropertyValue(nameof(_userName), ref _userName, value); }
	
    private string _title;
	[DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnTitle)]
	[Indexed]
	public string Title { get => _title; set => SetPropertyValue(nameof(_title), ref _title, value); }
	
    private string _about;
	[DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnAbout)]
	[Indexed]
	public string About { get => _about; set => SetPropertyValue(nameof(_about), ref _about, value); }
	
    private int _count;
	[DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnCount)]
	[Indexed]
	public int Count { get => _count; set => SetPropertyValue(nameof(_count), ref _count, value); }

	private string _directory;
	[DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnDirectory)]
	[Indexed]
	public string Directory { get => _directory; set => SetPropertyValue(nameof(_directory), ref _directory, value); }

	private int _firstId;
	[DefaultValue(1)]
	[Persistent(TgSqlConstants.ColumnFirstId)]
	[Indexed]
	public int FirstId { get => _firstId; set => SetPropertyValue(nameof(_firstId), ref _firstId, value); }

	private bool _isAutoUpdate;
	[DefaultValue(false)]
	[Persistent(TgSqlConstants.ColumnIsAutoUpdate)]
	[Indexed]
	public bool IsAutoUpdate { get => _isAutoUpdate; set => SetPropertyValue(nameof(_isAutoUpdate), ref _isAutoUpdate, value); }

	/// <summary>
	/// Default constructor.
	/// </summary>
	public TgSqlTableSourceModel() : base()
    {
        _id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
        _dtChanged = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(DtChanged));
		_userName = this.GetPropertyDefaultValue(nameof(UserName));
        _title = this.GetPropertyDefaultValue(nameof(Title));
        _about = this.GetPropertyDefaultValue(nameof(About));
		_count = this.GetPropertyDefaultValueAsGeneric<int>(nameof(Count));
		_directory = this.GetPropertyDefaultValue(nameof(Directory));
		_firstId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(FirstId));
		_isAutoUpdate = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsAutoUpdate));
	}

	/// <summary>
	/// Default constructor with session.
	/// </summary>
	/// <param name="session"></param>
	public TgSqlTableSourceModel(Session session) : base(session)
	{
		_id = this.GetPropertyDefaultValueAsGeneric<long>(nameof(Id));
		_dtChanged = this.GetPropertyDefaultValueAsGeneric<DateTime>(nameof(DtChanged));
		_userName = this.GetPropertyDefaultValue(nameof(UserName));
		_title = this.GetPropertyDefaultValue(nameof(Title));
		_about = this.GetPropertyDefaultValue(nameof(About));
		_count = this.GetPropertyDefaultValueAsGeneric<int>(nameof(Count));
		_directory = this.GetPropertyDefaultValue(nameof(Directory));
		_firstId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(FirstId));
		_isAutoUpdate = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(IsAutoUpdate));
	}

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public TgSqlTableSourceModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        _id = info.GetInt64(nameof(Id));
		_dtChanged = info.GetDateTime(nameof(DtChanged));
		_userName = info.GetString(nameof(UserName)) ?? this.GetPropertyDefaultValue(nameof(UserName));
		_title = info.GetString(nameof(Title)) ?? this.GetPropertyDefaultValue(nameof(Title));
		_about = info.GetString(nameof(About)) ?? this.GetPropertyDefaultValue(nameof(About));
		_count = info.GetInt32(nameof(Count));
		_directory = info.GetString(nameof(Directory)) ?? this.GetPropertyDefaultValue(nameof(Directory));
		_firstId = info.GetInt32(nameof(FirstId));
		_isAutoUpdate = info.GetBoolean(nameof(IsAutoUpdate));
	}

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public new void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Id), Id);
		info.AddValue(nameof(DtChanged), DtChanged);
        info.AddValue(nameof(UserName), UserName);
        info.AddValue(nameof(Title), Title);
        info.AddValue(nameof(About), About);
        info.AddValue(nameof(Count), Count);
		info.AddValue(nameof(Directory), Directory);
		info.AddValue(nameof(FirstId), FirstId);
		info.AddValue(nameof(IsAutoUpdate), IsAutoUpdate);
	}

	#endregion

	#region Public and private methods

	public override string ToString() =>
		$"{Id} | " +
		$"{(IsAutoUpdate ? "a" : " ")} | " +
		$"{(FirstId == Count ? "✓" : "x")} | " +
		$"{UserName} | " +
		$"{TgDataFormatUtils.TrimStringEnd(Title)} | " +
		$"{FirstId} {TgConstants.From} {Count} {TgConstants.Messages}";

	#endregion
}