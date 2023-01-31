// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderConsole.Helpers;

internal partial class MenuHelper
{
    #region Public and private methods

    private MenuStorage SetMenuStorage()
    {
        string prompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(TgLocale.MenuSwitchNumber)
                .PageSize(10)
                .MoreChoicesText(TgLocale.MoveUpDown)
                .AddChoices(TgLocale.MenuMainReturn,
                    TgLocale.MenuStorageCreateNew,
                    TgLocale.MenuStorageDeleteExists
                ));
        return prompt switch
        {
            "Create new storage" => MenuStorage.CreateNew,
            "Delete exists storage" => MenuStorage.DeleteExists,
            _ => MenuStorage.Return
        };
    }

    public void SetupStorage(TgDownloadSettingsModel tgDownloadSettings)
    {
        MenuStorage menu;
        do
        {
            ShowTableStorageSettings(tgDownloadSettings);
            menu = SetMenuStorage();
            switch (menu)
            {
                case MenuStorage.CreateNew:
                    TgStorage.CreateOrConnectDb(true);
                    break;
                case MenuStorage.DeleteExists:
                    TgStorage.DeleteExistsDb();
                    break;
                case MenuStorage.CreateTables:
                    TgStorage.CreateTables();
                    break;
                case MenuStorage.DropTables:
                    TgStorage.DropTables();
                    break;
                case MenuStorage.ViewStatistics:
                    TgStorage.ViewStatistics();
                    TgLog.Info(TgLocale.TypeAnyKeyForReturn);
                    Console.ReadKey();
                    break;
                case MenuStorage.ClearTables:
                    TgStorage.ClearTables();
                    break;
                case MenuStorage.Return:
                default:
                    break;
            }
        } while (menu is not MenuStorage.Return);
    }

    #endregion
}