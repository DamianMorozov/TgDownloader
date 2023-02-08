// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgCore.Localization;

namespace TgDownloaderConsole.Helpers;

internal partial class MenuHelper : IHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static MenuHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static MenuHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and internal fields, properties, constructor

    internal AppSettingsHelper AppSettings => AppSettingsHelper.Instance;
    internal TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
    internal TgLogHelper TgLog => TgLogHelper.Instance;
    internal TgClientHelper TgClient => TgClientHelper.Instance;
    internal Style StyleMain => new(Color.White, null, Decoration.Bold | Decoration.Conceal | Decoration.Italic);
    internal TgStorageHelper TgStorage => TgStorageHelper.Instance;
    internal MenuMain Value { get; set; }

    public MenuHelper()
    {
            //
    }

    #endregion

    #region Public and internal methods

    internal void ShowTableCore(TgDownloadSettingsModel tgDownloadSettings, string title, Action<Table> fillTableColumns, Action<TgDownloadSettingsModel, Table> fillTableRows)
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
        
        if (table.Rows.Count > 0) table.Rows.Clear();
        fillTableRows(tgDownloadSettings, table);

        table.Expand();
        AnsiConsole.Write(table);
    }

    internal void ShowTableMain(TgDownloadSettingsModel tgDownloadSettings) => ShowTableCore(tgDownloadSettings, TgLocale.MenuMain, FillTableColumns, FillTableRowsMain);

    internal void ShowTableStorageSettings(TgDownloadSettingsModel tgDownloadSettings) => ShowTableCore(tgDownloadSettings, TgLocale.MenuMainStorage, FillTableColumns, FillTableRowsStorage);

    internal void ShowTableAppSettings(TgDownloadSettingsModel tgDownloadSettings) => ShowTableCore(tgDownloadSettings, TgLocale.TgSettings, FillTableColumns, FillTableRowsApp);
    
    internal void ShowTableClient(TgDownloadSettingsModel tgDownloadSettings) => ShowTableCore(tgDownloadSettings, TgLocale.TgSettings, FillTableColumns, FillTableRowsClient);
    
    internal void ShowTableDownload(TgDownloadSettingsModel tgDownloadSettings) => ShowTableCore(tgDownloadSettings, TgLocale.MenuMainDownload, FillTableColumns, FillTableRowsDownload);

    internal void ShowTableAdvanced(TgDownloadSettingsModel tgDownloadSettings) => ShowTableCore(tgDownloadSettings, TgLocale.MenuMainDownload, FillTableColumns, FillTableRowsAdvanced);

    internal void ShowTableScanSources(TgDownloadSettingsModel tgDownloadSettings) => ShowTableCore(tgDownloadSettings, TgLocale.MenuMainDownload, FillTableColumns, FillTableRowsScanSources);

    internal void ShowTableViewSources(TgDownloadSettingsModel tgDownloadSettings) => ShowTableCore(tgDownloadSettings, TgLocale.MenuMainDownload, FillTableColumns, FillTableRowsViewSources);

    internal void FillTableColumns(Table table)
    {
        if (table.Columns.Count > 0) return;

        table.AddColumn(new TableColumn(
            new Markup(TgLocale.AppName, StyleMain)) { Width = 20 }.LeftAligned());
        table.AddColumn(new TableColumn(
            new Markup(TgLocale.AppValue, StyleMain)) { Width = 80 }.LeftAligned());
    }

    internal void FillTableRowsMain(TgDownloadSettingsModel tgDownloadSettings, Table table)
    {
        table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.AppVersion)), new Markup(AppSettings.AppXml.Version));

        // App settings.
        table.AddRow(new Markup(AppSettings.IsReady
                ? TgLocale.InfoMessage(TgLocale.MenuMainAppSettings) : TgLocale.WarningMessage(TgLocale.MenuMainAppSettings)),
            new Markup(AppSettings.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

        // Storage settings.
        table.AddRow(new Markup(TgStorage.IsReady
                ? TgLocale.InfoMessage(TgLocale.MenuMainStorage) : TgLocale.WarningMessage(TgLocale.MenuMainStorage)),
            new Markup(TgStorage.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

        // TG client settings.
        table.AddRow(new Markup(TgClient.IsReady ?
            TgLocale.InfoMessage(TgLocale.MenuMainClient) : TgLocale.WarningMessage(TgLocale.MenuMainClient)),
            new Markup(TgClient.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

        // Download settings.
        table.AddRow(new Markup(tgDownloadSettings.IsReady
            ? TgLocale.InfoMessage(TgLocale.MenuMainDownload) : TgLocale.WarningMessage(TgLocale.MenuMainDownload)),
            new Markup(tgDownloadSettings.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));
    }

    internal void FillTableRowsApp(TgDownloadSettingsModel tgDownloadSettings, Table table)
    {
        // App xml settings.
        table.AddRow(new Markup(AppSettings.IsReady ?
                TgLocale.InfoMessage(TgLocale.MenuMainAppSettings) : TgLocale.WarningMessage(TgLocale.MenuMainAppSettings)),
            new Markup(AppSettings.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));


        // File session is exists.
        if (AppSettings.AppXml.IsExistsFileSession)
            table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.FileSession)),
                new Markup(TgLog.GetMarkupString(AppSettings.AppXml.FileSession)));
        else
            table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.FileSession)),
                new Markup(TgLog.GetMarkupString(AppSettings.AppXml.FileSession)));
        
        // File storage is exists.
        if (AppSettings.AppXml.IsExistsFileStorage)
            table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.FileStorage)),
                new Markup(TgLog.GetMarkupString(AppSettings.AppXml.FileStorage)));
        else
            table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.FileStorage)),
                new Markup(TgLog.GetMarkupString(AppSettings.AppXml.FileStorage)));

        // Usage proxy.
        if (AppSettings.AppXml.IsUseProxy)
            table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.MenuAppUseProxy)),
                new Markup(TgLog.GetMarkupString(AppSettings.AppXml.IsUseProxy.ToString())));
        else
            table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.MenuAppUseProxy, true)),
                new Markup(TgLog.GetMarkupString(AppSettings.AppXml.IsUseProxy.ToString())));
    }

    internal void FillTableRowsStorage(TgDownloadSettingsModel tgDownloadSettings, Table table)
    {
        // Storage settings.
        table.AddRow(new Markup(TgStorage.IsReady
                ? TgLocale.InfoMessage(TgLocale.MenuMainStorage) : TgLocale.WarningMessage(TgLocale.MenuMainStorage)),
            new Markup(TgStorage.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));
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
        if (Equals(TgStorage.App.ProxyUid, Guid.Empty))
        {
            if (AppSettings.AppXml.IsUseProxy)
                table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientProxySetup)),
                    new Markup(TgLog.GetMarkupString(TgLocale.SettingsIsNeedSetup)));
            else
                table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientProxySetup)),
                    new Markup(TgLog.GetMarkupString(TgLocale.SettingsIsOk)));
        }
        else
        {
            // Proxy is not found.
            if (TgStorage.Proxy.IsNotExists || TgClient.Me is null)
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
                    new Markup(TgLog.GetMarkupString(TgStorage.Proxy.Type.ToString())));
                table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientProxyHostName)),
                    new Markup(TgLog.GetMarkupString(TgStorage.Proxy.HostName)));
                table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientProxyPort)),
                    new Markup(TgLog.GetMarkupString(TgStorage.Proxy.Port.ToString())));
                if (Equals(TgStorage.Proxy.Type, ProxyType.MtProto))
                    table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientProxySecret)),
                        new Markup(TgLog.GetMarkupString(TgStorage.Proxy.Secret)));
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

    internal void FillTableRowsSource(TgDownloadSettingsModel tgDownloadSettings, Table table)
    {
        if (!tgDownloadSettings.IsReadySourceId)
            table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.SettingsSource)),
                new Markup(TgLocale.SettingsIsNeedSetup));
        else
        {
            string sourceValue = tgDownloadSettings.IsReadySourceId ? tgDownloadSettings.SourceId.ToString() : TgLocale.Empty;
            if (!string.IsNullOrEmpty(tgDownloadSettings.SourceUserName))
                sourceValue += $" | @{tgDownloadSettings.SourceUserName}";
            table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.SettingsSource)), new Markup(sourceValue));

        }
    }

    internal void FillTableRowsDownload(TgDownloadSettingsModel tgDownloadSettings, Table table)
    {
        // Download settings.
        table.AddRow(new Markup(tgDownloadSettings.IsReady
                ? TgLocale.InfoMessage(TgLocale.MenuMainDownload) : TgLocale.WarningMessage(TgLocale.MenuMainDownload)),
            new Markup(tgDownloadSettings.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

        // Source ID/username.
        FillTableRowsSource(tgDownloadSettings, table);

        // Dest dir.
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
        table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.MenuDownloadSetIsAutoUpdate)),
            new Markup(tgDownloadSettings.IsAutoUpdate.ToString()));
    }

    internal void FillTableRowsAdvanced(TgDownloadSettingsModel tgDownloadSettings, Table table)
    {
        // Is auto update.
        table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.MenuDownloadSetIsAutoUpdate)),
            new Markup(tgDownloadSettings.IsAutoUpdate.ToString()));
    }

    internal void FillTableRowsScanSources(TgDownloadSettingsModel tgDownloadSettings, Table table)
    {
        // Source ID/username.
        FillTableRowsSource(tgDownloadSettings, table);
    }

    internal void FillTableRowsViewSources(TgDownloadSettingsModel tgDownloadSettings, Table table)
    {
        // Source ID/username.
        FillTableRowsSource(tgDownloadSettings, table);
    }

    public void StoreMessage(long? id, long? sourceId, DateTime dtCreate, string message, string type, long size) => 
        TgStorage.AddOrUpdateItemMessageDeprecated(id, sourceId, dtCreate, message, type, size, true);

    public void StoreDocument(long? id, long? sourceId, long? messageId, string fileName, long fileSize, long accessHash) => 
        TgStorage.AddOrUpdateItemDocumentDeprecated(id, sourceId, messageId, fileName, fileSize, accessHash, true);

    public bool FindExistsMessage(long? id, long? sourceId)
    {
        SqlTableMessageModel message = TgStorage.GetItemDeprecated<SqlTableMessageModel>(id, sourceId);
        return TgStorage.IsValid(message);
    }

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected MenuHelper(SerializationInfo info, StreamingContext context)
    {
        object? value = info.GetValue(nameof(Value), typeof(MenuMain));
        Value = value is not null ? (MenuMain)value : MenuMain.Exit;
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(Value), Value);
    }

    #endregion
}