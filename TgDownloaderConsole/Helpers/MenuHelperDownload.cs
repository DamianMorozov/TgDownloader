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
                    TgLocale.MenuDownloadSetSourceFirstIdAuto, 
                    TgLocale.MenuDownloadSetSourceFirstIdManual, 
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
            "Setup source first ID auto" => MenuDownload.SetSourceFirstIdAuto,
            "Setup source first ID manual" => MenuDownload.SetSourceFirstIdManual,
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
                case MenuDownload.SetSourceFirstIdAuto:
                    RunAction(SetTgDownloadSourceFirstIdAuto, true);
                    LoadTgClientSettings();
                    SetSourceWithSettings();
                    break;
                case MenuDownload.SetSourceFirstIdManual:
                    SetTgDownloadSourceFirstIdManual();
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
        do
        {
            TgClient.TgDownload.SourceId = AnsiConsole.Ask<long>(TgLog.GetLineStampInfo(TgLocale.TypeTgSourceId));
            isCheck = TgClient.TgDownload.IsReadySourceId;
        } while (!isCheck);
    }

    private void SetTgDownloadSourceFirstIdAuto()
    {
        Channel? channel = TgClient.PrepareDownloadMessages(true);
        if (channel is null) return;
        TgClient.SetChannelMessageIdFirst(channel, RefreshStatusForDownload);
    }

    private void SetTgDownloadSourceFirstIdManual()
    {
        bool isCheck;
        do
        {
            TgClient.TgDownload.SourceFirstId = AnsiConsole.Ask<int>(TgLog.GetLineStampInfo(TgLocale.TypeTgSourceFirstId));
            isCheck = TgClient.TgDownload.IsReadySourceFirstId;
        } while (!isCheck);
    }

    private void SetTgDownloadSourceUserName()
    {
        TgClient.TgDownload.SetDefault(1);
        bool isCheck;
        do
        {
            TgClient.TgDownload.SourceUserName = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo(TgLocale.TypeTgSourceUserName));
            if (!string.IsNullOrEmpty(TgClient.TgDownload.SourceUserName))
            {
                TgClient.TgDownload.SourceUserName = TgClient.TgDownload.SourceUserName.StartsWith(@"https://t.me/")
                    ? TgClient.TgDownload.SourceUserName.Replace("https://t.me/", string.Empty)
                    : TgClient.TgDownload.SourceUserName;
            }
            isCheck = !string.IsNullOrEmpty(TgClient.TgDownload.SourceUserName);
        } while (!isCheck);
    }

    private void SetTgDownloadDestDirectory()
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