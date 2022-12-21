// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// See https://aka.ms/new-console-template for more information

using TgStorageCore.Models;

namespace TgDownloaderConsole.Helpers;

internal partial class MenuHelper
{
    #region Public and private methods

    private MenuClient SetMenuClient()
    {
        string userChoose = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(TgLocale.MenuSwitchNumber)
                .PageSize(10)
                .MoreChoicesText(TgLocale.MoveUpDown)
                .AddChoices(
                    TgLocale.MenuMainReturn, 
                    TgLocale.MenuClientConnect, 
                    TgLocale.MenuClientGetInfo));
        return userChoose switch
        {
            "Connect the client to TG server" => MenuClient.Connect,
            "Get info" => MenuClient.GetInfo,
            _ => MenuClient.Return
        };
    }

    public void SetupClient()
    {
        MenuClient menu;
        do
        {
            ShowTableClient();
            menu = SetMenuClient();
            switch (menu)
            {
                case MenuClient.Connect:
                    ClientConnect();
                    break;
                case MenuClient.GetInfo:
                    ClientGetInfo();
                    break;
                case MenuClient.Return:
                default:
                    break;
            }
        } while (menu is not MenuClient.Return);
    }

    public void ClientConnectExists()
    {
        TableAppModel app = TgStorage.GetRecordApp();
        if (TgStorage.IsValid(app))
        {
            TgClient.Connect(app.ApiHash, app.PhoneNumber);
            if (TgClient.IsReady)
            {
                TgClient.CollectAllChats().GetAwaiter().GetResult();
            }
        }
    }

    public void ClientConnectNew()
    {
        TgClient.Connect(string.Empty, string.Empty);
        if (TgClient.IsReady)
        {
            TgStorage.AddRecordApp(TgClient.ApiHash, TgClient.PhoneNumber);
            TgClient.CollectAllChats().GetAwaiter().GetResult();
        }
    }

    public void ClientConnect()
    {
        ShowTableClient();
        ClientConnectNew();
        TgLog.Info(TgLocale.TgClientSetupComplete);
        Console.ReadKey();
    }

    public void ClientGetInfo()
    {
        ShowTableClient();
        if (!TgClient.IsReady)
        {
            TgLog.Warning(TgLocale.TgMustClientConnect);
            Console.ReadKey();
            return;
        }

        Dictionary<long, ChatBase> dicDialogs = TgClient.CollectAllDialogs();
        TgClient.PrintChatsInfo(dicDialogs, "dialogs");

        TgLog.Info(TgLocale.TgGetInfoComplete);
        Console.ReadKey();
    }

    #endregion
}