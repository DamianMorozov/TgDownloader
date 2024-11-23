// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

public partial class ListDetailsViewModel : ObservableRecipient, INavigationAware
{
	private readonly ISampleDataService _sampleDataService;

	[ObservableProperty]
	private SampleOrder? selected;

	public ObservableCollection<SampleOrder> SampleItems { get; private set; } = [];

	public ListDetailsViewModel(ISampleDataService sampleDataService)
	{
		_sampleDataService = sampleDataService;
	}

	public async void OnNavigatedTo(object parameter)
	{
		SampleItems.Clear();

		// TODO: Replace with real data.
		var data = await _sampleDataService.GetListDetailsDataAsync();

		foreach (var item in data)
		{
			SampleItems.Add(item);
		}
	}

	public void OnNavigatedFrom()
	{
	}

	public void EnsureItemSelected()
	{
		Selected ??= SampleItems.First();
	}
}
