// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

[DebuggerDisplay("{ToDebugString()}")]
internal sealed partial class TgMenuHelper : ITgHelper
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
	internal TgEnumMenuMain Value { get; set; }

	#endregion

	#region Public and internal methods

    public string ToDebugString() => TgLocale.UseOverrideMethod;

	internal void ShowTableCore(TgDownloadSettingsModel tgDownloadSettings, string title, Action<Table> fillTableColumns,
		Action<TgDownloadSettingsModel, Table> fillTableRows)
	{
		AnsiConsole.Clear();
		AnsiConsole.Write(new FigletText(TgLocale.AppTitle).Centered().Color(Color.Yellow));
		Table table = new()
		{
			ShowHeaders = true,
			Border = TableBorder.Rounded,
			Title = new(title, Style.Plain),
		};

		fillTableColumns(table);

		if (table.Rows.Count > 0)
			table.Rows.Clear();
		fillTableRows(tgDownloadSettings, table);

		table.Expand();
		AnsiConsole.Write(table);
	}

	internal void ShowTableMain(TgDownloadSettingsModel tgDownloadSettings) =>
		ShowTableCore(tgDownloadSettings, TgLocale.MenuMain, FillTableColumns, FillTableRowsMain);

	internal void ShowTableStorageSettings(TgDownloadSettingsModel tgDownloadSettings) =>
		ShowTableCore(tgDownloadSettings, TgLocale.MenuMainStorage, FillTableColumns, FillTableRowsStorage);

	internal void ShowTableFiltersSettings(TgDownloadSettingsModel tgDownloadSettings) =>
		ShowTableCore(tgDownloadSettings, TgLocale.MenuMainFilters, FillTableColumns, FillTableRowsFilters);

	internal void ShowTableAppSettings(TgDownloadSettingsModel tgDownloadSettings) =>
		ShowTableCore(tgDownloadSettings, TgLocale.MenuMainApp, FillTableColumns, FillTableRowsApp);

	internal void ShowTableClient(TgDownloadSettingsModel tgDownloadSettings) =>
		ShowTableCore(tgDownloadSettings, TgLocale.MenuMainClient, FillTableColumns, FillTableRowsClient);

	internal void ShowTableDownload(TgDownloadSettingsModel tgDownloadSettings) =>
		ShowTableCore(tgDownloadSettings, TgLocale.MenuMainDownload, FillTableColumns, FillTableRowsDownload);

	internal void ShowTableAdvanced(TgDownloadSettingsModel tgDownloadSettings) =>
		ShowTableCore(tgDownloadSettings, TgLocale.MenuMainAdvanced, FillTableColumns, FillTableRowsAdvanced);

	internal void ShowTableScanSources(TgDownloadSettingsModel tgDownloadSettings) =>
		ShowTableCore(tgDownloadSettings, TgLocale.MenuMainDownload, FillTableColumns, FillTableRowsScanDownloadedSources);

	internal void ShowTableViewSources(TgDownloadSettingsModel tgDownloadSettings) =>
		ShowTableCore(tgDownloadSettings, TgLocale.MenuMainAdvanced, FillTableColumns, FillTableRowsViewDownloadedSources);

	internal void FillTableColumns(Table table)
	{
		if (table.Columns.Count > 0)
			return;

		table.AddColumn(new TableColumn(
			new Markup(TgLocale.AppName, StyleMain))
		{ Width = 20 }.LeftAligned());
		table.AddColumn(new TableColumn(
			new Markup(TgLocale.AppValue, StyleMain))
		{ Width = 80 }.LeftAligned());
	}

	internal void FillTableRowsMain(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		// App version.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.AppVersion)), new Markup(TgAppSettings.AppXml.Version));
		TgSqlTableVersionModel version = !ContextManager.IsTableExists(TgSqlConstants.TableVersions) 
            ? new() : ContextManager.VersionRepository.GetItemLastAsync().Result;
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.StorageVersion)), new Markup($"v{version.Version}"));

		// App settings.
		table.AddRow(new Markup(TgAppSettings.IsReady
				? TgLocale.InfoMessage(TgLocale.MenuMainApp) : TgLocale.WarningMessage(TgLocale.MenuMainApp)),
			new Markup(TgAppSettings.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

		// Storage settings.
		table.AddRow(new Markup(ContextManager.IsReady
				? TgLocale.InfoMessage(TgLocale.MenuMainStorage) : TgLocale.WarningMessage(TgLocale.MenuMainStorage)),
			new Markup(ContextManager.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

		// TG client settings.
		table.AddRow(new Markup(TgClient.IsReady ?
			TgLocale.InfoMessage(TgLocale.MenuMainClient) : TgLocale.WarningMessage(TgLocale.MenuMainClient)),
			new Markup(TgClient.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

		// Download settings.
		table.AddRow(new Markup(tgDownloadSettings.SourceVm.IsReady
			? TgLocale.InfoMessage(TgLocale.MenuMainDownload) : TgLocale.WarningMessage(TgLocale.MenuMainDownload)),
			new Markup(tgDownloadSettings.SourceVm.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));
	}

	internal void FillTableRowsApp(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		// App xml settings.
		table.AddRow(new Markup(TgAppSettings.IsReady ?
				TgLocale.InfoMessage(TgLocale.MenuMainApp) : TgLocale.WarningMessage(TgLocale.MenuMainApp)),
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
			table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.MenuAppUseProxy)),
				new Markup(TgLog.GetMarkupString(TgAppSettings.AppXml.IsUseProxy.ToString())));
		else
			table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.MenuAppUseProxy, true)),
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
				? TgLocale.InfoMessage(TgLocale.MenuMainStorage) : TgLocale.WarningMessage(TgLocale.MenuMainStorage)),
			new Markup(ContextManager.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));
	}

	/// <summary>
	/// Filters settings.
	/// </summary>
	/// <param name="tgDownloadSettings"></param>
	/// <param name="table"></param>
	internal void FillTableRowsFilters(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		IEnumerable<TgSqlTableFilterModel> filters = ContextManager.FilterRepository.GetEnumerable();
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.MenuFiltersAllCount)), 
			new Markup($"{filters.Count()}"));
	}

	internal void FillTableRowsClient(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		// TG client settings.
		table.AddRow(new Markup(TgClient.IsReady ?
				TgLocale.InfoMessage(TgLocale.MenuMainClient) : TgLocale.WarningMessage(TgLocale.MenuMainClient)),
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
		if (Equals(ContextManager.AppRepository.GetFirstProxyUidAsync(), Guid.Empty))
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
			if (ContextManager.AppRepository.GetCurrentProxyAsync().Result.IsNotExists || TgClient.Me is null)
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
					new Markup(TgLog.GetMarkupString(ContextManager.AppRepository.GetCurrentProxyAsync().Result.Type.ToString())));
				table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientProxyHostName)),
					new Markup(TgLog.GetMarkupString(ContextManager.AppRepository.GetCurrentProxyAsync().Result.HostName)));
				table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientProxyPort)),
					new Markup(TgLog.GetMarkupString(ContextManager.AppRepository.GetCurrentProxyAsync().Result.Port.ToString())));
				if (Equals(ContextManager.AppRepository.GetCurrentProxyAsync().Result.Type, TgEnumProxyType.MtProto))
					table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientProxySecret)),
						new Markup(TgLog.GetMarkupString(ContextManager.AppRepository.GetCurrentProxyAsync().Result.Secret)));
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

	/// <summary>
	/// Source info.
	/// </summary>
	/// <param name="tgDownloadSettings"></param>
	/// <param name="table"></param>
	internal void FillTableRowsDownloadedSources(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		if (!tgDownloadSettings.SourceVm.IsReadySourceId)
			table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.SettingsSource)),
				new Markup(TgLocale.SettingsIsNeedSetup));
		else
		{
			TgSqlTableSourceModel source = ContextManager.SourceRepository.GetAsync(tgDownloadSettings.SourceVm.SourceId).Result;
			table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.SettingsSource)),
				new Markup(TgLog.GetMarkupString(source.ToConsoleStringShort())));
			table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.SettingsDtChanged)),
				new Markup(TgDataFormatUtils.GetDtFormat(source.DtChanged)));
		}
	}

	internal void FillTableRowsDownload(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		// Download.
		table.AddRow(new Markup(tgDownloadSettings.SourceVm.IsReady
				? TgLocale.InfoMessage(TgLocale.MenuMainDownload) : TgLocale.WarningMessage(TgLocale.MenuMainDownload)),
			new Markup(tgDownloadSettings.SourceVm.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

		// Source info.
		FillTableRowsDownloadedSources(tgDownloadSettings, table);

		// Destination dir.
		if (string.IsNullOrEmpty(tgDownloadSettings.SourceVm.SourceDirectory))
			table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgSettingsDestDirectory)),
				new Markup(TgLocale.SettingsIsNeedSetup));
		else
			table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgSettingsDestDirectory)),
				new Markup(tgDownloadSettings.SourceVm.SourceDirectory));

		// First/last ID.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgSettingsSourceFirstLastId)),
			new Markup($"{tgDownloadSettings.SourceVm.SourceFirstId} / {tgDownloadSettings.SourceVm.SourceLastId}"));

		// Rewrite files.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgSettingsIsRewriteFiles)),
			new Markup(tgDownloadSettings.IsRewriteFiles.ToString()));

		// Rewrite messages.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgSettingsIsRewriteMessages)),
			new Markup(tgDownloadSettings.IsRewriteMessages.ToString()));

		// Join message ID.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgSettingsIsJoinFileNameWithMessageId)),
			new Markup(tgDownloadSettings.IsJoinFileNameWithMessageId.ToString()));

		// Enable auto update.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.MenuDownloadSetIsAutoUpdate)),
			new Markup(tgDownloadSettings.SourceVm.IsAutoUpdate.ToString()));

        // Enabled filters.
        IEnumerable<TgSqlTableFilterModel> filters = ContextManager.FilterRepository.GetEnumerableEnabled();
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.MenuFiltersEnabledCount)), new Markup($"{filters.Count()}"));
	}

	internal void FillTableRowsAdvanced(TgDownloadSettingsModel tgDownloadSettings, Table table)
	{
		// Is auto update.
		table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.MenuDownloadSetIsAutoUpdate)),
			new Markup(tgDownloadSettings.SourceVm.IsAutoUpdate.ToString()));
	}

    /// <summary>
    /// Source ID/username.
    /// </summary>
    /// <param name="tgDownloadSettings"></param>
    /// <param name="table"></param>
    internal void FillTableRowsScanDownloadedSources(TgDownloadSettingsModel tgDownloadSettings, Table table) => 
        FillTableRowsDownloadedSources(tgDownloadSettings, table);

    /// <summary>
    /// Source ID/username.
    /// </summary>
    /// <param name="tgDownloadSettings"></param>
    /// <param name="table"></param>
    internal void FillTableRowsViewDownloadedSources(TgDownloadSettingsModel tgDownloadSettings, Table table) => 
        FillTableRowsDownloadedSources(tgDownloadSettings, table);

    public bool AskQuestionReturnPositive(string title, bool isTrueFirst = false)
	{
		string prompt = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title($"{title}?")
			.PageSize(Console.WindowHeight - 17)
			.AddChoices(isTrueFirst
				? new List<string> { TgLocale.MenuIsTrue, TgLocale.MenuIsFalse }
				: new List<string> { TgLocale.MenuIsFalse, TgLocale.MenuIsTrue }));
		return prompt.Equals(TgLocale.MenuIsTrue);
    }

	public bool AskQuestionReturnNegative(string question, bool isTrueFirst = false) =>
		!AskQuestionReturnPositive(question, isTrueFirst);

	public TgSqlTableSourceModel GetSourceFromEnumerable(string title, IEnumerable<TgSqlTableSourceModel> sources)
	{
		sources = sources.OrderBy(item => item.UserName).ThenBy(item => item.Id);
		List<string> list = new() { TgLocale.MenuMainReturn };
		list.AddRange(sources.Select(source => TgLog.GetMarkupString(source.ToConsoleString())));
		string sourceString = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title(title)
			.PageSize(Console.WindowHeight - 17)
			.AddChoices(list));
        if (!Equals(sourceString, TgLocale.MenuMainReturn))
        {
            int len = sourceString.IndexOf('|', 8) - 9;
            string sourceId = sourceString.Substring(8, len).TrimEnd(' ');
            if (long.TryParse(sourceId, out long id))
                return ContextManager.SourceRepository.GetAsync(id).Result;
        }
        return ContextManager.SourceRepository.GetNewAsync().Result;
	}

    #endregion
}