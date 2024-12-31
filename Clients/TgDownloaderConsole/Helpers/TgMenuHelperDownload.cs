// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

using TgInfrastructure.Enums;

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
			await ShowTableDownloadAsync(tgDownloadSettings);
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
					await SetupDownloadSourceFirstIdManualAsync(tgDownloadSettings);
					break;
				case TgEnumMenuDownload.SetDestDirectory:
					SetupDownloadDestDirectory(tgDownloadSettings);
					if (!tgDownloadSettings.SourceVm.Dto.IsAutoUpdate)
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
					await SetTgDownloadCountThreadsAsync(tgDownloadSettings);
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
		var tgDownloadSettings = SetupDownloadSourceCore(id);
		await LoadTgClientSettingsAsync(tgDownloadSettings);
		await TgClient.CreateChatAsync(tgDownloadSettings, isSilent: true);
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
					tgDownloadSettings.SourceVm.Dto.Id = sourceId;
					isCheck = tgDownloadSettings.SourceVm.Dto.IsReady;
				}
				else
				{
					tgDownloadSettings.SourceVm.Dto.UserName = source.StartsWith("https://t.me/")
						? source.Replace("https://t.me/", string.Empty)
						: source;
					isCheck = !string.IsNullOrEmpty(tgDownloadSettings.SourceVm.Dto.UserName);
				}
			}
		} while (!isCheck);
		return tgDownloadSettings;
	}

	private async Task SetupDownloadSourceFirstIdAutoAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		await LoadTgClientSettingsAsync(tgDownloadSettings);
		await TgClient.CreateChatAsync(tgDownloadSettings, isSilent: true);
		await TgClient.SetChannelMessageIdFirstAsync(tgDownloadSettings);
	}

	private async Task SetupDownloadSourceFirstIdManualAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		await LoadTgClientSettingsAsync(tgDownloadSettings);
		do
		{
			tgDownloadSettings.SourceVm.Dto.FirstId = AnsiConsole.Ask<int>(TgLog.GetLineStampInfo($"{TgLocale.TypeTgSourceFirstId}:"));
		} while (!tgDownloadSettings.SourceVm.Dto.IsReadySourceFirstId);
	}

	private void SetupDownloadDestDirectory(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		do
		{
			tgDownloadSettings.SourceVm.Dto.Directory = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.DirectoryDestType}:"));
			if (!Directory.Exists(tgDownloadSettings.SourceVm.Dto.Directory))
			{
				TgLog.MarkupInfo(TgLocale.DirectoryIsNotExists(tgDownloadSettings.SourceVm.Dto.Directory));
				if (AskQuestionReturnPositive(TgLocale.DirectoryCreate, true))
				{
					try
					{
						Directory.CreateDirectory(tgDownloadSettings.SourceVm.Dto.Directory);
					}
					catch (Exception ex)
					{
						TgLog.MarkupWarning(TgLocale.DirectoryCreateIsException(ex));
					}
				}
			}
		} while (!Directory.Exists(tgDownloadSettings.SourceVm.Dto.Directory));
	}

	private void SetTgDownloadIsRewriteFiles(TgDownloadSettingsViewModel tgDownloadSettings) =>
		tgDownloadSettings.IsRewriteFiles = AskQuestionReturnPositive(TgLocale.TgSettingsIsRewriteFiles, true);

	private void SetTgDownloadIsRewriteMessages(TgDownloadSettingsViewModel tgDownloadSettings) =>
		tgDownloadSettings.IsRewriteMessages = AskQuestionReturnPositive(TgLocale.TgSettingsIsRewriteMessages, true);

	private void SetTgDownloadIsJoinFileNameWithMessageId(TgDownloadSettingsViewModel tgDownloadSettings) =>
		tgDownloadSettings.IsJoinFileNameWithMessageId = AskQuestionReturnPositive(TgLocale.TgSettingsIsJoinFileNameWithMessageId, true);

	private void SetTgDownloadIsAutoUpdate(TgDownloadSettingsViewModel tgDownloadSettings) =>
		tgDownloadSettings.SourceVm.Dto.IsAutoUpdate = AskQuestionReturnPositive(TgLocale.MenuDownloadSetIsAutoUpdate, true);

	private async Task SetTgDownloadCountThreadsAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		await LoadTgClientSettingsAsync(tgDownloadSettings);
		tgDownloadSettings.CountThreads = AnsiConsole.Ask<int>(TgLog.GetLineStampInfo($"{TgLocale.MenuDownloadSetCountThreads}:"));
		if (tgDownloadSettings.CountThreads < 1)
			tgDownloadSettings.CountThreads = 1;
		else if (tgDownloadSettings.CountThreads > 20)
			tgDownloadSettings.CountThreads = 20;
	}

	private async Task UpdateSourceWithSettingsAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		await tgDownloadSettings.UpdateSourceWithSettingsAsync();
		await TgClient.UpdateStateSourceAsync(tgDownloadSettings.SourceVm.Dto.Id, tgDownloadSettings.SourceVm.Dto.FirstId, TgLocale.SettingsSource);
	}

	private async Task LoadTgClientSettingsAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		var source = await SourceRepository.GetItemAsync(new() { Id = tgDownloadSettings.SourceVm.Dto.Id });
		tgDownloadSettings.SourceVm.Dto = new TgEfSourceDto().Fill(source, isUidCopy: true);
	}

	private async Task ManualDownloadAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		await ShowTableDownloadAsync(tgDownloadSettings);
		await UpdateSourceWithSettingsAsync(tgDownloadSettings);
		await TgClient.DownloadAllDataAsync(tgDownloadSettings);
		await UpdateSourceWithSettingsAsync(tgDownloadSettings);
	}

	private async Task MarkHistoryReadCoreAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		await ShowTableMarkHistoryReadProgressAsync(tgDownloadSettings);
		await TgClient.MarkHistoryReadAsync();
		await ShowTableMarkHistoryReadCompleteAsync(tgDownloadSettings);
	}

	#endregion
}