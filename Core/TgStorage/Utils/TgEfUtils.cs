// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Domain.Apps;
using TgStorage.Domain.Proxies;
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

	public static TgEfContext CreateEfContext()
	{
		return new TgEfContext();
	}

	public static void VersionsView()
	{
		TgEfVersionRepository versionRepository = new(EfContext);
		TgEfOperResult<TgEfVersionEntity> operResult = versionRepository.GetEnumerable(TgEnumTableTopRecords.All, isNoTracking: true);
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
		TgEfOperResult<TgEfFilterEntity> operResult = filterRepository.GetEnumerable(TgEnumTableTopRecords.All, isNoTracking: true);
		if (operResult.IsExists)
		{
			foreach (TgEfFilterEntity filter in operResult.Items)
			{
				TgLog.WriteLine($"{filter}");
			}
		}
	}

	/// <summary> Update structures of tables </summary>
	private static async Task CheckTablesCrudAsync()
	{
		TgEfVersionEntity versionLast = GetLastVersion();
		if (versionLast.Version < 19)
		{
			// Upgrade DB.
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
			// Update UID.
			await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfAppEntity>();
			await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfDocumentEntity>();
			await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfFilterEntity>();
			await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfMessageEntity>();
			await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfProxyEntity>();
			await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfSourceEntity>();
			await EfContext.UpdateTableUidUpperCaseAllAsync<TgEfVersionEntity>();
		}

		if (!await CheckTableAppsCrudAsync())
			throw new(TgLocale.TablesAppsException);
		if (!await CheckTableDocumentsCrudAsync())
			throw new(TgLocale.TablesDocumentsException);
		if (!await CheckTableFiltersCrudAsync())
			throw new(TgLocale.TablesFiltersException);
		if (!await CheckTableMessagesCrudAsync())
			throw new(TgLocale.TablesMessagesException);
		if (!await CheckTableSourcesCrudAsync())
			throw new(TgLocale.TablesSourcesException);
		if (!await CheckTableProxiesCrudAsync())
			throw new(TgLocale.TablesProxiesException);
		if (!await CheckTableVersionsCrudAsync())
			throw new(TgLocale.TablesVersionsException);

		await FillTableVersionsAsync();
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

	public static Task<bool> CheckTableAppsCrudAsync() => CheckTableCrudAsync(new TgEfAppRepository(EfContext));

	public static Task<bool> CheckTableDocumentsCrudAsync() => CheckTableCrudAsync(new TgEfDocumentRepository(EfContext));

	public static Task<bool> CheckTableFiltersCrudAsync() => CheckTableCrudAsync(new TgEfFilterRepository(EfContext));

	public static Task<bool> CheckTableMessagesCrudAsync() => CheckTableCrudAsync(new TgEfMessageRepository(EfContext));

	public static Task<bool> CheckTableProxiesCrudAsync() => CheckTableCrudAsync(new TgEfProxyRepository(EfContext));

	public static Task<bool> CheckTableSourcesCrudAsync() => CheckTableCrudAsync(new TgEfSourceRepository(EfContext));

	public static Task<bool> CheckTableVersionsCrudAsync() => CheckTableCrudAsync(new TgEfVersionRepository(EfContext));

	public static TgEfVersionEntity GetLastVersion()
	{
		TgEfVersionRepository versionRepository = new(EfContext);
		TgEfVersionEntity versionLast = !EfContext.IsTableExists(TgEfConstants.TableVersions)
			? new() : versionRepository.GetEnumerable(TgEnumTableTopRecords.All, isNoTracking: true).Items
				.Where(x => x.Description != "New version")
				.OrderBy(x => x.Version)
				.Last();
		return versionLast;
	}

	public static async Task FillTableVersionsAsync()
	{
		TgEfVersionRepository versionRepository = new(EfContext);
		await versionRepository.DeleteNewAsync();
		bool isLast = false;
		while (!isLast)
		{
			TgEfVersionEntity versionLast = GetLastVersion();
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