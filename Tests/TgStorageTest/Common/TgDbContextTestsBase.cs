// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
#pragma warning disable NUnit1033

namespace TgStorageTest.Common;

internal abstract class TgDbContextTestsBase
{
	#region Public and private fields, properties, constructor

	protected TgDbContextTestsBase()
	{
		TgEfUtils.CreateAndUpdateDbAsync().GetAwaiter().GetResult();
	}

	#endregion

	#region Public and private methods

	protected static TgEfContext CreateEfContext()
	{
		LoggerFactory factory = new();
		DbContextOptionsBuilder<TgEfContext> dbBuilder = new DbContextOptionsBuilder<TgEfContext>()
			.UseLoggerFactory(factory)
			.UseSqlite($"{TgInfrastructure.Helpers.TgLocaleHelper.Instance.SqliteDataSource}={TgAppSettingsHelper.Instance.AppXml.XmlEfStorage}");
		TgEfContext efContext = new(dbBuilder.Options);
		TgEfUtils.CreateAndUpdateDbAsync().GetAwaiter().GetResult();
		TestContext.WriteLine(efContext.Database.GetConnectionString());
		return efContext;
	}

	protected static TgEfContext CreateEfContextMemory()
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