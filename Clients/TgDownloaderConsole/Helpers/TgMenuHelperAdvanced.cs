// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	private TgEnumMenuDownload SetMenuAdvanced()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgLocale.MenuSwitchNumber}")
				.PageSize(12)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					TgLocale.MenuMainReturn,
					TgLocale.MenuAutoDownload,
					TgLocale.MenuAutoViewEvents,
					TgLocale.MenuScanChats,
						TgLocale.MenuScanDialogs,
							TgLocale.MenuViewSources
				));
		if (prompt.Equals(TgLocale.MenuAutoDownload))
			return TgEnumMenuDownload.AutoDownload;
		if (prompt.Equals(TgLocale.MenuAutoViewEvents))
			return TgEnumMenuDownload.AutoViewEvents;
		if (prompt.Equals(TgLocale.MenuScanChats))
			return TgEnumMenuDownload.ScanChats;
		if (prompt.Equals(TgLocale.MenuScanDialogs))
			return TgEnumMenuDownload.ScanDialogs;
		if (prompt.Equals(TgLocale.MenuViewSources))
			return TgEnumMenuDownload.ViewSources;
		return TgEnumMenuDownload.Return;
	}

	public void SetupAdvanced(TgDownloadSettingsModel tgDownloadSettings)
	{
		TgEnumMenuDownload menu;
		do
		{
			ShowTableAdvanced(tgDownloadSettings);
			menu = SetMenuAdvanced();
			switch (menu)
			{
				case TgEnumMenuDownload.AutoDownload:
					RunAction(tgDownloadSettings, AutoDownload, true, false);
					break;
				case TgEnumMenuDownload.AutoViewEvents:
					RunAction(tgDownloadSettings, AutoViewEvents, true, false);
					break;
				case TgEnumMenuDownload.ScanChats:
					ScanSources(tgDownloadSettings, TgEnumSourceType.Chat);
					break;
				case TgEnumMenuDownload.ScanDialogs:
					ScanSources(tgDownloadSettings, TgEnumSourceType.Dialog);
					break;
				case TgEnumMenuDownload.ViewSources:
					ViewSources(tgDownloadSettings);
					break;
			}
		} while (menu is not TgEnumMenuDownload.Return);
	}

	private void ScanSources(TgDownloadSettingsModel tgDownloadSettings, TgEnumSourceType sourceType)
	{
		ShowTableAdvanced(tgDownloadSettings);
		if (!TgClient.IsReady)
		{
			TgLog.MarkupWarning(TgLocale.TgMustClientConnect);
			Console.ReadKey();
			return;
		}

		switch (sourceType)
		{
			case TgEnumSourceType.Chat:
				RunAction(tgDownloadSettings, ScanSourcesChatsWithSave, true, true);
				break;
			case TgEnumSourceType.Dialog:
				RunAction(tgDownloadSettings, ScanSourcesDialogsWithSave, true, true);
				break;
		}
	}

	private void ScanSourcesChatsWithSave(TgDownloadSettingsModel tgDownloadSettings) =>
		TgClient.ScanSourcesTgConsole(tgDownloadSettings, TgEnumSourceType.Chat);

	private void ScanSourcesChatsWithoutSave(TgDownloadSettingsModel tgDownloadSettings) =>
		TgClient.ScanSourcesTgConsole(tgDownloadSettings, TgEnumSourceType.Chat);

	private void ScanSourcesDialogsWithSave(TgDownloadSettingsModel tgDownloadSettings) =>
		TgClient.ScanSourcesTgConsole(tgDownloadSettings, TgEnumSourceType.Dialog);

	private void ScanSourcesDialogsWithoutSave(TgDownloadSettingsModel tgDownloadSettings) =>
		TgClient.ScanSourcesTgConsole(tgDownloadSettings, TgEnumSourceType.Dialog);

	private void ViewSources(TgDownloadSettingsModel tgDownloadSettings)
	{
		ShowTableViewSources(tgDownloadSettings);
		TgSqlTableSourceModel source = GetSourceFromEnumerable(TgLocale.MenuViewSources,
			ContextManager.SourceRepository.GetEnumerable());
		if (source.IsExists)
		{
			Value = TgEnumMenuMain.Download;
            tgDownloadSettings = SetupDownloadSource(source.Id);
			SetupDownload(tgDownloadSettings);
		}
	}

	private void AutoDownload(TgDownloadSettingsModel tgDownloadSettings)
	{
		IEnumerable<TgSqlTableSourceModel> sources = ContextManager.SourceRepository.GetEnumerable();
		foreach (TgSqlTableSourceModel source in sources.Where(sourceSetting => sourceSetting.IsAutoUpdate))
		{
            tgDownloadSettings = SetupDownloadSource(source.Id);
			string sourceId = string.IsNullOrEmpty(source.UserName) ? $"{source.Id}" : $"{source.Id} | @{source.UserName}";
			// StatusContext.
			TgClient.UpdateStateClient(
				source.Count <= 0
					? $"The source {sourceId} hasn't any messages!"
					: $"The source {sourceId} has {source.Count} messages.");
			// ManualDownload.
			if (source.Count > 0) 
                ManualDownload(tgDownloadSettings);
		}
	}

	private void AutoViewEvents(TgDownloadSettingsModel tgDownloadSettings)
	{
		TgClient.IsUpdateStatus = true;
		TgClient.UpdateStateClient("Auto view updates is started");
		TgLog.MarkupLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
		TgClient.IsUpdateStatus = false;
	}

	#endregion
}