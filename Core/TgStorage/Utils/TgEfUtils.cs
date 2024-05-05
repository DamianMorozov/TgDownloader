// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using ValidationResult = FluentValidation.Results.ValidationResult;

namespace TgStorage.Utils;

public static class TgEfUtils
{
	#region Public and private fields, properties, constructor

	public static TgLogHelper TgLog => TgLogHelper.Instance;
	public static TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
	public static short LastVersion => 24;


	#endregion

	#region Public and private methods

	public static ValidationResult GetEfValid<T>(T item) where T : TgEfEntityBase, ITgDbEntity, new() =>
		item switch
		{
			TgEfAppEntity app => GetEfValid(app),
			TgEfDocumentEntity document => GetEfValid(document),
			TgEfFilterEntity filter => GetEfValid(filter),
			TgEfMessageEntity message => GetEfValid(message),
			TgEfSourceEntity source => GetEfValid(source),
			TgEfProxyEntity proxy => GetEfValid(proxy),
			TgEfVersionEntity version => GetEfValid(version),
			_ => new ValidationResult { Errors = [new ValidationFailure(nameof(item), "Type error!")] }
		};

	public static ValidationResult GetEfValid(TgEfAppEntity item) => new TgEfAppValidator().Validate(item);
	public static ValidationResult GetEfValid(TgEfDocumentEntity item) => new TgEfDocumentValidator().Validate(item);
	public static ValidationResult GetEfValid(TgEfFilterEntity item) => new TgEfFilterValidator().Validate(item);
	public static ValidationResult GetEfValid(TgEfMessageEntity item) => new TgEfMessageValidator().Validate(item);
	public static ValidationResult GetEfValid(TgEfSourceEntity item) => new TgEfSourceValidator().Validate(item);
	public static ValidationResult GetEfValid(TgEfProxyEntity item) => new TgEfProxyValidator().Validate(item);
	public static ValidationResult GetEfValid(TgEfVersionEntity item) => new TgEfVersionValidator().Validate(item);

	public static TgEfContext EfContext => CreateEfContext();

