// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest;

[TestFixture]
internal sealed class TgEfContextUpdateTablesTests : TgDbContextTestsBase
{
	#region Public and private methods

	[Test]
	public void Upper_uid_at_apps()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfAppEntity> operResult = await new TgEfAppRepository(TgEfUtils.EfContext).GetFirstAsync(isNoTracking: true);
			if (operResult.IsExists)
			{
				operResult = await CreateEfContext().UpdateTableUidUpperCaseAsync<TgEfAppEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	[Test]
	public void Upper_uid_at_documents()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfDocumentEntity> operResult = await new TgEfDocumentRepository(TgEfUtils.EfContext).GetFirstAsync(isNoTracking: true);
			if (operResult.IsExists)
			{
				operResult = await CreateEfContext().UpdateTableUidUpperCaseAsync<TgEfDocumentEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	[Test]
	public void Upper_uid_at_filters()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfFilterEntity> operResult = await new TgEfFilterRepository(TgEfUtils.EfContext).GetFirstAsync(isNoTracking: true);
			if (operResult.IsExists)
			{
				operResult = await CreateEfContext().UpdateTableUidUpperCaseAsync<TgEfFilterEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	[Test]
	public void Upper_uid_at_messages()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfMessageEntity> operResult = await new TgEfMessageRepository(TgEfUtils.EfContext).GetFirstAsync(isNoTracking: true);
			if (operResult.IsExists)
			{
				operResult = await CreateEfContext().UpdateTableUidUpperCaseAsync<TgEfMessageEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	[Test]
	public void Upper_uid_at_proxies()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfProxyEntity> operResult = await new TgEfProxyRepository(TgEfUtils.EfContext).GetFirstAsync(isNoTracking: true);
			if (operResult.IsExists)
			{
				operResult = await CreateEfContext().UpdateTableUidUpperCaseAsync<TgEfProxyEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	[Test]
	public void Upper_uid_at_sources()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfSourceEntity> operResult = await new TgEfSourceRepository(TgEfUtils.EfContext).GetFirstAsync(isNoTracking: true);
			if (operResult.IsExists)
			{
				operResult = await CreateEfContext().UpdateTableUidUpperCaseAsync<TgEfSourceEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	[Test]
	public void Upper_uid_at_versions()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfVersionEntity> operResult = await new TgEfVersionRepository(TgEfUtils.EfContext).GetFirstAsync(isNoTracking: true);
			if (operResult.IsExists)
			{
				operResult = await CreateEfContext().UpdateTableUidUpperCaseAsync<TgEfVersionEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	#endregion
}