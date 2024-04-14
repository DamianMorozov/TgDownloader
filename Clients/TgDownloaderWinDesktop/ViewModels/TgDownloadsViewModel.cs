// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Domain.Downloads;

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgDownloadsViewModel : TgPageViewModelBase, INavigationAware
{
    #region Public and private fields, properties, constructor

    public ObservableCollection<TgEfTableDownloadViewModel> DownloadVms { get; set; } = new();

    #endregion

    #region Public and private methods

    public void OnNavigatedTo()
    {
        InitializeViewModelAsync().GetAwaiter();
    }

    public void OnNavigatedFrom() { }

    protected override async Task InitializeViewModelAsync()
    {
        await base.InitializeViewModelAsync();

        //await OnLoadSourcesFromStorageAsync();
    }

    /// <summary>
    /// Sort.
    /// </summary>
    private void SetOrderJobs(IEnumerable<TgEfTableDownloadViewModel> downloadVms)
    {
	    List<TgEfTableDownloadViewModel> list = downloadVms.ToList();
	    if (!list.Any()) return;
	    DownloadVms = new();

	    downloadVms = list.OrderBy(x => x.DownloadSetting.SourceVm.SourceUserName)
		    .ThenBy(x => x.DownloadSetting.SourceVm.SourceTitle).ToList();
	    if (downloadVms.Any())
		    foreach (TgEfTableDownloadViewModel downloadVm in downloadVms)
			    DownloadVms.Add(downloadVm);
    }

	/// <summary>
	/// Create new download settings.
	/// </summary>
	/// <param name="sourceVm"></param>
	/// <returns></returns>
	public TgDownloadSettingsModel CreateDownloadSettings(TgEfSourceViewModel sourceVm)
	{
		TgDownloadSettingsModel downloadSettings = new()
		{
			SourceVm = new TgEfSourceViewModel(EfContext)
			{
				SourceId = sourceVm.Item.Id,
				SourceFirstId = sourceVm.Item.FirstId,
				SourceDirectory = sourceVm.Item.Directory
			}
		};
        if (!DownloadVms.Any())
        {
            //DownloadVms.Add(downloadSettings);
        }
        return downloadSettings;
	}
    
	#endregion

	#region Public and private methods - RelayCommand

	// SortViewCommand
	[RelayCommand]
    public async Task OnSortViewAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            SetOrderJobs(DownloadVms.ToList());
        }, false);
    }

    // StartCommand
    [RelayCommand]
    public async Task OnStartAsync(TgEfTableDownloadViewModel jobVm)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            TgEfTableDownloadViewModel? findJobVm = DownloadVms.SingleOrDefault(x =>
	            x.DownloadSetting.SourceVm.SourceId.Equals(jobVm.DownloadSetting.SourceVm.SourceId));
            if (findJobVm is not null)
            {
	            // 
            }
		}, false);
    }

    // StopCommand
    [RelayCommand]
    public async Task OnStopAsync(TgEfTableDownloadViewModel jobVm)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            TgEfTableDownloadViewModel? findJobVm = DownloadVms.SingleOrDefault(x =>
	            x.DownloadSetting.SourceVm.SourceId.Equals(jobVm.DownloadSetting.SourceVm.SourceId));
            if (findJobVm is not null)
            {
	            // 
            }
        }, false);
    }

    #endregion
}