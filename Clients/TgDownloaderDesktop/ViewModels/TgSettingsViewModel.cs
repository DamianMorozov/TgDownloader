// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TL;

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public partial class TgSettingsViewModel : TgPageViewModelBase
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private IThemeSelectorService _themeSelectorService;
	[ObservableProperty]
	private ElementTheme _elementTheme;
	[ObservableProperty]
	private ObservableCollection<string> _appThemes;
	[ObservableProperty]
	private string _currentTheme = default!;
	[ObservableProperty]
	private string _xmlEfStorage = default!;
	[ObservableProperty]
	private bool _isExistsEfStorage;
	[ObservableProperty]
	private string _xmlFileSession = default!;
	[ObservableProperty]
	private bool _isExistsFileSession;

	public ICommand SettingsDefaultCommand { get; }
	public ICommand SettingsSaveCommand { get; }
	public ICommand SwitchThemeCommand { get; }

	public TgSettingsViewModel(IThemeSelectorService themeSelectorService)
	{
		ThemeSelectorService = themeSelectorService;
		ElementTheme = ThemeSelectorService.Theme;
		AppThemes =
			[ ResourceExtensions.GetSettingsThemeLight(), ResourceExtensions.GetSettingsThemeDark(), ResourceExtensions.GetSettingsThemeDefault() ];
		CurrentTheme = ElementTheme.ToString();
		LoadXmlSettings();

		SwitchThemeCommand = new RelayCommand<ElementTheme>(async theme => await SwitchThemeAsync(theme));
		SettingsDefaultCommand = new RelayCommand(async () => await SettingsDefaultAsync());
		SettingsSaveCommand = new RelayCommand(async () => await SettingsSaveAsync());
	}

	#endregion

	#region Public and private methods

	public async Task SwitchThemeAsync()
	{
		var theme = ElementTheme.Default;
		if (CurrentTheme == ResourceExtensions.GetSettingsThemeLight())
			theme = ElementTheme.Light;
		else if (CurrentTheme == ResourceExtensions.GetSettingsThemeDark())
			theme = ElementTheme.Dark;
		await SwitchThemeAsync(theme);
	}

	public async Task SwitchThemeAsync(ElementTheme theme)
	{
		if (ElementTheme != theme)
		{
			ElementTheme = theme;
			await ThemeSelectorService.SetThemeAsync(theme);
			CurrentTheme = theme.ToString();
		}
	}

	private async Task SettingsDefaultAsync()
	{
		if (XamlRootVm is null) return;
		ContentDialog dialog = new()
		{
			XamlRoot = XamlRootVm,
			Title = ResourceExtensions.AskSettingsDefault(),
			PrimaryButtonText = ResourceExtensions.GetYesButton(),
			CloseButtonText = ResourceExtensions.GetCancelButton(),
			DefaultButton = ContentDialogButton.Close,
			PrimaryButtonCommand = new RelayCommand(() =>
			{
				TgAppSettings.DefaultXmlSettings();
				LoadXmlSettings();
			})
		};
		_ = await dialog.ShowAsync();
	}

	private async Task SettingsSaveAsync()
	{
		if (XamlRootVm is null) return;
		ContentDialog dialog = new()
		{
			XamlRoot = XamlRootVm,
			Title = ResourceExtensions.AskAskSettingsSave(),
			PrimaryButtonText = ResourceExtensions.GetYesButton(),
			CloseButtonText = ResourceExtensions.GetCancelButton(),
			DefaultButton = ContentDialogButton.Close,
			PrimaryButtonCommand = new RelayCommand(() =>
			{
				TgAppSettings.AppXml.XmlEfStorage = XmlEfStorage;
				TgAppSettings.AppXml.XmlFileSession = XmlFileSession;
				TgAppSettings.StoreXmlSettingsUnsafe();
				LoadXmlSettings();
			})
		};
		_ = await dialog.ShowAsync();
	}

	private void LoadXmlSettings()
	{
		TgAppSettings.LoadXmlSettings();
		XmlEfStorage = TgAppSettings.AppXml.XmlEfStorage;
		IsExistsEfStorage = TgAppSettings.AppXml.IsExistsEfStorage;
		XmlFileSession = TgAppSettings.AppXml.XmlFileSession;
		IsExistsFileSession = TgAppSettings.AppXml.IsExistsFileSession;
	}

	#endregion
}
