// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	private TgMenuDownload SetMenuDownload()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgConstants.MenuSwitchNumber}")
				.PageSize(12)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					TgConstants.MenuMainReturn,
					TgConstants.MenuDownloadSetSource,
					TgConstants.MenuDownloadSetFolder,
					TgConstants.MenuDownloadSetSourceFirstIdAuto,
					TgConstants.MenuDownloadSetSourceFirstIdManual,
					TgConstants.MenuDownloadSetIsRewriteFiles,
					TgConstants.MenuDownloadSetIsRewriteMessages,
					TgConstants.MenuDownloadSetIsAddMessageId,
					TgConstants.MenuDownloadSetIsAutoUpdate,
					TgConstants.MenuSaveSettings,
					TgConstants.MenuDownloadManual
				));
		return prompt switch
		{
			TgConstants.MenuDownloadSetSource => TgMenuDownload.SetSource,
			TgConstants.MenuDownloadSetFolder => TgMenuDownload.SetDestDirectory,
			TgConstants.MenuDownloadSetSourceFirstIdAuto => TgMenuDownload.SetSourceFirstIdAuto,
			TgConstants.MenuDownloadSetSourceFirstIdManual => TgMenuDownload.SetSourceFirstIdManual,
			TgConstants.MenuDownloadSetIsRewriteFiles => TgMenuDownload.SetIsRewriteFiles,
			TgConstants.MenuDownloadSetIsRewriteMessages => TgMenuDownload.SetIsRewriteMessages,
			TgConstants.MenuDownloadSetIsAddMessageId => TgMenuDownload.SetIsAddMessageId,
			TgConstants.MenuDownloadSetIsAutoUpdate => TgMenuDownload.SetIsAutoUpdate,
			TgConstants.MenuSaveSettings => TgMenuDownload.SettingsSave,
			TgConstants.MenuDownloadManual => TgMenuDownload.DownloadManual,
			_ => TgMenuDownload.Return
		};
	}

	public void SetupDownload(TgDownloadSettingsModel tgDownloadSettings)
	{
		TgMenuDownload menu;
		do
		{
			ShowTableDownload(tgDownloadSettings);
			menu = SetMenuDownload();
			switch (menu)
			{
				case TgMenuDownload.SetSource:
					SetupDownloadSource(tgDownloadSettings);
					break;
				case TgMenuDownload.SetSourceFirstIdAuto:
					RunAction(tgDownloadSettings, SetupDownloadSourceFirstIdAuto, true, false);
					break;
				case TgMenuDownload.SetSourceFirstIdManual:
					SetupDownloadSourceFirstIdManual(tgDownloadSettings);
					break;
				case TgMenuDownload.SetDestDirectory:
					SetupDownloadDestDirectory(tgDownloadSettings);
					break;
				case TgMenuDownload.SetIsRewriteFiles:
					SetTgDownloadIsRewriteFiles(tgDownloadSettings);
					break;
				case TgMenuDownload.SetIsRewriteMessages:
					SetTgDownloadIsRewriteMessages(tgDownloadSettings);
					break;
				case TgMenuDownload.SetIsAddMessageId:
					SetTgDownloadIsJoinFileNameWithMessageId(tgDownloadSettings);
					break;
				case TgMenuDownload.SetIsAutoUpdate:
					SetTgDownloadIsAutoUpdate(tgDownloadSettings);
					break;
				case TgMenuDownload.SettingsSave:
					RunAction(tgDownloadSettings, SaveSettings, true, false);
					break;
				case TgMenuDownload.DownloadManual:
					RunAction(tgDownloadSettings, ManualDownload, false, false);
					break;
			}
		} while (menu is not TgMenuDownload.Return);
	}

	private void SetupDownloadSource(TgDownloadSettingsModel tgDownloadSettings, long? id = null)
	{
		tgDownloadSettings.Reset();
		tgDownloadSettings.SourceFirstId = 1;
		bool isCheck = false;
		do
		{
			string source = id is { } lid ? lid.ToString() : AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgConstants.MenuDownloadSetSource}:"));
			if (!string.IsNullOrEmpty(source))
			{
				if (long.TryParse(source, NumberStyles.Integer, CultureInfo.InvariantCulture, out long sourceId))
				{
					tgDownloadSettings.SourceId = sourceId;
					isCheck = tgDownloadSettings.IsReadySourceId;
				}
				else
				{
					tgDownloadSettings.SourceUserName = source.StartsWith(@"https://t.me/")
						? source.Replace("https://t.me/", string.Empty)
						: source;
					isCheck = !string.IsNullOrEmpty(tgDownloadSettings.SourceUserName);
				}
			}
		} while (!isCheck);
		Channel? channel = TgClient.PrepareChannelDownloadMessages(tgDownloadSettings, true);
		if (channel is null)
			_ = TgClient.PrepareChatBaseDownloadMessages(tgDownloadSettings, true);
		LoadTgClientSettings(tgDownloadSettings, false, false);
	}

	private void SetupDownloadSourceFirstIdAuto(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
	{
		Channel? channel = TgClient.PrepareChannelDownloadMessages(tgDownloadSettings, true);
		if (channel is not null)
		{
			TgClient.SetChannelMessageIdFirst(tgDownloadSettings, channel, refreshStatus);
			LoadTgClientSettings(tgDownloadSettings, true, false);
			return;
		}

		ChatBase? chatBase = TgClient.PrepareChatBaseDownloadMessages(tgDownloadSettings, true);
		if (chatBase is not null)
		{
			TgClient.SetChannelMessageIdFirst(tgDownloadSettings, chatBase, refreshStatus);
			LoadTgClientSettings(tgDownloadSettings, true, false);
		}
	}

	private void SetupDownloadSourceFirstIdManual(TgDownloadSettingsModel tgDownloadSettings)
	{
		do
		{
			tgDownloadSettings.SourceFirstId = AnsiConsole.Ask<int>(TgLog.GetLineStampInfo($"{TgLocale.TypeTgSourceFirstId}:"));
		} while (!tgDownloadSettings.IsReadySourceFirstId);
		LoadTgClientSettings(tgDownloadSettings, true, true);
	}

	private void SetupDownloadDestDirectory(TgDownloadSettingsModel tgDownloadSettings)
	{
		do
		{
			tgDownloadSettings.DestDirectory = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.DirectoryDestType}:"));
			if (!Directory.Exists(tgDownloadSettings.DestDirectory))
			{
				TgLog.MarkupInfo(TgLocale.DirectoryIsNotExists(tgDownloadSettings.DestDirectory));
				if (AskQuestionReturnPositive(TgLocale.DirectoryCreate, true))
				{
					try
					{
						Directory.CreateDirectory(tgDownloadSettings.DestDirectory);
					}
					catch (Exception ex)
					{
						TgLog.MarkupWarning(TgLocale.DirectoryCreateIsException(ex));
					}
				}
			}
		} while (!Directory.Exists(tgDownloadSettings.DestDirectory));
	}

	private void SetTgDownloadIsRewriteFiles(TgDownloadSettingsModel tgDownloadSettings)
	{
		tgDownloadSettings.IsRewriteFiles = AskQuestionReturnPositive(TgLocale.TgSettingsIsRewriteFiles, true);
	}

	private void SetTgDownloadIsRewriteMessages(TgDownloadSettingsModel tgDownloadSettings)
	{
		tgDownloadSettings.IsRewriteMessages = AskQuestionReturnPositive(TgLocale.TgSettingsIsRewriteMessages, true); ;
	}

	private void SetTgDownloadIsJoinFileNameWithMessageId(TgDownloadSettingsModel tgDownloadSettings)
	{
		tgDownloadSettings.IsJoinFileNameWithMessageId = AskQuestionReturnPositive(TgLocale.TgSettingsIsJoinFileNameWithMessageId, true); ; ;
	}

	private void SetTgDownloadIsAutoUpdate(TgDownloadSettingsModel tgDownloadSettings)
	{
		tgDownloadSettings.IsAutoUpdate = AskQuestionReturnPositive(TgConstants.MenuDownloadSetIsAutoUpdate, true); ; ; ;
	}

	private void UpdateSourceWithSettings(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
	{
		if (!tgDownloadSettings.IsReady) return;
		// Update source.
		ContextManager.Sources.AddOrUpdateItem(new() { Id = tgDownloadSettings.SourceId, UserName = tgDownloadSettings.SourceUserName,
Title = tgDownloadSettings.SourceTitle, About = tgDownloadSettings.SourceAbout, Count = tgDownloadSettings.SourceLastId,
Directory = tgDownloadSettings.DestDirectory, FirstId = tgDownloadSettings.SourceFirstId, IsAutoUpdate = tgDownloadSettings.IsAutoUpdate });
		// Refresh.
		refreshStatus(TgLocale.SettingsSource, false);
	}

	private void LoadTgClientSettings(TgDownloadSettingsModel tgDownloadSettings, bool isSkipFirstId, bool isSkipDestDirectory)
	{
		TgSqlTableSourceModel source = ContextManager.Sources.GetItem(tgDownloadSettings.SourceId);
		if (!isSkipFirstId)
			tgDownloadSettings.SourceFirstId = source.FirstId;
		if (!isSkipDestDirectory)
			tgDownloadSettings.DestDirectory = source.Directory;
		tgDownloadSettings.IsAutoUpdate = source.IsAutoUpdate;
	}

	private void UpdateSource(ChatBase chat, string about, int count)
	{
		if (chat is Channel channel)
			ContextManager.Sources.AddOrUpdateItem(new() { Id = channel.id, UserName = channel.username, 
				Title = channel.title, About = about, Count = count });
	}

	private void ManualDownload(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
	{
		UpdateSourceWithSettings(tgDownloadSettings, refreshStatus);
		ShowTableDownload(tgDownloadSettings);
		TgClient.DownloadAllData(tgDownloadSettings, refreshStatus, StoreMessage, StoreDocument, FindExistsMessage);
		// Update last id.
		UpdateSourceWithSettings(tgDownloadSettings, refreshStatus);
	}

	private void SaveSettings(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
	{
		UpdateSourceWithSettings(tgDownloadSettings, refreshStatus);
	}

	#endregion
}