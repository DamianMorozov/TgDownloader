// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSourcesViewModel : TgPageViewModelBase, INavigationAware
{
	#region Public and private fields, properties, constructor

	public ObservableCollection<TgSqlTableSourceViewModel> SourcesVms { get; set; }

	public TgSourcesViewModel()
	{
		SourcesVms = new();
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
		TgDesktopUtils.TgClient.UpdateStateSource = UpdateStateSource;
		LoadSourcesCommand.Execute(null);
	}

	/// <summary>
	/// Sort sources.
	/// </summary>
	private void SetOrderSources(IEnumerable<TgSqlTableSourceModel> sources)
	{
		// exe inside other TgDesktopUtils.RunAction(this, () =>
		sources = sources.OrderBy(x => x.Title).ToList()
			.OrderBy(x => x.UserName);
		SourcesVms.Clear();
		foreach (TgSqlTableSourceModel source in sources)
			SourcesVms.Add(new(source, LoadSource, UpdateSource, DownloadSource, EditSource));
	}

    /// <summary>
    /// Load sources from Telegram.
    /// </summary>
    /// <param name="sourceVm"></param>
    public void LoadFromTelegram(TgSqlTableSourceViewModel sourceVm)
    {
        TgDesktopUtils.RunAction(this, () =>
        {
            TgSqlTableSourceModel sourceDb = ContextManager.SourceRepository.Get(sourceVm.Source.Id);
            if (sourceDb.IsExists)
                sourceVm = new(sourceDb);
            SourcesVms.Add(sourceVm);
        });
    }

    /// <summary>
    /// Save sources into the Storage.
    /// </summary>
    public void SaveSourcesIntoStorage()
	{
		TgDesktopUtils.RunAction(this, () =>
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
			});
	}

	/// <summary>
	/// Create new download settings.
	/// </summary>
	/// <param name="sourceVm"></param>
	/// <returns></returns>
	public TgDownloadSettingsModel CreateDownloadSettings(TgSqlTableSourceViewModel sourceVm) =>
        new() { SourceVm = new TgSqlTableSourceViewModel()
            {
                SourceId = sourceVm.Source.Id,
                SourceFirstId = sourceVm.Source.FirstId,
                SourceDirectory = sourceVm.Source.Directory
            }
        };

    /// <summary>
	/// Update state.
	/// </summary>
	/// <param name="sourceId"></param>
	/// <param name="messageId"></param>
	/// <param name="message"></param>
	public override void UpdateStateSource(long sourceId, int messageId, string message)
	{
		base.UpdateStateSource(sourceId, messageId, message);
		TgDesktopUtils.RunAction(() =>
		{
		LoadSource(sourceId);
		});
	}

	#endregion

	#region Public and private methods - RelayCommand

	[RelayCommand]
	public async Task OnLoadSourcesAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgDesktopUtils.RunAction(this, () =>
		{
			SetOrderSources(ContextManager.SourceRepository.GetEnumerable());
		});
	}

	[RelayCommand]
	public async Task OnUpdateSourcesAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
				TgDesktopUtils.RunAction(this, () =>
				{
		if (!CheckClientReady()) return;
		foreach (TgSqlTableSourceViewModel sourceVm in SourcesVms) 
			UpdateSource(sourceVm);
				});
	}

	[RelayCommand]
	public async Task OnGetSourcesAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgDesktopUtils.RunAction(this, () =>
				{
		if (!CheckClientReady()) return;
		SourcesVms.Clear();
		TgDesktopUtils.TgClient.ScanSourcesTgDesktop(TgEnumSourceType.Chat, LoadFromTelegram);
		TgDesktopUtils.TgClient.ScanSourcesTgDesktop(TgEnumSourceType.Dialog, LoadFromTelegram);
				});
	}

	[RelayCommand]
	public async Task OnClearViewAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgDesktopUtils.RunAction(this, SourcesVms.Clear);
	}

	[RelayCommand]
	public async Task OnSortViewAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
				TgDesktopUtils.RunAction(this, () =>
				{
					SetOrderSources(SourcesVms.Select(x => x.Source).ToList());
				});
	}

	[RelayCommand]
	public async Task OnSaveSourcesAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgDesktopUtils.RunAction(this, SaveSourcesIntoStorage);
	}

	#endregion

	#region Public and private methods - RelayCommand from TgSqlTableSourceViewModel

	/// <summary>
	/// Load source from Storage.
	/// </summary>
	/// <param name="sourceVm"></param>
	internal void LoadSource(TgSqlTableSourceViewModel sourceVm)
	{
		TgDesktopUtils.RunAction(() =>
		{
			for (int i = 0; i < SourcesVms.Count; i++)
			{
				if (SourcesVms[i].SourceId.Equals(sourceVm.SourceId))
				{
					SourcesVms[i].Source = ContextManager.SourceRepository.Get(sourceVm.SourceId);
					break;
				}
			}
		});
	}

	/// <summary>
	/// Load source from Storage.
	/// </summary>
	/// <param name="sourceId"></param>
	internal void LoadSource(long sourceId)
	{
		TgDesktopUtils.RunAction(() =>
		{
			for (int i = 0; i < SourcesVms.Count; i++)
			{
				if (SourcesVms[i].SourceId.Equals(sourceId))
				{
					SourcesVms[i].Source = ContextManager.SourceRepository.Get(sourceId);
					break;
				}
			}
		});
	}

	/// <summary>
	/// Update source.
	/// </summary>
	/// <param name="sourceVm"></param>
	public void UpdateSource(TgSqlTableSourceViewModel sourceVm)
	{
		// Checks.
		if (!CheckClientReady()) return;
		TgDesktopUtils.RunAction(sourceVm, () =>
		{
			// Collect chats from Telegram.
		if (!TgDesktopUtils.TgClient.DicChatsAll.Any())
			TgDesktopUtils.TgClient.CollectAllChatsConsole();
		// Download settings.
		TgDownloadSettingsModel tgDownloadSettings = CreateDownloadSettings(sourceVm);
		// Update source from Telegram.
		TgDesktopUtils.TgClient.UpdateSourceDb(sourceVm, tgDownloadSettings);
		ContextManager.SourceRepository.Save(sourceVm.Source);
		// Check directory.
		if (!Directory.Exists(sourceVm.Source.Directory))
			TgDesktopUtils.TgClient.UpdateStateSource(sourceVm.Source.Id, sourceVm.Source.FirstId, $"Directory is not exists! {sourceVm.Source.Directory}");
		TgDesktopUtils.TgClient.UpdateStateClient(TgDesktopUtils.TgLocale.SettingsSource);
		TgDesktopUtils.TgClient.UpdateStateSource(sourceVm.Source.Id, sourceVm.Source.FirstId, TgDesktopUtils.TgLocale.SettingsSource);
			//TgDesktopUtils.TgClient.UpdateStateClient($"{TgLocale.GetSourceInfo} | {sourceVm.Source.Id} | {sourceVm.Source.UserName} | {sourceVm.Source.Title}");
		// Job.
		
		TgDesktopUtils.TgClient.UpdateStateClient(TgDesktopUtils.TgLocale.SettingsSource);
		});
	}

	/// <summary>
	/// Download source.
	/// </summary>
	/// <param name="sourceVm"></param>
	public void DownloadSource(TgSqlTableSourceViewModel sourceVm)
	{
		// Checks.
		if (!CheckClientReady()) return;
		// Scan source.
		UpdateSource(sourceVm);
		
		TgDesktopUtils.RunAction(sourceVm, () =>
		{
			// Check directory.
		if (!Directory.Exists(sourceVm.Source.Directory))
		{
			TgDesktopUtils.TgClient.UpdateStateSource(sourceVm.Source.Id, sourceVm.Source.FirstId, $"Directory is not exists! {sourceVm.Source.Directory}");
			return;
		}
			// Download settings.
		TgDownloadSettingsModel tgDownloadSettings = CreateDownloadSettings(sourceVm);
			// Job.
			TgDesktopUtils.TgClient.DownloadAllData(tgDownloadSettings);
			TgDesktopUtils.TgClient.UpdateStateClient(TgDesktopUtils.TgLocale.SettingsSource);
		});
	}

	/// <summary>
	/// Open edit page for source.
	/// </summary>
	/// <param name="sourceVm"></param>
	public void EditSource(TgSqlTableSourceViewModel sourceVm)
	{
		TgDesktopUtils.RunAction(sourceVm, () =>
		{
			//	TgDesktopUtils.RunAction(() =>
	//	{
	//	if (sender is not TextBox textBox)
	//		return;
	//	if (textBox.Tag is not long sourceId)
	//		return;
	//	if (!Directory.Exists(textBox.Text))
	//		return;
	//	TgSqlTableSourceModel sourceDb = TgDesktopUtils.TgSourcesVm.ContextManager.ContextTableSources.Get(sourceId);
	//	if (!sourceDb.IsExists)
	//		return;
	//	sourceDb.Directory = textBox.Text;
	//	TgDesktopUtils.TgSourcesVm.ContextManager.ContextTableSources.Save(sourceDb);
	//	});
		});
	}

	#endregion
}