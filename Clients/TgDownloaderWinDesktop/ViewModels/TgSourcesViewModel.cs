// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSourcesViewModel : TgPageViewModelBase, INavigationAware
{
    #region Public and private fields, properties, constructor

    public ObservableCollection<TgSqlTableSourceViewModel> SourcesVms { get; set; } = new();
    public Visibility GridVisibility { get; set; } = Visibility.Visible;

    #endregion

    #region Public and private methods

    public void OnNavigatedTo()
    {
        //if (!IsInitialized)
        InitializeViewModel();
    }

    public void OnNavigatedFrom()
    {
        //
    }

    protected override void InitializeViewModel()
    {
        base.InitializeViewModel();

        OnGetSourcesFromStorageAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Sort sources.
    /// </summary>
    private void SetOrderSources(IEnumerable<TgSqlTableSourceModel> sources)
    {
        SourcesVms.Clear();

        sources = sources.OrderBy(x => x.Title).ThenBy(x => x.UserName).ToList();
        foreach (TgSqlTableSourceModel source in sources)
        {
            SourcesVms.Add(new(source));
        }
    }

    /// <summary>
    /// Load sources from Telegram.
    /// </summary>
    /// <param name="sourceVm"></param>
    public void LoadFromTelegram(TgSqlTableSourceViewModel sourceVm)
    {
        TgSqlTableSourceModel sourceDb = ContextManager.SourceRepository.Get(sourceVm.Source.Id);
        if (sourceDb.IsExists)
            sourceVm = new(sourceDb);
        SourcesVms.Add(sourceVm);
    }

    /// <summary>
	/// Update state.
	/// </summary>
	/// <param name="sourceId"></param>
	/// <param name="messageId"></param>
	/// <param name="message"></param>
	public override void UpdateStateSource(long sourceId, int messageId, string message)
    {
        base.UpdateStateSource(sourceId, messageId, message);
        for (int i = 0; i < SourcesVms.Count; i++)
        {
            if (SourcesVms[i].SourceId.Equals(sourceId))
            {
                SourcesVms[i].Source = ContextManager.SourceRepository.Get(sourceId);
                break;
            }
        }
    }

    #endregion

    #region Public and private methods - RelayCommand - All

    // GetSourcesFromStorageCommand
    [RelayCommand]
    public async Task OnGetSourcesFromStorageAsync()
    {
        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            SetOrderSources(ContextManager.SourceRepository.GetEnumerable());
        }, false).ConfigureAwait(false);
    }

    // UpdateSourcesFromTelegram
    [RelayCommand]
    public async Task OnUpdateSourcesFromTelegramAsync()
    {
        if (!CheckClientReady())
            return;

        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            GridVisibility = Visibility.Hidden;
            foreach (TgSqlTableSourceViewModel sourceVm in SourcesVms)
                OnUpdateSourceFromTelegramAsync(sourceVm).ConfigureAwait(true);
            GridVisibility = Visibility.Visible;
        }, false).ConfigureAwait(false);
    }

    // GetSourcesFromTelegramCommand
    [RelayCommand]
    public async Task OnGetSourcesFromTelegramAsync()
    {
        if (!CheckClientReady())
            return;

        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            SourcesVms.Clear();
            GridVisibility = Visibility.Hidden;
            TgDesktopUtils.TgClient.ScanSourcesTgDesktop(TgEnumSourceType.Chat,
                sourceVm => LoadFromTelegram(sourceVm));
            TgDesktopUtils.TgClient.ScanSourcesTgDesktop(TgEnumSourceType.Dialog,
                sourceVm => LoadFromTelegram(sourceVm));
            GridVisibility = Visibility.Visible;
        }, false).ConfigureAwait(false);
    }

    // ClearViewCommand
    [RelayCommand]
    public async Task OnClearViewAsync()
    {
        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            SourcesVms.Clear();
        }, false).ConfigureAwait(true);
    }

    // SortViewCommand

    // SortViewCommand
    [RelayCommand]
    public async Task OnSortViewAsync()
    {
        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            if (SourcesVms.Any())
                SetOrderSources(SourcesVms.Select(x => x.Source).ToList());
        }, false).ConfigureAwait(false);
    }

    /// <summary>
    /// Save sources into the Storage.
    /// </summary>
    // SaveSourcesCommand
    [RelayCommand]
    public async Task OnSaveSourcesAsync()
    {
        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            // Checks.
            if (!SourcesVms.Any())
            {
                TgDesktopUtils.TgClient.UpdateStateSource(0, 0, "Empty sources list!");
                return;
            }
            foreach (TgSqlTableSourceViewModel sourceVm in SourcesVms)
            {
                TgSqlTableSourceModel sourceDb = ContextManager.SourceRepository.Get(sourceVm.Source.Id);
                if (!sourceDb.IsExists)
                {
                    ContextManager.SourceRepository.Save(sourceVm.Source);
                    TgDesktopUtils.TgClient.UpdateStateSource(sourceVm.Source.Id, 0, $"Saved source | {sourceVm.Source}");
                }
            }
        }, false).ConfigureAwait(false);
    }

    #endregion

    #region Public and private methods - RelayCommand - One

    // GetSourceFromStorageCommand
    [RelayCommand]
    public async Task OnGetSourceFromStorageAsync(TgSqlTableSourceViewModel sourceVm)
    {
        TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
        await TgDesktopUtils.TgItemSourceVm.OnGetSourceFromStorageAsync();

        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            for (int i = 0; i < SourcesVms.Count; i++)
            {
                if (SourcesVms[i].SourceId.Equals(sourceVm.SourceId))
                {
                    SourcesVms[i].Source.Fill(TgDesktopUtils.TgItemSourceVm.ItemSourceVm.Source, TgDesktopUtils.TgItemSourceVm.ItemSourceVm.Source.Uid);
                    break;
                }
            }
        }, false).ConfigureAwait(false);
    }

    // UpdateSourceFromTelegram
    [RelayCommand]
    public async Task OnUpdateSourceFromTelegramAsync(TgSqlTableSourceViewModel sourceVm)
    {
        TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
        await TgDesktopUtils.TgItemSourceVm.OnUpdateSourceFromTelegramAsync();

        await OnGetSourceFromStorageAsync(sourceVm).ConfigureAwait(false);
    }

    // DownloadCommand
    [RelayCommand]
    public async Task OnDownloadAsync(TgSqlTableSourceViewModel sourceVm)
    {
        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
            TgDesktopUtils.TgItemSourceVm.ViewModel = this;
            TgDesktopUtils.TgItemSourceVm.OnDownloadSourceAsync().ConfigureAwait(true);
            TgDesktopUtils.TgItemSourceVm.OnUpdateSourceFromTelegramAsync().ConfigureAwait(true);
        }, false).ConfigureAwait(false);
    }

    // EditSourceCommand
    [RelayCommand]
    public async Task OnEditSourceAsync(TgSqlTableSourceViewModel sourceVm)
    {
        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            if (Application.Current.MainWindow is MainWindow navigationWindow)
            {
                TgDesktopUtils.TgItemSourceVm.SetItemSourceVm(sourceVm);
                navigationWindow.ShowWindow();
                navigationWindow.Navigate(typeof(TgItemSourcePage));
            }
        }, false).ConfigureAwait(false);
    }

    #endregion
}