// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest;

[TestFixture]
internal class TgEfContextExistsTablesTests : TgDbContextTestsBase
{
	#region Public and private methods

	private void CheckTable(string tableName, bool isExists = true)
	{
		Assert.DoesNotThrow(() =>
		{
			if (isExists)
				Assert.That(EfProdContext.IsTableExists(tableName));
			else
				Assert.That(!EfProdContext.IsTableExists(tableName));
		});
	}

	[Test]
	public void Check_table_apps() => CheckTable(TgEfConstants.TableApps);

	[Test]
	public void Check_table_apps_test() => CheckTable(TgEfConstants.TableApps + "-TEST", isExists: false);

	[Test]
	public void Check_table_documents() => CheckTable(TgEfConstants.TableDocuments);

	[Test]
	public void Check_table_documents_test() => CheckTable(TgEfConstants.TableDocuments + "-TEST", isExists: false);

	[Test]
	public void Check_table_filters() => CheckTable(TgEfConstants.TableFilters);

	[Test]
	public void Check_table_filters_test() => CheckTable(TgEfConstants.TableFilters + "-TEST", isExists: false);

	[Test]
	public void Check_table_messages() => CheckTable(TgEfConstants.TableMessages);

	[Test]
	public void Check_table_messages_test() => CheckTable(TgEfConstants.TableMessages + "-TEST", isExists: false);

	[Test]
	public void Check_table_proxies() => CheckTable(TgEfConstants.TableProxies);

	[Test]
	public void Check_table_proxies_test() => CheckTable(TgEfConstants.TableProxies + "-TEST", isExists: false);

	[Test]
	public void Check_table_sources() => CheckTable(TgEfConstants.TableSources);

	[Test]
	public void Check_table_sources_test() => CheckTable(TgEfConstants.TableSources + "-TEST", isExists: false);

	[Test]
	public void Check_table_versions() => CheckTable(TgEfConstants.TableVersions);

	[Test]
	public void Check_table_versions_test() => CheckTable(TgEfConstants.TableVersions + "-TEST", isExists: false);

	#endregion
}