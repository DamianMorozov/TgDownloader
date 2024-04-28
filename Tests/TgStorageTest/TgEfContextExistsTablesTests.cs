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
	public void Check_table_apps() => CheckTable(TgStorageConstants.TableApps);

	[Test]
	public void Check_table_apps_test() => CheckTable(TgStorageConstants.TableApps + "-TEST", isExists: false);

	[Test]
	public void Check_table_documents() => CheckTable(TgStorageConstants.TableDocuments);

	[Test]
	public void Check_table_documents_test() => CheckTable(TgStorageConstants.TableDocuments + "-TEST", isExists: false);

	[Test]
	public void Check_table_filters() => CheckTable(TgStorageConstants.TableFilters);

	[Test]
	public void Check_table_filters_test() => CheckTable(TgStorageConstants.TableFilters + "-TEST", isExists: false);

	[Test]
	public void Check_table_messages() => CheckTable(TgStorageConstants.TableMessages);

	[Test]
	public void Check_table_messages_test() => CheckTable(TgStorageConstants.TableMessages + "-TEST", isExists: false);

	[Test]
	public void Check_table_proxies() => CheckTable(TgStorageConstants.TableProxies);

	[Test]
	public void Check_table_proxies_test() => CheckTable(TgStorageConstants.TableProxies + "-TEST", isExists: false);

	[Test]
	public void Check_table_sources() => CheckTable(TgStorageConstants.TableSources);

	[Test]
	public void Check_table_sources_test() => CheckTable(TgStorageConstants.TableSources + "-TEST", isExists: false);

	[Test]
	public void Check_table_versions() => CheckTable(TgStorageConstants.TableVersions);

	[Test]
	public void Check_table_versions_test() => CheckTable(TgStorageConstants.TableVersions + "-TEST", isExists: false);

	#endregion
}