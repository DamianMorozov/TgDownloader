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
        AnsiConsole.Write(new FigletText(Locale.AppTitle).Alignment(Justify.Center).Color(Color.Yellow));
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
            new Markup(Locale.AppName, StyleMain)) { Width = 20 }.LeftAligned());
        table.AddColumn(new TableColumn(
            new Markup(Locale.AppValue, StyleMain)) { Width = 80 }.LeftAligned());
    }

    internal void FillTableRows(Table table)
    {
        if (table.Rows.Count > 0) table.Rows.Clear();

        table.AddRow(new Markup(Locale.InfoMessage(Locale.AppVersion)), new Markup($@"v{GetAppVersion()}"));

        // TG client info.
        if (!TgClient.IsConnected)
        {
            table.AddRow(new Markup(Locale.WarningMessage(Locale.TgClientUserName)),
                new Markup(Locale.TgSettingsNotSet));
            table.AddRow(new Markup(Locale.WarningMessage(Locale.TgClientUserId)),
                new Markup(Locale.TgSettingsNotSet));
            table.AddRow(new Markup(Locale.WarningMessage(Locale.TgClientUserIsActive)),
                new Markup(Locale.TgSettingsNotSet));
            table.AddRow(new Markup(Locale.WarningMessage(Locale.TgClientUserStatus)),
                new Markup(Locale.TgSettingsNotSet));
        }
        else
        {
            User user = TgClient.MySelfUser;
            table.AddRow(new Markup(Locale.InfoMessage(Locale.TgClientUserName)),
                new Markup(user.username));
            table.AddRow(new Markup(Locale.InfoMessage(Locale.TgClientUserId)),
                new Markup(user.id.ToString()));
            table.AddRow(new Markup(Locale.InfoMessage(Locale.TgClientUserIsActive)),
                new Markup(user.IsActive.ToString()));
            table.AddRow(new Markup(Locale.InfoMessage(Locale.TgClientUserStatus)),
                new Markup(user.status.ToString() ?? ""));
        }

        // TG source user name.
        if (string.IsNullOrEmpty(TgClient.TgSettings.SourceUserName))
            table.AddRow(new Markup(Locale.WarningMessage(Locale.TgSettingsSourceUserName)),
                new Markup(Locale.TgSettingsNotSet));
        else
            table.AddRow(new Markup(Locale.InfoMessage(Locale.TgSettingsSourceUserName)),
                new Markup(TgClient.TgSettings.SourceUserName));

        // TG messages count.
        if (TgClient.TgSettings.MessageCount < 0)
            table.AddRow(new Markup(Locale.WarningMessage(Locale.TgSettingsMessageCount)),
                new Markup(Locale.TgSettingsNotSet));
        else
            table.AddRow(new Markup(Locale.InfoMessage(Locale.TgSettingsMessageCount)),
                new Markup(TgClient.TgSettings.MessageCount.ToString()));
        
        // Dest dir.
        if (string.IsNullOrEmpty(TgClient.TgSettings.DestDirectory))
            table.AddRow(new Markup(Locale.WarningMessage(Locale.TgSettingsDestDirectory)),
                new Markup(Locale.TgSettingsNotSet));
        else
        {
            if (!Directory.Exists(TgClient.TgSettings.DestDirectory))
                table.AddRow(new Markup(Locale.WarningMessage(Locale.TgSettingsDestDirectory)),
                    new Markup(Locale.DirIsNotExists));
            else
                table.AddRow(new Markup(Locale.InfoMessage(Locale.TgSettingsDestDirectory)),
                    new Markup(TgClient.TgSettings.DestDirectory));
        }

        // TG start ID.
        if (TgClient.TgSettings.MessageCurrentId < 0)
            table.AddRow(new Markup(Locale.WarningMessage(Locale.TgSettingsMessageStartId)),
                new Markup(Locale.TgSettingsNotSet));
        else
            table.AddRow(new Markup(Locale.InfoMessage(Locale.TgSettingsMessageStartId)),
                new Markup(TgClient.TgSettings.MessageCurrentId.ToString()));
    }

    internal TgMenuSettings SetOptionsSetupTg()
    {
        string menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(Locale.MenuSwitchNumber)
                .PageSize(4)
                .MoreChoicesText(Locale.MoveUpDown)
                .AddChoices(
                    "Return", 
                    "Setup user name", 
                    "Setup download folder", 
                    "Setup message current ID"));
        return menu switch
        {
            "Setup user name" => TgMenuSettings.SourceUsername,
            "Setup download folder" => TgMenuSettings.DestDirectory,
            "Setup message current ID" => TgMenuSettings.MessageCurrentId,
            _ => TgMenuSettings.Return
        };
    }

    internal double CalcSourceProgress(long count, long current) =>
        count == 0 ? 0 : (double)(current * 100) / count;

    private string GetLongString(long current) => current > 999 ? $"{current:### ###}" : $"{current:###}";

    public string GetStatus(Stopwatch sw, long count, long current) =>
        count == 0 && current == 0
            ? $"{sw.Elapsed} | "
            : $"{sw.Elapsed} | {CalcSourceProgress(count, current):#00.00} % | {GetLongString(current)}/{GetLongString(count)}";

    public string GetStatus(long count, long current) =>
        count == 0 && current == 0
            ? string.Empty
            : $"{CalcSourceProgress(count, current):#00.00} % | {GetLongString(current)}/{GetLongString(count)}";

    public bool CheckTgSettings() => 
        TgClient.IsConnected &&
        !string.IsNullOrEmpty(TgClient.TgSettings.SourceUserName) && 
        !string.IsNullOrEmpty(TgClient.TgSettings.DestDirectory);

    #endregion
}