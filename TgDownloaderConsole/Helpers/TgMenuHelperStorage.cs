// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	private TgMenuStorage SetMenuStorage()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgConstants.MenuSwitchNumber}")
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(TgConstants.MenuMainReturn,
					TgConstants.MenuStorageDbBackup,
					TgConstants.MenuStorageDbCreateNew,
					TgConstants.MenuStorageDbDeleteExists,
					TgConstants.MenuStorageTablesVersionsView,
					TgConstants.MenuStorageTablesClear
				));
		return prompt switch
		{
			TgConstants.MenuStorageDbBackup => TgMenuStorage.DbBackup,
			TgConstants.MenuStorageDbCreateNew => TgMenuStorage.DbCreateNew,
			TgConstants.MenuStorageDbDeleteExists => TgMenuStorage.DbDeleteExists,
			TgConstants.MenuStorageTablesVersionsView => TgMenuStorage.TablesVersionsView,
			TgConstants.MenuStorageTablesClear => TgMenuStorage.TablesClear,
			_ => TgMenuStorage.Return
		};
	}

	public void SetupStorage(TgDownloadSettingsModel tgDownloadSettings)
	{
		TgMenuStorage menu;
		do
		{
			ShowTableStorageSettings(tgDownloadSettings);
			menu = SetMenuStorage();
			switch (menu)
			{
				case TgMenuStorage.DbBackup:
					TgStorageBackupDb();
					break;
				case TgMenuStorage.DbCreateNew:
					TgStorageCreateNewDb();
					break;
				case TgMenuStorage.DbDeleteExists:
					TgStorageDeleteExistsDb();
					break;
				case TgMenuStorage.TablesVersionsView:
					TgStorageTablesVersionsView();
					break;
				case TgMenuStorage.TablesClear:
					TgStorageTablesClear();
					break;
			}
		} while (menu is not TgMenuStorage.Return);
	}

	private void TgStorageBackupDb()
	{
		if (AskQuestionReturnNegative(TgConstants.MenuStorageDbBackup)) return;
		TgLog.WriteLine($"{TgConstants.MenuStorageBackupDirectory}: {Path.GetDirectoryName(TgAppSettings.AppXml.FileStorage)}");
		(bool IsSuccess, string FileName) backupResult = ContextManager.BackupDb();
		TgLog.WriteLine($"{TgConstants.MenuStorageBackupFile}: {backupResult.FileName}");
		TgLog.WriteLine(backupResult.IsSuccess ? TgConstants.MenuStorageBackupSuccess : TgConstants.MenuStorageBackupFailed);
		TgLog.WriteLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
	}

	private void TgStorageCreateNewDb()
	{
		if (AskQuestionReturnNegative(TgConstants.MenuStorageDbCreateNew)) return;
		ContextManager.CreateOrConnectDb(true);
	}

	private void TgStorageDeleteExistsDb()
	{
		AnsiConsole.WriteLine(TgConstants.MenuStoragePerformSteps);
		AnsiConsole.WriteLine($"- {TgConstants.MenuStorageExitProgram}");
		AnsiConsole.WriteLine($"- {TgLocale.MenuStorageDeleteExistsInfo(TgAppSettings.AppXml.FileStorage)}");
		TgLog.WriteLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
	}

	private void TgStorageTablesVersionsView()
	{
		ContextManager.VersionsView();
		TgLog.WriteLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
	}

	private void TgStorageTablesClear()
	{
		if (AskQuestionReturnNegative(TgConstants.MenuStorageTablesClear)) return;
		ContextManager.DeleteTables();
		ContextManager.CreateOrConnectDb(true);
		TgLog.WriteLine(TgConstants.MenuStorageTablesClearFinished);
		Console.ReadKey();
	}

	#endregion
}