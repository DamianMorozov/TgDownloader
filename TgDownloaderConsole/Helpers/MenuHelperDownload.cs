// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Globalization;
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
                .PageSize(10)
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
                    TgLocale.MenuDownload
                    //TgLocale.MenuScan
                    //TgLocale.MenuUpdate
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
            "Download" => MenuDownload.Download,
            "Scan my sources" => MenuDownload.Scan,
            "Update marked source" => MenuDownload.Update,
            _ => MenuDownload.Return
        };
    }

    public void SetupDownload()
    {
        MenuDownload menu;
        do
        {
            ShowTableDownload();
            menu = SetMenuDownload();
            switch (menu)
            {
                case MenuDownload.SetSource:
                    SetupDownloadSource();
                    break;
                case MenuDownload.SetSourceFirstIdAuto:
                    SetupDownloadSourceFirstIdAuto();
                    break;
                case MenuDownload.SetSourceFirstIdManual:
                    SetupDownloadSourceFirstIdManual();
                    break;
                case MenuDownload.SetDestDirectory:
                    SetupDownloadDestDirectory();
                    break;
                case MenuDownload.SetIsRewriteFiles:
                    SetTgDownloadIsRewriteFiles();
                    break;
                case MenuDownload.SetIsRewriteMessages:
                    SetTgDownloadIsRewriteMessages();
                    break;
                case MenuDownload.SetIsAddMessageId:
                    SetTgDownloadIsJoinFileNameWithMessageId();
                    break;
                case MenuDownload.Download:
                    RunAction(Download, false);
                    break;
                case MenuDownload.Scan:
                    RunAction(Scan, true);
                    break;
                case MenuDownload.Update:
                    RunAction(Update, true);
                    break;
                case MenuDownload.Return:
                default:
                    break;
            }
        } while (menu is not MenuDownload.Return);
    }

    private void SetupDownloadSource(long? sId = null)
    {
        TgClient.TgDownload.SetDefault(1);
        bool isCheck = false;
        do
        {
            string source = sId is { } lId ? lId.ToString() : AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.MenuDownloadSetSource}:"));
            if (!string.IsNullOrEmpty(source))
            {
                if (long.TryParse(source, NumberStyles.Integer, CultureInfo.InvariantCulture, out long sourceId))
                {
                    TgClient.TgDownload.SourceId = sourceId;
                    isCheck = TgClient.TgDownload.IsReadySourceId;
                }
                else
                {
                    TgClient.TgDownload.SourceUserName = source.StartsWith(@"https://t.me/")
                        ? source.Replace("https://t.me/", string.Empty)
                        : source;
                    isCheck = !string.IsNullOrEmpty(TgClient.TgDownload.SourceUserName);
                }
            }
        } while (!isCheck);
        TgClient.PrepareDownloadMessages(true);
        LoadTgClientSettings(false, false);
    }

    private void SetupDownloadSourceFirstIdAuto()
    {
        RunAction(() =>
        {
            Channel? channel = TgClient.PrepareDownloadMessages(true);
            if (channel is null) return;
            TgClient.SetChannelMessageIdFirst(channel, RefreshStatusForDownload);
        }, true);
        LoadTgClientSettings(true, false);
    }

    private void SetupDownloadSourceFirstIdManual()
    {
        do
        {
            TgClient.TgDownload.SourceFirstId = AnsiConsole.Ask<int>(TgLog.GetLineStampInfo(TgLocale.TypeTgSourceFirstId));
        } while (!TgClient.TgDownload.IsReadySourceFirstId);
        LoadTgClientSettings(true, false);
    }

    private void SetupDownloadDestDirectory()
    {
        do
        {
            TgClient.TgDownload.DestDirectory = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo(TgLocale.TypeDestDirectory));
            if (!Directory.Exists(TgClient.TgDownload.DestDirectory))
                TgLog.Info(TgLocale.DirIsNotExistsSpecify(TgClient.TgDownload.DestDirectory));
        } while (!Directory.Exists(TgClient.TgDownload.DestDirectory));
    }

    private void SetTgDownloadIsRewriteFiles()
    {
        bool isResult = AnsiConsole.Prompt(new SelectionPrompt<bool>()
            .Title(TgLocale.TgSettingsIsRewriteFiles)
            .PageSize(10)
            //.MoreChoicesText("[grey](Move up and down to reveal more)[/]")
            .AddChoices(true, false));
        TgClient.TgDownload.IsRewriteFiles = isResult;
    }

    private void SetTgDownloadIsRewriteMessages()
    {
        bool isResult = AnsiConsole.Prompt(new SelectionPrompt<bool>()
            .Title(TgLocale.TgSettingsIsRewriteMessages)
            .PageSize(10)
            //.MoreChoicesText("[grey](Move up and down to reveal more)[/]")
            .AddChoices(true, false));
        TgClient.TgDownload.IsRewriteMessages = isResult;
    }

    private void SetTgDownloadIsJoinFileNameWithMessageId()
    {
        bool isResult = AnsiConsole.Prompt(new SelectionPrompt<bool>()
            .Title(TgLocale.TgSettingsIsJoinFileNameWithMessageId)
            .PageSize(10)
            //.MoreChoicesText("[grey](Move up and down to reveal more)[/]")
            .AddChoices(true, false));
        TgClient.TgDownload.IsJoinFileNameWithMessageId = isResult;
    }

    private void SetSourceWithSettings()
    {
        if (!TgClient.TgDownload.IsReady) return;
        TgStorage.AddOrUpdateRecordSource(TgClient.TgDownload.SourceId, TgClient.TgDownload.SourceUserName, 
            TgClient.TgDownload.SourceTitle, TgClient.TgDownload.SourceAbout, TgClient.TgDownload.SourceLastId, true);
        TgStorage.AddOrUpdateRecordSourceSetting(TgClient.TgDownload.SourceId, TgClient.TgDownload.DestDirectory, 
            TgClient.TgDownload.SourceFirstId, true);
    }

    private void LoadTgClientSettings(bool isSkipFirstId, bool isSkipDestDirectory)
    {
        TableSourceSettingModel sourceSettings = TgStorage.GetRecord<TableSourceSettingModel>(null, TgClient.TgDownload.SourceId);
        if (!isSkipFirstId)
            TgClient.TgDownload.SourceFirstId = sourceSettings.FirstId;
        if (!isSkipDestDirectory)
            TgClient.TgDownload.DestDirectory = sourceSettings.Directory;
    }

    private void StoreSource(ChatBase chat, string about, int count)
    {
        if (chat is Channel channel)
            TgStorage.AddOrUpdateRecordSource(channel.id, channel.username, channel.title, about, count, true);
    }

    private void Scan()
    {
        ShowTableScan();
        TgClient.FindAndStoreChannel(RefreshStatusForDownload, StoreSource);
    }

    private void Download()
    {
        SetSourceWithSettings();
        ShowTableDownload();
        TgClient.DownloadAllData(RefreshStatusForDownload, StoreMessage, StoreDocument, FindExistsMessage);
        SetSourceWithSettings();
    }

    private void Update()
    {
        List<TableSourceSettingModel> sourceSettings = TgStorage.GetRecords<TableSourceSettingModel>();
        foreach (TableSourceSettingModel sourceSetting in sourceSettings)
        {
            if (sourceSetting.IsTaskUpdate)
            {
                SetupDownloadSource(sourceSetting.SourceId);
                Download();
            }
        }
    }

    #endregion
}