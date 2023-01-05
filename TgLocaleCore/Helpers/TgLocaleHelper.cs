// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Runtime.Serialization;
using TgLocaleCore.Interfaces;

namespace TgLocaleCore.Helpers;

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
    public string InfoMessage(string message) => $"[green]i {message}[/]";
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
    public string MenuMainExit => "Exit";
    public string MenuMainDownload => "Download settings";
    public string MenuMainStorage => "Storage settings";
    public string MenuMainClient => "Client settings";
    public string MenuMainReturn => "Return";

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

    public string MenuDownload => "Download";
    public string MenuDownloadSetFolder => "Setup download folder";
    public string MenuDownloadSetIsAddMessageId => "Enable join message ID with file name";
    public string MenuDownloadSetIsRewriteFiles => "Enable rewrite exists files";
    public string MenuDownloadSetIsRewriteMessages => "Enable rewrite exists messages";
    public string MenuDownloadSetSourceId => "Setup source ID";
    public string MenuDownloadSetSourceStartIdAuto => "Setup source start ID auto";
    public string MenuDownloadSetSourceStartIdManual => "Setup source start ID manual";
    public string MenuDownloadSetSourceUserName => "Setup source user name";
    public string MenuScanMyChannels => "Scan my channels and groups";

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
    public string MenuSwitchNumber => "  Switch menu number";
    public string MoveUpDown => "(Move up and down to switch select)";
    public string SettingsIsNeedSetup => "Something is need setup";
    public string SettingsIsOk => "Everything is ok";
    public string StatusException => "Exception:";
    public string StatusFinish(Stopwatch sw) => $"Job is finished. Elapsed time: {sw.Elapsed}.";
    public string StatusInnerException => "Inner exception:";
    public string StorageFileExists => "Storage is exists";
    public string StorageFileName => "Storage file name";
    public string StorageSetDefaultFileName(string fileName) => $"Set default file name '{fileName}'";
    public string StorageTablesExists => "Tables are exists";
    public string TablesAreExists => "Tables are exists";
    public string TablesAreNotExists => "Tables are not exists";
    public string TgClientSetupComplete => "Setup the TG client was complete";
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
    public string TgSettingsSourceId => "Source ID";
    public string TgSettingsSourceStartLastId => "Start / last ID";
    public string TgSettingsSourceUserName => "Source user name";
    public string TgSetupApiHash => "Type API hash:";
    public string TgSetupAppId => "Type APP ID:";
    public string TgSetupCode => "Type code:";
    public string TgSetupFirstName => "Type first name:";
    public string TgSetupLastName => "Type last name:";
    public string TgSetupNotifications => "Type use notifications:";
    public string TgSetupPassword => "Type password:";
    public string TgSetupPhone => "Type phone number:";
    public string TypeAnyKeyForReturn => "Type any key to return into the main menu.";
    public string TypeDbFileName => "Type the DB file name:";
    public string TypeDestDirectory => "Type destination directory:";
    public string TypeTgIsAddMessageId => "Type the using join message ID with file name(example: true/false):";
    public string TypeTgIsRewriteFiles => "Type the using files rewrite (example: true/false):";
    public string TypeTgIsRewriteMessages => "Type the using message rewrite (example: true/false):";
    public string TypeTgMessageStartId => "Type the message start ID (example: 1):";
    public string TypeTgSourceId => "Type the source ID:";
    public string TypeTgSourceStartId => "Type the source start ID:";
    public string TypeTgSourceUserName => "Type the source user name:";

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