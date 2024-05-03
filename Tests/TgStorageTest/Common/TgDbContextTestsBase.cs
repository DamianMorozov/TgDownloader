// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Common;

internal abstract class TgDbContextTestsBase
{
    #region Public and private fields, properties, constructor

	/// <summary> Product EF DB context </summary>
	protected TgEfContext EfProdContext { get; }

	protected TgDbContextTestsBase()
    {
        LoggerFactory factory = new();
        
        //// Memory EF DB context.
        //DbContextOptionsBuilder<TgEfContext> builderMemory = new DbContextOptionsBuilder<TgEfContext>()
        //    .UseLoggerFactory(factory)
        //    .UseSqlite("DataSource=:memory:");
        //EfMemoryContext = new(builderMemory.Options);
        //TestContext.WriteLine(EfMemoryContext.Database.GetConnectionString());
        
        // Product EF DB Context.
		DbContextOptionsBuilder<TgEfContext> builderDbProd = new DbContextOptionsBuilder<TgEfContext>()
	        .UseLoggerFactory(factory)
	        .UseSqlite($"{TgLocalization.Helpers.TgLocaleHelper.Instance.SqliteDataSource}={TgAppSettingsHelper.Instance.AppXml.FileStorage}");
        EfProdContext = new(builderDbProd.Options);
        TestContext.WriteLine(EfProdContext.Database.GetConnectionString());
	}

	~TgDbContextTestsBase()
    {
	    //EfMemoryContext.Dispose();
        EfProdContext.Dispose();
    }

    #endregion
}