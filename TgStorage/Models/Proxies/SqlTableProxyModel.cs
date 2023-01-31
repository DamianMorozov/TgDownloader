// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;
using TgStorage.Enums;

namespace TgStorage.Models.Proxies;

[DebuggerDisplay("{nameof(SqlTableProxyModel)} | {Uid}")]
[Persistent("PROXIES")]
public class SqlTableProxyModel : SqlTableXpLiteBase
{
    #region Public and private fields, properties, constructor

    private ProxyType _type;
    [DefaultValue("None")]
    [Persistent("TYPE")]
    public ProxyType Type { get => _type; set => SetPropertyValue(nameof(_type), ref _type, value); }

    private string _hostName;
    [Size(128)]
    [DefaultValue("")]
    [Persistent("HOST_NAME")]
    public string HostName { get => _hostName; set => SetPropertyValue(nameof(_hostName), ref _hostName, value); }

    private ushort _port;
    [DefaultValue("0")]
    [Persistent("PORT")]
    public ushort Port { get => _port; set => SetPropertyValue(nameof(_port), ref _port, value); }

    private string _userName;
    [Size(128)]
    [DefaultValue("")]
    [Persistent("USER_NAME")]
    public string UserName { get => _userName; set => SetPropertyValue(nameof(_userName), ref _userName, value); }

    private string _password;
    [Size(128)]
    [DefaultValue("")]
    [Persistent("PASSWORD")]
    public string Password { get => _password; set => SetPropertyValue(nameof(_password), ref _password, value); }

    private string _secret;
    [Size(128)]
    [DefaultValue("")]
    [Persistent("SECRET")]
    public string Secret { get => _secret; set => SetPropertyValue(nameof(_secret), ref _secret, value); }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqlTableProxyModel()
    {
        _type = this.GetPropertyDefaultValueAsGeneric<ProxyType>(nameof(_type));
        _hostName = this.GetPropertyDefaultValueAsString(nameof(_hostName));
        _port = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(_port));
        _userName = this.GetPropertyDefaultValueAsString(nameof(_userName));
        _password = this.GetPropertyDefaultValueAsString(nameof(_password));
        _secret = this.GetPropertyDefaultValueAsString(nameof(_secret));
    }

    /// <summary>
    /// Default constructor with session.
    /// </summary>
    /// <param name="session"></param>
    public SqlTableProxyModel(Session session) : base(session)
    {
        _type = this.GetPropertyDefaultValueAsGeneric<ProxyType>(nameof(_type));
        _hostName = this.GetPropertyDefaultValueAsString(nameof(_hostName));
        _port = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(_port));
        _userName = this.GetPropertyDefaultValueAsString(nameof(_userName));
        _password = this.GetPropertyDefaultValueAsString(nameof(_password));
        _secret = this.GetPropertyDefaultValueAsString(nameof(_secret));
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected SqlTableProxyModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        object? type = info.GetValue(nameof(_type), typeof(ProxyType));
        _type = type is ProxyType proxyType ? proxyType : ProxyType.None;
        _hostName = info.GetString(nameof(_hostName)) ?? string.Empty;
        _port = info.GetUInt16(nameof(_port));
        _userName = info.GetString(nameof(_userName)) ?? string.Empty;
        _password = info.GetString(nameof(_password)) ?? string.Empty;
        _secret = info.GetString(nameof(_secret)) ?? string.Empty;
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public new void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(_type), _type);
        info.AddValue(nameof(_hostName), _hostName);
        info.AddValue(nameof(_port), _port);
        info.AddValue(nameof(_userName), _userName);
        info.AddValue(nameof(_password), _password);
        info.AddValue(nameof(_secret), _secret);
    }

    #endregion

    #region Public and private methods

    public override string ToString() =>
        base.ToString() + " | " +
        $"{nameof(Type)} = {Type} | " +
        $"{nameof(HostName)} = {HostName} | " +
        $"{nameof(Port)} = {Port} | " +
        $"{nameof(UserName)} = {UserName} | " +
        $"{nameof(Password)} = {Password} | " +
        $"{nameof(Secret)} = {Secret}";

    #endregion
}