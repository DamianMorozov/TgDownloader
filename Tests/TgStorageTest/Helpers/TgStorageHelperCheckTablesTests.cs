// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Helpers;

[TestFixture]
internal class TgStorageHelperCheckTablesTests
{
	#region Public and private methods

	[Test]
	public void Check_table_apps()
	{
		Assert.DoesNotThrow(() =>
		{
			Assert.IsTrue(TgStorageTestsUtils.DataCore.ContextManager.CheckTableProxies());
			Assert.IsTrue(TgStorageTestsUtils.DataCore.ContextManager.CheckTableApps());
		});
	}

    [Test]
	public void Check_table_documents()
	{
		Assert.DoesNotThrow(() =>
		{
			Assert.IsTrue(TgStorageTestsUtils.DataCore.ContextManager.CheckTableDocuments());
		});
	}

	[Test]
	public void Check_table_filters()
	{
		Assert.DoesNotThrow(() =>
		{
			Assert.IsTrue(TgStorageTestsUtils.DataCore.ContextManager.CheckTableFilters());
		});
	}

	[Test]
	public void Check_table_messages()
	{
		Assert.DoesNotThrow(() =>
		{
			Assert.IsTrue(TgStorageTestsUtils.DataCore.ContextManager.CheckTableMessages());
		});
	}

	[Test]
	public void Check_table_proxies()
	{
		Assert.DoesNotThrow(() =>
		{
			Assert.IsTrue(TgStorageTestsUtils.DataCore.ContextManager.CheckTableProxies());
		});
	}

	[Test]
	public void Check_table_sources()
	{
		Assert.DoesNotThrow(() =>
		{
			Assert.IsTrue(TgStorageTestsUtils.DataCore.ContextManager.CheckTableSources());
		});
	}

	[Test]
	public void Check_table_versions()
	{
		Assert.DoesNotThrow(() =>
		{
			Assert.IsTrue(TgStorageTestsUtils.DataCore.ContextManager.CheckTableVersions());
		});
	}

	#endregion
}