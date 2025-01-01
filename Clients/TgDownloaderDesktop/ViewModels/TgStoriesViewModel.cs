// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgStoriesViewModel : TgPageViewModelBase
{
    #region Public and private fields, properties, constructor

    private TgEfStoryRepository Repository { get; } = new(TgEfUtils.EfContext);
	[ObservableProperty]
	private ObservableCollection<TgEfStoryDto> _dtos = [];
	[ObservableProperty]
	private bool _isReady;
	public IRelayCommand LoadDataStorageCommand { get; }
	public IRelayCommand ClearDataStorageCommand { get; }
	public IRelayCommand DefaultSortCommand { get; }

	public TgStoriesViewModel(ITgSettingsService settingsService) : base(settingsService)
    {
		// Commands
		ClearDataStorageCommand = new AsyncRelayCommand(ClearDataStorageAsync);
		DefaultSortCommand = new AsyncRelayCommand(DefaultSortAsync);
		LoadDataStorageCommand = new AsyncRelayCommand(LoadDataStorageAsync);
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

	/// <summary> Sort data </summary>
	private void SetOrderData(IEnumerable<TgEfStoryDto> dtos)
	{
		List<TgEfStoryDto> list = dtos.ToList();
		if (!list.Any())
			return;
		Dtos = [];
		dtos = [.. list.OrderBy(x => x.DtChanged).ThenBy(x => x.FromName)];
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
		if (!SettingsService.IsExistsAppStorage)
			return;
		var dtos = await Repository.GetListDtosAsync(take: 0, skip: 0);
		SetOrderData(dtos);
	}

	private async Task DefaultSortAsync()
	{
		SetOrderData(Dtos);
		await Task.CompletedTask;
	}

	#endregion
}