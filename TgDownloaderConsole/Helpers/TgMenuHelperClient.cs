// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	private TgMenuClient SetMenuClient()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgConstants.MenuSwitchNumber}")
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					TgConstants.MenuMainReturn,
					TgConstants.MenuSetProxy,
					TgConstants.MenuClientConnect,
					TgConstants.MenuClientDisconnect));
		return prompt switch
		{
			TgConstants.MenuSetProxy => TgMenuClient.SetProxy,
			TgConstants.MenuClientConnect => TgMenuClient.Connect,
			TgConstants.MenuClientDisconnect => TgMenuClient.Disconnect,
			_ => TgMenuClient.Return
		};
	}

	public void SetupClient(TgDownloadSettingsModel tgDownloadSettings)
	{
		TgMenuClient menu;
		do
		{
			ShowTableClient(tgDownloadSettings);
			menu = SetMenuClient();
			switch (menu)
			{
				case TgMenuClient.SetProxy:
					SetupClientProxy();
					AskClientConnect(tgDownloadSettings);
					break;
				case TgMenuClient.Connect:
					ClientConnect(tgDownloadSettings);
					break;
				case TgMenuClient.Disconnect:
					ClientDisconnect(tgDownloadSettings);
					break;
			}
		} while (menu is not TgMenuClient.Return);
	}

	private TgSqlTableProxyModel AddOrUpdateProxy()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgConstants.MenuSwitchNumber}")
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					nameof(TgProxyType.None),
					nameof(TgProxyType.Http),
					nameof(TgProxyType.Socks),
					nameof(TgProxyType.MtProto)));
		TgSqlTableProxyModel proxy = new()
		{
			Type = prompt switch
			{
				nameof(TgProxyType.Http) => TgProxyType.Http,
				nameof(TgProxyType.Socks) => TgProxyType.Socks,
				nameof(TgProxyType.MtProto) => TgProxyType.MtProto,
				_ => TgProxyType.None
			},
		};
		if (!Equals(proxy.Type, TgProxyType.None))
		{
			proxy.HostName = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TypeTgProxyHostName}:"));
			proxy.Port = AnsiConsole.Ask<ushort>(TgLog.GetLineStampInfo($"{TgLocale.TypeTgProxyPort}:"));
		}
		TgSqlTableProxyModel proxyDb = ContextManager.Proxies.GetItem(proxy.Type, proxy.HostName, proxy.Port);
		if (proxyDb.IsNotExists)
			ContextManager.Proxies.AddOrUpdateItem(proxy);
		proxy = ContextManager.Proxies.GetItem(proxy.Type, proxy.HostName, proxy.Port);

		TgSqlTableAppModel app = ContextManager.Apps.GetCurrentItem();
		app.ProxyUid = proxy.Uid;
		ContextManager.Apps.AddOrUpdateItem(app);

		return proxy;
	}

	//private void SetupClientProxyCore()
	//{
	//	TgSqlTableProxyModel proxy = TgStorage.Proxies.GetItem(
	//		TgStorage.Apps.GetCurrentProxy.Type, TgStorage.Apps.GetCurrentProxy.HostName, TgStorage.Apps.GetCurrentProxy.Port);
	//	TgStorage.Proxies.AddOrUpdateItem(proxy);

	//	TgSqlTableAppModel app = TgStorage.Apps.GetItem();
	//	app.ProxyUid = proxy.Uid;
	//	TgStorage.Apps.AddOrUpdateItem(app);
	//}

	private void SetupClientProxy()
	{
		TgSqlTableProxyModel proxy = AddOrUpdateProxy();

		if (proxy.Type == TgProxyType.MtProto)
		{
			string prompt = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title($"  {TgConstants.MenuSwitchNumber}")
					.PageSize(10)
					.MoreChoicesText(TgLocale.MoveUpDown)
					.AddChoices("Use secret", "Do not use secret"));
			bool isSecret = prompt switch
			{
				"Use secret" => true,
				_ => false
			};
			if (isSecret)
				proxy.Secret = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TypeTgProxySecret}:"));
		}
		ContextManager.Proxies.AddOrUpdateItem(proxy);

		//SetupClientProxyCore();
	}

	private string? GetConfig(string what)
	{
		TgSqlTableAppModel appNew = ContextManager.Apps.NewItem();
		TgSqlTableAppModel app = ContextManager.Apps.GetCurrentItem();
		string? result = what switch
		{
			"api_hash" => !Equals(app.ApiHash, appNew.ApiHash) ? TgDataFormatUtils.ParseGuidToString(app.ApiHash)
				: TgDataFormatUtils.ParseGuidToString(app.ApiHash = TgDataFormatUtils.ParseStringToGuid(AnsiConsole.Ask<string>(
						TgLog.GetLineStampInfo($"{TgLocale.TgSetupApiHash}:")))),
			"api_id" => (app.ApiId = AnsiConsole.Ask<int>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupAppId}:"))).ToString(),
			"phone_number" => !Equals(app.PhoneNumber, appNew.PhoneNumber) ? app.PhoneNumber
				: app.PhoneNumber = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupPhone}:")),
			"verification_code" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupCode}:")),
			"notifications" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupNotifications}:")),
			"first_name" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupFirstName}:")),
			"last_name" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupLastName}:")),
			"session_pathname" => TgAppSettings.AppXml.FileSession,
			"password" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupPassword}:")),
			_ => null
		};
		switch (what)
		{
			case "api_hash":
			case "api_id":
			case "phone_number":
				ContextManager.Apps.AddOrUpdateItem(app);
				break;
		}
		return result;
	}

	public void ClientConnectExists()
	{
		if (!ContextManager.Apps.IsValidXpLite(ContextManager.Apps.GetCurrentItem())) return;
		TgClient.Connect(GetConfig, ContextManager.Apps.GetCurrentProxy);
		//if (TgClient.IsReady)
		//	TgClient.CollectAllChats();
	}

	private void AskClientConnect(TgDownloadSettingsModel tgDownloadSettings)
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title(TgConstants.MenuClientConnect)
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(TgConstants.MenuNo, TgConstants.MenuYes));
		bool isConnect = prompt switch
		{
			TgConstants.MenuYes => true,
			_ => false
		};
		if (isConnect)
			ClientConnect(tgDownloadSettings);
	}

	public void ClientConnect(TgDownloadSettingsModel tgDownloadSettings)
	{
		ShowTableClient(tgDownloadSettings);
		TgClient.Connect(GetConfig, ContextManager.Apps.GetCurrentProxy); 
		if (TgClient.ClientException.IsExists || TgClient.ProxyException.IsExists)
			TgLog.MarkupInfo(TgLocale.TgClientSetupCompleteError);
		else
			TgLog.MarkupInfo(TgLocale.TgClientSetupCompleteSuccess);
		Console.ReadKey();
	}

	public void ClientDisconnect(TgDownloadSettingsModel tgDownloadSettings)
	{
		ShowTableClient(tgDownloadSettings);
		TgSqlTableAppModel app = ContextManager.Apps.GetCurrentItem();
		ContextManager.Apps.DeleteItem(app);
		TgClient.UnLoginUser();
	}

	#endregion
}