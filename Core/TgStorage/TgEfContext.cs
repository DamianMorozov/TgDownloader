// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Microsoft.EntityFrameworkCore;

namespace TgStorage;

public sealed class TgEfContext : DbContext, IDisposable
{
	#region Public and private fields, properties, constructor

	/// <summary> App queries </summary>
	public DbSet<TgEfAppEntity> Apps { get; set; } = default!;
	/// <summary> Document queries </summary>
	public DbSet<TgEfDocumentEntity> Documents { get; set; } = default!;
	/// <summary> Filter queries </summary>
	public DbSet<TgEfFilterEntity> Filters { get; set; } = default!;
	/// <summary> Message queries </summary>
	public DbSet<TgEfMessageEntity> Messages { get; set; } = default!;
	/// <summary> Proxy queries </summary>
	public DbSet<TgEfProxyEntity> Proxies { get; set; } = default!;
	/// <summary> Source queries </summary>
	public DbSet<TgEfSourceEntity> Sources { get; set; } = default!;
	/// <summary> Version queries </summary>
	public DbSet<TgEfVersionEntity> Versions { get; set; } = default!;

	/// <summary> App repository </summary>
	public ITgEfRepository<TgEfAppEntity> AppRepository { get; set; } = default!;
	/// <summary> Document repository </summary>
	public ITgEfRepository<TgEfDocumentEntity> DocumentRepository { get; set; } = default!;
	/// <summary> Filter repository </summary>
	public ITgEfRepository<TgEfFilterEntity> FilterRepository { get; set; } = default!;
	/// <summary> Message repository </summary>
	public ITgEfRepository<TgEfMessageEntity> MessageRepository { get; set; } = default!;
	/// <summary> Proxy repository </summary>
	public ITgEfRepository<TgEfProxyEntity> ProxyRepository { get; set; } = default!;
	/// <summary> Source repository </summary>
	public ITgEfRepository<TgEfSourceEntity> SourceRepository { get; set; } = default!;
	/// <summary> Version repository </summary>
	public ITgEfRepository<TgEfVersionEntity> VersionRepository { get; set; } = default!;

	public TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
	public TgLogHelper TgLog => TgLogHelper.Instance;
	public TgLocaleHelper TgLocale => TgLocaleHelper.Instance;

	public bool IsReady =>
		TgAppSettings.AppXml.IsExistsFileStorage &&
		IsTableExists(TgStorageConstants.TableApps) && IsTableExists(TgStorageConstants.TableDocuments) &&
		IsTableExists(TgStorageConstants.TableFilters) && IsTableExists(TgStorageConstants.TableMessages) &&
		IsTableExists(TgStorageConstants.TableProxies) && IsTableExists(TgStorageConstants.TableSources) &&
		IsTableExists(TgStorageConstants.TableVersions);

	public bool IsNotReady => !IsReady;

	// Inject options.
	// options: The DbContextOptions{TgEfContext} for the context.
	public TgEfContext(DbContextOptions<TgEfContext> options) : base(options)
	{
		InitRepositories();
	}

	public TgEfContext() : base()
	{
		InitRepositories();
	}

	#endregion

	#region Public and private methods - IDisposable

	/// <summary> Locker object </summary>
	private readonly object _locker = new();
	/// <summary> To detect redundant calls </summary>
	private bool _disposed;

	/// <summary> Finalizer </summary>
	~TgEfContext() => Dispose(false);

	/// <summary> Throw exception if disposed </summary>
	private void CheckIfDisposed()
	{
		if (_disposed)
			throw new ObjectDisposedException($"{nameof(TgEfContext)}: object has been disposed off!");
	}

	/// <summary> Release managed resources </summary>
	private void ReleaseManagedResources()
	{
		//
	}

	/// <summary> Release unmanaged resources </summary>
	private void ReleaseUnmanagedResources()
	{
		//
	}

	/// <summary> Dispose pattern </summary>
	public override void Dispose()
	{
		Dispose(true);
		// Suppress finalization.
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (_disposed)
			return;
		lock (_locker)
		{
#if DEBUG
			Debug.WriteLine($"{nameof(ContextId)} {ContextId}: context disposed");
#endif
			// Base Dispose.
			base.Dispose();
			// Release managed resources.
			if (disposing)
				ReleaseManagedResources();
			// Release unmanaged resources.
			ReleaseUnmanagedResources();
			// Flag.
			_disposed = true;
		}
	}

