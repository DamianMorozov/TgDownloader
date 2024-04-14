// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgStorage.Utils;

public static class TgStorageConstants
{
	#region Public and private fields, properties, constructor - Tables

	public const string TableApps = "APPS";
	public const string TableDocuments = "DOCUMENTS";
	public const string TableFilters = "FILTERS";
	public const string TableMessages = "MESSAGES";
	public const string TableProxies = "PROXIES";
	public const string TableSources = "SOURCES";
	public const string TableVersions = "VERSIONS";
	public const string XPObjectType = "XPObjectType";

	public static Guid UidTruncate => Guid.Parse("00000000-0000-0000-0000-000000000001");
	public static Guid UidDrop => Guid.Parse("00000000-0000-0000-0000-000000000002");

	public static IEnumerable<string> GetTableNames()
    {
        yield return TableApps;
        yield return TableDocuments;
        yield return TableFilters;
        yield return TableMessages;
        yield return TableProxies;
        yield return TableSources;
        yield return TableVersions;
    }

    #endregion

	#region Public and private fields, properties, constructor - Columns

	public const string ColumnAbout = "ABOUT";
	public const string ColumnAccessHash = "ACCESS_HASH";
	public const string ColumnApiHash = "API_HASH";
	public const string ColumnApiId = "API_ID";
	public const string ColumnCount = "COUNT";
	public const string ColumnDescription = "DESCRIPTION";
	public const string ColumnDirectory = "DIRECTORY";
	public const string ColumnDtChanged = "DT_CHANGED";
	public const string ColumnDtCreated = "DT_CREATED";
	public const string ColumnFileName = "FILE_NAME";
	public const string ColumnFileSize = "FILE_SIZE";
	public const string ColumnFilterType = "FILTER_TYPE";
	public const string ColumnFirstId = "FIRST_ID";
	public const string ColumnHostName = "HOST_NAME";
	public const string ColumnId = "ID";
	public const string ColumnIsAutoUpdate = "IS_AUTO_UPDATE";
	public const string ColumnIsEnabled = "IS_ENABLED";
	public const string ColumnMask = "MASK";
	public const string ColumnMessage = "MESSAGE";
	public const string ColumnMessageId = "MESSAGE_ID";
	public const string ColumnName = "NAME";
	public const string ColumnPassword = "PASSWORD";
	public const string ColumnPhoneNumber = "PHONE_NUMBER";
	public const string ColumnPort = "PORT";
	public const string ColumnProxyUid = "PROXY_UID";
	public const string ColumnSecret = "SECRET";
	public const string ColumnSize = "SIZE";
	public const string ColumnSizeType = "SIZE_TYPE";
	public const string ColumnSourceId = "SOURCE_ID";
	public const string ColumnTitle = "TITLE";
	public const string ColumnType = "TYPE";
	public const string ColumnUid = "UID";
	public const string ColumnUserName = "USER_NAME";
	public const string ColumnVersion = "VERSION";
	public const string ColumnRowVersion = "RowVersion";

	#endregion
}