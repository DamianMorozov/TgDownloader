// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using NUnit.Framework;
using TgAssertCoreTests.Helpers;
using TgStorage.Models.Apps;
using TgStorage.Models.Proxies;

namespace TgStorageTest.Helpers;

[TestFixture]
internal class TgStorageHelperTests
{
    #region Public and private fields, properties, constructor

    private static DataCoreHelper DataCore => DataCoreHelper.Instance;

    #endregion

    #region Public and private methods

    [Test]
    public void TgStorage_GetItem_App()
    {
        Assert.DoesNotThrow(() =>
        {
            SqlTableAppModel? app = DataCore.TgStorage.GetItemNullable<SqlTableAppModel>();
            TestContext.WriteLine(app is { } ? app.ToString() : "<Empty>");
            app = DataCore.TgStorage.GetItem<SqlTableAppModel>();
            TestContext.WriteLine(app);
            if (app.IsExists)
                DataCore.TgStorage.AddOrUpdateItem(app);
            app = DataCore.TgStorage.GetItem<SqlTableAppModel>();
            TestContext.WriteLine(app);
        });
    }

    [Test]
    public void TgStorage_GetItem_Proxy()
    {
        Assert.DoesNotThrow(() =>
        {
            SqlTableProxyModel? proxy = DataCore.TgStorage.GetItemNullable<SqlTableProxyModel>();
            TestContext.WriteLine(proxy is { } ? proxy.ToString() : "<Empty>");
            proxy = DataCore.TgStorage.GetItem<SqlTableProxyModel>();
            TestContext.WriteLine(proxy);
            if (proxy.IsExists)
                DataCore.TgStorage.AddOrUpdateItem(proxy);
            proxy = DataCore.TgStorage.GetItem<SqlTableProxyModel>();
            TestContext.WriteLine(proxy);
        });
    }

    #endregion
}