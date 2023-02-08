// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;

namespace TgStorage.Models.Apps;

[DebuggerDisplay("{nameof(SqlTableAppModel)} | {Uid} | {ApiHash} | {PhoneNumber}")]
[Persistent("APPS")]
public class SqlTableAppModel : SqlTableXpLiteBase
{
    #region Public and private fields, properties, constructor

    [DefaultValue("")]
    private string _apiHash;
    [Size(32)]
    [Persistent("API_HASH")]
    public string ApiHash { get => _apiHash; set => SetPropertyValue(nameof(_apiHash), ref _apiHash, value); }

    [DefaultValue("")]
    private string _phoneNumber;
    [Size(32)]
    [Persistent("PHONE_NUMBER")]
    public string PhoneNumber { get => _phoneNumber; set => SetPropertyValue(nameof(_phoneNumber), ref _phoneNumber, value); }

    [DefaultValue("00000000-0000-0000-0000-000000000000")]
    private Guid _proxyUid;
    [Persistent("PROXY_UID")]
    public Guid ProxyUid { get => _proxyUid; set => SetPropertyValue(nameof(_proxyUid), ref _proxyUid, value); }

    [DefaultValue("11")]
    private ushort _dbVersion;
    [Persistent("DB_VERSION")]
    public ushort DbVersion { get => _dbVersion; set => SetPropertyValue(nameof(_dbVersion), ref _dbVersion, value); }
    
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqlTableAppModel()
    {
        _apiHash = this.GetPropertyDefaultValue(nameof(_apiHash));
        _phoneNumber = this.GetPropertyDefaultValue(nameof(_phoneNumber));
        _proxyUid = this.GetPropertyDefaultValueAsGeneric<Guid>(nameof(_proxyUid));
        _dbVersion = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(_dbVersion));
    }

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    /// <param name="session"></param>
    public SqlTableAppModel(Session session) : base(session)
    {
        _apiHash = this.GetPropertyDefaultValue(nameof(_apiHash));
        _phoneNumber = this.GetPropertyDefaultValue(nameof(_phoneNumber));
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
        _apiHash = info.GetString(nameof(_apiHash)) ?? this.GetPropertyDefaultValue(nameof(_apiHash));
        _phoneNumber = info.GetString(nameof(_phoneNumber)) ?? this.GetPropertyDefaultValue(nameof(_phoneNumber));
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
        $"{nameof(ProxyUid)} = {ProxyUid} | " +
        $"{nameof(DbVersion)} = {DbVersion}";

    #endregion

    public ushort GetLastDbVersion()
    {
        //ushort foo = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(_dbVersion));
        string str = this.GetPropertyDefaultValue(nameof(_dbVersion));
        return ushort.TryParse(str, out ushort result) ? result : default;
    }

    #endregion
}