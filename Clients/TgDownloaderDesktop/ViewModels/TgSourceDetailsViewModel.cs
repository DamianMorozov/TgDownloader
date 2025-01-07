// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSourceDetailsViewModel : TgPageViewModelBase
{
    #region Public and private fields, properties, constructor

    private TgEfSourceRepository Repository { get; } = new(TgEfUtils.EfContext);
    private TgEfMessageRepository MessageRepository { get; } = new(TgEfUtils.EfContext);
	[ObservableProperty]
	public partial Guid Uid { get; set; } = default!;
	[ObservableProperty]
	public partial TgEfSourceDto Dto { get; set; } = default!;
	[ObservableProperty]
	public partial ObservableCollection<TgEfMessageDto> Messages { get; set; } = default!;
	[ObservableProperty]
	public partial bool EmptyData { get; set; } = true;
	[ObservableProperty]
	public partial Action ScrollRequested { get; set; } = () => { };
	public IRelayCommand LoadDataStorageCommand { get; }
	public IRelayCommand ClearDataStorageCommand { get; }
	public IRelayCommand UpdateOnlineCommand { get; }

	public TgSourceDetailsViewModel(ITgSettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
	{
		// Commands
		ClearDataStorageCommand = new AsyncRelayCommand(ClearDataStorageAsync);
		LoadDataStorageCommand = new AsyncRelayCommand(LoadDataStorageAsync);
		UpdateOnlineCommand = new AsyncRelayCommand(UpdateOnlineAsync);
	}

	#endregion

	#region Public and private methods

	public override async Task OnNavigatedToAsync(NavigationEventArgs e) => await LoadDataAsync(async () =>
		{
			TgEfUtils.AppStorage = SettingsService.AppStorage;
			TgEfUtils.RecreateEfContext();
			Uid = e.Parameter is Guid uid ? uid : Guid.Empty;
			await LoadDataStorageCoreAsync();
			await ReloadUiAsync();
		});

	private async Task ReloadUiAsync()
    {
		ConnectionDt = string.Empty;
		ConnectionMsg = string.Empty;
		Exception.Default();
		await TgDesktopUtils.TgClient.CheckClientIsReadyAsync();
		IsOnlineReady = TgDesktopUtils.TgClient.IsReady;
		await Task.CompletedTask;
    }

	private async Task ClearDataStorageAsync() => await ContentDialogAsync(ClearDataStorageCoreAsync, TgResourceExtensions.AskDataClear());

	private async Task ClearDataStorageCoreAsync()
	{
		Dto = new();
		Messages = [];
		EmptyData = true;
		await Task.CompletedTask;
	}

	private async Task LoadDataStorageAsync() => await ContentDialogAsync(LoadDataStorageCoreAsync, TgResourceExtensions.AskDataLoad(), useLoadData: true);

	private async Task LoadDataStorageCoreAsync()
	{
		if (!SettingsService.IsExistsAppStorage) return;
		Dto = await Repository.GetDtoAsync(x => x.Uid == Uid);
		Messages = [.. await MessageRepository.GetListDtosAsync(take: 0, skip: 0, x => x.SourceId == Dto.Id, isReadOnly: true)];
		EmptyData = !Messages.Any();
		ScrollRequested?.Invoke();
	}

	private async Task UpdateOnlineAsync() => await ContentDialogAsync(UpdateOnlineCoreAsync, TgResourceExtensions.AskUpdateOnline());

	private async Task UpdateOnlineCoreAsync()
	{
		await LoadDataAsync(async () => {
			if (!await TgDesktopUtils.TgClient.CheckClientIsReadyAsync()) return;
			var tgDownloadSettings = new TgDownloadSettingsViewModel();
			var entity = Dto.GetEntity();
			tgDownloadSettings.SourceVm.Fill(entity);
			//await TgDesktopUtils.TgClient.SearchSourcesTgAsync(tgDownloadSettings, TgEnumSourceType.Channel);
			await TgDesktopUtils.TgClient.DownloadAllDataAsync(tgDownloadSettings);
			await LoadDataStorageCoreAsync();
		});
	}

	#endregion
}