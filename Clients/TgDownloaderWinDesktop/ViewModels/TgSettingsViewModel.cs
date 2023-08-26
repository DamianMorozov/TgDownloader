// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSettingsViewModel : TgPageViewModelBase, INavigationAware
{
    public static Wpf.Ui.Appearance.ThemeType CurrentTheme { get; set; } = Wpf.Ui.Appearance.ThemeType.Unknown;
    public static string AppVersionTitle { get; set; } = string.Empty;
    public static string AppVersionFull { get; set; } = string.Empty;
	
    public void OnNavigatedTo()
	{
		if (!IsInitialized)
			InitializeViewModel();
	}

	public void OnNavigatedFrom()
	{
		//
	}

	protected override void InitializeViewModel()
	{
		base.InitializeViewModel();
        CurrentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();
        AppVersionTitle = $"{TgDesktopUtils.TgLocale.AppTitleWinDesktop} " +
                          $"v{TgCommonUtils.GetTrimVersion(Assembly.GetExecutingAssembly().GetName().Version)}";
        AppVersionFull = $"{TgDesktopUtils.TgLocale.AppVersion}: v{TgCommonUtils.GetTrimVersion(Assembly.GetExecutingAssembly().GetName().Version)}";
    }

	private string GetAssemblyVersion()
	{
		return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
	}

	[RelayCommand]
	public void OnChangeTheme(string parameter)
	{
		switch (parameter)
		{
			case "theme_light":
				if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Light)
					break;
				Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
				CurrentTheme = Wpf.Ui.Appearance.ThemeType.Light;
				break;
			default:
				if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Dark)
					break;
				Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
				CurrentTheme = Wpf.Ui.Appearance.ThemeType.Dark;
				break;
		}
	}
}