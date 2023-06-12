// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Helpers;

/// <summary>
/// SQL data storage cache helper.
/// </summary>
public sealed class TgSqlContextCacheHelper
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlContextCacheHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlContextCacheHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    private TgSqlContextManagerHelper ContextManager => TgSqlContextManagerHelper.Instance;
    public TgSqlTableName TableName { get; private set; } = TgSqlTableName.None;
    public TgSqlTableTopRecords TableTopRecords { get; private set; } = TgSqlTableTopRecords.Top1000;
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

    public void Load(TgSqlTableName tableName)
    {
		// Tables.
		if (!Apps.Any() || Equals(tableName, TgSqlTableName.All) || Equals(tableName, TgSqlTableName.Apps))
			Apps = ContextManager.ContextTableApps.GetList(TableTopRecords);
		if (!Documents.Any() || Equals(tableName, TgSqlTableName.All) || Equals(tableName, TgSqlTableName.Documents))
			Documents = ContextManager.ContextTableDocuments.GetList(TableTopRecords);
		if (!Filters.Any() || Equals(tableName, TgSqlTableName.All) || Equals(tableName, TgSqlTableName.Filters))
			Filters = ContextManager.ContextTableFilters.GetList(TableTopRecords);
		if (!Messages.Any() || Equals(tableName, TgSqlTableName.All) || Equals(tableName, TgSqlTableName.Messages))
			Messages = ContextManager.ContextTableMessages.GetList(TableTopRecords);
		if (!Proxies.Any() || Equals(tableName, TgSqlTableName.All) || Equals(tableName, TgSqlTableName.Proxies))
			Proxies = ContextManager.ContextTableProxies.GetList(TableTopRecords);
		if (!Sources.Any() || Equals(tableName, TgSqlTableName.All) || Equals(tableName, TgSqlTableName.Sources))
			Sources = ContextManager.ContextTableSources.GetList(TableTopRecords);
		if (!Versions.Any() || Equals(tableName, TgSqlTableName.All) || Equals(tableName, TgSqlTableName.Versions))
			Versions = ContextManager.ContextTableVersions.GetList(TableTopRecords);

		// Optimize.
		if (TableName.Equals(TgSqlTableName.All))
            TableName = TgSqlTableName.None;
    }

    #endregion
}