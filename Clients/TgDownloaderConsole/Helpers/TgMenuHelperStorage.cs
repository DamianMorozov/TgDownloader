// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	private TgEnumMenuStorage SetMenuStorage()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgLocale.MenuSwitchNumber}")
				.PageSize(Console.WindowHeight - 17)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(TgLocale.MenuMainReturn,
					TgLocale.MenuStorageDbBackup,
					TgLocale.MenuStorageDbCreateNew,
					TgLocale.MenuStorageDbDeleteExists,
					TgLocale.MenuStorageTablesVersionsView,
					TgLocale.MenuStorageTablesClear,
					TgLocale.MenuStorageTablesCompact
				));
		if (prompt.Equals(TgLocale.MenuStorageDbBackup))
			return TgEnumMenuStorage.DbBackup;
		if (prompt.Equals(TgLocale.MenuStorageDbCreateNew))
			return TgEnumMenuStorage.DbCreateNew;
		if (prompt.Equals(TgLocale.MenuStorageDbDeleteExists))
			return TgEnumMenuStorage.DbDeleteExists;
		if (prompt.Equals(TgLocale.MenuStorageTablesVersionsView))
			return TgEnumMenuStorage.TablesVersionsView;
		if (prompt.Equals(TgLocale.MenuStorageTablesClear))
			return TgEnumMenuStorage.TablesClear;
		if (prompt.Equals(TgLocale.MenuStorageTablesCompact))
			return TgEnumMenuStorage.TablesCompact;
		return TgEnumMenuStorage.Return;
	}

	public void SetupStorage(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		TgEnumMenuStorage menu;
		do
		{
			ShowTableStorageSettings(tgDownloadSettings);
			menu = SetMenuStorage();
			switch (menu)
			{
				case TgEnumMenuStorage.DbBackup:
					TgStorageBackupDb();
					break;
				case TgEnumMenuStorage.DbCreateNew:
					TgStorageCreateNewDb();
					break;
				case TgEnumMenuStorage.DbDeleteExists:
					TgStorageDeleteExistsDb();
					break;
				case TgEnumMenuStorage.TablesVersionsView:
					TgStorageTablesVersionsView();
					break;
				case TgEnumMenuStorage.TablesClear:
					TgStorageTablesClear();
					break;
				case TgEnumMenuStorage.TablesCompact:
					TgStorageTablesCompact();
					break;
			}
		} while (menu is not TgEnumMenuStorage.Return);
	}

	private void TgStorageBackupDb()
	{
		if (AskQuestionReturnNegative(TgLocale.MenuStorageDbBackup))
			return;
		TgLog.WriteLine($"{TgLocale.MenuStorageBackupDirectory}: {Path.GetDirectoryName(TgAppSettings.AppXml.XmlEfStorage)}");
		(bool IsSuccess, string FileName) backupResult = EfContext.BackupDb();
		TgLog.WriteLine($"{TgLocale.MenuStorageBackupFile}: {backupResult.FileName}");
		TgLog.WriteLine(backupResult.IsSuccess ? TgLocale.MenuStorageBackupSuccess : TgLocale.MenuStorageBackupFailed);
		TgLog.WriteLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
	}

	private void TgStorageCreateNewDb()
	{
		if (AskQuestionReturnNegative(TgLocale.MenuStorageDbCreateNew))
			return;
		TgEfUtils.CreateAndUpdateDb();
	}

	private void TgStorageDeleteExistsDb()
	{
		AnsiConsole.WriteLine(TgLocale.MenuStoragePerformSteps);
		AnsiConsole.WriteLine($"- {TgLocale.MenuStorageExitProgram}");
		AnsiConsole.WriteLine($"- {TgLocale.MenuStorageDeleteExistsInfo(TgAppSettings.AppXml.XmlEfStorage)}");
		TgLog.WriteLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
	}

	private void TgStorageTablesVersionsView()
	{
		TgEfUtils.VersionsView();
		TgLog.WriteLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
	}

	private void TgStorageTablesClear()
	{
		if (AskQuestionReturnNegative(TgLocale.MenuStorageTablesClear))
			return;
		EfContext.DeleteTables();
		TgEfUtils.CreateAndUpdateDb();
		TgLog.WriteLine(TgLocale.MenuStorageTablesClearFinished);
		Console.ReadKey();
	}

	private void TgStorageTablesCompact()
	{
		if (AskQuestionReturnNegative(TgLocale.MenuStorageTablesCompact))
			return;
		EfContext.CompactDb();
		TgLog.WriteLine(TgLocale.MenuStorageTablesCompactFinished);
		Console.ReadKey();
	}

	#endregion
}