// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

// ReSharper disable InconsistentNaming
namespace TgCore.Localization;

public class TgLocaleHelper : ITgHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgLocaleHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgLocaleHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

	#endregion

	#region Public and private fields, properties, constructor

	public string DirectoryCreate => "Create directory";
	public string DirectoryCreateIsException(Exception ex) => $"Exception of create directory: {(ex.InnerException is null ? ex.Message : ex.Message + $" | {ex.InnerException.Message}")}";
	public string DirectoryDestType => "Type destination directory";
	public string DirectoryIsNotExists(string dir = "") => string.IsNullOrEmpty(dir) ? "Th directory is not exists!" : $"Thd directory \"{dir}\" is not exists!";
	public string Empty => "<Empty>";
	public string FileIsAlreadyExists => "File is already exists!";
	public string FileIsAlreadyExistsSpecify(string file) => $"File \"{file}\" is already exists!";
	public string FileIsExists => "file is exists";
	public string FileIsExistsSpecify(string file) => $"File \"{file}\" is exists.";
	public string FileIsNotExists => "File is not exists!";
	public string FileIsNotExistsSpecify(string file) => $"File \"{file}\" is not exists!";
	public string FileSession => "File session";
	public string FileSessionExists => "File session is exists";
	public string FileStorage => "File storage";
	public string FileStorageExists => "File storage is exists";
	public string FileStorageName => "Storage file name";
	public string FileStorageSetDefaultName(string fileName) => $"Set default file name '{fileName}'";
	public string FileStorageTablesExists => "Tables are exists";
	public string MoveUpDown => "(Move up and down to switch select)";
	public string SettingIsDisabled => "Setting is disabled";
	public string SettingIsEnabled => "Setting is enabled";
	public string SettingsIsNeedSetup => "Something is need setup";
	public string SettingsIsOk => "Everything is ok";
	public string SettingsSource => "Source info";
	public string StatusException => "Exception";
	public string StatusFinish(Stopwatch sw) => $"Job is finished. Elapsed time: {sw.Elapsed}.";
	public string StatusInnerException => "Inner exception";
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
	public string TgClientSetupCompleteError => "The TG client setup was completed with errors";
	public string TgClientSetupCompleteSuccess => "The TG client setup was completed successfully";
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
	public string TypeTgProxyHostName => "Type the proxy host name or ip-address";
	public string TypeTgProxyPort => "Type the proxy port";
	public string TypeTgProxySecret => "Type the secret";
	public string TypeTgProxyType => "Type the proxy type";
	public string TypeTgSourceFirstId => "Type the source first ID";

	#endregion

	#region Public and private fields, properties, constructor

	public TgLanguageLocale Language { get; set; }
    public string InfoMessage(string message, bool isUseX = false) => !isUseX ? $"[green]✓ {message}[/]" : $"[green]x {message}[/]";
    public string WarningMessage(string message) => $"[red]x {message}[/]";

    public TgLocaleHelper()
    {
        Language = TgLanguageLocale.English;
    }

	#endregion

	#region Menu storage

	public string MenuStorageDbIsNotFound(string fileName) => $"Storage was not found: {fileName}!";
	public string MenuStorageDbIsZeroSize(string fileName) => $"Storage is zero size: {fileName}!";
	public string MenuStorageDeleteExistsInfo(string fileName) => $"Manual delete the file: {fileName}";
    
#endregion
}