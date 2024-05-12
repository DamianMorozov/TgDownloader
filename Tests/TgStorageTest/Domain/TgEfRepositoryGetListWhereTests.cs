// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal sealed class TgEfRepositoryGetListWhereTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void GetListWhere<T>(ITgEfRepository<T> repo, TgEnumTableTopRecords count = TgEnumTableTopRecords.Top20) 
		where T : TgEfEntityBase, new()
	{
		Assert.DoesNotThrow(() =>
		{
			TgEfOperResult<T> operResult = repo.GetList(count, 0, TgEfUtils.WhereUidNotEmpty<T>(), isNoTracking: true);
			TestContext.WriteLine($"Found {operResult.Items.Count()} items.");
			foreach (T item in operResult.Items)
			{
				T itemFind = repo.Get(item, isNoTracking: true).Item;
				Assert.That(itemFind, Is.Not.Null);
                TestContext.WriteLine(itemFind.ToDebugString());
			}
		});
	}

	private void GetListWhereAsync<T>(ITgEfRepository<T> repo, TgEnumTableTopRecords count = TgEnumTableTopRecords.Top20) 
		where T : TgEfEntityBase, new()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<T> operResult = await repo.GetListAsync(count, 0, TgEfUtils.WhereUidNotEmpty<T>(), isNoTracking: true);
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
	public void Get_apps_where() => GetListWhere(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_apps_where_async() => GetListWhereAsync(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_documents_where() => GetListWhere(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_documents_where_async() => GetListWhereAsync(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_filters_where() => GetListWhere(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_filters_where_async() => GetListWhereAsync(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_messages_where() => GetListWhere(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_messages_where_async() => GetListWhereAsync(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_proxies_where() => GetListWhere(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_proxies_where_async() => GetListWhereAsync(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_sources_where() => GetListWhere(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_sources_where_async() => GetListWhereAsync(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_versions_where() => GetListWhere(new TgEfVersionRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_versions_where_async() => GetListWhereAsync(new TgEfVersionRepository(TgEfUtils.EfContext));

	#endregion
}