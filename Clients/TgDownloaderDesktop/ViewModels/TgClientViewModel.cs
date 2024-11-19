// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgClientViewModel : TgPageViewModelBase
{
    #region Public and private fields, properties, constructor

    public TgClientHelper TgClient { get; private set; } = TgClientHelper.Instance;
    private TgEfAppRepository AppRepository { get; } = new(TgEfUtils.EfContext);
    private TgEfProxyRepository ProxyRepository { get; } = new(TgEfUtils.EfContext);
    public TgEfAppViewModel AppVm { get; }
    public TgEfProxyViewModel ProxyVm { get; set; }
    public ObservableCollection<TgEfProxyViewModel> ProxiesVms { get; private set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Notifications { get; set; }
    public string Password { get; set; }
    public string VerificationCode { get; set; }
    public string ServerMessage { get; set; }
    [ObservableProperty]
    private bool _isNeedVerificationCode;
    [ObservableProperty]
    private bool _isNeedPassword;
    [ObservableProperty]
    private bool _isNeedFirstName;
	[ObservableProperty]
    private bool _isNeedLastName;
	[ObservableProperty]
    private bool _isReady;
	[ObservableProperty]
    private bool _isNotReady;
    
    public ICommand ClientConnectCommand { get; }
    public ICommand ClientDisconnectCommand { get; }
    public ICommand AppLoadCommand { get; }
    public ICommand AppSaveCommand { get; }
    public ICommand AppClearCommand { get; }
    public ICommand AppDeleteCommand { get; }


    public TgClientViewModel(ITgSettingsService settingsService) : base(settingsService)
	{
        AppVm = new(AppRepository.GetFirstAsync(isNoTracking: false).GetAwaiter().GetResult().Item);
        ProxyVm = new(new());
        ProxiesVms = new();

        FirstName = string.Empty;
        LastName = string.Empty;
        Notifications = string.Empty;
        Password = string.Empty;
        VerificationCode = string.Empty;
        StateConnectMsg = string.Empty;
        ServerMessage = string.Empty;
        AppEfStorage = SettingsService.EfStorage;
        AppTgSession = SettingsService.TgSession;

		ClientConnectCommand = new RelayCommand<TgEfProxyViewModel>(async proxyVm => await ClientConnectAsync(proxyVm));
        ClientDisconnectCommand = new RelayCommand(async () => await ClientDisconnectAsync());
        AppLoadCommand = new RelayCommand(async () => await AppLoadAsync());
        AppSaveCommand = new RelayCommand(async () => await AppSaveAsync());
        AppClearCommand = new RelayCommand(async () => await AppClearAsync());
        AppDeleteCommand = new RelayCommand(async () => await AppDeleteAsync());
    }

    #endregion

    #region Public and private methods

    public async Task OnNavigatedToAsync(object parameter)
    {
        OnLoaded(parameter);
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
	        await Task.Delay(1);
	        TgEfProxyEntity proxyNew = (await ProxyRepository.GetAsync(
		        new() { Uid = AppVm.App.ProxyUid ?? Guid.Empty }, isNoTracking: false)).Item;
	        ProxiesVms = new();
	        foreach (TgEfProxyEntity proxy in (await ProxyRepository.GetListAsync(TgEnumTableTopRecords.All, 0, isNoTracking: false)).Items)
	        {
		        ProxiesVms.Add(new(proxy));
	        }
	        if (!ProxiesVms.Select(p => p.Item.UserName).Contains(proxyNew.UserName))
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
            await Task.Delay(1);
            //UpdateStateConnect(TgDesktopUtils.TgClient.IsReady
            //    ? TgDesktopUtils.TgLocale.MenuClientIsConnected : TgDesktopUtils.TgLocale.MenuClientIsDisconnected);
            if (TgDesktopUtils.TgClient.IsReady)
                ViewModelClearConfig();
            //IsLoad = false;
        }, false);
    }

    public string? ConfigClientDesktop(string what)
    {
        TgDesktopUtils.TgClient.UpdateStateSourceAsync(0, 0, $"{TgResourceExtensions.GetMenuClientIsQuery()}: {what}").GetAwaiter();

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
                string sessionPath = Path.Combine(Directory.GetCurrentDirectory(), TgAppSettings.AppXml.XmlFileSession);
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

    public async Task ClientConnectAsync(TgEfProxyViewModel? proxyVm = null)
    {
        await AppSaveAsync();
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(1);
            if (!TgEfUtils.GetEfValid<TgEfAppEntity>(AppVm.App).IsValid)
                return;
            await TgDesktopUtils.TgClient.ConnectSessionAsync(proxyVm?.Item ?? ProxyVm.Item);
        }, true);
        ServerMessage = Exception.IsExist ? Exception.Message : string.Empty;
    }

    public async Task ClientDisconnectAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            TgDesktopUtils.TgClient.Disconnect();
            await Task.CompletedTask;
        }, false).ConfigureAwait(false);
    }

    public async Task AppLoadAsync()
    {
	    await TgDesktopUtils.RunFuncAsync(this, async () =>
	    {
		    AppVm.App = (await AppRepository.GetFirstAsync(isNoTracking: false)).Item;
		    await Task.CompletedTask;
		}, false).ConfigureAwait(false);
    }

    public async Task AppSaveAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await AppRepository.DeleteAllAsync();
            await AppRepository.SaveAsync(AppVm.App);
            await Task.CompletedTask;
		}, false).ConfigureAwait(false);
    }

    public async Task AppClearAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            AppVm.App = (await AppRepository.GetNewAsync(isNoTracking: false)).Item;
            await Task.CompletedTask;
		}, false).ConfigureAwait(true);
    }

    public async Task AppDeleteAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await AppRepository.DeleteAllAsync();
            await Task.CompletedTask;
		}, false).ConfigureAwait(true);

        await AppLoadAsync();
    }
    
    #endregion
}