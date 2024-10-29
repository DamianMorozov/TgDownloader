// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal sealed class TgEfRepositoryGetCountTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void GetCount<TEntity>(ITgEfRepository<TEntity> repo) where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		Assert.DoesNotThrow(() =>
		{
			int count = repo.GetCount();
			TestContext.WriteLine($"Found {count} items.");
		});
	}

	private void GetCountAsync<TEntity>(ITgEfRepository<TEntity> repo) where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			int count = await repo.GetCountAsync();
			TestContext.WriteLine($"Found {count} items.");
		});
	}

	[Test]
	public void TgEf_get_count_app() => GetCount(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_app_async() => GetCountAsync(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_document() => GetCount(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_document_async() => GetCountAsync(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_filter() => GetCount(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_filter_async() => GetCountAsync(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_message() => GetCount(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_message_async() => GetCountAsync(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_proxy() => GetCount(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_proxy_async() => GetCountAsync(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_source() => GetCount(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_source_async() => GetCountAsync(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_version() => GetCount(new TgEfVersionRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_version_async() => GetCountAsync(new TgEfVersionRepository(TgEfUtils.EfContext));

	#endregion
}