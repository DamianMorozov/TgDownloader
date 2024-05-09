// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	private TgEnumMenuAppSettings SetMenuApp()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgLocale.MenuSwitchNumber}")
				.PageSize(Console.WindowHeight - 17)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					TgLocale.MenuMainReturn,
					TgLocale.MenuMainReset,
					TgLocale.MenuAppFileSession,
					TgLocale.MenuAppEfStorage,
					TgLocale.MenuAppDeprecatedStorage,
					TgLocale.MenuAppUseProxy
			));
		if (prompt.Equals(TgLocale.MenuMainReset))
			return TgEnumMenuAppSettings.Reset;
		if (prompt.Equals(TgLocale.MenuAppFileSession))
			return TgEnumMenuAppSettings.SetFileSession;
		if (prompt.Equals(TgLocale.MenuAppEfStorage))
			return TgEnumMenuAppSettings.SetEfStorage;
		if (prompt.Equals(TgLocale.MenuAppDeprecatedStorage))
			return TgEnumMenuAppSettings.SetDepreactedStorage;
		if (prompt.Equals(TgLocale.MenuAppUseProxy))
			return TgEnumMenuAppSettings.SetUseProxy;
		return TgEnumMenuAppSettings.Return;
	}

	public void SetupAppSettings(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		TgEnumMenuAppSettings menu;
		do
		{
			ShowTableAppSettings(tgDownloadSettings);
			menu = SetMenuApp();
			switch (menu)
			{
				case TgEnumMenuAppSettings.Reset:
					ResetAppSettings();
					break;
				case TgEnumMenuAppSettings.SetFileSession:
					SetFileSession();
					break;
				case TgEnumMenuAppSettings.SetEfStorage:
					SetEfStorage();
					break;
				case TgEnumMenuAppSettings.SetDepreactedStorage:
					SetDeprecatedStorage();
					break;
				case TgEnumMenuAppSettings.SetUseProxy:
					SetUseProxy();
					AskClientConnect(tgDownloadSettings);
					break;
			}
		} while (menu is not TgEnumMenuAppSettings.Return);
	}

	private void SetFileAppSettings()
	{
		TgAppSettings.StoreXmlSettings();
	}

	private void ResetAppSettings()
	{
		TgAppSettings.AppXml = new();
		SetFileAppSettings();
	}

	private void SetFileSession()
	{
		TgAppSettings.AppXml.SetFileSessionPath(AnsiConsole.Ask<string>(
			TgLog.GetLineStampInfo($"{TgLocale.MenuAppFileSession}:")));
		SetFileAppSettings();
	}

	private void SetEfStorage()
	{
		TgAppSettings.AppXml.SetEfStoragePath(AnsiConsole.Ask<string>(
			TgLog.GetLineStampInfo($"{TgLocale.MenuAppEfStorage}:")));
		SetFileAppSettings();
	}

	private void SetDeprecatedStorage()
	{
		TgAppSettings.AppXml.SetDeprecatedStoragePath(AnsiConsole.Ask<string>(
			TgLog.GetLineStampInfo($"{TgLocale.MenuAppEfStorage}:")));
		SetFileAppSettings();
	}

	private void SetUseProxy()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgLocale.MenuSwitchNumber}")
				.PageSize(Console.WindowHeight - 17)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(TgLocale.MenuAppUseProxyDisable, TgLocale.MenuAppUseProxyEnable));
		TgAppSettings.IsUseProxy = prompt.Equals(TgLocale.MenuAppUseProxyEnable);
		SetFileAppSettings();
	}

	#endregion
}