//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//namespace TgStorageTest.Helpers;

//[TestFixture]
//internal class TgStorageUpdateTests : TgDbContextTestsBase
//{
//    #region Public and private methods

//    private void UpdateItem<T>(ITgXpoRepository<T> repository) where T : XPLiteObject, ITgDbEntity, new()
//	{
//	    Assert.DoesNotThrowAsync(async () =>
//	    {
//		    T item = (await repository.GetFirstAsync()).Item;
//		    TestContext.WriteLine($"{nameof(repository.GetFirstAsync)}: {item.ToDebugString()}");
//		    Assert.That(item.IsExist);

//		    Guid proxyUid = item.ProxyUid;
//		    item.ProxyUid = Guid.Empty;
//		    bool isUpdate = (await repository.SaveAsync(item)).IsExist;
//		    TestContext.WriteLine($"Update: {item.ToDebugString()}");
//		    Assert.That(isUpdate);

//		    item.ProxyUid = proxyUid;
//		    isUpdate = (await repository.SaveAsync(item)).IsExist;
//		    TestContext.WriteLine($"Update: {item.ToDebugString()}");
//		    Assert.That(isUpdate);
//	    });
//	}

//    [Test]
//    public void Update_app() => UpdateItem(XpoProdContext.AppRepository);

//    [Test]
//    public void Update_document()
//    {
//        Assert.DoesNotThrowAsync(async () =>
//        {
//            TgXpoDocumentRepository repository = XpoProdContext.DocumentRepository;
//            TgXpoDocumentEntity item = (await repository.GetFirstAsync()).Item;
//            TestContext.WriteLine($"{nameof(repository.GetFirstAsync)}: {item.ToDebugString()}");
//            Assert.That(item.IsExist);

//            string fileName = item.FileName;
//            item.FileName = "<Empty>";
//            bool isUpdate = (await repository.SaveAsync(item)).IsExist;
//            TestContext.WriteLine($"Update: {item.ToDebugString()}");
//            Assert.That(isUpdate);

//            item.FileName = fileName;
//            isUpdate = (await repository.SaveAsync(item)).IsExist;
//            TestContext.WriteLine($"Update: {item.ToDebugString()}");
//            Assert.That(isUpdate);
//        });
//    }

//    [Test]
//    public void Update_filter()
//    {
//        Assert.DoesNotThrow(() =>
//        {
//            //
//        });
//    }

//    [Test]
//    public void Update_message()
//    {
//        Assert.DoesNotThrowAsync(async () =>
//        {
//            TgXpoMessageRepository repository = XpoProdContext.MessageRepository;
//            TgXpoMessageEntity item = (await repository.GetFirstAsync()).Item;
//            TestContext.WriteLine($"{nameof(repository.GetFirstAsync)}: {item.ToDebugString()}");
//            Assert.That(item.IsExist);

//            string message = item.Message;
//            item.Message = "<Empty>";
//            bool isUpdate = (await repository.SaveAsync(item)).IsExist;
//            TestContext.WriteLine($"Update: {item.ToDebugString()}");
//            Assert.That(isUpdate);

//            item.Message = message;
//            isUpdate = (await repository.SaveAsync(item)).IsExist;
//            TestContext.WriteLine($"Update: {item.ToDebugString()}");
//            Assert.That(isUpdate);
//        });
//    }

//    [Test]
//    public void Update_proxy()
//    {
//        Assert.DoesNotThrowAsync(async () =>
//        {
//            TgXpoProxyRepository repository = XpoProdContext.ProxyRepository;
//            TgXpoProxyEntity item = (await repository.GetFirstAsync()).Item;
//            TestContext.WriteLine($"{nameof(repository.GetFirstAsync)}: {item.ToDebugString()}");
//            Assert.That(item.IsExist);

//            string secret = item.Secret;
//            item.Secret = "<Empty>";
//            bool isUpdate = (await repository.SaveAsync(item)).IsExist;
//            TestContext.WriteLine($"Update: {item.ToDebugString()}");
//            Assert.That(isUpdate);

//            item.Secret = secret;
//            isUpdate = (await repository.SaveAsync(item)).IsExist;
//            TestContext.WriteLine($"Update: {item.ToDebugString()}");
//            Assert.That(isUpdate);
//        });
//    }

//    [Test]
//    public void Update_source()
//    {
//        Assert.DoesNotThrowAsync(async () =>
//        {
//            TgXpoSourceRepository repository = XpoProdContext.SourceRepository;
//            TgXpoSourceEntity item = (await repository.GetFirstAsync()).Item;
//            TestContext.WriteLine($"{nameof(repository.GetFirstAsync)}: {item.ToDebugString()}");
//            Assert.That(item.IsExist);

//            string title = item.Title;
//            item.Title = "<Empty>";
//            bool isUpdate = (await repository.SaveAsync(item)).IsExist;
//            TestContext.WriteLine($"Update: {item.ToDebugString()}");
//            Assert.That(isUpdate);

//            item.Title = title;
//            isUpdate = (await repository.SaveAsync(item)).IsExist;
//            TestContext.WriteLine($"Update: {item.ToDebugString()}");
//            Assert.That(isUpdate);
//        });
//    }

//    [Test]
//    public void Update_version()
//    {
//        Assert.DoesNotThrowAsync(async () =>
//        {
//            TgXpoVersionRepository repository = XpoProdContext.VersionRepository;
//            TgXpoVersionEntity item = (await repository.GetFirstAsync()).Item;
//            TestContext.WriteLine($"{nameof(repository.GetFirstAsync)}: {item.ToDebugString()}");
//            Assert.That(item.IsExist);

//            string description = item.Description;
//            item.Description = "<Empty>";
//            bool isUpdate = (await repository.SaveAsync(item)).IsExist;
//            TestContext.WriteLine($"Update: {item.ToDebugString()}");
//            Assert.That(isUpdate);

//            item.Description = description;
//            isUpdate = (await repository.SaveAsync(item)).IsExist;
//            TestContext.WriteLine($"Update: {item.ToDebugString()}");
//            Assert.That(isUpdate);
//        });
//    }

//    #endregion
//}