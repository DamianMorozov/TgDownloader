// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgSettingsViewModel : TgBaseViewModel
{
	private bool _isInitialized;

	public string AppVersion { get; set; } = string.Empty;

	public Wpf.Ui.Appearance.ThemeType CurrentTheme { get; set; } = Wpf.Ui.Appearance.ThemeType.Unknown;

	public override void OnNavigatedTo()
	{
		if (!_isInitialized)
			InitializeViewModel();
	}

	private void InitializeViewModel()
	{
		CurrentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();
		AppVersion = $"{Locale.AppVersion}: {Assembly.GetExecutingAssembly().GetName().Version}";
		_isInitialized = true;
	}

	private string GetAssemblyVersion()
	{
		return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
	}

	[RelayCommand]
	private void OnChangeTheme(string parameter)
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
