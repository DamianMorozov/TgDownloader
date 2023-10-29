// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Proxies;

/// <summary>
/// View for TgSqlTableSourceModel.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSqlTableProxyViewModel : TgViewModelBase
{
	#region Public and private fields, properties, constructor

	public TgSqlTableProxyModel Proxy { get; set; }
    public Guid ProxyUid
    {
        get => Proxy.Uid;
        set => Proxy = TgSqlTableProxyRepository.Instance.Get(value) ?? TgSqlTableProxyRepository.Instance.GetNew();
    }

	public string PrettyName => $"{Proxy.Type} | {TgDataFormatUtils.GetFormatString(Proxy.HostName, 30)} | {Proxy.Port} | {Proxy.UserName}";

	public TgSqlTableProxyViewModel(TgSqlTableProxyModel proxy, Action<TgSqlTableProxyViewModel> deleteProxy)
	{
		Proxy = proxy;
    }

	public TgSqlTableProxyViewModel(TgSqlTableProxyModel proxy)
	{
		Proxy = proxy;
	}

	#endregion

	#region Public and private methods

	public override string ToString() => PrettyName;

	#endregion
}