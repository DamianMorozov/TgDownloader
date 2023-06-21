// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

using System.Text;

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	private TgEnumMenuClient SetMenuClient()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgLocale.MenuSwitchNumber}")
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					TgLocale.MenuMainReturn,
					TgLocale.MenuSetProxy,
					TgLocale.MenuClientConnect,
					TgLocale.MenuClientDisconnect));
		if (prompt.Equals(TgLocale.MenuSetProxy)) return TgEnumMenuClient.SetProxy;
		if (prompt.Equals(TgLocale.MenuClientConnect)) return TgEnumMenuClient.Connect;
		if (prompt.Equals(TgLocale.MenuClientDisconnect)) return TgEnumMenuClient.Disconnect;
		return TgEnumMenuClient.Return;
	}

	public void SetupClient(TgDownloadSettingsModel tgDownloadSettings)
	{
		TgEnumMenuClient menu;
		do
		{
			ShowTableClient(tgDownloadSettings);
			menu = SetMenuClient();
			switch (menu)
			{
				case TgEnumMenuClient.SetProxy:
					SetupClientProxy();
					AskClientConnect(tgDownloadSettings);
					break;
				case TgEnumMenuClient.Connect:
					ClientConnect(tgDownloadSettings);
					break;
				case TgEnumMenuClient.Disconnect:
					ClientDisconnect(tgDownloadSettings);
					break;
			}
		} while (menu is not TgEnumMenuClient.Return);
	}

	private TgSqlTableProxyModel AddOrUpdateProxy()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgLocale.MenuSwitchNumber}")
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					nameof(TgEnumProxyType.None),
					nameof(TgEnumProxyType.Http),
					nameof(TgEnumProxyType.Socks),
					nameof(TgEnumProxyType.MtProto)));
		TgSqlTableProxyModel proxy = new()
		{
			Type = prompt switch
			{
				nameof(TgEnumProxyType.Http) => TgEnumProxyType.Http,
				nameof(TgEnumProxyType.Socks) => TgEnumProxyType.Socks,
				nameof(TgEnumProxyType.MtProto) => TgEnumProxyType.MtProto,
				_ => TgEnumProxyType.None
			},
		};
		if (!Equals(proxy.Type, TgEnumProxyType.None))
		{
			proxy.HostName = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TypeTgProxyHostName}:"));
			proxy.Port = AnsiConsole.Ask<ushort>(TgLog.GetLineStampInfo($"{TgLocale.TypeTgProxyPort}:"));
		}
		TgSqlTableProxyModel proxyDb = ContextManager.ContextTableProxies.GetItem(proxy.Type, proxy.HostName, proxy.Port);
		if (proxyDb.IsNotExists)
			ContextManager.ContextTableProxies.AddOrUpdateItem(proxy);
		proxy = ContextManager.ContextTableProxies.GetItem(proxy.Type, proxy.HostName, proxy.Port);

		TgSqlTableAppModel app = ContextManager.ContextTableApps.GetCurrentItem();
		app.ProxyUid = proxy.Uid;
		ContextManager.ContextTableApps.AddOrUpdateItem(app);

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

		if (proxy.Type == TgEnumProxyType.MtProto)
		{
			string prompt = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title($"  {TgLocale.MenuSwitchNumber}")
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
		ContextManager.ContextTableProxies.AddOrUpdateItem(proxy);

		//SetupClientProxyCore();
	}

	private string? GetConsoleConfig(string what)
	{
		TgSqlTableAppModel appNew = ContextManager.ContextTableApps.NewItem();
		TgSqlTableAppModel app = ContextManager.ContextTableApps.GetCurrentItem();
		switch (what)
		{
			case "api_hash":
				string apiHash = !Equals(app.ApiHash, appNew.ApiHash)
					? TgDataFormatUtils.ParseGuidToString(app.ApiHash)
					: TgDataFormatUtils.ParseGuidToString(app.ApiHash = TgDataFormatUtils.ParseStringToGuid(
						AnsiConsole.Ask<string>(
							TgLog.GetLineStampInfo($"{TgLocale.TgSetupApiHash}:"))));
				app.ApiHash = TgDataFormatUtils.ParseStringToGuid(apiHash);
				ContextManager.ContextTableApps.AddOrUpdateItem(app);
				return apiHash;
			case "api_id": 
				string apiId = !Equals(app.ApiId, appNew.ApiId)
					? app.ApiId.ToString()
					: (app.ApiId = AnsiConsole.Ask<int>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupAppId}:")))
					.ToString();
				app.ApiId = int.Parse(apiId);
				ContextManager.ContextTableApps.AddOrUpdateItem(app);
				return apiId;
			case "phone_number":
				string phoneNumber = !Equals(app.PhoneNumber, appNew.PhoneNumber)
					? app.PhoneNumber
					: app.PhoneNumber = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupPhone}:"));
				app.PhoneNumber = phoneNumber;
				ContextManager.ContextTableApps.AddOrUpdateItem(app);
				return phoneNumber;
			case "verification_code":
				return AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgVerificationCode}:"));
			case "notifications":
				return AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupNotifications}:"));
			case "first_name":
				return AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupFirstName}:"));
			case "last_name":
				return AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupLastName}:"));
			case "session_pathname":
				string sessionPath = Path.Combine(Directory.GetCurrentDirectory(), TgAppSettings.AppXml.FileSession);
				return Encoding.UTF8.GetBytes(sessionPath).ToString();
			case "password":
				return AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupPassword}:"));
			case "session_key":
			case "server_address":
			case "device_model":
			case "system_version":
			case "app_version":
			case "system_lang_code":
			case "lang_pack":
			case "lang_code":
			default:
				return null;
		};
	}

	public void ClientConnectExists()
	{
		if (!ContextManager.ContextTableApps.GetValidXpLite(ContextManager.ContextTableApps.GetCurrentItem()).IsValid) 
			return;
		TgClient.ConnectSession(GetConsoleConfig, ContextManager.ContextTableApps.GetCurrentProxy());
	}

	private void AskClientConnect(TgDownloadSettingsModel tgDownloadSettings)
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title(TgLocale.MenuClientConnect)
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(TgLocale.MenuNo, TgLocale.MenuYes));
		bool isConnect = prompt.Equals(TgLocale.MenuYes);
		if (isConnect)
			ClientConnect(tgDownloadSettings);
	}

	public void ClientConnect(TgDownloadSettingsModel tgDownloadSettings)
	{
		ShowTableClient(tgDownloadSettings);
		TgClient.ConnectSession(GetConsoleConfig, ContextManager.ContextTableApps.GetCurrentProxy()); 
		if (TgClient.ClientException.IsExists || TgClient.ProxyException.IsExists)
			TgLog.MarkupInfo(TgLocale.TgClientSetupCompleteError);
		else
			TgLog.MarkupInfo(TgLocale.TgClientSetupCompleteSuccess);
		Console.ReadKey();
	}

	public void ClientDisconnect(TgDownloadSettingsModel tgDownloadSettings)
	{
		ShowTableClient(tgDownloadSettings);
		TgSqlTableAppModel app = ContextManager.ContextTableApps.GetCurrentItem();
		ContextManager.ContextTableApps.DeleteItem(app);
		TgClient.Disconnect();
	}

	#endregion
}