	/// <summary> Dispose async pattern | await using </summary>
	public override ValueTask DisposeAsync()
	{
#if DEBUG
		Debug.WriteLine($"{nameof(ContextId)} {ContextId}: context disposed async");
#endif
		// Base Dispose.
		var result = base.DisposeAsync();
		// Release unmanaged resources.
		Dispose(false);
		// Suppress finalization.
		GC.SuppressFinalize(this);
		// Result.
		return result;
	}

	#endregion

	#region Public and private methods

	private void InitRepositories()
	{
#if DEBUG
		Debug.WriteLine($"{nameof(ContextId)} {ContextId}: init repositories");
#endif
		AppRepository = new TgEfAppRepository(this);
		DocumentRepository = new TgEfDocumentRepository(this);
		FilterRepository = new TgEfFilterRepository(this);
		MessageRepository = new TgEfMessageRepository(this);
		ProxyRepository = new TgEfProxyRepository(this);
		SourceRepository = new TgEfSourceRepository(this);
		VersionRepository = new TgEfVersionRepository(this);

		CreateOrConnectDb();
	}

#if DEBUG
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder
			.LogTo(message => Debug.WriteLine($"{nameof(ContextId)} {ContextId}: {message}"), LogLevel.Information)
			.EnableThreadSafetyChecks()
			.EnableDetailedErrors()
			.EnableSensitiveDataLogging()
		;
	}
