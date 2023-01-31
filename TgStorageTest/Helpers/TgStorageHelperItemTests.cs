// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using NUnit.Framework;
using TgAssertCoreTests.Helpers;
using TgCore.Interfaces;
using TgStorage.Models;
using TgStorage.Models.Apps;
using TgStorage.Models.Proxies;

namespace TgStorageTest.Helpers;

[TestFixture]
internal class TgStorageHelperItemTests
{
    #region Public and private fields, properties, constructor

    private static DataCoreHelper DataCore => DataCoreHelper.Instance;

    #endregion

    #region Public and private methods

    [Test]
    public void TgStorage_GetTableModels_DoesNotThrow()
    {
        Assert.DoesNotThrow(() =>
        {
            List<ISqlTable> sqlTables = DataCore.TgStorage.GetTableModels();
            foreach (ISqlTable sqlTable in sqlTables)
            {
                TestContext.WriteLine(sqlTable.GetType());
            }
        });
    }

    [Test]
    public void TgStorage_GetItem_DoesNotThrow()
    {
        Assert.DoesNotThrow(() =>
        {
            List<Type> sqlTypes = DataCore.TgStorage.GetTableTypes();
            foreach (Type sqlType in sqlTypes)
            {
                switch (sqlType)
                {
                    case var cls when cls == typeof(SqlTableAppModel):
                        GetItem<SqlTableAppModel>();
                        if (NewEmptyItem<SqlTableAppModel>() is SqlTableAppModel app)
                        {
                            DataCore.TgStorage.AddOrUpdateItem(app);
                        }
                        break;
                    case var cls when cls == typeof(SqlTableProxyModel):
                        GetItem<SqlTableProxyModel>();
                        if (NewEmptyItem<SqlTableProxyModel>() is SqlTableProxyModel proxy)
                        {
                            DataCore.TgStorage.AddOrUpdateItem(proxy);
                        }
                        break;
                }
                TestContext.WriteLine();
            }
        });
    }

    private void GetItem<T>() where T : ISqlTable, new()
    {
        TestContext.WriteLine($"GetItem for {typeof(T)}");
        T? item = DataCore.TgStorage.GetItemNullable<T>();
        TestContext.WriteLine(item is { } ? item.ToString() : "<Empty>");
        item = DataCore.TgStorage.GetItem<T>();
        TestContext.WriteLine(item);
    }

    private SqlTableXpLiteBase NewEmptyItem<T>() where T : ISqlTable, new()
    {
        TestContext.WriteLine($"AddItem for {typeof(T)}");
        SqlTableXpLiteBase item = DataCore.TgStorage.NewEmpty<T>();
        TestContext.WriteLine(item);
        return item;
    }

    #endregion
}