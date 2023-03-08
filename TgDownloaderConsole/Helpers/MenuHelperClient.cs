// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Models.Apps;
using TgStorage.Models.Proxies;

namespace TgDownloaderConsole.Helpers;

internal partial class MenuHelper
{
	#region Public and private methods

	private MenuClient SetMenuClient()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title(TgLocale.MenuSwitchNumber)
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					TgLocale.MenuMainReturn,
					TgLocale.MenuSetProxy,
					TgLocale.MenuClientConnect,
					TgLocale.MenuClientGetInfo));
		return prompt switch
		{
			"Setup proxy" => MenuClient.SetProxy,
			"Connect the client to TG server" => MenuClient.Connect,
			"Get info" => MenuClient.GetInfo,
			_ => MenuClient.Return
		};
	}

	public void SetupClient(TgDownloadSettingsModel tgDownloadSettings)
	{
		MenuClient menu;
		do
		{
			ShowTableClient(tgDownloadSettings);
			menu = SetMenuClient();
			switch (menu)
			{
				case MenuClient.SetProxy:
					SetupClientProxy();
					AskClientConnect(tgDownloadSettings);
					break;
				case MenuClient.Connect:
					ClientConnect(tgDownloadSettings);
					break;
				case MenuClient.GetInfo:
					ClientGetInfo(tgDownloadSettings);
					break;
			}
		} while (menu is not MenuClient.Return);
	}

	private SqlTableProxyModel AddOrUpdateProxy()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title(TgLocale.MenuSwitchNumber)
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					nameof(ProxyType.None),
					nameof(ProxyType.Http),
					nameof(ProxyType.Socks),
					nameof(ProxyType.MtProto)));
		SqlTableProxyModel proxy = new()
		{
			Type = prompt switch
			{
				nameof(ProxyType.Http) => ProxyType.Http,
				nameof(ProxyType.Socks) => ProxyType.Socks,
				nameof(ProxyType.MtProto) => ProxyType.MtProto,
				_ => ProxyType.None
			},
		};
		if (!Equals(proxy.Type, ProxyType.None))
		{
			proxy.HostName = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TypeTgProxyHostName}:"));
			proxy.Port = AnsiConsole.Ask<ushort>(TgLog.GetLineStampInfo($"{TgLocale.TypeTgProxyPort}:"));
		}
		SqlTableProxyModel? proxyDb = TgStorage.GetProxyNullable(proxy.Type, proxy.HostName, proxy.Port);
		if (proxyDb is null)
			TgStorage.AddOrUpdateItem(proxy);
		proxy = TgStorage.GetProxy(proxy.Type, proxy.HostName, proxy.Port);

		SqlTableAppModel app = TgStorage.App;
		app.ProxyUid = proxy.Uid;
		TgStorage.AddOrUpdateItem(app);

		return proxy;
	}

	private void SetupClientProxyCore()
	{
		SqlTableProxyModel proxy = TgStorage.GetProxy(
			TgStorage.Proxy.Type, TgStorage.Proxy.HostName, TgStorage.Proxy.Port);
		TgStorage.AddOrUpdateItem(proxy);

		SqlTableAppModel app = TgStorage.App;
		app.ProxyUid = proxy.Uid;
		TgStorage.AddOrUpdateItem(app);
	}

	private void SetupClientProxy()
	{
		SqlTableProxyModel proxy = AddOrUpdateProxy();

		if (proxy.Type == ProxyType.MtProto)
		{
			string prompt = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title(TgLocale.MenuSwitchNumber)
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
		TgStorage.AddOrUpdateItem(proxy);

		SetupClientProxyCore();
	}

	private string? GetConfigExists(string what) =>
		what switch
		{
			"api_id" => TgClient.ApiId = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupAppId}:")),
			"api_hash" => TgClient.ApiHash,
			"phone_number" => TgClient.PhoneNumber,
			"verification_code" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupCode}:")),
			"notifications" => AnsiConsole.Ask<bool>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupNotifications}:")).ToString(),
			"first_name" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupFirstName}:")),
			"last_name" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupLastName}:")),
			"session_pathname" => FileUtils.Session,
			"password" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupPassword}:")),
			_ => null
		};

	private string? GetConfigUser(string what) =>
		what switch
		{
			"api_id" => TgClient.ApiId = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupAppId}:")),
			"api_hash" => TgClient.ApiHash = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupApiHash}:")),
			"phone_number" => TgClient.PhoneNumber = AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupPhone}:")),
			"verification_code" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupCode}:")),
			"notifications" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupNotifications}:")),
			"first_name" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupFirstName}:")),
			"last_name" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupLastName}:")),
			"session_pathname" => AppSettings.AppXml.FileSession,
			"password" => AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupPassword}:")),
			_ => null
		};

	public void ClientConnectExists(TgDownloadSettingsModel tgDownloadSettings)
	{
		if (!TgStorage.IsValidXpLite(TgStorage.App)) return;
		TgClient.Connect(TgStorage.App, GetConfigExists, null, TgStorage.Proxy);
		if (TgClient.IsReady)
			TgClient.CollectAllChats().GetAwaiter().GetResult();
	}

	private void ClientConnectUser()
	{
		TgClient.UnLoginUser();

		if (TgStorage.IsValidXpLite(TgStorage.App))
			TgClient.Connect(TgStorage.App, GetConfigExists, null, TgStorage.Proxy);
		else
			TgClient.Connect(TgStorage.App, null, GetConfigUser, TgStorage.Proxy);

		if (TgClient.IsReady)
		{
			TgStorage.App.ProxyUid = TgStorage.Proxy.Uid;
			TgStorage.AddOrUpdateItem(TgStorage.App);
			TgClient.CollectAllChats().GetAwaiter().GetResult();
		}
	}

	private void AskClientConnect(TgDownloadSettingsModel tgDownloadSettings)
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title(TgLocale.MenuClientConnect)
				.PageSize(10)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(TgLocale.MenuNo, TgLocale.MenuYes));
		bool isConnect = prompt switch
		{
			"Yes" => true,
			_ => false
		};
		if (isConnect)
			ClientConnect(tgDownloadSettings);
	}

	public void ClientConnect(TgDownloadSettingsModel tgDownloadSettings)
	{
		ShowTableClient(tgDownloadSettings);
		ClientConnectUser();
		if (TgClient.ClientException.IsExists || TgClient.ProxyException.IsExists)
			TgLog.MarkupInfo(TgLocale.TgClientSetupCompleteError);
		else
			TgLog.MarkupInfo(TgLocale.TgClientSetupCompleteSuccess);
		Console.ReadKey();
	}

	public void ClientGetInfo(TgDownloadSettingsModel tgDownloadSettings)
	{
		ShowTableClient(tgDownloadSettings);
		if (!TgClient.IsReady)
		{
			TgLog.MarkupWarning(TgLocale.TgMustClientConnect);
			Console.ReadKey();
			return;
		}

		Dictionary<long, ChatBase> dicDialogs = TgClient.CollectAllDialogs();
		TgClient.PrintChatsInfo(dicDialogs, "dialogs");

		TgLog.MarkupInfo(TgLocale.TgGetInfoComplete);
		Console.ReadKey();
	}

	#endregion
}