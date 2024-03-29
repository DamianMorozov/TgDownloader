﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgItemSourceViewModel : TgPageViewModelBase, INavigationAware
{
    #region Public and private fields, properties, constructor

    public TgSqlTableSourceViewModel ItemSourceVm { get; } = new();
    public Visibility ChartVisibility { get; private set; } = Visibility.Hidden;
    private LineSeries LineSerie { get; set; } = new()
    {
	    Title = string.Empty,
	    Values = new ChartValues<long> { 1, 2 },
	    StrokeThickness = 0.5,
	    PointGeometry = null,
    };
    private Stopwatch SwDownloadChart { get; set; } = Stopwatch.StartNew();
	public SeriesCollection FileSpeedSeries { get; } = new();
	public List<string> FileSpeedFormatterX { get; set; } = ["0", "1"];
	public Func<long, string> FileSpeedFormatterY { get; set; } = value => value.ToString();
	public TgPageViewModelBase? ViewModel { get; set; }
    private Guid SourceUid { get; set; }
    private long FileSizeMinSizeMonitoring => 52_428_800; // 50 * 1024 * 1024;

    #endregion

	#region Public and private methods

	public void OnNavigatedTo()
	{
        InitializeViewModelAsync().GetAwaiter();
    }

	public void OnNavigatedFrom() { }

    protected override async Task InitializeViewModelAsync()
    {
        TgDesktopUtils.TgClient.SetupUpdateStateItemSource(UpdateStateItemSourceAsync);
        TgDesktopUtils.TgClient.SetupUpdateStateFile(UpdateStateFileAsync);
        await base.InitializeViewModelAsync();
        await OnGetSourceFromStorageAsync();
    }

    private async Task UpdateStateItemSourceAsync(long sourceId)
    {
	    if (ItemSourceVm.SourceId.Equals(sourceId))
	    {
		    await OnGetSourceFromStorageAsync();
	    }
        //else
        //{
        //    await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        //    {
        //        await Task.Delay(TimeSpan.FromMilliseconds(1));
        //        SourceUid = Guid.Empty;
        //        TgSqlTableSourceModel source = await ContextManager.SourceRepository.GetNewAsync();
        //        SetItemSourceVm(source, source.Uid);
        //    }, true);
        //}
    }

    public void SetItemSourceVm(TgSqlTableSourceViewModel itemSourceVm) => 
        SetItemSourceVm(itemSourceVm.Source, itemSourceVm.Source.Uid);

    public void SetItemSourceVm(TgSqlTableSourceModel source, Guid? uid = null)
    {
        ItemSourceVm.Source.Fill(source, uid);
        //TgSqlTableSourceViewModel itemBackup = ItemSourceVm;
  //      ItemSourceVm = new()
  //      {
  //          Source = itemBackup.Source,
  //          CurrentFileName = itemBackup.CurrentFileName,
  //          CurrentFileSize = itemBackup.CurrentFileSize,
  //          CurrentFileTransmitted = itemBackup.CurrentFileTransmitted,
		//};
        //ItemSourceVm.SetIsDownload(itemBackup.IsDownload);
    }

    // GetSourceFromStorageCommand
    [RelayCommand]
    public async Task OnGetSourceFromStorageAsync()
    {
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            if (ItemSourceVm.SourceUid != SourceUid)
                SourceUid = ItemSourceVm.SourceUid;
            TgSqlTableSourceModel source = await ContextManager.SourceRepository.GetAsync(SourceUid);
			SetItemSourceVm(source, source.Uid);
        }, true);
    }

    /// <summary>
    /// Update download file state.
    /// </summary>
    /// <param name="sourceId"></param>
    /// <param name="messageId"></param>
    /// <param name="fileName"></param>
    /// <param name="fileSize"></param>
    /// <param name="transmitted"></param>
    /// <param name="fileSpeed"></param>
    /// <param name="isFileNewDownload"></param>
    /// <returns></returns>
	private async Task UpdateStateFileAsync(long sourceId, int messageId, string fileName, long fileSize, long transmitted, long fileSpeed, bool isFileNewDownload)
    {
	    await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
	    {
		    await Task.Delay(TimeSpan.FromMilliseconds(1));
			// Download job.
			if (!string.IsNullOrEmpty(fileName) && !isFileNewDownload && ItemSourceVm.SourceId.Equals(sourceId))
            {
				// Download chart job.
				if (fileSize > FileSizeMinSizeMonitoring && SwDownloadChart.Elapsed.Seconds > 5)
                {
	                ChartVisibility = Visibility.Visible;
                    LineSerie.Values.Add(fileSpeed / 1024 / 1024);
                    FileSpeedFormatterX.Add($"{SwDownloadChart.Elapsed.Seconds}");
                }
				else
				{
					ChartVisibility = Visibility.Hidden;
				}
				// Download status job.
				ItemSourceVm.SourceFirstId = messageId;
				ItemSourceVm.CurrentFileName = fileName;
                ItemSourceVm.CurrentFileSize = fileSize;
                ItemSourceVm.CurrentFileTransmitted = transmitted;
                ItemSourceVm.CurrentFileSpeed = fileSpeed;
			}
			// Download reset.
			else
			{
				ChartVisibility = Visibility.Hidden;
				// Download chart reset.
				SwDownloadChart = Stopwatch.StartNew();
				ItemSourceVm.CurrentFileName = fileName;
				LineSerie = new()
				{
					Title = fileName,
					Values = new ChartValues<long>(),
					StrokeThickness = 0.5,
					PointGeometry = null,
				};
				FileSpeedFormatterX = new List<string>();
				FileSpeedSeries.Clear();
				FileSpeedSeries.Add(LineSerie);
				// Download status reset.
				ItemSourceVm.SourceFirstId = messageId;
				ItemSourceVm.CurrentFileName = string.Empty;
				ItemSourceVm.CurrentFileSize = 0;
				ItemSourceVm.CurrentFileTransmitted = 0;
				ItemSourceVm.CurrentFileSpeed = 0;
			}
		}, true);
    }

	// UpdateSourceFromTelegramCommand
	[RelayCommand]
    public async Task OnUpdateSourceFromTelegramAsync()
    {
        if (!CheckClientReady())
            return;

        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            if (ItemSourceVm.SourceUid != SourceUid)
				SourceUid = ItemSourceVm.SourceUid;
            // Collect chats from Telegram.
            if (!TgDesktopUtils.TgClient.DicChatsAll.Any())
                await TgDesktopUtils.TgClient.CollectAllChatsAsync();
			// Download settings.
            TgDownloadSettingsModel tgDownloadSettings = TgDesktopUtils.TgDownloadsVm.CreateDownloadSettings(ItemSourceVm);
			// Update source from Telegram.
			await TgDesktopUtils.TgClient.UpdateSourceDbAsync(ItemSourceVm, tgDownloadSettings);
            await ContextManager.SourceRepository.SaveAsync(ItemSourceVm.Source);
            // Message.
            await TgDesktopUtils.TgClient.UpdateStateSourceAsync(ItemSourceVm.Source.Id, ItemSourceVm.Source.FirstId, TgDesktopUtils.TgLocale.SettingsSource);
        }, false);

        await OnGetSourceFromStorageAsync();
    }

    // DownloadSourceCommand
    [RelayCommand]
    public async Task<bool> OnDownloadSourceAsync()
    {
        if (!CheckClientReady())
            return false;

        await OnUpdateSourceFromTelegramAsync();

        bool result = true;
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            // Check directory.
            if (!Directory.Exists(ItemSourceVm.Source.Directory))
            {
                await TgDesktopUtils.TgClient.UpdateStateSourceAsync(ItemSourceVm.Source.Id, ItemSourceVm.Source.FirstId,
                    $"Directory is not exists! {ItemSourceVm.Source.Directory}");
                result = false;
                return;
            }

            // Download settings.
            TgDownloadSettingsModel tgDownloadSettings = TgDesktopUtils.TgDownloadsVm.CreateDownloadSettings(ItemSourceVm);
            SwDownloadChart = Stopwatch.StartNew();
			// Job.
			await TgDesktopUtils.TgClient.DownloadAllDataAsync(tgDownloadSettings);
			await TgDesktopUtils.TgClient.UpdateStateSourceAsync(ItemSourceVm.Source.Id, ItemSourceVm.Source.FirstId, TgDesktopUtils.TgLocale.SettingsSource);
        }, true);

        await OnGetSourceFromStorageAsync();
        return result;
    }

	// StopSourceCommand
	[RelayCommand]
	public async Task<bool> OnStopSourceAsync()
	{
        if (!CheckClientReady()) return false;

        bool result = true;
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
	        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
	        ItemSourceVm.SetIsDownload(false);
        }, true);

        await OnGetSourceFromStorageAsync();
        return result;
	}

	// ClearViewCommand
	[RelayCommand]
    public async Task OnClearViewAsync()
    {
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            //if (ItemSourceVm.SourceUid != SourceUid)
            //    SourceUid = ItemSourceVm.SourceUid;
            ItemSourceVm.Source = await ContextManager.SourceRepository.GetNewAsync();
        }, false);
    }

    // SaveSourceCommand
    [RelayCommand]
    public async Task OnSaveSourceAsync()
    {
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            await ContextManager.SourceRepository.SaveAsync(ItemSourceVm.Source, true);
        }, false);

        await OnGetSourceFromStorageAsync();
    }

    // ReturnToSectionSourcesCommand
    [RelayCommand]
    public async Task OnReturnToSectionSourcesAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            if (Application.Current.MainWindow is MainWindow navigationWindow)
            {
                navigationWindow.ShowWindow();
                navigationWindow.Navigate(typeof(TgSourcesPage));
            }
        }, false);
    }

    // CopyCommand
    [RelayCommand]
    public async Task OnCopyAsync(string fieldName)
    {
	    await TgDesktopUtils.RunFuncAsync(this, async () =>
	    {
		    await Task.Delay(TimeSpan.FromMilliseconds(1));
		    string value = fieldName switch
		    {
			    nameof(ItemSourceVm.SourceId) => ItemSourceVm.SourceId.ToString(),
			    nameof(ItemSourceVm.SourceUserName) => ItemSourceVm.SourceUserName,
			    nameof(ItemSourceVm.SourceTitle) => ItemSourceVm.SourceTitle,
			    nameof(ItemSourceVm.SourceAbout) => ItemSourceVm.SourceAbout,
			    nameof(ItemSourceVm.SourceFirstId) => ItemSourceVm.SourceFirstId.ToString(),
			    nameof(ItemSourceVm.SourceLastId) => ItemSourceVm.SourceLastId.ToString(),
			    nameof(ItemSourceVm.SourceDirectory) => ItemSourceVm.SourceDirectory,
			    nameof(ItemSourceVm.CurrentFileName) => ItemSourceVm.CurrentFileName,
			    nameof(ItemSourceVm.SourceDtChangedString) => ItemSourceVm.SourceDtChangedString,
			    _ => string.Empty
		    };
		    Clipboard.SetText(value);
		    await UpdateStateSourceAsync(0, 0, $"{fieldName} is copied");
	    }, false);
    }

	#endregion
}