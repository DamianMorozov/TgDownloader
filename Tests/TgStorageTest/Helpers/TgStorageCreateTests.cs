// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Helpers;

[TestFixture]
internal class TgStorageCreateTests
{
    #region Public and private methods

    public void CreateAndDeleteItem<T>(ITgSqlRepository<T> repository) where T : ITgSqlTable, new()
    {
        Assert.DoesNotThrow(() =>
        {
            TestContext.WriteLine($"{typeof(T)}");
            T item = repository.GetNew();
            TestContext.WriteLine($"{nameof(repository.GetNew)}: {item.ToDebugString()}");
            Assert.IsTrue(item.IsNotExists);

            bool isStore = repository.Save(item);
            TestContext.WriteLine($"{nameof(repository.Save)}: {isStore}");
            Assert.IsTrue(isStore);
            item = repository.Get(item);
            TestContext.WriteLine($"{nameof(repository.Get)}: {item.ToDebugString()}");
            Assert.IsTrue(item.IsExists);
            bool isDelete = repository.Delete(item);
            TestContext.WriteLine($"{nameof(repository.Delete)}: {isDelete}");
            Assert.IsTrue(isDelete);

            item = repository.Get(item);
            TestContext.WriteLine($"{nameof(repository.Get)}: {item.ToDebugString()}");
            Assert.IsTrue(item.IsNotExists);
        });
    }

    [Test, Order(0)]
    public void Delete_new_items()
    {
        Assert.DoesNotThrow(() =>
        {
            bool isDelete = TgSqlTableAppRepository.Instance.DeleteNew();
            TestContext.WriteLine($"{nameof(TgSqlTableAppRepository)}: {isDelete}");

            isDelete = TgSqlTableDocumentRepository.Instance.DeleteNew();
            TestContext.WriteLine($"{nameof(TgSqlTableDocumentRepository)}: {isDelete}");

            isDelete = TgSqlTableFilterRepository.Instance.DeleteNew();
            TestContext.WriteLine($"{nameof(TgSqlTableFilterRepository)}: {isDelete}");

            isDelete = TgSqlTableMessageRepository.Instance.DeleteNew();
            TestContext.WriteLine($"{nameof(TgSqlTableMessageRepository)}: {isDelete}");

            isDelete = TgSqlTableProxyRepository.Instance.DeleteNew();
            TestContext.WriteLine($"{nameof(TgSqlTableProxyRepository)}: {isDelete}");

            isDelete = TgSqlTableSourceRepository.Instance.DeleteNew();
            TestContext.WriteLine($"{nameof(TgSqlTableSourceRepository)}: {isDelete}");

            isDelete = TgSqlTableVersionRepository.Instance.DeleteNew();
            TestContext.WriteLine($"{nameof(TgSqlTableVersionRepository)}: {isDelete}");
        });
    }

    [Test]
    public void Create_and_delete_new_app() => CreateAndDeleteItem(TgSqlTableAppRepository.Instance);

    [Test]
    public void Create_and_delete_new_document() => CreateAndDeleteItem(TgSqlTableDocumentRepository.Instance);

    [Test]
    public void Create_and_delete_new_filter() => CreateAndDeleteItem(TgSqlTableFilterRepository.Instance);

    [Test]
    public void Create_and_delete_new_message() => CreateAndDeleteItem(TgSqlTableMessageRepository.Instance);

    [Test]
    public void Create_and_delete_new_proxy() => CreateAndDeleteItem(TgSqlTableProxyRepository.Instance);

    [Test]
    public void Create_and_delete_new_source() => CreateAndDeleteItem(TgSqlTableSourceRepository.Instance);

    [Test]
    public void Create_and_delete_new_version() => CreateAndDeleteItem(TgSqlTableVersionRepository.Instance);

	#endregion
}