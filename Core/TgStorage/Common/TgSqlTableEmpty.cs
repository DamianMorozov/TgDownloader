// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

/// <summary>
/// SQL table object base.
/// Do not make base from this class like TgSqlTableBase!!!
/// </summary>
[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public class TgSqlTableEmpty : XPLiteObject, ITgSqlTable
{
	#region Public and private fields, properties, constructor

	public TgLocaleHelper Locale => TgLocaleHelper.Instance;
	private Guid _uid;
	[Key(true)]
	[DefaultValue("00000000-0000-0000-0000-000000000000")]
	[Persistent(TgSqlConstants.ColumnUid)]
	[Indexed]
	public Guid Uid { get => _uid; set => SetPropertyValue(nameof(_uid), ref _uid, value); }
	public bool IsNotExists => Equals(Uid, Guid.Empty);
	public bool IsExists => !IsNotExists;

    public TgSqlTableEmpty()
	{
		_uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
	}

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    public TgSqlTableEmpty(Session session) : base(session)
	{
		_uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
	}

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public TgSqlTableEmpty(SerializationInfo info, StreamingContext context)
	{
		_uid = info.GetValue(nameof(Uid), typeof(Guid)) is Guid uid ? uid : Guid.Empty;
	}

	/// <summary>
	/// Get object data for serialization info.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(Uid), Uid);
    }

    #endregion

    #region Public and private methods

	public override string ToString() => $"{Uid}";

    public string ToDebugString() => ToString();

    #endregion
}