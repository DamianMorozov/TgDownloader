// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
	private readonly IThemeSelectorService _themeSelectorService;

	[ObservableProperty]
	private ElementTheme _elementTheme;

	[ObservableProperty]
	private string _versionDescription;

	public ICommand SwitchThemeCommand
	{
		get;
	}

	public SettingsViewModel(IThemeSelectorService themeSelectorService)
	{
		_themeSelectorService = themeSelectorService;
		_elementTheme = _themeSelectorService.Theme;
		_versionDescription = GetVersionDescription();

		SwitchThemeCommand = new RelayCommand<ElementTheme>(
			async (param) =>
			{
				if (ElementTheme != param)
				{
					ElementTheme = param;
					await _themeSelectorService.SetThemeAsync(param);
				}
			});
	}

	private static string GetVersionDescription()
	{
		Version version;

		if (RuntimeHelper.IsMSIX)
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
}
