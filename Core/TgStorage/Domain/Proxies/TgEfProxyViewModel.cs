// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Proxies;

/// <summary> View-model for TgSqlTableSourceModel </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgEfProxyViewModel : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	private TgEfProxyRepository ProxyRepository { get; } = new(TgEfUtils.EfContext);
	public TgEfProxyEntity Proxy { get; set; }
    public Guid ProxyUid
    {
	    get => Proxy.Uid;
	    set
	    {
		    TgEfOperResult<TgEfProxyEntity> operResult = ProxyRepository.GetAsync(
			    new TgEfProxyEntity { Uid = value }, isNoTracking: false).GetAwaiter().GetResult();
		    Proxy = operResult.IsExists
			    ? operResult.Item
			    : ProxyRepository.GetNewAsync(isNoTracking: false).GetAwaiter().GetResult().Item;
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

    public TgEfProxyViewModel(TgEfProxyEntity proxy) : base()
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