// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Domain;

[TestFixture]
internal class TgRepositoryCreateNewTests
{
    #region Public and private methods

    [Test]
    public void TgStorage_GetTableModels_DoesNotThrow()
    {
        Assert.DoesNotThrow(() =>
        {
            IEnumerable<ITgSqlTable> sqlTables = TgStorageTestsUtils.DataCore.ContextManager.GetTableModels();
            foreach (ITgSqlTable sqlTable in sqlTables)
            {
                TestContext.WriteLine(sqlTable.GetType());
            }
        });
    }

    private ITgSqlTable CreateNew<T>() where T : ITgSqlTable, new()
    {
        TestContext.WriteLine($"AddItem for {typeof(T)}");
        ITgSqlTable item = new TgSqlTableEmpty();
        switch (typeof(T))
        {
            case var cls when cls == typeof(TgSqlTableAppModel):
                item = TgSqlUtils.CreateNewApp();
                break;
            case var cls when cls == typeof(TgSqlTableDocumentModel):
                item = TgSqlUtils.CreateNewDocument();
                break;
            case var cls when cls == typeof(TgSqlTableFilterModel):
                item = TgSqlUtils.CreateNewFilter();
                break;
            case var cls when cls == typeof(TgSqlTableMessageModel):
                item = TgSqlUtils.CreateNewMessage();
                break;
            case var cls when cls == typeof(TgSqlTableProxyModel):
                item = TgSqlUtils.CreateNewProxy();
                break;
            case var cls when cls == typeof(TgSqlTableSourceModel):
                item = TgSqlUtils.CreateNewSource();
                break;
            case var cls when cls == typeof(TgSqlTableVersionModel):
                item = TgSqlUtils.CreateNewVersion();
                break;
        }
        TestContext.WriteLine(item);
        return item;
    }

    [Test]
    public void TgStorage_NewItem_DoesNotThrow()
    {
        Assert.DoesNotThrow(() =>
        {
            IEnumerable<Type> sqlTypes = TgStorageTestsUtils.DataCore.ContextManager.GetTableTypes();
            foreach (Type sqlType in sqlTypes)
            {
                switch (sqlType)
                {
                    case var cls when cls == typeof(TgSqlTableAppModel):
                        if (CreateNew<TgSqlTableAppModel>() is TgSqlTableAppModel app)
                            TestContext.WriteLine(app);
                        break;
                    case var cls when cls == typeof(TgSqlTableDocumentModel):
                        if (CreateNew<TgSqlTableDocumentModel>() is TgSqlTableDocumentModel doc)
                            TestContext.WriteLine(doc);
                        break;
                    case var cls when cls == typeof(TgSqlTableFilterModel):
                        if (CreateNew<TgSqlTableFilterModel>() is TgSqlTableFilterModel filter)
                            TestContext.WriteLine(filter);
                        break;
                    case var cls when cls == typeof(TgSqlTableMessageModel):
                        if (CreateNew<TgSqlTableMessageModel>() is TgSqlTableMessageModel message)
                            TestContext.WriteLine(message);
                        break;
                    case var cls when cls == typeof(TgSqlTableProxyModel):
                        if (CreateNew<TgSqlTableProxyModel>() is TgSqlTableProxyModel proxy)
                            TestContext.WriteLine(proxy);
                        break;
                    case var cls when cls == typeof(TgSqlTableSourceModel):
                        if (CreateNew<TgSqlTableSourceModel>() is TgSqlTableSourceModel source)
                            TestContext.WriteLine(source);
                        break;
                    case var cls when cls == typeof(TgSqlTableVersionModel):
                        if (CreateNew<TgSqlTableVersionModel>() is TgSqlTableVersionModel version)
                            TestContext.WriteLine(version);
                        break;
                }
                TestContext.WriteLine();
            }
        });
    }

    [Test]
    public void TgStorage_GetItem_DoesNotThrow()
    {
        Assert.DoesNotThrow(() =>
        {
            IEnumerable<Type> sqlTypes = TgStorageTestsUtils.DataCore.ContextManager.GetTableTypes();
            foreach (Type sqlType in sqlTypes)
            {
                switch (sqlType)
                {
                    case var cls when cls == typeof(TgSqlTableAppModel):
                        if (CreateNew<TgSqlTableAppModel>() is TgSqlTableAppModel app)
                            TgStorageTestsUtils.AppRepository.GetAsync(app).GetAwaiter().GetResult();
                        break;
                    case var cls when cls == typeof(TgSqlTableDocumentModel):
                        if (CreateNew<TgSqlTableDocumentModel>() is TgSqlTableDocumentModel doc)
                            TgStorageTestsUtils.DocumentRepository.GetAsync(doc).GetAwaiter().GetResult();
                        break;
                    case var cls when cls == typeof(TgSqlTableFilterModel):
                        if (CreateNew<TgSqlTableFilterModel>() is TgSqlTableFilterModel filter)
                            TgStorageTestsUtils.FilterRepository.GetAsync(filter).GetAwaiter().GetResult();
                        break;
                    case var cls when cls == typeof(TgSqlTableMessageModel):
                        if (CreateNew<TgSqlTableMessageModel>() is TgSqlTableMessageModel message)
                            TgStorageTestsUtils.DataCore.ContextManager.MessageRepository.GetAsync(message).GetAwaiter().GetResult();
                        break;
                    case var cls when cls == typeof(TgSqlTableProxyModel):
                        if (CreateNew<TgSqlTableProxyModel>() is TgSqlTableProxyModel proxy)
                            TgStorageTestsUtils.ProxyRepository.GetAsync(proxy).GetAwaiter().GetResult();
                        break;
                    case var cls when cls == typeof(TgSqlTableSourceModel):
                        if (CreateNew<TgSqlTableSourceModel>() is TgSqlTableSourceModel source)
                            TgStorageTestsUtils.SourceRepository.GetAsync(source).GetAwaiter().GetResult();
                        break;
                    case var cls when cls == typeof(TgSqlTableVersionModel):
                        if (CreateNew<TgSqlTableVersionModel>() is TgSqlTableVersionModel version)
                            TgStorageTestsUtils.VersionRepository.GetAsync(version).GetAwaiter().GetResult();
                        break;
                }
                TestContext.WriteLine();
            }
        });
    }

    #endregion
}