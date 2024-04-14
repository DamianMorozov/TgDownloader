// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal class TgEfRepositoryGetFirstTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void GetFirst<TEntity>(ITgEfRepository<TEntity> repo) where TEntity : TgEfEntityBase, new()
	{
		Assert.DoesNotThrow(() =>
		{
			TEntity item = repo.GetFirst(isNoTracking: true).Item;
			TestContext.WriteLine($"Found {item.ToDebugString()}");
		});
	}

	[Test]
	public void TgEf_get_first_app() => GetFirst(EfProdContext.AppRepository);

	[Test]
	public void TgEf_get_first_document() => GetFirst(EfProdContext.DocumentRepository);

	[Test]
	public void TgEf_get_first_filter() => GetFirst(EfProdContext.FilterRepository);

	[Test]
	public void TgEf_get_first_message() => GetFirst(EfProdContext.MessageRepository);

	[Test]
	public void TgEf_get_first_proxy() => GetFirst(EfProdContext.ProxyRepository);

	[Test]
	public void TgEf_get_first_source() => GetFirst(EfProdContext.SourceRepository);

	[Test]
	public void TgEf_get_first_version() => GetFirst(EfProdContext.VersionRepository);

	#endregion
}