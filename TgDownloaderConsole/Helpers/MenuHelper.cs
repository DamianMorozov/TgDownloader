// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorageCore.Helpers;
using TgStorageCore.Models;

namespace TgDownloaderConsole.Helpers;

internal partial class MenuHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static MenuHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static MenuHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and internal fields, properties, constructor

    internal AppHelper App => AppHelper.Instance;
    internal TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
    internal TgLogHelper TgLog => TgLogHelper.Instance;
    internal TgClientHelper TgClient => TgClientHelper.Instance;
    internal Style StyleMain => new(Color.White, null, Decoration.Bold | Decoration.Conceal | Decoration.Italic);
    internal StatusContext? StatusContext = null;
    internal TgStorageHelper TgStorage => TgStorageHelper.Instance;
    internal MenuMain Value = MenuMain.Exit;

    #endregion

    #region Public and internal methods

    internal void ShowTableCore(string title, Action<Table> fillTableColumns, Action<Table> fillTableRows)
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
        fillTableRows(table);

        table.Expand();
        AnsiConsole.Write(table);
    }

    internal void ShowTableMain() => ShowTableCore(TgLocale.MenuMain, FillTableColumns, FillTableRowsMain);

    internal void ShowTableStorageSettings() => ShowTableCore(TgLocale.MenuMainStorage, FillTableColumns, FillTableRowsStorage);

    internal void ShowTableClient() => ShowTableCore(TgLocale.TgSettings, FillTableColumns, FillTableRowsClient);
    
    internal void ShowTableDownload() => ShowTableCore(TgLocale.MenuMainClient, FillTableColumns, FillTableRowsDownload);

    internal void FillTableColumns(Table table)
    {
        if (table.Columns.Count > 0) return;

        table.AddColumn(new TableColumn(
            new Markup(TgLocale.AppName, StyleMain)) { Width = 20 }.LeftAligned());
        table.AddColumn(new TableColumn(
            new Markup(TgLocale.AppValue, StyleMain)) { Width = 80 }.LeftAligned());
    }

    internal void FillTableRowsMain(Table table)
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
        table.AddRow(new Markup(TgClient.TgDownload.IsReady
            ? TgLocale.InfoMessage(TgLocale.MenuMainDownload) : TgLocale.WarningMessage(TgLocale.MenuMainDownload)),
            new Markup(TgClient.TgDownload.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));
    }

    internal void FillTableRowsStorage(Table table)
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

    internal void FillTableRowsClient(Table table)
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
            User user = TgClient.MySelfUser;
            table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientUserName)),
                new Markup(user.username));
            table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientUserId)),
                new Markup(user.id.ToString()));
            table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgClientUserIsActive)),
                new Markup(user.IsActive.ToString()));
        }
    }

    internal void FillTableRowsDownload(Table table)
    {
        // Download settings.
        table.AddRow(new Markup(TgClient.TgDownload.IsReady
                ? TgLocale.InfoMessage(TgLocale.MenuMainDownload) : TgLocale.WarningMessage(TgLocale.MenuMainDownload)),
            new Markup(TgClient.TgDownload.IsReady ? TgLocale.SettingsIsOk : TgLocale.SettingsIsNeedSetup));

        // Source user name.
        if (string.IsNullOrEmpty(TgClient.TgDownload.SourceUserName))
            table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgSettingsSourceUserName)),
                new Markup(TgLocale.SettingsIsNeedSetup));
        else
            table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgSettingsSourceUserName)),
                new Markup(TgClient.TgDownload.SourceUserName));

        // Dest dir.
        if (string.IsNullOrEmpty(TgClient.TgDownload.DestDirectory))
            table.AddRow(new Markup(TgLocale.WarningMessage(TgLocale.TgSettingsDestDirectory)),
                new Markup(TgLocale.SettingsIsNeedSetup));
        else
            table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgSettingsDestDirectory)),
                new Markup(TgClient.TgDownload.DestDirectory));
        
        //// Is rewrite files.
        //table.AddRow(new Markup(TgLocale.InfoMessage(TgLocale.TgSettingsIsMessageRewrite)),
        //    new Markup(TgClient.TgDownload.IsRewriteFiles.ToString()));
    }

    internal double CalcSourceProgress(long count, long current) =>
        count == 0 ? 0 : (double)(current * 100) / count;

    private string GetLongString(long current) => current > 999 ? $"{current:### ###}" : $"{current:###}";

    public string GetStatus(Stopwatch sw, long count, long current) =>
        count == 0 && current == 0
            ? $"{TgLog.GetDtShortStamp()} | {sw.Elapsed} | "
            : $"{TgLog.GetDtShortStamp()} | {sw.Elapsed} | " +
              $"{CalcSourceProgress(count, current):#00.00} % | " +
              $"{GetLongString(current)} / {GetLongString(count)}";

    public string GetStatus(long count, long current) =>
        count == 0 && current == 0
            ? TgLog.GetDtShortStamp()
            : $"{TgLog.GetDtShortStamp()} | " +
              $"{CalcSourceProgress(count, current):#00.00} % | " +
              $"{GetLongString(current)} / {GetLongString(count)}";

    public bool CheckTgSettings(MenuDownload menuDownload) =>
        TgClient.IsReady &&
        !string.IsNullOrEmpty(TgClient.TgDownload.SourceUserName) &&
        (!string.IsNullOrEmpty(TgClient.TgDownload.DestDirectory));

    public void RefreshStatusForDownload(string message)
    {
        if (StatusContext is null) return;
        StatusContext.Status(TgLog.GetMarkupString(
            $"{GetStatus(TgClient.TgDownload.MessageCount, TgClient.TgDownload.MessageCurrentId)} | {message}"));
        StatusContext.Refresh();
    }

    public void StoreMessage(long? id, long? sourceId, string message, 
        string fileName, long fileSize, long accessHash)
    {
        TgStorage.AddRecordMessage(id, sourceId, message, fileName, fileSize, accessHash);
    }

    public bool FindExistsMessage(long? id, long? sourceId)
    {
        TableMessageModel message = TgStorage.GetRecordMessage(id, sourceId);
        return TgStorage.IsValid(message);
    }
    
    #endregion
}