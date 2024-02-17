// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgClientViewModel : TgPageViewModelBase, INavigationAware
{
    #region Public and private fields, properties, constructor

    public TgMvvmAppModel AppVm { get; }
    public TgSqlTableProxyViewModel ProxyVm { get; set; }
    public ObservableCollection<TgSqlTableProxyViewModel> ProxiesVms { get; private set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Notifications { get; set; }
    public string Password { get; set; }
    public string VerificationCode { get; set; }
    public string ServerMessage { get; set; }
    public Brush BackgroundVerificationCode { get; set; }
    public Brush BackgroundPassword { get; set; }
    public Brush BackgroundFirstName { get; set; }
    public Brush BackgroundLastName { get; set; }
    public Brush BackgroundServerMessage { get; set; }
    private bool _isNeedVerificationCode;
    public bool IsNeedVerificationCode
    {
        get => _isNeedVerificationCode;
        set
        {
            _isNeedVerificationCode = value;
            BackgroundVerificationCode = value ? new(Color.Yellow) : new SolidBrush(Color.Transparent);
        }
    }
    private bool _isNeedPassword;
    public bool IsNeedPassword
    {
        get => _isNeedPassword;
        set
        {
            _isNeedPassword = value;
            BackgroundPassword = value ? new(Color.Yellow) : new SolidBrush(Color.Transparent);
        }
    }
    private bool _isNeedFirstName;
    public bool IsNeedFirstName
    {
        get => _isNeedFirstName;
        set
        {
            _isNeedFirstName = value;
            BackgroundFirstName = value ? new(Color.Yellow) : new SolidBrush(Color.Transparent);
        }
    }
    private bool _isNeedLastName;
    public bool IsNeedLastName
    {
        get => _isNeedLastName;
        set
        {
            _isNeedLastName = value;
            BackgroundLastName = value ? new(Color.Yellow) : new SolidBrush(Color.Transparent);
        }
    }

    public TgClientViewModel()
    {
        AppVm = new(ContextManager.AppRepository.GetFirstAsync().GetAwaiter().GetResult());
        ProxyVm = new(new());
        ProxiesVms = new();

        FirstName = string.Empty;
        LastName = string.Empty;
        Notifications = string.Empty;
        Password = string.Empty;
        VerificationCode = string.Empty;
        BackgroundVerificationCode = new SolidBrush(Color.Transparent);
        BackgroundPassword = new SolidBrush(Color.Transparent);
        BackgroundFirstName = new SolidBrush(Color.Transparent);
        BackgroundLastName = new SolidBrush(Color.Transparent);
        BackgroundServerMessage = new SolidBrush(Color.Transparent);
        StateConnectMsg = string.Empty;
        ServerMessage = string.Empty;
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

        IsFileSession = TgAppSettings.AppXml.IsExistsFileSession;
        await LoadProxiesForClientAsync();
    }

    public async Task LoadProxiesForClientAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            TgSqlTableProxyModel proxyNew = await ContextManager.ProxyRepository.GetAsync(AppVm.App.ProxyUid);
            ProxiesVms = new();
            foreach (TgSqlTableProxyModel proxy in ContextManager.ProxyRepository.GetEnumerable())
            {
                ProxiesVms.Add(new(proxy));
            }
            if (!ProxiesVms.Select(p => p.Proxy.UserName).Contains(proxyNew.UserName))
            {
                ProxyVm = new(proxyNew);
                ProxiesVms.Add(ProxyVm);
            }
        }, false);
    }

    public async Task AfterClientConnectAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            //TgDesktopUtils.TgClient.UpdateStateConnect(TgDesktopUtils.TgClient.IsReady
            //    ? TgDesktopUtils.TgLocale.MenuClientIsConnected : TgDesktopUtils.TgLocale.MenuClientIsDisconnected);
            IsFileSession = TgAppSettings.AppXml.IsExistsFileSession;
            if (TgDesktopUtils.TgClient.IsReady)
                ViewModelClearConfig();
            IsLoad = false;
        }, false);
    }

    public string? ConfigClientDesktop(string what)
    {
        TgDesktopUtils.TgClient.UpdateStateSourceAsync(0, 0, $"{TgDesktopUtils.TgLocale.MenuClientIsQuery}: {what}").GetAwaiter();

        switch (what)
        {
            case "api_hash":
                string apiHash = TgDataFormatUtils.ParseGuidToString(AppVm.App.ApiHash);
                return apiHash;
            case "api_id":
                return AppVm.App.ApiId.ToString();
            case "phone_number":
                return AppVm.App.PhoneNumber;
            case "notifications":
                return Notifications;
            case "first_name":
                if (string.IsNullOrEmpty(FirstName))
                    IsNeedFirstName = true;
                return FirstName;
            case "last_name":
                if (string.IsNullOrEmpty(LastName))
                    IsNeedLastName = true;
                return LastName;
            case "session_pathname":
                string sessionPath = Path.Combine(Directory.GetCurrentDirectory(), TgAppSettings.AppXml.FileSession);
                return sessionPath;
            case "verification_code":
                if (string.IsNullOrEmpty(VerificationCode))
                    IsNeedVerificationCode = true;
                return VerificationCode;
            case "password":
                if (string.IsNullOrEmpty(Password))
                    IsNeedPassword = true;
                return Password;
            //case "session_key":
            //case "server_address":
            //case "device_model":
            //case "system_version":
            //case "app_version":
            //case "system_lang_code":
            //case "lang_pack":
            //case "lang_code":
            default:
                return null;
        }
    }

    private void ViewModelClearConfig()
    {
        IsNeedVerificationCode = false;
        VerificationCode = string.Empty;
        IsNeedFirstName = false;
        FirstName = string.Empty;
        IsNeedLastName = false;
        LastName = string.Empty;
        IsNeedPassword = false;
        Password = string.Empty;
    }

    // ClientConnectCommand
    [RelayCommand]
    public async Task OnClientConnectAsync(TgSqlTableProxyViewModel? proxyVm = null)
    {
        await OnAppSaveAsync();

        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            if (!TgSqlUtils.GetValidXpLite(AppVm.App).IsValid)
                return;
            await TgDesktopUtils.TgClient.ConnectSessionAsync(proxyVm ?? ProxyVm);
        }, true);

        ServerMessage = TgDesktopUtils.TgClientVm.Exception.IsExists 
            ? TgDesktopUtils.TgClientVm.Exception.Message : string.Empty;
    }

    // ClientDisconnectCommand
    [RelayCommand]
    public async Task OnClientDisconnectAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            TgDesktopUtils.TgClient.Disconnect();
        }, false).ConfigureAwait(false);
    }

    // AppSaveCommand
    [RelayCommand]
    public async Task OnAppSaveAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            await ContextManager.AppRepository.DeleteAllItemsAsync();
            await ContextManager.AppRepository.SaveAsync(AppVm.App);
        }, false).ConfigureAwait(false);
    }

    // AppClearCommand
    [RelayCommand]
    public async Task OnAppClearAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            AppVm.App = await ContextManager.AppRepository.GetNewAsync();
        }, false).ConfigureAwait(true);
    }

    // AppLoadCommand
    [RelayCommand]
    public async Task OnAppLoadAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            AppVm.App = await ContextManager.AppRepository.GetFirstAsync();
        }, false).ConfigureAwait(false);
    }
    
    // AppEmptyCommand
    [RelayCommand]
    public async Task OnAppEmptyAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            await ContextManager.AppRepository.DeleteAllItemsAsync();
        }, false).ConfigureAwait(true);

        await OnAppLoadAsync();
    }
    
    #endregion
}