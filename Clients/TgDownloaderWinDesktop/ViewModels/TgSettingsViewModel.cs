// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSettingsViewModel : TgPageViewModelBase, INavigationAware
{
	public string AppVersion { get; set; } = string.Empty;

	public Wpf.Ui.Appearance.ThemeType CurrentTheme { get; set; } = Wpf.Ui.Appearance.ThemeType.Unknown;

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
		AppVersion = $"{TgDesktopUtils.TgLocale.AppVersion}: {Assembly.GetExecutingAssembly().GetName().Version}";
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