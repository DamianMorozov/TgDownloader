// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
#pragma warning disable NUnit1033

using TgInfrastructure.Contracts;

namespace TgStorageTest.Domain;

[TestFixture]
internal sealed class TgEfRepositoryCreateNewTests : TgDbContextTestsBase
{
	#region Public and private methods

	[Test]
    public void Get_table_models()
    {
        Assert.DoesNotThrow(() =>
        {
            IEnumerable<ITgDbEntity> sqlTables = TgEfUtils.GetTableModels();
            foreach (ITgDbEntity sqlTable in sqlTables)
            {
                TestContext.WriteLine(sqlTable.GetType());
            }
        });
    }

    [Test]
    public void Create_new_items_and_delete()
    {
        Assert.DoesNotThrowAsync(async () =>
        {
			IEnumerable<Type> sqlTypes = TgEfUtils.GetTableTypes();
            foreach (Type sqlType in sqlTypes)
            {
                switch (sqlType)
                {
                    case var cls when cls == typeof(TgEfAppEntity):
	                    await CreateNewItemAndDeleteAsync(new TgEfAppRepository(TgEfUtils.EfContext));
						break;
                    case var cls when cls == typeof(TgEfContactEntity):
	                    await CreateNewItemAndDeleteAsync(new TgEfContactRepository(TgEfUtils.EfContext));
						break;
                    case var cls when cls == typeof(TgEfDocumentEntity):
	                    await CreateNewItemAndDeleteAsync(new TgEfDocumentRepository(TgEfUtils.EfContext));
						break;
                    case var cls when cls == typeof(TgEfFilterEntity):
	                    await CreateNewItemAndDeleteAsync(new TgEfFilterRepository(TgEfUtils.EfContext));
						break;
                    case var cls when cls == typeof(TgEfMessageEntity):
	                    await CreateNewItemAndDeleteAsync(new TgEfMessageRepository(TgEfUtils.EfContext));
						break;
                    case var cls when cls == typeof(TgEfProxyEntity):
	                    await CreateNewItemAndDeleteAsync(new TgEfProxyRepository(TgEfUtils.EfContext));
                        break;
                    case var cls when cls == typeof(TgEfSourceEntity):
	                    await CreateNewItemAndDeleteAsync(new TgEfSourceRepository(TgEfUtils.EfContext));
						break;
                    case var cls when cls == typeof(TgEfStoryEntity):
	                    await CreateNewItemAndDeleteAsync(new TgEfStoryRepository(TgEfUtils.EfContext));
						break;
                    case var cls when cls == typeof(TgEfVersionEntity):
	                    await CreateNewItemAndDeleteAsync(new TgEfVersionRepository(TgEfUtils.EfContext));
						break;
                }
                TestContext.WriteLine();
            }
        });
    }

    private async Task CreateNewItemAndDeleteAsync<TEntity>(ITgEfRepository<TEntity> repository) where TEntity : ITgDbFillEntity<TEntity>, new()
    {
		TgEfStorageResult<TEntity> storageResult = await repository.CreateNewAsync();
		Assert.That(storageResult.IsExists);
		TestContext.WriteLine(storageResult.Item.ToDebugString());
		storageResult = await repository.DeleteAsync(storageResult.Item);
		Assert.That(!storageResult.IsExists);
    }

	[Test]
    public void Get_new_items_and_delete()
    {
        Assert.DoesNotThrowAsync(async () =>
        {
			IEnumerable<Type> sqlTypes = TgEfUtils.GetTableTypes();
            foreach (Type sqlType in sqlTypes)
            {
				switch (sqlType)
				{
					case var cls when cls == typeof(TgEfAppEntity):
						await GetNewItemsAndDeleteAsync(new TgEfAppRepository(TgEfUtils.EfContext));
						break;
					case var cls when cls == typeof(TgEfContactEntity):
						await GetNewItemsAndDeleteAsync(new TgEfContactRepository(TgEfUtils.EfContext));
						break;
					case var cls when cls == typeof(TgEfDocumentEntity):
						await GetNewItemsAndDeleteAsync(new TgEfDocumentRepository(TgEfUtils.EfContext));
						break;
					case var cls when cls == typeof(TgEfFilterEntity):
						await GetNewItemsAndDeleteAsync(new TgEfFilterRepository(TgEfUtils.EfContext));
						break;
					case var cls when cls == typeof(TgEfMessageEntity):
						await GetNewItemsAndDeleteAsync(new TgEfMessageRepository(TgEfUtils.EfContext));
						break;
					case var cls when cls == typeof(TgEfProxyEntity):
						await GetNewItemsAndDeleteAsync(new TgEfProxyRepository(TgEfUtils.EfContext));
						break;
					case var cls when cls == typeof(TgEfSourceEntity):
						await GetNewItemsAndDeleteAsync(new TgEfSourceRepository(TgEfUtils.EfContext));
						break;
					case var cls when cls == typeof(TgEfStoryEntity):
						await GetNewItemsAndDeleteAsync(new TgEfStoryRepository(TgEfUtils.EfContext));
						break;
					case var cls when cls == typeof(TgEfVersionEntity):
						await GetNewItemsAndDeleteAsync(new TgEfVersionRepository(TgEfUtils.EfContext));
						break;
				}
				TestContext.WriteLine();
            }
        });
    }

	private static async Task GetNewItemsAndDeleteAsync<TEntity>(ITgEfRepository<TEntity> repository) where TEntity : ITgDbFillEntity<TEntity>, new()
	{
		TgEfStorageResult<TEntity> storageResult;
		do
		{
			storageResult = await repository.GetNewAsync(isReadOnly: false);
			if (storageResult.IsExists)
			{
				TestContext.WriteLine(storageResult.Item.ToDebugString());
				TgEfStorageResult<TEntity> storageResultDelete = await repository.DeleteAsync(storageResult.Item);
				Assert.That(!storageResultDelete.IsExists);
			}
		} while (storageResult.IsExists);
	}

	#endregion
}