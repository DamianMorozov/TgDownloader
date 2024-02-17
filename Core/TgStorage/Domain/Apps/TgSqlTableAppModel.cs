// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Apps;

/// <summary>
/// SQL table APPS.
/// Do not make base class!
/// </summary>
[Persistent(TgSqlConstants.TableApps)]
[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgSqlTableAppModel : XPLiteObject, ITgSqlTable
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

	private Guid _apiHash;
	[DefaultValue("00000000-0000-0000-0000-000000000000")]
	[Persistent(TgSqlConstants.ColumnApiHash)]
	[Indexed]
	public Guid ApiHash { get => _apiHash; set => SetPropertyValue(nameof(_apiHash), ref _apiHash, value); }

	private int _apiId;
	[DefaultValue(0)]
	[Persistent(TgSqlConstants.ColumnApiId)]
	[Indexed]
	public int ApiId { get => _apiId; set => SetPropertyValue(nameof(_apiId), ref _apiId, value); }

	private string _phoneNumber;
	[DefaultValue("")]
	[Persistent(TgSqlConstants.ColumnPhoneNumber)]
	[Size(16)]
	[Indexed]
	public string PhoneNumber { get => _phoneNumber; set => SetPropertyValue(nameof(_phoneNumber), ref _phoneNumber, value); }

	private Guid _proxyUid;
	[DefaultValue("00000000-0000-0000-0000-000000000000")]
	//[Association]
	[Persistent(TgSqlConstants.ColumnProxyUid)]
	[Indexed]
	public Guid ProxyUid { get => _proxyUid; set => SetPropertyValue(nameof(_proxyUid), ref _proxyUid, value); }

	/// <summary>
	/// Default constructor.
	/// </summary>
	public TgSqlTableAppModel()
	{
		_uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
		_apiHash = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(ApiHash));
		_apiId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(ApiId));
		_phoneNumber = this.GetPropertyDefaultValue(nameof(PhoneNumber));
		_proxyUid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(ProxyUid));
	}

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    /// <param name="session"></param>
    public TgSqlTableAppModel(Session session) : base(session)
	{
        _uid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
        _apiHash = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(ApiHash));
        _apiId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(ApiId));
        _phoneNumber = this.GetPropertyDefaultValue(nameof(PhoneNumber));
        _proxyUid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(ProxyUid));
    }

    public void Fill(TgSqlTableAppModel item, Guid? uid = null)
	{
		_uid = uid ?? this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(Uid));
        if (item is { } app)
        {
            _apiHash = app.ApiHash;
		    _apiId = app.ApiId;
		    _phoneNumber = app.PhoneNumber;
		    _proxyUid = app.ProxyUid;
        }
        else
        {
            _apiHash = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(ApiHash));
            _apiId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(ApiId));
            _phoneNumber = this.GetPropertyDefaultValue(nameof(PhoneNumber));
            _proxyUid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(ProxyUid));
        }
    }

    #endregion

	#region Public and private methods

	public override string ToString() => $"{ApiHash} | {ApiId} | {PhoneNumber} | {ProxyUid}";

    public string ToDebugString() => $"{TgCommonUtils.GetIsExists(IsExists)} | {Uid} | {ApiHash} | {ApiId} | {PhoneNumber} | {ProxyUid}";

    public void Clear()
	{
		ApiId = 0;
		ApiHash = Guid.Empty;
		PhoneNumber = string.Empty;
		ProxyUid = Guid.Empty;
	}

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Uid.GetHashCode();
            hashCode = (hashCode * 397) ^ ApiHash.GetHashCode();
            hashCode = (hashCode * 397) ^ ApiId.GetHashCode();
            hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(PhoneNumber) ? 0 : PhoneNumber.GetHashCode());
            hashCode = (hashCode * 397) ^ ProxyUid.GetHashCode();
            return hashCode;
        }
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        if (obj is not TgSqlTableAppModel item) return false;
        return Equals(Uid, item.Uid) && Equals(ApiHash, item.ApiHash) && Equals(ApiId, item.ApiId) && 
               Equals(PhoneNumber, item.PhoneNumber) && Equals(ProxyUid, item.ProxyUid);
    }

    #endregion
}