	public static TgEfContext CreateEfContext() => new();

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
		TgEfVersionEntity versionLast = GetLastVersion(efContext);
		if (versionLast.Version < 19)
		{
			UpgradeDb();
			UpdateDbTableUidUpperCase();
		}
		CheckDbTables(efContext);
		FillTableVersions(efContext);
		efContext.CompactDb();
	}

	/// <summary> Create and update storage </summary>
	public static async Task CreateAndUpdateDbAsync()
	{
		await using TgEfContext efContext = CreateEfContext();
		await efContext.CreateAndUpdateDbAsync();
		TgEfVersionEntity versionLast = GetLastVersion(efContext);
		if (versionLast.Version < 19)
		{
			await UpgradeDbAsync();
			await UpdateDbTableUidUpperCaseAsync();
		}
		await CheckDbTablesAsync(efContext);
		await FillTableVersionsAsync(efContext);
		await efContext.CompactDbAsync();
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

	public static TgEfVersionEntity GetLastVersion(TgEfContext efContext)
	{
		TgEfVersionRepository versionRepository = new(efContext);
		TgEfVersionEntity versionLast = new();
		if (EfContext.IsTableExists(TgEfConstants.TableVersions))
		{
			List<TgEfVersionEntity> versions = versionRepository.GetList(TgEnumTableTopRecords.All, isNoTracking: true).Items
				.Where(x => x.Version != new TgEfVersionEntity().Version).OrderBy(x => x.Version).ToList();
			if (versions.Any())
				versionLast = versions[^1];
		}
		return versionLast;
	}

	public static void FillTableVersions(TgEfContext efContext)
	{
		TgEfVersionRepository versionRepository = new(efContext);
		versionRepository.DeleteNew();
		bool isLast = false;
		while (!isLast)
		{
			TgEfVersionEntity versionLast = GetLastVersion(efContext);
			if (Equals(versionLast.Version, short.MaxValue))
				versionLast.Version = 0;
			switch (versionLast.Version)
			{
				case 0:
					versionRepository.Save(new() { Version = 1, Description = "Added versions table" });
					break;
				case 1:
					versionRepository.Save(new() { Version = 2, Description = "Added apps table" });
					break;
				case 2:
					versionRepository.Save(new() { Version = 3, Description = "Added documents table" });
					break;
				case 3:
					versionRepository.Save(new() { Version = 4, Description = "Added filters table" });
					break;
				case 4:
					versionRepository.Save(new() { Version = 5, Description = "Added messages table" });
					break;
				case 5:
					versionRepository.Save(new() { Version = 6, Description = "Added proxies table" });
					break;
				case 6:
					versionRepository.Save(new() { Version = 7, Description = "Added sources table" });
					break;
				case 7:
					versionRepository.Save(new() { Version = 8, Description = "Added source settings table" });
					break;
				case 8:
					versionRepository.Save(new() { Version = 9, Description = "Upgrade versions table" });
					break;
				case 9:
					versionRepository.Save(new() { Version = 10, Description = "Upgrade apps table" });
					break;
				case 10:
					versionRepository.Save(new() { Version = 11, Description = "Upgrade storage on XPO framework" });
					break;
				case 11:
					versionRepository.Save(new() { Version = 12, Description = "Upgrade apps table" });
					break;
				case 12:
					versionRepository.Save(new() { Version = 13, Description = "Upgrade documents table" });
					break;
				case 13:
					versionRepository.Save(new() { Version = 14, Description = "Upgrade filters table" });
					break;
				case 14:
					versionRepository.Save(new() { Version = 15, Description = "Upgrade messages table" });
					break;
				case 15:
					versionRepository.Save(new() { Version = 16, Description = "Upgrade proxies table" });
					break;
				case 16:
					versionRepository.Save(new() { Version = 17, Description = "Upgrade sources table" });
					break;
				case 17:
					versionRepository.Save(new() { Version = 18, Description = "Updating the UID field in the apps table" });
					break;
				case 18:
					versionRepository.Save(new() { Version = 19, Description = "Updating the UID field in the documents table" });
					break;
				case 19:
					versionRepository.Save(new() { Version = 20, Description = "Updating the UID field in the filters table" });
					break;
				case 20:
					versionRepository.Save(new() { Version = 21, Description = "Updating the UID field in the messages table" });
					break;
				case 21:
					versionRepository.Save(new() { Version = 22, Description = "Updating the UID field in the proxies table" });
					break;
				case 22:
					versionRepository.Save(new() { Version = 23, Description = "Updating the UID field in the sources table" });
					break;
				case 23:
					versionRepository.Save(new() { Version = 24, Description = "Updating the UID field in the versions table" });
					break;
			}
			if (versionLast.Version >= LastVersion)
				isLast = true;
		}
	}

	public static async Task FillTableVersionsAsync(TgEfContext efContext)
	{
		TgEfVersionRepository versionRepository = new(efContext);
		await versionRepository.DeleteNewAsync();
		bool isLast = false;
		while (!isLast)
		{
			TgEfVersionEntity versionLast = GetLastVersion(efContext);
			if (Equals(versionLast.Version, short.MaxValue))
				versionLast.Version = 0;
			switch (versionLast.Version)
			{
				case 0:
					await versionRepository.SaveAsync(new() { Version = 1, Description = "Added versions table" });
					break;
				case 1:
					await versionRepository.SaveAsync(new() { Version = 2, Description = "Added apps table" });
					break;
				case 2:
					await versionRepository.SaveAsync(new() { Version = 3, Description = "Added documents table" });
					break;
				case 3:
					await versionRepository.SaveAsync(new() { Version = 4, Description = "Added filters table" });
					break;
				case 4:
					await versionRepository.SaveAsync(new() { Version = 5, Description = "Added messages table" });
					break;
				case 5:
					await versionRepository.SaveAsync(new() { Version = 6, Description = "Added proxies table" });
					break;
				case 6:
					await versionRepository.SaveAsync(new() { Version = 7, Description = "Added sources table" });
					break;
				case 7:
					await versionRepository.SaveAsync(new() { Version = 8, Description = "Added source settings table" });
					break;
				case 8:
					await versionRepository.SaveAsync(new() { Version = 9, Description = "Upgrade versions table" });
					break;
				case 9:
					await versionRepository.SaveAsync(new() { Version = 10, Description = "Upgrade apps table" });
					break;
				case 10:
					await versionRepository.SaveAsync(new() { Version = 11, Description = "Upgrade storage on XPO framework" });
					break;
				case 11:
					await versionRepository.SaveAsync(new() { Version = 12, Description = "Upgrade apps table" });
					break;
				case 12:
					await versionRepository.SaveAsync(new() { Version = 13, Description = "Upgrade documents table" });
					break;
				case 13:
					await versionRepository.SaveAsync(new() { Version = 14, Description = "Upgrade filters table" });
					break;
				case 14:
					await versionRepository.SaveAsync(new() { Version = 15, Description = "Upgrade messages table" });
					break;
				case 15:
					await versionRepository.SaveAsync(new() { Version = 16, Description = "Upgrade proxies table" });
					break;
				case 16:
					await versionRepository.SaveAsync(new() { Version = 17, Description = "Upgrade sources table" });
					break;
				case 17:
					await versionRepository.SaveAsync(new() { Version = 18, Description = "Updating the UID field in the apps table" });
					break;
				case 18:
					await versionRepository.SaveAsync(new() { Version = 19, Description = "Updating the UID field in the documents table" });
					break;
				case 19:
					await versionRepository.SaveAsync(new() { Version = 20, Description = "Updating the UID field in the filters table" });
					break;
				case 20:
					await versionRepository.SaveAsync(new() { Version = 21, Description = "Updating the UID field in the messages table" });
					break;
				case 21:
					await versionRepository.SaveAsync(new() { Version = 22, Description = "Updating the UID field in the proxies table" });
					break;
				case 22:
					await versionRepository.SaveAsync(new() { Version = 23, Description = "Updating the UID field in the sources table" });
					break;
				case 23:
					await versionRepository.SaveAsync(new() { Version = 24, Description = "Updating the UID field in the versions table" });
					break;
			}
			if (versionLast.Version >= LastVersion)
				isLast = true;
		}
	}

	#endregion
}