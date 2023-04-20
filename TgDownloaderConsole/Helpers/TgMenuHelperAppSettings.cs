// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	private TgMenuAppSettings SetMenuApp()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgConstants.MenuSwitchNumber}")
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					TgConstants.MenuMainReturn,
					TgConstants.MenuMainReset,
					TgConstants.MenuAppFileSession,
					TgConstants.MenuAppFileStorage,
					TgConstants.MenuAppUseProxy
			));
		return prompt switch
		{
			TgConstants.MenuMainReset => TgMenuAppSettings.Reset,
			TgConstants.MenuAppFileSession => TgMenuAppSettings.SetFileSession,
			TgConstants.MenuAppFileStorage => TgMenuAppSettings.SetFileStorage,
			TgConstants.MenuAppUseProxy => TgMenuAppSettings.SetUseProxy,
			_ => TgMenuAppSettings.Return
		};
	}

	public void SetupAppSettings(TgDownloadSettingsModel tgDownloadSettings)
	{
		TgMenuAppSettings menu;
		do
		{
			ShowTableAppSettings(tgDownloadSettings);
			menu = SetMenuApp();
			switch (menu)
			{
				case TgMenuAppSettings.Reset:
					ResetAppSettings();
					break;
				case TgMenuAppSettings.SetFileSession:
					SetFileSession();
					break;
				case TgMenuAppSettings.SetFileStorage:
					SetFileStorage();
					break;
				case TgMenuAppSettings.SetUseProxy:
					SetUseProxy();
					AskClientConnect(tgDownloadSettings);
					break;
			}
		} while (menu is not TgMenuAppSettings.Return);
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
			TgLog.GetLineStampInfo($"{TgConstants.MenuAppFileSession}:")));
		SetFileAppSettings();
	}

	private void SetFileStorage()
	{
		TgAppSettings.AppXml.SetFileStoragePath(AnsiConsole.Ask<string>(
			TgLog.GetLineStampInfo($"{TgConstants.MenuAppFileStorage}:")));
		SetFileAppSettings();
	}

	private void SetUseProxy()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgConstants.MenuSwitchNumber}")
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(TgConstants.MenuAppUseProxyDisable, TgConstants.MenuAppUseProxyEnable));
		TgAppSettings.AppXml.IsUseProxy = prompt switch
		{
			TgConstants.MenuAppUseProxyEnable => true,
			_ => false
		};
		SetFileAppSettings();
	}

	#endregion
}