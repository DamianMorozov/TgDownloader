// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal class TgEfRepositoryCreateNewTests : TgDbContextTestsBase
{
	#region Public and private methods

	[Test]
    public void Get_table_models()
    {
        Assert.DoesNotThrow(() =>
        {
            IEnumerable<ITgDbEntity> sqlTables = EfProdContext.GetTableModels();
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
			IEnumerable<Type> sqlTypes = EfProdContext.GetTableTypes();
            foreach (Type sqlType in sqlTypes)
            {
                switch (sqlType)
                {
                    case var cls when cls == typeof(TgEfAppEntity):
	                    await CreateNewItemAndDeleteAsync(new TgEfAppRepository(TgEfUtils.EfContext));
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
                    case var cls when cls == typeof(TgEfVersionEntity):
	                    await CreateNewItemAndDeleteAsync(new TgEfVersionRepository(TgEfUtils.EfContext));
                        break;
                }
                TestContext.WriteLine();
            }
        });
    }

    private async Task CreateNewItemAndDeleteAsync<T>(ITgEfRepository<T> repository) where T : TgEfEntityBase, ITgDbEntity, new()
    {
		TgEfOperResult<T> operResult = await repository.CreateNewAsync();
		Assert.That(operResult.IsExists);
		TestContext.WriteLine(operResult.Item.ToDebugString());
		operResult = await repository.DeleteAsync(operResult.Item, isSkipFind: false);
		Assert.That(!operResult.IsExists);
    }

	[Test]
    public void Get_new_items_and_delete()
    {
        Assert.DoesNotThrowAsync(async () =>
        {
			IEnumerable<Type> sqlTypes = EfProdContext.GetTableTypes();
            foreach (Type sqlType in sqlTypes)
            {
				switch (sqlType)
				{
					case var cls when cls == typeof(TgEfAppEntity):
						await GetNewItemsAndDeleteAsync(new TgEfAppRepository(TgEfUtils.EfContext));
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
					case var cls when cls == typeof(TgEfVersionEntity):
						await GetNewItemsAndDeleteAsync(new TgEfVersionRepository(TgEfUtils.EfContext));
						break;
				}
				TestContext.WriteLine();
            }
        });
    }

	private async Task GetNewItemsAndDeleteAsync<T>(ITgEfRepository<T> repository) where T : TgEfEntityBase, ITgDbEntity, new()
	{
		TgEfOperResult<T> operResult;
		do
		{
			operResult = await repository.GetNewAsync(isNoTracking: false);
			if (operResult.IsExists)
			{
				TestContext.WriteLine(operResult.Item.ToDebugString());
				TgEfOperResult<T> operResultDelete = await repository.DeleteAsync(operResult.Item, isSkipFind: false);
				Assert.That(!operResultDelete.IsExists);
			}
		} while (operResult.IsExists);
	}

	#endregion
}