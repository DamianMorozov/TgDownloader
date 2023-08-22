// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo.Logger.Transport;

namespace TgStorage.Domain.Proxies;

/// <summary>
/// SQL table PROXIES.
/// Do not make base class!
/// </summary>
[Persistent(TgSqlConstants.TableProxies)]
[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgSqlTableProxyModel : XPLiteObject, ITgSqlTable
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

	private TgEnumProxyType _type;
	[DefaultValue(TgEnumProxyType.None)]
	[Persistent(TgSqlConstants.ColumnType)]
	[Indexed]
	public TgEnumProxyType Type { get => _type; set => SetPropertyValue(nameof(_type), ref _type, value); }

	private string _hostName;
	[DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnHostName)]
	[Size(128)]
	[Indexed]
	public string HostName { get => _hostName; set => SetPropertyValue(nameof(_hostName), ref _hostName, value); }

	private ushort _port;
	[DefaultValue(0)]
	[Persistent(TgSqlConstants.ColumnPort)]
	[Indexed]
	public ushort Port { get => _port; set => SetPropertyValue(nameof(_port), ref _port, value); }

	private string _userName;
	[DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnUserName)]
	[Size(128)]
	[Indexed]
	public string UserName { get => _userName; set => SetPropertyValue(nameof(_userName), ref _userName, value); }

	private string _password;
	[DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnPassword)]
	[Size(128)]
	[Indexed]
	public string Password { get => _password; set => SetPropertyValue(nameof(_password), ref _password, value); }

	private string _secret;
	[DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnSecret)]
	[Size(128)]
	[Indexed]
	public string Secret { get => _secret; set => SetPropertyValue(nameof(_secret), ref _secret, value); }

	/// <summary>
	/// Default constructor.
	/// </summary>
	public TgSqlTableProxyModel()
	{
		_uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
		_type = this.GetPropertyDefaultValueAsGeneric<TgEnumProxyType>(nameof(Type));
		_hostName = this.GetPropertyDefaultValue(nameof(HostName));
		_port = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(Port));
		_userName = this.GetPropertyDefaultValue(nameof(UserName));
		_password = this.GetPropertyDefaultValue(nameof(Password));
		_secret = this.GetPropertyDefaultValue(nameof(Secret));
	}

    /// <summary>
	/// Default constructor with session.
	/// </summary>
	/// <param name="session"></param>
    public TgSqlTableProxyModel(Session session) : base(session)
	{
		_uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
		_type = this.GetPropertyDefaultValueAsGeneric<TgEnumProxyType>(nameof(Type));
		_hostName = this.GetPropertyDefaultValue(nameof(HostName));
		_port = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(Port));
		_userName = this.GetPropertyDefaultValue(nameof(UserName));
		_password = this.GetPropertyDefaultValue(nameof(Password));
		_secret = this.GetPropertyDefaultValue(nameof(Secret));
	}

    public void Fill(TgSqlTableProxyModel item, Guid? uid = null)
    {
		_uid = uid ?? this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
        if (item is { } proxy)
        {
		    _type = proxy.Type;
		    _hostName = proxy.HostName;
		    _port = proxy.Port;
		    _userName = proxy.UserName;
		    _password = proxy.Password;
		    _secret = proxy.Secret;
        }
        else
        {
            _type = this.GetPropertyDefaultValueAsGeneric<TgEnumProxyType>(nameof(Type));
            _hostName = this.GetPropertyDefaultValue(nameof(HostName));
            _port = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(Port));
            _userName = this.GetPropertyDefaultValue(nameof(UserName));
            _password = this.GetPropertyDefaultValue(nameof(Password));
            _secret = this.GetPropertyDefaultValue(nameof(Secret));
        }
    }

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public TgSqlTableProxyModel(SerializationInfo info, StreamingContext context)
	{
		_uid = info.GetValue(nameof(Uid), typeof(Guid)) is Guid uid ? uid : Guid.Empty;
		object? type = info.GetValue(nameof(Type), typeof(TgEnumProxyType));
		_type = type is TgEnumProxyType proxyType ? proxyType : TgEnumProxyType.None;
		_hostName = info.GetString(nameof(HostName)) ?? string.Empty;
		_port = info.GetUInt16(nameof(Port));
		_userName = info.GetString(nameof(UserName)) ?? string.Empty;
		_password = info.GetString(nameof(Password)) ?? string.Empty;
		_secret = info.GetString(nameof(Secret)) ?? string.Empty;
	}

	/// <summary>
	/// Get object data for serialization info.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(Uid), Uid);
		info.AddValue(nameof(Type), Type);
		info.AddValue(nameof(HostName), HostName);
		info.AddValue(nameof(Port), Port);
		info.AddValue(nameof(UserName), UserName);
		info.AddValue(nameof(Password), Password);
		info.AddValue(nameof(Secret), Secret);
	}

	#endregion

	#region Public and private methods

	public override string ToString() => 
		$"{Type} | {HostName} | {Port} | {UserName} | {Password} | {Secret}";

	public string ToDebugString() => 
		$"{TgCommonUtils.GetIsExists(IsExists)} | {Uid} | {Type} | {HostName} | {Port} | {UserName} | {Password} | " +
        $"{TgCommonUtils.GetIsFlag(!string.IsNullOrEmpty(Secret), Secret, "<No secret>")}";

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Uid.GetHashCode();
            hashCode = (hashCode * 397) ^ Type.GetHashCode();
            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(HostName) ? 0 : HostName.GetHashCode());
            hashCode = (hashCode * 397) ^ Port.GetHashCode();
            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(UserName) ? 0 : UserName.GetHashCode());
            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(Password) ? 0 : Password.GetHashCode());
            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(Secret) ? 0 : Secret.GetHashCode());
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
        if (obj is not TgSqlTableProxyModel item)
            return false;
        return Equals(Uid, item.Uid) && Equals(Type, item.Type) && Equals(HostName, item.HostName) &&
               Equals(Port, item.Port) && Equals(UserName, item.UserName) &&
               Equals(Password, item.Password) && Equals(Secret, item.Secret);
    }

    #endregion
}