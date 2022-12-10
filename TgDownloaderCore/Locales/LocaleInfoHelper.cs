// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderCore.Locales;

public class LocaleInfoHelper
{
    #region Design pattern "Lazy Singleton"

    private static LocaleInfoHelper _instance;
    public static LocaleInfoHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields and properties

    public string AnyKey => "[green]Type any key[/]";
    public string AppName => "[green]Name[/]";
    public string AppStatus => "[green]Status[/]";
    public string AppTitle => "TG-Downloader";
    public string AppValue => "[green]Value[/]";
    public string AppVersion => "[green]App version[/]";
    public string DestDirectory => "[green]Enter destination directory:[/]";
    public string FileWrite(string file) => $"[green]File '{file}' is open for write.[/]";
    public string MenuDownloadFiles => "Download files";
    public string MenuReturn => "[green]Type any key to return into the main menu.[/]";
    public string MenuSetupTg => "Setup TG";
    public string MenuSwitchNumber => "[green]Switch menu number:[/]";
    public string Message(string message) => $"[green]i {message}[/]";
    public string MoveUpDown => "[grey](Move up and down to switch select)[/]";
    public string SourceFileInfo(string file) => $"[green]Source file: '{file}'.[/]";
    public string StatusException => "[green]Exception:[/]";
    public string StatusFinish(Stopwatch sw) => $"[green]Job is finished. Elapsed time: {sw.Elapsed}.[/]";
    public string StatusInnerException => "[green]Inner exception:[/]";
    public string StatusStart => "[green]Job is started.[/]";
    public string TgClientUserId => "[green]My user ID[/]";
    public string TgClientUserIsActive => "[green]My user active[/]";
    public string TgClientUserName => "[green]My user name[/]";
    public string TgClientUserStatus => "[green]My user status[/]";
    public string TgSettingsDestDirectory => "[green]Dest directory[/]";
    public string TgSettingsSourceUserName => "[green]Source user name[/]";
    public string TgSettingsMessageStartId => "[green]Message start ID[/]";
    public string TgSettingsMessageCount => "[green]Messages count[/]";
    public string TgSetupApiHash => "[green]Type API hash: [/]";
    public string TgSetupAppId => "[green]Type APP ID: [/]";
    public string TgSetupCode => "[green]Type code: [/]";
    public string TgSetupFirstName => "[green]Type first name: [/]";
    public string TgSetupLastName => "[green]Type last name: [/]";
    public string TgSetupNotifications => "[green]Type use notifications: [/]";
    public string TgSetupPassword => "[green]Type password: [/]";
    public string TgSetupPhone => "[green]Type phone number: [/]";
    public string[] MenuMain => new[] { "Exit", "Connect", "Settings", "Download files" };

    #endregion
}