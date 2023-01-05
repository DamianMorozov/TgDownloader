// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderConsole.Helpers;

internal partial class MenuHelper
{
    #region Public and private methods

    private MenuStorage SetMenuStorage()
    {
        string userChoose = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(TgLocale.MenuSwitchNumber)
                .PageSize(10)
                .MoreChoicesText(TgLocale.MoveUpDown)
                .AddChoices(TgLocale.MenuMainReturn,
                    TgLocale.MenuStorageCreateNew,
                    TgLocale.MenuStorageDeleteExists
                    //TgLocale.MenuStorageCreateTables,
                    //TgLocale.MenuStorageDropTables,
                    //TgLocale.MenuStorageViewStatistics,
                    //TgLocale.MenuStorageClearTables
                ));
        return userChoose switch
        {
            "Create new storage" => MenuStorage.CreateNew,
            "Delete exists storage" => MenuStorage.DeleteExists,
            //"Create tables" => MenuStorage.CreateTables,
            //"Drop tables" => MenuStorage.DropTables,
            //"View statistics" => MenuStorage.ViewStatistics,
            //"Clear tables" => MenuStorage.ClearTables,
            _ => MenuStorage.Return
        };
    }

    public void SetupStorage()
    {
        MenuStorage menu;
        do
        {
            ShowTableStorageSettings();
            menu = SetMenuStorage();
            switch (menu)
            {
                case MenuStorage.CreateNew:
                    TgStorage.CreateOrConnectDb();
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