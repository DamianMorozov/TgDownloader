// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgProxiesViewModel : TgPageViewModelBase, INavigationAware
{
	#region Public and private fields, properties, constructor

	public ObservableCollection<TgEfProxyViewModel> ProxiesVms { get; set; } = new();
	private TgEfProxyRepository ProxyRepository { get; } = new(TgEfUtils.EfContext);

	#endregion

	#region Public and private methods

	public void OnNavigatedTo()
	{
        InitializeViewModelAsync().ConfigureAwait(true);
    }

	public void OnNavigatedFrom() { }

    protected override async Task InitializeViewModelAsync()
    {
        await base.InitializeViewModelAsync();

        await OnLoadProxiesFromStorageAsync();
    }
	
    /// <summary>
    /// Sort proxies.
    /// </summary>
    private void SetOrderProxies(IEnumerable<TgEfProxyEntity> proxies)
	{
        List<TgEfProxyEntity> list = proxies.ToList();
        if (!list.Any()) return;
        ProxiesVms = new();

        proxies = list.OrderBy(x => x.Port).ThenBy(x => x.HostName).ToList();
        if (proxies.Any())
            foreach (TgEfProxyEntity proxy in proxies)
                ProxiesVms.Add(new(proxy));
    }

    #endregion

    #region Public and private methods - RelayCommand

    // LoadProxiesFromStorageCommand
    [RelayCommand]
	public async Task OnLoadProxiesFromStorageAsync()
	{
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            TgEfOperResult<TgEfProxyEntity> operResult = await ProxyRepository.GetEnumerableAsync(TgEnumTableTopRecords.All, isNoTracking: false);
			SetOrderProxies(operResult.Items);
        }, false).ConfigureAwait(false);
	}

    // ClearViewCommand
    [RelayCommand]
	public async Task OnClearViewAsync()
	{
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            ProxiesVms = new();
        }, false).ConfigureAwait(true);
	}

    // DeleteProxyCommand
    [RelayCommand]
	public async Task OnDeleteProxyAsync(TgEfProxyViewModel proxyVm)
	{
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            await ProxyRepository.DeleteAsync(proxyVm.Item, isSkipFind: false);
            LoadProxiesFromStorageCommand.Execute(null);
            await TgDesktopUtils.TgClientVm.LoadProxiesForClientAsync();
        }, false).ConfigureAwait(false);
	}

    // EditProxyCommand
    [RelayCommand]
    public async Task OnEditProxyAsync(TgEfProxyViewModel proxyVm)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
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
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            if (Application.Current.MainWindow is MainWindow navigationWindow)
            {
                TgDesktopUtils.TgItemProxyVm.SetItemProxyVm(new TgEfProxyViewModel(new TgEfProxyEntity()));
                navigationWindow.ShowWindow();
                navigationWindow.Navigate(typeof(TgItemProxyPage));
            }
        }, false).ConfigureAwait(false);
    }

    #endregion
}