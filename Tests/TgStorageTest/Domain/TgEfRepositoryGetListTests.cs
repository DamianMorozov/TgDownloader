// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
#pragma warning disable NUnit1033

namespace TgStorageTest.Domain;

[TestFixture]
internal sealed class TgEfRepositoryGetListTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void GetListAsync<TEntity>(ITgEfRepository<TEntity> repo, TgEnumTableTopRecords count = TgEnumTableTopRecords.Top20) 
		where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfStorageResult<TEntity> storageResult = await repo.GetListAsync(count, 0);
			TestContext.WriteLine($"Found {storageResult.Items.Count()} items.");
			foreach (TEntity item in storageResult.Items)
			{
				TEntity itemFind = (await repo.GetAsync(item)).Item;
				Assert.That(itemFind, Is.Not.Null);
                TestContext.WriteLine(itemFind.ToDebugString());
			}
		});
	}

	[Test]
	public void Get_apps_async() => GetListAsync(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_contacts_async() => GetListAsync(new TgEfContactRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_documents_async() => GetListAsync(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_filters_async() => GetListAsync(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_messages_async() => GetListAsync(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_proxies_async() => GetListAsync(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_sources_async() => GetListAsync(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_stories_async() => GetListAsync(new TgEfStoryRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_versions_async() => GetListAsync(new TgEfVersionRepository(TgEfUtils.EfContext));

	#endregion
}