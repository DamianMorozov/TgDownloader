// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderConsole.Helpers;

internal class MenuHelper
{
    #region Design pattern "Lazy Singleton"

    private static MenuHelper _instance;
    public static MenuHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and internal fields, properties, constructor

    internal static LocaleHelper Locale => LocaleHelper.Instance;
    internal static TgClientHelper TgClient => TgClientHelper.Instance;
    internal static Style StyleMain => new(Color.White, null, Decoration.Bold | Decoration.Conceal | Decoration.Italic);

    internal long SourceFileSize { get; set; }
    internal long SourceFileRead { get; set; }
    internal double SourceFileProgress
    {
        get
        {
            if (SourceFileSize == 0)
                return 0;
            return (double)(SourceFileRead * 100) / SourceFileSize;
        }
    }
    internal TgMenu MenuItem { get; set; } = TgMenu.Exit;

    #endregion

    #region Public and internal methods

    internal static string GetAppVersion()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        string version = fvi.FileVersion;
        return version ?? string.Empty;
    }

    internal void ShowTable(string title)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText(Locale.Info.AppTitle).Alignment(Justify.Center).Color(Color.Yellow));
        Table table = new()
        {
            ShowHeaders = true,
            Border = TableBorder.Rounded,
            Title = new(title, Style.Plain),
        };

        FillTableColumns(table);
        FillTableRows(table);

        table.Expand();
        AnsiConsole.Write(table);
    }

    internal void FillTableColumns(Table table)
    {
        if (table.Columns.Count > 0) return;

        table.AddColumn(new TableColumn(
            new Markup(Locale.Info.AppName, StyleMain)) { Width = 20 }.LeftAligned());
        table.AddColumn(new TableColumn(
            new Markup(Locale.Info.AppValue, StyleMain)) { Width = 80 }.LeftAligned());
    }

    internal void FillTableRows(Table table)
    {
        if (table.Rows.Count > 0) table.Rows.Clear();

        table.AddRow(new Markup(Locale.InfoMessage(Locale.Info.AppVersion)), new Markup($@"v{GetAppVersion()}"));

        // TG client info.
        if (!TgClient.IsConnected)
        {
            table.AddRow(new Markup(Locale.WarningMessage(Locale.Info.TgClientUserName)),
                new Markup(Locale.Warning.TgSettingsNotSet));
            table.AddRow(new Markup(Locale.WarningMessage(Locale.Info.TgClientUserId)),
                new Markup(Locale.Warning.TgSettingsNotSet));
            table.AddRow(new Markup(Locale.WarningMessage(Locale.Info.TgClientUserIsActive)),
                new Markup(Locale.Warning.TgSettingsNotSet));
            table.AddRow(new Markup(Locale.WarningMessage(Locale.Info.TgClientUserStatus)),
                new Markup(Locale.Warning.TgSettingsNotSet));
        }
        else
        {
            User user = TgClient.MySelfUser;
            table.AddRow(new Markup(Locale.InfoMessage(Locale.Info.TgClientUserName)),
                new Markup(user.username));
            table.AddRow(new Markup(Locale.InfoMessage(Locale.Info.TgClientUserId)),
                new Markup(user.id.ToString()));
            table.AddRow(new Markup(Locale.InfoMessage(Locale.Info.TgClientUserIsActive)),
                new Markup(user.IsActive.ToString()));
            table.AddRow(new Markup(Locale.InfoMessage(Locale.Info.TgClientUserStatus)),
                new Markup(user.status.ToString() ?? ""));
        }

        // TG source user name.
        if (string.IsNullOrEmpty(TgClient.TgSettings.SourceUserName))
            table.AddRow(new Markup(Locale.WarningMessage(Locale.Info.TgSettingsSourceUserName)),
                new Markup(Locale.Warning.TgSettingsNotSet));
        else
            table.AddRow(new Markup(Locale.InfoMessage(Locale.Info.TgSettingsSourceUserName)),
                new Markup(TgClient.TgSettings.SourceUserName));

        // Dest dir.
        if (string.IsNullOrEmpty(TgClient.TgSettings.DestDirectory))
            table.AddRow(new Markup(Locale.WarningMessage(Locale.Info.TgSettingsDestDirectory)),
                new Markup(Locale.Warning.TgSettingsNotSet));
        else
        {
            if (!Directory.Exists(TgClient.TgSettings.DestDirectory))
                table.AddRow(new Markup(Locale.WarningMessage(Locale.Info.TgSettingsDestDirectory)),
                    new Markup(Locale.Warning.DirIsNotExists));
            else
                table.AddRow(new Markup(Locale.InfoMessage(Locale.Info.TgSettingsDestDirectory)),
                    new Markup(TgClient.TgSettings.DestDirectory));
        }

        // TG start ID.
        if (TgClient.TgSettings.MessageStartId < 0)
            table.AddRow(new Markup(Locale.WarningMessage(Locale.Info.TgSettingsMessageStartId)),
                new Markup(Locale.Warning.TgSettingsNotSet));
        else
            table.AddRow(new Markup(Locale.InfoMessage(Locale.Info.TgSettingsMessageStartId)),
                new Markup(TgClient.TgSettings.MessageStartId.ToString()));
        
        // TG messages count.
        if (TgClient.TgSettings.MessageCount < 0)
            table.AddRow(new Markup(Locale.WarningMessage(Locale.Info.TgSettingsMessageCount)),
                new Markup(Locale.Warning.TgSettingsNotSet));
        else
            table.AddRow(new Markup(Locale.InfoMessage(Locale.Info.TgSettingsMessageCount)),
                new Markup(TgClient.TgSettings.MessageCount.ToString()));
        
        // TG messages max count.
        if (TgClient.TgSettings.MessageMaxCount < 0)
            table.AddRow(new Markup(Locale.WarningMessage(Locale.Info.TgSettingsMessageMaxCount)),
                new Markup(Locale.Warning.TgSettingsNotSet));
        else
            table.AddRow(new Markup(Locale.InfoMessage(Locale.Info.TgSettingsMessageMaxCount)),
                new Markup(TgClient.TgSettings.MessageMaxCount.ToString()));
    }

    internal string GetTableRowsSize(long value)
    {
        if (value > 0)
        {
            if (value < 1024)
                return $"{value:##0.000} B";
            if (value < 1024 * 1024)
                return $"{(double)value / 1024:##0.000} KB";
            return value < 1024 * 1024 * 1024 
                ? $"{(double)value / 1024L / 1024:##0.000} MB" 
                : $"{(double)value / 1024L / 1024L / 1024:##0.000} GB";
        }
        return "0 B";
    }

    internal void SetOptionsSetupTg()
    {
        string subMenu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(Locale.Info.MenuSwitchNumber)
                .PageSize(6)
                .MoreChoicesText(Locale.Info.MoveUpDown)
                .AddChoices(
                    "Return", 
                    "Setup user name", 
                    "Setup download folder", 
                    "Setup message start ID",
                    "Setup messages count"));
        MenuItem = subMenu switch
        {
            "Return" => TgMenu.Return,
            "Setup user name" => TgMenu.SetTgSourceUsername,
            "Setup download folder" => TgMenu.SetTgDestDirectory,
            "Setup message start ID" => TgMenu.SetTgMessageStartId,
            "Setup messages count" => TgMenu.SetTgMessageCount,
            _ => MenuItem
        };
    }

    internal string GetStatusInfo(Stopwatch sw) => 
        $"{sw.Elapsed} | {SourceFileProgress:##0.000} % | {GetTableRowsSize(SourceFileRead)} |";

    public bool CheckTgSettings() => 
        TgClient.IsConnected &&
        !string.IsNullOrEmpty(TgClient.TgSettings.SourceUserName) && 
        !string.IsNullOrEmpty(TgClient.TgSettings.DestDirectory);

    #endregion
}