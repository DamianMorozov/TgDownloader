// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal class TgEfRepositoryGetTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void GetEnumerable<T>(ITgEfRepository<T> repo, TgEnumTableTopRecords count = TgEnumTableTopRecords.Top20) 
		where T : TgEfEntityBase, new()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<T> operResult = await repo.GetEnumerableAsync(count, isNoTracking: true);
			TestContext.WriteLine($"Found {operResult.Items.Count()} items.");
			foreach (T item in operResult.Items)
			{
				T itemFind = (await repo.GetAsync(item, isNoTracking: true)).Item;
				Assert.That(itemFind, Is.Not.Null);
                TestContext.WriteLine(itemFind.ToDebugString());
			}
		});
	}

	[Test]
	public void Get_apps() => GetEnumerable(EfProdContext.AppRepository);

	[Test]
	public void Get_documents() => GetEnumerable(EfProdContext.DocumentRepository);

	[Test]
	public void Get_filters() => GetEnumerable(EfProdContext.FilterRepository);

	[Test]
	public void Get_messages() => GetEnumerable(EfProdContext.MessageRepository);

	[Test]
	public void Get_proxies() => GetEnumerable(EfProdContext.ProxyRepository);

	[Test]
	public void Get_sources() => GetEnumerable(EfProdContext.SourceRepository);

	[Test]
	public void Get_versions() => GetEnumerable(EfProdContext.VersionRepository);

	#endregion
}