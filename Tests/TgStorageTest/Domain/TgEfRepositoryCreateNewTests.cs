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
	                    await CreateNewItemAndDeleteAsync(EfProdContext.AppRepository);
						break;
                    case var cls when cls == typeof(TgEfDocumentEntity):
	                    await CreateNewItemAndDeleteAsync(EfProdContext.DocumentRepository);
                        break;
                    case var cls when cls == typeof(TgEfFilterEntity):
	                    await CreateNewItemAndDeleteAsync(EfProdContext.FilterRepository);
                        break;
                    case var cls when cls == typeof(TgEfMessageEntity):
	                    await CreateNewItemAndDeleteAsync(EfProdContext.MessageRepository);
                        break;
                    case var cls when cls == typeof(TgEfProxyEntity):
	                    await CreateNewItemAndDeleteAsync(EfProdContext.ProxyRepository);
                        break;
                    case var cls when cls == typeof(TgEfSourceEntity):
	                    await CreateNewItemAndDeleteAsync(EfProdContext.SourceRepository);
                        break;
                    case var cls when cls == typeof(TgEfVersionEntity):
	                    await CreateNewItemAndDeleteAsync(EfProdContext.VersionRepository);
                        break;
                }
                TestContext.WriteLine();
            }
        });
    }

    private async Task CreateNewItemAndDeleteAsync<T>(ITgEfRepository<T> repository) where T : TgEfEntityBase, ITgDbEntity, new()
    {
		TgEfOperResult<T> operResult = await repository.CreateNewAsync();
		Assert.That(operResult.IsExist);
		TestContext.WriteLine(operResult.Item.ToDebugString());
		operResult = await repository.DeleteAsync(operResult.Item, isSkipFind: false);
		Assert.That(operResult.NotExist);
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
						await GetNewItemsAndDeleteAsync(EfProdContext.AppRepository);
						break;
					case var cls when cls == typeof(TgEfDocumentEntity):
						await GetNewItemsAndDeleteAsync(EfProdContext.DocumentRepository);
						break;
					case var cls when cls == typeof(TgEfFilterEntity):
						await GetNewItemsAndDeleteAsync(EfProdContext.FilterRepository);
						break;
					case var cls when cls == typeof(TgEfMessageEntity):
						await GetNewItemsAndDeleteAsync(EfProdContext.MessageRepository);
						break;
					case var cls when cls == typeof(TgEfProxyEntity):
						await GetNewItemsAndDeleteAsync(EfProdContext.ProxyRepository);
						break;
					case var cls when cls == typeof(TgEfSourceEntity):
						await GetNewItemsAndDeleteAsync(EfProdContext.SourceRepository);
						break;
					case var cls when cls == typeof(TgEfVersionEntity):
						await GetNewItemsAndDeleteAsync(EfProdContext.VersionRepository);
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
			if (operResult.IsExist)
			{
				TestContext.WriteLine(operResult.Item.ToDebugString());
				TgEfOperResult<T> operResultDelete = await repository.DeleteAsync(operResult.Item, isSkipFind: false);
				Assert.That(operResultDelete.NotExist);
			}
		} while (operResult.IsExist);
	}

	#endregion
}