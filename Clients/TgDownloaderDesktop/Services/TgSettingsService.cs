// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Services;

[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgSettingsService : ITgSettingsService
{
	#region Public and private fields, properties, constructor

	private const string SettingsKeyTheme = "AppBackgroundRequestedTheme";
	private const string SettingsKeyEfStorage = "EfStorage ";
	private const string SettingsKeyTgSession = "TgSession";
	public ElementTheme Theme { get; set; }
	public string EfStorage { get; set; } = default!;
	public string TgSession { get; set; } = default!;

	private const string DefaultApplicationDataFolder = "TgDownloaderDesktop/ApplicationData";
	private const string DefaultLocalSettingsFile = "TgLocalSettings.json";
	private readonly IFileService _fileService;
	private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
	private readonly string _applicationDataFolder;
	private readonly string _localSettingsFile;
	private IDictionary<string, object> _settings;
	private bool _isInitialized;
	private LocalSettingsOptions _options;

	public TgSettingsService(IFileService fileService, IOptions<LocalSettingsOptions> options)
	{
		_fileService = fileService;
		_options = options.Value;
		_applicationDataFolder = Path.Combine(_localApplicationData, _options.ApplicationDataFolder ?? DefaultApplicationDataFolder);
		_localSettingsFile = _options.LocalSettingsFile ?? DefaultLocalSettingsFile;
		_settings = new Dictionary<string, object>();
		Default();
	}

	#endregion

	#region Public and private methods

	public string ToDebugString() =>
		$"{nameof(Theme)}: {Theme} | {nameof(EfStorage)}: {EfStorage} | {nameof(TgSession)}: {TgSession}";

	public async Task SetThemeAsync(ElementTheme theme)
	{
		Theme = theme;
		await SetRequestedThemeAsync();
		await SaveThemeInSettingsAsync(Theme);
	}

	public async Task SetEfStorageAsync(string efStorage)
	{
		EfStorage = efStorage;
		await SaveEfStorageInSettingsAsync(EfStorage);
	}

	public async Task SetTgSessionAsync(string tgSession)
	{
		TgSession = tgSession;
		await SaveTgSessionInSettingsAsync(TgSession);
	}

	public async Task SetRequestedThemeAsync()
	{
		if (App.MainWindow.Content is FrameworkElement rootElement)
		{
			rootElement.RequestedTheme = Theme;
			TitleBarHelper.UpdateTitleBar(Theme);
		}
		await Task.CompletedTask;
	}

	private async Task<ElementTheme> LoadThemeFromSettingsAsync()
	{
		var themeName = await ReadSettingAsync<string>(SettingsKeyTheme);
		if (Enum.TryParse(themeName, out ElementTheme cacheTheme))
		{
			return cacheTheme;
		}
		return ElementTheme.Default;
	}

	private async Task<string> LoadEfStorageFromSettingsAsync() => await ReadSettingAsync<string>(SettingsKeyEfStorage) ?? TgFileUtils.FileEfStorage;

	private async Task<string> LoadTgSessionFromSettingsAsync() => await ReadSettingAsync<string>(SettingsKeyTgSession) ?? TgFileUtils.FileTgSession;

	private async Task SaveThemeInSettingsAsync(ElementTheme theme) => await SaveSettingAsync(SettingsKeyTheme, theme.ToString());

	private async Task SaveEfStorageInSettingsAsync(string efStorage) => await SaveSettingAsync(SettingsKeyEfStorage, efStorage);

	private async Task SaveTgSessionInSettingsAsync(string tgSession) => await SaveSettingAsync(SettingsKeyTgSession, tgSession);

	public void Default()
	{
		Theme = ElementTheme.Default;
		EfStorage = TgFileUtils.FileEfStorage;
		TgSession = TgFileUtils.FileTgSession;
#if DEBUG
		EfStorage = _options.EfStorage ?? TgFileUtils.FileEfStorage;
		TgSession = _options.TgSession ?? TgFileUtils.FileTgSession;
#endif
	}

	public async Task LoadAsync()
	{
		Theme = await LoadThemeFromSettingsAsync();
		EfStorage = await LoadEfStorageFromSettingsAsync();
		TgSession = await LoadTgSessionFromSettingsAsync();
		await Task.CompletedTask;
	}

	public async Task SaveAsync()
	{
		await SaveThemeInSettingsAsync(Theme);
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
