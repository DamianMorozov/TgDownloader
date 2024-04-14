// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal class TgXpoRepositoryCreateNewTests : TgDbContextTestsBase
{
	#region Public and private methods

	[Test]
    public void Get_table_models()
    {
        Assert.DoesNotThrow(() =>
        {
            IEnumerable<ITgDbEntity> sqlTables = XpoProdContext.GetTableModels();
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
			IEnumerable<Type> sqlTypes = XpoProdContext.GetTableTypes();
            foreach (Type sqlType in sqlTypes)
            {
                switch (sqlType)
                {
                    case var cls when cls == typeof(TgXpoAppEntity):
	                    await CreateNewItemAndDeleteAsync(XpoProdContext.AppRepository);
						break;
                    case var cls when cls == typeof(TgXpoDocumentEntity):
	                    await CreateNewItemAndDeleteAsync(XpoProdContext.DocumentRepository);
                        break;
                    case var cls when cls == typeof(TgXpoFilterEntity):
	                    await CreateNewItemAndDeleteAsync(XpoProdContext.FilterRepository);
                        break;
                    case var cls when cls == typeof(TgXpoMessageEntity):
	                    await CreateNewItemAndDeleteAsync(XpoProdContext.MessageRepository);
                        break;
                    case var cls when cls == typeof(TgXpoProxyEntity):
	                    await CreateNewItemAndDeleteAsync(XpoProdContext.ProxyRepository);
                        break;
                    case var cls when cls == typeof(TgXpoSourceEntity):
	                    await CreateNewItemAndDeleteAsync(XpoProdContext.SourceRepository);
                        break;
                    case var cls when cls == typeof(TgXpoVersionEntity):
	                    await CreateNewItemAndDeleteAsync(XpoProdContext.VersionRepository);
                        break;
                }
                TestContext.WriteLine();
            }
        });
    }

    private async Task CreateNewItemAndDeleteAsync<T>(ITgXpoRepository<T> repository) where T : XPLiteObject, ITgDbEntity, new()
    {
		TgXpoOperResult<T> operResult = await repository.CreateNewAsync();
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
			IEnumerable<Type> sqlTypes = XpoProdContext.GetTableTypes();
            foreach (Type sqlType in sqlTypes)
            {
				switch (sqlType)
				{
					case var cls when cls == typeof(TgXpoAppEntity):
						await GetNewItemsAndDeleteAsync(XpoProdContext.AppRepository);
						break;
					case var cls when cls == typeof(TgXpoDocumentEntity):
						await GetNewItemsAndDeleteAsync(XpoProdContext.DocumentRepository);
						break;
					case var cls when cls == typeof(TgXpoFilterEntity):
						await GetNewItemsAndDeleteAsync(XpoProdContext.FilterRepository);
						break;
					case var cls when cls == typeof(TgXpoMessageEntity):
						await GetNewItemsAndDeleteAsync(XpoProdContext.MessageRepository);
						break;
					case var cls when cls == typeof(TgXpoProxyEntity):
						await GetNewItemsAndDeleteAsync(XpoProdContext.ProxyRepository);
						break;
					case var cls when cls == typeof(TgXpoSourceEntity):
						await GetNewItemsAndDeleteAsync(XpoProdContext.SourceRepository);
						break;
					case var cls when cls == typeof(TgXpoVersionEntity):
						await GetNewItemsAndDeleteAsync(XpoProdContext.VersionRepository);
						break;
				}
				TestContext.WriteLine();
            }
        });
    }

	private async Task GetNewItemsAndDeleteAsync<T>(ITgXpoRepository<T> repository) where T : XPLiteObject, ITgDbEntity, new()
	{
		TgXpoOperResult<T> operResult;
		do
		{
			operResult = await repository.GetNewAsync();
			if (operResult.IsExist)
			{
				TestContext.WriteLine(operResult.Item.ToDebugString());
				TgXpoOperResult<T> operResultDelete = await repository.DeleteAsync(operResult.Item, isSkipFind: false);
				Assert.That(operResultDelete.NotExist);
			}
		} while (operResult.IsExist);
	}

	#endregion
}