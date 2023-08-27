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
	public Action<TgSqlTableProxyViewModel> ConnectClientByProxy { get; set; }
	public Action<TgSqlTableProxyViewModel> DisconnectClient { get; set; }
	public Action<TgSqlTableProxyViewModel> DeleteProxy { get; set; }

	public string PrettyName => $"{Proxy.Type} | {TgDataFormatUtils.GetFormatString(Proxy.HostName, 20)} | {Proxy.Port} | {Proxy.UserName}";

	public TgSqlTableProxyViewModel(TgSqlTableProxyModel proxy, 
        Action<TgSqlTableProxyViewModel> connectClient, Action<TgSqlTableProxyViewModel> disconnectClient, 
        Action<TgSqlTableProxyViewModel> deleteProxy)
	{
		Proxy = proxy;
		ConnectClientByProxy = connectClient;
        DisconnectClient = disconnectClient;
        DeleteProxy = deleteProxy;
    }

	public TgSqlTableProxyViewModel(TgSqlTableProxyModel proxy)
	{
		Proxy = proxy;
		ConnectClientByProxy = _ => { };
		DisconnectClient = _ => { };
        DeleteProxy = _ => { };
	}

	#endregion

	#region Public and private methods

	public override string ToString() => PrettyName;

	[RelayCommand]
	public async Task OnConnectClientByProxyAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgAsyncUtils.ExecAction(() =>
		{
			ConnectClientByProxy(this);
		});
	}

	[RelayCommand]
	public async Task OnDisconnectClientAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgAsyncUtils.ExecAction(() =>
		{
			DisconnectClient(this);
		});
	}

	[RelayCommand]
	public async Task OnDeleteProxyAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgAsyncUtils.ExecAction(() =>
		{
            DeleteProxy(this);
        });
	}

	#endregion
}