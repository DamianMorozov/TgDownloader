// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCoreTests;

internal class TgEfContextBase
{
	#region Public and private fields, properties, constructor

	/// <summary> Memory DB context </summary>
    protected TgEfContext MemoryContext { get; set; }
    /// <summary> Product DB context </summary>
    protected TgEfContext DbProdContext { get; set; }
    /// <summary> Test DB context </summary>
    protected TgEfContext DbTestContext { get; set; }

    public TgEfContextBase()
    {
	    LoggerFactory factory = new();

	    // Product DB Context.
		DbContextOptionsBuilder<TgEfContext> builderDbProd = new DbContextOptionsBuilder<TgEfContext>()
		    .UseLoggerFactory(factory)
		    .UseSqlite($"{TgLocalization.Helpers.TgLocaleHelper.Instance.SqliteDataSource}={TgAppSettingsHelper.Instance.AppXml.FileStorage}");
	    DbProdContext = new(builderDbProd.Options);
		TestContext.WriteLine(DbProdContext.Database.GetConnectionString());

		// Test DB Context.
	    DbContextOptionsBuilder<TgEfContext> builderDbTest = new DbContextOptionsBuilder<TgEfContext>()
		    .UseLoggerFactory(factory)
		    .UseSqlite($"{TgLocalization.Helpers.TgLocaleHelper.Instance.SqliteDataSource}={TgAppSettingsHelper.Instance.AppXml.TestStorage}");
	    DbTestContext = new(builderDbTest.Options);
		TestContext.WriteLine(DbTestContext.Database.GetConnectionString());

		// Memory context.
		DbContextOptionsBuilder<TgEfContext> builderMemory = new DbContextOptionsBuilder<TgEfContext>()
			.UseLoggerFactory(factory)
            //.UseInMemoryDatabase(databaseName: "TgDownloaderMemory")
            .UseSqlite("DataSource=:memory:")
            ;
		MemoryContext = new(builderMemory.Options);
        TestContext.WriteLine(MemoryContext.Database.GetConnectionString());
    }

	~TgEfContextBase()
	{
		DbProdContext?.Dispose();
		DbTestContext?.Dispose();
		MemoryContext?.Dispose();
	}

	#endregion
}