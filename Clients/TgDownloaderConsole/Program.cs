// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

TgAsyncUtils.SetAppType(TgEnumAppType.Console);
TgAppSettingsHelper tgAppSettings = TgAppSettingsHelper.Instance;
TgMenuHelper menu = TgMenuHelper.Instance;
TgLocaleHelper tgLocale = TgLocaleHelper.Instance;
TgLogHelper tgLog = TgLogHelper.Instance;
TgDownloadSettingsModel tgDownloadSettings = new();

if (!Setup()) return;

do
{
	try
	{
		menu.ShowTableMain(tgDownloadSettings);
		tgLog.MarkupLine(tgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title($"  {tgLocale.MenuSwitchNumber}")
			.PageSize(Console.WindowHeight - 17)
			.MoreChoicesText(tgLocale.MoveUpDown)
			.AddChoices(
				tgLocale.MenuMainExit, tgLocale.MenuMainApp, tgLocale.MenuMainStorage, tgLocale.MenuMainClient,
				tgLocale.MenuMainFilters, tgLocale.MenuMainDownload, tgLocale.MenuMainAdvanced));
		if (prompt.Equals(tgLocale.MenuMainExit))
			menu.Value = TgEnumMenuMain.Exit;
		if (prompt.Equals(tgLocale.MenuMainApp))
		{
			menu.Value = TgEnumMenuMain.AppSettings;
			menu.SetupAppSettings(tgDownloadSettings);
		}
		if (prompt.Equals(tgLocale.MenuMainStorage))
		{
			menu.Value = TgEnumMenuMain.Storage;
			menu.SetupStorage(tgDownloadSettings);
		}
		if (prompt.Equals(tgLocale.MenuMainClient))
		{
			menu.Value = TgEnumMenuMain.Client;
			menu.SetupClient(tgDownloadSettings);
		}
		if (prompt.Equals(tgLocale.MenuMainFilters))
		{
			menu.Value = TgEnumMenuMain.Filters;
			menu.SetupFilters(tgDownloadSettings);
		}
		if (prompt.Equals(tgLocale.MenuMainDownload))
		{
			menu.Value = TgEnumMenuMain.Download;
			menu.SetupDownload(tgDownloadSettings);
		}
		if (prompt.Equals(tgLocale.MenuMainAdvanced))
		{
			menu.Value = TgEnumMenuMain.Advanced;
			menu.SetupAdvanced(tgDownloadSettings);
		}
	}
	catch (Exception ex)
	{
		tgLog.MarkupLine($"{tgLocale.StatusException}: " + tgLog.GetMarkupString(ex.Message));
		if (ex.InnerException is not null)
			tgLog.MarkupLine($"{tgLocale.StatusInnerException}: " + tgLog.GetMarkupString(ex.InnerException.Message));
		tgLog.WriteLine(tgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
	}
} while (menu.Value is not TgEnumMenuMain.Exit);

bool Setup()
{
	// App.
	tgAppSettings.AppXml.SetVersion(Assembly.GetExecutingAssembly());
	// Console.
	Console.OutputEncoding = Encoding.UTF8;
	Console.Title = TgConstants.AppTitleConsoleShort;
	tgLog.SetMarkupLine(AnsiConsole.WriteLine);
	tgLog.SetMarkupLineStamp(AnsiConsole.MarkupLine);
	// Create new storage?
	if (!tgAppSettings.AppXml.IsExistsFileStorage)
	{
		AnsiConsole.WriteLine(tgLocale.MenuStorageDbIsNotFound(tgAppSettings.AppXml.FileStorage));
		if (menu.AskQuestionReturnNegative(tgLocale.MenuStorageDbCreateNew))
			return false;
	}
	// Storage is exist.
	else if (Equals(TgFileUtils.CalculateFileSize(tgAppSettings.AppXml.FileStorage), (long)0))
	{
		AnsiConsole.WriteLine(tgLocale.MenuStorageDbIsZeroSize(tgAppSettings.AppXml.FileStorage));
		if (menu.AskQuestionReturnNegative(tgLocale.MenuStorageDbCreateNew))
			return false;
	}
	// Client.
	menu.ClientConnectConsole();
	return true;
}