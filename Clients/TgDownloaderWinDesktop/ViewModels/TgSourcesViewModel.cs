// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSourcesViewModel : TgPageViewModelBase, INavigationAware
{
    #region Public and private fields, properties, constructor

    public ObservableCollection<TgSqlTableSourceViewModel> SourcesVms { get; set; } = new();

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

        await OnLoadSourcesFromStorageAsync();
    }

    /// <summary>
    /// Sort sources.
    /// </summary>
    private void SetOrderSources(IEnumerable<TgSqlTableSourceModel> sources)
    {
        List<TgSqlTableSourceModel> listSources = sources.ToList();
        if (!listSources.Any()) return;
        SourcesVms = new();

        sources = listSources.OrderBy(x => x.UserName).ThenBy(x => x.Title).ToList();
        if (sources.Any())
            foreach (TgSqlTableSourceModel source in sources)
                SourcesVms.Add(new(source));
    }

    /// <summary>
    /// Load sources from Telegram.
    /// </summary>
    /// <param name="sourceVm"></param>
    public async Task LoadFromTelegramAsync(TgSqlTableSourceViewModel sourceVm)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            TgSqlTableSourceModel sourceDb = await ContextManager.SourceRepository.GetAsync(sourceVm.Source.Id);
            if (sourceDb.IsExists)
                sourceVm = new(sourceDb);
            if (!SourcesVms.Select(x => x.SourceId).Contains(sourceVm.SourceId))
                SourcesVms.Add(sourceVm);
            await SaveSourceAsync(sourceVm);
        }, false);
    }

    /// <summary>
    /// Update state.
    /// </summary>
    /// <param name="sourceId"></param>
    /// <param name="messageId"></param>
    /// <param name="message"></param>
    public override async Task UpdateStateSourceAsync(long sourceId, int messageId, string message)
    {
        await base.UpdateStateSourceAsync(sourceId, messageId, message);
        for (int i = 0; i < SourcesVms.Count; i++)
        {
            if (SourcesVms[i].SourceId.Equals(sourceId))
            {
                SourcesVms[i].Source = await ContextManager.SourceRepository.GetAsync(sourceId);
                break;
            }
        }
    }

    #endregion

    #region Public and private methods - RelayCommand - All

    // LoadSourcesFromStorageCommand
    [RelayCommand]
    public async Task OnLoadSourcesFromStorageAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            List<TgSqlTableSourceModel> sources = ContextManager.SourceRepository.GetEnumerable().ToList();
            //if (!sources.Any())
            //{
            //    TgSqlTableSourceModel sourceDefault = ContextManager.SourceRepository.CreateNew(true);
            //    sourceDefault.Id = 1710176740;
            //    sourceDefault.UserName = "TgDownloader";
            //    sourceDefault.Directory = Path.Combine(Environment.CurrentDirectory, sourceDefault.UserName);
            //    sourceDefault.Title = "TgDownloader News";
            //    sourceDefault.About = "Telegram Files Downloader";
            //    await ContextManager.SourceRepository.SaveAsync(sourceDefault);
            //    sources.Add(sourceDefault);
            //}

            SetOrderSources(sources);
        }, false);
    }

    // UpdateSourcesFromTelegram
    [RelayCommand]
    public async Task OnUpdateSourcesFromTelegramAsync()
    {
        if (!CheckClientReady())
            return;

        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            foreach (TgSqlTableSourceViewModel sourceVm in SourcesVms)
                await OnUpdateSourceFromTelegramAsync(sourceVm);
        }, false);
    }

    // GetSourcesFromTelegramCommand
    [RelayCommand]
    public async Task OnGetSourcesFromTelegramAsync()
    {
        if (!CheckClientReady())
            return;

        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            await TgDesktopUtils.TgClient.ScanSourcesTgDesktopAsync(TgEnumSourceType.Chat, LoadFromTelegramAsync);
            await TgDesktopUtils.TgClient.ScanSourcesTgDesktopAsync(TgEnumSourceType.Dialog, LoadFromTelegramAsync);
        }, false);
    }

    // ClearViewCommand
    [RelayCommand]
    public async Task OnClearViewAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            SourcesVms = new();
        }, false);
    }

    // SortViewCommand

    // SortViewCommand
    [RelayCommand]
    public async Task OnSortViewAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            SetOrderSources(SourcesVms.Select(x => x.Source).ToList());
        }, false);
    }

    /// <summary>
    /// Save sources into the Storage.
    /// </summary>
    // SaveSourcesCommand
    [RelayCommand]
    public async Task OnSaveSourcesAsync()
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            // Checks.
            if (!SourcesVms.Any())
            {
                await TgDesktopUtils.TgClient.UpdateStateSourceAsync(0, 0, "Empty sources list!");
                return;
            }
            foreach (TgSqlTableSourceViewModel sourceVm in SourcesVms)
            {
                await SaveSourceAsync(sourceVm);
            }
        }, false);
    }

    public async Task SaveSourceAsync(TgSqlTableSourceViewModel sourceVm)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            TgSqlTableSourceModel sourceDb = await ContextManager.SourceRepository.GetAsync(sourceVm.Source.Id);
            if (!sourceDb.IsExists)
            {
                await ContextManager.SourceRepository.SaveAsync(sourceVm.Source);
                await TgDesktopUtils.TgClient.UpdateStateSourceAsync(sourceVm.Source.Id, 0, $"Saved source | {sourceVm.Source}");
            }
        }, false);
    }

    #endregion

    #region Public and private methods - RelayCommand - One

    // GetSourceFromStorageCommand
    [RelayCommand]
    public async Task OnGetSourceFromStorageAsync(TgSqlTableSourceViewModel sourceVm)
    {
        TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
        await TgDesktopUtils.TgItemSourceVm.OnGetSourceFromStorageAsync();

        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            for (int i = 0; i < SourcesVms.Count; i++)
            {
                if (SourcesVms[i].SourceId.Equals(sourceVm.SourceId))
                {
                    SourcesVms[i].Source.Fill(TgDesktopUtils.TgItemSourceVm.ItemSourceVm.Source, TgDesktopUtils.TgItemSourceVm.ItemSourceVm.Source.Uid);
                    break;
                }
            }
        }, false);
    }

    // UpdateSourceFromTelegram
    [RelayCommand]
    public async Task OnUpdateSourceFromTelegramAsync(TgSqlTableSourceViewModel sourceVm)
    {
        TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
        await TgDesktopUtils.TgItemSourceVm.OnUpdateSourceFromTelegramAsync();

        await OnGetSourceFromStorageAsync(sourceVm);
    }

    // DownloadCommand
    [RelayCommand]
    public async Task OnDownloadAsync(TgSqlTableSourceViewModel sourceVm)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
            TgDesktopUtils.TgItemSourceVm.ViewModel = this;
            if (await TgDesktopUtils.TgItemSourceVm.OnDownloadSourceAsync())
                await TgDesktopUtils.TgItemSourceVm.OnUpdateSourceFromTelegramAsync();
        }, false);
    }

    // EditSourceCommand
    [RelayCommand]
    public async Task OnEditSourceAsync(TgSqlTableSourceViewModel sourceVm)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            if (Application.Current.MainWindow is MainWindow navigationWindow)
            {
                TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
                navigationWindow.ShowWindow();
                navigationWindow.Navigate(typeof(TgItemSourcePage));
            }
        }, false);
    }

    #endregion
}