// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgLocalization.Helpers;

/// <summary>
/// Localization helper.
/// </summary>
public sealed partial class TgLocaleHelper : ObservableObject, INotifyPropertyChanged
{
	#region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	private static TgLocaleHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public static TgLocaleHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

	#endregion

	public override string ToString() => Language.ToString();

	#region App

	public string AppName => "Name";
	public string AppTitle => "TG-Downloader";
	public string AppTitleConsole => "TG-Downloader Console app";
	public string AppValue => "Value";
	public string AppVersion => "App version";
	public string From => "from";
	public string HomeLoadAllFromXml => "Load all from XML";
	public string HomeResetToDefault => "Reset to default";
	public string HomeSaveToXml => "Save to XML";
	public string InDevelopment => "In development";
	public string MenuAppFileSession => "Setting the path to the session file";
	public string MenuAppFileStorage => "Setting the path to the storage file";
	public string MenuAppUseProxy => "Usage proxy";
	public string MenuAppUseProxyDisable => "Disable proxy";
	public string MenuAppUseProxyEnable => "Enable proxy";
	public string MenuException => "Exception";
	public string MenuLastException => "Last exception";
	public string MenuState => "State";
	public string Messages => "messages";
	public string StorageVersion => "Storage version";

	#endregion

	#region Advanced

	public string AdvancedSaveSourceInfo => "Save source info";
	public string CollectChats => "Collect all chats...";
	public string CollectDialogs => "Collect all dialogs ...";
	public string GetSourceInfo => "Get source info";

	#endregion

	#region Client

	public string MenuClientApiHash => "API hash";
	public string MenuClientApiId => "API ID";
	public string MenuClientClearSettings => "Clear settings";
	public string MenuClientConnect => "Connect the client to TG server";
	public string MenuClientDisconnect => "Disconnect the client from TG server";
	public string MenuClientException => "Client exception";
	public string MenuClientFirstName => "First name";
	public string MenuClientIsReady => "Client is ready";
	public string MenuClientLastName => "Last name";
	public string MenuClientLoadAllFromStorage => "Load all from storage";
	public string MenuClientNotifications => "Notifications";
	public string MenuClientPassword => "Password";
	public string MenuClientPhoneNumber => "Phone number";
	public string MenuClientProgressStatus => "Progress status";
	public string MenuClientProxy => "Proxy";
	public string MenuClientSaveToStorage => "Save to storage";
	public string MenuClientTelegramQuery => "Telegram query";
	public string MenuClientVerificationCode => "Verification code";
	public string MenuClientIsNotReady => "Client is not ready";
	public string MenuClientIsQuery => "Client is query";

	#endregion

	#region Download

	public string MenuAutoDownload => "Auto download";
	public string MenuAutoViewEvents => "Auto view events";
	public string MenuManualDownload => "Manual download";
	public string MenuDownloadSetFolder => "Setup download folder";
	public string MenuDownloadSetIsAddMessageId => "Enable join message ID with file name";
	public string MenuDownloadSetIsAutoUpdate => "Enable auto update";
	public string MenuDownloadSetIsRewriteFiles => "Enable rewrite exists files";
	public string MenuDownloadSetIsRewriteMessages => "Enable rewrite exists messages";
	public string MenuDownloadSetSource => "Setup source (ID/username)";
	public string MenuDownloadSetSourceFirstIdAuto => "Setup first ID auto";
	public string MenuDownloadSetSourceFirstIdManual => "Setup first ID manual";
	public string MenuSaveSettings => "Save settings";
	public string MenuScanChats => "Scan my chats";
	public string MenuScanDialogs => "Scan my dialogs";
	public string MenuSetProxy => "Setup proxy";
	public string MenuViewSources => "View sources";

	#endregion

	#region Filters

