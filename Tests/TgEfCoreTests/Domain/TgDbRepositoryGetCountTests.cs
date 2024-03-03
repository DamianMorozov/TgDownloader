// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCoreTests.Domain;

[TestFixture]
internal class TgDbRepositoryGetCountTests : TgEfContextBase
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
	public void TgEf_get_count_app() => GetCount(DbProdContext.AppsRepo);

	[Test]
	public void TgEf_get_count_document() => GetCount(DbProdContext.DocuemntRepo);

	[Test]
	public void TgEf_get_count_filter() => GetCount(DbProdContext.FilterRepo);

	[Test]
	public void TgEf_get_count_message() => GetCount(DbProdContext.MessageRepo);

	[Test]
	public void TgEf_get_count_proxy() => GetCount(DbProdContext.ProxyRepo);

	[Test]
	public void TgEf_get_count_source() => GetCount(DbProdContext.SourceRepo);

	[Test]
	public void TgEf_get_count_version() => GetCount(DbProdContext.VersionRepo);

	#endregion
}