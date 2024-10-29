// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal sealed class TgEfSourceRepositorySaveTests : TgDbContextTestsBase
{
	#region Public and private methods

	[Test]
	public void Save_sources()
	{
		Assert.DoesNotThrow(() =>
		{
			TgEfSourceRepository repo = new(TgEfUtils.EfContext);
			IEnumerable<TgEfSourceEntity> itemsFind = repo.GetList(TgEnumTableTopRecords.All, 0, isNoTracking: true).Items;
			foreach (TgEfSourceEntity item in itemsFind)
			{
				TgEfSourceEntity itemCopy = new();
				itemCopy.Id = item.Id;
				TgEfStorageResult<TgEfSourceEntity> storageResult = repo.Get(itemCopy, isNoTracking: false);
				TestContext.WriteLine(item.ToDebugString());
				// Save
				//storageResult = repo.Save(item);
				//Assert.That(storageResult.IsExists);
			}
		});
	}

	[Test]
	public void Save_sources_async()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfSourceRepository repo = new(TgEfUtils.EfContext);
			TgEfStorageResult<TgEfSourceEntity> storageResult = await repo.GetFirstAsync(isNoTracking: true);
			if (storageResult.IsExists)
			{
				TgEfSourceEntity itemFind = (await repo.GetAsync(storageResult.Item, isNoTracking: true)).Item;
				Assert.That(itemFind, Is.Not.Null);
				TestContext.WriteLine(itemFind.ToDebugString());
				// Save
				storageResult = await repo.SaveAsync(itemFind);
				Assert.That(storageResult.IsExists);
			}
		});
	}

	#endregion
}