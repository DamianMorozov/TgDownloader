//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//namespace TgStorage;

///// <summary>
///// SQL data storage cache helper.
///// </summary>
//[DebuggerDisplay("{ToDebugString()}")]
//public sealed class TgXpoContextCache
//{
//    #region Public and private fields, properties, constructor

//    private TgXpoContext ContextManager { get; } = new(TgEnumStorageType.Prod);
//	public TgEnumTableName TableName { get; private set; } = TgEnumTableName.None;
//    public TgEnumTableTopRecords TableTopRecords { get; private set; } = TgEnumTableTopRecords.Top1000;
//    public IEnumerable<TgXpoAppEntity> Apps { get; private set; } = Enumerable.Empty<TgXpoAppEntity>();
//    public IEnumerable<TgXpoDocumentEntity> Documents { get; private set; } = Enumerable.Empty<TgXpoDocumentEntity>();
//    public IEnumerable<TgXpoFilterEntity> Filters { get; private set; } = Enumerable.Empty<TgXpoFilterEntity>();
//    public IEnumerable<TgXpoMessageEntity> Messages { get; private set; } = Enumerable.Empty<TgXpoMessageEntity>();
//    public IEnumerable<TgXpoProxyEntity> Proxies { get; private set; } = Enumerable.Empty<TgXpoProxyEntity>();
//    public IEnumerable<TgXpoSourceEntity> Sources { get; private set; } = Enumerable.Empty<TgXpoSourceEntity>();
//    public IEnumerable<TgXpoVersionEntity> Versions { get; private set; } = Enumerable.Empty<TgXpoVersionEntity>();

//    #endregion

//    #region Public and private methods

//    public string ToDebugString() => $"{ContextManager}";

//    public void Load() => Load(TableName);

//    public void Load(TgEnumTableName tableName)
//    {
//        // Tables.
//        if (!Apps.Any() || Equals(tableName, TgEnumTableName.All) || Equals(tableName, TgEnumTableName.Apps))
//        {
//            TgXpoOperResult<TgXpoAppEntity> operResult = ContextManager.AppRepository.GetEnumerableAsync(TableTopRecords).GetAwaiter().GetResult();
//            Apps = operResult.IsExist
//                ? operResult.Items
//                : new List<TgXpoAppEntity>();
//        }

//        if (!Documents.Any() || Equals(tableName, TgEnumTableName.All) ||
//            Equals(tableName, TgEnumTableName.Documents))
//        {
//            TgXpoOperResult<TgXpoDocumentEntity> operResult = ContextManager.DocumentRepository.GetEnumerableAsync(TableTopRecords).GetAwaiter().GetResult();
//            Documents = operResult.State == TgEnumEntityState.IsExist
//                ? operResult.Items
//                : new List<TgXpoDocumentEntity>();
//        }

//        if (!Filters.Any() || Equals(tableName, TgEnumTableName.All) || Equals(tableName, TgEnumTableName.Filters))
//        {
//            TgXpoOperResult<TgXpoFilterEntity> operResult = ContextManager.FilterRepository.GetEnumerableAsync(TableTopRecords).GetAwaiter().GetResult();
//            Filters = operResult.IsExist
//                ? operResult.Items
//                : new List<TgXpoFilterEntity>();
//        }

//        if (!Messages.Any() || Equals(tableName, TgEnumTableName.All) || Equals(tableName, TgEnumTableName.Messages))
//        {
//            TgXpoOperResult<TgXpoMessageEntity> operResult = ContextManager.MessageRepository.GetEnumerableAsync(TableTopRecords).GetAwaiter().GetResult();
//            Messages = operResult.IsExist
//                ? operResult.Items
//                : new List<TgXpoMessageEntity>();
//        }

//        if (!Proxies.Any() || Equals(tableName, TgEnumTableName.All) || Equals(tableName, TgEnumTableName.Proxies))
//        {
//            TgXpoOperResult<TgXpoProxyEntity> operResult = ContextManager.ProxyRepository.GetEnumerableAsync(TableTopRecords).GetAwaiter().GetResult();
//            Proxies = operResult.IsExist
//                ? operResult.Items
//                : new List<TgXpoProxyEntity>();
//        }

//        if (!Sources.Any() || Equals(tableName, TgEnumTableName.All) || Equals(tableName, TgEnumTableName.Sources))
//        {
//            TgXpoOperResult<TgXpoSourceEntity> operResult = ContextManager.SourceRepository.GetEnumerableAsync(TableTopRecords).GetAwaiter().GetResult();
//            Sources = operResult.IsExist
//                ? operResult.Items
//                : new List<TgXpoSourceEntity>();
//        }

//        if (!Versions.Any() || Equals(tableName, TgEnumTableName.All) || Equals(tableName, TgEnumTableName.Versions))
//        {
//            TgXpoOperResult<TgXpoVersionEntity> operResult = ContextManager.VersionRepository.GetEnumerableAsync(TableTopRecords).GetAwaiter().GetResult();
//            Versions = operResult.IsExist
//                ? operResult.Items
//                : new List<TgXpoVersionEntity>();
//        }

//        // Optimize.
//        if (TableName.Equals(TgEnumTableName.All))
//            TableName = TgEnumTableName.None;
//    }

//    #endregion
//}