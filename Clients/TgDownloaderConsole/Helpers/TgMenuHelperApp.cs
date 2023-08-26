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
					TgLocale.MenuAppFileStorage,
					TgLocale.MenuAppUseProxy
			));
		if (prompt.Equals(TgLocale.MenuMainReset))
			return TgEnumMenuAppSettings.Reset;
		if (prompt.Equals(TgLocale.MenuAppFileSession))
			return TgEnumMenuAppSettings.SetFileSession;
		if (prompt.Equals(TgLocale.MenuAppFileStorage))
			return TgEnumMenuAppSettings.SetFileStorage;
		if (prompt.Equals(TgLocale.MenuAppUseProxy))
			return TgEnumMenuAppSettings.SetUseProxy;
		return TgEnumMenuAppSettings.Return;
	}

	public void SetupAppSettings(TgDownloadSettingsModel tgDownloadSettings)
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
				case TgEnumMenuAppSettings.SetFileStorage:
					SetFileStorage();
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
		ContextManager.CreateOrConnectDb(true);
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

	private void SetFileStorage()
	{
		TgAppSettings.AppXml.SetFileStoragePath(AnsiConsole.Ask<string>(
			TgLog.GetLineStampInfo($"{TgLocale.MenuAppFileStorage}:")));
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
		TgAppSettings.AppXml.IsUseProxy = prompt.Equals(TgLocale.MenuAppUseProxyEnable);
		SetFileAppSettings();
	}

	#endregion
}