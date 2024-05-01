// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest.Helpers;

[TestFixture]
internal class TgEfCheckTablesCrudTests : TgDbContextTestsBase
{
	#region Public and private methods

	[Test]
	public void Check_table_apps_crud()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableProxiesCrudAsync());
			Assert.That(await TgEfUtils.CheckTableAppsCrudAsync());
		});
	}

    [Test]
	public void Check_table_documents_crud()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableDocumentsCrudAsync());
		});
	}

	[Test]
	public void Check_table_filters_crud()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableFiltersCrudAsync());
		});
	}

	[Test]
	public void Check_table_messages_crud()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableMessagesCrudAsync());
		});
	}

	[Test]
	public void Check_table_proxies_crud()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableProxiesCrudAsync());
		});
	}

	[Test]
	public void Check_table_sources_crud()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableSourcesCrudAsync());
		});
	}

	[Test]
	public void Check_table_versions_crud()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableVersionsCrudAsync());
		});
	}

	#endregion
}