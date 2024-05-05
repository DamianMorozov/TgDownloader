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
		Assert.DoesNotThrow(() =>
		{
			Assert.That(TgEfUtils.CheckTableProxiesCrud(CreateEfContext()));
			Assert.That(TgEfUtils.CheckTableAppsCrud(CreateEfContext()));
		});
	}

	[Test]
	public void Check_table_apps_crud_async()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableProxiesCrudAsync(CreateEfContext()));
			Assert.That(await TgEfUtils.CheckTableAppsCrudAsync(CreateEfContext()));
		});
	}

    [Test]
	public void Check_table_documents_crud()
	{
		Assert.DoesNotThrow(() =>
		{
			Assert.That(TgEfUtils.CheckTableDocumentsCrud(CreateEfContext()));
		});
	}

    [Test]
	public void Check_table_documents_crud_async()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableDocumentsCrudAsync(CreateEfContext()));
		});
	}

	[Test]
	public void Check_table_filters_crud()
	{
		Assert.DoesNotThrow(() =>
		{
			Assert.That(TgEfUtils.CheckTableFiltersCrud(CreateEfContext()));
		});
	}

	[Test]
	public void Check_table_filters_crud_async()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableFiltersCrudAsync(CreateEfContext()));
		});
	}

	[Test]
	public void Check_table_messages_crud()
	{
		Assert.DoesNotThrow(() =>
		{
			Assert.That(TgEfUtils.CheckTableMessagesCrud(CreateEfContext()));
		});
	}

	[Test]
	public void Check_table_messages_crud_async()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableMessagesCrudAsync(CreateEfContext()));
		});
	}

	[Test]
	public void Check_table_proxies_crud()
	{
		Assert.DoesNotThrow(() =>
		{
			Assert.That(TgEfUtils.CheckTableProxiesCrud(CreateEfContext()));
		});
	}

	[Test]
	public void Check_table_proxies_crud_async()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableProxiesCrudAsync(CreateEfContext()));
		});
	}

	[Test]
	public void Check_table_sources_crud()
	{
		Assert.DoesNotThrow(() =>
		{
			Assert.That(TgEfUtils.CheckTableSourcesCrud(CreateEfContext()));
		});
	}

	[Test]
	public void Check_table_sources_crud_async()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableSourcesCrudAsync(CreateEfContext()));
		});
	}

	[Test]
	public void Check_table_versions_crud_async()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			Assert.That(await TgEfUtils.CheckTableVersionsCrudAsync(CreateEfContext()));
		});
	}

	[Test]
	public void Check_table_versions_crud()
	{
		Assert.DoesNotThrow(() =>
		{
			Assert.That(TgEfUtils.CheckTableVersionsCrud(CreateEfContext()));
		});
	}

	#endregion
}