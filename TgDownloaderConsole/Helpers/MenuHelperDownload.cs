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
                    TgLocale.MenuDownloadSetUserName, 
                    TgLocale.MenuDownloadSetFolder, 
                    //TgLocale.MenuDownloadSetMessageCurrentId,
                    //TgLocale.MenuDownloadSetIsRewriteFiles,
                    TgLocale.MenuDownload
                ));
        return userChoose switch
        {
            "Setup user name" => MenuDownload.SetSourceUsername,
            "Setup download folder" => MenuDownload.SetDestDirectory,
            //"Setup message current ID" => MenuDownload.SetMessageCurrentId,
            //"Enable rewrite exists files" => MenuDownload.SetIsRewriteFiles,
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
                case MenuDownload.SetSourceUsername:
                    TgClient.TgDownload.SetSourceUserName();
                    TgClient.PrepareDownloadMessages(true);
                    break;
                case MenuDownload.SetDestDirectory:
                    TgClient.TgDownload.SetDestDirectory();
                    break;
                case MenuDownload.SetMessageCurrentId:
                    TgClient.TgDownload.SetMessageCurrentId();
                    break;
                case MenuDownload.SetIsRewriteFiles:
                    TgClient.TgDownload.SetIsRewriteFiles();
                    break;
                case MenuDownload.Download:
                    RunAction(menu, Download);
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
        TgStorage.AddRecordSource(TgClient.TgDownload.SourceId, TgClient.TgDownload.SourceUserName);
        TgClient.DownloadMessages(RefreshStatusForDownload, StoreMessage, FindExistsMessage);
    }

    #endregion
}