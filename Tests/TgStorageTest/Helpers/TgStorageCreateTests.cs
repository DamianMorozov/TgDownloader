// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Helpers;

[TestFixture]
internal class TgStorageCreateTests : TgDbContextTestsBase
{

	#region Public and private methods

	[Test, Order(0)]
	public void Delete_new_items()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			bool isDelete = (await XpoProdContext.AppRepository.DeleteNewAsync()).NotExist;
			TestContext.WriteLine($"{nameof(TgXpoAppRepository)}: {isDelete}");

			isDelete = (await XpoProdContext.DocumentRepository.DeleteNewAsync()).NotExist;
			TestContext.WriteLine($"{nameof(TgXpoDocumentRepository)}: {isDelete}");

			isDelete = (await XpoProdContext.FilterRepository.DeleteNewAsync()).NotExist;
			TestContext.WriteLine($"{nameof(TgXpoFilterRepository)}: {isDelete}");

			isDelete = (await XpoProdContext.MessageRepository.DeleteNewAsync()).NotExist;
			TestContext.WriteLine($"{nameof(TgXpoMessageRepository)}: {isDelete}");

			isDelete = (await XpoProdContext.ProxyRepository.DeleteNewAsync()).NotExist;
			TestContext.WriteLine($"{nameof(TgXpoProxyRepository)}: {isDelete}");

			isDelete = (await XpoProdContext.SourceRepository.DeleteNewAsync()).NotExist;
			TestContext.WriteLine($"{nameof(TgXpoSourceRepository)}: {isDelete}");

			isDelete = (await XpoProdContext.VersionRepository.DeleteNewAsync()).NotExist;
			TestContext.WriteLine($"{nameof(TgXpoVersionRepository)}: {isDelete}");
		});
	}

	public void CreateAndDeleteItem<T>(ITgXpoRepository<T> repository, Action<T> action) where T : XPLiteObject, ITgDbEntity, new()
    {
        Assert.DoesNotThrowAsync(async () =>
        {
            TestContext.WriteLine($"{typeof(T)}");
            T item = (await repository.GetNewAsync()).Item;
            TestContext.WriteLine($"{item.ToDebugString()}");
            Assert.That(item.NotExist);

            bool isSave = (await repository.SaveAsync(item)).IsExist;
            Assert.That(isSave);
            item = (await repository.GetAsync(item)).Item;
            TestContext.WriteLine($"{item.ToDebugString()}");
            Assert.That(item.IsExist);

            action(item);
			isSave = (await repository.SaveAsync(item)).IsExist;
            Assert.That(isSave);
            item = (await repository.GetAsync(item)).Item;
            TestContext.WriteLine($"{item.ToDebugString()}");
            Assert.That(item.IsExist);
            
            bool isDelete = (await repository.DeleteAsync(item, isSkipFind: false)).NotExist;
            Assert.That(isDelete);

            var operResult = await repository.GetAsync(item);
            Assert.That(operResult.NotExist);
        });
    }

    [Test]
    public void Create_and_delete_new_app() => CreateAndDeleteItem(XpoProdContext.AppRepository,
	    item => { item.PhoneNumber = "Changed"; });

    [Test]
    public void Create_and_delete_new_document() => CreateAndDeleteItem(XpoProdContext.DocumentRepository,
	    item => { item.FileName = "Changed"; });

    [Test]
    public void Create_and_delete_new_filter() => CreateAndDeleteItem(XpoProdContext.FilterRepository,
	    item => { item.Name = "Changed"; });

	[Test]
    public void Create_and_delete_new_message() => CreateAndDeleteItem(XpoProdContext.MessageRepository,
	    item => { item.Message = "Changed"; });

	[Test]
    public void Create_and_delete_new_proxy() => CreateAndDeleteItem(XpoProdContext.ProxyRepository,
	    item => { item.HostName = "Changed"; });

	[Test]
    public void Create_and_delete_new_source() => CreateAndDeleteItem(XpoProdContext.SourceRepository,
	    item => { item.About = "Changed"; });

	[Test]
    public void Create_and_delete_new_version() => CreateAndDeleteItem(XpoProdContext.VersionRepository,
	    item => { item.Description = "Changed"; });

	#endregion
}