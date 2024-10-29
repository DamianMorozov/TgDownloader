// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgItemProxyViewModel : TgPageViewModelBase, INavigationAware
{
    #region Public and private fields, properties, constructor

    private TgEfProxyRepository ProxyRepository { get; } = new(TgEfUtils.EfContext);
    public TgEfProxyViewModel ItemProxyVm { get; private set; }
    public IReadOnlyList<TgEnumProxyType> ProxyTypes { get; }
    public TgPageViewModelBase? ViewModel { get; set; }
    private Guid ProxyUid { get; set; }

    public TgItemProxyViewModel()
	{
        ProxyTypes = GetProxyTypes();
		ItemProxyVm = new(ProxyRepository.CreateNewAsync().GetAwaiter().GetResult().Item);
	}

	#endregion

	#region Public and private methods

	public IReadOnlyList<TgEnumProxyType> GetProxyTypes() =>
		Enum.GetValues(typeof(TgEnumProxyType)).Cast<TgEnumProxyType>().ToList();

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

    public void SetItemProxyVm(TgEfProxyViewModel itemProxyVm) =>
        SetItemProxyVm(itemProxyVm.Item);

    public void SetItemProxyVm(TgEfProxyEntity proxy)
    {
        ItemProxyVm.Item.Fill(proxy, isUidCopy: false);
        TgEfProxyViewModel itemBackup = ItemProxyVm;
        ItemProxyVm = new(itemBackup.Item);
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
            TgEfProxyEntity proxy = (await ProxyRepository.GetAsync(new TgEfProxyEntity { Uid = ProxyUid }, isNoTracking: false)).Item;
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
            ItemProxyVm.Item = (await ProxyRepository.GetNewAsync(isNoTracking: false)).Item;
        }, false);
    }

    // SaveProxyCommand
    [RelayCommand]
    public async Task OnSaveProxyAsync()
    {
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            await ProxyRepository.SaveAsync(ItemProxyVm.Item);
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