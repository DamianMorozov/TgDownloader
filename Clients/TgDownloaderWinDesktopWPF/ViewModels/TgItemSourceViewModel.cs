// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktopWPF.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgItemSourceViewModel : TgPageViewModelBase, INavigationAware
{
    #region Public and private fields, properties, constructor

    private TgEfSourceRepository SourceRepository { get; } = new(TgEfUtils.EfContext);
    public TgEfSourceViewModel ItemSourceVm { get; } = new();
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
	    if (ItemSourceVm.Dto.Id.Equals(sourceId))
	    {
		    await OnGetSourceFromStorageAsync();
	    }
        //else
        //{
        //    await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        //    {
        //        await Task.Delay(1);
        //        SourceUid = Guid.Empty;
        //        TgSqlTableSourceModel source = await ContextManager.SourceRepository.GetNewAsync();
        //        SetItemSourceVm(source, source.Uid);
        //    }, true);
        //}
    }

    public void SetItemSourceVm(TgEfSourceViewModel itemSourceVm) => SetItemSourceVm(itemSourceVm.Dto);

    public void SetItemSourceVm(TgEfSourceDto dto)
    {
        ItemSourceVm.Dto.Fill(dto, isUidCopy: false);
    }

    // GetSourceFromStorageCommand
    [RelayCommand]
    public async Task OnGetSourceFromStorageAsync()
    {
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await Task.Delay(1);
            if (ItemSourceVm.Dto.Uid != SourceUid)
                SourceUid = ItemSourceVm.Dto.Uid;
            TgEfSourceEntity source = (await SourceRepository.GetAsync(new TgEfSourceEntity() { Uid = SourceUid })).Item;
            var dto = TgEfHelper.ConvertToDto(source);
			SetItemSourceVm(dto);
        }, true);
    }

    /// <summary> Update download file state </summary>
    private async Task UpdateStateFileAsync(long sourceId, int messageId, string fileName, long fileSize, long transmitted, long fileSpeed, bool isFileNewDownload,
	    int threadNumber)
    {
	    await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
	    {
		    await Task.Delay(1);
			// Download job.
			if (!string.IsNullOrEmpty(fileName) && !isFileNewDownload && ItemSourceVm.Dto.Id.Equals(sourceId))
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
				ItemSourceVm.Dto.FirstId = messageId;
				ItemSourceVm.Dto.CurrentFileName = fileName;
                ItemSourceVm.Dto.CurrentFileSize = fileSize;
                ItemSourceVm.Dto.CurrentFileTransmitted = transmitted;
                ItemSourceVm.Dto.CurrentFileSpeed = fileSpeed;
			}
			// Download reset.
			else
			{
				ChartVisibility = Visibility.Hidden;
				// Download chart reset.
				SwDownloadChart = Stopwatch.StartNew();
				ItemSourceVm.Dto.CurrentFileName = fileName;
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
				ItemSourceVm.Dto.FirstId = messageId;
				ItemSourceVm.Dto.CurrentFileName = string.Empty;
				ItemSourceVm.Dto.CurrentFileSize = 0;
				ItemSourceVm.Dto.CurrentFileTransmitted = 0;
				ItemSourceVm.Dto.CurrentFileSpeed = 0;
			}
		}, true);
    }

	// UpdateSourceFromTelegramCommand
	[RelayCommand]
    public async Task OnUpdateSourceFromTelegramAsync()
    {
        if (!await CheckClientReadyAsync())
            return;

        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await Task.Delay(1);
            if (ItemSourceVm.Dto.Uid != SourceUid)
				SourceUid = ItemSourceVm.Dto.Uid;
            // Collect chats from Telegram
            if (!TgDesktopUtils.TgClient.DicChatsAll.Any())
                await TgDesktopUtils.TgClient.CollectAllChatsAsync();
			// Download settings
            TgDownloadSettingsViewModel tgDownloadSettings = TgDesktopUtils.TgDownloadsVm.CreateDownloadSettings(ItemSourceVm);
			// Update source from Telegram
			await TgDesktopUtils.TgClient.UpdateSourceDbAsync(ItemSourceVm, tgDownloadSettings);
            var entity = TgEfHelper.ConvertToEntity(ItemSourceVm.Dto);
            await SourceRepository.SaveAsync(entity);
            // Message
            await TgDesktopUtils.TgClient.UpdateStateSourceAsync(ItemSourceVm.Dto.Id, ItemSourceVm.Dto.FirstId, TgDesktopUtils.TgLocale.SettingsSource);
        }, false);

        await OnGetSourceFromStorageAsync();
    }

    // DownloadSourceCommand
    [RelayCommand]
    public async Task<bool> OnDownloadSourceAsync()
    {
        if (!await CheckClientReadyAsync())
            return false;

        await OnUpdateSourceFromTelegramAsync();

        bool result = true;
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await Task.Delay(1);
            // Check directory.
            if (!Directory.Exists(ItemSourceVm.Dto.Directory))
            {
                await TgDesktopUtils.TgClient.UpdateStateSourceAsync(ItemSourceVm.Dto.Id, ItemSourceVm.Dto.FirstId,
                    $"Directory is not exists! {ItemSourceVm.Dto.Directory}");
                result = false;
                return;
            }

            // Download settings.
            TgDownloadSettingsViewModel tgDownloadSettings = TgDesktopUtils.TgDownloadsVm.CreateDownloadSettings(ItemSourceVm);
            SwDownloadChart = Stopwatch.StartNew();
			// Job.
			await TgDesktopUtils.TgClient.DownloadAllDataAsync(tgDownloadSettings);
			await TgDesktopUtils.TgClient.UpdateStateSourceAsync(ItemSourceVm.Dto.Id, ItemSourceVm.Dto.FirstId, TgDesktopUtils.TgLocale.SettingsSource);
        }, true);

        await OnGetSourceFromStorageAsync();
        return result;
    }

	// StopSourceCommand
	[RelayCommand]
	public async Task<bool> OnStopSourceAsync()
	{
        if (!await CheckClientReadyAsync()) return false;

        bool result = true;
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
	        await Task.Delay(1).ConfigureAwait(false);
	        ItemSourceVm.Dto.SetIsDownload(false);
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
            await Task.Delay(1);
            //if (ItemSourceVm.SourceUid != SourceUid)
            //    SourceUid = ItemSourceVm.SourceUid;
            var entity = (await SourceRepository.GetNewAsync()).Item;
            ItemSourceVm.Dto = TgEfHelper.ConvertToDto(entity);
        }, false);
    }

    // SaveSourceCommand
    [RelayCommand]
    public async Task OnSaveSourceAsync()
    {
        await TgDesktopUtils.RunFuncAsync(ViewModel ?? this, async () =>
        {
            await Task.Delay(1);
            var entity = TgEfHelper.ConvertToEntity(ItemSourceVm.Dto);
            await SourceRepository.SaveAsync(entity);
        }, false);

        await OnGetSourceFromStorageAsync();
    }

    // ReturnToSectionSourcesCommand
    [RelayCommand]
    public async Task OnReturnToSectionSourcesAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(1);
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
		    await Task.Delay(1);
		    string value = fieldName switch
		    {
			    nameof(ItemSourceVm.Dto.Id) => ItemSourceVm.Dto.Id.ToString(),
			    nameof(ItemSourceVm.Dto.UserName) => ItemSourceVm.Dto.UserName,
			    nameof(ItemSourceVm.Dto.Title) => ItemSourceVm.Dto.Title,
			    nameof(ItemSourceVm.Dto.About) => ItemSourceVm.Dto.About,
			    nameof(ItemSourceVm.Dto.FirstId) => ItemSourceVm.Dto.FirstId.ToString(),
			    nameof(ItemSourceVm.Dto.Count) => ItemSourceVm.Dto.Count.ToString(),
			    nameof(ItemSourceVm.Dto.Directory) => ItemSourceVm.Dto.Directory,
			    nameof(ItemSourceVm.Dto.CurrentFileName) => ItemSourceVm.Dto.CurrentFileName,
			    nameof(ItemSourceVm.Dto.SourceDtChangedString) => ItemSourceVm.Dto.SourceDtChangedString,
			    _ => string.Empty
		    };
		    Clipboard.SetText(value);
		    await UpdateStateSourceAsync(0, 0, $"{fieldName} is copied");
	    }, false);
    }

	#endregion
}