﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Services;

public class ThemeSelectorService : IThemeSelectorService
{
	#region Public and private fields, properties, constructor

	private const string SettingsKey = "AppBackgroundRequestedTheme";

	public ElementTheme Theme { get; set; } = ElementTheme.Default;

	private readonly ILocalSettingsService _localSettingsService;

	public ThemeSelectorService(ILocalSettingsService localSettingsService)
	{
		_localSettingsService = localSettingsService;
	}

	#endregion

	#region Public and private methods

	public async Task InitializeAsync()
	{
		Theme = await LoadThemeFromSettingsAsync();
		await Task.CompletedTask;
	}

	public async Task SetThemeAsync(ElementTheme theme)
	{
		Theme = theme;
		await SetRequestedThemeAsync();
		await SaveThemeInSettingsAsync(Theme);
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
		var themeName = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

		if (Enum.TryParse(themeName, out ElementTheme cacheTheme))
		{
			return cacheTheme;
		}

		return ElementTheme.Default;
	}

	private async Task SaveThemeInSettingsAsync(ElementTheme theme)
	{
		await _localSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());
	}

	#endregion
}
