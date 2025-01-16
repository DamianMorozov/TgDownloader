// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public partial class TgMainViewModel : TgPageViewModelBase
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	public partial string VersionDescription { get; set; } = string.Empty;
	[ObservableProperty]
	public partial string AppVersionTitle { get; set; } = string.Empty;
	[ObservableProperty]
	public partial string AppVersionShort { get; set; } = string.Empty;
	[ObservableProperty]
	public partial string AppVersionFull { get; set; } = string.Empty;
	[ObservableProperty]
	public partial string LicenseDescription { get; set; } = string.Empty;
	[ObservableProperty]
	public partial string DonateUsdtTrc20 { get; set; } = "TBTDRWnMBw7acfpkhAXjSQNSNHQGFR662Y";
	[ObservableProperty]
	public partial string DonateUsdtTon { get; set; } = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	public partial string DonateToncoin { get; set; } = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	public partial string DonateBitcoin { get; set; } = "1FJayytWUK6vkxK2nUcD2TJskk3g9ZnmfW";
	[ObservableProperty]
	public partial string DonateNotcoin { get; set; } = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	public partial string DonateDogs { get; set; } = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	public partial string DonateHmstr { get; set; } = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	public partial string DonateX { get; set; } = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	public partial string DonateCatizen { get; set; } = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	public partial string DonateMajor { get; set; } = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	public partial string UpdateLog { get; set; } = string.Empty;

	public TgMainViewModel(ITgSettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
	{
		VersionDescription = GetVersionDescription();
		AppVersionTitle =
			$"{TgConstants.AppTitleDesktop} " +
			$"v{TgCommonUtils.GetTrimVersion(Assembly.GetExecutingAssembly().GetName().Version)}";
		AppVersionShort = $"v{TgCommonUtils.GetTrimVersion(Assembly.GetExecutingAssembly().GetName().Version)}";
		AppVersionFull = $"{TgResourceExtensions.GetAppVersion()}: {AppVersionShort}";
		LicenseDescription = LicenseManager.CurrentLicense.Description;
	}

	#endregion

	#region Public and private methods

	public override async Task OnNavigatedToAsync(NavigationEventArgs e) => await LoadDataAsync(async () =>
	{
		// Velopack installer update
		await VelopackUpdateAsync();
		await ReloadUiAsync();
	});

	private static string GetVersionDescription()
	{
		Version version;
		if (TgRuntimeHelper.IsMSIX)
		{
			var packageVersion = Package.Current.Id.Version;
			version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
		}
		else
		{
			version = Assembly.GetExecutingAssembly().GetName().Version!;
		}
		return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
	}

	/// <summary> Velopack installer update </summary>
	private async Task VelopackUpdateAsync()
	{
		UpdateLog = string.Empty;
		UpdateLog += $"{TgConstants.AppTitleDesktop} {TgAppSettingsHelper.Instance.AppVersion} started" + Environment.NewLine;

		UpdateLog += $"Checking updates on the link {TgConstants.LinkGitHub}..." + Environment.NewLine;
		var mgr = new UpdateManager(new GithubSource(TgConstants.LinkGitHub, string.Empty, prerelease: false));
		// Check for new version
		try
		{
			var newVersion = await mgr.CheckForUpdatesAsync();
			if (newVersion is null)
			{
				UpdateLog += "No update available" + Environment.NewLine;
				return;
			}
			// Download new version
			UpdateLog += "Download new version..." + Environment.NewLine;
			await mgr.DownloadUpdatesAsync(newVersion);
			//// Install new version and restart app
			//var prompt = AnsiConsole.Prompt(
			//	new SelectionPrompt<string>()
			//		.Title("Install new version and restart app?")
			//		.PageSize(Console.WindowHeight - 5)
			//		.MoreChoicesText(TgLocale.MoveUpDown)
			//		.AddChoices(TgLocale.MenuNo, TgLocale.MenuYes));
			//var isInstall = prompt.Equals(TgLocale.MenuYes);
			//if (isInstall)
			//	mgr.ApplyUpdatesAndRestart(newVersion);
		}
		// Cannot perform this operation in an application which is not installed
		catch (Exception ex)
		{
			UpdateLog += ex.Message + Environment.NewLine;
		}
	}

	#endregion
}
