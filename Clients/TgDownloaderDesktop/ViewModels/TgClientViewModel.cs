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
    private TgEfProxyViewModel? _proxyVm = new();
	[ObservableProperty]
    private ObservableCollection<TgEfProxyViewModel> _proxiesVms = [];
	[ObservableProperty]
	private Guid _apiHash;
	[ObservableProperty]
	private int _apiId;
	[ObservableProperty]
	private string _phoneNumber = default!;
	[ObservableProperty]
	private string _firstName = default!;
	[ObservableProperty]
	private string _lastName = default!;
	[ObservableProperty]
	private string _password = default!;
    [ObservableProperty]
    private string _verificationCode = default!;
	[ObservableProperty]
    private bool _isReady;
	[ObservableProperty]
    private bool _isNotReady;
	[ObservableProperty]
    private string _mtProxyUrl = default!;
	[ObservableProperty]
    private string _userName = default!;
	[ObservableProperty]
    private string _maxAutoReconnects = default!;
	[ObservableProperty]
    private string _floodRetryThreshold = default!;
	[ObservableProperty]
    private string _pingInterval = default!;
	[ObservableProperty]
    private string _maxCodePwdAttempts = default!;

	private Guid _newApiHash = Guid.Empty;
	private int _newApiId = 0;
	private string _newFirstName = "";
	private string _newLastName = "";
	private string _newPassword = "";
	private string _newPhoneNumber = "";
	private string _newVerificationCode = "";

	public ICommand ClientConnectCommand { get; }
    public ICommand ClientDisconnectCommand { get; }
    public ICommand AppLoadCommand { get; }
    public ICommand AppSaveCommand { get; }
    public ICommand AppClearCommand { get; }
    public ICommand AppDeleteCommand { get; }


    public TgClientViewModel(ITgSettingsService settingsService) : base(settingsService)
    {
	    AppClearCoreAsync().GetAwaiter().GetResult();
		// Commands
		ClientConnectCommand = new AsyncRelayCommand(ClientConnectAsync);
        ClientDisconnectCommand = new AsyncRelayCommand(ClientDisconnectAsync);
        AppLoadCommand = new AsyncRelayCommand(AppLoadAsync);
        AppSaveCommand = new AsyncRelayCommand(AppSaveAsync);
        AppClearCommand = new AsyncRelayCommand(AppClearAsync);
        AppDeleteCommand = new AsyncRelayCommand(AppDeleteAsync);
		// Delegates
		//TgDesktopUtils.TgClient.SetupUpdateStateConnect(UpdateStateConnectAsync);
		//TgDesktopUtils.TgClient.SetupUpdateStateProxy(UpdateStateProxyAsync);
		//TgDesktopUtils.TgClient.SetupUpdateStateSource(UpdateStateSourceAsync);
		//TgDesktopUtils.TgClient.SetupUpdateStateMessage(UpdateStateMessageAsync);
		TgDesktopUtils.TgClient.SetupUpdateException(UpdateExceptionAsync);
		//TgDesktopUtils.TgClient.SetupUpdateStateExceptionShort(UpdateStateExceptionShortAsync);
		TgDesktopUtils.TgClient.SetupAfterClientConnect(AfterClientConnectAsync);
		//TgDesktopUtils.TgClient.SetupGetClientDesktopConfig(ConfigClientDesktop);
	}

	#endregion

	#region Public and private methods

	public async Task OnNavigatedToAsync(object parameter)
    {
        OnLoaded(parameter);
		await AppLoadCoreAsync();
	}

	public async Task AfterClientConnectAsync()
	{
		ConnectionDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
		ConnectionMsg = TgDesktopUtils.TgClient.Client is null || TgDesktopUtils.TgClient.Client.Disconnected
			? TgResourceExtensions.GetClientIsDisconnected() : TgResourceExtensions.GetClientIsConnected();
		if (TgDesktopUtils.TgClient.Client is not null)
		{
			UserName = TgDesktopUtils.TgClient.Client.User?.MainUsername ?? string.Empty;
			MtProxyUrl = TgDesktopUtils.TgClient.Client.MTProxyUrl;
			MaxAutoReconnects = TgDesktopUtils.TgClient.Client.MaxAutoReconnects.ToString();
			FloodRetryThreshold = TgDesktopUtils.TgClient.Client.FloodRetryThreshold.ToString();
			PingInterval = TgDesktopUtils.TgClient.Client.PingInterval.ToString();
			MaxCodePwdAttempts = TgDesktopUtils.TgClient.Client.MaxCodePwdAttempts.ToString();
		}
		else
		{
			await ReloadUiAsync(isClearPassw: false);
		}
	}

	public string? ConfigClientDesktop(string what)
    {
		// await TgDesktopUtils.TgClient.UpdateStateSourceAsync(0, 0, $"{TgResourceExtensions.GetMenuClientIsQuery()}: {what}"));
		switch (what)
		{
			case "api_hash":
				return TgDataFormatUtils.ParseGuidToString(_newApiHash);
			case "api_id":
				return _newApiId.ToString();
			case "phone_number":
				return _newPhoneNumber;
			case "first_name":
				return _newFirstName;
			case "last_name":
				return _newLastName;
			case "password":
				return _newPassword;
			case "verification_code":
				return _newVerificationCode;
			case "session_pathname":
				return SettingsService.AppSession;
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
			//case "init_params":
			default:
				return null;
		}
    }

    public async Task ClientConnectAsync() => await ClientConnectCoreAsync(isRetry: false);

	private async Task ClientConnectCoreAsync(bool isRetry)
	{
        try
        {
	        Exception.Default();
	        _newApiHash = ApiHash;
	        _newApiId = ApiId;
	        _newFirstName = FirstName;
	        _newLastName = LastName;
	        _newPassword = Password;
			_newPhoneNumber = PhoneNumber;
	        _newVerificationCode = VerificationCode;
			await TgDesktopUtils.TgClient.ConnectSessionDesktopAsync(ProxyVm?.Item, ConfigClientDesktop);
        }
        catch (Exception ex)
        {
	        Exception.Set(ex);
	        await TgDesktopUtils.FileLogAsync(ex);
			if (isRetry) return;
	        if (Exception.Message.Contains("or delete the file to start a new session"))
	        {
		        await TgDesktopUtils.DeleteFileStorageExistsAsync(SettingsService.AppSession);
				await ClientConnectCoreAsync(isRetry: true);
	        }
		}
	}

    public async Task ClientDisconnectAsync() => await ContentDialogAsync(TgDesktopUtils.TgClient.DisconnectAsync, TgResourceExtensions.AskClientDisconnect());

    public async Task AppLoadAsync() => await ContentDialogAsync(AppLoadCoreAsync, TgResourceExtensions.AskSettingsLoad());

    public async Task AppLoadCoreAsync()
    {
		TgEfUtils.AppStorage = SettingsService.AppStorage;
		TgEfUtils.RecreateEfContext();

		var storageResult = await AppRepository.GetFirstAsync(isNoTracking: false);
		App = storageResult.IsExists ? storageResult.Item : new();

		await ReloadUiAsync(isClearPassw: false);
	}

    private async Task ReloadUiAsync(bool isClearPassw)
    {
	    ApiHash = App.ApiHash;
	    ApiId = App.ApiId;
		PhoneNumber = App.PhoneNumber;
		FirstName = App.FirstName;
	    LastName = App.LastName;

		if (isClearPassw)
		{
			Password = string.Empty;
			VerificationCode = string.Empty;
		}

	    UserName = string.Empty;
	    MtProxyUrl = string.Empty;
	    MaxAutoReconnects = string.Empty;
	    FloodRetryThreshold = string.Empty;
	    PingInterval = string.Empty;
	    MaxCodePwdAttempts = string.Empty;

		ConnectionDt = string.Empty;
		ConnectionMsg = string.Empty;
		Exception.Default();

		await ReloadProxyAsync();
    }

    private async Task ReloadProxyAsync()
    {
	    ProxiesVms.Clear();
	    var storageResult = await ProxyRepository.GetListAsync(TgEnumTableTopRecords.All, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfProxyEntity proxy in storageResult.Items)
			{
				ProxiesVms.Add(new(proxy));
			}
		}
	    // Insert empty proxy if not exists
	    TgEfProxyViewModel? emptyProxyVm = null;
	    var proxiesVmsEmpty = ProxiesVms.Where(x =>
		    x.ProxyType == TgEnumProxyType.None && (x.ProxyUserName == "No user" || x.ProxyPassword == "No password"));
	    if (!proxiesVmsEmpty.Any())
	    {
		    emptyProxyVm = new(new());
		    ProxiesVms.Add(emptyProxyVm);
	    }
	    // Select proxy
	    var proxiesUids = ProxiesVms.Select(x => x.Item.Uid).ToList();
	    if (App.ProxyUid is { } proxyUid && proxiesUids.Contains(proxyUid))
	    {
		    ProxyVm = ProxiesVms.FirstOrDefault(x => x.ProxyUid == proxyUid);
	    }
	    // Select empty proxy
	    else
	    {
		    ProxyVm = emptyProxyVm;
	    }
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
		App.ProxyUid = ProxyVm?.ProxyUid;
		if (App.ProxyUid is null || App.ProxyUid == Guid.Empty)
			App.Proxy = null;
		
		await AppRepository.SaveAsync(App);
	}

    public async Task AppClearAsync() => await ContentDialogAsync(AppClearCoreAsync, TgResourceExtensions.AskSettingsClear());

	private async Task AppClearCoreAsync()
	{
		var storageResult = await AppRepository.GetNewAsync(isNoTracking: false);
        App = storageResult.IsExists ? storageResult.Item : new();
		ProxiesVms.Clear();
		ProxyVm?.Default();
        
		await ReloadUiAsync(isClearPassw: true);
    }

    public async Task AppDeleteAsync() => await ContentDialogAsync(AppDeleteCoreAsync, TgResourceExtensions.AskSettingsDelete());

    public async Task AppDeleteCoreAsync()
    {
        await AppRepository.DeleteAllAsync();
        await AppLoadCoreAsync();
    }

	#endregion
}