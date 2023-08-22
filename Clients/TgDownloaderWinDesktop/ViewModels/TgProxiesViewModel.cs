// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgProxiesViewModel : TgPageViewModelBase, INavigationAware
{
	#region Public and private fields, properties, constructor

	public ObservableCollection<TgSqlTableProxyViewModel> ProxiesVms { get; set; }

	public TgProxiesViewModel()
	{
		ProxiesVms = new();
	}

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

	/// <summary>
	/// Sort proxies.
	/// </summary>
	private void SetOrderProxies(IEnumerable<TgSqlTableProxyModel> proxies)
	{
		proxies = proxies.OrderBy(x => x.Port).ToList()
			.OrderBy(x => x.HostName);
		ProxiesVms.Clear();
		foreach (TgSqlTableProxyModel proxy in proxies)
			ProxiesVms.Add(new(proxy, ConnectProxy));
	}

	/// <summary>
	/// TODO: Connect proxy.
	/// </summary>
	/// <param name="proxyVm"></param>
	public void ConnectProxy(TgSqlTableProxyViewModel proxyVm)
	{
		// Checks.
		if (!CheckClientReady())
			return;
	}

	#endregion

	#region Public and private methods - RelayCommand

	[RelayCommand]
	public async Task OnLoadProxiesFromStorageAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgDesktopUtils.RunAction(this, () =>
		{
			SetOrderProxies(ContextManager.ProxyRepository.GetEnumerable());
		});
	}

	[RelayCommand]
	public async Task OnClearViewAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgDesktopUtils.RunAction(this, ProxiesVms.Clear);
	}

	#endregion
}