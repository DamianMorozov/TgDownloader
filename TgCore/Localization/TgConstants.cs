// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgCore.Localization;

public static class TgConstants
{
	#region App

	public const string AppName = "Name";
	public const string AppTitle = "TG-Downloader";
	public const string AppValue = "Value";
	public const string AppVersion = "App version";
	public const string StorageVersion = "Storage version";

	public const string MenuAppFileSession = "Setting the path to the session file";
	public const string MenuAppFileStorage = "Setting the path to the storage file";
	public const string MenuAppUseProxy = "Usage proxy";
	public const string MenuAppUseProxyEnable = "Enable proxy";
	public const string MenuAppUseProxyDisable = "Disable proxy";

	#endregion

	#region Advanced

	public const string AdvancedSaveSourceInfo = "Save source info";
	public const string CollectChats = "Collect all chats...";
	public const string CollectDialogs = "Collect all dialogs ...";

	#endregion

	#region Client

	public const string MenuClientConnect = "Connect the client to TG server";
	public const string MenuClientDisconnect = "Disconnect the client from TG server";

	#endregion

	#region Download

	public const string MenuDownloadAuto = "Auto download";
	public const string MenuDownloadManual = "Manual download";
	public const string MenuDownloadSetFolder = "Setup download folder";
	public const string MenuDownloadSetIsAddMessageId = "Enable join message ID with file name";
	public const string MenuDownloadSetIsAutoUpdate = "Enable auto update";
	public const string MenuDownloadSetIsRewriteFiles = "Enable rewrite exists files";
	public const string MenuDownloadSetIsRewriteMessages = "Enable rewrite exists messages";
	public const string MenuDownloadSetSource = "Setup source (ID/username)";
	public const string MenuDownloadSetSourceFirstIdAuto = "Setup first ID auto";
	public const string MenuDownloadSetSourceFirstIdManual = "Setup first ID manual";
	public const string MenuSaveSettings = "Save settings";
	public const string MenuScanChats = "Scan my chats";
	public const string MenuScanDialogs = "Scan my dialogs";
	public const string MenuSetProxy = "Setup proxy";
	public const string MenuViewSources = "View my sources";

	#endregion

	#region Filters

	public const string MenuFiltersAdd = "Add filter";
	public const string MenuFiltersAllCount = "All filters count";
	public const string MenuFiltersEnabledCount = "Enabled filters count";
	public const string MenuFiltersEdit = "Edit filter";
	public const string MenuFiltersError = "Error";
	public const string MenuFiltersRemove = "Remove filter";
	public const string MenuFiltersReset = "Reset filters";
	public const string MenuFiltersSetEnabled = "Set filter enabled";
	public const string MenuFiltersSetIsEnabled = "Is enabled";
	public const string MenuFiltersSetMask = "Set mask";
	public const string MenuFiltersSetMaxSize = "File maximum size";
	public const string MenuFiltersSetMinSize = "File minimum size";
	public const string MenuFiltersSetMultiExtension = "Multi extension";
	public const string MenuFiltersSetMultiName = "Multi name";
	public const string MenuFiltersSetName = "Set name";
	public const string MenuFiltersSetSingleExtension = "Single extension";
	public const string MenuFiltersSetSingleName = "Single name";
	public const string MenuFiltersSetSizeType = "Set file size type";
	public const string MenuFiltersSetType = "Set filter type";
	public const string MenuFiltersView = "View filters";

	#endregion

	#region Main menu

	public const string MenuMain = "Main menu";
	public const string MenuMainAdvanced = "Advanced";
	public const string MenuMainApp = "Application";
	public const string MenuMainClient = "Client";
	public const string MenuMainDownload = "Download";
	public const string MenuMainExit = "Exit";
	public const string MenuMainFilters = "Filters";
	public const string MenuMainReset = "Reset";
	public const string MenuMainReturn = "Return";
	public const string MenuMainStorage = "Storage";

	#endregion

	#region Menu

	public const string MenuNo = "No";
	public const string MenuYes = "Yes";
	public const string MenuSwitchNumber = "Switch menu number";
	public const string MenuIsFalse = "False";
	public const string MenuIsTrue = "True";

	#endregion

	#region Storage

	public const string MenuStorageBackupDirectory = "Backup directory";
	public const string MenuStorageBackupFailed = "Backup storage was failed";
	public const string MenuStorageBackupFile = "Backup file";
	public const string MenuStorageBackupSuccess = "Backup storage was successful";
	public const string MenuStorageDbBackup = "Create backup";
	public const string MenuStorageDbCreateNew = "Create new storage";
	public const string MenuStorageDbDeleteExists = "Delete exists storage";
	public const string MenuStorageExitProgram = "Exit the program";
	public const string MenuStoragePerformSteps = "Perform the following set of steps";
	public const string MenuStorageTablesClear = "Clear tables";
	public const string MenuStorageTablesClearFinished = "Clear tables was finished";
	public const string MenuStorageTablesVersionsView = "Versions info";

	#endregion
}