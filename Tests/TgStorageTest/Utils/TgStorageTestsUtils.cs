// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Utils;

internal static class TgStorageTestsUtils
{
	#region Public and private fields, properties, constructor

	internal static TgDataHelper DataCore => TgDataHelper.Instance;
    internal static TgSqlTableAppRepository AppRepository => TgStorageTestsUtils.DataCore.ContextManager.AppRepository;
    internal static TgSqlTableDocumentRepository DocumentRepository => TgStorageTestsUtils.DataCore.ContextManager.DocumentRepository;
    internal static TgSqlTableFilterRepository FilterRepository => TgStorageTestsUtils.DataCore.ContextManager.FilterRepository;
    internal static TgSqlTableMessageRepository MessageRepository => TgStorageTestsUtils.DataCore.ContextManager.MessageRepository;
    internal static TgSqlTableProxyRepository ProxyRepository => TgStorageTestsUtils.DataCore.ContextManager.ProxyRepository;
    internal static TgSqlTableSourceRepository SourceRepository => TgStorageTestsUtils.DataCore.ContextManager.SourceRepository;
    internal static TgSqlTableVersionRepository VersionRepository => TgStorageTestsUtils.DataCore.ContextManager.VersionRepository;

    #endregion
}