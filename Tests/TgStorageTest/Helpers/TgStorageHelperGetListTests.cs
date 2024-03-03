// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Helpers;

[TestFixture]
internal class TgStorageHelperGetEnumerableTests
{
	#region Public and private methods

	[Test]
	public void Get_enumerable_of_apps()
	{
		Assert.DoesNotThrow(() =>
		{
            IEnumerable<TgSqlTableAppModel> items = TgStorageTestsUtils.AppRepository.GetEnumerable(TgSqlEnumTableTopRecords.Top20);
			foreach (TgSqlTableAppModel item in items)
			{
				TestContext.WriteLine(item);
			}
		});
	}

	[Test]
	public void Get_enumerable_of_documents()
	{
		Assert.DoesNotThrow(() =>
		{
			IEnumerable<TgSqlTableDocumentModel> items = TgStorageTestsUtils.DocumentRepository.GetEnumerable(TgSqlEnumTableTopRecords.Top20);
			foreach (TgSqlTableDocumentModel item in items)
			{
				TestContext.WriteLine(item);
			}
		});
	}

	[Test]
	public void Get_enumerable_of_filters()
	{
		Assert.DoesNotThrow(() =>
		{
			IEnumerable<TgSqlTableFilterModel> items = TgStorageTestsUtils.FilterRepository.GetEnumerable(TgSqlEnumTableTopRecords.Top20);
			foreach (TgSqlTableFilterModel item in items)
			{
				TestContext.WriteLine(item);
			}
		});
	}

	[Test]
	public void Get_enumerable_of_proxies()
	{
		Assert.DoesNotThrow(() =>
		{
			IEnumerable<TgSqlTableProxyModel> items = TgStorageTestsUtils.ProxyRepository.GetEnumerable(TgSqlEnumTableTopRecords.Top20);
			foreach (TgSqlTableProxyModel item in items)
			{
				TestContext.WriteLine(item);
			}
		});
	}

	[Test]
	public void Get_enumerable_of_sources()
	{
		Assert.DoesNotThrow(() =>
		{
			IEnumerable<TgSqlTableSourceModel> items = TgStorageTestsUtils.SourceRepository.GetEnumerable(TgSqlEnumTableTopRecords.Top1000);
			foreach (TgSqlTableSourceModel item in items)
			{
				TestContext.WriteLine(item);
			}
		});
	}

	[Test]
	public void Get_enumerable_of_versions()
	{
		Assert.DoesNotThrow(() =>
		{
			IEnumerable<TgSqlTableVersionModel> items = TgStorageTestsUtils.VersionRepository.GetEnumerable();
            int i = 0;
			foreach (TgSqlTableVersionModel item in items)
			{
				if (i < 18)
				{
					TestContext.WriteLine(item);
					i = item.Version;
				}
                else
                {
                    TgStorageTestsUtils.VersionRepository.DeleteAsync(item).GetAwaiter().GetResult();
                }
            }
		});
	}

	[Test]
	public void Check_last_table_version_after_fill()
	{
		Assert.DoesNotThrow(() =>
		{
			TgStorageTestsUtils.DataCore.ContextManager.FillTableVersions();
			TgSqlTableVersionModel versionLast =
				!TgStorageTestsUtils.DataCore.ContextManager.IsTableExists(TgSqlConstants.TableVersions)
				? TgStorageTestsUtils.VersionRepository.GetNewAsync().GetAwaiter().GetResult()
                : TgStorageTestsUtils.VersionRepository.GetItemLastAsync().GetAwaiter().GetResult();
			TestContext.WriteLine(versionLast);
			Assert.That(Equals(TgStorageTestsUtils.VersionRepository.LastVersion, versionLast.Version));
		});
	}

	#endregion
}