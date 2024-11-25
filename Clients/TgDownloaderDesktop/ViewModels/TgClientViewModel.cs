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
	private Guid _apiHash = default!;
	[ObservableProperty]
	private int _apiId = default!;
	[ObservableProperty]
	private string _phoneNumber = default!;
	[ObservableProperty]
	private string _firstName = default!;
	[ObservableProperty]
	private string _lastName = default!;
	[ObservableProperty]
	private Guid? _proxyUid = default!;
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
	    AppClearCoreAsync().GetAwaiter().GetResult();
		ClientConnectCommand = new AsyncRelayCommand(ClientConnectAsync);
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
	        await AppLoadCoreAsync();
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
        await TgDesktopUtils.TgClient.UpdateStateSourceAsync(0, 0, $"{TgResourceExtensions.GetMenuClientIsQuery()}: {what}");
        switch (what)
        {
            case "api_hash":
                string apiHash = TgDataFormatUtils.ParseGuidToString(ApiHash);
                return apiHash;
            case "api_id":
                return ApiId.ToString();
            case "phone_number":
                return PhoneNumber;
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
            //case "notifications":
            //    return Notifications;
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

    public async Task ClientConnectAsync() => await ContentDialogAsync(ClientConnectCoreAsync, TgResourceExtensions.AskClientConnect());

    private async Task ClientConnectCoreAsync()
	{
        await AppSaveCoreAsync();
        if (!TgEfUtils.GetEfValid(App).IsValid)
        {
	        await ContentDialogAsync(TgResourceExtensions.ClientSettingsAreNotValid());
	        return;
        }
        await TgDesktopUtils.TgClient.ConnectSessionAsync(ProxyVm.Item);
        ServerMessage = Exception.IsExist ? Exception.Message : string.Empty;
        await Task.CompletedTask;
	}

    public async Task ClientDisconnectAsync() => await ContentDialogAsync(ClientDisconnectCoreAsync, TgResourceExtensions.AskClientDisconnect());

    private async Task ClientDisconnectCoreAsync()
    {
        TgDesktopUtils.TgClient.Disconnect();
        await Task.CompletedTask;
    }

    public async Task AppLoadAsync() => await ContentDialogAsync(AppLoadCoreAsync, TgResourceExtensions.AskSettingsLoad());

    public async Task AppLoadCoreAsync()
    {
	    StateConnectMsg = string.Empty;
	    AppEfStorage = SettingsService.EfStorage;
	    AppTgSession = SettingsService.TgSession;

		var storageResult = await AppRepository.GetFirstAsync(isNoTracking: false);
		App = storageResult.IsExists ? storageResult.Item : new();

		ProxiesVms = [];
		TgEfProxyEntity proxyNew = (await ProxyRepository.GetAsync(new() { Uid = ProxyUid ?? Guid.Empty }, isNoTracking: false)).Item;
		foreach (TgEfProxyEntity proxy in (await ProxyRepository.GetListAsync(TgEnumTableTopRecords.All, 0, isNoTracking: false)).Items)
		{
			ProxiesVms.Add(new(proxy));
		}
		if (!ProxiesVms.Select(p => p.Item.UserName).Contains(proxyNew.UserName))
		{
			ProxyVm = new(proxyNew);
			ProxiesVms.Add(ProxyVm);
		}
		else
		{
			ProxyVm = new();
		}

		await ReloadUiAsync();
		await Task.CompletedTask;
	}

    private async Task ReloadUiAsync()
    {
	    ApiHash = App.ApiHash;
	    ApiId = App.ApiId;
	    FirstName = App.FirstName;
	    LastName = App.LastName;
	    PhoneNumber = App.PhoneNumber;
	    ProxyUid = App.ProxyUid;

	    Password = string.Empty;
		VerificationCode = string.Empty;
		ServerMessage = string.Empty;

		await Task.CompletedTask;
    }

	public async Task AppSaveAsync() => await ContentDialogAsync(AppSaveCoreAsync, TgResourceExtensions.AskSettingsSave());

	private async Task AppSaveCoreAsync()
	{
		await AppRepository.DeleteAllAsync();
		
		App.ApiHash = ApiHash;
		App.ApiId = ApiId;
		App.FirstName = FirstName;
		App.LastName = LastName;
		App.PhoneNumber = PhoneNumber;
		App.ProxyUid = ProxyUid;
		
		await AppRepository.SaveAsync(App);
		await Task.CompletedTask;
	}

    public async Task AppClearAsync() => await ContentDialogAsync(AppClearCoreAsync, TgResourceExtensions.AskSettingsClear());

	private async Task AppClearCoreAsync()
	{
		var storageResult = await AppRepository.GetNewAsync(isNoTracking: false);
        App = storageResult.IsExists ? storageResult.Item : new();
		ProxiesVms = [];
		ProxyVm = new();
        await ReloadUiAsync();
		await Task.CompletedTask;
    }

    public async Task AppDeleteAsync() => await ContentDialogAsync(AppDeleteCoreAsync, TgResourceExtensions.AskSettingsDelete());

    public async Task AppDeleteCoreAsync()
    {
        await AppRepository.DeleteAllAsync();
        await AppLoadCoreAsync();
        await Task.CompletedTask;
    }

	#endregion
}