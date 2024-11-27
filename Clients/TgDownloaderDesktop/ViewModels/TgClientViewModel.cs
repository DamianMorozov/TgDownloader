// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using WTelegram;

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
        SetupClient();
    }

    #endregion

    #region Public and private methods

    public async Task OnNavigatedToAsync(object parameter)
    {
        OnLoaded(parameter);
		await AppLoadCoreAsync();
	}

    public void SetupClient()
    {
	    //TgDesktopUtils.TgClient.SetupUpdateStateConnect(UpdateStateConnectAsync);
	    //TgDesktopUtils.TgClient.SetupUpdateStateProxy(UpdateStateProxyAsync);
	    //TgDesktopUtils.TgClient.SetupUpdateStateSource(UpdateStateSourceAsync);
	    //TgDesktopUtils.TgClient.SetupUpdateStateMessage(UpdateStateMessageAsync);
	    //TgDesktopUtils.TgClient.SetupUpdateStateException(UpdateStateExceptionAsync);
	    //TgDesktopUtils.TgClient.SetupUpdateStateExceptionShort(UpdateStateExceptionShortAsync);
	    TgDesktopUtils.TgClient.SetupAfterClientConnect(AfterClientConnectAsync);
		//TgDesktopUtils.TgClient.SetupGetClientDesktopConfig(ConfigClientDesktop);
	}

	public async Task AfterClientConnectAsync()
	{
		UpdateStateConnect(TgDesktopUtils.TgClient.Client is null || TgDesktopUtils.TgClient.Client.Disconnected 
			? TgResourceExtensions.GetClientIsDisconnected() : TgResourceExtensions.GetClientIsConnected());
		if (TgDesktopUtils.TgClient.Client is not null)
		{
			UserName = TgDesktopUtils.TgClient.Client.User.MainUsername;
			MtProxyUrl = TgDesktopUtils.TgClient.Client.MTProxyUrl;
		}
		else
		{
			await ReloadUiAsync();
		}
		await Task.CompletedTask;
	}

	public string? ConfigClientDesktop(string what)
    {
	    //TgDownloaderDesktop.App.MainWindow.DispatcherQueue.TryEnqueue(async () => 
		   // await TgDesktopUtils.TgClient.UpdateStateSourceAsync(0, 0, $"{TgResourceExtensions.GetMenuClientIsQuery()}: {what}"));
		switch (what)
		{
			case "api_hash":
				var apiHash = TgDataFormatUtils.ParseGuidToString(ApiHash);
				return apiHash;
			case "api_id":
				return ApiId.ToString();
			case "phone_number":
				return PhoneNumber;
			case "first_name":
				return FirstName;
			case "last_name":
				return LastName;
			case "password":
				return Password;
			case "verification_code":
				return VerificationCode;
			//case "notifications":
			//    return Notifications;
			case "session_pathname":
				var sessionPath = Path.Combine(Package.Current.InstalledLocation.Path, AppSettings.AppXml.XmlFileSession);
				return sessionPath;
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

    public async Task ClientConnectAsync() => await ClientConnectCoreAsync();

    private async Task ClientConnectCoreAsync() => await ClientConnectCoreAsync(isRetry: false);

	private async Task ClientConnectCoreAsync(bool isRetry)
	{
        try
        {
	        Exception.Default();
			await TgDesktopUtils.TgClient.ConnectSessionDesktopAsync(ProxyVm?.Item, ConfigClientDesktop);
			await Task.CompletedTask;
        }
        catch (Exception ex)
        {
	        Exception.Set(ex);
			if (isRetry) return;
	        if (Exception.Message.Contains("or delete the file to start a new session"))
	        {
		        var sessionPath = Path.Combine(Package.Current.InstalledLocation.Path, AppSettings.AppXml.XmlFileSession);
				if (File.Exists(sessionPath))
					File.Delete(sessionPath);
				await ClientConnectCoreAsync(isRetry: true);
	        }
		}
	}

    public async Task ClientDisconnectAsync() => await ContentDialogAsync(ClientDisconnectCoreAsync, TgResourceExtensions.AskClientDisconnect());

    private async Task ClientDisconnectCoreAsync()
    {
        await TgDesktopUtils.TgClient.DisconnectAsync();
        await Task.CompletedTask;
    }

    public async Task AppLoadAsync() => await ContentDialogAsync(AppLoadCoreAsync, TgResourceExtensions.AskSettingsLoad());

    public async Task AppLoadCoreAsync()
    {
	    AppEfStorage = SettingsService.EfStorage;
	    AppTgSession = SettingsService.TgSession;

		var storageResult = await AppRepository.GetFirstAsync(isNoTracking: false);
		App = storageResult.IsExists ? storageResult.Item : new();

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

	    Password = string.Empty;
	    VerificationCode = string.Empty;
	    UserName = string.Empty;
	    MtProxyUrl = string.Empty;
	    StateConnectMsg = string.Empty;
	    StateConnectDt = string.Empty;
		Exception.Default();

		await ReloadProxyAsync();
		await Task.CompletedTask;
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
		await Task.CompletedTask;
	}

    public async Task AppClearAsync() => await ContentDialogAsync(AppClearCoreAsync, TgResourceExtensions.AskSettingsClear());

	private async Task AppClearCoreAsync()
	{
		var storageResult = await AppRepository.GetNewAsync(isNoTracking: false);
        App = storageResult.IsExists ? storageResult.Item : new();
		ProxiesVms.Clear();
		ProxyVm?.Default();
        
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