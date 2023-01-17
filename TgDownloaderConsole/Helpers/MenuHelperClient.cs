// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgCore.Utils;
using TgDownloaderCore.Models;
using TgLocalization.Enums;
using TgStorageCore.Models.Apps;

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

    public void SetupClient(TgDownloadSettingsModel tgDownloadSettings)
    {
        MenuClient menu;
        do
        {
            ShowTableClient(tgDownloadSettings);
            menu = SetMenuClient();
            switch (menu)
            {
                case MenuClient.Connect:
                    ClientConnect(tgDownloadSettings);
                    break;
                case MenuClient.GetInfo:
                    ClientGetInfo(tgDownloadSettings);
                    break;
                case MenuClient.Return:
                default:
                    break;
            }
        } while (menu is not MenuClient.Return);
    }

    private string? GetConfigExists(string what) =>
        what switch
        {
            "api_id" => TgClient.ApiId = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupAppId}:")),
            "api_hash" => TgClient.ApiHash,
            "phone_number" => TgClient.PhoneNumber,
            "verification_code" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupCode}:")),
            "notifications" => AnsiConsole.Ask<bool>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupNotifications}:")).ToString(),
            "first_name" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupFirstName}:")),
            "last_name" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupLastName}:")),
            "session_pathname" => FileNameUtils.Session,
            "password" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupPassword}:")),
            _ => null
        };

    private string? GetConfigUser(string what) =>
        what switch
        {
            "api_id" => TgClient.ApiId = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupAppId}:")),
            "api_hash" => TgClient.ApiHash = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupApiHash}:")),
            "phone_number" => TgClient.PhoneNumber = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupPhone}:")),
            "verification_code" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupCode}:")),
            "notifications" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupNotifications}:")),
            "first_name" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupFirstName}:")),
            "last_name" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupLastName}:")),
            "session_pathname" => FileNameUtils.Session,
            "password" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupPassword}:")),
            _ => null
        };

    public void ClientConnectExists()
    {
        SqlTableAppModel app = TgStorage.GetItem<SqlTableAppModel>();
        if (TgStorage.IsValid(app))
        {
            TgClient.Connect(app.ApiHash, app.PhoneNumber, GetConfigExists, null);
            TgClient.CollectAllChats().GetAwaiter().GetResult();
        }
    }

    public void ClientConnectNew()
    {
        TgClient.Connect(string.Empty, string.Empty, null, GetConfigUser);
        if (TgClient.IsReady)
            TgStorage.AddOrUpdateRecordApp(TgClient.ApiHash, TgClient.PhoneNumber, false);
        TgClient.CollectAllChats().GetAwaiter().GetResult();
    }

    public void ClientConnect(TgDownloadSettingsModel tgDownloadSettings)
    {
        ShowTableClient(tgDownloadSettings);
        ClientConnectNew();
        TgLog.Info(TgLocale.TgClientSetupComplete);
        Console.ReadKey();
    }

    public void ClientGetInfo(TgDownloadSettingsModel tgDownloadSettings)
    {
        ShowTableClient(tgDownloadSettings);
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