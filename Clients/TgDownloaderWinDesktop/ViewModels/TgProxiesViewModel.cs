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
		//if (!IsInitialized)
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
        ProxiesVms.Clear();
        proxies = proxies.OrderBy(x => x.Port).ThenBy(x => x.HostName).ToList();
        foreach (TgSqlTableProxyModel proxy in proxies)
            ProxiesVms.Add(new(proxy));
    }

    #endregion

    #region Public and private methods - RelayCommand

    // LoadProxiesFromStorageCommand
    [RelayCommand]
	public async Task OnLoadProxiesFromStorageAsync()
	{
        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            SetOrderProxies(ContextManager.ProxyRepository.GetEnumerable());
        }, false).ConfigureAwait(false);
	}

    // ClearViewCommand
    [RelayCommand]
	public async Task OnClearViewAsync()
	{
        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            ProxiesVms.Clear();
        }, false).ConfigureAwait(true);
	}

    // DeleteProxyCommand
    [RelayCommand]
	public async Task OnDeleteProxyAsync(TgSqlTableProxyViewModel proxyVm)
	{
        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            ContextManager.ProxyRepository.Delete(proxyVm.Proxy);
            LoadProxiesFromStorageCommand.Execute(null);
            TgDesktopUtils.TgClientVm.LoadProxiesForClient();
        }, false).ConfigureAwait(false);
	}

    // EditProxyCommand
    [RelayCommand]
    public async Task OnEditProxyAsync(TgSqlTableProxyViewModel proxyVm)
    {
        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            if (Application.Current.MainWindow is MainWindow navigationWindow)
            {
                TgDesktopUtils.TgItemProxyVm.SetItemProxyVm(proxyVm);
                navigationWindow.ShowWindow();
                navigationWindow.Navigate(typeof(TgItemProxyPage));
            }
        }, false).ConfigureAwait(false);
    }

    // AddProxyCommand
    [RelayCommand]
    public async Task OnAddProxyAsync()
    {
        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            if (Application.Current.MainWindow is MainWindow navigationWindow)
            {
                TgDesktopUtils.TgItemProxyVm.SetItemProxyVm(new TgSqlTableProxyViewModel(new TgSqlTableProxyModel()));
                navigationWindow.ShowWindow();
                navigationWindow.Navigate(typeof(TgItemProxyPage));
            }
        }, false).ConfigureAwait(false);
    }

    #endregion
}