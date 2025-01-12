// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
#pragma warning disable NUnit1033

namespace TgStorageTest.Domain;

[TestFixture]
internal sealed class TgEfRepositoryGetFirstTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void GetFirst<TEntity>(ITgEfRepository<TEntity> repo) where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TEntity item = await repo.GetFirstItemAsync();
			TestContext.WriteLine($"Found {item.ToDebugString()}");
		});
	}

	[Test]
	public void TgEf_get_first_app() => GetFirst(new TgEfAppRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_first_bot() => GetFirst(new TgEfBotRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_first_contact() => GetFirst(new TgEfContactRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_first_document() => GetFirst(new TgEfDocumentRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_first_filter() => GetFirst(new TgEfFilterRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_first_message() => GetFirst(new TgEfMessageRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_first_proxy() => GetFirst(new TgEfProxyRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_first_source() => GetFirst(new TgEfSourceRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_first_story() => GetFirst(new TgEfStoryRepository(TgEfUtils.EfContext));

	[Test]
	public void TgEf_get_first_version() => GetFirst(new TgEfVersionRepository(TgEfUtils.EfContext));

	#endregion
}