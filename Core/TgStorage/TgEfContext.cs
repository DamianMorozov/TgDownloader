// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

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
	public TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
	public TgLocaleHelper TgLocale => TgLocaleHelper.Instance;

	public bool IsReady =>
		TgAppSettings.AppXml.IsExistsFileStorage &&
		IsTableExists(TgEfConstants.TableApps) && IsTableExists(TgEfConstants.TableDocuments) &&
		IsTableExists(TgEfConstants.TableFilters) && IsTableExists(TgEfConstants.TableMessages) &&
		IsTableExists(TgEfConstants.TableProxies) && IsTableExists(TgEfConstants.TableSources) &&
		IsTableExists(TgEfConstants.TableVersions);

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
			throw new ObjectDisposedException($"{nameof(TgEfContext)}: {TgLocaleHelper.Instance.ObjectHasBeenDisposedOff}!");
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

	public TgEfContext() : base()
	{
#if DEBUG
		Debug.WriteLine($"Created TgEfContext with {nameof(ContextId)} {ContextId}");
#endif
	}

	/// <summary> Inject options </summary>
	/// <param name="options"></param>
	// For using: services.AddDbContextFactory<TgEfContext>
	public TgEfContext(DbContextOptions<TgEfContext> options) : base(options)
	{
#if DEBUG
		Debug.WriteLine($"Created TgEfContext with {nameof(ContextId)} {ContextId}");
#endif
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		LoggerFactory factory = new();
		optionsBuilder
#if DEBUG
			.LogTo(message => Debug.WriteLine($"{nameof(ContextId)} {ContextId}: {message}"), LogLevel.Information)
			.EnableDetailedErrors()
			.EnableSensitiveDataLogging()
#endif
			.EnableThreadSafetyChecks()
			.UseLoggerFactory(factory)
			.UseSqlite($"{TgLocaleHelper.Instance.SqliteDataSource}={TgAppSettingsHelper.Instance.AppXml.FileStorage}")
			//.UseSqlite(b => b.MigrationsAssembly(assemblyName))
		;
	}

	//// Magic string.
	//// Define the model.
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Concurrency tokens
		// https://learn.microsoft.com/en-us/ef/core/modeling/table-splitting
		// This property isn't on the C# class, so we set it up as a "shadow" property and use it for concurrency.
		modelBuilder.Entity<TgEfAppEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfDocumentEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfFilterEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfMessageEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfProxyEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfSourceEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		modelBuilder.Entity<TgEfVersionEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
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

	//public string ToConsoleStringShort(TgEfSourceEntity source) =>
	//	$"{GetPercentCountString(source)} | {(source.IsAutoUpdate ? "a | " : "")} | {source.Id} | " +
	//	$"{(string.IsNullOrEmpty(source.UserName) ? "" : TgDataFormatUtils.GetFormatString(source.UserName, 30))} | " +
	//	$"{(string.IsNullOrEmpty(source.Title) ? "" : TgDataFormatUtils.GetFormatString(source.Title, 30))} | " +
	//	$"{source.FirstId} {TgLocale.From} {source.Count} {TgLocale.Messages}";

	public string ToConsoleString(TgEfSourceEntity source) =>
		$"{GetPercentCountString(source)} | {(source.IsAutoUpdate ? "a" : " ")} | {source.Id} | " +
		$"{(!string.IsNullOrEmpty(source.UserName) ? TgDataFormatUtils.GetFormatString(source.UserName, 30) : string.Empty)} | " +
		$"{(!string.IsNullOrEmpty(source.Title) ? TgDataFormatUtils.GetFormatString(source.Title, 30) : string.Empty)} | " +
		$"{source.FirstId} {TgLocale.From} {source.Count} {TgLocale.Messages}";

	public string ToConsoleString(TgEfVersionEntity version) => $"{version.Version}	{version.Description}";

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
				cmd = $"TRUNCATE TABLE {TgEfConstants.TableApps};";
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				cmd = $"TRUNCATE TABLE {TgEfConstants.TableDocuments};";
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				cmd = $"TRUNCATE TABLE {TgEfConstants.TableFilters};";
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				cmd = $"TRUNCATE TABLE {TgEfConstants.TableMessages};";
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				cmd = $"TRUNCATE TABLE {TgEfConstants.TableProxies};";
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				cmd = $"TRUNCATE TABLE {TgEfConstants.TableSources};";
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				cmd = $"TRUNCATE TABLE {TgEfConstants.TableVersions};";
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
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableApps};";
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableDocuments};";
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableFilters};";
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableMessages};";
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableProxies};";
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableSources};";
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableVersions};";
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
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableApps};";
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableDocuments};";
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableFilters};";
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableMessages};";
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableProxies};";
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableSources};";
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				cmd = $"DROP TABLE IF EXISTS {TgEfConstants.TableVersions};";
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

	public void CompactDb() => Database.ExecuteSqlRaw("VACUUM;");

	public async Task CompactDbAsync() => await Database.ExecuteSqlRawAsync("VACUUM;");

	/// <summary> Update UID field to upper case </summary>
	/// <param name="uid"> PK UID </param>
	public async Task<TgEfOperResult<T>> UpdateTableUidUpperCaseAllAsync<T>() where T : TgEfEntityBase, ITgDbEntity, new()
	{
		string cmd = string.Empty;
		switch (typeof(T))
		{
			case var cls when cls == typeof(TgEfAppEntity):
				cmd = $"UPDATE [{TgEfConstants.TableApps}] SET [UID] = UPPER([UID]);";
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				cmd = $"UPDATE [{TgEfConstants.TableDocuments}] SET [UID] = UPPER([UID]);";
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				cmd = $"UPDATE [{TgEfConstants.TableFilters}] SET [UID] = UPPER([UID]);";
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				cmd = $"UPDATE [{TgEfConstants.TableMessages}] SET [UID] = UPPER([UID]);";
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				cmd = $"UPDATE [{TgEfConstants.TableProxies}] SET [UID] = UPPER([UID]);";
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				cmd = $"UPDATE [{TgEfConstants.TableSources}] SET [UID] = UPPER([UID]);";
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				cmd = $"UPDATE [{TgEfConstants.TableVersions}] SET [UID] = UPPER([UID]);";
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
				cmd = $"UPDATE [{TgEfConstants.TableApps}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				cmd = $"UPDATE [{TgEfConstants.TableDocuments}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				cmd = $"UPDATE [{TgEfConstants.TableFilters}] SET [UID] = '{uidUpper}' WHERE U[UID]ID = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				cmd = $"UPDATE [{TgEfConstants.TableMessages}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				cmd = $"UPDATE [{TgEfConstants.TableProxies}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				cmd = $"UPDATE [{TgEfConstants.TableSources}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				cmd = $"UPDATE [{TgEfConstants.TableVersions}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';";
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
				cmd = $"UPDATE [{TgEfConstants.TableApps}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				cmd = $"UPDATE [{TgEfConstants.TableDocuments}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				cmd = $"UPDATE [{TgEfConstants.TableFilters}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				cmd = $"UPDATE [{TgEfConstants.TableMessages}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				cmd = $"UPDATE [{TgEfConstants.TableProxies}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				cmd = $"UPDATE [{TgEfConstants.TableSources}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				cmd = $"UPDATE [{TgEfConstants.TableVersions}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';";
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