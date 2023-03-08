// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using NUnit.Framework;
using TgStorage.Models.Apps;
using TgStorage.Models.Filters;
using TgStorage.Models.Proxies;
using TgStorage.Models.Versions;
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
	public void TgStorage_GetItem_Filter()
	{
		Assert.DoesNotThrow(() =>
		{
			SqlTableFilterModel? filter = TgStorageTestsUtils.DataCore.TgStorage.GetItemNullable<SqlTableFilterModel>();
			TestContext.WriteLine(filter is { } ? filter.ToString() : "<Empty>");
			filter = TgStorageTestsUtils.DataCore.TgStorage.GetItemFirstOrDefault<SqlTableFilterModel>();
			TestContext.WriteLine(filter);
			if (filter.IsExists)
				TgStorageTestsUtils.DataCore.TgStorage.AddOrUpdateItem(filter);
			filter = TgStorageTestsUtils.DataCore.TgStorage.GetItemFirstOrDefault<SqlTableFilterModel>();
			TestContext.WriteLine(filter);
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

	[Test]
    public void TgStorage_GetItem_Version()
    {
        Assert.DoesNotThrow(() =>
        {
            SqlTableVersionModel? version = TgStorageTestsUtils.DataCore.TgStorage.GetItemNullable<SqlTableVersionModel>();
            TestContext.WriteLine(version is { } ? version.ToString() : "<Empty>");
            version = TgStorageTestsUtils.DataCore.TgStorage.GetItemFirstOrDefault<SqlTableVersionModel>();
            TestContext.WriteLine(version);
            if (version.IsExists)
                TgStorageTestsUtils.DataCore.TgStorage.AddOrUpdateItem(version);
            version = TgStorageTestsUtils.DataCore.TgStorage.GetItemFirstOrDefault<SqlTableVersionModel>();
            TestContext.WriteLine(version);
        });
    }

    #endregion
}