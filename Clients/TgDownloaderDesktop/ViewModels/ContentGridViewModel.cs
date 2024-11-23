// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

public partial class ContentGridViewModel : ObservableRecipient, INavigationAware
{
	private readonly INavigationService _navigationService;
	private readonly ISampleDataService _sampleDataService;

	public ObservableCollection<SampleOrder> Source { get; } = [];

	public ContentGridViewModel(INavigationService navigationService, ISampleDataService sampleDataService)
	{
		_navigationService = navigationService;
		_sampleDataService = sampleDataService;
	}

	public async void OnNavigatedTo(object parameter)
	{
		Source.Clear();

		// TODO: Replace with real data.
		var data = await _sampleDataService.GetContentGridDataAsync();
		foreach (var item in data)
		{
			Source.Add(item);
		}
	}

	public void OnNavigatedFrom()
	{
	}

	[RelayCommand]
	private void OnItemClick(SampleOrder? clickedItem)
	{
		if (clickedItem != null)
		{
			_navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
			_navigationService.NavigateTo(typeof(ContentGridDetailViewModel).FullName!, clickedItem.OrderID);
		}
	}
}
