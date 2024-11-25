// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Domain;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace TgStorage.Utils;

public static class TgEfUtils
{
	#region Public and private fields, properties, constructor

	public static TgLogHelper TgLog => TgLogHelper.Instance;
	public static TgLocaleHelper TgLocale => TgLocaleHelper.Instance;

	#endregion

	#region Public and private methods

	public static IEnumerable<ITgDbEntity> GetTableModels()
	{
		yield return new TgEfAppEntity();
		yield return new TgEfDocumentEntity();
		yield return new TgEfFilterEntity();
		yield return new TgEfProxyEntity();
		yield return new TgEfSourceEntity();
		yield return new TgEfVersionEntity();
	}

	public static IEnumerable<Type> GetTableTypes()
	{
		yield return typeof(TgEfAppEntity);
		yield return typeof(TgEfDocumentEntity);
		yield return typeof(TgEfFilterEntity);
		yield return typeof(TgEfProxyEntity);
		yield return typeof(TgEfSourceEntity);
		yield return typeof(TgEfVersionEntity);
	}

	public static string GetPercentCountString(TgEfSourceEntity source)
	{
		float percent = source.Count <= source.FirstId ? 100 : source.FirstId > 1 ? (float)source.FirstId * 100 / source.Count : 0;
		if (IsPercentCountAll(source))
			return "100.00 %";
		return percent > 9 ? $" {percent:00.00} %" : $"  {percent:0.00} %";
	}

	public static bool IsPercentCountAll(TgEfSourceEntity source) => source.Count <= source.FirstId;

	public static ValidationResult GetEfValid<TEntity>(TEntity item) where TEntity : ITgDbFillEntity<TEntity>, new() =>
		item switch
		{
			TgEfAppEntity app => new TgEfAppValidator().Validate(app),
			TgEfDocumentEntity document => new TgEfDocumentValidator().Validate(document),
			TgEfFilterEntity filter => new TgEfFilterValidator().Validate(filter),
			TgEfMessageEntity message => new TgEfMessageValidator().Validate(message),
			TgEfSourceEntity source => new TgEfSourceValidator().Validate(source),
			TgEfProxyEntity proxy => new TgEfProxyValidator().Validate(proxy),
			TgEfVersionEntity version => new TgEfVersionValidator().Validate(version),
			_ => new() { Errors = [new ValidationFailure(nameof(item), "Type error!")] }
		};

