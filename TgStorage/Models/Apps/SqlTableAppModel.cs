// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;

namespace TgStorage.Models.Apps;

[DebuggerDisplay("{nameof(SqlTableAppModel)} | {Uid} | {ApiHash} | {PhoneNumber}")]
[Persistent("APPS")]
public class SqlTableAppModel : SqlTableXpLiteBase
{
    #region Public and private fields, properties, constructor

    private string _apiHash;
    [Size(32)]
    [DefaultValue("")]
    [Persistent("API_HASH")]
    public string ApiHash { get => _apiHash; set => SetPropertyValue(nameof(_apiHash), ref _apiHash, value); }

    private string _phoneNumber;
    [Size(32)]
    [DefaultValue("")]
    [Persistent("PHONE_NUMBER")]
    public string PhoneNumber { get => _phoneNumber; set => SetPropertyValue(nameof(_phoneNumber), ref _phoneNumber, value); }

    private bool _isUseProxy;
    [DefaultValue(0)]
    [Persistent("IS_USE_PROXY")]
    public bool IsUseProxy { get => _isUseProxy; set => SetPropertyValue(nameof(_isUseProxy), ref _isUseProxy, value); }

    private Guid _proxyUid;
    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    [Persistent("PROXY_UID")]
    public Guid ProxyUid { get => _proxyUid; set => SetPropertyValue(nameof(_proxyUid), ref _proxyUid, value); }

    private ushort _dbVersion;
    [DefaultValue("0")]
    [Persistent("DB_VERSION")]
    public ushort DbVersion { get => _dbVersion; set => SetPropertyValue(nameof(_dbVersion), ref _dbVersion, value); }
    
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqlTableAppModel()
    {
        _apiHash = this.GetPropertyDefaultValueAsString(nameof(_apiHash));
        _phoneNumber = this.GetPropertyDefaultValueAsString(nameof(_phoneNumber));
        _isUseProxy = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(_isUseProxy));
        _proxyUid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(_proxyUid));
        _dbVersion = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(_dbVersion));
    }

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    /// <param name="session"></param>
    public SqlTableAppModel(Session session) : base(session)
    {
        _apiHash = this.GetPropertyDefaultValueAsString(nameof(_apiHash));
        _phoneNumber = this.GetPropertyDefaultValueAsString(nameof(_phoneNumber));
        _isUseProxy = this.GetPropertyDefaultValueAsGeneric<bool>(nameof(_isUseProxy));
        _proxyUid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(_proxyUid));
        _dbVersion = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(_dbVersion));
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected SqlTableAppModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        _apiHash = info.GetString(nameof(_apiHash)) ?? this.GetPropertyDefaultValueAsString(nameof(_apiHash));
        _phoneNumber = info.GetString(nameof(_phoneNumber)) ?? this.GetPropertyDefaultValueAsString(nameof(_phoneNumber));
        _isUseProxy = info.GetBoolean(nameof(_isUseProxy));
        object? proxyUid = info.GetValue(nameof(_proxyUid), typeof(Guid));
        _proxyUid = proxyUid is Guid pUid ? pUid : this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(_proxyUid));
        _dbVersion = info.GetUInt16(nameof(_dbVersion));
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public new void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(_apiHash), _apiHash);
        info.AddValue(nameof(_phoneNumber), _phoneNumber);
        info.AddValue(nameof(_isUseProxy), _isUseProxy);
        info.AddValue(nameof(_proxyUid), _proxyUid);
        info.AddValue(nameof(_dbVersion), _dbVersion);
    }

    #endregion

    #region Public and private methods

    #region Public and private methods

    public override string ToString() =>
        base.ToString() + " | " +
        $"{nameof(ApiHash)} = {ApiHash} | " +
        $"{nameof(PhoneNumber)} = {PhoneNumber} | " +
        $"{nameof(IsUseProxy)} = {IsUseProxy} | " +
        $"{nameof(ProxyUid)} = {ProxyUid} | " +
        $"{nameof(DbVersion)} = {DbVersion}";

    #endregion

    public void UpdateDbVersion()
    {
        //_dbVersion = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(_dbVersion));
        _dbVersion = 10;
    }

    #endregion
}