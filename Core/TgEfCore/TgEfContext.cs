// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgEfCore.Domain.Filters;

namespace TgEfCore;

public class TgEfContext : DbContext
{
	#region Public and private fields, properties, constructor

	/// <summary> App queries </summary>
	public DbSet<TgEfAppEntity> Apps { get; set; }
	/// <summary> Filter queries </summary>
	public DbSet<TgEfFilterEntity> Filters { get; set; }
	/// <summary> Proxy queries </summary>
	public DbSet<TgEfProxyEntity> Proxies { get; set; }

	/// <summary> App repository </summary>
	public TgEfAppRepository AppsRepo { get; set; }
    /// <summary> Filter repository </summary>
    public TgEfFilterRepository FilterRepo { get; set; }
	/// <summary> Proxy repository </summary>
	public TgEfProxyRepository ProxyRepo { get; set; }

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
		AppsRepo = new(this);
		FilterRepo = new(this);
		ProxyRepo = new(this);
	}

#if DEBUG
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
		optionsBuilder
			//.UseSqlite($"Data Source={TgAppSettings.AppXml.FileStorage}")
			.LogTo(message => Debug.WriteLine($"{nameof(ContextId)} {ContextId}: {message}"), LogLevel.Information)
	        .EnableThreadSafetyChecks()
	        .EnableDetailedErrors()
	        .EnableSensitiveDataLogging()
		;
	}
#endif

	// Magic string.
	//public static readonly string RowVersion = nameof(RowVersion);
	// Define the model.
	//protected override void OnModelCreating(ModelBuilder modelBuilder)
	//{
	//	// This property isn't on the C# class,
	//	// so we set it up as a "shadow" property and use it for concurrency.
	//	modelBuilder.Entity<TgEfTableProxy>()
	//		.Property<byte[]>(RowVersion)
	//		.IsRowVersion();

	//	base.OnModelCreating(modelBuilder);
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