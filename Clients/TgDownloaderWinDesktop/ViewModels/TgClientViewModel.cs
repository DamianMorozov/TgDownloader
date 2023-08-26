// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Utils;

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgClientViewModel : TgPageViewModelBase, INavigationAware
{
    #region Public and private fields, properties, constructor

    public TgMvvmAppModel AppVm { get; }
    public TgSqlTableProxyViewModel ProxyVm { get; set; }
    public ObservableCollection<TgSqlTableProxyViewModel> ProxiesVms { get; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Notifications { get; set; }
    public string Password { get; set; }
    public string VerificationCode { get; set; }
    public Brush BackgroundVerificationCode { get; set; }
    public Brush BackgroundPassword { get; set; }
    public Brush BackgroundFirstName { get; set; }
    public Brush BackgroundLastName { get; set; }
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
        AppVm = new(ContextManager.AppRepository.GetFirst());
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
        StateClientMsg = string.Empty;
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

    protected override void InitializeViewModel()
    {
        base.InitializeViewModel();
        TgDispatcherUtils.DispatcherUpdateMainWindow(() =>
        {
            IsFileSession = TgAppSettings.AppXml.IsExistsFileSession;
            ProxiesVms.Clear();
            foreach (TgSqlTableProxyModel proxy in ContextManager.ProxyRepository.GetEnumerable())
            {
                ProxiesVms.Add(new(proxy));
            }
            ProxyVm.Proxy = ContextManager.ProxyRepository.Get(AppVm.App.ProxyUid) ?? ContextManager.ProxyRepository.GetNew();
        });
    }

    public void AfterClientConnect()
    {
        try
        {
            TgDispatcherUtils.DispatcherUpdateMainWindow(() =>
            {
                TgDesktopUtils.TgClient.UpdateStateClient(TgDesktopUtils.TgClient.IsReady
                    ? TgDesktopUtils.TgLocale.MenuClientIsReady : TgDesktopUtils.TgLocale.MenuClientIsNotReady);
                IsFileSession = TgAppSettings.AppXml.IsExistsFileSession;
                if (TgDesktopUtils.TgClient.IsReady)
                    ViewModelClearConfig();
                IsLoad = false;
            });
        }
        catch (Exception ex)
        {
            Exception.Set(ex);
        }
    }

    public string? GetDesktopConfig(string what)
    {
        TgDesktopUtils.TgClient.UpdateStateClient($"{TgDesktopUtils.TgLocale.MenuClientIsQuery}: {what}");
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

    [RelayCommand]
    public async Task OnClientConnectAsync()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        TgDesktopUtils.RunAction(this, () =>
        {
            if (!TgSqlUtils.GetValidXpLite(AppVm.App).IsValid) return;
            TgDesktopUtils.TgClient.ConnectSessionDesktop(ProxyVm.Proxy);
        });
    }

    [RelayCommand]
    public async Task OnClientDisconnectAsync()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        TgDesktopUtils.RunAction(this, () =>
        {
            if (!TgSqlUtils.GetValidXpLite(AppVm.App).IsValid)
                return;
            TgDesktopUtils.TgClient.Disconnect();
            TgDesktopUtils.TgClient.UpdateStateClient(TgDesktopUtils.TgLocale.MenuClientIsNotReady);
        });
    }

    #endregion
}