// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgCore.Localization;

AppSettingsHelper appSettings = AppSettingsHelper.Instance;
MenuHelper menu = MenuHelper.Instance;
TgLocaleHelper locale = TgLocaleHelper.Instance;
TgLogHelper log = TgLogHelper.Instance;
TgStorageHelper tgStorage = TgStorageHelper.Instance;
TgDownloadSettingsModel tgDownloadSettings = new();

Setup();

do
{
    try
    {
        menu.ShowTableMain(tgDownloadSettings);
        string prompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title(locale.MenuSwitchNumber)
            .PageSize(10)
            .MoreChoicesText(locale.MoveUpDown)
            .AddChoices(
                locale.MenuMainExit, locale.MenuMainAppSettings, locale.MenuMainStorage, locale.MenuMainClient, 
                locale.MenuMainDownload, locale.MenuMainAdvanced));
        switch (prompt)
        {
            case "Exit":
                menu.Value = MenuMain.Exit;
                break;
            case "Application settings":
                menu.Value = MenuMain.AppSettings;
                menu.SetupAppSettings(tgDownloadSettings);
                break;
            case "Storage settings":
                menu.Value = MenuMain.Storage;
                menu.SetupStorage(tgDownloadSettings);
                break;
            case "Client settings":
                menu.Value = MenuMain.Client;
                menu.SetupClient(tgDownloadSettings);
                break;
            case "Download settings":
                menu.Value = MenuMain.Download;
                menu.SetupDownload(tgDownloadSettings);
                break;
            case "Advanced":
                menu.Value = MenuMain.Advanced;
                menu.SetupAdvanced(tgDownloadSettings);
                break;
        }
    }
    catch (Exception ex)
    {
        log.Line(locale.StatusException + log.GetMarkupString(ex.Message));
        if (ex.InnerException is not null)
            log.Line(locale.StatusInnerException + log.GetMarkupString(ex.InnerException.Message));
        menu.Value = MenuMain.Exit;
    }
} while (menu.Value is not MenuMain.Exit);

void Setup()
{
    // App.
    appSettings.AppXml.SetVersion(Assembly.GetExecutingAssembly());
    // Console.
    Console.OutputEncoding = Encoding.UTF8;
    log.SetMarkupLineStamp(AnsiConsole.MarkupLine);
    // Storage.
    tgStorage.CreateOrConnectDb(true);
    // Client.
    menu.ClientConnectExists(tgDownloadSettings);
}