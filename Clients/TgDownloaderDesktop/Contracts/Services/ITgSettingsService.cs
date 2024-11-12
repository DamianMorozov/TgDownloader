// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Contracts.Services;

public interface ITgSettingsService
{
	ElementTheme Theme { get; }
	string EfStorage { get; }
	string TgSession { get; }
	string AppLanguage { get; }
	Task SetAppThemeAsync(ElementTheme theme);
	Task SetAppEfStorageAsync(string efStorage);
	Task SetAppTgSessionAsync(string tgSession);
	Task SetAppLanguageAsync(string appLanguage);
	Task SetRequestedThemeAsync();
	void Default();
	Task LoadAsync();
	Task SaveAsync();
	Task<T?> ReadSettingAsync<T>(string key);
	Task SaveSettingAsync<T>(string key, T value);
}
