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
    [UID] CHAR(36) NOT NULL COLLATE NOCASE,
    [API_HASH] CHAR(36),
    [API_ID] INT,
    [PHONE_NUMBER] NVARCHAR(16),
    [PROXY_UID] CHAR(36),
    PRIMARY KEY ([UID])
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
    [UID] CHAR(36) NOT NULL COLLATE NOCASE,
    [SOURCE_ID] INT(20), 
    [ID] INT(20), 
    [MESSAGE_ID] INT(20), 
    [FILE_NAME] NVARCHAR(100), 
    [FILE_SIZE] INT(20), 
    [ACCESS_HASH] INT(20),
    PRIMARY KEY ([UID])
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
    [UID] CHAR(36) NOT NULL COLLATE NOCASE,
    [IS_ENABLED] BIT,
    [FILTER_TYPE] INT,
    [NAME] NVARCHAR(128),
    [MASK] NVARCHAR(128),
    [SIZE] INT(20),
    [SIZE_TYPE] INT,
    PRIMARY KEY ([UID])
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
    [UID] CHAR(36) NOT NULL COLLATE NOCASE,
    [SOURCE_ID] INT(20), 
    [ID] INT(20), 
    [DT_CREATED] DATETIME, 
    [TYPE] INT, 
    [SIZE] INT(20), 
    [MESSAGE] NVARCHAR(100),
    PRIMARY KEY ([UID])
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
    [UID] CHAR(36) NOT NULL COLLATE NOCASE,
    [TYPE] INT, 
    [HOST_NAME] NVARCHAR(128), 
    [PORT] INT(5), 
    [USER_NAME] NVARCHAR(128), 
    [PASSWORD] NVARCHAR(128), 
    [SECRET] NVARCHAR(128),
    PRIMARY KEY ([UID])
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
    [UID] CHAR(36) NOT NULL COLLATE NOCASE,
    [ID] INT(20), 
    [USER_NAME] NVARCHAR(128), 
    [TITLE] NVARCHAR(256), 
    [ABOUT] NVARCHAR(1024), 
    [COUNT] INT, 
    [DIRECTORY] NVARCHAR(256), 
    [FIRST_ID] INT, 
    [IS_AUTO_UPDATE] BIT, 
    [DT_CHANGED] DATETIME,
    PRIMARY KEY ([UID])
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
    [UID] CHAR(36) NOT NULL COLLATE NOCASE,
    [VERSION] SMALLINT, 
    [DESCRIPTION] NVARCHAR(128),
    PRIMARY KEY ([UID])
);
INSERT INTO [VERSIONS_TEMP] SELECT * FROM [VERSIONS];
DROP TABLE [VERSIONS];
ALTER TABLE [VERSIONS_TEMP] RENAME TO [VERSIONS];
COMMIT TRANSACTION;
		".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');

	#endregion
}