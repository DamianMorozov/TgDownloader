// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

#pragma warning disable S2589
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
		CheckIfDisposed();
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
		CheckIfDisposed();
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
		CheckIfDisposed();
		string cmd = typeof(T) switch
		{
			var cls when cls == typeof(TgEfAppEntity) => $"TRUNCATE TABLE {TgEfConstants.TableApps};",
			var cls when cls == typeof(TgEfDocumentEntity) => $"TRUNCATE TABLE {TgEfConstants.TableDocuments};",
			var cls when cls == typeof(TgEfFilterEntity) => $"TRUNCATE TABLE {TgEfConstants.TableFilters};",
			var cls when cls == typeof(TgEfMessageEntity) => $"TRUNCATE TABLE {TgEfConstants.TableMessages};",
			var cls when cls == typeof(TgEfProxyEntity) => $"TRUNCATE TABLE {TgEfConstants.TableProxies};",
			var cls when cls == typeof(TgEfSourceEntity) => $"TRUNCATE TABLE {TgEfConstants.TableSources};",
			var cls when cls == typeof(TgEfVersionEntity) => $"TRUNCATE TABLE {TgEfConstants.TableVersions};",
			_ => string.Empty
		};
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
		CheckIfDisposed();
		string cmd = typeof(T) switch
		{
			var cls when cls == typeof(TgEfAppEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableApps};",
			var cls when cls == typeof(TgEfDocumentEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableDocuments};",
			var cls when cls == typeof(TgEfFilterEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableFilters};",
			var cls when cls == typeof(TgEfMessageEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableMessages};",
			var cls when cls == typeof(TgEfProxyEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableProxies};",
			var cls when cls == typeof(TgEfSourceEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableSources};",
			var cls when cls == typeof(TgEfVersionEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableVersions};",
			_ => string.Empty
		};
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
		CheckIfDisposed();
		string cmd = typeof(T) switch
		{
			var cls when cls == typeof(TgEfAppEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableApps};",
			var cls when cls == typeof(TgEfDocumentEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableDocuments};",
			var cls when cls == typeof(TgEfFilterEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableFilters};",
			var cls when cls == typeof(TgEfMessageEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableMessages};",
			var cls when cls == typeof(TgEfProxyEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableProxies};",
			var cls when cls == typeof(TgEfSourceEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableSources};",
			var cls when cls == typeof(TgEfVersionEntity) => $"DROP TABLE IF EXISTS {TgEfConstants.TableVersions};",
			_ => string.Empty
		};
		if (!string.IsNullOrEmpty(cmd))
		{
			int result = await Database.ExecuteSqlRawAsync(cmd);
			return new TgEfOperResult<T>(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new TgEfOperResult<T>(TgEnumEntityState.NotExecuted);
	}

	public TgEfOperResult<T> AlterTableNoCaseUid<T>() where T : TgEfEntityBase, ITgDbEntity, new()
	{
		CheckIfDisposed();
		string cmd = typeof(T) switch
		{
			var cls when cls == typeof(TgEfAppEntity) => TgEfQueries.QueryAlterApps,
			var cls when cls == typeof(TgEfDocumentEntity) => TgEfQueries.QueryAlterDocuments,
			var cls when cls == typeof(TgEfFilterEntity) => TgEfQueries.QueryAlterFilters,
			var cls when cls == typeof(TgEfMessageEntity) => TgEfQueries.QueryAlterMessages,
			var cls when cls == typeof(TgEfProxyEntity) => TgEfQueries.QueryAlterProxies,
			var cls when cls == typeof(TgEfSourceEntity) => TgEfQueries.QueryAlterSources,
			var cls when cls == typeof(TgEfVersionEntity) => TgEfQueries.QueryAlterVersions,
			_ => string.Empty
		};
		if (!string.IsNullOrEmpty(cmd))
		{
			int result = Database.ExecuteSqlRaw(cmd);
			return new TgEfOperResult<T>(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new TgEfOperResult<T>(TgEnumEntityState.NotExecuted);
	}

	public async Task<TgEfOperResult<T>> AlterTableNoCaseUidAsync<T>() where T : TgEfEntityBase, ITgDbEntity, new()
	{
		CheckIfDisposed();
		string cmd = typeof(T) switch
		{
			var cls when cls == typeof(TgEfAppEntity) => TgEfQueries.QueryAlterApps,
			var cls when cls == typeof(TgEfDocumentEntity) => TgEfQueries.QueryAlterDocuments,
			var cls when cls == typeof(TgEfFilterEntity) => TgEfQueries.QueryAlterFilters,
			var cls when cls == typeof(TgEfMessageEntity) => TgEfQueries.QueryAlterMessages,
			var cls when cls == typeof(TgEfProxyEntity) => TgEfQueries.QueryAlterProxies,
			var cls when cls == typeof(TgEfSourceEntity) => TgEfQueries.QueryAlterSources,
			var cls when cls == typeof(TgEfVersionEntity) => TgEfQueries.QueryAlterVersions,
			_ => string.Empty
		};
		if (!string.IsNullOrEmpty(cmd))
		{
			int result = await Database.ExecuteSqlRawAsync(cmd);
			return new TgEfOperResult<T>(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new TgEfOperResult<T>(TgEnumEntityState.NotExecuted);
	}

	public void CompactDb()
	{
		CheckIfDisposed();
		Database.ExecuteSqlRaw("VACUUM;");
	}

	public async Task CompactDbAsync()
	{
		CheckIfDisposed();
		await Database.ExecuteSqlRawAsync("VACUUM;");
	}

	/// <summary> Update UID field to upper case </summary>
	/// <param name="uid"> PK UID </param>
	public TgEfOperResult<T> UpdateTableUidUpperCaseAll<T>() where T : TgEfEntityBase, ITgDbEntity, new()
	{
		CheckIfDisposed();
		string cmd = typeof(T) switch
		{
			var cls when cls == typeof(TgEfAppEntity) => $"UPDATE [{TgEfConstants.TableApps}] SET [UID] = UPPER([UID]);",
			var cls when cls == typeof(TgEfDocumentEntity) => $"UPDATE [{TgEfConstants.TableDocuments}] SET [UID] = UPPER([UID]);",
			var cls when cls == typeof(TgEfFilterEntity) => $"UPDATE [{TgEfConstants.TableFilters}] SET [UID] = UPPER([UID]);",
			var cls when cls == typeof(TgEfMessageEntity) => $"UPDATE [{TgEfConstants.TableMessages}] SET [UID] = UPPER([UID]);",
			var cls when cls == typeof(TgEfProxyEntity) => $"UPDATE [{TgEfConstants.TableProxies}] SET [UID] = UPPER([UID]);",
			var cls when cls == typeof(TgEfSourceEntity) => $"UPDATE [{TgEfConstants.TableSources}] SET [UID] = UPPER([UID]);",
			var cls when cls == typeof(TgEfVersionEntity) => $"UPDATE [{TgEfConstants.TableVersions}] SET [UID] = UPPER([UID]);",
			_ => string.Empty
		};
		if (!string.IsNullOrEmpty(cmd))
		{
			int result = Database.ExecuteSqlRaw(cmd);
			return new TgEfOperResult<T>(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new TgEfOperResult<T>(TgEnumEntityState.NotExecuted);
	}

	/// <summary> Update UID field to upper case </summary>
	/// <param name="uid"> PK UID </param>
	public async Task<TgEfOperResult<T>> UpdateTableUidUpperCaseAllAsync<T>() where T : TgEfEntityBase, ITgDbEntity, new()
	{
		CheckIfDisposed();
		string cmd = typeof(T) switch
		{
			var cls when cls == typeof(TgEfAppEntity) => $"UPDATE [{TgEfConstants.TableApps}] SET [UID] = UPPER([UID]);",
			var cls when cls == typeof(TgEfDocumentEntity) => $"UPDATE [{TgEfConstants.TableDocuments}] SET [UID] = UPPER([UID]);",
			var cls when cls == typeof(TgEfFilterEntity) => $"UPDATE [{TgEfConstants.TableFilters}] SET [UID] = UPPER([UID]);",
			var cls when cls == typeof(TgEfMessageEntity) => $"UPDATE [{TgEfConstants.TableMessages}] SET [UID] = UPPER([UID]);",
			var cls when cls == typeof(TgEfProxyEntity) => $"UPDATE [{TgEfConstants.TableProxies}] SET [UID] = UPPER([UID]);",
			var cls when cls == typeof(TgEfSourceEntity) => $"UPDATE [{TgEfConstants.TableSources}] SET [UID] = UPPER([UID]);",
			var cls when cls == typeof(TgEfVersionEntity) => $"UPDATE [{TgEfConstants.TableVersions}] SET [UID] = UPPER([UID]);",
			_ => string.Empty
		};
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
		CheckIfDisposed();
		string uidUpper = uid.ToString().ToUpper();
		string uidString = uid.ToString();
		string cmd = typeof(T) switch
		{
			var cls when cls == typeof(TgEfAppEntity) => 
				$"UPDATE [{TgEfConstants.TableApps}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';",
			var cls when cls == typeof(TgEfDocumentEntity) => 
				$"UPDATE [{TgEfConstants.TableDocuments}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';",
			var cls when cls == typeof(TgEfFilterEntity) => 
				$"UPDATE [{TgEfConstants.TableFilters}] SET [UID] = '{uidUpper}' WHERE U[UID]ID = '{uidString}';",
			var cls when cls == typeof(TgEfMessageEntity) => 
				$"UPDATE [{TgEfConstants.TableMessages}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';",
			var cls when cls == typeof(TgEfProxyEntity) => 
				$"UPDATE [{TgEfConstants.TableProxies}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';",
			var cls when cls == typeof(TgEfSourceEntity) => 
				$"UPDATE [{TgEfConstants.TableSources}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';",
			var cls when cls == typeof(TgEfVersionEntity) => 
				$"UPDATE [{TgEfConstants.TableVersions}] SET [UID] = '{uidUpper}' WHERE [UID] = '{uidString}';",
			_ => string.Empty
		};
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
		CheckIfDisposed();
		string uidLower = uid.ToString().ToLower();
		string uidString = uid.ToString();
		string cmd = typeof(T) switch
		{
			var cls when cls == typeof(TgEfAppEntity) =>
				$"UPDATE [{TgEfConstants.TableApps}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';",
			var cls when cls == typeof(TgEfDocumentEntity) =>
				$"UPDATE [{TgEfConstants.TableDocuments}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';",
			var cls when cls == typeof(TgEfFilterEntity) =>
				$"UPDATE [{TgEfConstants.TableFilters}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';",
			var cls when cls == typeof(TgEfMessageEntity) =>
				$"UPDATE [{TgEfConstants.TableMessages}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';",
			var cls when cls == typeof(TgEfProxyEntity) =>
				$"UPDATE [{TgEfConstants.TableProxies}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';",
			var cls when cls == typeof(TgEfSourceEntity) =>
				$"UPDATE [{TgEfConstants.TableSources}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';",
			var cls when cls == typeof(TgEfVersionEntity) =>
				$"UPDATE [{TgEfConstants.TableVersions}] SET [UID] = '{uidLower}' WHERE [UID] = '{uidString}';",
			_ => string.Empty
		};
		if (!string.IsNullOrEmpty(cmd))
		{
			int result = await Database.ExecuteSqlRawAsync(cmd);
			return new TgEfOperResult<T>(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new TgEfOperResult<T>(TgEnumEntityState.NotExecuted);
	}

	/// <summary> Create and update storage </summary>
	public void CreateAndUpdateDb()
	{
		CheckIfDisposed();
		Database.EnsureCreated();
		Database.Migrate();
	}

	/// <summary> Create and update storage </summary>
	public async Task CreateAndUpdateDbAsync()
	{
		CheckIfDisposed();
		await Database.EnsureCreatedAsync();
		await Database.MigrateAsync();
	}

	#endregion
}