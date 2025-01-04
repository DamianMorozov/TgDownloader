// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgContactsViewModel : TgPageViewModelBase
{
    #region Public and private fields, properties, constructor

    private TgEfContactRepository Repository { get; } = new(TgEfUtils.EfContext);
	[ObservableProperty]
	public partial ObservableCollection<TgEfContactDto> Dtos { get; set; } = [];
	public IRelayCommand LoadDataStorageCommand { get; }
	public IRelayCommand ClearDataStorageCommand { get; }
	public IRelayCommand DefaultSortCommand { get; }
	public IRelayCommand UpdateOnlineCommand { get; }

	public TgContactsViewModel(ITgSettingsService settingsService) : base(settingsService)
    {
		// Commands
		ClearDataStorageCommand = new AsyncRelayCommand(ClearDataStorageAsync);
		DefaultSortCommand = new AsyncRelayCommand(DefaultSortAsync);
		LoadDataStorageCommand = new AsyncRelayCommand(LoadDataStorageAsync);
		UpdateOnlineCommand = new AsyncRelayCommand(UpdateOnlineAsync);
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
		await TgDesktopUtils.TgClient.CheckClientIsReadyAsync();
		IsOnlineReady = TgDesktopUtils.TgClient.IsReady;
		await Task.CompletedTask;
    }

	/// <summary> Sort data </summary>
	private void SetOrderData(IEnumerable<TgEfContactDto> dtos)
	{
		List<TgEfContactDto> list = dtos.ToList();
		if (!list.Any())
			return;
		Dtos = [];
		dtos = [.. list.OrderBy(x => x.UserName).ThenBy(x => x.FirstName).ThenBy(x => x.LastName)];
		if (dtos.Any())
			foreach (var dto in dtos)
				Dtos.Add(dto);
	}

	private async Task ClearDataStorageAsync() => await ContentDialogAsync(ClearDataStorageCoreAsync, TgResourceExtensions.AskDataClear());

	private async Task ClearDataStorageCoreAsync()
	{
		Dtos.Clear();
		await Task.CompletedTask;
	}

	private async Task LoadDataStorageAsync() => await ContentDialogAsync(LoadDataStorageCoreAsync, TgResourceExtensions.AskDataLoad(), useLoadData: true);

	private async Task LoadDataStorageCoreAsync()
	{
		if (!SettingsService.IsExistsAppStorage) return;
		SetOrderData(await Repository.GetListDtosAsync(take: 0, skip: 0));
	}

	private async Task DefaultSortAsync()
	{
		SetOrderData(Dtos);
		await Task.CompletedTask;
	}

	private async Task UpdateOnlineAsync() => await ContentDialogAsync(UpdateOnlineCoreAsync, TgResourceExtensions.AskUpdateOnline());

	private async Task UpdateOnlineCoreAsync()
	{
		await LoadDataAsync(async () => {
			if (!await TgDesktopUtils.TgClient.CheckClientIsReadyAsync()) return;
			var tgDownloadSettings = new TgDownloadSettingsViewModel();
			await TgDesktopUtils.TgClient.SearchSourcesTgAsync(tgDownloadSettings, TgEnumSourceType.Contact);
			await LoadDataStorageCoreAsync();
		});
	}

	#endregion
}