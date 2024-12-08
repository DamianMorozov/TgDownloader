// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSourcesViewModel : TgPageViewModelBase
{
    #region Public and private fields, properties, constructor

    private TgEfSourceRepository SourceRepository { get; } = new(TgEfUtils.EfContext);
	public ObservableCollection<TgEfSourceViewModel> SourcesVms { get; set; } = [];
	[ObservableProperty]
	private bool _isReady;
	public IRelayCommand UpdateSourcesFromTelegramCommand { get; }
	public IRelayCommand GetSourcesFromTelegramCommand { get; }
	public IRelayCommand MarkAllMessagesAsReadCommand { get; }
	public IRelayCommand ClearViewCommand { get; }
	public IRelayCommand SortViewCommand { get; }
	public IRelayCommand SaveSourcesCommand { get; }
	public IRelayCommand LoadSourcesFromStorageCommand { get; }
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
		ClearViewCommand = new AsyncRelayCommand(ClearViewAsync);
		SortViewCommand = new AsyncRelayCommand(SortViewAsync);
		SaveSourcesCommand = new AsyncRelayCommand(SaveSourcesAsync);
		LoadSourcesFromStorageCommand = new AsyncRelayCommand(LoadSourcesFromStorageAsync);
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

	public async Task OnNavigatedToAsync(object parameter)
    {
        OnLoaded(parameter);
		await AppLoadCoreAsync();
	}

    public async Task AppLoadCoreAsync()
    {
		TgEfUtils.AppStorage = SettingsService.AppStorage;
		TgEfUtils.RecreateEfContext();

		await LoadSourcesFromStorageAsync();
		await ReloadUiAsync();
	}

	private async Task ReloadUiAsync()
    {
		ConnectionDt = string.Empty;
		ConnectionMsg = string.Empty;
		Exception.Default();
		await Task.CompletedTask;
    }

	public async Task LoadSourcesFromStorageAsync()
	{
		var storageResult = await SourceRepository.GetListAsync(take: 0, skip: 0, isNoTracking: false);
		List<TgEfSourceEntity> sources = storageResult.IsExists ? storageResult.Items.ToList() : [];
		SetOrderSources(sources);
	}

	/// <summary> Sort sources </summary>
	private void SetOrderSources(IEnumerable<TgEfSourceEntity> sources)
	{
		List<TgEfSourceEntity> list = sources.ToList();
		if (!list.Any())
			return;
		SourcesVms = [];
		sources = [.. list.OrderBy(x => x.UserName).ThenBy(x => x.Title)];
		if (sources.Any())
			foreach (TgEfSourceEntity source in sources)
				SourcesVms.Add(new(source));
	}

	public async Task UpdateSourcesFromTelegramAsync()
	{
		if (!TgDesktopUtils.TgClient.CheckClientIsReady()) return;
		foreach (TgEfSourceViewModel sourceVm in SourcesVms)
			await UpdateSourceFromTelegramAsync(sourceVm);
	}

	public async Task GetSourcesFromTelegramAsync()
	{
		if (!TgDesktopUtils.TgClient.CheckClientIsReady()) return;
		await TgDesktopUtils.TgClient.ScanSourcesTgDesktopAsync(TgEnumSourceType.Chat, LoadFromTelegramAsync);
		await TgDesktopUtils.TgClient.ScanSourcesTgDesktopAsync(TgEnumSourceType.Dialog, LoadFromTelegramAsync);
	}

	/// <summary> Load sources from Telegram /// </summary>
	public async Task LoadFromTelegramAsync(TgEfSourceViewModel sourceVm)
	{
		var storageResult = await SourceRepository.GetAsync(new TgEfSourceEntity { Id = sourceVm.Item.Id }, isNoTracking: false);
		if (storageResult.IsExists)
			sourceVm = new(storageResult.Item);
		if (!SourcesVms.Select(x => x.SourceId).Contains(sourceVm.SourceId))
			SourcesVms.Add(sourceVm);
		await SaveSourceAsync(sourceVm);
	}

	public async Task MarkAllMessagesAsReadAsync()
	{
		if (!TgDesktopUtils.TgClient.CheckClientIsReady()) return;
		await TgDesktopUtils.TgClient.MarkHistoryReadAsync();
	}

	public async Task ClearViewAsync()
	{
		SourcesVms = [];
		await Task.CompletedTask;
	}

	public async Task SortViewAsync()
	{
		SetOrderSources(SourcesVms.Select(x => x.Item).ToList());
		await Task.CompletedTask;
	}

	/// <summary> Save sources into the Storage </summary>
	public async Task SaveSourcesAsync()
	{
		if (!SourcesVms.Any())
		{
			await TgDesktopUtils.TgClient.UpdateStateSourceAsync(0, 0, "Empty sources list!");
			return;
		}
		foreach (TgEfSourceViewModel sourceVm in SourcesVms)
		{
			await SaveSourceAsync(sourceVm);
		}
	}

	public async Task SaveSourceAsync(TgEfSourceViewModel sourceVm)
	{
		if (sourceVm is null) return;
		var storageResult = await SourceRepository.GetAsync(new TgEfSourceEntity { Id = sourceVm.Item.Id }, isNoTracking: false);
		if (!storageResult.IsExists)
		{
			await SourceRepository.SaveAsync(sourceVm.Item);
			await TgDesktopUtils.TgClient.UpdateStateSourceAsync(sourceVm.Item.Id, 0, $"Saved source | {sourceVm.Item}");
		}
	}

	public async Task GetSourceFromStorageAsync(TgEfSourceViewModel? sourceVm)
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

	public async Task UpdateSourceFromTelegramAsync(TgEfSourceViewModel? sourceVm)
	{
		if (sourceVm is null) return;
		//TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
		//await TgDesktopUtils.TgItemSourceVm.OnUpdateSourceFromTelegramAsync();
		await GetSourceFromStorageAsync(sourceVm);
	}

	public async Task DownloadAsync(TgEfSourceViewModel? sourceVm)
	{
		if (sourceVm is null) return;
		//TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
		//TgDesktopUtils.TgItemSourceVm.ViewModel = this;
		//if (await TgDesktopUtils.TgItemSourceVm.OnDownloadSourceAsync())
		//	await TgDesktopUtils.TgItemSourceVm.OnUpdateSourceFromTelegramAsync();
		await Task.CompletedTask;
	}

	public async Task EditSourceAsync(TgEfSourceViewModel? sourceVm)
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