// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderConsole.Helpers;

internal partial class MenuHelper
{
    #region Public and private methods

    private MenuAppSettings SetMenuApp()
    {
        string prompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(TgLocale.MenuSwitchNumber)
                .PageSize(10)
                .MoreChoicesText(TgLocale.MoveUpDown)
                .AddChoices(
                    TgLocale.MenuMainReturn,
                    TgLocale.MenuMainReset,
                    TgLocale.MenuAppFileSession,
                    TgLocale.MenuAppFileStorage,
                    TgLocale.MenuAppUseProxy
));
        return prompt switch
        {
            "Reset" => MenuAppSettings.Reset,
            "Setting the path to the session file" => MenuAppSettings.SetFileSession,
            "Setting the path to the storage file" => MenuAppSettings.SetFileStorage,
            "Usage proxy" => MenuAppSettings.SetUseProxy,
            _ => MenuAppSettings.Return
        };
    }

    public void SetupAppSettings(TgDownloadSettingsModel tgDownloadSettings)
    {
        MenuAppSettings menu;
        do
        {
            ShowTableAppSettings(tgDownloadSettings);
            menu = SetMenuApp();
            switch (menu)
            {
                case MenuAppSettings.Reset:
                    ResetAppSettings();
                    break;
                case MenuAppSettings.SetFileSession:
                    SetFileSession();
                    break;
                case MenuAppSettings.SetFileStorage:
                    SetFileStorage();
                    break;
                case MenuAppSettings.SetUseProxy:
                    SetUseProxy();
                    AskClientConnect(tgDownloadSettings);
                    break;
            }
        } while (menu is not MenuAppSettings.Return);
    }

    private void SetFileAppSettings()
    {
        AppSettings.StoreXmlSettings();
        TgStorage.CreateOrConnectDb(true);
    }

    private void ResetAppSettings()
    {
        AppSettings.AppXml = new();
        SetFileAppSettings();
    }

    private void SetFileSession()
    {
        AppSettings.AppXml.SetFileSessionPath(AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.MenuAppFileSession}:")));
        SetFileAppSettings();
    }

    private void SetFileStorage()
    {
        AppSettings.AppXml.SetFileStoragePath(AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.MenuAppFileStorage}:")));
        SetFileAppSettings();
    }

    private void SetUseProxy()
    {
        string prompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(TgLocale.MenuSwitchNumber)
                .PageSize(10)
                .MoreChoicesText(TgLocale.MoveUpDown)
                .AddChoices(TgLocale.MenuAppUseProxyDisable, TgLocale.MenuAppUseProxyEnable));
        AppSettings.AppXml.IsUseProxy = prompt switch
        {
            "Enable proxy" => true,
            _ => false
        };
        SetFileAppSettings();
    }

    #endregion
}