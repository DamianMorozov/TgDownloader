// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// See https://aka.ms/new-console-template for more information

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
                    TgLocale.MenuDownloadSetIsRewriteFiles,
                    TgLocale.MenuDownloadSetIsRewriteMessages,
                    TgLocale.MenuDownloadSetIsAddMessageId,
                    TgLocale.MenuDownload
                ));
        return userChoose switch
        {
            "Setup source ID" => MenuDownload.SetSourceId,
            "Setup source user name" => MenuDownload.SetSourceUserName,
            "Setup download folder" => MenuDownload.SetDestDirectory,
            "Enable rewrite exists files" => MenuDownload.SetIsRewriteFiles,
            "Enable rewrite exists messages" => MenuDownload.SetIsRewriteMessages,
            "Enable join message ID with file name" => MenuDownload.SetIsAddMessageId,
            "Download" => MenuDownload.Download,
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
                    TgClient.TgDownload.SetSourceIdByAsk();
                    TgClient.PrepareDownloadMessages(true);
                    break;
                case MenuDownload.SetSourceUserName:
                    TgClient.TgDownload.SetSourceUserNameByAsk();
                    TgClient.PrepareDownloadMessages(true);
                    break;
                case MenuDownload.SetDestDirectory:
                    TgClient.TgDownload.SetDestDirectory();
                    break;
                case MenuDownload.SetIsRewriteFiles:
                    TgClient.TgDownload.SetIsRewriteFiles();
                    break;
                case MenuDownload.SetIsRewriteMessages:
                    TgClient.TgDownload.SetIsRewriteMessages();
                    break;
                case MenuDownload.SetIsAddMessageId:
                    TgClient.TgDownload.SetIsAddMessageId();
                    break;
                case MenuDownload.Download:
                    RunAction(Download);
                    break;
                case MenuDownload.Return:
                default:
                    break;
            }
        } while (menu is not MenuDownload.Return);
    }

    public void Download()
    {
        ShowTableDownload();
        TgStorage.AddOrUpdateRecordSource(TgClient.TgDownload.SourceId, TgClient.TgDownload.SourceUserName, false);
        TgClient.DownloadAllData(RefreshStatusForDownload, StoreMessage, StoreDocument, FindExistsMessage);
    }

    #endregion
}