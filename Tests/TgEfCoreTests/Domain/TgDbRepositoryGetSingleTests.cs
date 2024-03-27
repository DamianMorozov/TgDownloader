// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCoreTests.Domain;

[TestFixture]
internal class TgDbRepositoryGetSingleTests : TgEfContextBase
{
	#region Public and private methods

	private void GetSingle<TEntity>(ITgEfRepository<TEntity> repo, TgSqlEnumTableTopRecords count = TgSqlEnumTableTopRecords.Top20) 
		where TEntity : TgEfEntityBase, new()
	{
		Assert.DoesNotThrow(() =>
		{
			List<TEntity> items = repo.GetEnumerable(count).ToList();
			TestContext.WriteLine($"Found {items.Count} items.");
			
            foreach (TEntity item in items)
			{
				TEntity itemFind = repo.GetSingle(item.Uid);
				Assert.That(itemFind, Is.Not.Null);
				
                TestContext.WriteLine(itemFind.ToDebugString());
			}
		});
	}

	[Test]
	public void TgEf_get_single_app() => GetSingle(DbProdContext.AppsRepo);

	[Test]
	public void TgEf_get_single_document() => GetSingle(DbProdContext.DocuemntRepo);

	[Test]
	public void TgEf_get_single_filter() => GetSingle(DbProdContext.FilterRepo);

	[Test]
	public void TgEf_get_single_message() => GetSingle(DbProdContext.MessageRepo);

	[Test]
	public void TgEf_get_single_proxy() => GetSingle(DbProdContext.ProxyRepo);

	[Test]
	public void TgEf_get_single_source() => GetSingle(DbProdContext.SourceRepo);

	[Test]
	public void TgEf_get_single_version() => GetSingle(DbProdContext.VersionRepo);

	#endregion
}