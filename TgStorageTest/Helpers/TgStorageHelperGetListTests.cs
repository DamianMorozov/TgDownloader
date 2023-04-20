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
			List<TgSqlTableAppModel> items = TgStorageTestsUtils.DataCore.ContextManager.Apps.GetList(true);
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
			List<TgSqlTableDocumentModel> items = TgStorageTestsUtils.DataCore.ContextManager.Documents.GetList(true);
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
			List<TgSqlTableFilterModel> items = TgStorageTestsUtils.DataCore.ContextManager.Filters.GetList(true);
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
			List<TgSqlTableProxyModel> items = TgStorageTestsUtils.DataCore.ContextManager.Proxies.GetList(true);
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
			List<TgSqlTableSourceModel> items = TgStorageTestsUtils.DataCore.ContextManager.Sources.GetList(true);
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
			List<TgSqlTableVersionModel> items = TgStorageTestsUtils.DataCore.ContextManager.Versions.GetList(true);
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
				? TgStorageTestsUtils.DataCore.ContextManager.Versions.GetNewItem() 
				: TgStorageTestsUtils.DataCore.ContextManager.Versions.GetItemLast();
			TestContext.WriteLine(versionLast);
			Assert.That(Equals(TgStorageTestsUtils.DataCore.ContextManager.Versions.VersionLast, versionLast.Version));
		});
	}

	#endregion
}