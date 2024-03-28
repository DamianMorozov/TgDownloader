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
            T item = repository.GetNewAsync(true).Result;
            TestContext.WriteLine($"{nameof(repository.GetNewAsync)}: {item.ToDebugString()}");
            Assert.That(item.IsNotExists);

            bool isStore = repository.SaveAsync(item).Result;
            TestContext.WriteLine($"{nameof(repository.SaveAsync)}: {isStore}");
            Assert.That(isStore);
            item = repository.GetAsync(item).Result;
            TestContext.WriteLine($"{nameof(repository.GetAsync)}: {item.ToDebugString()}");
            Assert.That(item.IsExists);
            bool isDelete = repository.DeleteAsync(item).Result;
            TestContext.WriteLine($"{nameof(repository.DeleteAsync)}: {isDelete}");
            Assert.That(isDelete);

            item = repository.GetAsync(item).Result;
            TestContext.WriteLine($"{nameof(repository.GetAsync)}: {item.ToDebugString()}");
            Assert.That(item.IsNotExists);
        });
    }

    [Test, Order(0)]
    public void Delete_new_items()
    {
        Assert.DoesNotThrow(() =>
        {
            bool isDelete = TgSqlTableAppRepository.Instance.DeleteNewAsync(true).Result;
            TestContext.WriteLine($"{nameof(TgSqlTableAppRepository)}: {isDelete}");

            isDelete = TgSqlTableDocumentRepository.Instance.DeleteNewAsync(true).Result;
            TestContext.WriteLine($"{nameof(TgSqlTableDocumentRepository)}: {isDelete}");

            isDelete = TgSqlTableFilterRepository.Instance.DeleteNewAsync(true).Result;
            TestContext.WriteLine($"{nameof(TgSqlTableFilterRepository)}: {isDelete}");

            isDelete = TgSqlTableMessageRepository.Instance.DeleteNewAsync(true).Result;
            TestContext.WriteLine($"{nameof(TgSqlTableMessageRepository)}: {isDelete}");

            isDelete = TgSqlTableProxyRepository.Instance.DeleteNewAsync(true).Result;
            TestContext.WriteLine($"{nameof(TgSqlTableProxyRepository)}: {isDelete}");

            isDelete = TgSqlTableSourceRepository.Instance.DeleteNewAsync(true).Result;
            TestContext.WriteLine($"{nameof(TgSqlTableSourceRepository)}: {isDelete}");

            isDelete = TgSqlTableVersionRepository.Instance.DeleteNewAsync(true).Result;
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