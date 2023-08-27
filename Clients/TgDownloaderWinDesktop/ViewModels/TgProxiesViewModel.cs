// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgProxiesViewModel : TgPageViewModelBase, INavigationAware
{
	#region Public and private fields, properties, constructor

	public ObservableCollection<TgSqlTableProxyViewModel> ProxiesVms { get; set; } = new();

    #endregion

	#region Public and private methods

	public void OnNavigatedTo()
	{
		if (!IsInitialized)
			InitializeViewModel();
	}

	public void OnNavigatedFrom()
    {
        //
    }

    protected override void InitializeViewModel()
    {
        base.InitializeViewModel();
        // Load sources from storage.
        LoadProxiesFromStorageCommand.Execute(null);
    }
	
    /// <summary>
    /// Sort proxies.
    /// </summary>
    private void SetOrderProxies(IEnumerable<TgSqlTableProxyModel> proxies)
	{
		proxies = proxies.OrderBy(x => x.Port).ToList()
			.OrderBy(x => x.HostName);
		ProxiesVms.Clear();
		foreach (TgSqlTableProxyModel proxy in proxies)
			ProxiesVms.Add(new(proxy, ConnectClientByProxy, DisconnectClient, DeleteProxy));
	}

    /// <summary>
    /// Connect client through proxy.
    /// </summary>
    /// <param name="proxyVm"></param>
    public void ConnectClientByProxy(TgSqlTableProxyViewModel proxyVm)
    {
        _ = Task.Run(async () =>
        {
            await TgDesktopUtils.TgClientVm.OnClientConnectAsync().ConfigureAwait(false);
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Disconnect client.
    /// </summary>
    /// <param name="proxyVm"></param>
    public void DisconnectClient(TgSqlTableProxyViewModel proxyVm)
    {
        _ = Task.Run(async () =>
        {
            await TgDesktopUtils.TgClientVm.OnClientDisconnectAsync().ConfigureAwait(false);
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Delete proxy.
    /// </summary>
    /// <param name="proxyVm"></param>
    public void DeleteProxy(TgSqlTableProxyViewModel proxyVm)
    {
        ContextManager.ProxyRepository.Delete(proxyVm.Proxy);
        LoadProxies();
        TgDesktopUtils.TgClientVm.LoadProxiesForClient();
    }

    private void LoadProxies()
    {
        TgDispatcherUtils.DispatcherUpdateMainWindow(() =>
        {
            SetOrderProxies(ContextManager.ProxyRepository.GetEnumerable());
        });
    }

    #endregion

    #region Public and private methods - RelayCommand

    [RelayCommand]
	public async Task OnLoadProxiesFromStorageAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgDesktopUtils.RunAction(this, LoadProxies);
	}

	[RelayCommand]
	public async Task OnClearViewAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgDesktopUtils.RunAction(this, ProxiesVms.Clear);
	}

	#endregion
}