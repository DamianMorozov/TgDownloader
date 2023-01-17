// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Globalization;
using TgDownloaderCore.Models;
using TgLocalization.Enums;
using TgStorageCore.Models.Sources;
using TgStorageCore.Models.SourcesSettings;

namespace TgDownloaderConsole.Helpers;

internal partial class MenuHelper
{
    #region Public and private methods

    private MenuDownload SetMenuDownload()
    {
        string userChoose = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(TgLocale.MenuSwitchNumber)
                .PageSize(12)
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
                    TgLocale.MenuIsAutoUpdate,
                    //TgLocale.MenuScan
                    TgLocale.MenuSaveSettings,
                    TgLocale.MenuDownloadAuto,
                    TgLocale.MenuDownloadManual
                ));
        return userChoose switch
        {
            "Setup source (ID/username)" => MenuDownload.SetSource,
            "Setup download folder" => MenuDownload.SetDestDirectory,
            "Setup first ID auto" => MenuDownload.SetSourceFirstIdAuto,
            "Setup first ID manual" => MenuDownload.SetSourceFirstIdManual,
            "Enable rewrite exists files" => MenuDownload.SetIsRewriteFiles,
            "Enable rewrite exists messages" => MenuDownload.SetIsRewriteMessages,
            "Enable join message ID with file name" => MenuDownload.SetIsAddMessageId,
            "Enable auto update" => MenuDownload.SetIsAutoUpdate,
            "Scan my sources" => MenuDownload.Scan,
            "Save settings" => MenuDownload.SettingsSave,
            "Auto download" => MenuDownload.DownloadAuto,
            "Manual download" => MenuDownload.DownloadManual,
            _ => MenuDownload.Return
        };
    }

    public void SetupDownload(TgDownloadSettingsModel tgDownloadSettings)
    {
        MenuDownload menu;
        do
        {
            ShowTableDownload(tgDownloadSettings);
            menu = SetMenuDownload();
            switch (menu)
            {
                case MenuDownload.SetSource:
                    SetupDownloadSource(tgDownloadSettings);
                    break;
                case MenuDownload.SetSourceFirstIdAuto:
                    RunAction(tgDownloadSettings, SetupDownloadSourceFirstIdAuto, true);
                    break;
                case MenuDownload.SetSourceFirstIdManual:
                    SetupDownloadSourceFirstIdManual(tgDownloadSettings);
                    break;
                case MenuDownload.SetDestDirectory:
                    SetupDownloadDestDirectory(tgDownloadSettings);
                    break;
                case MenuDownload.SetIsRewriteFiles:
                    SetTgDownloadIsRewriteFiles(tgDownloadSettings);
                    break;
                case MenuDownload.SetIsRewriteMessages:
                    SetTgDownloadIsRewriteMessages(tgDownloadSettings);
                    break;
                case MenuDownload.SetIsAddMessageId:
                    SetTgDownloadIsJoinFileNameWithMessageId(tgDownloadSettings);
                    break;
                case MenuDownload.SetIsAutoUpdate:
                    SetTgDownloadIsAutoUpdate(tgDownloadSettings);
                    break;
                case MenuDownload.Scan:
                    RunAction(tgDownloadSettings, Scan, true);
                    break;
                case MenuDownload.SettingsSave:
                    RunAction(tgDownloadSettings, SaveSettings, true);
                    break;
                case MenuDownload.DownloadAuto:
                    RunAction(tgDownloadSettings, AutoDownload, true);
                    break;
                case MenuDownload.DownloadManual:
                    RunAction(tgDownloadSettings, ManualDownload, false);
                    break;
                case MenuDownload.Return:
                default:
                    break;
            }
        } while (menu is not MenuDownload.Return);
    }

    private void SetupDownloadSource(TgDownloadSettingsModel tgDownloadSettings, long? sId = null)
    {
        tgDownloadSettings.SetDefault(1);
        bool isCheck = false;
        do
        {
            string source = sId is { } lId ? lId.ToString() : AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.MenuDownloadSetSource}:"));
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
        TgClient.PrepareDownloadMessages(tgDownloadSettings, true);
        LoadTgClientSettings(tgDownloadSettings, false, false);
    }

    private void SetupDownloadSourceFirstIdAuto(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
    {
            Channel? channel = TgClient.PrepareDownloadMessages(tgDownloadSettings, true);
            if (channel is null) return;
            TgClient.SetChannelMessageIdFirst(tgDownloadSettings, channel, refreshStatus);
        LoadTgClientSettings(tgDownloadSettings, true, false);
    }

    private void SetupDownloadSourceFirstIdManual(TgDownloadSettingsModel tgDownloadSettings)
    {
        do
        {
            tgDownloadSettings.SourceFirstId = AnsiConsole.Ask<int>(TgLog.GetLineStampInfo($"{TgLocale.TypeTgSourceFirstId}:"));
        } while (!tgDownloadSettings.IsReadySourceFirstId);
        LoadTgClientSettings(tgDownloadSettings, true, false);
    }

    private void SetupDownloadDestDirectory(TgDownloadSettingsModel tgDownloadSettings)
    {
        do
        {
            tgDownloadSettings.DestDirectory = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TypeDestDirectory}:"));
            if (!Directory.Exists(tgDownloadSettings.DestDirectory))
                TgLog.Info(TgLocale.DirIsNotExistsSpecify(tgDownloadSettings.DestDirectory));
        } while (!Directory.Exists(tgDownloadSettings.DestDirectory));
    }

    private void SetTgDownloadIsRewriteFiles(TgDownloadSettingsModel tgDownloadSettings)
    {
        bool isResult = AnsiConsole.Prompt(new SelectionPrompt<bool>()
            .Title(TgLocale.TgSettingsIsRewriteFiles)
            .PageSize(10)
            .AddChoices(true, false));
        tgDownloadSettings.IsRewriteFiles = isResult;
    }

    private void SetTgDownloadIsRewriteMessages(TgDownloadSettingsModel tgDownloadSettings)
    {
        bool isResult = AnsiConsole.Prompt(new SelectionPrompt<bool>()
            .Title(TgLocale.TgSettingsIsRewriteMessages)
            .PageSize(10)
            .AddChoices(true, false));
        tgDownloadSettings.IsRewriteMessages = isResult;
    }

    private void SetTgDownloadIsJoinFileNameWithMessageId(TgDownloadSettingsModel tgDownloadSettings)
    {
        bool isResult = AnsiConsole.Prompt(new SelectionPrompt<bool>()
            .Title(TgLocale.TgSettingsIsJoinFileNameWithMessageId)
            .PageSize(10)
            .AddChoices(true, false));
        tgDownloadSettings.IsJoinFileNameWithMessageId = isResult;
    }

    private void SetTgDownloadIsAutoUpdate(TgDownloadSettingsModel tgDownloadSettings)
    {
        bool isResult = AnsiConsole.Prompt(new SelectionPrompt<bool>()
            .Title(TgLocale.MenuIsAutoUpdate)
            .PageSize(10)
            .AddChoices(true, false));
        tgDownloadSettings.IsAutoUpdate = isResult;
    }

    private void UpdateSourceWithSettings(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
    {
        if (!tgDownloadSettings.IsReady) return;
        // Update source.
        TgStorage.AddOrUpdateRecordSource(tgDownloadSettings.SourceId, tgDownloadSettings.SourceUserName, 
            tgDownloadSettings.SourceTitle, tgDownloadSettings.SourceAbout, tgDownloadSettings.SourceLastId, true);
        // Update source settings.
        TgStorage.AddOrUpdateRecordSourceSetting(tgDownloadSettings.SourceId, tgDownloadSettings.DestDirectory, 
            tgDownloadSettings.SourceFirstId, tgDownloadSettings.IsAutoUpdate, true);
        // Refresh.
        refreshStatus(TgLocale.SettingsSource, false);
    }

    private void LoadTgClientSettings(TgDownloadSettingsModel tgDownloadSettings, bool isSkipFirstId, bool isSkipDestDirectory)
    {
        SqlTableSourceSettingModel sourceSettings = TgStorage.GetItem<SqlTableSourceSettingModel>(null, tgDownloadSettings.SourceId);
        if (!isSkipFirstId)
            tgDownloadSettings.SourceFirstId = sourceSettings.FirstId;
        if (!isSkipDestDirectory)
            tgDownloadSettings.DestDirectory = sourceSettings.Directory;
        tgDownloadSettings.IsAutoUpdate = sourceSettings.IsAutoUpdate;
    }

    private void UpdateSource(ChatBase chat, string about, int count)
    {
        if (chat is Channel channel)
            TgStorage.AddOrUpdateRecordSource(channel.id, channel.username, channel.title, about, count, true);
    }

    private void ManualDownload(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
    {
        UpdateSourceWithSettings(tgDownloadSettings, refreshStatus);
        ShowTableDownload(tgDownloadSettings);
        TgClient.DownloadAllData(tgDownloadSettings, refreshStatus, StoreMessage, StoreDocument, FindExistsMessage);
        // Update last id.
        UpdateSourceWithSettings(tgDownloadSettings, refreshStatus);
    }

    private void AutoDownload(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
    {
        List<SqlTableSourceSettingModel> sourceSettings = TgStorage.GetList<SqlTableSourceSettingModel>();
        foreach (SqlTableSourceSettingModel sourceSetting in sourceSettings.Where(sourceSetting => sourceSetting.IsAutoUpdate))
        {
            SetupDownloadSource(tgDownloadSettings, sourceSetting.SourceId);

            SqlTableSourceModel source = TgStorage.GetItem<SqlTableSourceModel>(sourceSetting.SourceId);
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

    private void Scan(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
    {
        ShowTableScan(tgDownloadSettings);
        TgClient.FindAndStoreChannel(refreshStatus, UpdateSource);
    }

    private void SaveSettings(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
    {
        ShowTableScan(tgDownloadSettings);
        UpdateSourceWithSettings(tgDownloadSettings, refreshStatus);
    }

    #endregion
}