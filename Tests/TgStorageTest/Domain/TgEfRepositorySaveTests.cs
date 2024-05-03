// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal sealed class TgEfRepositorySaveTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void SaveItem<T>(ITgEfRepository<T> repo) 
		where T : TgEfEntityBase, new()
	{
		Assert.DoesNotThrow(() =>
		{
			TgEfOperResult<T> operResult = repo.GetFirst(isNoTracking: true);
			if (operResult.IsExists)
			{
				T itemFind = repo.Get(operResult.Item, isNoTracking: true).Item;
				Assert.That(itemFind, Is.Not.Null);
				TestContext.WriteLine(itemFind.ToDebugString());
				// Save
				operResult = repo.Save(itemFind);
				Assert.That(operResult.IsExists);
			}
		});
	}

	private void SaveItemAsync<T>(ITgEfRepository<T> repo) 
		where T : TgEfEntityBase, new()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<T> operResult = await repo.GetFirstAsync(isNoTracking: true);
			if (operResult.IsExists)
			{
				T itemFind = (await repo.GetAsync(operResult.Item, isNoTracking: true)).Item;
				Assert.That(itemFind, Is.Not.Null);
				TestContext.WriteLine(itemFind.ToDebugString());
				// Save
				operResult = await repo.SaveAsync(itemFind);
				Assert.That(operResult.IsExists);
			}
		});
	}

	[Test]
	public void Save_apps() => SaveItem(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void Save_apps_async() => SaveItemAsync(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void Save_documents() => SaveItem(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void Save_documents_async() => SaveItemAsync(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void Save_filters() => SaveItem(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void Save_filters_async() => SaveItemAsync(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void Save_messages() => SaveItem(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void Save_messages_async() => SaveItemAsync(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void Save_proxies() => SaveItem(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void Save_proxies_async() => SaveItemAsync(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void Save_sources() => SaveItem(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void Save_sources_async() => SaveItemAsync(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void Save_versions() => SaveItem(new TgEfVersionRepository(TgEfUtils.EfContext));

	[Test]
	public void Save_versions_async() => SaveItemAsync(new TgEfVersionRepository(TgEfUtils.EfContext));

	#endregion
}