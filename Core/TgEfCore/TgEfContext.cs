// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore;

public class TgEfContext : DbContext
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
	public ITgEfRepository<TgEfAppEntity> AppsRepo { get; set; } = default!;
	/// <summary> Document repository </summary>
	public ITgEfRepository<TgEfDocumentEntity> DocuemntRepo { get; set; } = default!;
	/// <summary> Filter repository </summary>
	public ITgEfRepository<TgEfFilterEntity> FilterRepo { get; set; } = default!;
	/// <summary> Message repository </summary>
	public ITgEfRepository<TgEfMessageEntity> MessageRepo { get; set; } = default!;
	/// <summary> Proxy repository </summary>
	public ITgEfRepository<TgEfProxyEntity> ProxyRepo { get; set; } = default!;
	/// <summary> Source repository </summary>
	public ITgEfRepository<TgEfSourceEntity> SourceRepo { get; set; } = default!;
	/// <summary> Version repository </summary>
	public ITgEfRepository<TgEfVersionEntity> VersionRepo { get; set; } = default!;

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

	#region Public and private methods

	private void InitRepositories()
	{
#if DEBUG
		Debug.WriteLine($"{nameof(ContextId)} {ContextId}: init repositories");
#endif
        AppsRepo = new TgEfAppRepository(this);
        DocuemntRepo = new TgEfDocumentRepository(this);
        FilterRepo = new TgEfFilterRepository(this);
        MessageRepo = new TgEfMessageRepository(this);
		ProxyRepo = new TgEfProxyRepository(this);
        SourceRepo = new TgEfSourceRepository(this);
        VersionRepo = new TgEfVersionRepository(this);
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
	//[NotMapped]
	//public static readonly string RowVersion = nameof(RowVersion);
	//// Define the model.
	//protected override void OnModelCreating(ModelBuilder modelBuilder)
	//{
 //       // This property isn't on the C# class, so we set it up as a "shadow" property and use it for concurrency.
 //       modelBuilder.Entity<TgEfAppEntity>().Property<byte[]>(RowVersion).IsRowVersion();
 //       modelBuilder.Entity<TgEfFilterEntity>().Property<byte[]>(RowVersion).IsRowVersion();
 //       modelBuilder.Entity<TgEfProxyEntity>().Property<byte[]>(RowVersion).IsRowVersion();
 //       modelBuilder.Entity<TgEfVersionEntity>().Property<byte[]>(RowVersion).IsRowVersion();

 //       base.OnModelCreating(modelBuilder);
	//}

	// Dispose pattern.
	public override void Dispose()
	{
#if DEBUG
		Debug.WriteLine($"{nameof(ContextId)} {ContextId}: context disposed");
#endif
		base.Dispose();
	}

	// Dispose pattern.
	public override ValueTask DisposeAsync()
	{
#if DEBUG
		Debug.WriteLine($"{nameof(ContextId)} {ContextId}: context disposed async");
#endif
		return base.DisposeAsync();
	}

	#endregion
}