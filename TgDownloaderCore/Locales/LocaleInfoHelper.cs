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

    public string AnyKey => "Type any key";
    public string AppName => "Name";
    public string AppTitle => "TG-Downloader";
    public string AppValue => "Value";
    public string AppVersion => "App version";
    public string DestDirectory => "Enter destination directory:";
    public string MenuDownloadFiles => "Download files";
    public string MenuReturn => "Type any key to return into the main menu.";
    public string MenuSetupTg => "Setup TG";
    public string MenuSwitchNumber => "Switch menu number: ";
    public string MoveUpDown => "(Move up and down to switch select)";
    public string StatusException => "Exception: ";
    public string StatusFinish(Stopwatch sw) => $"Job is finished. Elapsed time: {sw.Elapsed}.";
    public string StatusInnerException => "Inner exception: ";
    public string StatusStart => "Job is started";
    public string TgClientUserId => "My user ID";
    public string TgClientUserIsActive => "My user active";
    public string TgClientUserName => "My user name";
    public string TgClientUserStatus => "My user status";
    public string TgSettingsDestDirectory => "Dest directory";
    public string TgSettingsSourceUserName => "Source user name";
    public string TgSettingsMessageStartId => "Message start ID";
    public string TgSettingsMessageCount => "Messages count";
    public string TgSettingsMessageMaxCount => "Messages max count";
    public string TgSetupApiHash => "Type API hash: ";
    public string TgSetupAppId => "Type APP ID: ";
    public string TgSetupCode => "Type code: ";
    public string TgSetupFirstName => "Type first name: ";
    public string TgSetupLastName => "Type last name: ";
    public string TgSetupNotifications => "Type use notifications: ";
    public string TgSetupPassword => "Type password: ";
    public string TgSetupPhone => "Type phone number: ";
    public string[] MenuMain => new[] { "Exit", "Connect", "Settings", "Download files" };

    #endregion
}