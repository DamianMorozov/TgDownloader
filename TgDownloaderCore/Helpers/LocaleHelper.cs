// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderCore.Helpers;

public class LocaleHelper
{
    #region Design pattern "Lazy Singleton"

    private static LocaleHelper _instance;
    public static LocaleHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private methods

    public string InfoMessage(string message) => $"[green]i {message}[/]";
    public string WarningMessage(string message) => $"[red]x {message}[/]";

    public string AppMainMenu => "Main menu";
    public string AppName => "Name";
    public string AppTitle => "TG-Downloader";
    public string AppValue => "Value";
    public string AppVersion => "App version";
    public string DestDirectory => "Enter destination directory:";
    public string DirIsNotExists => "Directory is not exists!";
    public string DirNotFound(string dir) => $"Directory '{dir}' is not found!";
    public string MenuDownloadFiles => "Download files";
    public string MenuReturn => "Type any key to return into the main menu.";
    public string MenuSwitchNumber => "Switch menu number: ";
    public string MoveUpDown => "(Move up and down to switch select)";
    public string StatusException => "Exception: ";
    public string StatusFinish(Stopwatch sw) => $"Job is finished. Elapsed time: {sw.Elapsed}.";
    public string StatusInnerException => "Inner exception: ";
    public string StatusStart => "Job is started";
    public string TgClientConnect => "Connect the TG client";
    public string TgClientGetInfo => "Info about me";
    public string TgClientSetupComplete => "Setup the TG client was complete";
    public string TgClientUserId => "My user ID";
    public string TgClientUserIsActive => "My user active";
    public string TgClientUserName => "My user name";
    public string TgClientUserStatus => "My user status";
    public string TgGetDialogsInfo => "Getting all dialogs info";
    public string TgGetInfoComplete => "Get TG info was complete";
    public string TgMustClientConnect => "You must connect the client before";
    public string TgMustSetSettings => "You must setup the settings before";
    public string TgSettings => "Settings";
    public string TgSettingsDestDirectory => "Dest directory";
    public string TgSettingsMessageCount => "Messages count";
    public string TgSettingsMessageStartId => "Message current ID";
    public string TgSettingsNotSet => "<not set>";
    public string TgSettingsSourceUserName => "Source user name";
    public string TgSetupApiHash => "Type API hash: ";
    public string TgSetupAppId => "Type APP ID: ";
    public string TgSetupCode => "Type code: ";
    public string TgSetupFirstName => "Type first name: ";
    public string TgSetupLastName => "Type last name: ";
    public string TgSetupNotifications => "Type use notifications: ";
    public string TgSetupPassword => "Type password: ";
    public string TgSetupPhone => "Type phone number: ";
    public string TypeTgMessageStartId => "Type the message start ID (example: 1): ";
    public string TypeTgSourceUserName => "Type the source user name (example: https://t.me/some_channel): ";
    public string[] MenuMain => new[] { "Exit", "Connect", "Info", "Settings", "Download files" };

    #endregion
}