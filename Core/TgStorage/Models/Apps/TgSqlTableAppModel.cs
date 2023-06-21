// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Apps;

[DebuggerDisplay("{ToString()}")]
[Persistent(TgSqlConstants.TableApps)]
public sealed class TgSqlTableAppModel : TgSqlTableBase
{
	#region Public and private fields, properties, constructor

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
	public TgSqlTableAppModel() : base()
	{
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
		_apiHash = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(ApiHash));
		_apiId = this.GetPropertyDefaultValueAsGeneric<int>(nameof(ApiId));
		_phoneNumber = this.GetPropertyDefaultValue(nameof(PhoneNumber));
		_proxyUid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(ProxyUid));
	}

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public TgSqlTableAppModel(SerializationInfo info, StreamingContext context) : base(info, context)
	{
		object? apiHash = info.GetValue(nameof(ProxyUid), typeof(Guid));
		_apiHash = apiHash is Guid apiHash2 ? apiHash2 : this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(ApiHash));
		_apiId = info.GetInt32(nameof(ApiId));
		_phoneNumber = info.GetString(nameof(PhoneNumber)) ?? this.GetPropertyDefaultValue(nameof(PhoneNumber));
        object? proxy = info.GetValue(nameof(ProxyUid), typeof(Guid));
        _proxyUid = proxy is Guid proxy2 ? proxy2 : this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(ApiHash));
	}

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public new void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ApiHash), ApiHash);
        info.AddValue(nameof(ApiId), ApiId);
        info.AddValue(nameof(PhoneNumber), PhoneNumber);
        info.AddValue(nameof(ProxyUid), ProxyUid);
    }

    #endregion

    #region Public and private methods

    public override string ToString() =>
		$"{nameof(ApiHash)} = {ApiHash} | " +
		$"{nameof(ApiId)} = {ApiId} | " +
        $"{nameof(PhoneNumber)} = {PhoneNumber} | " +
        $"{nameof(ProxyUid)} = {ProxyUid}";

    public void Clear()
    {
	    ApiId = 0;
ApiHash = Guid.Empty;
PhoneNumber = string.Empty;
ProxyUid = Guid.Empty;
    }

    #endregion
}