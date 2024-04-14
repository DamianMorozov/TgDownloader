// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal class TgEfRepositoryGetCountTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void GetCount<TEntity>(ITgEfRepository<TEntity> repo) where TEntity : TgEfEntityBase, new()
	{
		Assert.DoesNotThrow(() =>
		{
			int count = repo.GetCount();
			TestContext.WriteLine($"Found {count} items.");
		});
	}

	[Test]
	public void TgEf_get_count_app() => GetCount(EfProdContext.AppRepository);

	[Test]
	public void TgEf_get_count_document() => GetCount(EfProdContext.DocumentRepository);

	[Test]
	public void TgEf_get_count_filter() => GetCount(EfProdContext.FilterRepository);

	[Test]
	public void TgEf_get_count_message() => GetCount(EfProdContext.MessageRepository);

	[Test]
	public void TgEf_get_count_proxy() => GetCount(EfProdContext.ProxyRepository);

	[Test]
	public void TgEf_get_count_source() => GetCount(EfProdContext.SourceRepository);

	[Test]
	public void TgEf_get_count_version() => GetCount(EfProdContext.VersionRepository);

	#endregion
}