// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
#pragma warning disable S2589

namespace TgStorage.Domain;

/// <summary> DB context </summary>
public sealed class TgEfContext : DbContext
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

    #endregion

    #region Public and private methods

    public TgEfContext()
    {
#if DEBUG
        Debug.WriteLine($"Created TgEfContext with {nameof(ContextId)} {ContextId}", TgConstants.LogTypeStorage);
#endif
    }

    /// <summary> Inject options </summary>
    // For using: services.AddDbContextFactory<TgEfContext>
    public TgEfContext(DbContextOptions<TgEfContext> options) : base(options)
    {
#if DEBUG
        Debug.WriteLine($"Created TgEfContext with {nameof(ContextId)} {ContextId}", TgConstants.LogTypeStorage);
#endif
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        LoggerFactory factory = new();
        var storagePath = string.Empty;
		// Console app
		if (TgAsyncUtils.AppType == TgEnumAppType.Default || TgAsyncUtils.AppType == TgEnumAppType.Console)
        {
	        if (string.IsNullOrEmpty(TgAppSettingsHelper.Instance.AppXml.XmlEfStorage))
		        TgAppSettingsHelper.Instance.AppXml.XmlEfStorage = TgEfUtils.FileEfStorage;
	        storagePath = $"{TgLocaleHelper.Instance.SqliteDataSource}={TgAppSettingsHelper.Instance.AppXml.XmlEfStorage}";
        }
		// Desktop app
		if (TgAsyncUtils.AppType == TgEnumAppType.Desktop)
		{
            if (!string.IsNullOrEmpty(TgEfUtils.AppStorage))
				storagePath = $"{TgLocaleHelper.Instance.SqliteDataSource}={TgEfUtils.AppStorage}";
        }
		if (string.IsNullOrEmpty(storagePath))
	        throw new ArgumentException(nameof(storagePath));
        optionsBuilder
#if DEBUG
            .LogTo(message => Debug.WriteLine($"{nameof(ContextId)} {ContextId}: {message}", TgConstants.LogTypeStorage), LogLevel.Debug)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
#endif
            .EnableThreadSafetyChecks()
            .UseLoggerFactory(factory)
            .UseSqlite(storagePath)
        ;
#if DEBUG
        Debug.WriteLine(storagePath, TgConstants.LogTypeStorage);
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
		//// Ignore
		//modelBuilder.Entity<TgEfAppEntity>().Ignore(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfDocumentEntity>().Ignore(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfFilterEntity>().Ignore(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfMessageEntity>().Ignore(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfProxyEntity>().Ignore(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfSourceEntity>().Ignore(TgEfConstants.ColumnRowVersion);
		//modelBuilder.Entity<TgEfVersionEntity>().Ignore(TgEfConstants.ColumnRowVersion);

		// APPS
		modelBuilder.Entity<TgEfAppEntity>(entity =>
		{
			entity.ToTable(TgEfConstants.TableApps);
		});
		modelBuilder.Entity<TgEfAppEntity>()
			.HasOne(app => app.Proxy)
			.WithMany(proxy => proxy.Apps)
			.HasForeignKey(app => app.ProxyUid)
			.HasPrincipalKey(proxy => proxy.Uid);
        
		// DOCUMENTS
		modelBuilder.Entity<TgEfDocumentEntity>(entity =>
		{
			entity.ToTable(TgEfConstants.TableDocuments);
		});
		modelBuilder.Entity<TgEfDocumentEntity>()
			.HasOne(document => document.Source)
			.WithMany(source => source.Documents)
			.HasForeignKey(document => document.SourceId)
			.HasPrincipalKey(source => source.Id);
        
		// FILTERS
		modelBuilder.Entity<TgEfFilterEntity>(entity =>
		{
			entity.ToTable(TgEfConstants.TableFilters);
		});
        
		// MESSAGES
		modelBuilder.Entity<TgEfMessageEntity>(entity =>
		{
			entity.ToTable(TgEfConstants.TableMessages);
		});
		modelBuilder.Entity<TgEfMessageEntity>()
			.HasOne(message => message.Source)
			.WithMany(source => source.Messages)
			.HasForeignKey(message => message.SourceId)
			.HasPrincipalKey(source => source.Id);
        
		// PROXIES
		modelBuilder.Entity<TgEfProxyEntity>(entity =>
		{
			entity.ToTable(TgEfConstants.TableProxies);
		});
        
		// SOURCES
		modelBuilder.Entity<TgEfSourceEntity>(entity =>
		{
			entity.ToTable(TgEfConstants.TableSources);
		});
        
		// VERSIONS
		modelBuilder.Entity<TgEfVersionEntity>(entity =>
		{
			entity.ToTable(TgEfConstants.TableVersions);
		});
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
        var result = Database
            .SqlQuery<string>($"SELECT [name] AS [Value] FROM [sqlite_master]")
            .SingleOrDefault(x => x == tableName);
        return tableName == result;
    }

    public (bool IsSuccess, string FileName) BackupDb()
    {
        if (File.Exists(TgAppSettings.AppXml.XmlEfStorage))
        {
            var dt = DateTime.Now;
            var fileBackup =
                $"{Path.GetDirectoryName(TgAppSettings.AppXml.XmlEfStorage)}\\" +
                $"{Path.GetFileNameWithoutExtension(TgAppSettings.AppXml.XmlEfStorage)}_{dt:yyyyMMdd}_{dt:HHmmss}.bak";
            File.Copy(TgAppSettings.AppXml.XmlEfStorage, fileBackup);
            return (File.Exists(fileBackup), fileBackup);
        }
        return (false, string.Empty);
    }

	/// <summary> Shrink storage </summary>
	public async Task CompactDbAsync() => await Database.ExecuteSqlRawAsync("VACUUM;");

    /// <summary> Create and update storage </summary>
    public async Task CreateAndUpdateDbAsync() => await Database.MigrateAsync();

    #endregion
}
