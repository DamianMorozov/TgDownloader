// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCoreTests.Domain;

[TestFixture]
internal class TgDbRepositoryGetFirstTests : TgEfContextBase
{
	#region Public and private methods

	private void GetFirst<TEntity>(ITgEfRepository<TEntity> repo) where TEntity : TgEfEntityBase, new()
	{
		Assert.DoesNotThrow(() =>
		{
			TEntity item = repo.GetFirst();
			TestContext.WriteLine($"Found {item.ToDebugString()}");
		});
	}

	[Test]
	public void TgEf_get_first_app() => GetFirst(DbProdContext.AppsRepo);

	[Test]
	public void TgEf_get_first_document() => GetFirst(DbProdContext.DocuemntRepo);

	[Test]
	public void TgEf_get_first_filter() => GetFirst(DbProdContext.FilterRepo);

	[Test]
	public void TgEf_get_first_message() => GetFirst(DbProdContext.MessageRepo);

	[Test]
	public void TgEf_get_first_proxy() => GetFirst(DbProdContext.ProxyRepo);

	[Test]
	public void TgEf_get_first_source() => GetFirst(DbProdContext.SourceRepo);

	[Test]
	public void TgEf_get_first_version() => GetFirst(DbProdContext.VersionRepo);

	#endregion
}