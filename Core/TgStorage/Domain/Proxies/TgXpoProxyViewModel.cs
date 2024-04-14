// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Proxies;

/// <summary>
/// View for TgSqlTableSourceModel.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgXpoProxyViewModel : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	public TgXpoProxyEntity Proxy { get; set; }
    public Guid ProxyUid
    {
	    get => Proxy.Uid;
	    set
	    {
		    TgXpoOperResult<TgXpoProxyEntity> operResult = XpoContext.ProxyRepository.GetAsync(value).GetAwaiter().GetResult();
		    Proxy = operResult.IsExist
			    ? operResult.Item
			    : XpoContext.ProxyRepository.GetNewAsync().GetAwaiter().GetResult().Item;
	    }
    }

    [DefaultValue(0)]
    public TgEnumProxyType ProxyType { get => Proxy.Type; set => Proxy.Type = value; }
    [DefaultValue("")]
    public string ProxyHostName { get => Proxy.HostName; set => Proxy.HostName = value; }
    [DefaultValue(0)]
    public ushort ProxyPort { get => Proxy.Port; set => Proxy.Port = value; }
    [DefaultValue("")]
    public string ProxyUserName { get => Proxy.UserName; set => Proxy.UserName = value; }
    [DefaultValue("")]
    public string ProxyPassword { get => Proxy.Password; set => Proxy.Password = value; }
    [DefaultValue("")]
    public string ProxySecret { get => Proxy.Secret; set => Proxy.Secret = value; }

    public string PrettyName => $"{Proxy.Type} | {TgDataFormatUtils.GetFormatString(Proxy.HostName, 30)} | {Proxy.Port} | {Proxy.UserName}";

    public TgXpoProxyViewModel(TgXpoProxyEntity proxy)
	{
		Proxy = proxy;
        ProxyType = proxy.Type;
        ProxyHostName = proxy.HostName;
        ProxyPort = proxy.Port;
        ProxyUserName = proxy.UserName;
        ProxyPassword = proxy.Password;
        ProxySecret = proxy.Secret;
    }

    #endregion

    #region Public and private methods

    public override string ToString() => PrettyName;

	#endregion
}