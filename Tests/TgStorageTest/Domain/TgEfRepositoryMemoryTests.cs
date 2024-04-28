// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal class TgEfRepositoryMemoryTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void CreateEntity<T>(ITgEfRepository<T> repo) where T : TgEfEntityBase, new()
	{
		Assert.DoesNotThrow(() =>
		{
			TgEfOperResult<T> operResult = repo.CreateNew();
			TgEfOperResult<T> operResultFind = repo.Get(operResult.Item, isNoTracking: false);

			Assert.That(operResultFind.IsExists);
			TestContext.WriteLine(operResultFind.Item.ToDebugString());
		});
	}

	//[Test]
	//public void TgEf_create_app() => CreateEntity(MemoryContext.AppsRepo);

	//[Test]
	//public void TgEf_create_document() => CreateEntity(MemoryContext.DocumentRepo);

	//[Test]
	//public void TgEf_create_filter() => CreateEntity(MemoryContext.FilterRepo);

	//[Test]
	//public void TgEf_create_message() => CreateEntity(MemoryContext.MessageRepo);

	//[Test]
	//public void TgEf_create_proxy() => CreateEntity(MemoryContext.ProxyRepo);

	//[Test]
	//public void TgEf_create_proxy() => CreateEntity(MemoryContext.SourceRepo);

	//[Test]
	//public void TgEf_create_version() => CreateEntity(MemoryContext.VersionRepo);

	#endregion
}