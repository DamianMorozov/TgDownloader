﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Helpers;

public static class TgResourceExtensions
{
    #region Public and private fields, properties, constructor

    private static ResourceLoader LocalResourceLoader { get; } = new();

    public static string GetLocalized(this string resourceKey) => LocalResourceLoader.GetString(resourceKey);

	#endregion

	#region Public and private methods

	public static string AskAskSettingsLoad() => "AskSettingsLoad".GetLocalized();
	public static string AskAskSettingsSave() => "AskSettingsSave".GetLocalized();
	public static string AskSettingsDefault() => "AskSettingsDefault".GetLocalized();
	public static string GetAppVersion() => "AppVersion".GetLocalized();
	public static string GetCancelButton() => "CancelButton".GetLocalized();
	public static string GetMenuClientIsQuery() => "MenuClientIsQuery".GetLocalized();
	public static string GetNoButton() => "NoButton".GetLocalized();
	public static string GetSettingsThemeDark() => "SettingsThemeDark".GetLocalized();
	public static string GetSettingsThemeDefault() => "SettingsThemeDefault".GetLocalized();
	public static string GetSettingsThemeLight() => "SettingsThemeLight".GetLocalized();
	public static string GetYesButton() => "YesButton".GetLocalized();

	#endregion
}