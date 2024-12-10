// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

public partial class DataGridViewModel : ObservableRecipient, INavigationAware
{
	private readonly ISampleDataService _sampleDataService;

	[ObservableProperty]
	private ObservableCollection<SampleOrder> _source = [];

	public DataGridViewModel(ISampleDataService sampleDataService)
	{
		_sampleDataService = sampleDataService;
	}

	public async void OnNavigatedTo(object parameter)
	{
		Source.Clear();

		// TODO: Replace with real data.
		var data = await _sampleDataService.GetGridDataAsync();

		foreach (var item in data)
		{
			Source.Add(item);
		}
	}

	public void OnNavigatedFrom()
	{
	}
}
