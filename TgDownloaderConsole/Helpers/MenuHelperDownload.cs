// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Globalization;

namespace TgDownloaderConsole.Helpers;

internal partial class MenuHelper
{
    #region Public and private methods

    private MenuDownload SetMenuDownload()
    {
        string prompt = AnsiConsole.Prompt(
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
                    TgLocale.MenuDownloadSetIsAutoUpdate,
                    TgLocale.MenuSaveSettings,
                    TgLocale.MenuDownloadManual
                ));
        return prompt switch
        {
            "Setup source (ID/username)" => MenuDownload.SetSource,
            "Setup download folder" => MenuDownload.SetDestDirectory,
            "Setup first ID auto" => MenuDownload.SetSourceFirstIdAuto,
            "Setup first ID manual" => MenuDownload.SetSourceFirstIdManual,
            "Enable rewrite exists files" => MenuDownload.SetIsRewriteFiles,
            "Enable rewrite exists messages" => MenuDownload.SetIsRewriteMessages,
            "Enable join message ID with file name" => MenuDownload.SetIsAddMessageId,
            "Enable auto update" => MenuDownload.SetIsAutoUpdate,
            "Save settings" => MenuDownload.SettingsSave,
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
                case MenuDownload.SettingsSave:
                    RunAction(tgDownloadSettings, SaveSettings, true);
                    break;
                case MenuDownload.DownloadManual:
                    RunAction(tgDownloadSettings, ManualDownload, false);
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
            tgDownloadSettings.DestDirectory = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.DirectoryDestType}:"));
            if (!Directory.Exists(tgDownloadSettings.DestDirectory))
            {
                TgLog.Info(TgLocale.DirectoryIsNotExists(tgDownloadSettings.DestDirectory));
                string prompt = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title($"{TgLocale.DirectoryCreate}?")
                    .PageSize(10)
                    .AddChoices(TgLocale.IsTrue, TgLocale.IsFalse));
                bool isCreate = prompt switch
                {
                    "True" => true,
                    _ => false
                };
                if (isCreate)
                {
                    try
                    {
                        Directory.CreateDirectory(tgDownloadSettings.DestDirectory);
                    }
                    catch (Exception ex)
                    {
                        TgLog.Warning(TgLocale.DirectoryCreateIsException(ex));
                    }
                }
            }
        } while (!Directory.Exists(tgDownloadSettings.DestDirectory));
    }

    private void SetTgDownloadIsRewriteFiles(TgDownloadSettingsModel tgDownloadSettings)
    {
        string prompt = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title(TgLocale.TgSettingsIsRewriteFiles)
            .PageSize(10)
            .AddChoices(TgLocale.IsTrue, TgLocale.IsFalse));
        tgDownloadSettings.IsRewriteFiles = prompt switch
        {
            "True" => true,
            _ => false
        };
    }

    private void SetTgDownloadIsRewriteMessages(TgDownloadSettingsModel tgDownloadSettings)
    {
        string prompt = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title(TgLocale.TgSettingsIsRewriteMessages)
            .PageSize(10)
            .AddChoices(TgLocale.IsTrue, TgLocale.IsFalse));
        tgDownloadSettings.IsRewriteMessages = prompt switch
        {
            "True" => true,
            _ => false
        };
    }

    private void SetTgDownloadIsJoinFileNameWithMessageId(TgDownloadSettingsModel tgDownloadSettings)
    {
        string prompt = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title(TgLocale.TgSettingsIsJoinFileNameWithMessageId)
            .PageSize(10)
            .AddChoices(TgLocale.IsTrue, TgLocale.IsFalse));
        tgDownloadSettings.IsJoinFileNameWithMessageId = prompt switch
        {
            "True" => true,
            _ => false
        };
    }

    private void SetTgDownloadIsAutoUpdate(TgDownloadSettingsModel tgDownloadSettings)
    {
        string prompt = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title(TgLocale.MenuDownloadSetIsAutoUpdate)
            .PageSize(10)
            .AddChoices(TgLocale.IsTrue, TgLocale.IsFalse));
        tgDownloadSettings.IsAutoUpdate = prompt switch
        {
            "True" => true,
            _ => false
        };
    }

    private void UpdateSourceWithSettings(TgDownloadSettingsModel tgDownloadSettings, Action<string, bool> refreshStatus)
    {
        if (!tgDownloadSettings.IsReady) return;
        // Update source.
        TgStorage.AddOrUpdateItemSourceDeprecated(tgDownloadSettings.SourceId, tgDownloadSettings.SourceUserName,
            tgDownloadSettings.SourceTitle, tgDownloadSettings.SourceAbout, tgDownloadSettings.SourceLastId, true);
        // Update source settings.
        TgStorage.AddOrUpdateItemSourceSettingDeprecated(tgDownloadSettings.SourceId, tgDownloadSettings.DestDirectory,
            tgDownloadSettings.SourceFirstId, tgDownloadSettings.IsAutoUpdate, true);
        // Refresh.
        refreshStatus(TgLocale.SettingsSource, false);
    }

    private void LoadTgClientSettings(TgDownloadSettingsModel tgDownloadSettings, bool isSkipFirstId, bool isSkipDestDirectory)
    {
        SqlTableSourceSettingModel sourceSettings = TgStorage.GetItemDeprecated<SqlTableSourceSettingModel>(null, tgDownloadSettings.SourceId);
        if (!isSkipFirstId)
            tgDownloadSettings.SourceFirstId = sourceSettings.FirstId;
        if (!isSkipDestDirectory)
            tgDownloadSettings.DestDirectory = sourceSettings.Directory;
        tgDownloadSettings.IsAutoUpdate = sourceSettings.IsAutoUpdate;
    }

    private void UpdateSource(ChatBase chat, string about, int count)
    {
        if (chat is Channel channel)
            TgStorage.AddOrUpdateItemSourceDeprecated(channel.id, channel.username, channel.title, about, count, true);
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