// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using ValidationResult = FluentValidation.Results.ValidationResult;

namespace TgStorage.Utils;

public static class TgEfUtils
{
	#region Public and private fields, properties, constructor

	public static TgLogHelper TgLog => TgLogHelper.Instance;
	public static TgLocaleHelper TgLocale => TgLocaleHelper.Instance;

	#endregion

	#region Public and private methods

	public static ValidationResult GetEfValid<T>(T item) where T : TgEfEntityBase, ITgDbEntity, new() =>
		item switch
		{
			TgEfAppEntity app => new TgEfAppValidator().Validate(app),
			TgEfDocumentEntity document => new TgEfDocumentValidator().Validate(document),
			TgEfFilterEntity filter => new TgEfFilterValidator().Validate(filter),
			TgEfMessageEntity message => new TgEfMessageValidator().Validate(message),
			TgEfSourceEntity source => new TgEfSourceValidator().Validate(source),
			TgEfProxyEntity proxy => new TgEfProxyValidator().Validate(proxy),
			TgEfVersionEntity version => new TgEfVersionValidator().Validate(version),
			_ => new ValidationResult { Errors = [new ValidationFailure(nameof(item), "Type error!")] }
		};

	public static TgEfContext EfContext => CreateEfContext();

	public static TgEfContext CreateEfContext() => new();

	public static TgEfContext CreateEfContext(string fileStorage) => new(fileStorage);

	public static void VersionsView()
	{
		TgEfVersionRepository versionRepository = new(EfContext);
		TgEfOperResult<TgEfVersionEntity> operResult = versionRepository.GetList(TgEnumTableTopRecords.All, isNoTracking: true);
		if (operResult.IsExists)
		{
			foreach (TgEfVersionEntity version in operResult.Items)
			{
				TgLog.WriteLine($" {version.Version:00} | {version.Description}");
			}
		}
	}

	public static void FiltersView()
	{
		TgEfFilterRepository filterRepository = new(EfContext);
		TgEfOperResult<TgEfFilterEntity> operResult = filterRepository.GetList(TgEnumTableTopRecords.All, isNoTracking: true);
		if (operResult.IsExists)
		{
			foreach (TgEfFilterEntity filter in operResult.Items)
			{
				TgLog.WriteLine($"{filter}");
			}
		}
	}

	/// <summary> Create and update storage </summary>
	public static void CreateAndUpdateDb()
	{
		using TgEfContext efContext = CreateEfContext();
		efContext.CreateAndUpdateDb();
		TgEfVersionRepository versionRepository = new(efContext);
		//TgEfVersionEntity versionLast = versionRepository.GetLastVersion();
		//if (versionLast.Version < 19)
		//{
		//	UpgradeDb();
		//	UpdateDbTableUidUpperCase();
		//}
		CheckDbTables(efContext);
		versionRepository.FillTableVersions();
		efContext.CompactDb();
	}

	/// <summary> Create and update storage </summary>
	public static async Task CreateAndUpdateDbAsync()
	{
		await using TgEfContext efContext = CreateEfContext();
		await efContext.CreateAndUpdateDbAsync();
		TgEfVersionRepository versionRepository = new(efContext);
		//TgEfVersionEntity versionLast = versionRepository.GetLastVersion();
		//if (versionLast.Version < 19)
		//{
		//	await UpgradeDbAsync();
		//	await UpdateDbTableUidUpperCaseAsync();
		//}
		await CheckDbTablesAsync(efContext);
		await versionRepository.FillTableVersionsAsync();
		await efContext.CompactDbAsync();
	}

	// TODO
	/// <summary> Data transfer between storages </summary>
	public static void DataTransferBetweenStorages()
	{
		using TgEfContext efContextFrom = CreateEfContext(TgAppSettingsHelper.Instance.AppXml.XmlFileStorage);
		using TgEfContext efContextTo = CreateEfContext();

		TgEfOperResult<TgEfAppEntity> operResultApps = new TgEfAppRepository(efContextFrom).GetList(TgEnumTableTopRecords.All, isNoTracking: true);
		if (operResultApps.IsExists)
		{
			TgEfAppRepository appRepositoryTo = new(efContextTo);
			foreach (TgEfAppEntity app in operResultApps.Items)
			{
				appRepositoryTo.Save(app);
			}
		}
	}

	private static void UpdateDbTableUidUpperCase()
	{
		EfContext.UpdateTableUidUpperCaseAll<TgEfAppEntity>();
		EfContext.UpdateTableUidUpperCaseAll<TgEfDocumentEntity>();
		EfContext.UpdateTableUidUpperCaseAll<TgEfFilterEntity>();
		EfContext.UpdateTableUidUpperCaseAll<TgEfMessageEntity>();
		EfContext.UpdateTableUidUpperCaseAll<TgEfProxyEntity>();
		EfContext.UpdateTableUidUpperCaseAll<TgEfSourceEntity>();
		EfContext.UpdateTableUidUpperCaseAll<TgEfVersionEntity>();
	}

	private static async Task UpdateDbTableUidUpperCaseAsync()
	{
		await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfAppEntity>();
		await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfDocumentEntity>();
		await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfFilterEntity>();
		await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfMessageEntity>();
		await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfProxyEntity>();
		await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfSourceEntity>();
		await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfVersionEntity>();
	}

