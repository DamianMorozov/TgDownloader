// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Common;

internal class TgDbContextTestsBase
{
    #region Public and private fields, properties, constructor

    /// <summary> Memory EF DB context </summary>
    protected TgEfContext EfMemoryContext { get; }
    /// <summary> Test EF DB context </summary>
    protected TgEfContext EfTestContext { get; }
	/// <summary> Product EF DB context </summary>
	protected TgEfContext EfProdContext { get; }
	/// <summary> Test XPO DB context </summary>
	protected TgXpoContext XpoTestContext { get; }
	/// <summary> Product XPO DB context </summary>
	protected TgXpoContext XpoProdContext { get; }

	public TgDbContextTestsBase()
    {
        LoggerFactory factory = new();
        
        // Memory EF DB context.
        DbContextOptionsBuilder<TgEfContext> builderMemory = new DbContextOptionsBuilder<TgEfContext>()
            .UseLoggerFactory(factory)
            .UseSqlite("DataSource=:memory:");
        EfMemoryContext = new(builderMemory.Options);
        TestContext.WriteLine(EfMemoryContext.Database.GetConnectionString());
        // Test EF DB Context.
        DbContextOptionsBuilder<TgEfContext> builderDbTest = new DbContextOptionsBuilder<TgEfContext>()
	        .UseLoggerFactory(factory)
	        .UseSqlite($"{TgLocalization.Helpers.TgLocaleHelper.Instance.SqliteDataSource}={TgAppSettingsHelper.Instance.AppXml.TestStorage}");
        EfTestContext = new(builderDbTest.Options);
        TestContext.WriteLine(EfTestContext.Database.GetConnectionString());
		// Product EF DB Context.
		DbContextOptionsBuilder<TgEfContext> builderDbProd = new DbContextOptionsBuilder<TgEfContext>()
	        .UseLoggerFactory(factory)
	        .UseSqlite($"{TgLocalization.Helpers.TgLocaleHelper.Instance.SqliteDataSource}={TgAppSettingsHelper.Instance.AppXml.FileStorage}");
        EfProdContext = new(builderDbProd.Options);
        TestContext.WriteLine(EfProdContext.Database.GetConnectionString());
        
        // Test DB Context.
        XpoTestContext = new(TgEnumStorageType.Test);
        TestContext.WriteLine(XpoTestContext.ConnectionStringLowerCase);
        TestContext.WriteLine(XpoTestContext.ConnectionStringUpperCase);
		// Product DB Context.
		XpoProdContext = new(TgEnumStorageType.Prod);
		TestContext.WriteLine(XpoProdContext.ConnectionStringLowerCase);
		TestContext.WriteLine(XpoProdContext.ConnectionStringUpperCase);
	}

	~TgDbContextTestsBase()
    {
        EfProdContext.Dispose();
        EfTestContext.Dispose();
        EfMemoryContext.Dispose();
    }

    #endregion
}