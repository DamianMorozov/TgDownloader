// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Security.Cryptography;

namespace TgStorage.Domain.Versions;

/// <summary>
/// SQL table VERSIONS.
/// Do not make base class!
/// </summary>
[Persistent(TgSqlConstants.TableVersions)]
[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgSqlTableVersionModel : XPLiteObject, ITgSqlTable
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
	public TgSqlTableVersionModel()
	{
		_uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
		_version = this.GetPropertyDefaultValueAsGeneric<short>(nameof(Version));
		_description = this.GetPropertyDefaultValue(nameof(Description));
	}

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    /// <param name="session"></param>
    public TgSqlTableVersionModel(Session session) : base(session)
	{
		_uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
		_version = this.GetPropertyDefaultValueAsGeneric<short>(nameof(Version));
		_description = this.GetPropertyDefaultValue(nameof(Description));
	}

	public TgSqlTableVersionModel(short version, string description)
	{
		_uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
		_version = version;
		_description = description;
	}

    public void Fill(TgSqlTableVersionModel item, Guid? uid = null)
	{
		_uid = uid ?? this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
        if (item is { } version)
        {
            _version = version.Version;
            _description = version.Description;
        }
        else
        {
            _version = this.GetPropertyDefaultValueAsGeneric<short>(nameof(Version));
            _description = this.GetPropertyDefaultValue(nameof(Description));
        }
    }

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public TgSqlTableVersionModel(SerializationInfo info, StreamingContext context)
	{
		_uid = info.GetValue(nameof(Uid), typeof(Guid)) is Guid uid ? uid : Guid.Empty;
		_version = info.GetInt16(nameof(Version));
		_description = info.GetString(nameof(Description)) ?? string.Empty;
	}

	/// <summary>
	/// Get object data for serialization info.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(Uid), Uid);
		info.AddValue(nameof(Version), Version);
		info.AddValue(nameof(Description), Description);
	}

	#endregion

	#region Public and private methods

	public override string ToString() => $"{Version} | {Description}";

    public string ToDebugString() => $"{TgCommonUtils.GetIsExists(IsExists)} | {Uid} | {Version} | {Description}";

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Uid.GetHashCode();
            hashCode = (hashCode * 397) ^ Version.GetHashCode();
            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(Description) ? 0 : Description.GetHashCode());
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
        if (obj is not TgSqlTableVersionModel item)
            return false;
        return Equals(Uid, item.Uid) && Equals(Version, item.Version) && Equals(Description, item.Description);
    }

    #endregion
}