	private static void UpgradeDb()
	{
		TgEfOperResult<TgEfAppEntity> operResultApps = EfContext.AlterTableNoCaseUid<TgEfAppEntity>();
		if (operResultApps.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
		TgEfOperResult<TgEfDocumentEntity> operResultDocuments = EfContext.AlterTableNoCaseUid<TgEfDocumentEntity>();
		if (operResultDocuments.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
		TgEfOperResult<TgEfFilterEntity> operResultFilters = EfContext.AlterTableNoCaseUid<TgEfFilterEntity>();
		if (operResultFilters.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
		TgEfOperResult<TgEfMessageEntity> operResultMessages = EfContext.AlterTableNoCaseUid<TgEfMessageEntity>();
		if (operResultMessages.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
		TgEfOperResult<TgEfProxyEntity> operResultProxies = EfContext.AlterTableNoCaseUid<TgEfProxyEntity>();
		if (operResultProxies.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
		TgEfOperResult<TgEfSourceEntity> operResultSources = EfContext.AlterTableNoCaseUid<TgEfSourceEntity>();
		if (operResultSources.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
		TgEfOperResult<TgEfVersionEntity> operResultVersions = EfContext.AlterTableNoCaseUid<TgEfVersionEntity>();
		if (operResultVersions.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
	}

	private static async Task UpgradeDbAsync()
	{
		TgEfOperResult<TgEfAppEntity> operResultApps = await EfContext.AlterTableNoCaseUidAsync<TgEfAppEntity>();
		if (operResultApps.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
		TgEfOperResult<TgEfDocumentEntity> operResultDocuments = await EfContext.AlterTableNoCaseUidAsync<TgEfDocumentEntity>();
		if (operResultDocuments.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
		TgEfOperResult<TgEfFilterEntity> operResultFilters = await EfContext.AlterTableNoCaseUidAsync<TgEfFilterEntity>();
		if (operResultFilters.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
		TgEfOperResult<TgEfMessageEntity> operResultMessages = await EfContext.AlterTableNoCaseUidAsync<TgEfMessageEntity>();
		if (operResultMessages.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
		TgEfOperResult<TgEfProxyEntity> operResultProxies = await EfContext.AlterTableNoCaseUidAsync<TgEfProxyEntity>();
		if (operResultProxies.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
		TgEfOperResult<TgEfSourceEntity> operResultSources = await EfContext.AlterTableNoCaseUidAsync<TgEfSourceEntity>();
		if (operResultSources.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
		TgEfOperResult<TgEfVersionEntity> operResultVersions = await EfContext.AlterTableNoCaseUidAsync<TgEfVersionEntity>();
		if (operResultVersions.State == TgEnumEntityState.NotExecuted)
			throw new(TgLocale.TablesAppsException);
	}

	private static void CheckDbTables(TgEfContext efContext)
	{
		if (!CheckTableAppsCrud(efContext))
			throw new(TgLocale.TablesAppsException);
		if (!CheckTableDocumentsCrud(efContext))
			throw new(TgLocale.TablesDocumentsException);
		if (!CheckTableFiltersCrud(efContext))
			throw new(TgLocale.TablesFiltersException);
		if (!CheckTableMessagesCrud(efContext))
			throw new(TgLocale.TablesMessagesException);
		if (!CheckTableSourcesCrud(efContext))
			throw new(TgLocale.TablesSourcesException);
		if (!CheckTableProxiesCrud(efContext))
			throw new(TgLocale.TablesProxiesException);
		if (!CheckTableVersionsCrud(efContext))
			throw new(TgLocale.TablesVersionsException);
	}

	private static async Task CheckDbTablesAsync(TgEfContext efContext)
	{
		if (!await CheckTableAppsCrudAsync(efContext))
			throw new(TgLocale.TablesAppsException);
		if (!await CheckTableDocumentsCrudAsync(efContext))
			throw new(TgLocale.TablesDocumentsException);
		if (!await CheckTableFiltersCrudAsync(efContext))
			throw new(TgLocale.TablesFiltersException);
		if (!await CheckTableMessagesCrudAsync(efContext))
			throw new(TgLocale.TablesMessagesException);
		if (!await CheckTableSourcesCrudAsync(efContext))
			throw new(TgLocale.TablesSourcesException);
		if (!await CheckTableProxiesCrudAsync(efContext))
			throw new(TgLocale.TablesProxiesException);
		if (!await CheckTableVersionsCrudAsync(efContext))
			throw new(TgLocale.TablesVersionsException);
	}

	private static bool CheckTableCrud<T>(ITgEfRepository<T> repository) where T : TgEfEntityBase, ITgDbEntity, new()
	{
		var operResult = repository.CreateNew();
		if (!operResult.IsExists)
			return false;
		operResult = repository.GetNew(isNoTracking: false);
		if (!operResult.IsExists)
			return false;
		operResult = repository.Save(operResult.Item);
		if (operResult.State != TgEnumEntityState.IsSaved)
			return false;
		operResult = repository.Delete(operResult.Item, isSkipFind: false);
		return operResult.State == TgEnumEntityState.IsDeleted;
	}

	private static async Task<bool> CheckTableCrudAsync<T>(ITgEfRepository<T> repository) where T : TgEfEntityBase, ITgDbEntity, new()
	{
		var operResult = await repository.CreateNewAsync();
		if (!operResult.IsExists)
			return false;
		operResult = await repository.GetNewAsync(isNoTracking: false);
		if (!operResult.IsExists)
			return false;
		operResult = await repository.SaveAsync(operResult.Item);
		if (operResult.State != TgEnumEntityState.IsSaved)
			return false;
		operResult = await repository.DeleteAsync(operResult.Item, isSkipFind: false);
		return operResult.State == TgEnumEntityState.IsDeleted;
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