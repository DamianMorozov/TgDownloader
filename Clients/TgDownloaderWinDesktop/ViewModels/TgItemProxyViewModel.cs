// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgItemProxyViewModel : TgPageViewModelBase, INavigationAware
{
    #region Public and private fields, properties, constructor

    public TgSqlTableProxyViewModel ItemProxyVm { get; private set; } = new(TgSqlUtils.CreateNewProxy());
    public IReadOnlyList<TgEnumProxyType> ProxyTypes { get; }
    public TgPageViewModelBase? ViewModel { get; set; }
    private Guid ProxyUid { get; set; }

    public TgItemProxyViewModel()
    {
        ProxyTypes = ContextManager.ProxyRepository.GetProxyTypes();
    }

    #endregion

    #region Public and private methods

    public void OnNavigatedTo()
    {
        _ = Task.Run(InitializeViewModelAsync).ConfigureAwait(true);
    }

    public void OnNavigatedFrom() { }

    protected override async Task InitializeViewModelAsync()
    {
        await base.InitializeViewModelAsync();

        await OnGetProxyFromStorageAsync();
    }

    public void SetItemProxyVm(TgSqlTableProxyViewModel itemProxyVm) =>
        SetItemProxyVm(itemProxyVm.Proxy, itemProxyVm.Proxy.Uid);

    public void SetItemProxyVm(TgSqlTableProxyModel proxy, Guid? uid = null)
    {
        ItemProxyVm.Proxy.Fill(proxy, uid);
        TgSqlTableProxyViewModel itemBackup = ItemProxyVm;
        ItemProxyVm = new(itemBackup.Proxy);
    }

    // GetProxyFromStorageCommand
    [RelayCommand]
    public async Task OnGetProxyFromStorageAsync()
    {
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            if (ItemProxyVm.ProxyUid != Guid.Empty)
                ProxyUid = ItemProxyVm.ProxyUid;
            TgSqlTableProxyModel proxy = await ContextManager.ProxyRepository.GetAsync(ProxyUid);
            SetItemProxyVm(proxy, proxy.Uid);
        }, true).ConfigureAwait(true);
    }

    // ClearViewCommand
    [RelayCommand]
    public async Task OnClearViewAsync()
    {
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            if (ItemProxyVm.ProxyUid != Guid.Empty)
                ProxyUid = ItemProxyVm.ProxyUid;
            ItemProxyVm.Proxy = await ContextManager.ProxyRepository.GetNewAsync();
        }, false).ConfigureAwait(true);
    }

    // SaveProxyCommand
    [RelayCommand]
    public async Task OnSaveProxyAsync()
    {
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await ContextManager.ProxyRepository.SaveAsync(ItemProxyVm.Proxy, true);
        }, false).ConfigureAwait(false);
    }

    // ReturnToSectionProxiesCommand
    [RelayCommand]
    public async Task OnReturnToSectionProxiesAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            if (Application.Current.MainWindow is MainWindow navigationWindow)
            {
                navigationWindow.ShowWindow();
                navigationWindow.Navigate(typeof(TgProxiesPage));
            }
        }, false).ConfigureAwait(false);
    }

    #endregion
}