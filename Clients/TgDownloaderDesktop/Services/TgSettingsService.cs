// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
#pragma warning disable CA1416

namespace TgDownloaderDesktop.Services;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgSettingsService : ObservableRecipient, ITgSettingsService
{
	#region Public and private fields, properties, constructor

	private const string SettingsKeyAppTheme = nameof(LocalSettingsOptions.AppTheme);
	private const string SettingsKeyAppLanguage = nameof(LocalSettingsOptions.AppLanguage);
	private const string SettingsKeyAppStorage = nameof(LocalSettingsOptions.AppStorage);
	private const string SettingsKeyAppSession = nameof(LocalSettingsOptions.AppSession);
	[ObservableProperty]
	private ObservableCollection<TgEnumTheme> _appThemes = default!;
	[ObservableProperty]
	private TgEnumTheme _appTheme = TgEnumTheme.Default;
	[ObservableProperty]
	private ObservableCollection<TgEnumLanguage> _appLanguages = default!;
	[ObservableProperty]
	private TgEnumLanguage _appLanguage = TgEnumLanguage.Default;
	[ObservableProperty]
	private string _appFolder = default!;
	private string _appStorage = default!;
	public string AppStorage
	{
		get => _appStorage;
		set
		{
			SetProperty(ref _appStorage, value);
			OnPropertyChanged();
			IsExistsAppStorage = TgDesktopUtils.CheckFileStorageExists(AppStorage);
		}
	}
	private string _appSession = default!;
	public string AppSession
	{
		get => _appSession;
		set
		{
			SetProperty(ref _appSession, value);
			IsExistsAppSession = TgDesktopUtils.CheckFileStorageExists(AppSession);
			OnPropertyChanged();
		}
	}
	[ObservableProperty]
	private bool _isExistsAppStorage;
	[ObservableProperty]
	private bool _isExistsAppSession;

	private const string DefaultApplicationDataFolder = "TgDownloaderDesktop/ApplicationData";
	private const string DefaultLocalSettingsFile = "TgLocalSettings.json";
	private readonly IFileService _fileService;
	private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
	private readonly string _applicationDataFolder;
	private readonly string _localSettingsFile;
	private IDictionary<string, object> _settings;
	private bool _isInitialized;
	private readonly LocalSettingsOptions _options;

	public TgSettingsService(IFileService fileService, IOptions<LocalSettingsOptions> options)
	{
		_fileService = fileService;
		_options = options.Value;
		_applicationDataFolder = Path.Combine(_localApplicationData, _options.ApplicationDataFolder ?? DefaultApplicationDataFolder);
		_localSettingsFile = _options.LocalSettingsFile ?? DefaultLocalSettingsFile;
		_settings = new Dictionary<string, object>();
		AppThemes = [TgEnumTheme.Default, TgEnumTheme.Light, TgEnumTheme.Dark];
		AppLanguages = [TgEnumLanguage.Default, TgEnumLanguage.English, TgEnumLanguage.Russian];
		Default();
	}

	#endregion

	#region Public and private methods

	public string ToDebugString() => TgObjectUtils.ToDebugString(this);

	public async Task SetAppThemeAsync()
	{
		await Task.CompletedTask;
	}

	public async Task SetAppLanguageAsync()
	{
		await Task.CompletedTask;
	}

	private async Task<TgEnumTheme> LoadAppThemeFromSettingsAsync()
	{
		var themeName = await ReadSettingAsync<string>(SettingsKeyAppTheme);
		return Enum.TryParse(themeName, out TgEnumTheme cacheTheme) ? cacheTheme : TgEnumTheme.Default;
	}

	private async Task<string> LoadAppStorageFromSettingsAsync() => await ReadSettingAsync<string>(SettingsKeyAppStorage) ?? TgFileUtils.FileEfStorage;

	private async Task<string> LoadAppSessionFromSettingsAsync() => await ReadSettingAsync<string>(SettingsKeyAppSession) ?? TgFileUtils.FileTgSession;

