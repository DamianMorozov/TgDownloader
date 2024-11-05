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

	public bool IsReady =>
		TgAppSettings.AppXml.IsExistsEfStorage &&
		IsTableExists(TgEfConstants.TableApps) && IsTableExists(TgEfConstants.TableDocuments) &&
		IsTableExists(TgEfConstants.TableFilters) && IsTableExists(TgEfConstants.TableMessages) &&
		IsTableExists(TgEfConstants.TableProxies) && IsTableExists(TgEfConstants.TableSources) &&
		IsTableExists(TgEfConstants.TableVersions);

	private string FileStorage { get; }

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

	public TgEfContext()
	{
		FileStorage = string.Empty;
#if DEBUG
		Debug.WriteLine($"Created TgEfContext with {nameof(ContextId)} {ContextId}");
#endif
	}

	public TgEfContext(string fileStorage)
	{
		FileStorage = fileStorage;
#if DEBUG
		Debug.WriteLine($"Created TgEfContext with {nameof(ContextId)} {ContextId}");
#endif
	}

	/// <summary> Inject options </summary>
	/// <param name="options"></param>
	// For using: services.AddDbContextFactory<TgEfContext>
	public TgEfContext(DbContextOptions<TgEfContext> options) : base(options)
	{
		FileStorage = string.Empty;
#if DEBUG
		Debug.WriteLine($"Created TgEfContext with {nameof(ContextId)} {ContextId}");
#endif
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		LoggerFactory factory = new();
		string storagePath = string.IsNullOrEmpty(FileStorage)
			? $"{TgLocaleHelper.Instance.SqliteDataSource}={TgAppSettingsHelper.Instance.AppXml.XmlEfStorage}"
			: $"{TgLocaleHelper.Instance.SqliteDataSource}={FileStorage}";
		optionsBuilder
#if DEBUG
			.LogTo(message => Debug.WriteLine($"{nameof(ContextId)} {ContextId}: {message}"), LogLevel.Information)
			.EnableDetailedErrors()
			.EnableSensitiveDataLogging()
#endif
			.EnableThreadSafetyChecks()
			.UseLoggerFactory(factory)
			.UseSqlite(storagePath)
		;
#if DEBUG
		Debug.WriteLine(storagePath);
#endif
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		//// Magic string - Define the model
		//// Concurrency tokens
		//// https://learn.microsoft.com/en-us/ef/core/modeling/table-splitting
		// This property isn't on the C# class, so we set it up as a "shadow" property and use it for concurrency.
		//modelBuilder.Entity<TgEfAppEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfDocumentEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfFilterEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfMessageEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfProxyEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfSourceEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfVersionEntity>().Property<byte[]>(x => x.RowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//// This property isn't on the C# class, so we set it up as a "shadow" property and use it for concurrency.
		//modelBuilder.Entity<TgEfAppEntity>().Property<byte[]>(TgEfConstants.ColumnRowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfDocumentEntity>().Property<byte[]>(TgEfConstants.ColumnRowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfFilterEntity>().Property<byte[]>(TgEfConstants.ColumnRowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfMessageEntity>().Property<byte[]>(TgEfConstants.ColumnRowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfProxyEntity>().Property<byte[]>(TgEfConstants.ColumnRowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfSourceEntity>().Property<byte[]>(TgEfConstants.ColumnRowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfVersionEntity>().Property<byte[]>(TgEfConstants.ColumnRowVersion).IsRowVersion().HasColumnName(TgEfConstants.ColumnRowVersion);
		//// Ingore
		//modelBuilder.Entity<TgEfAppEntity>().Ignore(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfDocumentEntity>().Ignore(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfFilterEntity>().Ignore(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfMessageEntity>().Ignore(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfProxyEntity>().Ignore(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfSourceEntity>().Ignore(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfVersionEntity>().Ignore(TgEfConstants.ColumnRowVersion);
		
		// FOREIGN KEY: Apps
		modelBuilder.Entity<TgEfAppEntity>()
			.HasOne(app => app.Proxy)
			.WithMany(proxy => proxy.Apps)
			.HasForeignKey(app => app.ProxyUid)
			.HasPrincipalKey(proxy => proxy.Uid);
		// FOREIGN KEY: Documents
		modelBuilder.Entity<TgEfDocumentEntity>()
			.HasOne(document => document.Source)
			.WithMany(source => source.Documents)
			.HasForeignKey(document => document.SourceId)
			.HasPrincipalKey(source => source.Id);
		// FOREIGN KEY: Messages
		modelBuilder.Entity<TgEfMessageEntity>()
			.HasOne(message => message.Source)
			.WithMany(source => source.Messages)
			.HasForeignKey(message => message.SourceId)
			.HasPrincipalKey(source => source.Id);
	}

	public string ToConsoleString(TgEfSourceEntity source) =>
		$"{TgEfUtils.GetPercentCountString(source)} | {(source.IsAutoUpdate ? "a" : " ")} | {source.Id} | " +
		$"{(!string.IsNullOrEmpty(source.UserName) ? TgDataFormatUtils.GetFormatString(source.UserName, 30) : string.Empty)} | " +
		$"{(!string.IsNullOrEmpty(source.Title) ? TgDataFormatUtils.GetFormatString(source.Title, 30) : string.Empty)} | " +
		$"{source.FirstId} {TgLocaleHelper.Instance.From} {source.Count} {TgLocaleHelper.Instance.Messages}";

	public string ToConsoleString(TgEfVersionEntity version) => $"{version.Version}	{version.Description}";

	/// <summary> Check table exists </summary>
	public bool IsTableExists(string tableName)
	{
		string? result = Database
			.SqlQuery<string>($"SELECT [name] AS [Value] FROM [sqlite_master]")
			.SingleOrDefault(x => x == tableName);
		return tableName == result;
	}

	public (bool IsSuccess, string FileName) BackupDb()
	{
		if (File.Exists(TgAppSettings.AppXml.XmlEfStorage))
		{
			DateTime dt = DateTime.Now;
			string fileBackup =
				$"{Path.GetDirectoryName(TgAppSettings.AppXml.XmlEfStorage)}\\" +
				$"{Path.GetFileNameWithoutExtension(TgAppSettings.AppXml.XmlEfStorage)}_{dt:yyyyMMdd}_{dt:HHmmss}.bak";
			File.Copy(TgAppSettings.AppXml.XmlEfStorage, fileBackup);
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
	public async Task<TgEfStorageResult<TEntity>> TruncateTableAsync<TEntity>() where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		CheckIfDisposed();
		string cmd = typeof(TEntity) switch
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
			return new(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new(TgEnumEntityState.NotExecuted);
	}

	/// <summary> Drop sql table </summary>
	public TgEfStorageResult<TEntity> DeleteTable<TEntity>() where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		CheckIfDisposed();
		string cmd = typeof(TEntity) switch
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
			return new(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new(TgEnumEntityState.NotExecuted);
	}

	/// <summary> Drop sql table </summary>
	public async Task<TgEfStorageResult<TEntity>> DeleteTableAsync<TEntity>() where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		CheckIfDisposed();
		string cmd = typeof(TEntity) switch
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
			return new(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new(TgEnumEntityState.NotExecuted);
	}

	public TgEfStorageResult<TEntity> AlterTableNoCaseUid<TEntity>() where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		CheckIfDisposed();
		string cmd = typeof(TEntity) switch
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
		if (string.IsNullOrEmpty(cmd))
			return new(TgEnumEntityState.NotExecuted);
		int result = Database.ExecuteSqlRaw(cmd);
		return new(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
	}

	public async Task<TgEfStorageResult<TEntity>> AlterTableNoCaseUidAsync<TEntity>() where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		CheckIfDisposed();
		string cmd = typeof(TEntity) switch
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
		if (string.IsNullOrEmpty(cmd))
			return new(TgEnumEntityState.NotExecuted);
		int result = await Database.ExecuteSqlRawAsync(cmd);
		return new(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
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
	public TgEfStorageResult<TEntity> UpdateTableUidUpperCaseAll<TEntity>() where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		CheckIfDisposed();
		string cmd = typeof(TEntity) switch
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
			return new(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new(TgEnumEntityState.NotExecuted);
	}

	/// <summary> Update UID field to upper case </summary>
	public async Task<TgEfStorageResult<TEntity>> UpdateTableUidUpperCaseAllAsync<TEntity>() where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		CheckIfDisposed();
		string cmd = typeof(TEntity) switch
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
			return new(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new(TgEnumEntityState.NotExecuted);
	}

	/// <summary> Update UID field to upper case </summary>
	/// <param name="uid"> PK UID </param>
	public async Task<TgEfStorageResult<TEntity>> UpdateTableUidUpperCaseAsync<TEntity>(Guid uid) where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		CheckIfDisposed();
		string uidUpper = uid.ToString().ToUpper();
		string uidString = uid.ToString();
		string cmd = typeof(TEntity) switch
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
			return new(result > 0 ? TgEnumEntityState.IsExecuted : TgEnumEntityState.NotExecuted);
		}
		return new(TgEnumEntityState.NotExecuted);
	}

	/// <summary> Create and update storage </summary>
	public void CreateAndUpdateDb()
	{
		CheckIfDisposed();
		Database.Migrate();
	}

	/// <summary> Create and update storage </summary>
	public async Task CreateAndUpdateDbAsync()
	{
		CheckIfDisposed();
		await Database.MigrateAsync();
	}

	#endregion
}