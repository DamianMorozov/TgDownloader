// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
#pragma warning disable NUnit1033

using TgInfrastructure.Contracts;
using TgInfrastructure.Enums;

namespace TgStorageTest.Domain;

[TestFixture]
internal sealed class TgEfRepositoryGetListWhereTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void GetListWhereAsync<TEntity>(ITgEfRepository<TEntity> repo, TgEnumTableTopRecords count = TgEnumTableTopRecords.Top20) 
		where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfStorageResult<TEntity> storageResult = await repo.GetListAsync(count, 0, TgEfUtils.WhereUidNotEmpty<TEntity>());
			TestContext.WriteLine($"Found {storageResult.Items.Count()} items.");
			foreach (TEntity item in storageResult.Items)
			{
				TEntity itemFind = await repo.GetItemAsync(item);
				Assert.That(itemFind, Is.Not.Null);
                TestContext.WriteLine(itemFind.ToDebugString());
			}
		});
	}

	[Test]
	public void Get_apps_where_async() => GetListWhereAsync(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_contacts_where_async() => GetListWhereAsync(new TgEfContactRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_documents_where_async() => GetListWhereAsync(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_filters_where_async() => GetListWhereAsync(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_messages_where_async() => GetListWhereAsync(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_proxies_where_async() => GetListWhereAsync(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_sources_where_async() => GetListWhereAsync(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_stories_where_async() => GetListWhereAsync(new TgEfStoryRepository(TgEfUtils.EfContext));

	[Test]
	public void Get_versions_where_async() => GetListWhereAsync(new TgEfVersionRepository(TgEfUtils.EfContext));

	#endregion
}