	public string MenuFiltersAdd => "Add filter";
	public string MenuFiltersAllCount => "All filters count";
	public string MenuFiltersEnabledCount => "Enabled filters count";
	public string MenuFiltersEdit => "Edit filter";
	public string MenuFiltersError => "Error";
	public string MenuFiltersRemove => "Remove filter";
	public string MenuFiltersReset => "Reset filters";
	public string MenuFiltersSetEnabled => "Set filter enabled";
	public string MenuFiltersSetIsEnabled => "Is enabled";
	public string MenuFiltersSetMask => "Set mask";
	public string MenuFiltersSetMaxSize => "File maximum size";
	public string MenuFiltersSetMinSize => "File minimum size";
	public string MenuFiltersSetMultiExtension => "Multi extension";
	public string MenuFiltersSetMultiName => "Multi name";
	public string MenuFiltersSetName => "Set name";
	public string MenuFiltersSetSingleExtension => "Single extension";
	public string MenuFiltersSetSingleName => "Single name";
	public string MenuFiltersSetSizeType => "Set file size type";
	public string MenuFiltersSetType => "Set filter type";
	public string MenuFiltersView => "View filters";

	#endregion

	#region Main menu

	public string MenuMain => "Main menu";
	public string MenuMainAbout => "About";
	public string MenuMainAdvanced => "Advanced";
	public string MenuMainApp => "Application";
	public string MenuMainClient => "Client";
	public string MenuMainDownload => "Download";
	public string MenuMainExit => "Exit";
	public string MenuMainProxies => "Proxies";
	public string MenuMainFilters => "Filters";
	public string MenuMainReset => "Reset";
	public string MenuMainReturn => "Return";
	public string MenuMainStorage => "Storage";

	#endregion

	#region Menu

	public string MenuNo => "No";
	public string MenuYes => "Yes";
	public string MenuSwitchNumber => "Switch menu number";
	public string MenuIsFalse => "False";
	public string MenuIsTrue => "True";

	#endregion

	#region Storage

	public string MenuStorageBackupDirectory => "Backup directory";
	public string MenuStorageBackupFailed => "Backup storage was failed";
	public string MenuStorageBackupFile => "Backup file";
	public string MenuStorageBackupSuccess => "Backup storage was successful";
	public string MenuStorageDbBackup => "Create backup";
	public string MenuStorageDbCreateNew => "Create new storage";
	public string MenuStorageDbDeleteExists => "Delete exists storage";
	public string MenuStorageExitProgram => "Exit the program";
	public string MenuStoragePerformSteps => "Perform the following set of steps";
	public string MenuStorageTablesClear => "Clear tables";
	public string MenuStorageTablesClearFinished => "Clear tables was finished";
	public string MenuStorageTablesVersionsView => "Versions info";

	#endregion

	#region Public and private fields, properties, constructor

