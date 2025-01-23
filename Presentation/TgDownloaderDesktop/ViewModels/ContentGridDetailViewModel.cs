// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

public partial class ContentGridDetailViewModel : ObservableRecipient, INavigationAware
{
	private readonly ISampleDataService _sampleDataService;

	[ObservableProperty]
	public partial SampleOrder? Item { get; set; }

	public ContentGridDetailViewModel(ISampleDataService sampleDataService)
	{
		_sampleDataService = sampleDataService;
	}

	public async void OnNavigatedTo(object parameter)
	{
		if (parameter is long orderID)
		{
			var data = await _sampleDataService.GetContentGridDataAsync();
			Item = data.First(i => i.OrderID == orderID);
		}
	}

	public void OnNavigatedFrom()
	{
	}
}
