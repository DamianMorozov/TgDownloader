// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Helpers;

/// <summary>
/// SQL data storage cache helper.
/// </summary>
public sealed class TgSqlContextCacheHelper : ITgHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlContextCacheHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlContextCacheHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    private TgSqlContextManagerHelper ContextManager => TgSqlContextManagerHelper.Instance;
    public TgSqlEnumTableName TableName { get; private set; } = TgSqlEnumTableName.None;
    public TgSqlEnumTableTopRecords TableTopRecords { get; private set; } = TgSqlEnumTableTopRecords.Top1000;
    public List<TgSqlTableAppModel> Apps { get; private set; } = new();
    public List<TgSqlTableDocumentModel> Documents { get; private set; } = new();
    public List<TgSqlTableFilterModel> Filters { get; private set; } = new();
    public List<TgSqlTableMessageModel> Messages { get; private set; } = new();
    public List<TgSqlTableProxyModel> Proxies { get; private set; } = new();
    public List<TgSqlTableSourceModel> Sources { get; private set; } = new();
    public List<TgSqlTableVersionModel> Versions { get; private set; } = new();

	#endregion

	#region Public and private methods

	public void Load() => Load(TableName);

    public void Load(TgSqlEnumTableName tableName)
    {
		// Tables.
		if (!Apps.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Apps))
			Apps = ContextManager.ContextTableApps.GetList(TableTopRecords);
		if (!Documents.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Documents))
			Documents = ContextManager.ContextTableDocuments.GetList(TableTopRecords);
		if (!Filters.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Filters))
			Filters = ContextManager.ContextTableFilters.GetList(TableTopRecords);
		if (!Messages.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Messages))
			Messages = ContextManager.ContextTableMessages.GetList(TableTopRecords);
		if (!Proxies.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Proxies))
			Proxies = ContextManager.ContextTableProxies.GetList(TableTopRecords);
		if (!Sources.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Sources))
			Sources = ContextManager.ContextTableSources.GetList(TableTopRecords);
		if (!Versions.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Versions))
			Versions = ContextManager.ContextTableVersions.GetList(TableTopRecords);

		// Optimize.
		if (TableName.Equals(TgSqlEnumTableName.All))
            TableName = TgSqlEnumTableName.None;
    }

    #endregion
}