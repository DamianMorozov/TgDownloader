// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgItemProxyViewModel : TgPageViewModelBase, INavigationAware
{
    #region Public and private fields, properties, constructor

    public TgXpoProxyViewModel ItemProxyVm { get; private set; }
    public IReadOnlyList<TgEnumProxyType> ProxyTypes { get; }
    public TgPageViewModelBase? ViewModel { get; set; }
    private Guid ProxyUid { get; set; }

    public TgItemProxyViewModel(TgXpoProxyRepository proxyRepository)
	{
        ProxyTypes = XpoContext.ProxyRepository.GetProxyTypes();
        ItemProxyVm = new (proxyRepository.CreateNewAsync().GetAwaiter().GetResult().Item);
    }

    #endregion

    #region Public and private methods

    public void OnNavigatedTo()
    {
        InitializeViewModelAsync().GetAwaiter();
    }

    public void OnNavigatedFrom() { }

    protected override async Task InitializeViewModelAsync()
    {
        await base.InitializeViewModelAsync();

        await OnGetProxyFromStorageAsync();
    }

    public void SetItemProxyVm(TgXpoProxyViewModel itemProxyVm) =>
        SetItemProxyVm(itemProxyVm.Proxy);

    public void SetItemProxyVm(TgXpoProxyEntity proxy)
    {
        ItemProxyVm.Proxy.Fill(proxy);
        TgXpoProxyViewModel itemBackup = ItemProxyVm;
        ItemProxyVm = new(itemBackup.Proxy);
    }

    // GetProxyFromStorageCommand
    [RelayCommand]
    public async Task OnGetProxyFromStorageAsync()
    {
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            if (ItemProxyVm.ProxyUid != Guid.Empty)
                ProxyUid = ItemProxyVm.ProxyUid;
            TgXpoProxyEntity proxy = (await XpoContext.ProxyRepository.GetAsync(ProxyUid)).Item;
            SetItemProxyVm(proxy);
        }, true);
    }

    // ClearViewCommand
    [RelayCommand]
    public async Task OnClearViewAsync()
    {
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            if (ItemProxyVm.ProxyUid != Guid.Empty)
                ProxyUid = ItemProxyVm.ProxyUid;
            ItemProxyVm.Proxy = (await XpoContext.ProxyRepository.GetNewAsync()).Item;
        }, false);
    }

    // SaveProxyCommand
    [RelayCommand]
    public async Task OnSaveProxyAsync()
    {
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            await XpoContext.ProxyRepository.SaveAsync(ItemProxyVm.Proxy);
        }, false);
    }

    // ReturnToSectionProxiesCommand
    [RelayCommand]
    public async Task OnReturnToSectionProxiesAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            if (Application.Current.MainWindow is MainWindow navigationWindow)
            {
                navigationWindow.ShowWindow();
                navigationWindow.Navigate(typeof(TgProxiesPage));
            }
        }, false);
    }

    #endregion
}