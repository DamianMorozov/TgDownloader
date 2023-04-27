// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Versions;

[DebuggerDisplay("{ToString()}")]
[Persistent(TgSqlConstants.TableVersions)]
public sealed class TgSqlTableVersionModel : TgSqlTableBase
{
	#region Public and private fields, properties, constructor

	private short _version;
    [DefaultValue(1)]
	[Persistent(TgSqlConstants.ColumnVersion)]
    [Size(4)]
	[Indexed]
	public short Version { get => _version; set => SetPropertyValue(nameof(_version), ref _version, value); }

    private string _description;
    [DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnDescription)]
	[Size(128)]
	[Indexed]
	public string Description { get => _description; set => SetPropertyValue(nameof(_description), ref _description, value); }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public TgSqlTableVersionModel() : base()
	{
        _version = this.GetPropertyDefaultValueAsGeneric<short>(nameof(Version));
        _description = this.GetPropertyDefaultValue(nameof(Description));
    }

    public TgSqlTableVersionModel(short version, string description) : base()
    {
	    _version = version;
	    _description = description;
    }

	/// <summary>
	/// Default constructor with session.
	/// </summary>
	/// <param name="session"></param>
	public TgSqlTableVersionModel(Session session) : base(session)
	{
		_version = this.GetPropertyDefaultValueAsGeneric<short>(nameof(Version));
		_description = this.GetPropertyDefaultValue(nameof(Description));
	}

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public TgSqlTableVersionModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        _version = info.GetInt16(nameof(Version));
        _description = info.GetString(nameof(Description)) ?? string.Empty;
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public new void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Version), Version);
        info.AddValue(nameof(Description), Description);
    }

    #endregion

    #region Public and private methods

    public override string ToString() =>
        $"{nameof(Version)} = {Version} | " +
        $"{nameof(Description)} = {Description}";

    #endregion
}