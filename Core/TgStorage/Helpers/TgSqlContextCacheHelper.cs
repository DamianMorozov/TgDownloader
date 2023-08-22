// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Helpers;

/// <summary>
/// SQL data storage cache helper.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
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
	public TgSqlEnumTableTopRecords TableTopRecords { get; } = TgSqlEnumTableTopRecords.Top1000;
	public IEnumerable<TgSqlTableAppModel> Apps { get; private set; } = Enumerable.Empty<TgSqlTableAppModel>();
	public IEnumerable<TgSqlTableDocumentModel> Documents { get; private set; } = Enumerable.Empty<TgSqlTableDocumentModel>();
	public IEnumerable<TgSqlTableFilterModel> Filters { get; private set; } = Enumerable.Empty<TgSqlTableFilterModel>();
	public IEnumerable<TgSqlTableMessageModel> Messages { get; private set; } = Enumerable.Empty<TgSqlTableMessageModel>();
	public IEnumerable<TgSqlTableProxyModel> Proxies { get; private set; } = Enumerable.Empty<TgSqlTableProxyModel>();
	public IEnumerable<TgSqlTableSourceModel> Sources { get; private set; } = Enumerable.Empty<TgSqlTableSourceModel>();
	public IEnumerable<TgSqlTableVersionModel> Versions { get; private set; } = Enumerable.Empty<TgSqlTableVersionModel>();

	#endregion

	#region Public and private methods

    public string ToDebugString() => $"{ContextManager}";

	public void Load() => Load(TableName);

	public void Load(TgSqlEnumTableName tableName)
	{
		// Tables.
		if (!Apps.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Apps))
			Apps = ContextManager.AppRepository.GetEnumerable(TableTopRecords);
		if (!Documents.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Documents))
			Documents = ContextManager.DocumentRepository.GetEnumerable(TableTopRecords);
		if (!Filters.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Filters))
			Filters = ContextManager.FilterRepository.GetEnumerable(TableTopRecords);
		if (!Messages.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Messages))
			Messages = ContextManager.MessageRepository.GetEnumerable(TableTopRecords);
		if (!Proxies.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Proxies))
			Proxies = ContextManager.ProxyRepository.GetEnumerable(TableTopRecords);
		if (!Sources.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Sources))
			Sources = ContextManager.SourceRepository.GetEnumerable(TableTopRecords);
		if (!Versions.Any() || Equals(tableName, TgSqlEnumTableName.All) || Equals(tableName, TgSqlEnumTableName.Versions))
			Versions = ContextManager.VersionRepository.GetEnumerable(TableTopRecords);

		// Optimize.
		if (TableName.Equals(TgSqlEnumTableName.All))
			TableName = TgSqlEnumTableName.None;
	}

	#endregion
}