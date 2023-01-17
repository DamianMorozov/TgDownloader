// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Runtime.Serialization;
using TgCore.Helpers;
using TgCore.Interfaces;
using TgDownloaderCore.Models;
using TgLocalization.Enums;
using TgLocalization.Helpers;
using TgStorageCore.Helpers;
using TgStorageCore.Models.Messages;

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

    internal TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
    internal TgLogHelper TgLog => TgLogHelper.Instance;
    internal TgClientHelper TgClient => TgClientHelper.Instance;
    internal Style StyleMain => new(Color.White, null, Decoration.Bold | Decoration.Conceal | Decoration.Italic);
    internal TgStorageHelper TgStorage => TgStorageHelper.Instance;
    internal AppModel App { get; set; }
    internal MenuMain Value { get; set; } = MenuMain.Exit;

    public MenuHelper()
    {
        App = new();
    }

    #endregion

    #region Public and internal methods

    internal void ShowTableCore(TgDownloadSettingsModel tgDownloadSettings, string title, Action<Table> fillTableColumns, Action<TgDownloadSettingsModel, Table> fillTableRows)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText(TgLocale.AppTitle).Alignment(Justify.Center).Color(Color.Yellow));
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

    internal void ShowTableClient(TgDownloadSettingsModel tgDownloadSettings) => ShowTableCore(tgDownloadSettings, TgLocale.TgSettings, FillTableColumns, FillTableRowsClient);
    
    internal void ShowTableDownload(TgDownloadSettingsModel tgDownloadSettings) => ShowTableCore(tgDownloadSettings, TgLocale.MenuMainDownload, FillTableColumns, FillTableRowsDownload);

    internal void ShowTableScan(TgDownloadSettingsModel tgDownloadSettings) => ShowTableCore(tgDownloadSettings, TgLocale.MenuMainDownload, FillTableColumns, FillTableRowsScan);

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
        table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.AppVersion)), new Markup(App.Version));

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

    internal void FillTableRowsStorage(TgDownloadSettingsModel tgDownloadSettings, Table table)
    {
        // Storage settings.
        table.AddRow(new Markup(TgStorage.IsReady
                ? TgLocale.InfoMessage(TgLocale.MenuMainStorage) : TgLocale.WarningMessage(TgLocale.MenuMainStorage)),
            new Markup(TgStorage.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));
        // Storage is exists.
        table.AddRow(new Markup(TgStorage.IsReadyFileExists
            ? TgLocale.InfoMessage(TgLocale.StorageFileExists) : TgLocale.WarningMessage(TgLocale.StorageFileExists)),
            new Markup(TgStorage.IsReadyFileExists ? TgLocale.FileIsExists : TgLocale.FileIsNotExists));
    }

    internal void FillTableRowsClient(TgDownloadSettingsModel tgDownloadSettings, Table table)
    {
        // TG client settings.
        table.AddRow(new Markup(TgClient.IsReady ?
                TgLocale.InfoMessage(TgLocale.MenuMainClient) : TgLocale.WarningMessage(TgLocale.MenuMainClient)),
            new Markup(TgClient.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

        if (!TgClient.IsReady)
        {
            table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientUserName)),
                new Markup(TgLocale.SettingsIsNeedSetup));
            table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientUserId)),
                new Markup(TgLocale.SettingsIsNeedSetup));
            table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgClientUserIsActive)),
                new Markup(TgLocale.SettingsIsNeedSetup));
        }
        else
        {
            User user = TgClient.Me;
            table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientUserName)),
                new Markup(user.username));
            table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientUserId)),
                new Markup(user.id.ToString()));
            table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientUserIsActive)),
                new Markup(user.IsActive.ToString()));
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
        table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.MenuIsAutoUpdate)),
            new Markup(tgDownloadSettings.IsAutoUpdate.ToString()));
    }

    internal void FillTableRowsScan(TgDownloadSettingsModel tgDownloadSettings, Table table)
    {
        // Source ID/username.
        FillTableRowsSource(tgDownloadSettings, table);
    }

    public void StoreMessage(long? id, long? sourceId, DateTime dtCreate, string message, string type, long size) => 
        TgStorage.AddOrUpdateRecordMessage(id, sourceId, dtCreate, message, type, size, true);

    public void StoreDocument(long? id, long? sourceId, long? messageId, string fileName, long fileSize, long accessHash) => 
        TgStorage.AddOrUpdateRecordDocument(id, sourceId, messageId, fileName, fileSize, accessHash, true);

    public bool FindExistsMessage(long? id, long? sourceId)
    {
        SqlTableMessageModel message = TgStorage.GetItem<SqlTableMessageModel>(id, sourceId);
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
        App = info.GetValue(nameof(App), typeof(AppModel)) as AppModel ?? new();
        Value = (MenuMain)info.GetValue(nameof(Value), typeof(MenuMain));
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(App), App);
        info.AddValue(nameof(Value), Value);
    }

    #endregion
}