// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorageTest;

[TestFixture]
internal class TgEfContextUpdateTablesTests : TgDbContextTestsBase
{
	#region Public and private methods

	[Test]
	public void Upper_uid_at_apps()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfAppEntity> operResult = await EfProdContext.AppRepository.GetFirstAsync(isNoTracking: true);
			if (operResult.IsExist)
			{
				operResult = await EfProdContext.UpdateTableUidUpperCaseAsync<TgEfAppEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	[Test]
	public void Upper_uid_at_documents()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfDocumentEntity> operResult = await EfProdContext.DocumentRepository.GetFirstAsync(isNoTracking: true);
			if (operResult.IsExist)
			{
				operResult = await EfProdContext.UpdateTableUidUpperCaseAsync<TgEfDocumentEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	[Test]
	public void Upper_uid_at_filters()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfFilterEntity> operResult = await EfProdContext.FilterRepository.GetFirstAsync(isNoTracking: true);
			if (operResult.IsExist)
			{
				operResult = await EfProdContext.UpdateTableUidUpperCaseAsync<TgEfFilterEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	[Test]
	public void Upper_uid_at_messages()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfMessageEntity> operResult = await EfProdContext.MessageRepository.GetFirstAsync(isNoTracking: true);
			if (operResult.IsExist)
			{
				operResult = await EfProdContext.UpdateTableUidUpperCaseAsync<TgEfMessageEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	[Test]
	public void Upper_uid_at_proxies()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfProxyEntity> operResult = await EfProdContext.ProxyRepository.GetFirstAsync(isNoTracking: true);
			if (operResult.IsExist)
			{
				operResult = await EfProdContext.UpdateTableUidUpperCaseAsync<TgEfProxyEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	[Test]
	public void Upper_uid_at_sources()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfSourceEntity> operResult = await EfProdContext.SourceRepository.GetFirstAsync(isNoTracking: true);
			if (operResult.IsExist)
			{
				operResult = await EfProdContext.UpdateTableUidUpperCaseAsync<TgEfSourceEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	[Test]
	public void Upper_uid_at_versions()
	{
		Assert.DoesNotThrowAsync(async () =>
		{
			TgEfOperResult<TgEfVersionEntity> operResult = await EfProdContext.VersionRepository.GetFirstAsync(isNoTracking: true);
			if (operResult.IsExist)
			{
				operResult = await EfProdContext.UpdateTableUidUpperCaseAsync<TgEfVersionEntity>(operResult.Item.Uid);
				Assert.That(operResult.State == TgEnumEntityState.IsExecuted);
			}
		});
	}

	#endregion
}