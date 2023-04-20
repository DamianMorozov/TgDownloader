// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	private TgMenuDownload SetMenuAdvanced()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgConstants.MenuSwitchNumber}")
				.PageSize(12)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					TgConstants.MenuMainReturn,
					TgConstants.MenuScanChats,
						TgConstants.MenuScanDialogs,
							TgConstants.MenuViewSources,
					TgConstants.MenuDownloadAuto
				));
		return prompt switch
		{
			TgConstants.MenuScanChats => TgMenuDownload.ScanChats,
			TgConstants.MenuScanDialogs => TgMenuDownload.ScanDialogs,
			TgConstants.MenuViewSources => TgMenuDownload.ViewSources,
			TgConstants.MenuDownloadAuto => TgMenuDownload.DownloadAuto,
			_ => TgMenuDownload.Return
		};
	}

	public void SetupAdvanced(TgDownloadSettingsModel tgDownloadSettings)
	{
		TgMenuDownload menu;
		do
		{
			ShowTableAdvanced(tgDownloadSettings);
			menu = SetMenuAdvanced();
			switch (menu)
			{
				case TgMenuDownload.ScanChats:
					ScanSources(tgDownloadSettings, TgSourceType.Chat);
					break;
				case TgMenuDownload.ScanDialogs:
					ScanSources(tgDownloadSettings, TgSourceType.Dialog);
					break;
				case TgMenuDownload.ViewSources:
					ViewSources(tgDownloadSettings);
					break;
				case TgMenuDownload.DownloadAuto:
					RunAction(tgDownloadSettings, AutoDownload, true, false);
					break;
			}
		} while (menu is not TgMenuDownload.Return);
	}

	private void ScanSources(TgDownloadSettingsModel tgDownloadSettings, TgSourceType sourceType)
	{
		ShowTableAdvanced(tgDownloadSettings);
		if (!TgClient.IsReady)
		{
			TgLog.MarkupWarning(TgLocale.TgMustClientConnect);
			Console.ReadKey();
			return;
		}
		bool isSave = AskQuestionReturnPositive(TgConstants.AdvancedSaveSourceInfo, false);

		switch (sourceType)
		{
			case TgSourceType.Chat:
				RunAction(tgDownloadSettings, isSave ? ScanSourcesChatsWithSave : ScanSourcesChatsWithoutSave, true, true);
				break;
			case TgSourceType.Dialog:
		RunAction(tgDownloadSettings, isSave ? ScanSourcesDialogsWithSave : ScanSourcesDialogsWithoutSave, true, true);
				break;
		}
	}

	private void ScanSourcesChatsWithSave(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus) => 
		TgClient.ScanSource(tgDownloadSettings, refreshStatus, TgSourceType.Chat, UpdateSource);

	private void ScanSourcesChatsWithoutSave(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus) => 
		TgClient.ScanSource(tgDownloadSettings, refreshStatus, TgSourceType.Chat, (_, _, _) => { });

	private void ScanSourcesDialogsWithSave(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus) => 
		TgClient.ScanSource(tgDownloadSettings, refreshStatus, TgSourceType.Dialog, UpdateSource);

	private void ScanSourcesDialogsWithoutSave(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus) => 
		TgClient.ScanSource(tgDownloadSettings, refreshStatus, TgSourceType.Dialog, (_, _, _) => { });

	private void ViewSources(TgDownloadSettingsModel tgDownloadSettings)
	{
		ShowTableViewSources(tgDownloadSettings);
		TgSqlTableSourceModel source = GetSourceFromList(TgConstants.MenuViewSources, ContextManager.Sources.GetList(false));
		if (source.IsExists)
		{
			Value = TgMenuMain.Download;
			SetupDownloadSource(tgDownloadSettings, source.Id);
			SetupDownload(tgDownloadSettings);
		}
	}

	private void AutoDownload(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
	{
		List<TgSqlTableSourceModel> sources = ContextManager.Sources.GetList(false);
		foreach (TgSqlTableSourceModel source in sources.Where(sourceSetting => sourceSetting.IsAutoUpdate))
		{
			SetupDownloadSource(tgDownloadSettings, source.Id);
			string sourceId = string.IsNullOrEmpty(source.UserName) ? $"{source.Id}" : $"{source.Id} | @{source.UserName}";
			// StatusContext.
			if (source.Count <= 0)
			{
				refreshStatus($"The source {sourceId} hasn't any messages!", false);
			}
			else
			{
				refreshStatus($"The source {sourceId} has {source.Count} messages.", false);
			}
			// ManualDownload.
			if (source.Count > 0)
			{
				ManualDownload(tgDownloadSettings, refreshStatus);
			}
		}
	}

	#endregion
}