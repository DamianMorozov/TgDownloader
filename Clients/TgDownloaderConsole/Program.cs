// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

// App
using Velopack.Sources;

TgAppSettingsHelper tgAppSettings = TgAppSettingsHelper.Instance;
tgAppSettings.SetVersion(Assembly.GetExecutingAssembly());
// Console
TgLocaleHelper tgLocale = TgLocaleHelper.Instance;
TgLogHelper tgLog = TgLogHelper.Instance;

// Velopack installer update
await VelopackUpdateAsync(tgLog, tgLocale);

// Register TgEfContext as the DbContext for EF Core
tgLog.WriteLine("EF Core init ...");
await TgEfUtils.CreateAndUpdateDbAsync();
tgLog.WriteLine("EF Core init success");

TgDownloadSettingsViewModel tgDownloadSettings = new();
TgMenuHelper menu = new();
if (!await SetupAsync()) return;

do
{
	try
	{
		await menu.ShowTableMainAsync(tgDownloadSettings);
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
			await menu.SetupAppSettingsAsync(tgDownloadSettings);
		}
		if (prompt.Equals(tgLocale.MenuMainStorage))
		{
			menu.Value = TgEnumMenuMain.Storage;
			await menu.SetupStorageAsync(tgDownloadSettings);
		}
		if (prompt.Equals(tgLocale.MenuMainClient))
		{
			menu.Value = TgEnumMenuMain.Client;
			await menu.SetupClientAsync(tgDownloadSettings);
		}
		if (prompt.Equals(tgLocale.MenuMainFilters))
		{
			menu.Value = TgEnumMenuMain.Filters;
			await menu.SetupFiltersAsync(tgDownloadSettings);
		}
		if (prompt.Equals(tgLocale.MenuMainDownload))
		{
			menu.Value = TgEnumMenuMain.Download;
			await menu.SetupDownloadAsync(tgDownloadSettings);
		}
		if (prompt.Equals(tgLocale.MenuMainAdvanced))
		{
			menu.Value = TgEnumMenuMain.Advanced;
			await menu.SetupAdvancedAsync(tgDownloadSettings);
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

async Task<bool> SetupAsync()
{
	// Menu
	tgLog.WriteLine("Menu init ...");
	TgAsyncUtils.SetAppType(TgEnumAppType.Console);
	tgLog.WriteLine("Menu init success");

	// Client
	tgLog.WriteLine("TG client connect ...");
	await menu.ClientConnectConsoleAsync();
	tgLog.WriteLine("TG client connect success");
	return true;
}

// Velopack installer update
static async Task VelopackUpdateAsync(TgLogHelper tgLog, TgLocaleHelper tgLocale)
{
	Console.OutputEncoding = Encoding.UTF8;
	Console.Title = TgConstants.AppTitleConsoleShort;
	tgLog.SetMarkupLine(AnsiConsole.WriteLine);
	tgLog.SetMarkupLineStamp(AnsiConsole.MarkupLine);
	tgLog.WriteLine($"{TgConstants.AppTitleConsole} {TgAppSettingsHelper.Instance.AppVersion} started");

	VelopackApp.Build()
#if WINDOWS
		.WithBeforeUninstallFastCallback((v) => {
			// delete / clean up some files before uninstallation
			tgLog.WriteLine($"Uninstalling the {TgConstants.AppTitleConsole}!");
		})
#endif
		.WithFirstRun((v) => {
			tgLog.WriteLine($"Thanks for installing the {TgConstants.AppTitleConsole}!");
		})
		.Run();
	tgLog.WriteLine($"Checking updates on the link {TgConstants.LinkGitHub}...");
	var mgr = new UpdateManager(new GithubSource(TgConstants.LinkGitHub, string.Empty, prerelease: false));
	// Check for new version
	try
	{
		var newVersion = await mgr.CheckForUpdatesAsync();
		if (newVersion is null)
		{
			tgLog.WriteLine("No update available");
			return;
		}
		// Download new version
		tgLog.WriteLine("Download new version...");
		await mgr.DownloadUpdatesAsync(newVersion);
		// Install new version and restart app
		var prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("Install new version and restart app?")
				.PageSize(Console.WindowHeight - 5)
				.MoreChoicesText(tgLocale.MoveUpDown)
				.AddChoices(tgLocale.MenuNo, tgLocale.MenuYes));
		var isInstall = prompt.Equals(tgLocale.MenuYes);
		if (isInstall)
			mgr.ApplyUpdatesAndRestart(newVersion);
	}
	// Cannot perform this operation in an application which is not installed
	catch (Exception ex)
	{
		tgLog.WriteLine(ex.Message);
	}
}