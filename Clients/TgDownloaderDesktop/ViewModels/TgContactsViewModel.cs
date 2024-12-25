// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgContactsViewModel : TgPageViewModelBase
{
    #region Public and private fields, properties, constructor

    private TgEfContactRepository ContactRepository { get; } = new(TgEfUtils.EfContext);
	[ObservableProperty]
	private ObservableCollection<TgEfContactDto> _contactsVms = [];
	[ObservableProperty]
	private bool _isReady;
	public IRelayCommand LoadDataStorageCommand { get; }
	public IRelayCommand ClearDataStorageCommand { get; }
	public IRelayCommand DefaultSortCommand { get; }

	public TgContactsViewModel(ITgSettingsService settingsService) : base(settingsService)
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
	private void SetOrderData(IEnumerable<TgEfContactDto> dtos)
	{
		List<TgEfContactDto> list = dtos.ToList();
		if (!list.Any())
			return;
		ContactsVms = [];
		dtos = [.. list.OrderBy(x => x.UserName).ThenBy(x => x.FirstName).ThenBy(x => x.LastName)];
		if (dtos.Any())
			foreach (var dto in dtos)
				ContactsVms.Add(dto);
	}

	private async Task ClearDataStorageAsync() => await ContentDialogAsync(ClearDataStorageCoreAsync, TgResourceExtensions.AskDataClear());

	private async Task ClearDataStorageCoreAsync()
	{
		ContactsVms.Clear();
		await Task.CompletedTask;
	}

	private async Task LoadDataStorageAsync() => await ContentDialogAsync(LoadDataStorageCoreAsync, TgResourceExtensions.AskDataLoad(), useLoadData: true);

	private async Task LoadDataStorageCoreAsync()
	{
		if (!SettingsService.IsExistsAppStorage)
			return;
		var storageResult = await ContactRepository.GetListDtoAsync(take: 0, skip: 0, isReadOnly: false);
		List<TgEfContactDto> sourcesDtos = storageResult.IsExists ? storageResult.Items.ToList() : [];
		SetOrderData(sourcesDtos);
	}

	private async Task DefaultSortAsync()
	{
		SetOrderData(ContactsVms);
		await Task.CompletedTask;
	}

	#endregion
}