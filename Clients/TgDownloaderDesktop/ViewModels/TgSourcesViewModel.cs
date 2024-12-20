// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSourcesViewModel : TgPageViewModelBase
{
    #region Public and private fields, properties, constructor

    private TgEfSourceRepository SourceRepository { get; } = new(TgEfUtils.EfContext);
	[ObservableProperty]
	private ObservableCollection<TgEfSourceViewModel> _sourcesVms = [];
	[ObservableProperty]
	private bool _isReady;
	public IRelayCommand UpdateSourcesFromTelegramCommand { get; }
	public IRelayCommand GetSourcesFromTelegramCommand { get; }
	public IRelayCommand MarkAllMessagesAsReadCommand { get; }
	public IRelayCommand ClearDataStorageCommand { get; }
	public IRelayCommand DefaultSortCommand { get; }
	public IRelayCommand LoadDataStorageCommand { get; }
	public IRelayCommand<TgEfSourceViewModel> GetSourceFromStorageCommand { get; }
	public IRelayCommand<TgEfSourceViewModel> UpdateSourceFromTelegramCommand { get; }
	public IRelayCommand<TgEfSourceViewModel> DownloadCommand { get; }
	public IRelayCommand<TgEfSourceViewModel> EditSourceCommand { get; }

	public TgSourcesViewModel(ITgSettingsService settingsService) : base(settingsService)
    {
		//AppClearCoreAsync().GetAwaiter().GetResult();
		// Commands
		UpdateSourcesFromTelegramCommand = new AsyncRelayCommand(UpdateSourcesFromTelegramAsync);
		GetSourcesFromTelegramCommand = new AsyncRelayCommand(GetSourcesFromTelegramAsync);
		MarkAllMessagesAsReadCommand = new AsyncRelayCommand(MarkAllMessagesAsReadAsync);
		ClearDataStorageCommand = new AsyncRelayCommand(ClearDataStorageAsync);
		DefaultSortCommand = new AsyncRelayCommand(DefaultSortAsync);
		LoadDataStorageCommand = new AsyncRelayCommand(LoadDataStorageAsync);
		GetSourceFromStorageCommand = new AsyncRelayCommand<TgEfSourceViewModel>(GetSourceFromStorageAsync);
		UpdateSourceFromTelegramCommand = new AsyncRelayCommand<TgEfSourceViewModel>(UpdateSourceFromTelegramAsync);
		DownloadCommand = new AsyncRelayCommand<TgEfSourceViewModel>(DownloadAsync);
		EditSourceCommand = new AsyncRelayCommand<TgEfSourceViewModel>(EditSourceAsync);
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

	public override async Task OnNavigatedToAsync(NavigationEventArgs e) => await LoadDataAsync(async () =>
		{
			TgEfUtils.AppStorage = SettingsService.AppStorage;
			TgEfUtils.RecreateEfContext();
			await LoadDataStorageCoreAsync();
			await ReloadUiAsync();
		});

	private async Task ReloadUiAsync()
    {
		ConnectionDt = string.Empty;
		ConnectionMsg = string.Empty;
		Exception.Default();
		await Task.CompletedTask;
    }

	/// <summary> Sort sources </summary>
	private void SetOrderSources(IEnumerable<TgEfSourceDto> sourcesDtos)
	{
		List<TgEfSourceDto> list = sourcesDtos.ToList();
		if (!list.Any())
			return;
		SourcesVms = [];
		sourcesDtos = [.. list.OrderBy(x => x.UserName).ThenBy(x => x.Title)];
		if (sourcesDtos.Any())
			foreach (var sourceDto in sourcesDtos)
				SourcesVms.Add(new(sourceDto.ConvertToEntity()));
	}

	private async Task UpdateSourcesFromTelegramAsync()
	{
		if (!TgDesktopUtils.TgClient.CheckClientIsReady()) return;
		foreach (TgEfSourceViewModel sourceVm in SourcesVms)
			await UpdateSourceFromTelegramAsync(sourceVm);
	}

	private async Task GetSourcesFromTelegramAsync()
	{
		if (!TgDesktopUtils.TgClient.CheckClientIsReady()) return;
		await TgDesktopUtils.TgClient.ScanSourcesTgDesktopAsync(TgEnumSourceType.Chat, LoadFromTelegramAsync);
		await TgDesktopUtils.TgClient.ScanSourcesTgDesktopAsync(TgEnumSourceType.Dialog, LoadFromTelegramAsync);
	}

	/// <summary> Load sources from Telegram </summary>
	private async Task LoadFromTelegramAsync(TgEfSourceViewModel sourceVm)
	{
		var storageResult = await SourceRepository.GetAsync(new TgEfSourceEntity { Id = sourceVm.Item.Id }, isNoTracking: false);
		if (storageResult.IsExists)
			sourceVm = new(storageResult.Item);
		if (!SourcesVms.Select(x => x.SourceId).Contains(sourceVm.SourceId))
			SourcesVms.Add(sourceVm);
		await SaveSourceAsync(sourceVm);
	}

	private async Task MarkAllMessagesAsReadAsync()
	{
		if (!TgDesktopUtils.TgClient.CheckClientIsReady()) return;
		await TgDesktopUtils.TgClient.MarkHistoryReadAsync();
	}

	private async Task ClearDataStorageAsync() => await ContentDialogAsync(ClearDataStorageCoreAsync, TgResourceExtensions.AskDataClear());

	private async Task ClearDataStorageCoreAsync()
	{
		SourcesVms.Clear();
		await Task.CompletedTask;
	}

	private async Task LoadDataStorageAsync() => await ContentDialogAsync(LoadDataStorageCoreAsync, TgResourceExtensions.AskDataLoad(), useLoadData: true);

	private async Task LoadDataStorageCoreAsync()
	{
		if (!SettingsService.IsExistsAppStorage)
			return;
		var storageResult = await SourceRepository.GetListDtoAsync(take: 0, skip: 0, isNoTracking: false);
		List<TgEfSourceDto> sourcesDtos = storageResult.IsExists ? storageResult.Items.ToList() : [];
		SetOrderSources(sourcesDtos);
	}

	private async Task DefaultSortAsync()
	{
		SetOrderSources(SourcesVms.Select(x => x.Item.ConvertToDto()).ToList());
		await Task.CompletedTask;
	}

	private async Task SaveSourceAsync(TgEfSourceViewModel sourceVm)
	{
		if (sourceVm is null) return;
		var storageResult = await SourceRepository.GetAsync(new TgEfSourceEntity { Id = sourceVm.Item.Id }, isNoTracking: false);
		if (!storageResult.IsExists)
		{
			await SourceRepository.SaveAsync(sourceVm.Item);
			await TgDesktopUtils.TgClient.UpdateStateSourceAsync(sourceVm.Item.Id, 0, $"Saved source | {sourceVm.Item}");
		}
	}

	private async Task GetSourceFromStorageAsync(TgEfSourceViewModel? sourceVm)
	{
		if (sourceVm is null) return;
		//TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
		//await TgDesktopUtils.TgItemSourceVm.OnGetSourceFromStorageAsync();

		//for (int i = 0; i < SourcesVms.Count; i++)
		//{
		//	if (SourcesVms[i].SourceId.Equals(sourceVm.SourceId))
		//	{
		//		SourcesVms[i].Item.Fill(TgDesktopUtils.TgItemSourceVm.ItemSourceVm.Item, isUidCopy: false);
		//		break;
		//	}
		//}
		await Task.CompletedTask;
	}

	private async Task UpdateSourceFromTelegramAsync(TgEfSourceViewModel? sourceVm)
	{
		if (sourceVm is null) return;
		//TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
		//await TgDesktopUtils.TgItemSourceVm.OnUpdateSourceFromTelegramAsync();
		await GetSourceFromStorageAsync(sourceVm);
	}

	private async Task DownloadAsync(TgEfSourceViewModel? sourceVm)
	{
		if (sourceVm is null) return;
		//TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
		//TgDesktopUtils.TgItemSourceVm.ViewModel = this;
		//if (await TgDesktopUtils.TgItemSourceVm.OnDownloadSourceAsync())
		//	await TgDesktopUtils.TgItemSourceVm.OnUpdateSourceFromTelegramAsync();
		await Task.CompletedTask;
	}

	private async Task EditSourceAsync(TgEfSourceViewModel? sourceVm)
	{
		if (sourceVm is null) return;
		//if (Application.Current.MainWindow is MainWindow navigationWindow)
		//{
		//	TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
		//	navigationWindow.ShowWindow();
		//	navigationWindow.Navigate(typeof(TgItemSourcePage));
		//}
		await Task.CompletedTask;
	}

	#endregion
}