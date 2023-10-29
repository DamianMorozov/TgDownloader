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
		if (prompt.Equals(TgLocale.MenuSaveSettings))
			return TgEnumMenuDownload.SettingsSave;
		if (prompt.Equals(TgLocale.MenuManualDownload))
			return TgEnumMenuDownload.ManualDownload;
		return TgEnumMenuDownload.Return;
	}

	public void SetupDownload(TgDownloadSettingsModel tgDownloadSettings)
	{
		TgEnumMenuDownload menu;
		do
		{
			ShowTableDownload(tgDownloadSettings);
			menu = SetMenuDownload();
			switch (menu)
			{
				case TgEnumMenuDownload.SetSource:
                    tgDownloadSettings = SetupDownloadSource();
					break;
				case TgEnumMenuDownload.SetSourceFirstIdAuto:
					RunAction(tgDownloadSettings, SetupDownloadSourceFirstIdAuto, true, false);
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
				case TgEnumMenuDownload.SettingsSave:
					RunAction(tgDownloadSettings, UpdateSourceWithSettings, true, false);
					break;
				case TgEnumMenuDownload.ManualDownload:
					RunAction(tgDownloadSettings, ManualDownload, false, false);
					break;
			}
		} while (menu is not TgEnumMenuDownload.Return);
	}

	private TgDownloadSettingsModel SetupDownloadSource(long? id = null)
	{
        TgDownloadSettingsModel tgDownloadSettings = TgDownloadSettingsModel.CreateNew();
		//tgDownloadSettings.SourceVm.SourceFirstId = 1;
		SetupDownloadSourceCore(id, tgDownloadSettings);
        Channel? channel = TgClient.PrepareChannelDownloadMessages(tgDownloadSettings, true);
		if (channel is null)
			_ = TgClient.PrepareChatBaseDownloadMessages(tgDownloadSettings, true);
		LoadTgClientSettings(tgDownloadSettings, false, false);
        return tgDownloadSettings;
    }

    private void SetupDownloadSourceCore(long? id, TgDownloadSettingsModel tgDownloadSettings)
    {
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
    }

    private void SetupDownloadSourceFirstIdAuto(TgDownloadSettingsModel tgDownloadSettings)
	{
		Channel? channel = TgClient.PrepareChannelDownloadMessages(tgDownloadSettings, true);
		if (channel is not null)
		{
			TgClient.SetChannelMessageIdFirstWithLock(tgDownloadSettings, channel);
			LoadTgClientSettings(tgDownloadSettings, true, false);
			return;
		}

		ChatBase? chatBase = TgClient.PrepareChatBaseDownloadMessages(tgDownloadSettings, true);
		if (chatBase is not null)
		{
			TgClient.SetChannelMessageIdFirstWithLock(tgDownloadSettings, chatBase);
			LoadTgClientSettings(tgDownloadSettings, true, false);
		}
	}

	private void SetupDownloadSourceFirstIdManual(TgDownloadSettingsModel tgDownloadSettings)
	{
		do
		{
			tgDownloadSettings.SourceVm.SourceFirstId = AnsiConsole.Ask<int>(TgLog.GetLineStampInfo($"{TgLocale.TypeTgSourceFirstId}:"));
		} while (!tgDownloadSettings.SourceVm.IsReadySourceFirstId);
		LoadTgClientSettings(tgDownloadSettings, true, true);
	}

	private void SetupDownloadDestDirectory(TgDownloadSettingsModel tgDownloadSettings)
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

	private void SetTgDownloadIsRewriteFiles(TgDownloadSettingsModel tgDownloadSettings) =>
		tgDownloadSettings.IsRewriteFiles = AskQuestionReturnPositive(TgLocale.TgSettingsIsRewriteFiles, true);

	private void SetTgDownloadIsRewriteMessages(TgDownloadSettingsModel tgDownloadSettings) =>
		tgDownloadSettings.IsRewriteMessages = AskQuestionReturnPositive(TgLocale.TgSettingsIsRewriteMessages, true);

	private void SetTgDownloadIsJoinFileNameWithMessageId(TgDownloadSettingsModel tgDownloadSettings) =>
		tgDownloadSettings.IsJoinFileNameWithMessageId = AskQuestionReturnPositive(TgLocale.TgSettingsIsJoinFileNameWithMessageId, true);

	private void SetTgDownloadIsAutoUpdate(TgDownloadSettingsModel tgDownloadSettings) =>
		tgDownloadSettings.SourceVm.IsAutoUpdate = AskQuestionReturnPositive(TgLocale.MenuDownloadSetIsAutoUpdate, true);

    private void UpdateSourceWithSettings(TgDownloadSettingsModel tgDownloadSettings)
    {
        tgDownloadSettings.UpdateSourceWithSettings();
		// Refresh.
		TgClient.UpdateStateMessage(TgLocale.SettingsSource);
	}

	private void LoadTgClientSettings(TgDownloadSettingsModel tgDownloadSettings, bool isSkipLoadFirstId, bool isSkipLoadDirectory)
    {
        int sourceFirstId = tgDownloadSettings.SourceVm.SourceFirstId;
        string sourceDirectory = tgDownloadSettings.SourceVm.SourceDirectory;
        TgSqlTableSourceModel source = ContextManager.SourceRepository.Get(tgDownloadSettings.SourceVm.SourceId);
        tgDownloadSettings.SourceVm.Source = source;
		if (isSkipLoadFirstId)
			tgDownloadSettings.SourceVm.SourceFirstId = sourceFirstId;
		if (isSkipLoadDirectory)
			tgDownloadSettings.SourceVm.SourceDirectory = sourceDirectory;
	}

	private void ManualDownload(TgDownloadSettingsModel tgDownloadSettings)
	{
		ShowTableDownload(tgDownloadSettings);
		TgClient.DownloadAllData(tgDownloadSettings);
        UpdateSourceWithSettings(tgDownloadSettings);
	}

	#endregion
}