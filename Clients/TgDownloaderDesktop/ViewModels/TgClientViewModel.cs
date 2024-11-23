// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgClientViewModel : TgPageViewModelBase
{
    #region Public and private fields, properties, constructor

    private TgEfAppRepository AppRepository { get; } = new(TgEfUtils.EfContext);
    private TgEfProxyRepository ProxyRepository { get; } = new(TgEfUtils.EfContext);
	[ObservableProperty]
	private TgEfAppEntity _app = default!;
	[ObservableProperty]
    private TgEfProxyViewModel _proxyVm = default!;
	[ObservableProperty]
    private ObservableCollection<TgEfProxyViewModel> _proxiesVms = default!;
	[ObservableProperty]
	private string _firstName = default!;
	[ObservableProperty]
    private string _lastName = default!;
    [ObservableProperty]
    private string _notifications = default!;
    [ObservableProperty]
    private string _password = default!;
    [ObservableProperty]
    private string _verificationCode = default!;
    [ObservableProperty]
    private string _serverMessage = default!;
    [ObservableProperty]
    private bool _isNeedVerificationCode = default!;
    [ObservableProperty]
    private bool _isNeedPassword = default!;
    [ObservableProperty]
    private bool _isNeedFirstName = default!;
	[ObservableProperty]
    private bool _isNeedLastName = default!;
	[ObservableProperty]
    private bool _isReady = default!;
	[ObservableProperty]
    private bool _isNotReady = default!;
    
    public ICommand ClientConnectCommand { get; }
    public ICommand ClientDisconnectCommand { get; }
    public ICommand AppLoadCommand { get; }
    public ICommand AppSaveCommand { get; }
    public ICommand AppClearCommand { get; }
    public ICommand AppDeleteCommand { get; }


    public TgClientViewModel(ITgSettingsService settingsService) : base(settingsService)
	{
		AppLoadCore().GetAwaiter().GetResult();

		ClientConnectCommand = new AsyncRelayCommand<TgEfProxyViewModel>(ClientConnectAsync);
        ClientDisconnectCommand = new AsyncRelayCommand(ClientDisconnectAsync);
        AppLoadCommand = new AsyncRelayCommand(AppLoadAsync);
        AppSaveCommand = new AsyncRelayCommand(AppSaveAsync);
        AppClearCommand = new AsyncRelayCommand(AppClearAsync);
        AppDeleteCommand = new AsyncRelayCommand(AppDeleteAsync);
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
		        new() { Uid = App.ProxyUid ?? Guid.Empty }, isNoTracking: false)).Item;
	        ProxiesVms = [];
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
        //UpdateStateConnect(TgDesktopUtils.TgClient.IsReady
        //    ? TgDesktopUtils.TgLocale.MenuClientIsConnected : TgDesktopUtils.TgLocale.MenuClientIsDisconnected);
        if (TgDesktopUtils.TgClient.IsReady)
            ViewModelClearConfig();
		//IsLoad = false;
		await Task.CompletedTask;
    }

    public async Task<string> ConfigClientDesktop(string what)
    {
        //TgDesktopUtils.TgClient.UpdateStateSourceAsync(0, 0, $"{TgResourceExtensions.GetMenuClientIsQuery()}: {what}").GetAwaiter();
        await TgDesktopUtils.TgClient.UpdateStateSourceAsync(0, 0, $"{TgResourceExtensions.GetMenuClientIsQuery()}: {what}");
        switch (what)
        {
            case "api_hash":
                string apiHash = TgDataFormatUtils.ParseGuidToString(App.ApiHash);
                return apiHash;
            case "api_id":
                return App.ApiId.ToString();
            case "phone_number":
                return App.PhoneNumber;
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
                return string.Empty;
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
        if (!TgEfUtils.GetEfValid(App).IsValid) return;
        await TgDesktopUtils.TgClient.ConnectSessionAsync(proxyVm?.Item ?? ProxyVm.Item);
        ServerMessage = Exception.IsExist ? Exception.Message : string.Empty;
    }

    public async Task ClientDisconnectAsync()
    {
        TgDesktopUtils.TgClient.Disconnect();
        await Task.CompletedTask;
    }

    public async Task AppLoadAsync()
    {
	    if (XamlRootVm is null) return;
        ContentDialog dialog = new()
        {
            XamlRoot = XamlRootVm,
            Title = TgResourceExtensions.AskSettingsLoad(),
            PrimaryButtonText = TgResourceExtensions.GetYesButton(),
            CloseButtonText = TgResourceExtensions.GetCancelButton(),
            DefaultButton = ContentDialogButton.Close,
            PrimaryButtonCommand = new AsyncRelayCommand(AppLoadCore)
        };
        _ = await dialog.ShowAsync();
	    await Task.CompletedTask;
    }

    private async Task AppLoadCore()
    {
	    App = (await AppRepository.GetFirstAsync(isNoTracking: false)).Item;
	    ProxyVm = new(new());
	    ProxiesVms = [];
	    FirstName = string.Empty;
	    LastName = string.Empty;
	    Notifications = string.Empty;
	    Password = string.Empty;
	    VerificationCode = string.Empty;
	    StateConnectMsg = string.Empty;
	    ServerMessage = string.Empty;
	    AppEfStorage = SettingsService.EfStorage;
	    AppTgSession = SettingsService.TgSession;
	}

	public async Task AppSaveAsync()
    {
        await AppRepository.DeleteAllAsync();
        await AppRepository.SaveAsync(App);
        await Task.CompletedTask;
    }

    public async Task AppClearAsync()
    {
        App = (await AppRepository.GetNewAsync(isNoTracking: false)).Item;
        await Task.CompletedTask;
    }

    public async Task AppDeleteAsync()
    {
        await AppRepository.DeleteAllAsync();
        await AppLoadCore();
        await Task.CompletedTask;
    }

	#endregion
}