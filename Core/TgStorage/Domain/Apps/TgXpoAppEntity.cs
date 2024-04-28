//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//using DevExpress.Xpo;

//namespace TgStorage.Domain.Apps;

///// <summary>
///// SQL table APPS.
///// Do not make base class!
///// </summary>
//[Persistent(TgStorageConstants.TableApps)]
//[DoNotNotify]
//[DebuggerDisplay("{ToDebugString()}")]
//public sealed class TgXpoAppEntity : XPLiteObject, ITgDbEntity
//{
//	#region Public and private fields, properties, constructor

//	private Guid _uid;
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
//    public bool NotExist => !IsExist;
//    [NonPersistent]
//	public TgEnumLetterCase LetterCase { get; set; }

//	private Guid _apiHash;
//	[DefaultValue("00000000-0000-0000-0000-000000000000")]
//	[Persistent(TgStorageConstants.ColumnApiHash)]
//	[Indexed]
//	public Guid ApiHash { get => _apiHash; set => SetPropertyValue(nameof(_apiHash), ref _apiHash, value); }

//	private int _apiId;
//	[DefaultValue(0)]
//	[Persistent(TgStorageConstants.ColumnApiId)]
//	[Indexed]
//	public int ApiId { get => _apiId; set => SetPropertyValue(nameof(_apiId), ref _apiId, value); }

//	private string _phoneNumber = default!;

//	[DefaultValue("+00000000000")]
//	[Persistent(TgStorageConstants.ColumnPhoneNumber)]
//	[Size(16)]
//	[Indexed]
//	public string PhoneNumber
//	{
//		get => _phoneNumber;
//		set => SetPropertyValue(nameof(_phoneNumber), ref _phoneNumber, value);
//	}

//	private Guid _proxyUid;
//	[DefaultValue("00000000-0000-0000-0000-000000000000")]
//	//[Association]
//	[Persistent(TgStorageConstants.ColumnProxyUid)]
//	[Indexed]
//	public Guid ProxyUid { get => _proxyUid; set => SetPropertyValue(nameof(_proxyUid), ref _proxyUid, value); }

//	/// <summary>
//	/// Default constructor.
//	/// </summary>
//	public TgXpoAppEntity() : base()
//	{
//		Default();
//	}

//    /// <summary>
//    /// Default constructor with session.
//    /// </summary>
//    /// <param name="session"></param>
//    public TgXpoAppEntity(Session session) : base(session)
//	{
//		Default();
//    }

//    #endregion

//    #region Public and private methods

//    public void Default()
//    {
//	    Uid = this.GetDefaultPropertyGuid(nameof(Uid));
//	    ApiHash = this.GetDefaultPropertyGuid(nameof(ApiHash));
//	    ApiId = this.GetDefaultPropertyInt(nameof(ApiId));
//	    PhoneNumber = this.GetDefaultPropertyString(nameof(PhoneNumber));
//	    ProxyUid = this.GetDefaultPropertyGuid(nameof(ProxyUid));
//    }

//	public void Fill(object item)
//	{
//		if (item is not TgXpoAppEntity app)
//			throw new ArgumentException($"The {nameof(item)} is not {nameof(TgXpoAppEntity)}!");

//		ApiHash = app.ApiHash;
//		ApiId = app.ApiId;
//		PhoneNumber = app.PhoneNumber;
//		ProxyUid = app.ProxyUid;
//    }

//	public void Backup(object item)
//	{
//		Fill(item);
//		Uid = (item as TgXpoAppEntity)!.Uid;
//	}

//	public override string ToString() => $"{ApiHash} | {ApiId} | {PhoneNumber} | {ProxyUid}";

//    public string ToDebugString() => 
//	    $"{TgStorageConstants.TableApps} | {TgCommonUtils.GetIsExists(IsExist)} | {Uid} | {ApiHash} | {ApiId} | {PhoneNumber} | {ProxyUid}";

//    public void Clear()
//	{
//		ApiId = 0;
//		ApiHash = Guid.Empty;
//		PhoneNumber = string.Empty;
//		ProxyUid = Guid.Empty;
//	}

//    public override int GetHashCode()
//    {
//        unchecked
//        {
//            int hashCode = Uid.GetHashCode();
//            hashCode = (hashCode * 397) ^ ApiHash.GetHashCode();
//            hashCode = (hashCode * 397) ^ ApiId.GetHashCode();
//            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(PhoneNumber) ? 0 : PhoneNumber.GetHashCode());
//            hashCode = (hashCode * 397) ^ ProxyUid.GetHashCode();
//            return hashCode;
//        }
//    }

//    public override bool Equals(object? obj)
//    {
//        if (ReferenceEquals(null, obj)) return false;
//        if (ReferenceEquals(this, obj)) return true;
//        if (obj.GetType() != GetType()) return false;
//        if (obj is not TgXpoAppEntity item) return false;
//        return Equals(Uid, item.Uid) && Equals(ApiHash, item.ApiHash) && Equals(ApiId, item.ApiId) && 
//               Equals(PhoneNumber, item.PhoneNumber) && Equals(ProxyUid, item.ProxyUid);
//    }

//    #endregion
//}