	public string ClearView => "Clear view";
	public string DirectoryCreate => "Create directory";
	public string DirectoryCreateIsException(Exception ex) => $"Exception of create directory: {(ex.InnerException is null ? ex.Message : ex.Message + $" | {ex.InnerException.Message}")}";
	public string DirectoryDestType => "Type destination directory";
	public string DirectoryIsNotExists(string dir = "") => string.IsNullOrEmpty(dir) ? "Th directory is not exists!" : $"The directory \"{dir}\" is not exists!";
	public string Empty => "<Empty>";
	public string FileIsAlreadyExists => "File is already exists!";
	public string FileIsAlreadyExistsSpecify(string file) => $"File \"{file}\" is already exists!";
	public string FileIsExists => "file is exists";
	public string FileIsExistsSpecify(string file) => $"File \"{file}\" is exists.";
	public string FileIsNotExists => "File is not exists!";
	public string FileIsNotExistsSpecify(string file) => $"File \"{file}\" is not exists!";
	public string FileSession => "File session";
	public string FileSessionExists => "File session is exists";
	public string FileStorage => "File Storage";
	public string FileStorageExists => "File storage is exists";
	public string FileStorageName => "Storage file name";
	public string FileStorageSetDefaultName(string fileName) => $"Set default file name '{fileName}'";
	public string FileStorageTablesExists => "Tables are exists";
	public string Load=> "Load";
	public string LoadStorage => "Load Storage";
	public string UpdateFromStorage => "Update from Storage";
	public string LoadTelegram => "Load Telegram";
	public string LoadOnline => "Load online";
	public string GetSources => "Get sources";
	public string Scan => "Scan";
	public string Update => "Update";
	public string UpdateOnline => "Update online";
	public string MoveUpDown => "(Move up and down to switch select)";
	public string Save => "Save";
	public string SaveToStorage=> "Save to Storage";
	public string Settings => "Settings";
	public string SettingCheck => "Check";
	public string SettingIsDisabled => "Setting is disabled";
	public string SettingIsEnabled => "Setting is enabled";
	public string SettingName => "Setting";
	public string SettingsDtChanged => "Changed";
	public string SettingsIsNeedSetup => "Something is need setup";
	public string SettingsIsOk => "Everything is ok";
	public string SettingsSource => "Source info";
	public string SettingValue => "Value";
	public string SortView => "Sort view";
	public string StatusException => "Exception";
	public string StatusFinish(Stopwatch sw) => $"Job is finished. Elapsed time: {sw.Elapsed}.";
	public string StatusInnerException => "Inner exception";
	public string TablesAppsException => "Table APPS exception!";
	public string TablesDocumentsException => "Table DOCUMENTS exception!";
	public string TablesFiltersException => "Table FILTERS exception!";
	public string TablesMessagesException => "Table MESSAGES exception!";
	public string TablesProxiesException => "Table PROXIES exception!";
	public string TablesSourcesException => "Table SOURCES exception!";
	public string TablesVersionsException => "Table VERSIONS exception!";
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
	public string TgSetupFirstName => "Type first name";
	public string TgSetupLastName => "Type last name";
	public string TgSetupNotifications => "Type use notifications";
	public string TgSetupPassword => "Type password";
	public string TgSetupPhone => "Type phone number";
	public string TgVerificationCode => "Verification code";
	public string TypeAnyKeyForReturn => "Type any key to return into the main menu";
	public string TypeTgProxyHostName => "Type the proxy host name or ip-address";
	public string TypeTgProxyPort => "Type the proxy port";
	public string TypeTgProxySecret => "Type the secret";
	public string TypeTgProxyType => "Type the proxy type";
	public string TypeTgSourceFirstId => "Type the source first ID";

	#endregion

	#region Public and private fields, properties, constructor

	public TgEnumLanguage Language { get; set; }
	public string InfoMessage(string message, bool isUseX = false) => !isUseX ? $"[green]v {message}[/]" : $"[green]x {message}[/]";
	public string WarningMessage(string message) => $"[red]x {message}[/]";

	public TgLocaleHelper()
	{
		Language = TgEnumLanguage.English;
	}

	#endregion

	#region Menu storage

	public string MenuStorageDbIsNotFound(string fileName) => $"Storage was not found: {fileName}!";
	public string MenuStorageDbIsZeroSize(string fileName) => $"Storage is zero size: {fileName}!";
	public string MenuStorageDeleteExistsInfo(string fileName) => $"Manual delete the file: {fileName}";

	#endregion

	#region Public and private methods

	public string TableSources => "Sources";

	#endregion

	#region Fields

	public string FieldAbout => "About";
	public string FieldAll => "All";
	public string FieldCount => "Count";
	public string FieldDb => "DB";
	public string FieldDir => "Dir";
	public string FieldEdit => "Edit";
	public string FieldFirstId => "First ID";
	public string FieldId => "ID";
	public string FieldIsAutoUpdate => "Auto-update";
	public string FieldIsCountComplete => "Count complete";
	public string FieldLinkTelegramApps => "https://my.telegram.org/apps/";
	public string FieldLinkTgDownloader => "https://github.com/DamianMorozov/TgDownloader";
	public string FieldDown => "Down";
	public string FieldLoad => "Load";
	public string FieldPercent => "Percent";
	public string FieldPercentSpec => "%";
	public string FieldSourceId => "Source ID";
	public string FieldTgDownloader => "TgDownloader on GitHub";
	public string FieldTitle => "Title";
	public string FieldUpd => "Upd";
	public string FieldUserName => "Username";

	#endregion
	
	#region Public and private methods

	public string ActionReloadAllFromStorage => "Reload all from Storage";

	#endregion

	#region System

    public string UseOverrideMethod => "Use override method!";

    #endregion
}