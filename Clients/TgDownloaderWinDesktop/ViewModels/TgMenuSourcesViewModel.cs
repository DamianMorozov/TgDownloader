// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TL;

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgMenuSourcesViewModel : TgViewBase, INavigationAware
{
	#region Public and private fields, properties, constructor

	public ObservableCollection<TgMvvmSourceModel> MvvmSources { get; set; }

	public TgMenuSourcesViewModel()
	{
		MvvmSources = new();
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

	private void InitializeViewModel()
	{
		TgClient.UpdateStateSource = UpdateStateSource;
		IsInitialized = true;
	}

	/// <summary>
	/// Load sources from Storage.
	/// </summary>
	public void LoadSourcesFromStorage()
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			SetOrderSources(ContextManager.ContextTableSources.GetList());
		});
	}

	/// <summary>
	/// Clear sources.
	/// </summary>
	public void ClearSources() => Application.Current.Dispatcher.Invoke(MvvmSources.Clear);

	/// <summary>
	/// Sort sources.
	/// </summary>
	private void SetOrderSources(List<TgSqlTableSourceModel> sources)
	{
			Application.Current.Dispatcher.Invoke(() =>
		{
			sources = sources.OrderBy(x => x.Title).ToList().OrderBy(x => x.UserName).ToList();
			ClearSources();
			foreach (TgSqlTableSourceModel source in sources) 
				MvvmSources.Add(new(source, DownloadSourceFromTelegram));
		});
	}

	/// <summary>
	/// Load sources from Telegram.
	/// </summary>
	/// <param name="mvvmSource"></param>
	public void LoadFromTelegram(TgMvvmSourceModel mvvmSource)
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			TgSqlTableSourceModel sourceDb = ContextManager.ContextTableSources.GetItem(mvvmSource.Source.Id);
			if (sourceDb.IsExists) mvvmSource = new(sourceDb);
			MvvmSources.Add(mvvmSource);
		});
	}

	/// <summary>
	/// Save sources into the Storage.
	/// </summary>
	public void SaveSourcesIntoStorage()
	{
		// Checks.
		if (!MvvmSources.Any())
		{
			UpdateStateSource(0, 0, "Empty sources list!");
			return;
		}

		foreach (TgMvvmSourceModel mvvmSource in MvvmSources)
		{
			TgSqlTableSourceModel sourceDb = ContextManager.ContextTableSources.GetItem(mvvmSource.Source.Id);
			if (!sourceDb.IsExists)
			{
				ContextManager.ContextTableSources.AddOrUpdateItem(mvvmSource.Source);
				UpdateStateSource(mvvmSource.Source.Id, 0, $"Saved source | {mvvmSource.Source}");
			}
		}
	}

	/// <summary>
	/// Create new download settings.
	/// </summary>
	/// <param name="mvvmSource"></param>
	/// <returns></returns>
	public TgDownloadSettingsModel CreateDownloadSettings(TgMvvmSourceModel mvvmSource) =>
		new()
		{
			SourceId = mvvmSource.Source.Id,
			SourceFirstId = mvvmSource.Source.FirstId,
			DestDirectory = mvvmSource.Source.Directory
		};

	/// <summary>
	/// Update state.
	/// </summary>
	/// <param name="sourceId"></param>
	/// <param name="messageId"></param>
	/// <param name="message"></param>
	public void UpdateStateSource(long sourceId, int messageId, string message)
	{
		UpdateState(sourceId > 0
			? $"{TgDataFormatUtils.DtFormat(DateTime.Now)} | Source {sourceId} | Message {messageId} | {message}"
			: $"{TgDataFormatUtils.DtFormat(DateTime.Now)} | {message}");
		LoadSourceInfoFromStorage(sourceId);
	}

	/// <summary>
	/// Download source.
	/// </summary>
	/// <param name="mvvmSource"></param>
	public void DownloadSourceFromTelegram(TgMvvmSourceModel mvvmSource)
	{
		// Checks.
		if (!CheckClientReady()) return;
		TgSqlTableSourceModel sourceDb = ContextManager.ContextTableSources.GetItem(mvvmSource.Source.Id);

		// Prepare.
		mvvmSource.IsLoad = true;
		TgDownloadSettingsModel tgDownloadSettings = CreateDownloadSettings(mvvmSource);
		tgDownloadSettings.SourceFirstId = sourceDb.FirstId;
		tgDownloadSettings.DestDirectory = sourceDb.Directory;

		// Update source from Telegram.
		Channel? channel = TgClient.PrepareChannelDownloadMessages(tgDownloadSettings, true);
		if (channel is not null)
		{
			sourceDb.UserName = tgDownloadSettings.SourceUserName;
			sourceDb.Count = tgDownloadSettings.SourceLastId;
			sourceDb.Title = tgDownloadSettings.SourceTitle;
			sourceDb.About = tgDownloadSettings.SourceAbout;
		}
		else
		{
			ChatBase? chat = TgClient.PrepareChatBaseDownloadMessages(tgDownloadSettings, true);
			if (chat is not null)
			{
				sourceDb.UserName = tgDownloadSettings.SourceUserName;
				sourceDb.Count = tgDownloadSettings.SourceLastId;
				sourceDb.Title = tgDownloadSettings.SourceTitle;
				sourceDb.About = tgDownloadSettings.SourceAbout;
			}
		}
		ContextManager.ContextTableSources.AddOrUpdateItem(sourceDb);
		mvvmSource.Source = sourceDb;

		// Check directory.
		if (!Directory.Exists(sourceDb.Directory))
		{
			UpdateStateSource(mvvmSource.Source.Id, mvvmSource.FirstId,
				$"Directory is not exists! {sourceDb.Directory}");
			mvvmSource.IsLoad = false;
			return;
		}
		
		UpdateState(TgLocale.SettingsSource);
		// Job.
		TgClient.DownloadAllData(tgDownloadSettings,
			ContextManager.ContextTableMessages.StoreMessage,
			ContextManager.ContextTableDocuments.StoreDocument,
			ContextManager.ContextTableMessages.FindExistsMessage);
		UpdateState(TgLocale.SettingsSource);
		mvvmSource.IsLoad = false;
	}

	/// <summary>
	/// Load source first ID from Storage.
	/// </summary>
	/// <param name="sourceId"></param>
	internal void LoadSourceInfoFromStorage(long sourceId)
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			foreach (TgMvvmSourceModel mvvmSource in MvvmSources)
			{
				if (sourceId > 0 && mvvmSource.Source.Id.Equals(sourceId) || sourceId == 0)
				{
					mvvmSource.Source.FirstId = ContextManager.ContextTableSources.GetItem(sourceId).FirstId;
					mvvmSource.Source.Count = ContextManager.ContextTableSources.GetItem(sourceId).Count;
					break;
				}
				if (sourceId > 0 && mvvmSource.Source.Id.Equals(sourceId)) break;
			}
		});
	}

	#endregion

	#region Public and private methods - RelayCommand

	[RelayCommand]
	public async Task OnLoadSourcesFromStorageAsync()
	{
		IsLoad = true;
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		LoadSourcesFromStorage();
		IsLoad = false;
	}

	[RelayCommand]
	public async Task OnLoadSourcesFromTelegramAsync()
	{
		IsLoad = true;
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		if (!CheckClientReady())
		{
			IsLoad = false;
			return;
		}
		ClearSources();
		TgClient.ScanSourceDesktop(TgEnumSourceType.Chat, LoadFromTelegram);
		TgClient.ScanSourceDesktop(TgEnumSourceType.Dialog, LoadFromTelegram);
		//SetSourceLoad();
		IsLoad = false;
	}

	[RelayCommand]
	public async Task OnClearSourcesAsync()
	{
		IsLoad = true;
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		ClearSources();
		IsLoad = false;
	}

	[RelayCommand]
	public async Task OnSaveSourcesAsync()
	{
		IsLoad = true;
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		SaveSourcesIntoStorage();
		IsLoad = false;
	}

	[RelayCommand]
	public async Task OnSortSourcesAsync()
	{
		IsLoad = true;
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		Application.Current.Dispatcher.Invoke(() =>
		{
		SetOrderSources(MvvmSources.Select(x => x.Source).ToList());
		});
		IsLoad = false;
	}

	#endregion
}