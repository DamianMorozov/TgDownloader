// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSourcesViewModel : TgPageViewModelBase
{
    #region Public and private fields, properties, constructor

    private TgEfSourceRepository SourceRepository { get; } = new(TgEfUtils.EfContext);

	public ICommand ClientConnectCommand { get; }

	public TgSourcesViewModel(ITgSettingsService settingsService) : base(settingsService)
    {
	    //AppClearCoreAsync().GetAwaiter().GetResult();
		// Commands
		//ClientConnectCommand = new AsyncRelayCommand(ClientConnectAsync);
		// Delegates
		//TgDesktopUtils.TgClient.SetupUpdateStateConnect(UpdateStateConnectAsync);
		//TgDesktopUtils.TgClient.SetupUpdateStateProxy(UpdateStateProxyAsync);
		//TgDesktopUtils.TgClient.SetupUpdateStateSource(UpdateStateSourceAsync);
		//TgDesktopUtils.TgClient.SetupUpdateStateMessage(UpdateStateMessageAsync);
		//TgDesktopUtils.TgClient.SetupUpdateException(UpdateExceptionAsync);
		//TgDesktopUtils.TgClient.SetupUpdateStateExceptionShort(UpdateStateExceptionShortAsync);
		//TgDesktopUtils.TgClient.SetupAfterClientConnect(AfterClientConnectAsync);
		//TgDesktopUtils.TgClient.SetupGetClientDesktopConfig(ConfigClientDesktop);
	}

	#endregion

	#region Public and private methods

	public async Task OnNavigatedToAsync(object parameter)
    {
        OnLoaded(parameter);
		await AppLoadCoreAsync();
	}

    public async Task AppLoadCoreAsync()
    {
		TgEfUtils.AppStorage = SettingsService.AppStorage;
		TgEfUtils.RecreateEfContext();

		var storageResult = await SourceRepository.GetListAsync(take: 0, skip: 0, isNoTracking: false);

		await ReloadUiAsync();
	}

    private async Task ReloadUiAsync()
    {

		ConnectionDt = string.Empty;
		ConnectionMsg = string.Empty;
		Exception.Default();
    }

	#endregion
}