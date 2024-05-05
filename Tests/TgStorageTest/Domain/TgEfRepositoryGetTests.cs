// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal sealed class TgEfRepositoryGetTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void GetList<T>(ITgEfRepository<T> repo, TgEnumTableTopRecords count = TgEnumTableTopRecords.Top20) 
		where T : TgEfEntityBase, new()
	{
		Assert.DoesNotThrow(() =>
		{
			TgEfOperResult<T> operResult = repo.GetList(count, isNoTracking: true);
			TestContext.WriteLine($"Found {operResult.Items.Count()} items.");
			foreach (T item in operResult.Items)
			{
				T itemFind = repo.Get(item, isNoTracking: true).Item;
				Assert.That(itemFind, Is.Not.Null);
                TestContext.WriteLine(itemFind.ToDebugString());
			}
		});
	}

	private void GetListAsync<T>(ITgEfRepository<T> repo, TgEnumTableTopRecords count = TgEnumTableTopRecords.Top20) 
		where T : TgEfEntityBase, new()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<T> operResult = await repo.GetListAsync(count, isNoTracking: true);
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
	public void Get_apps() => GetList(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_apps_async() => GetListAsync(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_documents() => GetList(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_documents_async() => GetListAsync(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_filters() => GetList(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_filters_async() => GetListAsync(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_messages() => GetList(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_messages_async() => GetListAsync(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_proxies() => GetList(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_proxies_async() => GetListAsync(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_sources() => GetList(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_sources_async() => GetListAsync(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_versions() => GetList(new TgEfVersionRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_versions_async() => GetListAsync(new TgEfVersionRepository(TgEfUtils.EfContext));

	#endregion
}