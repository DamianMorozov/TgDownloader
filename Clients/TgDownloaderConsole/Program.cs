// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

TgAppSettingsHelper tgAppSettings = TgAppSettingsHelper.Instance;
TgMenuHelper menu = TgMenuHelper.Instance;
TgLocaleHelper tgLocale = TgLocaleHelper.Instance;
TgLogHelper tgLog = TgLogHelper.Instance;
TgSqlContextManagerHelper contextManager = TgSqlContextManagerHelper.Instance;
TgDownloadSettingsModel tgDownloadSettings = new();

if (!Setup()) return;

do
{
	try
	{
		menu.ShowTableMain(tgDownloadSettings);
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title($"  {TgConstants.MenuSwitchNumber}")
			.PageSize(10)
			.MoreChoicesText(tgLocale.MoveUpDown)
			.AddChoices(
				TgConstants.MenuMainExit, TgConstants.MenuMainApp, TgConstants.MenuMainStorage, TgConstants.MenuMainClient,
				TgConstants.MenuMainFilters, TgConstants.MenuMainDownload, TgConstants.MenuMainAdvanced));
		switch (prompt)
		{
			case TgConstants.MenuMainExit:
				menu.Value = TgMenuMain.Exit;
				break;
			case TgConstants.MenuMainApp:
				menu.Value = TgMenuMain.AppSettings;
				menu.SetupAppSettings(tgDownloadSettings);
				break;
			case TgConstants.MenuMainStorage:
				menu.Value = TgMenuMain.Storage;
				menu.SetupStorage(tgDownloadSettings);
				break;
			case TgConstants.MenuMainClient:
				menu.Value = TgMenuMain.Client;
				menu.SetupClient(tgDownloadSettings);
				break;
			case TgConstants.MenuMainFilters:
				menu.Value = TgMenuMain.Filters;
				menu.SetupFilters(tgDownloadSettings);
				break;
			case TgConstants.MenuMainDownload:
				menu.Value = TgMenuMain.Download;
				menu.SetupDownload(tgDownloadSettings);
				break;
			case TgConstants.MenuMainAdvanced:
				menu.Value = TgMenuMain.Advanced;
				menu.SetupAdvanced(tgDownloadSettings);
				break;
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
} while (menu.Value is not TgMenuMain.Exit);

bool Setup()
{
	// App.
	tgAppSettings.AppXml.SetVersion(Assembly.GetExecutingAssembly());
	// Console.
	Console.OutputEncoding = Encoding.UTF8;
	tgLog.SetMarkupLine(AnsiConsole.WriteLine);
	tgLog.SetMarkupLineStamp(AnsiConsole.MarkupLine);
	// Storage.
	if (!contextManager.IsExistsDb())
	{
		AnsiConsole.WriteLine(tgLocale.MenuStorageDbIsNotFound(tgAppSettings.AppXml.FileStorage));
		if (menu.AskQuestionReturnNegative(TgConstants.MenuStorageDbCreateNew)) return false;
	}
	else if (Equals(TgFileUtils.CalculateFileSize(tgAppSettings.AppXml.FileStorage), (long)0))
	{
		AnsiConsole.WriteLine(tgLocale.MenuStorageDbIsZeroSize(tgAppSettings.AppXml.FileStorage));
		if (menu.AskQuestionReturnNegative(TgConstants.MenuStorageDbCreateNew)) return false;
	}
	contextManager.CreateOrConnectDb(true);
	// Client.
	menu.ClientConnectExists();
	return true;
}