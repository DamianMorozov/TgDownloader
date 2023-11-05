// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Helpers;

[TestFixture]
internal class TgStorageUpdateTests
{
    #region Public and private methods

    [Test]
    public void Update_app()
    {
        Assert.DoesNotThrow(() =>
        {
            TgSqlTableAppRepository repository = TgSqlTableAppRepository.Instance;
            TgSqlTableAppModel item = repository.GetFirstAsync().GetAwaiter().GetResult();
            TestContext.WriteLine($"{nameof(repository.GetFirstAsync)}: {item.ToDebugString()}");
            Assert.IsTrue(item.IsExists);

            Guid proxyUid = item.ProxyUid;
            item.ProxyUid = Guid.Empty;
            bool isUpdate = repository.SaveAsync(item).GetAwaiter().GetResult();
            TestContext.WriteLine($"Update: {item.ToDebugString()}");
            Assert.IsTrue(isUpdate);

            item.ProxyUid = proxyUid;
            isUpdate = repository.SaveAsync(item).GetAwaiter().GetResult();
            TestContext.WriteLine($"Update: {item.ToDebugString()}");
            Assert.IsTrue(isUpdate);
        });
    }

    [Test]
    public void Update_document()
    {
        Assert.DoesNotThrow(() =>
        {
            TgSqlTableDocumentRepository repository = TgSqlTableDocumentRepository.Instance;
            TgSqlTableDocumentModel item = repository.GetFirstAsync().GetAwaiter().GetResult();
            TestContext.WriteLine($"{nameof(repository.GetFirstAsync)}: {item.ToDebugString()}");
            Assert.IsTrue(item.IsExists);

            string fileName = item.FileName;
            item.FileName = "<Empty>";
            bool isUpdate = repository.SaveAsync(item).GetAwaiter().GetResult();
            TestContext.WriteLine($"Update: {item.ToDebugString()}");
            Assert.IsTrue(isUpdate);

            item.FileName = fileName;
            isUpdate = repository.SaveAsync(item).GetAwaiter().GetResult();
            TestContext.WriteLine($"Update: {item.ToDebugString()}");
            Assert.IsTrue(isUpdate);
        });
    }

    [Test]
    public void Update_filter()
    {
        Assert.DoesNotThrow(() =>
        {
            //
        });
    }

    [Test]
    public void Update_message()
    {
        Assert.DoesNotThrow(() =>
        {
            TgSqlTableMessageRepository repository = TgSqlTableMessageRepository.Instance;
            TgSqlTableMessageModel item = repository.GetFirstAsync().GetAwaiter().GetResult();
            TestContext.WriteLine($"{nameof(repository.GetFirstAsync)}: {item.ToDebugString()}");
            Assert.IsTrue(item.IsExists);

            string message = item.Message;
            item.Message = "<Empty>";
            bool isUpdate = repository.SaveAsync(item).GetAwaiter().GetResult();
            TestContext.WriteLine($"Update: {item.ToDebugString()}");
            Assert.IsTrue(isUpdate);

            item.Message = message;
            isUpdate = repository.SaveAsync(item).GetAwaiter().GetResult();
            TestContext.WriteLine($"Update: {item.ToDebugString()}");
            Assert.IsTrue(isUpdate);
        });
    }

    [Test]
    public void Update_proxy()
    {
        Assert.DoesNotThrow(() =>
        {
            TgSqlTableProxyRepository repository = TgSqlTableProxyRepository.Instance;
            TgSqlTableProxyModel item = repository.GetFirstAsync().GetAwaiter().GetResult();
            TestContext.WriteLine($"{nameof(repository.GetFirstAsync)}: {item.ToDebugString()}");
            Assert.IsTrue(item.IsExists);

            string secret = item.Secret;
            item.Secret = "<Empty>";
            bool isUpdate = repository.SaveAsync(item).GetAwaiter().GetResult();
            TestContext.WriteLine($"Update: {item.ToDebugString()}");
            Assert.IsTrue(isUpdate);

            item.Secret = secret;
            isUpdate = repository.SaveAsync(item).GetAwaiter().GetResult();
            TestContext.WriteLine($"Update: {item.ToDebugString()}");
            Assert.IsTrue(isUpdate);
        });
    }

    [Test]
    public void Update_source()
    {
        Assert.DoesNotThrow(() =>
        {
            TgSqlTableSourceRepository repository = TgSqlTableSourceRepository.Instance;
            TgSqlTableSourceModel item = repository.GetFirstAsync().GetAwaiter().GetResult();
            TestContext.WriteLine($"{nameof(repository.GetFirstAsync)}: {item.ToDebugString()}");
            Assert.IsTrue(item.IsExists);

            string title = item.Title;
            item.Title = "<Empty>";
            bool isUpdate = repository.SaveAsync(item).GetAwaiter().GetResult();
            TestContext.WriteLine($"Update: {item.ToDebugString()}");
            Assert.IsTrue(isUpdate);

            item.Title = title;
            isUpdate = repository.SaveAsync(item).GetAwaiter().GetResult();
            TestContext.WriteLine($"Update: {item.ToDebugString()}");
            Assert.IsTrue(isUpdate);
        });
    }

    [Test]
    public void Update_version()
    {
        Assert.DoesNotThrow(() =>
        {
            TgSqlTableVersionRepository repository = TgSqlTableVersionRepository.Instance;
            TgSqlTableVersionModel item = repository.GetFirstAsync().GetAwaiter().GetResult();
            TestContext.WriteLine($"{nameof(repository.GetFirstAsync)}: {item.ToDebugString()}");
            Assert.IsTrue(item.IsExists);

            string description = item.Description;
            item.Description = "<Empty>";
            bool isUpdate = repository.SaveAsync(item).GetAwaiter().GetResult();
            TestContext.WriteLine($"Update: {item.ToDebugString()}");
            Assert.IsTrue(isUpdate);

            item.Description = description;
            isUpdate = repository.SaveAsync(item).GetAwaiter().GetResult();
            TestContext.WriteLine($"Update: {item.ToDebugString()}");
            Assert.IsTrue(isUpdate);
        });
    }

    #endregion
}