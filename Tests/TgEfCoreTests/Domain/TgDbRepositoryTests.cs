// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCoreTests.Domain;

[TestFixture]
internal class TgDbRepositoryTests : TgEfContextBase
{
	#region Public and private methods

	private void GetSingle<TEntity>(ITgEfRepository<TEntity> repo) where TEntity : TgEfEntityBase, new()
	{
		Assert.DoesNotThrow(() =>
		{
			List<TEntity> items = repo.GetEnumerable().ToList();
			TestContext.WriteLine($"Found {items.Count} items.");
			
            foreach (TEntity item in items)
			{
				TEntity itemFind = repo.GetSingle(item.Uid);
				Assert.IsNotNull(itemFind);
				
                TestContext.WriteLine(itemFind.ToDebugString());
			}
		});
	}

	[Test]
	public void TgEf_get_app() => GetSingle(DbProdContext.AppsRepo);

	[Test]
	public void TgEf_get_filter() => GetSingle(DbProdContext.FilterRepo);

	[Test]
	public void TgEf_get_proxy() => GetSingle(DbProdContext.ProxyRepo);

	#endregion
}