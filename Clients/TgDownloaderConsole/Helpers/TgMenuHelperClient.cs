// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	private TgEnumMenuClient SetMenuClient()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgLocale.MenuSwitchNumber}")
				.PageSize(Console.WindowHeight - 17)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					TgLocale.MenuMainReturn,
					TgLocale.MenuSetProxy,
					TgLocale.MenuClientConnect,
					TgLocale.MenuClientDisconnect));
		if (prompt.Equals(TgLocale.MenuSetProxy))
			return TgEnumMenuClient.SetProxy;
		if (prompt.Equals(TgLocale.MenuClientConnect))
			return TgEnumMenuClient.Connect;
		if (prompt.Equals(TgLocale.MenuClientDisconnect))
			return TgEnumMenuClient.Disconnect;
		return TgEnumMenuClient.Return;
	}

	public async Task SetupClientAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		TgEnumMenuClient menu;
		do
		{
			ShowTableClient(tgDownloadSettings);
			menu = SetMenuClient();
			switch (menu)
			{
				case TgEnumMenuClient.SetProxy:
					SetupClientProxyAsync().GetAwaiter().GetResult();
					await AskClientConnectAsync(tgDownloadSettings);
					break;
				case TgEnumMenuClient.Connect:
					await ClientConnectAsync(tgDownloadSettings, false);
					break;
				case TgEnumMenuClient.Disconnect:
					ClientDisconnect(tgDownloadSettings);
					break;
			}
		} while (menu is not TgEnumMenuClient.Return);
	}

	private async Task<TgEfProxyEntity> AddOrUpdateProxyAsync()
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title($"  {TgLocale.MenuSwitchNumber}")
				.PageSize(Console.WindowHeight - 17)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(
					nameof(TgEnumProxyType.None),
					nameof(TgEnumProxyType.Http),
					nameof(TgEnumProxyType.Socks),
					nameof(TgEnumProxyType.MtProto)));
		TgEfProxyEntity proxy = new()
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
		TgEfStorageResult<TgEfProxyEntity> storageResult = await ProxyRepository.GetAsync(
			new TgEfProxyEntity { Type = proxy.Type, HostName = proxy.HostName, Port = proxy.Port}, isNoTracking: false);
		if (!storageResult.IsExists)
			await ProxyRepository.SaveAsync(proxy);
		proxy = (await ProxyRepository.GetAsync(
			new TgEfProxyEntity { Type = proxy.Type, HostName = proxy.HostName, Port = proxy.Port}, isNoTracking: false)).Item;

		TgEfAppEntity app = (await AppRepository.GetFirstAsync(isNoTracking: false)).Item;
		app.ProxyUid = proxy.Uid;
		await AppRepository.SaveAsync(app);

		return proxy;
	}

	private async Task SetupClientProxyAsync()
	{
		TgEfProxyEntity proxy = await AddOrUpdateProxyAsync();

		if (proxy.Type == TgEnumProxyType.MtProto)
		{
			string prompt = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title($"  {TgLocale.MenuSwitchNumber}")
					.PageSize(Console.WindowHeight - 17)
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
        await ProxyRepository.SaveAsync(proxy);
		//SetupClientProxyCore();
	}

	private string? ConfigConsole(string what)
	{
		TgEfAppEntity appNew = AppRepository.GetNew(isNoTracking: false).Item;
		TgEfAppEntity app = AppRepository.GetFirst(isNoTracking: false).Item;
		switch (what)
		{
			case "api_hash":
				string apiHash = !Equals(app.ApiHash, appNew.ApiHash)
					? TgDataFormatUtils.ParseGuidToString(app.ApiHash)
					: TgDataFormatUtils.ParseGuidToString(TgDataFormatUtils.ParseStringToGuid(
						AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupApiHash}:"))));
				if (app.ApiHash != TgDataFormatUtils.ParseStringToGuid(apiHash))
				{
					app.ApiHash = TgDataFormatUtils.ParseStringToGuid(apiHash);
					AppRepository.Save(app);
				}
				return apiHash;
			case "api_id":
				string apiId = !Equals(app.ApiId, appNew.ApiId)
					? app.ApiId.ToString()
					: AnsiConsole.Ask<int>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupAppId}:"))
					.ToString();
				if (app.ApiId != int.Parse(apiId))
				{
					app.ApiId = int.Parse(apiId);
					AppRepository.Save(app);
				}
				return apiId;
			case "phone_number":
				string phoneNumber = !Equals(app.PhoneNumber, appNew.PhoneNumber)
					? app.PhoneNumber
					: AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupPhone}:"));
				if (app.PhoneNumber != phoneNumber)
				{
					app.PhoneNumber = phoneNumber;
					AppRepository.Save(app);
				}
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
				if (string.IsNullOrEmpty(TgAppSettings.AppXml.XmlFileSession))
					TgAppSettings.AppXml.XmlFileSession = TgFileUtils.FileSession;
				string sessionPath = Path.Combine(Directory.GetCurrentDirectory(), TgAppSettings.AppXml.XmlFileSession);
				return sessionPath;
			case "password":
				return AnsiConsole.Ask<string>(TgLog.GetLineStampInfo($"{TgLocale.TgSetupPassword}:"));
			//case "session_key":
			//case "server_address":
			//case "device_model":
			//case "system_version":
			//case "app_version":
			//case "system_lang_code":
			//case "lang_pack":
			//case "lang_code":
			//case "init_params":
			default:
				return null;
		}
	}

	public async Task ClientConnectConsoleAsync() => 
		await TgClient.ConnectSessionConsoleAsync(ConfigConsole, (await ProxyRepository.GetCurrentProxyAsync(
			await AppRepository.GetCurrentAppAsync())).Item);

	private async Task AskClientConnectAsync(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		string prompt = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title(TgLocale.MenuClientConnect)
				.PageSize(Console.WindowHeight - 17)
				.MoreChoicesText(TgLocale.MoveUpDown)
				.AddChoices(TgLocale.MenuNo, TgLocale.MenuYes));
		bool isConnect = prompt.Equals(TgLocale.MenuYes);
		if (isConnect)
			await ClientConnectAsync(tgDownloadSettings, false);
	}

	public async Task ClientConnectAsync(TgDownloadSettingsViewModel tgDownloadSettings, bool isSilent)
	{
		ShowTableClient(tgDownloadSettings);
		await TgClient.ConnectSessionConsoleAsync(ConfigConsole, (
			await ProxyRepository.GetCurrentProxyAsync(await AppRepository.GetCurrentAppAsync())).Item);
		if (TgClient.ClientException.IsExist || TgClient.ProxyException.IsExist)
			TgLog.MarkupInfo(TgLocale.TgClientSetupCompleteError);
		else
			TgLog.MarkupInfo(TgLocale.TgClientSetupCompleteSuccess);
		if (!isSilent)
		    Console.ReadKey();
	}

	public void ClientDisconnect(TgDownloadSettingsViewModel tgDownloadSettings)
	{
		ShowTableClient(tgDownloadSettings);
		//TgSqlTableAppModel app = ContextManager.AppRepository.GetFirst();
		//ContextManager.AppRepository.Delete(app);
		TgClient.Disconnect();
	}

	#endregion
}