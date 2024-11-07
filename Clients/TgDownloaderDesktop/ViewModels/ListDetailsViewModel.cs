using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using TgDownloaderDesktop.Contracts.ViewModels;
using TgDownloaderDesktop.Core.Contracts.Services;
using TgDownloaderDesktop.Core.Models;

namespace TgDownloaderDesktop.ViewModels;

public partial class ListDetailsViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    [ObservableProperty]
    private SampleOrder? selected;

    public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

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
