// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Utils;

public static class TgDatabaseUtility
{
    // Method to see the database. Should not be used in production: demo purposes only.
    // options: The configured options.
    // count: The number of contacts to seed.
    public static async Task EnsureDbCreatedAndSeedWithCountOfAsync(DbContextOptions<TgEfContext> options, int count)
    {
#if DEBUG
        Console.WriteLine($"EnsureDbCreatedAndSeedWithCountOfAsync");
#endif
        // Empty to avoid logging while inserting (otherwise will flood console).
        LoggerFactory factory = new();
        DbContextOptionsBuilder<TgEfContext> builder = new DbContextOptionsBuilder<TgEfContext>()
            .UseLoggerFactory(factory)
            .UseSqlite($"{TgLocaleHelper.Instance.SqliteDataSource}={TgAppSettingsHelper.Instance.AppXml.FileStorage}");
        await using var dbContext = new TgEfContext(builder.Options);
#if DEBUG
        Console.WriteLine(dbContext.Database.GetConnectionString());
#endif

        // Result is true if the database had to be created.
        //if (await dbContext.Database.EnsureCreatedAsync())
        //{
        //	//var seed = new SeedContacts();
        //	//await seed.SeedDatabaseWithContactCountOfAsync(context, count);
        //	List<TgEfProxyEntity> proxies = dbContext.Proxies.Select(x => x).Take(1).ToList();
        //}
    }
}