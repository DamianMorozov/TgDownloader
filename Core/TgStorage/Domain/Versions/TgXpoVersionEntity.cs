// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;

namespace TgStorage.Domain.Versions;

/// <summary>
/// SQL table VERSIONS.
/// Do not make base class!
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
[Persistent(TgStorageConstants.TableVersions)]
[DoNotNotify]
public sealed class TgXpoVersionEntity : XPLiteObject, ITgDbEntity
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

	private short _version;
	[DefaultValue(short.MaxValue)]
	[Persistent(TgStorageConstants.ColumnVersion)]
	[Size(4)]
	[Indexed]
	public short Version { get => _version; set => SetPropertyValue(nameof(_version), ref _version, value); }

	private string _description = default!;
	[DefaultValue("New version")]
	[Persistent(TgStorageConstants.ColumnDescription)]
	[Size(128)]
	[Indexed]
	public string Description { get => _description; set => SetPropertyValue(nameof(_description), ref _description, value); }

	/// <summary>
	/// Default constructor.
	/// </summary>
	public TgXpoVersionEntity()
	{
		Default();
	}

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    /// <param name="session"></param>
    public TgXpoVersionEntity(Session session) : base(session)
	{
		Default();
	}

	public TgXpoVersionEntity(short version, string description)
	{
		Uid = this.GetDefaultPropertyGuid(nameof(Uid));
		Version = version;
		Description = description;
	}

	#endregion

	#region Public and private methods

	public void Default()
	{
		Uid = this.GetDefaultPropertyGuid(nameof(Uid));
		Version = this.GetDefaultPropertyShort(nameof(Version));
		Description = this.GetDefaultPropertyString(nameof(Description));
	}

    public void Fill(object item)
	{
		if (item is not TgXpoVersionEntity version)
			throw new ArgumentException($"The {nameof(item)} is not {nameof(TgXpoVersionEntity)}!");

        Version = version.Version;
        Description = version.Description;
    }

    public void Backup(object item)
    {
	    Fill(item);
	    Uid = (item as TgXpoVersionEntity)!.Uid;
	}

    public override string ToString() => $"{Version} | {Description}";

    public string ToDebugString() => 
	    $"{TgStorageConstants.TableVersions} | {TgCommonUtils.GetIsExists(IsExist)} | {Uid} | {Version} | {Description}";

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
        if (obj is not TgXpoVersionEntity item)
            return false;
        return Equals(Uid, item.Uid) && Equals(Version, item.Version) && Equals(Description, item.Description);
    }

    #endregion
}