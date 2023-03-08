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
					TgLocale.MenuStorageVersionsView,
					TgLocale.MenuStorageCreateNew,
					TgLocale.MenuStorageDeleteExists
				));
		return prompt switch
		{
			"Versions info" => MenuStorage.VersionsView,
			"Create new storage" => MenuStorage.DbCreateNew,
			"Delete exists storage" => MenuStorage.DbDeleteExists,
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
				case MenuStorage.VersionsView:
					TgStorageVersionsView();
					break;
				case MenuStorage.DbCreateNew:
					TgStorageCreateNewDb();
					break;
				case MenuStorage.DbDeleteExists:
					TgStorageDeleteExistsDb();
					break;
			}
		} while (menu is not MenuStorage.Return);
	}

	private void TgStorageVersionsView()
	{
		TgStorage.VersionsView();
		TgLog.WriteLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
	}

	private void TgStorageCreateNewDb()
	{
		if (AskQuestionReturnNegative(TgLocale.MenuStorageCreateNew)) return;
		TgStorage.CreateOrConnectDb();
	}

	private void TgStorageDeleteExistsDb()
	{
		if (AskQuestionReturnNegative(TgLocale.MenuStorageDeleteExists)) return;
		AnsiConsole.WriteLine(TgLocale.MenuStorageDeleteExistsInfo(AppSettings.AppXml.FileStorage));
		TgLog.WriteLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
	}

	#endregion
}