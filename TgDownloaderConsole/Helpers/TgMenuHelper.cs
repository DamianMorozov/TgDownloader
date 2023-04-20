// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

using Spectre.Console;
using TgStorage.Utils;

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper : ITgHelper
{
	#region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	private static TgMenuHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public static TgMenuHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

	#endregion

	#region Public and internal fields, properties, constructor

	internal TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
	internal TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
	internal TgLogHelper TgLog => TgLogHelper.Instance;
	internal TgClientHelper TgClient => TgClientHelper.Instance;
	internal Style StyleMain => new(Color.White, null, Decoration.Bold | Decoration.Conceal | Decoration.Italic);
	internal TgSqlContextManagerHelper ContextManager => TgSqlContextManagerHelper.Instance;
	internal TgMenuMain Value { get; set; }

	#endregion

	#region Public and internal methods

	internal void ShowTableCore(TgDownloadSettingsModel tgDownloadSettings, string title, Action<Table> fillTableColumns, 
		Action<TgDownloadSettingsModel, Table> fillTableRows)
	{
		AnsiConsole.Clear();
		AnsiConsole.Write(new FigletText(TgConstants.AppTitle).Centered().Color(Color.Yellow));
		Table table = new()
		{
			ShowHeaders = true,
			Border = TableBorder.Rounded,
			Title = new(title, Style.Plain),
		};

		fillTableColumns(table);

		if (table.Rows.Count > 0) table.Rows.Clear();
		fillTableRows(tgDownloadSettings, table);

		table.Expand();
		AnsiConsole.Write(table);
	}

	internal void ShowTableMain(TgDownloadSettingsModel tgDownloadSettings) => 
		ShowTableCore(tgDownloadSettings, TgConstants.MenuMain, FillTableColumns, FillTableRowsMain);

	internal void ShowTableStorageSettings(TgDownloadSettingsModel tgDownloadSettings) => 
		ShowTableCore(tgDownloadSettings, TgConstants.MenuMainStorage, FillTableColumns, FillTableRowsStorage);

	internal void ShowTableFiltersSettings(TgDownloadSettingsModel tgDownloadSettings) => 
		ShowTableCore(tgDownloadSettings, TgConstants.MenuMainFilters, FillTableColumns, FillTableRowsFilters);

	internal void ShowTableAppSettings(TgDownloadSettingsModel tgDownloadSettings) => 
		ShowTableCore(tgDownloadSettings, TgConstants.MenuMainApp, FillTableColumns, FillTableRowsApp);

	internal void ShowTableClient(TgDownloadSettingsModel tgDownloadSettings) => 
		ShowTableCore(tgDownloadSettings, TgConstants.MenuMainClient, FillTableColumns, FillTableRowsClient);

	internal void ShowTableDownload(TgDownloadSettingsModel tgDownloadSettings) => 
		ShowTableCore(tgDownloadSettings, TgConstants.MenuMainDownload, FillTableColumns, FillTableRowsDownload);

	internal void ShowTableAdvanced(TgDownloadSettingsModel tgDownloadSettings) => 
		ShowTableCore(tgDownloadSettings, TgConstants.MenuMainAdvanced, 
		FillTableColumns, FillTableRowsAdvanced);

	internal void ShowTableScanSources(TgDownloadSettingsModel tgDownloadSettings) => 
		ShowTableCore(tgDownloadSettings, TgConstants.MenuMainDownload, FillTableColumns, FillTableRowsScanDownloadedSources);

	internal void ShowTableViewSources(TgDownloadSettingsModel tgDownloadSettings) => 
		ShowTableCore(tgDownloadSettings, TgConstants.MenuMainAdvanced, FillTableColumns, FillTableRowsViewDownloadedSources);

	internal void FillTableColumns(Table table)
	{
		if (table.Columns.Count > 0) return;

		table.AddColumn(new TableColumn(
			new Markup(TgConstants.AppName, StyleMain))
		{ Width = 20 }.LeftAligned());
		table.AddColumn(new TableColumn(
			new Markup(TgConstants.AppValue, StyleMain))
		{ Width = 80 }.LeftAligned());
	}

	internal void FillTableRowsMain(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		// App version.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgConstants.AppVersion)), new Markup(TgAppSettings.AppXml.Version));
		TgSqlTableVersionModel version = !ContextManager.IsTableExists(TgSqlConstants.TableVersions) ? new() : ContextManager.Versions.GetItemLast();
		table.AddRow(new Markup(TgLocale.InfoMessage(TgConstants.StorageVersion)), new Markup($"v{version.Version}"));

		// App settings.
		table.AddRow(new Markup(TgAppSettings.IsReady
				? TgLocale.InfoMessage(TgConstants.MenuMainApp) : TgLocale.WarningMessage(TgConstants.MenuMainApp)),
			new Markup(TgAppSettings.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

		// Storage settings.
		table.AddRow(new Markup(ContextManager.IsReady
				? TgLocale.InfoMessage(TgConstants.MenuMainStorage) : TgLocale.WarningMessage(TgConstants.MenuMainStorage)),
			new Markup(ContextManager.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

		// TG client settings.
		table.AddRow(new Markup(TgClient.IsReady ?
			TgLocale.InfoMessage(TgConstants.MenuMainClient) : TgLocale.WarningMessage(TgConstants.MenuMainClient)),
			new Markup(TgClient.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

		// Download settings.
		table.AddRow(new Markup(tgDownloadSettings.IsReady
			? TgLocale.InfoMessage(TgConstants.MenuMainDownload) : TgLocale.WarningMessage(TgConstants.MenuMainDownload)),
			new Markup(tgDownloadSettings.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));
	}

	internal void FillTableRowsApp(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		// App xml settings.
		table.AddRow(new Markup(TgAppSettings.IsReady ?
				TgLocale.InfoMessage(TgConstants.MenuMainApp) : TgLocale.WarningMessage(TgConstants.MenuMainApp)),
			new Markup(TgAppSettings.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));


		// File session is exists.
		if (TgAppSettings.AppXml.IsExistsFileSession)
			table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.FileSession)),
				new Markup(TgLog.GetMarkupString(TgAppSettings.AppXml.FileSession)));
		else
			table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.FileSession)),
				new Markup(TgLog.GetMarkupString(TgAppSettings.AppXml.FileSession)));

		// File storage is exists.
		if (TgAppSettings.AppXml.IsExistsFileStorage)
			table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.FileStorage)),
				new Markup(TgLog.GetMarkupString(TgAppSettings.AppXml.FileStorage)));
		else
			table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.FileStorage)),
				new Markup(TgLog.GetMarkupString(TgAppSettings.AppXml.FileStorage)));

		// Usage proxy.
		if (TgAppSettings.AppXml.IsUseProxy)
			table.AddRow(new Markup(TgLocale.InfoMessage(TgConstants.MenuAppUseProxy)),
				new Markup(TgLog.GetMarkupString(TgAppSettings.AppXml.IsUseProxy.ToString())));
		else
			table.AddRow(new Markup(TgLocale.InfoMessage(TgConstants.MenuAppUseProxy, true)),
				new Markup(TgLog.GetMarkupString(TgAppSettings.AppXml.IsUseProxy.ToString())));
	}

	/// <summary>
	/// Storage settings.
	/// </summary>
	/// <param name="tgDownloadSettings"></param>
	/// <param name="table"></param>
	internal void FillTableRowsStorage(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		table.AddRow(new Markup(ContextManager.IsReady
				? TgLocale.InfoMessage(TgConstants.MenuMainStorage) : TgLocale.WarningMessage(TgConstants.MenuMainStorage)),
			new Markup(ContextManager.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));
	}

	/// <summary>
	/// Filters settings.
	/// </summary>
	/// <param name="tgDownloadSettings"></param>
	/// <param name="table"></param>
	internal void FillTableRowsFilters(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		List<TgSqlTableFilterModel> filters = ContextManager.Filters.GetList(false);
		table.AddRow(new Markup(TgLocale.InfoMessage(TgConstants.MenuFiltersAllCount)), new Markup($"{filters.Count}"));
	}

	internal void FillTableRowsClient(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		// TG client settings.
		table.AddRow(new Markup(TgClient.IsReady ?
				TgLocale.InfoMessage(TgConstants.MenuMainClient) : TgLocale.WarningMessage(TgConstants.MenuMainClient)),
			new Markup(TgClient.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

		if (TgClient.Me is null)
		{
			table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientUserName)),
				new Markup(TgLog.GetMarkupString(TgLocale.SettingsIsNeedSetup)));
			table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientUserId)),
				new Markup(TgLog.GetMarkupString(TgLocale.SettingsIsNeedSetup)));
			table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientUserIsActive)),
				new Markup(TgLog.GetMarkupString(TgLocale.SettingsIsNeedSetup)));
		}
		else
		{
			table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientUserName)),
				new Markup(TgLog.GetMarkupString(TgClient.Me.username)));
			table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientUserId)),
				new Markup(TgLog.GetMarkupString(TgClient.Me.id.ToString())));
			table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientUserIsActive)),
				new Markup(TgLog.GetMarkupString(TgClient.Me.IsActive.ToString())));
		}

		// Proxy setup.
		if (Equals(ContextManager.Apps.GetCurrentProxyUid, Guid.Empty))
		{
			if (TgAppSettings.AppXml.IsUseProxy)
				table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientProxySetup)),
					new Markup(TgLog.GetMarkupString(TgLocale.SettingsIsNeedSetup)));
			else
				table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientProxySetup)),
					new Markup(TgLog.GetMarkupString(TgLocale.SettingsIsOk)));
		}
		else
		{
			// Proxy is not found.
			if (ContextManager.Apps.GetCurrentProxy.IsNotExists || TgClient.Me is null)
			{
				table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientProxySetup)),
					new Markup(TgLog.GetMarkupString(TgLocale.SettingsIsNeedSetup)));
				table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientProxyType)),
					new Markup(TgLog.GetMarkupString(TgLocale.SettingsIsNeedSetup)));
				table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientProxyHostName)),
					new Markup(TgLog.GetMarkupString(TgLocale.SettingsIsNeedSetup)));
				table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientProxyPort)),
					new Markup(TgLog.GetMarkupString(TgLocale.SettingsIsNeedSetup)));
				table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientProxySecret)),
					new Markup(TgLog.GetMarkupString(TgLocale.SettingsIsNeedSetup)));
			}
			// Proxy is found.
			else
			{
				table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientProxySetup)),
					new Markup(TgLog.GetMarkupString(TgLocale.SettingsIsOk)));
				table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientProxyType)),
					new Markup(TgLog.GetMarkupString(ContextManager.Apps.GetCurrentProxy.Type.ToString())));
				table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientProxyHostName)),
					new Markup(TgLog.GetMarkupString(ContextManager.Apps.GetCurrentProxy.HostName)));
				table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientProxyPort)),
					new Markup(TgLog.GetMarkupString(ContextManager.Apps.GetCurrentProxy.Port.ToString())));
				if (Equals(ContextManager.Apps.GetCurrentProxy.Type, TgProxyType.MtProto))
					table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientProxySecret)),
						new Markup(TgLog.GetMarkupString(ContextManager.Apps.GetCurrentProxy.Secret)));
			}
		}

		// Exceptions.
		if (TgClient.ProxyException.IsExists)
		{
			table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientProxyException)),
				new Markup(TgLog.GetMarkupString(TgClient.ProxyException.Message)));
		}
		if (TgClient.ClientException.IsExists)
		{
			table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientException)),
				new Markup(TgLog.GetMarkupString(TgClient.ClientException.Message)));
		}
	}

	internal void FillTableRowsDownloadedSources(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		if (!tgDownloadSettings.IsReadySourceId)
			table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.SettingsSource)),
				new Markup(TgLocale.SettingsIsNeedSetup));
		else
		{
			string sourceValue = tgDownloadSettings.IsReadySourceId ? tgDownloadSettings.SourceId.ToString() : TgLocale.Empty;
			if (!string.IsNullOrEmpty(tgDownloadSettings.SourceUserName))
				sourceValue += $" | https://t.me/{tgDownloadSettings.SourceUserName}";
			if (!string.IsNullOrEmpty(tgDownloadSettings.SourceTitle))
				sourceValue += $" | {TgLog.GetMarkupString(tgDownloadSettings.SourceTitle)}";
			table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.SettingsSource)), new Markup(sourceValue));
		}
	}

	internal void FillTableRowsDownload(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		// Download settings.
		table.AddRow(new Markup(tgDownloadSettings.IsReady
				? TgLocale.InfoMessage(TgConstants.MenuMainDownload) : TgLocale.WarningMessage(TgConstants.MenuMainDownload)),
			new Markup(tgDownloadSettings.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

		// Source ID/username.
		FillTableRowsDownloadedSources(tgDownloadSettings, table);

		// Destination dir.
		if (string.IsNullOrEmpty(tgDownloadSettings.DestDirectory))
			table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgSettingsDestDirectory)),
				new Markup(TgLocale.SettingsIsNeedSetup));
		else
			table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgSettingsDestDirectory)),
				new Markup(tgDownloadSettings.DestDirectory));

		// Source start ID / last ID.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgSettingsSourceFirstLastId)),
			new Markup($"{tgDownloadSettings.SourceFirstId} / {tgDownloadSettings.SourceLastId}"));

		// Is rewrite files.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgSettingsIsRewriteFiles)),
			new Markup(tgDownloadSettings.IsRewriteFiles.ToString()));

		// Is rewrite messages.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgSettingsIsRewriteMessages)),
			new Markup(tgDownloadSettings.IsRewriteMessages.ToString()));

		// Is join message ID with file name.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgSettingsIsJoinFileNameWithMessageId)),
			new Markup(tgDownloadSettings.IsJoinFileNameWithMessageId.ToString()));

		// Is auto update.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgConstants.MenuDownloadSetIsAutoUpdate)),
			new Markup(tgDownloadSettings.IsAutoUpdate.ToString()));

		// Enabled filters.
		List<TgSqlTableFilterModel> filters = ContextManager.Filters.GetListEnabled();
		table.AddRow(new Markup(TgLocale.InfoMessage(TgConstants.MenuFiltersEnabledCount)), new Markup($"{filters.Count}"));
	}

	internal void FillTableRowsAdvanced(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		// Is auto update.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgConstants.MenuDownloadSetIsAutoUpdate)),
			new Markup(tgDownloadSettings.IsAutoUpdate.ToString()));
	}

	internal void FillTableRowsScanDownloadedSources(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		// Source ID/username.
		FillTableRowsDownloadedSources(tgDownloadSettings, table);
	}

	internal void FillTableRowsViewDownloadedSources(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		// Source ID/username.
		FillTableRowsDownloadedSources(tgDownloadSettings, table);
	}

	public void StoreMessage(int id, long sourceId, DateTime dtCreate, TgMessageType type, long size, string message)
	{
		ContextManager.Messages.AddOrUpdateItem(new()
		{
			Id = id,
			SourceId = sourceId,
			DtCreated = dtCreate,
			Type = type,
			Size = size,
			Message = message
		});
		TgSqlTableSourceModel source = ContextManager.Sources.GetItem(sourceId);
		source.FirstId = id;
		ContextManager.Sources.AddOrUpdateItem(source);
	}

	public void StoreDocument(long id, long sourceId, long messageId, string fileName, long fileSize, long accessHash) =>
		ContextManager.Documents.AddOrUpdateItem(new() { Id = id, SourceId = sourceId, MessageId = messageId,
			FileName = fileName, FileSize = fileSize, AccessHash = accessHash });

	public bool FindExistsMessage(long id, long sourceId) => ContextManager.Messages.GetItem(sourceId, id).IsExists;

	public bool AskQuestionReturnPositive(string title, bool isTrueFirst = false) => 
		AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title($"{title}?")
			.PageSize(3)
			.AddChoices(isTrueFirst ? new() { TgConstants.MenuIsTrue, TgConstants.MenuIsFalse }
					: new List<string> { TgConstants.MenuIsFalse, TgConstants.MenuIsTrue }))
	switch { TgConstants.MenuIsTrue => true, _ => false };

	public bool AskQuestionReturnNegative(string question, bool isTrueFirst = false) =>
		!AskQuestionReturnPositive(question, isTrueFirst);

	public TgSqlTableSourceModel GetSourceFromList(string title, List<TgSqlTableSourceModel> sources)
	{
		sources = sources.OrderBy(item => item.Id).ToList();
		sources = sources.OrderBy(item => item.UserName).ToList();
		List<string> list = sources.Select(item => TgLog.GetMarkupString(item.ToString())).ToList();
string sourceString = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title(title)
			.PageSize(15)
			.AddChoices(list));
return long.TryParse(sourceString.Substring(0, sourceString.IndexOf('|')).TrimEnd(' '), out long id) 
	? ContextManager.Sources.GetItem(id) : new();
	}

	#endregion
}