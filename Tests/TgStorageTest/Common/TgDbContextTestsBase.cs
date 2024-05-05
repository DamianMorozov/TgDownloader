// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Common;

internal abstract class TgDbContextTestsBase
{
    #region Public and private fields, properties, constructor

    protected object Locker { get; } = new();

    protected TgEfContext CreateEfContext()
	{
		LoggerFactory factory = new();
		DbContextOptionsBuilder<TgEfContext> builderDbProd = new DbContextOptionsBuilder<TgEfContext>()
			.UseLoggerFactory(factory)
			.UseSqlite($"{TgLocalization.Helpers.TgLocaleHelper.Instance.SqliteDataSource}={TgAppSettingsHelper.Instance.AppXml.FileStorage}");
		TgEfContext efContext = new(builderDbProd.Options);
		TestContext.WriteLine(efContext.Database.GetConnectionString());
		return efContext;
	}

	protected TgEfContext CreateEfContextMemory()
	{
		LoggerFactory factory = new();
		DbContextOptionsBuilder<TgEfContext> builderMemory = new DbContextOptionsBuilder<TgEfContext>()
			.UseLoggerFactory(factory)
			.UseSqlite("DataSource=:memory:");
		TgEfContext efContext = new(builderMemory.Options);
		TestContext.WriteLine(efContext.Database.GetConnectionString());
		return efContext;
	}

	#endregion
}