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
				.PageSize(Console.WindowHeight - 17)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					TgLocale.MenuMainReturn,
					TgLocale.MenuAutoDownload,
					TgLocale.MenuAutoViewEvents,
					TgLocale.MenuScanChats,
					TgLocale.MenuScanDialogs,
					TgLocale.MenuMarkAllMessagesAsRead,
					TgLocale.MenuViewVersions, 
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
		if (prompt.Equals(TgLocale.MenuMarkAllMessagesAsRead))
			return TgEnumMenuDownload.MarkHistoryRead;
		if (prompt.Equals(TgLocale.MenuViewVersions))
			return TgEnumMenuDownload.ViewVersions;
		if (prompt.Equals(TgLocale.MenuViewSources))
			return TgEnumMenuDownload.ViewSources;
		return TgEnumMenuDownload.Return;
	}

	public void SetupAdvanced(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		TgEnumMenuDownload menu;
		do
		{
			ShowTableAdvanced(tgDownloadSettings);
			menu = SetMenuAdvanced();
			switch (menu)
			{
				case TgEnumMenuDownload.AutoDownload:
					RunActionStatus(tgDownloadSettings, AutoDownload, true, false);
					break;
				case TgEnumMenuDownload.AutoViewEvents:
					RunActionStatus(tgDownloadSettings, AutoViewEvents, true, false);
					break;
				case TgEnumMenuDownload.ScanChats:
					ScanSources(tgDownloadSettings, TgEnumSourceType.Chat);
					break;
				case TgEnumMenuDownload.ScanDialogs:
					ScanSources(tgDownloadSettings, TgEnumSourceType.Dialog);
					break;
				case TgEnumMenuDownload.MarkHistoryRead:
					MarkHistoryRead(tgDownloadSettings);
					break;
				case TgEnumMenuDownload.ViewSources:
                    ViewSources(tgDownloadSettings);
					break;
				case TgEnumMenuDownload.ViewVersions:
                    ViewVersions(tgDownloadSettings);
					break;
			}
		} while (menu is not TgEnumMenuDownload.Return);
	}

	private void ScanSources(TgDownloadSettingsViewModel tgDownloadSettings, TgEnumSourceType sourceType)
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
				RunActionStatus(tgDownloadSettings, ScanSourcesChatsWithSave, true, true);
				break;
			case TgEnumSourceType.Dialog:
				RunActionStatus(tgDownloadSettings, ScanSourcesDialogsWithSave, true, true);
				break;
		}
	}

	private void ScanSourcesChatsWithSave(TgDownloadSettingsViewModel tgDownloadSettings) =>
        TgClient.ScanSourcesTgConsoleAsync(tgDownloadSettings, TgEnumSourceType.Chat).GetAwaiter().GetResult();

	private void ScanSourcesDialogsWithSave(TgDownloadSettingsViewModel tgDownloadSettings) =>
        TgClient.ScanSourcesTgConsoleAsync(tgDownloadSettings, TgEnumSourceType.Dialog).GetAwaiter().GetResult();

	private void ViewSources(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		ShowTableViewSources(tgDownloadSettings);
		TgEfOperResult<TgEfSourceEntity> operResult = SourceRepository.GetEnumerable(TgEnumTableTopRecords.All, isNoTracking: true);
		TgEfSourceEntity source= GetSourceFromEnumerable(TgLocale.MenuViewSources, operResult.Items);
		if (source.Uid != Guid.Empty)
		{
			Value = TgEnumMenuMain.Download;
            tgDownloadSettings = SetupDownloadSource(source.Id);
			SetupDownload(tgDownloadSettings);
		}
	}

	private void ViewVersions(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		ShowTableViewVersions(tgDownloadSettings);
		GetVersionFromEnumerable(TgLocale.MenuViewSources, 
			VersionRepository.GetEnumerable(TgEnumTableTopRecords.All, isNoTracking: true).Items);
	}

	private void MarkHistoryRead(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		RunActionStatus(tgDownloadSettings, MarkHistoryReadCore, true, false);
	}

	private void AutoDownload(TgDownloadSettingsViewModel _)
	{
		IEnumerable<TgEfSourceEntity> sources = SourceRepository.GetEnumerable(TgEnumTableTopRecords.All, isNoTracking: true).Items;
		foreach (TgEfSourceEntity source in sources.Where(sourceSetting => sourceSetting.IsAutoUpdate))
		{
            TgDownloadSettingsViewModel tgDownloadSettings = SetupDownloadSource(source.Id);
			string sourceId = string.IsNullOrEmpty(source.UserName) ? $"{source.Id}" : $"{source.Id} | @{source.UserName}";
            // StatusContext.
            TgClient.UpdateStateSourceAsync(source.Id, source.FirstId, 
				source.Count <= 0
					? $"The source {sourceId} hasn't any messages!"
					: $"The source {sourceId} has {source.Count} messages.")
	            .GetAwaiter().GetResult();
			// ManualDownload.
			if (source.Count > 0)
				ManualDownload(tgDownloadSettings);
		}
	}

	private void AutoViewEvents(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		TgClient.IsUpdateStatus = true;
		TgClient.UpdateStateSourceAsync(tgDownloadSettings.SourceVm.SourceId, tgDownloadSettings.SourceVm.SourceFirstId, 
			"Auto view updates is started").GetAwaiter().GetResult();
		TgLog.MarkupLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
		TgClient.IsUpdateStatus = false;
	}

	#endregion
}