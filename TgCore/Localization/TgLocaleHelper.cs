// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgCore.Localization;

public class TgLocaleHelper : IHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgLocaleHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgLocaleHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public LanguageLocale Language { get; set; }
    public string InfoMessage(string message, bool isUseX = false) => !isUseX ? $"[green]✓ {message}[/]" : $"[green]x {message}[/]";
    public string WarningMessage(string message) => $"[red]x {message}[/]";

    public TgLocaleHelper()
    {
        Language = LanguageLocale.English;
    }

    #endregion

    #region Public and private fields, properties, constructor - App

    public string AppName => "Name";
    public string AppTitle => "TG-Downloader";
    public string AppValue => "Value";
    public string AppVersion => "App version";

    #endregion

    #region Public and private fields, properties, constructor - Main menu

    public string MenuMain => "Main menu";
    public string MenuMainAppSettings => "Application settings";
    public string MenuMainAdvanced => "Advanced";
    public string MenuMainClient => "Client settings";
    public string MenuMainDownload => "Download settings";
    public string MenuMainExit => "Exit";
    public string MenuMainReset => "Reset";
    public string MenuMainReturn => "Return";
    public string MenuMainStorage => "Storage settings";
    public string MenuYes => "Yes";
    public string MenuNo => "No";

    #endregion

    #region Public and private fields, properties, constructor - App settings

    public string MenuAppFileSession => "Setting the path to the session file";
    public string MenuAppFileStorage => "Setting the path to the storage file";
    public string MenuAppUseProxy => "Usage proxy";
    public string MenuAppUseProxyEnable => "Enable proxy";
    public string MenuAppUseProxyDisable => "Disable proxy";

    #endregion

    #region Public and private fields, properties, constructor - Storage

    public string MenuStorageClearTables => "Clear tables";
    public string MenuStorageCreateNew => "Create new storage";
    public string MenuStorageCreateTables => "Create tables";
    public string MenuStorageDeleteExists => "Delete exists storage";
    public string MenuStorageDropTables => "Drop tables";
    public string MenuStorageViewStatistics => "View statistics";

    #endregion

    #region Public and private fields, properties, constructor - Client

    public string MenuClientConnect => "Connect the client to TG server";
    public string MenuClientGetInfo => "Get info";

    #endregion

    #region Public and private fields, properties, constructor - Download

    public string MenuDownloadAuto => "Auto download";
    public string MenuDownloadManual => "Manual download";
    public string MenuDownloadSetFolder => "Setup download folder";
    public string MenuDownloadSetIsAddMessageId => "Enable join message ID with file name";
    public string MenuDownloadSetIsAutoUpdate => "Enable auto update";
    public string MenuDownloadSetIsRewriteFiles => "Enable rewrite exists files";
    public string MenuDownloadSetIsRewriteMessages => "Enable rewrite exists messages";
    public string MenuDownloadSetSource => "Setup source (ID/username)";
    public string MenuDownloadSetSourceFirstIdAuto => "Setup first ID auto";
    public string MenuDownloadSetSourceFirstIdManual => "Setup first ID manual";
    public string MenuSaveSettings => "Save settings";
    public string MenuScanSources => "Scan local sources";
    public string MenuSetProxy => "Setup proxy";
    public string MenuViewSources => "View local sources";

    #endregion

    #region Public and private fields, properties, constructor

    public string DirIsNotExists => "Directory is not exists!";
    public string DirIsNotExistsSpecify(string dir) => string.IsNullOrEmpty(dir) ? "Directory is not exists!" : $"Directory \"{dir}\" is not exists!";
    public string Empty => "<Empty>";
    public string FileIsAlreadyExists => "File is already exists!";
    public string FileIsAlreadyExistsSpecify(string file) => $"File \"{file}\" is already exists!";
    public string FileIsExists => "file is exists";
    public string FileIsExistsSpecify(string file) => $"File \"{file}\" is exists.";
    public string FileIsNotExists => "File is not exists!";
    public string FileIsNotExistsSpecify(string file) => $"File \"{file}\" is not exists!";
    public string IsFalse => "False";
    public string IsTrue => "True";
    public string MenuSwitchNumber => "  Switch menu number";
    public string MoveUpDown => "(Move up and down to switch select)";
    public string SettingIsDisabled => "Setting is disabled";
    public string SettingIsEnabled => "Setting is enabled";
    public string SettingsIsNeedSetup => "Something is need setup";
    public string SettingsIsOk => "Everything is ok";
    public string SettingsSource => "Source ID/username";
    public string StatusException => "Exception:";
    public string StatusFinish(Stopwatch sw) => $"Job is finished. Elapsed time: {sw.Elapsed}.";
    public string StatusInnerException => "Inner exception:";
    public string FileSession => "File session";
    public string FileSessionExists => "File session is exists";
    public string FileStorage => "File storage";
    public string FileStorageExists => "File storage is exists";
    public string FileStorageName => "Storage file name";
    public string FileStorageSetDefaultName(string fileName) => $"Set default file name '{fileName}'";
    public string FileStorageTablesExists => "Tables are exists";
    public string TablesAreExists => "Tables are exists";
    public string TablesAreNotExists => "Tables are not exists";
    public string TgClientException => "Client exception";
    public string TgClientProxyException => "Proxy exception";
    public string TgClientProxyHostName => "Proxy hostname";
    public string TgClientProxyPort => "Proxy port";
    public string TgClientProxySecret => "Proxy secret";
    public string TgClientProxySetup => "Proxy setup";
    public string TgClientProxyType => "Proxy type";
    public string TgClientProxyUsage => "Proxy usage";
    public string TgClientSetupCompleteSuccess => "The TG client setup was completed successfully";
    public string TgClientSetupCompleteError => "The TG client setup was completed with errors";
    public string TgClientUserId => "User ID";
    public string TgClientUserIsActive => "User active";
    public string TgClientUserName => "User name";
    public string TgGetDialogsInfo => "Getting all dialogs info";
    public string TgGetInfoComplete => "Get TG info was complete";
    public string TgMustClientConnect => "You must connect the client before";
    public string TgMustSetSettings => "You must setup the settings before";
    public string TgSettings => "TG settings";
    public string TgSettingsDestDirectory => "Destination";
    public string TgSettingsIsJoinFileNameWithMessageId => "Join message ID";
    public string TgSettingsIsRewriteFiles => "Rewrite files";
    public string TgSettingsIsRewriteMessages => "Rewrite messages";
    public string TgSettingsSourceFirstLastId => "First/last ID";
    public string TgSetupApiHash => "Type API hash";
    public string TgSetupAppId => "Type APP ID";
    public string TgSetupCode => "Type code";
    public string TgSetupFirstName => "Type first name";
    public string TgSetupLastName => "Type last name";
    public string TgSetupNotifications => "Type use notifications";
    public string TgSetupPassword => "Type password";
    public string TgSetupPhone => "Type phone number";
    public string TypeAnyKeyForReturn => "Type any key to return into the main menu";
    public string TypeDestDirectory => "Type destination directory";
    public string TypeTgProxyHostName => "Type the proxy host name or ip-address";
    public string TypeTgProxyPort => "Type the proxy port";
    public string TypeTgProxySecret => "Type the secret";
    public string TypeTgProxyType => "Type the proxy type";
    public string TypeTgSourceFirstId => "Type the source first ID";

    #endregion

    #region Public and private methods - ISerializable

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected TgLocaleHelper(SerializationInfo info, StreamingContext context)
    {
        //
    }

    /// <summary>
    /// Get object data for serialization info.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        //
    }

    #endregion
}