// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	private TgEnumMenuDownload SetMenuDownload()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgLocale.MenuSwitchNumber}")
				.PageSize(Console.WindowHeight - 17)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					TgLocale.MenuMainReturn,
					TgLocale.MenuDownloadSetSource,
					TgLocale.MenuDownloadSetFolder,
					TgLocale.MenuDownloadSetSourceFirstIdAuto,
					TgLocale.MenuDownloadSetSourceFirstIdManual,
					TgLocale.MenuDownloadSetIsRewriteFiles,
					TgLocale.MenuDownloadSetIsRewriteMessages,
					TgLocale.MenuDownloadSetIsAddMessageId,
					TgLocale.MenuDownloadSetIsAutoUpdate,
					TgLocale.MenuDownloadSetCountThreads,
					TgLocale.MenuSaveSettings,
					TgLocale.MenuManualDownload
				));
		if (prompt.Equals(TgLocale.MenuDownloadSetSource))
			return TgEnumMenuDownload.SetSource;
		if (prompt.Equals(TgLocale.MenuDownloadSetFolder))
			return TgEnumMenuDownload.SetDestDirectory;
		if (prompt.Equals(TgLocale.MenuDownloadSetSourceFirstIdAuto))
			return TgEnumMenuDownload.SetSourceFirstIdAuto;
		if (prompt.Equals(TgLocale.MenuDownloadSetSourceFirstIdManual))
			return TgEnumMenuDownload.SetSourceFirstIdManual;
		if (prompt.Equals(TgLocale.MenuDownloadSetIsRewriteFiles))
			return TgEnumMenuDownload.SetIsRewriteFiles;
		if (prompt.Equals(TgLocale.MenuDownloadSetIsRewriteMessages))
			return TgEnumMenuDownload.SetIsRewriteMessages;
		if (prompt.Equals(TgLocale.MenuDownloadSetIsAddMessageId))
			return TgEnumMenuDownload.SetIsAddMessageId;
		if (prompt.Equals(TgLocale.MenuDownloadSetIsAutoUpdate))
			return TgEnumMenuDownload.SetIsAutoUpdate;
		if (prompt.Equals(TgLocale.MenuDownloadSetCountThreads))
			return TgEnumMenuDownload.SetCountThreads;
		if (prompt.Equals(TgLocale.MenuSaveSettings))
			return TgEnumMenuDownload.SettingsSave;
		if (prompt.Equals(TgLocale.MenuManualDownload))
			return TgEnumMenuDownload.ManualDownload;
		return TgEnumMenuDownload.Return;
	}

	public async Task SetupDownloadAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		TgEnumMenuDownload menu;
		do
		{
			ShowTableDownload(tgDownloadSettings);
			menu = SetMenuDownload();
			switch (menu)
			{
				case TgEnumMenuDownload.SetSource:
					tgDownloadSettings = await SetupDownloadSourceAsync();
					break;
				case TgEnumMenuDownload.SetSourceFirstIdAuto:
					await RunTaskStatusAsync(tgDownloadSettings, SetupDownloadSourceFirstIdAutoAsync, isSkipCheckTgSettings: true, 
						isScanCount: false, isWaitComplete: true);
					break;
				case TgEnumMenuDownload.SetSourceFirstIdManual:
					SetupDownloadSourceFirstIdManual(tgDownloadSettings);
					break;
				case TgEnumMenuDownload.SetDestDirectory:
					SetupDownloadDestDirectory(tgDownloadSettings);
					if (!tgDownloadSettings.SourceVm.IsAutoUpdate)
						SetTgDownloadIsAutoUpdate(tgDownloadSettings);
					break;
				case TgEnumMenuDownload.SetIsRewriteFiles:
					SetTgDownloadIsRewriteFiles(tgDownloadSettings);
					break;
				case TgEnumMenuDownload.SetIsRewriteMessages:
					SetTgDownloadIsRewriteMessages(tgDownloadSettings);
					break;
				case TgEnumMenuDownload.SetIsAddMessageId:
					SetTgDownloadIsJoinFileNameWithMessageId(tgDownloadSettings);
					break;
				case TgEnumMenuDownload.SetIsAutoUpdate:
					SetTgDownloadIsAutoUpdate(tgDownloadSettings);
					break;
				case TgEnumMenuDownload.SetCountThreads:
					SetTgDownloadCountThreads(tgDownloadSettings);
					break;
				case TgEnumMenuDownload.SettingsSave:
					await RunTaskStatusAsync(tgDownloadSettings, UpdateSourceWithSettingsAsync, isSkipCheckTgSettings: true, isScanCount: false, isWaitComplete: false);
					break;
				case TgEnumMenuDownload.ManualDownload:
					await RunTaskProgressAsync(tgDownloadSettings, ManualDownloadAsync, isSkipCheckTgSettings: false, isScanCount: false);
					break;
			}
		} while (menu is not TgEnumMenuDownload.Return);
	}

	private async Task<TgDownloadSettingsViewModel> SetupDownloadSourceAsync(long? id = null)
	{
		TgDownloadSettingsViewModel tgDownloadSettings = SetupDownloadSourceCore(id);
		_ = await TgClient.CreateSmartSourceAsync(tgDownloadSettings, isSilent: true, isReplaceItem: true);
		LoadTgClientSettings(tgDownloadSettings, false, false);
		return tgDownloadSettings;
	}

	private TgDownloadSettingsViewModel SetupDownloadSourceCore(long? id)
	{
		TgDownloadSettingsViewModel tgDownloadSettings = new();
		bool isCheck = false;
		do
		{
			string source = id is { } lid
				? lid.ToString()
				: AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.MenuDownloadSetSource}:"));
			if (!string.IsNullOrEmpty(source))
			{
				if (long.TryParse(source, NumberStyles.Integer, CultureInfo.InvariantCulture, out long sourceId))
				{
					tgDownloadSettings.SourceVm.SourceId = sourceId;
					isCheck = tgDownloadSettings.SourceVm.IsReadySourceId;
				}
				else
				{
					tgDownloadSettings.SourceVm.SourceUserName = source.StartsWith("https://t.me/")
						? source.Replace("https://t.me/", string.Empty)
						: source;
					isCheck = !string.IsNullOrEmpty(tgDownloadSettings.SourceVm.SourceUserName);
				}
			}
		} while (!isCheck);
		return tgDownloadSettings;
	}

	private async Task SetupDownloadSourceFirstIdAutoAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		TgDownloadSmartSource smartSource = await TgClient.CreateSmartSourceAsync(tgDownloadSettings, isSilent: true, isReplaceItem: false);
		if (smartSource.ChatBase is not null)
		{
			await TgClient.SetChannelMessageIdFirstAsync(tgDownloadSettings, smartSource.ChatBase);
			LoadTgClientSettings(tgDownloadSettings, true, false);
		}
	}

	private void SetupDownloadSourceFirstIdManual(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		do
		{
			tgDownloadSettings.SourceVm.SourceFirstId = AnsiConsole.Ask<int>(TgLog.GetLineStampInfo($"{TgLocale.TypeTgSourceFirstId}:"));
		} while (!tgDownloadSettings.SourceVm.IsReadySourceFirstId);
		LoadTgClientSettings(tgDownloadSettings, true, true);
	}

	private void SetupDownloadDestDirectory(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		do
		{
			tgDownloadSettings.SourceVm.SourceDirectory = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.DirectoryDestType}:"));
			if (!Directory.Exists(tgDownloadSettings.SourceVm.SourceDirectory))
			{
				TgLog.MarkupInfo(TgLocale.DirectoryIsNotExists(tgDownloadSettings.SourceVm.SourceDirectory));
				if (AskQuestionReturnPositive(TgLocale.DirectoryCreate, true))
				{
					try
					{
						Directory.CreateDirectory(tgDownloadSettings.SourceVm.SourceDirectory);
					}
					catch (Exception ex)
					{
						TgLog.MarkupWarning(TgLocale.DirectoryCreateIsException(ex));
					}
				}
			}
		} while (!Directory.Exists(tgDownloadSettings.SourceVm.SourceDirectory));
	}

	private void SetTgDownloadIsRewriteFiles(TgDownloadSettingsViewModel tgDownloadSettings) =>
		tgDownloadSettings.IsRewriteFiles = AskQuestionReturnPositive(TgLocale.TgSettingsIsRewriteFiles, true);

	private void SetTgDownloadIsRewriteMessages(TgDownloadSettingsViewModel tgDownloadSettings) =>
		tgDownloadSettings.IsRewriteMessages = AskQuestionReturnPositive(TgLocale.TgSettingsIsRewriteMessages, true);

	private void SetTgDownloadIsJoinFileNameWithMessageId(TgDownloadSettingsViewModel tgDownloadSettings) =>
		tgDownloadSettings.IsJoinFileNameWithMessageId = AskQuestionReturnPositive(TgLocale.TgSettingsIsJoinFileNameWithMessageId, true);

	private void SetTgDownloadIsAutoUpdate(TgDownloadSettingsViewModel tgDownloadSettings) =>
		tgDownloadSettings.SourceVm.IsAutoUpdate = AskQuestionReturnPositive(TgLocale.MenuDownloadSetIsAutoUpdate, true);

	private void SetTgDownloadCountThreads(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		do
		{
			tgDownloadSettings.CountThreads = AnsiConsole.Ask<int>(TgLog.GetLineStampInfo($"{TgLocale.MenuDownloadSetCountThreads}:"));
			if (tgDownloadSettings.CountThreads < 1)
				tgDownloadSettings.CountThreads = 1;
			else if (tgDownloadSettings.CountThreads > 20)
				tgDownloadSettings.CountThreads = 20;
		} while (!tgDownloadSettings.SourceVm.IsReadySourceFirstId);
		LoadTgClientSettings(tgDownloadSettings, true, true);
	}

	private async Task UpdateSourceWithSettingsAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		await tgDownloadSettings.UpdateSourceWithSettingsAsync();
		// Refresh
		await TgClient.UpdateStateSourceAsync(tgDownloadSettings.SourceVm.SourceId, tgDownloadSettings.SourceVm.SourceFirstId, TgLocale.SettingsSource);
	}

	private void LoadTgClientSettings(TgDownloadSettingsViewModel tgDownloadSettings, bool isSkipLoadFirstId, bool isSkipLoadDirectory)
	{
		int sourceFirstId = tgDownloadSettings.SourceVm.SourceFirstId;
		string sourceDirectory = tgDownloadSettings.SourceVm.SourceDirectory;
		TgEfSourceEntity source = SourceRepository.Get(new() { Id = tgDownloadSettings.SourceVm.SourceId }, isNoTracking: false).Item;
		tgDownloadSettings.SourceVm.Item = source;
		if (isSkipLoadFirstId)
			tgDownloadSettings.SourceVm.SourceFirstId = sourceFirstId;
		if (isSkipLoadDirectory)
			tgDownloadSettings.SourceVm.SourceDirectory = sourceDirectory;
	}

	private async Task ManualDownloadAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		ShowTableDownload(tgDownloadSettings);
		await UpdateSourceWithSettingsAsync(tgDownloadSettings);
		await TgClient.DownloadAllDataAsync(tgDownloadSettings);
		await UpdateSourceWithSettingsAsync(tgDownloadSettings);
	}

	private async Task MarkHistoryReadCoreAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		ShowTableMarkHistoryReadProgress(tgDownloadSettings);
		await TgClient.MarkHistoryReadAsync();
		ShowTableMarkHistoryReadComplete(tgDownloadSettings);
	}

	#endregion
}