// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderConsole.Helpers;

internal partial class MenuHelper
{
    #region Public and private methods

    private MenuDownload SetMenuAdvanced()
    {
        string prompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(TgLocale.MenuSwitchNumber)
                .PageSize(12)
                .MoreChoicesText(TgLocale.MoveUpDown)
                .AddChoices(
                    TgLocale.MenuMainReturn,
                    //TgLocale.MenuScanSources,
                    TgLocale.MenuDownloadAuto
                    //TgLocale.MenuViewSources
                ));
        return prompt switch
        {
            "Scan local sources" => MenuDownload.ScanSources,
            "Auto download" => MenuDownload.DownloadAuto,
            "View local sources" => MenuDownload.ViewSources,
            _ => MenuDownload.Return
        };
    }

    public void SetupAdvanced(TgDownloadSettingsModel tgDownloadSettings)
    {
        MenuDownload menu;
        do
        {
            ShowTableAdvanced(tgDownloadSettings);
            menu = SetMenuAdvanced();
            switch (menu)
            {
                case MenuDownload.ScanSources:
                    RunAction(tgDownloadSettings, ScanSources, true);
                    break;
                case MenuDownload.DownloadAuto:
                    RunAction(tgDownloadSettings, AutoDownload, true);
                    break;
                case MenuDownload.ViewSources:
                    RunAction(tgDownloadSettings, ViewSources, true);
                    break;
            }
        } while (menu is not MenuDownload.Return);
    }

    private void ScanSources(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
    {
        ShowTableScanSources(tgDownloadSettings);
        TgClient.ScanLocalSources(refreshStatus, UpdateSource);
    }

    private void ViewSources(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
    {
        ShowTableViewSources(tgDownloadSettings);
        List<SqlTableSourceSettingModel> sourceSettings = TgStorage.GetList<SqlTableSourceSettingModel>();
        foreach (SqlTableSourceSettingModel sourceSetting in sourceSettings.Where(sourceSetting =>
                     sourceSetting.IsAutoUpdate))
        {
            //TgClient.ScanLocalSources(refreshStatus, UpdateSource);
        }
    }

    private void AutoDownload(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
    {
        List<SqlTableSourceSettingModel> sourceSettings = TgStorage.GetList<SqlTableSourceSettingModel>();
        foreach (SqlTableSourceSettingModel sourceSetting in sourceSettings.Where(sourceSetting => sourceSetting.IsAutoUpdate))
        {
            SetupDownloadSource(tgDownloadSettings, sourceSetting.SourceId);

            SqlTableSourceModel source = TgStorage.GetItemDeprecated<SqlTableSourceModel>(sourceSetting.SourceId);
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