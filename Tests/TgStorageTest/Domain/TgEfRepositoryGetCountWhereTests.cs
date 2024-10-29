// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal sealed class TgEfRepositoryGetCountWhereTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void GetCountWhere<TEntity>(ITgEfRepository<TEntity> repo) where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		Assert.DoesNotThrow(() =>
		{
			int count = repo.GetCount(TgEfUtils.WhereUidNotEmpty<TEntity>());
			TestContext.WriteLine($"Found {count} items.");
		});
	}

	private void GetCountWhereAsync<TEntity>(ITgEfRepository<TEntity> repo) where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			int count = await repo.GetCountAsync(TgEfUtils.WhereUidNotEmpty<TEntity>());
			TestContext.WriteLine($"Found {count} items.");
		});
	}

	[Test]
	public void TgEf_get_count_app() => GetCountWhere(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_app_async() => GetCountWhereAsync(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_document() => GetCountWhere(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_document_async() => GetCountWhereAsync(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_filter() => GetCountWhere(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_filter_async() => GetCountWhereAsync(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_message() => GetCountWhere(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_message_async() => GetCountWhereAsync(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_proxy() => GetCountWhere(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_proxy_async() => GetCountWhereAsync(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_source() => GetCountWhere(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_source_async() => GetCountWhereAsync(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_version() => GetCountWhere(new TgEfVersionRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_count_version_async() => GetCountWhereAsync(new TgEfVersionRepository(TgEfUtils.EfContext));

	#endregion
}