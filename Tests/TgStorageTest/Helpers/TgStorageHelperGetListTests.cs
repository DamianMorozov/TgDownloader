// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Helpers;

[TestFixture]
internal class TgStorageHelperGetListTests
{
	#region Public and private methods

	[Test]
	public void Get_list_of_apps()
	{
		Assert.DoesNotThrow(() =>
		{
			List<TgSqlTableAppModel> items = TgStorageTestsUtils.DataCore.ContextManager.ContextTableApps.GetList(TgSqlEnumTableTopRecords.Top200);
			foreach (TgSqlTableAppModel item in items)
			{
				TestContext.WriteLine(item);
			}
		});
	}

	[Test]
	public void Get_list_of_documents()
	{
		Assert.DoesNotThrow(() =>
		{
			List<TgSqlTableDocumentModel> items = TgStorageTestsUtils.DataCore.ContextManager.ContextTableDocuments.GetList(TgSqlEnumTableTopRecords.Top200);
			foreach (TgSqlTableDocumentModel item in items)
			{
				TestContext.WriteLine(item);
			}
		});
	}

	[Test]
	public void Get_list_of_filters()
	{
		Assert.DoesNotThrow(() =>
		{
			List<TgSqlTableFilterModel> items = TgStorageTestsUtils.DataCore.ContextManager.ContextTableFilters.GetList(TgSqlEnumTableTopRecords.Top200);
			foreach (TgSqlTableFilterModel item in items)
			{
				TestContext.WriteLine(item);
			}
		});
	}

	[Test]
	public void Get_list_of_proxies()
	{
		Assert.DoesNotThrow(() =>
		{
			List<TgSqlTableProxyModel> items = TgStorageTestsUtils.DataCore.ContextManager.ContextTableProxies.GetList(TgSqlEnumTableTopRecords.Top200);
			foreach (TgSqlTableProxyModel item in items)
			{
				TestContext.WriteLine(item);
			}
		});
	}

	[Test]
	public void Get_list_of_sources()
	{
		Assert.DoesNotThrow(() =>
		{
			List<TgSqlTableSourceModel> items = TgStorageTestsUtils.DataCore.ContextManager.ContextTableSources.GetList(TgSqlEnumTableTopRecords.Top1000);
			foreach (TgSqlTableSourceModel item in items)
			{
				TestContext.WriteLine(item);
			}
		});
	}

	[Test]
	public void Get_list_of_versions()
	{
		Assert.DoesNotThrow(() =>
		{
			List<TgSqlTableVersionModel> items = TgStorageTestsUtils.DataCore.ContextManager.ContextTableVersions.GetList(TgSqlEnumTableTopRecords.Top200);
			foreach (TgSqlTableVersionModel item in items)
			{
				TestContext.WriteLine(item);
			}
		});
	}

	[Test]
	public void Check_last_table_version_after_fill()
	{
		Assert.DoesNotThrow(() =>
		{
			//TgStorageTestsUtils.DataCore.TgStorage.CheckTableVersions();
			TgStorageTestsUtils.DataCore.ContextManager.FillTableVersions();
			TgSqlTableVersionModel versionLast =
				!TgStorageTestsUtils.DataCore.ContextManager.IsTableExists(TgSqlConstants.TableVersions)
				? TgStorageTestsUtils.DataCore.ContextManager.ContextTableVersions.GetNewItem() 
				: TgStorageTestsUtils.DataCore.ContextManager.ContextTableVersions.GetItemLast();
			TestContext.WriteLine(versionLast);
			Assert.That(Equals(TgStorageTestsUtils.DataCore.ContextManager.ContextTableVersions.VersionLast, versionLast.Version));
		});
	}

	#endregion
}