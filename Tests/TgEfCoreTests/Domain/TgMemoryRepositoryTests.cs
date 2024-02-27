// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCoreTests.Domain;

[TestFixture]
internal class TgMemoryRepositoryTests : TgEfContextBase
{
	#region Public and private methods

	private void CreateEntity<TEntity>(ITgEfRepository<TEntity> repo) where TEntity : TgEfEntityBase, new()
	{
		Assert.DoesNotThrow(() =>
		{
			TEntity item = repo.CreateNew();
            TEntity itemFind = repo.GetSingle(item.Uid);

			Assert.IsNotNull(itemFind);
			TestContext.WriteLine(itemFind.ToDebugString());
		});
	}

	[Test]
	public void TgEf_create_app() => CreateEntity(MemoryContext.AppsRepo);

	[Test]
	public void TgEf_create_filter() => CreateEntity(MemoryContext.FilterRepo);

	[Test]
	public void TgEf_create_proxy() => CreateEntity(MemoryContext.ProxyRepo);

	#endregion
}