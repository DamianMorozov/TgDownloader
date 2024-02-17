// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Domain.Downloads;

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgDownloadsViewModel : TgPageViewModelBase, INavigationAware
{
    #region Public and private fields, properties, constructor

    public ObservableCollection<TgSqlTableDownloadViewModel> DownloadVms { get; set; } = new();

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
    private void SetOrderJobs(IEnumerable<TgSqlTableDownloadViewModel> downloadVms)
    {
	    List<TgSqlTableDownloadViewModel> list = downloadVms.ToList();
	    if (!list.Any()) return;
	    DownloadVms = new();

	    downloadVms = list.OrderBy(x => x.DownloadSetting.SourceVm.SourceUserName)
		    .ThenBy(x => x.DownloadSetting.SourceVm.SourceTitle).ToList();
	    if (downloadVms.Any())
		    foreach (TgSqlTableDownloadViewModel downloadVm in downloadVms)
			    DownloadVms.Add(downloadVm);
    }

	/// <summary>
	/// Create new download settings.
	/// </summary>
	/// <param name="sourceVm"></param>
	/// <returns></returns>
	public TgDownloadSettingsModel CreateDownloadSettings(TgSqlTableSourceViewModel sourceVm)
	{
		TgDownloadSettingsModel downloadSettings = new TgDownloadSettingsModel
		{
			SourceVm = new TgSqlTableSourceViewModel()
			{
				SourceId = sourceVm.Source.Id,
				SourceFirstId = sourceVm.Source.FirstId,
				SourceDirectory = sourceVm.Source.Directory
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
    public async Task OnStartAsync(TgSqlTableDownloadViewModel jobVm)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            TgSqlTableDownloadViewModel? findJobVm = DownloadVms.SingleOrDefault(x =>
	            x.DownloadSetting.SourceVm.SourceId.Equals(jobVm.DownloadSetting.SourceVm.SourceId));
            if (findJobVm is not null)
            {
	            // 
            }
		}, false);
    }

    // StopCommand
    [RelayCommand]
    public async Task OnStopAsync(TgSqlTableDownloadViewModel jobVm)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            TgSqlTableDownloadViewModel? findJobVm = DownloadVms.SingleOrDefault(x =>
	            x.DownloadSetting.SourceVm.SourceId.Equals(jobVm.DownloadSetting.SourceVm.SourceId));
            if (findJobVm is not null)
            {
	            // 
            }
        }, false);
    }

    #endregion
}