	public static void Normilize<TEntity>(TEntity item) where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		switch (item)
		{
			case TgEfAppEntity app:
				if (app.ProxyUid == Guid.Empty)
					app.ProxyUid = null;
				break;
			case TgEfDocumentEntity document:
				if (document.SourceId == 0)
					document.SourceId = null;
				break;
			case TgEfFilterEntity filter:
				break;
			case TgEfMessageEntity message:
				if (message.SourceId == 0)
					message.SourceId = null;
				break;
			case TgEfSourceEntity source:
				break;
			case TgEfProxyEntity proxy:
				break;
			case TgEfVersionEntity version:
				break;
		}
		if (item.Uid == Guid.Empty)
			item.Uid = Guid.NewGuid();
	}

	public static TgEfContext EfContext => CreateEfContext();

	public static TgEfContext CreateEfContext() => new();

	//public static TgEfContext CreateEfContext(string efStorage) => new(efStorage);

	public static void VersionsView()
	{
		TgEfVersionRepository versionRepository = new(EfContext);
		TgEfStorageResult<TgEfVersionEntity> storageResult = versionRepository.GetList(TgEnumTableTopRecords.All, 0, isNoTracking: true);
		if (storageResult.IsExists)
		{
			foreach (TgEfVersionEntity version in storageResult.Items)
			{
				TgLog.WriteLine($" {version.Version:00} | {version.Description}");
			}
		}
	}

	public static void FiltersView()
	{
		TgEfFilterRepository filterRepository = new(EfContext);
		TgEfStorageResult<TgEfFilterEntity> storageResult = filterRepository.GetList(TgEnumTableTopRecords.All, 0, isNoTracking: true);
		if (storageResult.IsExists)
		{
			foreach (TgEfFilterEntity filter in storageResult.Items)
			{
				TgLog.WriteLine($"{filter}");
			}
		}
	}

	/// <summary> Create and update storage </summary>
	public static async Task CreateAndUpdateDbAsync()
	{
		await using TgEfContext efContext = CreateEfContext();
		await efContext.CreateAndUpdateDbAsync();
		TgEfVersionRepository versionRepository = new(efContext);
		await versionRepository.FillTableVersionsAsync();
		await efContext.CompactDbAsync();
	}

	///// <summary> Data transfer between storages </summary>
	//public static async Task DataTransferBetweenStoragesAsync(TgEfContext efContextFrom, TgEfContext efContextTo, Action<string> logWrite)
	//{
	//	// Transferring apps
	//	await DataTransferCoreAsync(new TgEfAppRepository(efContextFrom), new TgEfAppRepository(efContextTo), logWrite, TgEfConstants.TableApps);
	//	// Transferring filters
	//	await DataTransferCoreAsync(new TgEfFilterRepository(efContextFrom), new TgEfFilterRepository(efContextTo), logWrite, TgEfConstants.TableFilters);
	//	// Transferring proxies
	//	await DataTransferCoreAsync(new TgEfProxyRepository(efContextFrom), new TgEfProxyRepository(efContextTo), logWrite, TgEfConstants.TableProxies);
	//	// Transferring sources
	//	await DataTransferCoreAsync(new TgEfSourceRepository(efContextFrom), new TgEfSourceRepository(efContextTo), logWrite, TgEfConstants.TableSources);
	//}

	private static async Task DataTransferCoreAsync<TEntity>(ITgEfRepository<TEntity> repoFrom, ITgEfRepository<TEntity> repoTo, 
		Action<string> logWrite, string tableName) where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		logWrite($"Transferring table {tableName}: ...");
		int batchSizeFrom = 100;
		//const int batchSizeTo = 100;
		int countFrom = await repoFrom.GetCountAsync(WhereUidNotEmpty<TEntity>());
		//int countTo = await repoTo.GetCountAsync(WhereUidNotEmpty<TEntity>());

		for (int i = 0; i < countFrom; i += batchSizeFrom)
		{
			try
			{
				TgEfStorageResult<TEntity> storageResultFrom = await repoFrom.GetListAsync(
					batchSizeFrom, i, WhereUidNotEmpty<TEntity>(), isNoTracking: false);
				if (storageResultFrom.IsExists)
				{
					//List<TEntity> itemsTo = storageResultTo.Items.ToList();
					//List<TEntity> itemsFrom = storageResultFrom.Items.Where(itemFrom => itemsTo.All(itemTo => itemTo.Uid != itemFrom.Uid)).ToList();
					//List<TEntity> itemsFrom = storageResultFrom.Items.ToList();
					int countErrors = 0;
					//await using var transaction = await repoTo.BeginTransactionAsync();
					var tasks = new List<Task>();
					if (storageResultFrom.Items.Any())
					{
						foreach (TEntity itemFrom in storageResultFrom.Items)
						{
							tasks.Add(Task.Run(async () =>
							{
								try
								{
									await repoTo.SaveAsync(itemFrom);
								}
								catch (Exception ex)
								{
									Interlocked.Increment(ref countErrors);
#if DEBUG
									logWrite(ex.Message);
									if (ex.InnerException is not null)
										logWrite(ex.InnerException.Message);
#endif
								}
							}));
						}
						// Ожидаем завершения всех задач
						await Task.WhenAll(tasks);
					}
					var countSuccess = storageResultFrom.Items.Count() - countErrors;
					if (countSuccess > 0)
						logWrite($"Transferring table {tableName}: copied {countSuccess} records");
					if (countErrors > 0)
						logWrite($"Transferring table {tableName}: errors in {countErrors} records");
				}
			}
			catch (Exception ex)
			{
#if DEBUG
				logWrite(ex.Message);
#endif
				throw;
			}
		}
		logWrite($"Transferring table {tableName}: completed");
	}

	public static Expression<Func<TEntity, bool>> WhereUidNotEmpty<TEntity>() where TEntity : ITgDbFillEntity<TEntity>, new() => x => x.Uid != Guid.Empty;
	
	public static Expression<Func<TEntity, List<TEntity>, bool>> WhereUidNotEquals<TEntity>() where TEntity : ITgDbFillEntity<TEntity>, new() =>
		(itemFrom, itemsTo) => itemsTo.All(itemTo => itemTo.Uid.ToString().ToUpper() != itemFrom.Uid.ToString().ToUpper());

	//private static void UpdateDbTableUidUpperCase()
	//{
	//	EfContext.UpdateTableUidUpperCaseAll<TgEfAppEntity>();
	//	EfContext.UpdateTableUidUpperCaseAll<TgEfDocumentEntity>();
	//	EfContext.UpdateTableUidUpperCaseAll<TgEfFilterEntity>();
	//	EfContext.UpdateTableUidUpperCaseAll<TgEfMessageEntity>();
	//	EfContext.UpdateTableUidUpperCaseAll<TgEfProxyEntity>();
	//	EfContext.UpdateTableUidUpperCaseAll<TgEfSourceEntity>();
	//	EfContext.UpdateTableUidUpperCaseAll<TgEfVersionEntity>();
	//}

	//private static async Task UpdateDbTableUidUpperCaseAsync()
	//{
	//	await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfAppEntity>();
	//	await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfDocumentEntity>();
	//	await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfFilterEntity>();
	//	await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfMessageEntity>();
	//	await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfProxyEntity>();
	//	await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfSourceEntity>();
	//	await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfVersionEntity>();
	//}

	//private static void UpgradeDb()
	//{
	//	TgEfStorageResult<TgEfAppEntity> storageResultApps = EfContext.AlterTableNoCaseUid<TgEfAppEntity>();
	//	if (storageResultApps.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//	TgEfStorageResult<TgEfDocumentEntity> storageResultDocuments = EfContext.AlterTableNoCaseUid<TgEfDocumentEntity>();
	//	if (storageResultDocuments.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//	TgEfStorageResult<TgEfFilterEntity> storageResultFilters = EfContext.AlterTableNoCaseUid<TgEfFilterEntity>();
	//	if (storageResultFilters.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//	TgEfStorageResult<TgEfMessageEntity> storageResultMessages = EfContext.AlterTableNoCaseUid<TgEfMessageEntity>();
	//	if (storageResultMessages.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//	TgEfStorageResult<TgEfProxyEntity> storageResultProxies = EfContext.AlterTableNoCaseUid<TgEfProxyEntity>();
	//	if (storageResultProxies.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//	TgEfStorageResult<TgEfSourceEntity> storageResultSources = EfContext.AlterTableNoCaseUid<TgEfSourceEntity>();
	//	if (storageResultSources.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//	TgEfStorageResult<TgEfVersionEntity> storageResultVersions = EfContext.AlterTableNoCaseUid<TgEfVersionEntity>();
	//	if (storageResultVersions.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//}

	//private static async Task UpgradeDbAsync()
	//{
	//	TgEfStorageResult<TgEfAppEntity> storageResultApps = await EfContext.AlterTableNoCaseUidAsync<TgEfAppEntity>();
	//	if (storageResultApps.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//	TgEfStorageResult<TgEfDocumentEntity> storageResultDocuments = await EfContext.AlterTableNoCaseUidAsync<TgEfDocumentEntity>();
	//	if (storageResultDocuments.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//	TgEfStorageResult<TgEfFilterEntity> storageResultFilters = await EfContext.AlterTableNoCaseUidAsync<TgEfFilterEntity>();
	//	if (storageResultFilters.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//	TgEfStorageResult<TgEfMessageEntity> storageResultMessages = await EfContext.AlterTableNoCaseUidAsync<TgEfMessageEntity>();
	//	if (storageResultMessages.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//	TgEfStorageResult<TgEfProxyEntity> storageResultProxies = await EfContext.AlterTableNoCaseUidAsync<TgEfProxyEntity>();
	//	if (storageResultProxies.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//	TgEfStorageResult<TgEfSourceEntity> storageResultSources = await EfContext.AlterTableNoCaseUidAsync<TgEfSourceEntity>();
	//	if (storageResultSources.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//	TgEfStorageResult<TgEfVersionEntity> storageResultVersions = await EfContext.AlterTableNoCaseUidAsync<TgEfVersionEntity>();
	//	if (storageResultVersions.State == TgEnumEntityState.NotExecuted)
	//		throw new(TgLocale.TablesAppsException);
	//}

	//private static void CheckDbTables(TgEfContext efContext)
	//{
	//	if (!CheckTableAppsCrud(efContext))
	//		throw new(TgLocale.TablesAppsException);
	//	if (!CheckTableDocumentsCrud(efContext))
	//		throw new(TgLocale.TablesDocumentsException);
	//	if (!CheckTableFiltersCrud(efContext))
	//		throw new(TgLocale.TablesFiltersException);
	//	if (!CheckTableMessagesCrud(efContext))
	//		throw new(TgLocale.TablesMessagesException);

	//	TgEfStorageResult<TgEfSourceEntity> storageResultSource = new TgEfSourceRepository(efContext).GetNew(isNoTracking: false);
	//	if (storageResultSource.IsExists)
	//	{
	//		// Delete wrong messages
	//		TgEfMessageRepository messageRepository = new(efContext);
	//		TgEfStorageResult<TgEfMessageEntity> storageResultMessages = messageRepository.GetList(TgEnumTableTopRecords.All, 0, 
	//			item => item.SourceId == storageResultSource.Item.Id, isNoTracking: false);
	//		if (storageResultMessages.IsExists)
	//		{
	//			foreach (TgEfMessageEntity message in storageResultMessages.Items)
	//			{
	//				messageRepository.Delete(message, isSkipFind: true);
	//			}
	//		}
	//		// Delete wrong documents
	//		TgEfDocumentRepository documentRepository = new(efContext);
	//		TgEfStorageResult<TgEfDocumentEntity> storageResultDocuments = documentRepository.GetList(TgEnumTableTopRecords.All, 0,
	//			item => item.SourceId == storageResultSource.Item.Id, isNoTracking: false);
	//		if (storageResultDocuments.IsExists)
	//		{
	//			foreach (TgEfDocumentEntity document in storageResultDocuments.Items)
	//			{
	//				documentRepository.Delete(document, isSkipFind: true);
	//			}
	//		}
	//	}

	//	if (!CheckTableSourcesCrud(efContext))
	//		throw new(TgLocale.TablesSourcesException);
	//	if (!CheckTableProxiesCrud(efContext))
	//		throw new(TgLocale.TablesProxiesException);
	//	if (!CheckTableVersionsCrud(efContext))
	//		throw new(TgLocale.TablesVersionsException);
	//}

	//private static async Task CheckDbTablesAsync(TgEfContext efContext)
	//{
	//	if (!await CheckTableAppsCrudAsync(efContext))
	//		throw new(TgLocale.TablesAppsException);
	//	if (!await CheckTableDocumentsCrudAsync(efContext))
	//		throw new(TgLocale.TablesDocumentsException);
	//	if (!await CheckTableFiltersCrudAsync(efContext))
	//		throw new(TgLocale.TablesFiltersException);
	//	if (!await CheckTableMessagesCrudAsync(efContext))
	//		throw new(TgLocale.TablesMessagesException);

	//	TgEfStorageResult<TgEfSourceEntity> storageResultSource = await (new TgEfSourceRepository(efContext)).GetNewAsync(isNoTracking: false);
	//	if (storageResultSource.IsExists)
	//	{
	//		// Delete wrong messages
	//		TgEfMessageRepository messageRepository = new(efContext);
	//		TgEfStorageResult<TgEfMessageEntity> storageResultMessages = await messageRepository.GetListAsync(TgEnumTableTopRecords.All, 0,
	//			item => item.SourceId == storageResultSource.Item.Id, isNoTracking: false);
	//		if (storageResultMessages.IsExists)
	//		{
	//			foreach (TgEfMessageEntity message in storageResultMessages.Items)
	//			{
	//				await messageRepository.DeleteAsync(message, isSkipFind: true);
	//			}
	//		}
	//		// Delete wrong documents
	//		TgEfDocumentRepository documentRepository = new(efContext);
	//		TgEfStorageResult<TgEfDocumentEntity> storageResultDocuments = await documentRepository.GetListAsync(TgEnumTableTopRecords.All, 0,
	//			item => item.SourceId == storageResultSource.Item.Id, isNoTracking: false);
	//		if (storageResultDocuments.IsExists)
	//		{
	//			foreach (TgEfDocumentEntity document in storageResultDocuments.Items)
	//			{
	//				await documentRepository.DeleteAsync(document, isSkipFind: true);
	//			}
	//		}
	//	}

	//	if (!await CheckTableSourcesCrudAsync(efContext))
	//		throw new(TgLocale.TablesSourcesException);
	//	if (!await CheckTableProxiesCrudAsync(efContext))
	//		throw new(TgLocale.TablesProxiesException);
	//	if (!await CheckTableVersionsCrudAsync(efContext))
	//		throw new(TgLocale.TablesVersionsException);
	//}

	private static bool CheckTableCrud<TEntity>(ITgEfRepository<TEntity> repository) where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		var storageResult = repository.CreateNew();
		if (!storageResult.IsExists)
			return false;
		storageResult = repository.GetNew(isNoTracking: false);
		if (!storageResult.IsExists)
			return false;
		storageResult = repository.Save(storageResult.Item);
		if (storageResult.State != TgEnumEntityState.IsSaved)
			return false;
		storageResult = repository.Delete(storageResult.Item, isSkipFind: false);
		return storageResult.State == TgEnumEntityState.IsDeleted;
	}

	private static async Task<bool> CheckTableCrudAsync<TEntity>(ITgEfRepository<TEntity> repository) where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		var storageResult = await repository.CreateNewAsync();
		if (!storageResult.IsExists)
			return false;
		storageResult = await repository.GetNewAsync(isNoTracking: false);
		if (!storageResult.IsExists)
			return false;
		storageResult = await repository.SaveAsync(storageResult.Item);
		if (storageResult.State != TgEnumEntityState.IsSaved)
			return false;
		storageResult = await repository.DeleteAsync(storageResult.Item, isSkipFind: false);
		return storageResult.State == TgEnumEntityState.IsDeleted;
	}

	public static bool CheckTableAppsCrud(TgEfContext efContext) => CheckTableCrud(new TgEfAppRepository(efContext));

	public static Task<bool> CheckTableAppsCrudAsync(TgEfContext efContext) => CheckTableCrudAsync(new TgEfAppRepository(efContext));

	public static bool CheckTableDocumentsCrud(TgEfContext efContext) => CheckTableCrud(new TgEfDocumentRepository(efContext));

	public static Task<bool> CheckTableDocumentsCrudAsync(TgEfContext efContext) => CheckTableCrudAsync(new TgEfDocumentRepository(efContext));

	public static bool CheckTableFiltersCrud(TgEfContext efContext) => CheckTableCrud(new TgEfFilterRepository(efContext));

	public static Task<bool> CheckTableFiltersCrudAsync(TgEfContext efContext) => CheckTableCrudAsync(new TgEfFilterRepository(efContext));

	public static bool CheckTableMessagesCrud(TgEfContext efContext) => CheckTableCrud(new TgEfMessageRepository(efContext));

	public static Task<bool> CheckTableMessagesCrudAsync(TgEfContext efContext) => CheckTableCrudAsync(new TgEfMessageRepository(efContext));

	public static bool CheckTableProxiesCrud(TgEfContext efContext) => CheckTableCrud(new TgEfProxyRepository(efContext));

	public static Task<bool> CheckTableProxiesCrudAsync(TgEfContext efContext) => CheckTableCrudAsync(new TgEfProxyRepository(efContext));

	public static bool CheckTableSourcesCrud(TgEfContext efContext) => CheckTableCrud(new TgEfSourceRepository(efContext));

	public static Task<bool> CheckTableSourcesCrudAsync(TgEfContext efContext) => CheckTableCrudAsync(new TgEfSourceRepository(efContext));

	public static bool CheckTableVersionsCrud(TgEfContext efContext) => CheckTableCrud(new TgEfVersionRepository(efContext));

	public static Task<bool> CheckTableVersionsCrudAsync(TgEfContext efContext) => CheckTableCrudAsync(new TgEfVersionRepository(efContext));

	#endregion
}