	private async Task<TgEnumLanguage> LoadAppLanguageFromSettingsAsync()
	{
		var appLanguage = await ReadSettingAsync<string>(SettingsKeyAppLanguage) ?? nameof(TgEnumLanguage.Default);
		return TgEnumUtils.GetLanguageAsEnum(appLanguage);
	}

	public void ApplyTheme(TgEnumTheme appTheme)
	{
		if (App.MainWindow.Content is FrameworkElement rootElement)
		{
			rootElement.RequestedTheme = TgThemeHelper.GetElementTheme(AppTheme);
			TitleBarHelper.UpdateTitleBar(rootElement.RequestedTheme);
		}
	}

	private async Task SaveAppThemeInSettingsAsync(TgEnumTheme appTheme)
	{
		ApplyTheme(appTheme);
		await SaveSettingAsync(SettingsKeyAppTheme, appTheme.ToString());
	}

	private async Task SaveAppLanguageInSettingsAsync(TgEnumLanguage appLanguage)
	{
		Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = TgEnumUtils.GetLanguageAsString(AppLanguage);
		await SaveSettingAsync(SettingsKeyAppLanguage, appLanguage.ToString());
	}

	private async Task SaveAppStorageInSettingsAsync(string appStorage) => await SaveSettingAsync(SettingsKeyAppStorage, appStorage);

	private async Task SaveAppSessionInSettingsAsync(string appSession) => await SaveSettingAsync(SettingsKeyAppSession, appSession);

	public void Default()
	{
		AppTheme = AppThemes.First(x => x == TgEnumTheme.Default);
		AppLanguage = AppLanguages.First(x => x == TgEnumLanguage.Default);
		AppStorage = Path.Combine(TgDesktopUtils.LocalFolder, TgFileUtils.FileEfStorage);
		AppSession = Path.Combine(TgDesktopUtils.LocalFolder, TgFileUtils.FileTgSession);
		AppFolder = TgDesktopUtils.LocalFolder;
	}

	public async Task LoadAsync()
	{
		var appTheme = await LoadAppThemeFromSettingsAsync();
		AppTheme = AppThemes.First(x => x == appTheme);
		var appLanguage = await LoadAppLanguageFromSettingsAsync();
		AppLanguage = AppLanguages.First(x => x == appLanguage);
		AppStorage = await LoadAppStorageFromSettingsAsync();
		AppSession = await LoadAppSessionFromSettingsAsync();
	}

	public async Task SaveAsync()
	{
		await SaveAppThemeInSettingsAsync(AppTheme);
		await SaveAppLanguageInSettingsAsync(AppLanguage);
		await SaveAppStorageInSettingsAsync(AppStorage);
		await SaveAppSessionInSettingsAsync(AppSession);
	}

	private async Task InitializeAsync()
	{
		if (!_isInitialized)
		{
			_settings = await Task.Run(() => _fileService.Read<IDictionary<string, object>>(_applicationDataFolder, _localSettingsFile)) ?? new Dictionary<string, object>();
			_isInitialized = true;
		}
	}

	public async Task<T?> ReadSettingAsync<T>(string key)
	{
		if (RuntimeHelper.IsMSIX)
		{
			if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var obj))
			{
				return await Json.ToObjectAsync<T>((string)obj);
			}
		}
		else
		{
			await InitializeAsync();
			if (_settings != null && _settings.TryGetValue(key, out var obj))
			{
				return await Json.ToObjectAsync<T>((string)obj);
			}
		}
		return default;
	}

	public async Task SaveSettingAsync<T>(string key, T value)
	{
		if (RuntimeHelper.IsMSIX)
		{
			ApplicationData.Current.LocalSettings.Values[key] = await Json.StringifyAsync(value);
		}
		else
		{
			await InitializeAsync();
			_settings[key] = await Json.StringifyAsync(value);
			await Task.Run(() => _fileService.Save(_applicationDataFolder, _localSettingsFile, _settings));
		}
	}

	#endregion
}
