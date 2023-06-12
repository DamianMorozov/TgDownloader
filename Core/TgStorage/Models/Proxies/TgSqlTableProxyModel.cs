// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Proxies;

[DebuggerDisplay("{ToString()}")]
[Persistent(TgSqlConstants.TableProxies)]
public sealed class TgSqlTableProxyModel : TgSqlTableBase
{
    #region Public and private fields, properties, constructor

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
	public TgSqlTableProxyModel() : base()
	{
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
		_type = this.GetPropertyDefaultValueAsGeneric<TgEnumProxyType>(nameof(Type));
		_hostName = this.GetPropertyDefaultValue(nameof(HostName));
		_port = this.GetPropertyDefaultValueAsGeneric<ushort>(nameof(Port));
		_userName = this.GetPropertyDefaultValue(nameof(UserName));
		_password = this.GetPropertyDefaultValue(nameof(Password));
		_secret = this.GetPropertyDefaultValue(nameof(Secret));
	}

	#endregion

	#region Public and private methods - ISerializable

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	public TgSqlTableProxyModel(SerializationInfo info, StreamingContext context) : base(info, context)
    {
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
	public new void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Type), Type);
        info.AddValue(nameof(HostName), HostName);
        info.AddValue(nameof(Port), Port);
        info.AddValue(nameof(UserName), UserName);
        info.AddValue(nameof(Password), Password);
        info.AddValue(nameof(Secret), Secret);
        //info.AddValue(nameof(App), App);
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