#endif

	//// Magic string.
	//// Define the model.
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Concurrency tokens
		// https://learn.microsoft.com/en-us/ef/core/modeling/table-splitting
		// This property isn't on the C# class, so we set it up as a "shadow" property and use it for concurrency.
		modelBuilder.Entity<TgEfAppEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfDocumentEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfFilterEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfMessageEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfProxyEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfSourceEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfVersionEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgStorageConstants.ColumnRowVersion);
		// Ingore.
		modelBuilder.Ignore<TgEfEntityBase>();
		modelBuilder.Entity<TgEfAppEntity>().Ignore(x => x.RowVersion);
		modelBuilder.Entity<TgEfDocumentEntity>().Ignore(x => x.RowVersion);
		modelBuilder.Entity<TgEfFilterEntity>().Ignore(x => x.RowVersion);
		modelBuilder.Entity<TgEfMessageEntity>().Ignore(x => x.RowVersion);
		modelBuilder.Entity<TgEfProxyEntity>().Ignore(x => x.RowVersion);
		modelBuilder.Entity<TgEfSourceEntity>().Ignore(x => x.RowVersion);
		modelBuilder.Entity<TgEfVersionEntity>().Ignore(x => x.RowVersion);
		// FK.
		modelBuilder.Entity<TgEfAppEntity>()
			.HasOne(app => app.Proxy)
			.WithMany(proxy => proxy.Apps)
			.HasForeignKey(app => app.ProxyUid)
			.HasPrincipalKey(proxy => proxy.Uid);
		modelBuilder.Entity<TgEfDocumentEntity>()
			.HasOne(document => document.Source)
			.WithMany(source => source.Documents)
			.HasForeignKey(document => document.SourceId)
			.HasPrincipalKey(source => source.Id)
			.IsRequired();
		modelBuilder.Entity<TgEfMessageEntity>()
			.HasOne(message => message.Source)
			.WithMany(source => source.Messages)
			.HasForeignKey(message => message.SourceId)
			.HasPrincipalKey(source => source.Id)
			.IsRequired();
		// Result.
		base.OnModelCreating(modelBuilder);
	}

	public IEnumerable<ITgDbEntity> GetTableModels()
	{
		yield return new TgEfAppEntity();
		yield return new TgEfDocumentEntity();
		yield return new TgEfFilterEntity();
		yield return new TgEfProxyEntity();
		yield return new TgEfSourceEntity();
		yield return new TgEfVersionEntity();
	}

	public IEnumerable<Type> GetTableTypes()
	{
		yield return typeof(TgEfAppEntity);
		yield return typeof(TgEfDocumentEntity);
		yield return typeof(TgEfFilterEntity);
		yield return typeof(TgEfProxyEntity);
		yield return typeof(TgEfSourceEntity);
		yield return typeof(TgEfVersionEntity);
	}

	public TgEfOperResult<TgEfProxyEntity> GetCurrentProxy()
	{
		TgEfOperResult<TgEfAppEntity> operResultApp = AppRepository.GetFirst(isNoTracking: true);
		if (operResultApp.NotExist)
			return new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExist);
		TgEfOperResult<TgEfProxyEntity> operResultProxy = ProxyRepository.Get(
			new TgEfProxyEntity { Uid = operResultApp.Item.ProxyUid ?? Guid.Empty }, isNoTracking: true);
		return operResultProxy.IsExist ? operResultProxy : new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExist);
	}

	public async Task<TgEfOperResult<TgEfProxyEntity>> GetCurrentProxyAsync()
	{
		TgEfOperResult<TgEfAppEntity> operResultApp = await AppRepository.GetFirstAsync(isNoTracking: true);
		if (operResultApp.NotExist)
			return new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExist);
		TgEfOperResult<TgEfProxyEntity> operResultProxy = await ProxyRepository.GetAsync(
			new TgEfProxyEntity { Uid = operResultApp.Item.ProxyUid ?? Guid.Empty }, isNoTracking: true);
		return operResultProxy.IsExist ? operResultProxy : new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExist);
	}

	public Guid GetCurrentProxyUid() => GetCurrentProxy().Item.Uid;

	public async Task<Guid> GetCurrentProxyUidAsync() => (await GetCurrentProxyAsync()).Item.Uid;

	public string ToConsoleStringShort(TgEfSourceEntity source) =>
		$"{GetPercentCountString(source)} | {(source.IsAutoUpdate ? "a | " : "")} | {source.Id} | " +
		$"{(string.IsNullOrEmpty(source.UserName) ? "" : TgDataFormatUtils.GetFormatString(source.UserName, 30))} | " +
		$"{(string.IsNullOrEmpty(source.Title) ? "" : TgDataFormatUtils.GetFormatString(source.Title, 30))} | " +
		$"{source.FirstId} {TgLocale.From} {source.Count} {TgLocale.Messages}";

	public string ToConsoleString(TgEfSourceEntity source) =>
		$"{GetPercentCountString(source)} | {(source.IsAutoUpdate ? "a" : " ")} | {source.Id} | " +
		$"{TgDataFormatUtils.GetFormatString(source.UserName, 30)} | " +
		$"{TgDataFormatUtils.GetFormatString(source.Title, 30)} | " +
		$"{source.FirstId} {TgLocale.From} {source.Count} {TgLocale.Messages}";

	public string ToConsoleString(TgEfVersionEntity version) =>
		$"{version.Version}	{version.Description}";

	public string GetPercentCountString(TgEfSourceEntity source)
	{
		float percent = source.Count <= source.FirstId ? 100 : source.FirstId > 1 ? (float)source.FirstId * 100 / source.Count : 0;
		if (IsPercentCountAll(source))
			return "100.00 %";
		return percent > 9 ? $" {percent:00.00} %" : $"  {percent:0.00} %";
	}

	public bool IsPercentCountAll(TgEfSourceEntity source) => source.Count <= source.FirstId;

	/// <summary> Check table exists </summary>
	/// <param name="tableName"></param>
	public bool IsTableExists(string tableName)
	{
		string? result = Database.SqlQuery<string>(
			$"SELECT [name] AS [Value] FROM [sqlite_master]").SingleOrDefault(x => x == tableName);
		return tableName == result;
	}

	public static short LastVersion => 24;

	public void VersionsView()
	{
		TgEfOperResult<TgEfVersionEntity> operResult = VersionRepository.GetEnumerable(TgEnumTableTopRecords.All, isNoTracking: true);
		if (operResult.IsExist)
		{
			foreach (TgEfVersionEntity version in operResult.Items)
			{
				TgLog.WriteLine($" {version.Version:00} | {version.Description}");
			}
		}
	}

	public void FiltersView()
	{
		TgEfOperResult<TgEfFilterEntity> operResult = FilterRepository.GetEnumerable(TgEnumTableTopRecords.All, isNoTracking: true);
		if (operResult.IsExist)
		{
			foreach (TgEfFilterEntity filter in operResult.Items)
			{
				TgLog.WriteLine($"{filter}");
			}
		}
	}

	public (bool IsSuccess, string FileName) BackupDb()
	{
		if (File.Exists(TgAppSettings.AppXml.FileStorage))
		{
			DateTime dt = DateTime.Now;
			string fileBackup =
				$"{Path.GetDirectoryName(TgAppSettings.AppXml.FileStorage)}\\" +
				$"{Path.GetFileNameWithoutExtension(TgAppSettings.AppXml.FileStorage)}_{dt:yyyyMMdd}_{dt:HHmmss}.bak";
			File.Copy(TgAppSettings.AppXml.FileStorage, fileBackup);
			return (File.Exists(fileBackup), fileBackup);
		}
		return (false, string.Empty);
	}

	public void CreateOrConnectDb()
	{
		CheckTablesCrudAsync().GetAwaiter().GetResult();
	}

	public async Task CreateOrConnectDbAsync()
	{
		await CheckTablesCrudAsync();
	}

	/// <summary> Update structures of tables </summary>
	private async Task CheckTablesCrudAsync()
	{
		TgEfVersionEntity versionLast = GetLastVersion();
		if (versionLast.Version < 19)
		{
			// Upgrade DB.
			TgEfOperResult<TgEfAppEntity> operResultApps = await AlterTableNoCaseUidAsync<TgEfAppEntity>();
			if (operResultApps.State == TgEnumEntityState.NotExecuted)
				throw new(TgLocale.TablesAppsException);
			TgEfOperResult<TgEfDocumentEntity> operResultDocuments = await AlterTableNoCaseUidAsync<TgEfDocumentEntity>();
			if (operResultDocuments.State == TgEnumEntityState.NotExecuted)
				throw new(TgLocale.TablesAppsException);
			TgEfOperResult<TgEfFilterEntity> operResultFilters = await AlterTableNoCaseUidAsync<TgEfFilterEntity>();
			if (operResultFilters.State == TgEnumEntityState.NotExecuted)
				throw new(TgLocale.TablesAppsException);
			TgEfOperResult<TgEfMessageEntity> operResultMessages = await AlterTableNoCaseUidAsync<TgEfMessageEntity>();
			if (operResultMessages.State == TgEnumEntityState.NotExecuted)
				throw new(TgLocale.TablesAppsException);
			TgEfOperResult<TgEfProxyEntity> operResultProxies = await AlterTableNoCaseUidAsync<TgEfProxyEntity>();
			if (operResultProxies.State == TgEnumEntityState.NotExecuted)
				throw new(TgLocale.TablesAppsException);
			TgEfOperResult<TgEfSourceEntity> operResultSources = await AlterTableNoCaseUidAsync<TgEfSourceEntity>();
			if (operResultSources.State == TgEnumEntityState.NotExecuted)
				throw new(TgLocale.TablesAppsException);
			TgEfOperResult<TgEfVersionEntity> operResultVersions = await AlterTableNoCaseUidAsync<TgEfVersionEntity>();
			if (operResultVersions.State == TgEnumEntityState.NotExecuted)
				throw new(TgLocale.TablesAppsException);
			// Update UID.
			await UpdateTableUidUpperCaseAllAsync<TgEfAppEntity>();
			await UpdateTableUidUpperCaseAllAsync<TgEfDocumentEntity>();
			await UpdateTableUidUpperCaseAllAsync<TgEfFilterEntity>();
			await UpdateTableUidUpperCaseAllAsync<TgEfMessageEntity>();
			await UpdateTableUidUpperCaseAllAsync<TgEfProxyEntity>();
			await UpdateTableUidUpperCaseAllAsync<TgEfSourceEntity>();
			await UpdateTableUidUpperCaseAllAsync<TgEfVersionEntity>();
			// Compact DB.
			await CompactDbAsync();
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

	private async Task<bool> CheckTableCrudAsync<T>(ITgEfRepository<T> repository) where T : TgEfEntityBase, ITgDbEntity, new()
	{
		var operResult = await repository.CreateNewAsync();
		if (operResult.NotExist)
			return false;
		operResult = await repository.GetNewAsync(isNoTracking: false);
		if (operResult.NotExist)
			return false;
		operResult = await repository.SaveAsync(operResult.Item);
		if (operResult.State != TgEnumEntityState.IsSaved)
			return false;
		operResult = await repository.DeleteAsync(operResult.Item, isSkipFind: false);
		return operResult.State == TgEnumEntityState.IsDeleted;
	}

	public Task<bool> CheckTableAppsCrudAsync() => CheckTableCrudAsync(AppRepository);

	public Task<bool> CheckTableDocumentsCrudAsync() => CheckTableCrudAsync(DocumentRepository);

	public Task<bool> CheckTableFiltersCrudAsync() => CheckTableCrudAsync(FilterRepository);

	public Task<bool> CheckTableMessagesCrudAsync() => CheckTableCrudAsync(MessageRepository);

	public Task<bool> CheckTableProxiesCrudAsync() => CheckTableCrudAsync(ProxyRepository);

	public Task<bool> CheckTableSourcesCrudAsync() => CheckTableCrudAsync(SourceRepository);

	public Task<bool> CheckTableVersionsCrudAsync() => CheckTableCrudAsync(VersionRepository);

	public TgEfVersionEntity GetLastVersion()
	{
		TgEfVersionEntity versionLast = !IsTableExists(TgStorageConstants.TableVersions)
			? new() : VersionRepository.GetEnumerable(TgEnumTableTopRecords.All, isNoTracking: true).Items
				.Where(x => x.Description != "New version")
				.OrderBy(x => x.Version)
				.Last();
		return versionLast;
	}

	public async Task FillTableVersionsAsync()
	{
		await VersionRepository.DeleteNewAsync();
		bool isLast = false;
		while (!isLast)
		{
			TgEfVersionEntity versionLast = GetLastVersion();
			if (Equals(versionLast.Version, short.MaxValue))
				versionLast.Version = 0;
			switch (versionLast.Version)
			{
				case 0:
					await VersionRepository.SaveAsync(new() { Version = 1, Description = "Added versions table" });
					break;
				case 1:
					await VersionRepository.SaveAsync(new() { Version = 2, Description = "Added apps table" });
					break;
				case 2:
					await VersionRepository.SaveAsync(new() { Version = 3, Description = "Added documents table" });
					break;
				case 3:
					await VersionRepository.SaveAsync(new() { Version = 4, Description = "Added filters table" });
					break;
				case 4:
					await VersionRepository.SaveAsync(new() { Version = 5, Description = "Added messages table" });
					break;
				case 5:
					await VersionRepository.SaveAsync(new() { Version = 6, Description = "Added proxies table" });
					break;
				case 6:
					await VersionRepository.SaveAsync(new() { Version = 7, Description = "Added sources table" });
					break;
				case 7:
					await VersionRepository.SaveAsync(new() { Version = 8, Description = "Added source settings table" });
					break;
				case 8:
					await VersionRepository.SaveAsync(new() { Version = 9, Description = "Upgrade versions table" });
					break;
				case 9:
					await VersionRepository.SaveAsync(new() { Version = 10, Description = "Upgrade apps table" });
					break;
				case 10:
					await VersionRepository.SaveAsync(new() { Version = 11, Description = "Upgrade storage on XPO framework" });
					break;
				case 11:
					await VersionRepository.SaveAsync(new() { Version = 12, Description = "Upgrade apps table" });
					break;
				case 12:
					await VersionRepository.SaveAsync(new() { Version = 13, Description = "Upgrade documents table" });
					break;
				case 13:
					await VersionRepository.SaveAsync(new() { Version = 14, Description = "Upgrade filters table" });
					break;
				case 14:
					await VersionRepository.SaveAsync(new() { Version = 15, Description = "Upgrade messages table" });
					break;
				case 15:
					await VersionRepository.SaveAsync(new() { Version = 16, Description = "Upgrade proxies table" });
					break;
				case 16:
					await VersionRepository.SaveAsync(new() { Version = 17, Description = "Upgrade sources table" });
					break;
				case 17:
					await VersionRepository.SaveAsync(new() { Version = 18, Description = "Updating the UID field in the apps table" });
					break;
				case 18:
					await VersionRepository.SaveAsync(new() { Version = 19, Description = "Updating the UID field in the documents table" });
					break;
				case 19:
					await VersionRepository.SaveAsync(new() { Version = 20, Description = "Updating the UID field in the filters table" });
					break;
				case 20:
					await VersionRepository.SaveAsync(new() { Version = 21, Description = "Updating the UID field in the messages table" });
					break;
				case 21:
					await VersionRepository.SaveAsync(new() { Version = 22, Description = "Updating the UID field in the proxies table" });
					break;
				case 22:
					await VersionRepository.SaveAsync(new() { Version = 23, Description = "Updating the UID field in the sources table" });
					break;
				case 23:
					await VersionRepository.SaveAsync(new() { Version = 24, Description = "Updating the UID field in the versions table" });
					break;
			}
			if (versionLast.Version >= LastVersion)
				isLast = true;
		}
	}

	public void DeleteTables()
	{
		DeleteTable<TgEfAppEntity>();
		DeleteTable<TgEfProxyEntity>();
		DeleteTable<TgEfFilterEntity>();
		DeleteTable<TgEfDocumentEntity>();
		DeleteTable<TgEfMessageEntity>();
		DeleteTable<TgEfSourceEntity>();
		DeleteTable<TgEfVersionEntity>();
	}

	public async Task DeleteTablesAsync()
	{
		await DeleteTableAsync<TgEfAppEntity>();
		await DeleteTableAsync<TgEfProxyEntity>();
		await DeleteTableAsync<TgEfFilterEntity>();
		await DeleteTableAsync<TgEfDocumentEntity>();
		await DeleteTableAsync<TgEfMessageEntity>();
		await DeleteTableAsync<TgEfSourceEntity>();
		await DeleteTableAsync<TgEfVersionEntity>();
	}

	/// <summary> Truncate sql table by name </summary>
	public async Task<TgEfOperResult<T>> TruncateTableAsync<T>() where T : TgEfEntityBase, ITgDbEntity, new()
	{
		string cmd = string.Empty;
		switch (typeof(T))
		{
			case var cls when cls == typeof(TgEfAppEntity):
				cmd = $"TRUNCATE TABLE {TgStorageConstants.TableApps};";
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				cmd = $"TRUNCATE TABLE {TgStorageConstants.TableDocuments};";
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				cmd = $"TRUNCATE TABLE {TgStorageConstants.TableFilters};";
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				cmd = $"TRUNCATE TABLE {TgStorageConstants.TableMessages};";
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				cmd = $"TRUNCATE TABLE {TgStorageConstants.TableProxies};";
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				cmd = $"TRUNCATE TABLE {TgStorageConstants.TableSources};";
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				cmd = $"TRUNCATE TABLE {TgStorageConstants.TableVersions};";
				break;
		}
		if (!string.IsNullOrEmpty(cmd))
		{
			int result = await Database.ExecuteSqlRawAsync(cmd);
			return new TgEfOperResult<T>(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new TgEfOperResult<T>(TgEnumEntityState.NotExecuted);
	}

	/// <summary> Drop sql table </summary>
	public TgEfOperResult<T> DeleteTable<T>() where T : TgEfEntityBase, ITgDbEntity, new()
	{
		string cmd = string.Empty;
		switch (typeof(T))
		{
			case var cls when cls == typeof(TgEfAppEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableApps};";
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableDocuments};";
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableFilters};";
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableMessages};";
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableProxies};";
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableSources};";
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableVersions};";
				break;
		}
		if (!string.IsNullOrEmpty(cmd))
		{
			int result = Database.ExecuteSqlRaw(cmd);
			return new TgEfOperResult<T>(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new TgEfOperResult<T>(TgEnumEntityState.NotExecuted);
	}

	/// <summary> Drop sql table </summary>
	public async Task<TgEfOperResult<T>> DeleteTableAsync<T>() where T : TgEfEntityBase, ITgDbEntity, new()
	{
		string cmd = string.Empty;
		switch (typeof(T))
		{
			case var cls when cls == typeof(TgEfAppEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableApps};";
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableDocuments};";
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableFilters};";
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableMessages};";
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableProxies};";
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableSources};";
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				cmd = $"DROP TABLE IF EXISTS {TgStorageConstants.TableVersions};";
				break;
		}
		if (!string.IsNullOrEmpty(cmd))
		{
			int result = await Database.ExecuteSqlRawAsync(cmd);
			return new TgEfOperResult<T>(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new TgEfOperResult<T>(TgEnumEntityState.NotExecuted);
	}

	public async Task<TgEfOperResult<T>> AlterTableNoCaseUidAsync<T>() where T : TgEfEntityBase, ITgDbEntity, new()
	{
		string cmd = string.Empty;
		switch (typeof(T))
		{
			case var cls when cls == typeof(TgEfAppEntity):
				cmd = @"
-- APPS_ALTER
BEGIN TRANSACTION;
CREATE TABLE [APPS_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [API_HASH] char(36),
    [API_ID] int,
    [PHONE_NUMBER] nvarchar(16),
    [PROXY_UID] char(36), 
    primary key ([UID])
);
INSERT INTO [APPS_TEMP] SELECT * FROM [APPS];
DROP TABLE [APPS];
ALTER TABLE [APPS_TEMP] RENAME TO [APPS];
COMMIT TRANSACTION;
					".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				cmd = @"
-- DOCUMENTS_ALTER
BEGIN TRANSACTION;
CREATE TABLE [DOCUMENTS_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [SOURCE_ID] numeric(20,0), 
    [ID] numeric(20,0), 
    [MESSAGE_ID] numeric(20,0), 
    [FILE_NAME] nvarchar(100), 
    [FILE_SIZE] numeric(20,0), 
    [ACCESS_HASH] numeric(20,0),
    primary key ([UID])
);
INSERT INTO [DOCUMENTS_TEMP] SELECT * FROM [DOCUMENTS];
DROP TABLE [DOCUMENTS];
ALTER TABLE [DOCUMENTS_TEMP] RENAME TO [DOCUMENTS];
COMMIT TRANSACTION;
					".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				cmd = @"
-- FILTERS_ALTER
BEGIN TRANSACTION;
CREATE TABLE [FILTERS_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [IS_ENABLED] bit, 
    [FILTER_TYPE] int, 
    [NAME] nvarchar(128), 
    [MASK] nvarchar(128), 
    [SIZE] numeric(20,0), 
    [SIZE_TYPE] int,
    primary key ([UID])
);
INSERT INTO [FILTERS_TEMP] SELECT * FROM [FILTERS];
DROP TABLE [FILTERS];
ALTER TABLE [FILTERS_TEMP] RENAME TO [FILTERS];
COMMIT TRANSACTION;
					".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				cmd = @"
-- MESSAGES_ALTER
BEGIN TRANSACTION;
CREATE TABLE [MESSAGES_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [SOURCE_ID] numeric(20,0), 
    [ID] numeric(20,0), 
    [DT_CREATED] datetime, 
    [TYPE] int, 
    [SIZE] numeric(20,0), 
    [MESSAGE] nvarchar(100),
    primary key ([UID])
);
INSERT INTO [MESSAGES_TEMP] SELECT * FROM [MESSAGES];
DROP TABLE [MESSAGES];
ALTER TABLE [MESSAGES_TEMP] RENAME TO [MESSAGES];
COMMIT TRANSACTION;
					".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				cmd = @"
-- PROXIES_ALTER
BEGIN TRANSACTION;
CREATE TABLE [PROXIES_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [TYPE] int, 
    [HOST_NAME] nvarchar(128), 
    [PORT] numeric(5,0), 
    [USER_NAME] nvarchar(128), 
    [PASSWORD] nvarchar(128), 
    [SECRET] nvarchar(128),
    primary key ([UID])
);
INSERT INTO [PROXIES_TEMP] SELECT * FROM [PROXIES];
DROP TABLE [PROXIES];
ALTER TABLE [PROXIES_TEMP] RENAME TO [PROXIES];
COMMIT TRANSACTION;
					".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				cmd = @"
-- SOURCES_ALTER
BEGIN TRANSACTION;
CREATE TABLE [SOURCES_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [ID] numeric(20,0), 
    [USER_NAME] nvarchar(100), 
    [TITLE] nvarchar(100), 
    [ABOUT] nvarchar(100), 
    [COUNT] int, 
    [DIRECTORY] nvarchar(100), 
    [FIRST_ID] int, 
    [IS_AUTO_UPDATE] bit, 
    [DT_CHANGED] datetime,
    primary key ([UID])
);
INSERT INTO [SOURCES_TEMP] SELECT * FROM [SOURCES];
DROP TABLE [SOURCES];
ALTER TABLE [SOURCES_TEMP] RENAME TO [SOURCES];
COMMIT TRANSACTION;
					".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				cmd = @"
-- VERSIONS_ALTER
BEGIN TRANSACTION;
CREATE TABLE [VERSIONS_TEMP] (
    [UID] char(36) NOT NULL COLLATE NOCASE,
    [VERSION] smallint, 
    [DESCRIPTION] nvarchar(128),
    primary key ([UID])
);
INSERT INTO [VERSIONS_TEMP] SELECT * FROM [VERSIONS];
DROP TABLE [VERSIONS];
ALTER TABLE [VERSIONS_TEMP] RENAME TO [VERSIONS];
COMMIT TRANSACTION;
					".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');
				break;
		}
		if (!string.IsNullOrEmpty(cmd))
		{
			int result = await Database.ExecuteSqlRawAsync(cmd);
			return new TgEfOperResult<T>(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new TgEfOperResult<T>(TgEnumEntityState.NotExecuted);
	}

	public async Task CompactDbAsync() => await Database.ExecuteSqlRawAsync("VACUUM;");

	/// <summary> Update UID field to upper case </summary>
	/// <param name="uid"> PK UID </param>
	public async Task<TgEfOperResult<T>> UpdateTableUidUpperCaseAllAsync<T>() where T : TgEfEntityBase, ITgDbEntity, new()
	{
		string cmd = string.Empty;
		switch (typeof(T))
		{
			case var cls when cls == typeof(TgEfAppEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableApps}] SET [UID] = UPPER([UID]);";
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableDocuments}] SET [UID] = UPPER([UID]);";
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableFilters}] SET [UID] = UPPER([UID]);";
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableMessages}] SET [UID] = UPPER([UID]);";
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableProxies}] SET [UID] = UPPER([UID]);";
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableSources}] SET [UID] = UPPER([UID]);";
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableVersions}] SET [UID] = UPPER([UID]);";
				break;
		}
		if (!string.IsNullOrEmpty(cmd))
		{
			int result = await Database.ExecuteSqlRawAsync(cmd);
			return new TgEfOperResult<T>(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new TgEfOperResult<T>(TgEnumEntityState.NotExecuted);
	}

	/// <summary> Update UID field to upper case </summary>
	/// <param name="uid"> PK UID </param>
	public async Task<TgEfOperResult<T>> UpdateTableUidUpperCaseAsync<T>(Guid uid) where T : TgEfEntityBase, ITgDbEntity, new()
	{
		string cmd = string.Empty;
		string uidUpper = uid.ToString().ToUpper();
		string uidString = uid.ToString();
		switch (typeof(T))
		{
			case var cls when cls == typeof(TgEfAppEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableApps}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableDocuments}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableFilters}] SET [UID] = '{uidUpper}' WHERE U[UID]ID = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableMessages}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableProxies}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableSources}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableVersions}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';";
				break;
		}
		if (!string.IsNullOrEmpty(cmd))
		{
			int result = await Database.ExecuteSqlRawAsync(cmd);
			return new TgEfOperResult<T>(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new TgEfOperResult<T>(TgEnumEntityState.NotExecuted);
	}

	/// <summary> Update UID field to lower case </summary>
	/// <param name="uid"> PK UID </param>
	public async Task<TgEfOperResult<T>> UpdateTableUidLowerCaseAsync<T>(Guid uid) where T : TgEfEntityBase, ITgDbEntity, new()
	{
		string cmd = string.Empty;
		string uidLower = uid.ToString().ToLower();
		string uidString = uid.ToString();
		switch (typeof(T))
		{
			case var cls when cls == typeof(TgEfAppEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableApps}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableDocuments}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableFilters}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableMessages}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableProxies}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableSources}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				cmd = $"UPDATE [{TgStorageConstants.TableVersions}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
				break;
		}
		if (!string.IsNullOrEmpty(cmd))
		{
			int result = await Database.ExecuteSqlRawAsync(cmd);
			return new TgEfOperResult<T>(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new TgEfOperResult<T>(TgEnumEntityState.NotExecuted);
	}

	#endregion
}