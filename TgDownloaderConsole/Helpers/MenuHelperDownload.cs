// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

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
                    TgLocale.MenuDownloadSetSourceId, 
                    TgLocale.MenuDownloadSetSourceUserName, 
                    TgLocale.MenuDownloadSetFolder, 
                    TgLocale.MenuDownloadSetSourceStartIdAuto, 
                    TgLocale.MenuDownloadSetSourceStartIdManual, 
                    TgLocale.MenuDownloadSetIsRewriteFiles,
                    TgLocale.MenuDownloadSetIsRewriteMessages,
                    TgLocale.MenuDownloadSetIsAddMessageId,
                    TgLocale.MenuDownload
                    //TgLocale.MenuScanMyChannels
                ));
        return userChoose switch
        {
            "Setup source ID" => MenuDownload.SetSourceId,
            "Setup source user name" => MenuDownload.SetSourceUserName,
            "Setup download folder" => MenuDownload.SetDestDirectory,
            "Setup source start ID auto" => MenuDownload.SetSourceStartIdAuto,
            "Setup source start ID manual" => MenuDownload.SetSourceStartIdManual,
            "Enable rewrite exists files" => MenuDownload.SetIsRewriteFiles,
            "Enable rewrite exists messages" => MenuDownload.SetIsRewriteMessages,
            "Enable join message ID with file name" => MenuDownload.SetIsAddMessageId,
            "Download" => MenuDownload.Download,
            //"Scan my channels and groups" => MenuDownload.ScanChannels,
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
                case MenuDownload.SetSourceId:
                    SetTgDownloadSourceId();
                    TgClient.PrepareDownloadMessages(true);
                    LoadTgClientSettings();
                    SetSourceWithSettings();
                    break;
                case MenuDownload.SetSourceStartIdAuto:
                    RunAction(SetTgDownloadSourceStartIdAuto, true);
                    LoadTgClientSettings();
                    SetSourceWithSettings();
                    break;
                case MenuDownload.SetSourceStartIdManual:
                    SetTgDownloadSourceStartIdManual();
                    LoadTgClientSettings();
                    SetSourceWithSettings();
                    break;
                case MenuDownload.SetSourceUserName:
                    SetTgDownloadSourceUserName();
                    TgClient.PrepareDownloadMessages(true);
                    LoadTgClientSettings();
                    SetSourceWithSettings();
                    break;
                case MenuDownload.SetDestDirectory:
                    SetTgDownloadDestDirectory();
                    SetSourceWithSettings();
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
                case MenuDownload.ScanChannels:
                    RunAction(ScanRange, true);
                    break;
                case MenuDownload.Return:
                default:
                    break;
            }
        } while (menu is not MenuDownload.Return);
    }

    private void SetTgDownloadSourceId()
    {
        TgClient.TgDownload.SetDefault(1);
        bool isCheck;
        long sourceId;
        do
        {
            sourceId = AnsiConsole.Ask<long>(TgLog.GetLineStampInfo(TgLocale.TypeTgSourceId));
            isCheck = TgClient.TgDownload.IsReadySourceId;
        } while (!isCheck);
        TgClient.TgDownload.SourceId = sourceId;
    }

    private void SetTgDownloadSourceStartIdAuto()
    {
        Channel? channel = TgClient.PrepareDownloadMessages(true);
        if (channel is null) return;
        TgClient.SetChannelMessageIdFirst(channel, RefreshStatusForDownload);
    }

    private void SetTgDownloadSourceStartIdManual()
    {
        bool isCheck;
        int startId;
        do
        {
            startId = AnsiConsole.Ask<int>(TgLog.GetLineStampInfo(TgLocale.TypeTgSourceStartId));
            isCheck = TgClient.TgDownload.IsReadySourceStartId;
        } while (!isCheck);
        TgClient.TgDownload.SourceStartId = startId;
    }

    private void SetTgDownloadSourceUserName()
    {
        TgClient.TgDownload.SetDefault(1);
        bool isCheck;
        string sourceUserName;
        do
        {
            sourceUserName = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo(TgLocale.TypeTgSourceUserName));
            if (!string.IsNullOrEmpty(sourceUserName))
            {
                sourceUserName = sourceUserName.StartsWith(@"https://t.me/")
                    ? sourceUserName.Replace("https://t.me/", string.Empty)
                    : sourceUserName;
            }

            isCheck = !string.IsNullOrEmpty(sourceUserName);
        } while (!isCheck);
        TgClient.TgDownload.SourceUserName = sourceUserName;
    }

    private void SetTgDownloadDestDirectory()
    {
        string destDirectory;
        do
        {
            destDirectory = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo(TgLocale.TypeDestDirectory));
            if (!Directory.Exists(destDirectory))
                TgLog.Info(TgLocale.DirIsNotExistsSpecify(destDirectory));
        } while (!Directory.Exists(destDirectory));
        TgClient.TgDownload.DestDirectory = destDirectory;
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
        TgStorage.AddOrUpdateRecordSourceSetting(TgClient.TgDownload.SourceId, TgClient.TgDownload.DestDirectory, true);
    }

    private void LoadTgClientSettings()
    {
        TableSourceSettingModel sourceSettings = TgStorage.GetRecord<TableSourceSettingModel>(null, TgClient.TgDownload.SourceId);
        if (!TgClient.TgDownload.IsReadyDescription)
            TgClient.TgDownload.DestDirectory = sourceSettings.Directory;
    }

    private void StoreSource(ChatBase chat, string about, int count)
    {
        if (chat is Channel channel)
            TgStorage.AddOrUpdateRecordSource(channel.id, channel.username, channel.title, about, count, true);
    }

    private void Download()
    {
        ShowTableDownload();
        TgClient.DownloadAllData(RefreshStatusForDownload, StoreMessage, StoreDocument, FindExistsMessage);
    }

    private void ScanRange()
    {
        ShowTableScanRange();
        TgClient.FindAndStoreChannel(RefreshStatusForDownload, StoreSource);
    }

    #endregion
}