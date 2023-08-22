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
	public Action<TgSqlTableProxyViewModel> ConnectProxy { get; set; }
	public string PrettyName => $"{Proxy.Type} | {Proxy.HostName} | {Proxy.Port} | {Proxy.UserName}";
	public TgSqlTableProxyViewModel(TgSqlTableProxyModel proxy, Action<TgSqlTableProxyViewModel> connectProxy)
	{
		Proxy = proxy;
		ConnectProxy = connectProxy;
	}

	public TgSqlTableProxyViewModel(TgSqlTableProxyModel proxy)
	{
		Proxy = proxy;
		ConnectProxy = _ => { };
	}

	#endregion

	#region Public and private methods

	public override string ToString() => PrettyName;

	[RelayCommand]
	public async Task OnConnectProxyAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgAsyncUtils.ExecAction(() =>
		{
			ConnectProxy(this);
		});
	}

	#endregion
}