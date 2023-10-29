// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgItemSourceViewModel : TgPageViewModelBase, INavigationAware
{
    #region Public and private fields, properties, constructor

    public TgSqlTableSourceViewModel ItemSourceVm { get; private set; } = new();
    public TgPageViewModelBase? ViewModel { get; set; }
    private long SourceId { get; set; }

    public TgItemSourceViewModel()
	{
		//
	}

	#endregion

	#region Public and private methods

	public void OnNavigatedTo()
	{
		if (!IsInitialized)
			InitializeViewModel();
    }

	public void OnNavigatedFrom()
	{
		//
	}

    protected override void InitializeViewModel()
    {
        base.InitializeViewModel();

        OnGetSourceFromStorageAsync().ConfigureAwait(false);
    }

    public void SetItemSourceVm(TgSqlTableSourceViewModel itemSourceVm) => 
        SetItemSourceVm(itemSourceVm.Source, itemSourceVm.Source.Uid);

    public void SetItemSourceVm(TgSqlTableSourceModel source, Guid? uid = null)
    {
        ItemSourceVm.Source.Fill(source, uid);
        TgSqlTableSourceViewModel itemBackup = ItemSourceVm;
        ItemSourceVm = new()
        {
            Source = itemBackup.Source,
        };
    }

    /// <summary>
    /// Create new download settings.
    /// </summary>
    /// <param name="sourceVm"></param>
    /// <returns></returns>
    public TgDownloadSettingsModel CreateDownloadSettings(TgSqlTableSourceViewModel sourceVm) =>
        new()
        {
            SourceVm = new TgSqlTableSourceViewModel()
            {
                SourceId = sourceVm.Source.Id,
                SourceFirstId = sourceVm.Source.FirstId,
                SourceDirectory = sourceVm.Source.Directory
            }
        };

    // GetSourceFromStorageCommand
    [RelayCommand]
    public async Task OnGetSourceFromStorageAsync()
    {
        await TgDesktopUtils.RunActionAsync(ViewModel ?? this, () =>
        {
            if (SourceId < 2)
                SourceId = ItemSourceVm.SourceId;
            var source = ContextManager.SourceRepository.Get(SourceId);
            SetItemSourceVm(source, source.Uid);
        }, true).ConfigureAwait(true);
    }

    // UpdateSourceFromTelegramCommand
    [RelayCommand]
    public async Task OnUpdateSourceFromTelegramAsync()
    {
        if (!CheckClientReady())
            return;

        await TgDesktopUtils.RunActionAsync(ViewModel ?? this, () =>
        {
            if (SourceId < 2)
                SourceId = ItemSourceVm.SourceId;
            // Collect chats from Telegram.
            if (!TgDesktopUtils.TgClient.DicChatsAll.Any())
                TgDesktopUtils.TgClient.CollectAllChatsDesktopAsync(_ => { }, _ => { });
            // Download settings.
            TgDownloadSettingsModel tgDownloadSettings = CreateDownloadSettings(ItemSourceVm);
            // Update source from Telegram.
            TgDesktopUtils.TgClient.UpdateSourceDb(ItemSourceVm, tgDownloadSettings);
            ContextManager.SourceRepository.Save(ItemSourceVm.Source);
            // Message.
            TgDesktopUtils.TgClient.UpdateStateMessage(TgDesktopUtils.TgLocale.SettingsSource);
            TgDesktopUtils.TgClient.UpdateStateSource(ItemSourceVm.Source.Id, ItemSourceVm.Source.FirstId, TgDesktopUtils.TgLocale.SettingsSource);
        }, false).ConfigureAwait(false);

        await OnGetSourceFromStorageAsync();
    }

    // DownloadSourceCommand
    [RelayCommand]
    public async Task OnDownloadSourceAsync()
    {
        if (!CheckClientReady())
            return;

        await OnUpdateSourceFromTelegramAsync();

        _ = Task.Run(async () =>
        {
            await TgDesktopUtils.RunAction2Async(ViewModel ?? this, () =>
            {
                // Check directory.
                if (!Directory.Exists(ItemSourceVm.Source.Directory))
                {
                    TgDesktopUtils.TgClient.UpdateStateSource(ItemSourceVm.Source.Id, ItemSourceVm.Source.FirstId,
                        $"Directory is not exists! {ItemSourceVm.Source.Directory}");
                    return;
                }

                // Download settings.
                TgDownloadSettingsModel tgDownloadSettings = CreateDownloadSettings(ItemSourceVm);
                // Job.
                TgDesktopUtils.TgClient.DownloadAllData(tgDownloadSettings);
                TgDesktopUtils.TgClient.UpdateStateMessage(TgDesktopUtils.TgLocale.SettingsSource);
            }, true).ConfigureAwait(false);
        }).ConfigureAwait(false);
    }

    // ClearViewCommand
    [RelayCommand]
    public async Task OnClearViewAsync()
    {
        await TgDesktopUtils.RunActionAsync(ViewModel ?? this, () =>
        {
            if (SourceId < 2)
                SourceId = ItemSourceVm.SourceId;
            ItemSourceVm.Source = ContextManager.SourceRepository.GetNew();
        }, false).ConfigureAwait(true);
    }

    // SaveSourceCommand
    [RelayCommand]
    public async Task OnSaveSourceAsyTask()
    {
        await TgDesktopUtils.RunActionAsync(ViewModel ?? this, () =>
        {
            if (ItemSourceVm.SourceId > 2)
                ContextManager.SourceRepository.Save(ItemSourceVm.Source);
        }, false).ConfigureAwait(false);
    }

    #endregion
}