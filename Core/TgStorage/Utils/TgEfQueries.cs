// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgStorage.Utils;

public static class TgEfQueries
{
	#region Queries

	public static string QueryAlterApps = @"
-- APPS_ALTER
BEGIN TRANSACTION;
CREATE TABLE [APPS_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [API_HASH] char(36),
    [API_ID] int,
    [PHONE_NUMBER] nvarchar(16),
    [PROXY_UID] char(36), 
    primary key ([UID])
);
INSERT INTO [APPS_TEMP] SELECT * FROM [APPS];
DROP TABLE [APPS];
ALTER TABLE [APPS_TEMP] RENAME TO [APPS];
COMMIT TRANSACTION;
		".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');

	public static string QueryAlterDocuments = @"
-- DOCUMENTS_ALTER
BEGIN TRANSACTION;
CREATE TABLE [DOCUMENTS_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [SOURCE_ID] numeric(20,0), 
    [ID] numeric(20,0), 
    [MESSAGE_ID] numeric(20,0), 
    [FILE_NAME] nvarchar(100), 
    [FILE_SIZE] numeric(20,0), 
    [ACCESS_HASH] numeric(20,0),
    primary key ([UID])
);
INSERT INTO [DOCUMENTS_TEMP] SELECT * FROM [DOCUMENTS];
DROP TABLE [DOCUMENTS];
ALTER TABLE [DOCUMENTS_TEMP] RENAME TO [DOCUMENTS];
COMMIT TRANSACTION;
		".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');

	public static string QueryAlterFilters = @"
-- FILTERS_ALTER
BEGIN TRANSACTION;
CREATE TABLE [FILTERS_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [IS_ENABLED] bit, 
    [FILTER_TYPE] int, 
    [NAME] nvarchar(128), 
    [MASK] nvarchar(128), 
    [SIZE] numeric(20,0), 
    [SIZE_TYPE] int,
    primary key ([UID])
);
INSERT INTO [FILTERS_TEMP] SELECT * FROM [FILTERS];
DROP TABLE [FILTERS];
ALTER TABLE [FILTERS_TEMP] RENAME TO [FILTERS];
COMMIT TRANSACTION;
		".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');


	public static string QueryAlterMessages = @"
-- MESSAGES_ALTER
BEGIN TRANSACTION;
CREATE TABLE [MESSAGES_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [SOURCE_ID] numeric(20,0), 
    [ID] numeric(20,0), 
    [DT_CREATED] datetime, 
    [TYPE] int, 
    [SIZE] numeric(20,0), 
    [MESSAGE] nvarchar(100),
    primary key ([UID])
);
INSERT INTO [MESSAGES_TEMP] SELECT * FROM [MESSAGES];
DROP TABLE [MESSAGES];
ALTER TABLE [MESSAGES_TEMP] RENAME TO [MESSAGES];
COMMIT TRANSACTION;
		".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');


	public static string QueryAlterProxies = @"
-- PROXIES_ALTER
BEGIN TRANSACTION;
CREATE TABLE [PROXIES_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [TYPE] int, 
    [HOST_NAME] nvarchar(128), 
    [PORT] numeric(5,0), 
    [USER_NAME] nvarchar(128), 
    [PASSWORD] nvarchar(128), 
    [SECRET] nvarchar(128),
    primary key ([UID])
);
INSERT INTO [PROXIES_TEMP] SELECT * FROM [PROXIES];
DROP TABLE [PROXIES];
ALTER TABLE [PROXIES_TEMP] RENAME TO [PROXIES];
COMMIT TRANSACTION;
		".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');

	public static string QueryAlterSources = @"
-- SOURCES_ALTER
BEGIN TRANSACTION;
CREATE TABLE [SOURCES_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [ID] numeric(20,0), 
    [USER_NAME] nvarchar(100), 
    [TITLE] nvarchar(100), 
    [ABOUT] nvarchar(100), 
    [COUNT] int, 
    [DIRECTORY] nvarchar(100), 
    [FIRST_ID] int, 
    [IS_AUTO_UPDATE] bit, 
    [DT_CHANGED] datetime,
    primary key ([UID])
);
INSERT INTO [SOURCES_TEMP] SELECT * FROM [SOURCES];
DROP TABLE [SOURCES];
ALTER TABLE [SOURCES_TEMP] RENAME TO [SOURCES];
COMMIT TRANSACTION;
		".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');

	public static string QueryAlterVersions = @"
-- VERSIONS_ALTER
BEGIN TRANSACTION;
CREATE TABLE [VERSIONS_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [VERSION] smallint, 
    [DESCRIPTION] nvarchar(128),
    primary key ([UID])
);
INSERT INTO [VERSIONS_TEMP] SELECT * FROM [VERSIONS];
DROP TABLE [VERSIONS];
ALTER TABLE [VERSIONS_TEMP] RENAME TO [VERSIONS];
COMMIT TRANSACTION;
		".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');

	#endregion
}