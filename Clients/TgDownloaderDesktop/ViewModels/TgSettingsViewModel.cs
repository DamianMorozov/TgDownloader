// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public partial class TgSettingsViewModel : TgPageViewModelBase
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private ITgSettingsService _settingsService;
	[ObservableProperty]
	private ElementTheme _elementTheme;
	[ObservableProperty]
	private ObservableCollection<string> _appThemes;
	[ObservableProperty]
	private string _appTheme = default!;
	private string _appEfStorage = default!;
	public string AppEfStorage
	{
		get => _appEfStorage;
		set
		{
			if (SetProperty(ref _appEfStorage, value))
			{
				SetEfStorageAsync().ConfigureAwait(false);
			}
			IsExistsEfStorage = File.Exists(value);
		}
	}
	[ObservableProperty]
	private bool _isExistsEfStorage;
	private string _appTgSession = default!;
	public string AppTgSession
	{
		get => _appTgSession;
		set
		{
			if (SetProperty(ref _appTgSession, value))
			{
				SetTgSessionAsync().ConfigureAwait(false);
			}
			IsExistsTgSession = File.Exists(value);
		}
	}
	[ObservableProperty]
	private bool _isExistsTgSession;

	public ICommand SettingsDefaultCommand { get; }
	public ICommand SwitchThemeCommand { get; }

	public TgSettingsViewModel(ITgSettingsService settingsService)
	{
		SettingsService = settingsService;
		AppThemes =
			[ResourceExtensions.GetSettingsThemeLight(), ResourceExtensions.GetSettingsThemeDark(), ResourceExtensions.GetSettingsThemeDefault()];
		Load();

		SettingsDefaultCommand = new RelayCommand(async () => await SettingsDefaultAsync());
		SwitchThemeCommand = new RelayCommand<ElementTheme>(async theme => await SetThemeAsync(theme));
	}

	#endregion

	#region Public and private methods

	private void Load()
	{
		ElementTheme = SettingsService.Theme;
		AppTheme = ElementTheme.ToString();
		AppEfStorage = SettingsService.EfStorage;
		AppTgSession = SettingsService.TgSession;
	}

	public async Task SetThemeAsync()
	{
		var theme = ElementTheme.Default;
		if (AppTheme == ResourceExtensions.GetSettingsThemeLight())
			theme = ElementTheme.Light;
		else if (AppTheme == ResourceExtensions.GetSettingsThemeDark())
			theme = ElementTheme.Dark;
		await SetThemeAsync(theme);
	}

	public async Task SetThemeAsync(ElementTheme theme)
	{
		if (ElementTheme != theme)
		{
			ElementTheme = theme;
			await SettingsService.SetThemeAsync(ElementTheme);
			AppTheme = ElementTheme.ToString();
		}
	}

	public async Task SetEfStorageAsync()
	{
		await SettingsService.SetEfStorageAsync(AppEfStorage);
		await Task.CompletedTask;
	}

	public async Task SetTgSessionAsync()
	{
		await SettingsService.SetTgSessionAsync(AppTgSession);
		await Task.CompletedTask;
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
				SettingsService.Default();
				Load();
			})
		};
		_ = await dialog.ShowAsync();
	}

	#endregion
}
