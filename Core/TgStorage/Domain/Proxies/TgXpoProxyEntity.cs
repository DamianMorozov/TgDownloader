//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//using DevExpress.Xpo;

//namespace TgStorage.Domain.Proxies;

///// <summary>
///// SQL table PROXIES.
///// Do not make base class!
///// </summary>
//[DebuggerDisplay("{ToDebugString()}")]
//[Persistent(TgStorageConstants.TableProxies)]
//[DoNotNotify]
//public sealed class TgXpoProxyEntity : XPLiteObject, ITgDbProxy
//{
//	#region Public and private fields, properties, constructor

//    private Guid _uid;
//	[DevExpress.Xpo.Key(true)]
//	[DefaultValue("00000000-0000-0000-0000-000000000000")]
//	[Persistent(TgStorageConstants.ColumnUid)]
//	[Indexed]
//	public Guid Uid { get => _uid; set => SetPropertyValue(nameof(_uid), ref _uid, value); }
//	[NonPersistent]
//	public string UidString
//	{
//		get => Uid.ToString();
//		set => Uid = Guid.TryParse(value, out Guid uid) ? uid : Guid.Empty;
//	}
//	[NonPersistent]
//	public bool IsExist => !Equals(Uid, Guid.Empty);
//	[NonPersistent]
//	public bool NotExist => !IsExist;
//	[NonPersistent]
//	public TgEnumLetterCase LetterCase { get; set; }

//	private TgEnumProxyType _type;
//	[DefaultValue(TgEnumProxyType.None)]
//	[Persistent(TgStorageConstants.ColumnType)]
//	[Indexed]
//	public TgEnumProxyType Type { get => _type; set => SetPropertyValue(nameof(_type), ref _type, value); }

//	private string _hostName = default!;
//	[DefaultValue("No proxy")]
//	[Persistent(TgStorageConstants.ColumnHostName)]
//	[Size(128)]
//	[Indexed]
//	public string HostName { get => _hostName; set => SetPropertyValue(nameof(_hostName), ref _hostName, value); }

//	private ushort _port;
//	[DefaultValue(404)]
//	[Persistent(TgStorageConstants.ColumnPort)]
//	[Indexed]
//	public ushort Port { get => _port; set => SetPropertyValue(nameof(_port), ref _port, value); }

//	private string _userName = default!;
//	[DefaultValue("No user")]
//	[Persistent(TgStorageConstants.ColumnUserName)]
//	[Size(128)]
//	[Indexed]
//	public string UserName { get => _userName; set => SetPropertyValue(nameof(_userName), ref _userName, value); }

//	private string _password = default!;
//	[DefaultValue("No password")]
//	[Persistent(TgStorageConstants.ColumnPassword)]
//	[Size(128)]
//	[Indexed]
//	public string Password { get => _password; set => SetPropertyValue(nameof(_password), ref _password, value); }

//	private string _secret = default!;
//	[DefaultValue("")]
//	[Persistent(TgStorageConstants.ColumnSecret)]
//	[Size(128)]
//	[Indexed]
//	public string Secret { get => _secret; set => SetPropertyValue(nameof(_secret), ref _secret, value); }

//	/// <summary>
//	/// Default constructor.
//	/// </summary>
//	public TgXpoProxyEntity()
//	{
//		Default();
//	}

//    /// <summary>
//	/// Default constructor with session.
//	/// </summary>
//	/// <param name="session"></param>
//    public TgXpoProxyEntity(Session session) : base(session)
//	{
//		Default();
//	}

//	#endregion

//	#region Public and private methods

//	public void Default()
//	{
//		Uid = this.GetDefaultPropertyGuid(nameof(Uid));
//		Type = this.GetDefaultPropertyGeneric<TgEnumProxyType>(nameof(Type));
//		HostName = this.GetDefaultPropertyString(nameof(HostName));
//		Port = this.GetDefaultPropertyUshort(nameof(Port));
//		UserName = this.GetDefaultPropertyString(nameof(UserName));
//		Password = this.GetDefaultPropertyString(nameof(Password));
//		Secret = this.GetDefaultPropertyString(nameof(Secret));
//	}

//	public void Fill(object item)
//    {
//	    if (item is not TgXpoProxyEntity proxy)
//		    throw new ArgumentException($"The {nameof(item)} is not {nameof(TgXpoProxyEntity)}!");

//	    Type = proxy.Type;
//	    HostName = proxy.HostName;
//	    Port = proxy.Port;
//	    UserName = proxy.UserName;
//	    Password = proxy.Password;
//	    Secret = proxy.Secret;
//    }

//	public void Backup(object item)
//	{
//		Fill(item);
//		Uid = (item as TgXpoProxyEntity)!.Uid;
//	}

//	public override string ToString() => 
//		$"{Type} | {HostName} | {Port} | {UserName} | {Password} | {Secret}";

//	public string ToDebugString() => 
//		$"{TgStorageConstants.TableProxies} | {Uid} | {Type} | {HostName} | {Port} | {UserName} | {Password} | " +
//        $"{TgCommonUtils.GetIsFlag(!string.IsNullOrEmpty(Secret), Secret, "<No secret>")}";

//    public override int GetHashCode()
//    {
//        unchecked
//        {
//            int hashCode = Uid.GetHashCode();
//            hashCode = (hashCode * 397) ^ Type.GetHashCode();
//            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(HostName) ? 0 : HostName.GetHashCode());
//            hashCode = (hashCode * 397) ^ Port.GetHashCode();
//            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(UserName) ? 0 : UserName.GetHashCode());
//            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(Password) ? 0 : Password.GetHashCode());
//            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(Secret) ? 0 : Secret.GetHashCode());
//            return hashCode;
//        }
//    }

//    public override bool Equals(object? obj)
//    {
//        if (ReferenceEquals(null, obj))
//            return false;
//        if (ReferenceEquals(this, obj))
//            return true;
//        if (obj.GetType() != GetType())
//            return false;
//        if (obj is not TgXpoProxyEntity item)
//            return false;
//        return Equals(Uid, item.Uid) && Equals(Type, item.Type) && Equals(HostName, item.HostName) &&
//               Equals(Port, item.Port) && Equals(UserName, item.UserName) &&
//               Equals(Password, item.Password) && Equals(Secret, item.Secret);
//    }

//    #endregion
//}