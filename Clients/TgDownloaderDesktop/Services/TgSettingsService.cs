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
	public partial ObservableCollection<TgEnumTheme> AppThemes { get; set; } = default!;
	[ObservableProperty]
	public partial TgEnumTheme AppTheme { get; set; } = TgEnumTheme.Default;
	[ObservableProperty]
	public partial ObservableCollection<TgEnumLanguage> AppLanguages { get; set; } = default!;
	[ObservableProperty]
	public partial TgEnumLanguage AppLanguage { get; set; } = TgEnumLanguage.Default;
	[ObservableProperty]
	public partial string AppFolder { get; set; } = default!;
	private string _appStorage = default!;
	public string AppStorage
	{
		get => _appStorage;
		set
		{
			SetProperty(ref _appStorage, value);
			OnPropertyChanged();
			IsExistsAppStorage = File.Exists(AppStorage);
		}
	}
	private string _appSession = default!;
	public string AppSession
	{
		get => _appSession;
		set
		{
			SetProperty(ref _appSession, value);
			IsExistsAppSession = File.Exists(AppSession);
			OnPropertyChanged();
		}
	}
	[ObservableProperty]
	public partial bool IsExistsAppStorage { get; set; }
	[ObservableProperty]
	public partial bool IsExistsAppSession { get; set; }

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

	private async Task<string> LoadAppStorageFromSettingsAsync() => await ReadSettingAsync<string>(SettingsKeyAppStorage) ?? TgEfUtils.FileEfStorage;

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
			rootElement.RequestedTheme = TgThemeUtils.GetElementTheme(AppTheme);
			TgTitleBarHelper.UpdateTitleBar(rootElement.RequestedTheme);
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
		AppFolder = TgDesktopUtils.LocalFolder;
		AppStorage = Path.Combine(TgDesktopUtils.LocalFolder, TgEfUtils.FileEfStorage);
		AppSession = Path.Combine(TgDesktopUtils.LocalFolder, TgFileUtils.FileTgSession);
	}

	public async Task LoadAsync()
	{
		var appTheme = await LoadAppThemeFromSettingsAsync();
		AppTheme = AppThemes.First(x => x == appTheme);
		var appLanguage = await LoadAppLanguageFromSettingsAsync();
		AppLanguage = AppLanguages.First(x => x == appLanguage);
		AppStorage = await LoadAppStorageFromSettingsAsync();
		if (!File.Exists(AppStorage))
			AppStorage = Path.Combine(TgDesktopUtils.LocalFolder, TgEfUtils.FileEfStorage);
		AppSession = await LoadAppSessionFromSettingsAsync();
		if (!File.Exists(AppSession))
			AppSession = Path.Combine(TgDesktopUtils.LocalFolder, TgFileUtils.FileTgSession);
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
		if (TgRuntimeHelper.IsMSIX)
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
		if (TgRuntimeHelper.IsMSIX)
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
