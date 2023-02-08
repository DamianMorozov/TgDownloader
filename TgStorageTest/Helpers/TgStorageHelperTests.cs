// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using NUnit.Framework;
using TgStorage.Models.Apps;
using TgStorage.Models.Proxies;
using TgStorageTest.Utils;

namespace TgStorageTest.Helpers;

[TestFixture]
internal class TgStorageHelperTests
{
    #region Public and private methods

    [Test]
    public void TgStorage_GetItem_App()
    {
        Assert.DoesNotThrow(() =>
        {
            SqlTableAppModel? app = TgStorageTestsUtils.DataCore.TgStorage.GetItemNullable<SqlTableAppModel>();
            TestContext.WriteLine(app is { } ? app.ToString() : "<Empty>");
            app = TgStorageTestsUtils.DataCore.TgStorage.GetItemFirstOrDefault<SqlTableAppModel>();
            TestContext.WriteLine(app);
            if (app.IsExists)
                TgStorageTestsUtils.DataCore.TgStorage.AddOrUpdateItem(app);
            app = TgStorageTestsUtils.DataCore.TgStorage.GetItemFirstOrDefault<SqlTableAppModel>();
            TestContext.WriteLine(app);
        });
    }

    [Test]
    public void TgStorage_GetItem_Proxy()
    {
        Assert.DoesNotThrow(() =>
        {
            SqlTableProxyModel? proxy = TgStorageTestsUtils.DataCore.TgStorage.GetItemNullable<SqlTableProxyModel>();
            TestContext.WriteLine(proxy is { } ? proxy.ToString() : "<Empty>");
            proxy = TgStorageTestsUtils.DataCore.TgStorage.GetItemFirstOrDefault<SqlTableProxyModel>();
            TestContext.WriteLine(proxy);
            if (proxy.IsExists)
                TgStorageTestsUtils.DataCore.TgStorage.AddOrUpdateItem(proxy);
            proxy = TgStorageTestsUtils.DataCore.TgStorage.GetItemFirstOrDefault<SqlTableProxyModel>();
            TestContext.WriteLine(proxy);
        });
    }

    #endregion
}