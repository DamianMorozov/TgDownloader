// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
#pragma warning disable NUnit1033

namespace TgStorageTest.Domain;

[TestFixture]
internal sealed class TgEfRepositoryGetCountWhereTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void GetCountWhereAsync<TEntity>(ITgEfRepository<TEntity> repo) where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			int count = await repo.GetCountAsync(TgEfUtils.WhereUidNotEmpty<TEntity>());
			TestContext.WriteLine($"Found {count} items.");
		});
	}

	[Test]
	public void TgEf_get_count_apps_async() => GetCountWhereAsync(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_contacts_async() => GetCountWhereAsync(new TgEfContactRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_documents_async() => GetCountWhereAsync(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_filters_async() => GetCountWhereAsync(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_messages_async() => GetCountWhereAsync(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_proxies_async() => GetCountWhereAsync(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_sources_async() => GetCountWhereAsync(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_stories_async() => GetCountWhereAsync(new TgEfStoryRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_versions_async() => GetCountWhereAsync(new TgEfVersionRepository(TgEfUtils.EfContext));

	#endregion
}