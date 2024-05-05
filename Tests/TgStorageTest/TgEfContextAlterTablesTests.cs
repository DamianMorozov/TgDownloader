// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest;

[TestFixture]
internal sealed class TgEfContextAlterTablesTests : TgDbContextTestsBase
{
	#region Public and private methods

	[Test]
	public void Alter_apps_no_case_uid()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfAppEntity> operResult = await CreateEfContext().AlterTableNoCaseUidAsync<TgEfAppEntity>();
			Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
		});
	}

	[Test]
	public void Alter_documents_no_case_uid()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfDocumentEntity> operResult = await CreateEfContext().AlterTableNoCaseUidAsync<TgEfDocumentEntity>();
			Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
		});
	}

	[Test]
	public void Alter_filters_no_case_uid()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfFilterEntity> operResult = await CreateEfContext().AlterTableNoCaseUidAsync<TgEfFilterEntity>();
			Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
		});
	}

	[Test]
	public void Alter_messages_no_case_uid()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfMessageEntity> operResult = await CreateEfContext().AlterTableNoCaseUidAsync<TgEfMessageEntity>();
			Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
		});
	}

	[Test]
	public void Alter_proxies_no_case_uid()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfProxyEntity> operResult = await CreateEfContext().AlterTableNoCaseUidAsync<TgEfProxyEntity>();
			Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
		});
	}

	[Test]
	public void Alter_sources_no_case_uid()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfSourceEntity> operResult = await CreateEfContext().AlterTableNoCaseUidAsync<TgEfSourceEntity>();
			Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
		});
	}

	[Test]
	public void Alter_versions_no_case_uid()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfVersionEntity> operResult = await CreateEfContext().AlterTableNoCaseUidAsync<TgEfVersionEntity>();
			Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
		});
	}

	#endregion
}