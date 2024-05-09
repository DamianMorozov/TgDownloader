//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//namespace TgStorageTest;

//[TestFixture]
//internal sealed class TgEfContextAlterTablesTests : TgDbContextTestsBase
//{
//	#region Public and private methods

//	[Test]
//	public void Alter_apps_no_case_uid()
//	{
//		Assert.DoesNotThrow(() =>
//		{
//			TgEfContext efContext = CreateEfContext();
//			TgEfOperResult<TgEfAppEntity> operResultGet = new TgEfAppRepository(efContext).GetList(TgEnumTableTopRecords.All, isNoTracking: true);
//			TgEfOperResult<TgEfAppEntity> operResult = efContext.AlterTableNoCaseUid<TgEfAppEntity>();
//			Assert.That(operResultGet.IsExists ? operResult.State == TgEnumEntityState.IsExecuted : operResult.State == TgEnumEntityState.NotExecuted);
//		});
//	}

//	[Test]
//	public void Alter_documents_no_case_uid()
//	{
//		Assert.DoesNotThrow(() =>
//		{
//			TgEfContext efContext = CreateEfContext();
//			TgEfOperResult<TgEfDocumentEntity> operResultGet = new TgEfDocumentRepository(efContext).GetList(TgEnumTableTopRecords.All, isNoTracking: true);
//			TgEfOperResult<TgEfDocumentEntity> operResult = efContext.AlterTableNoCaseUid<TgEfDocumentEntity>();
//			Assert.That(operResultGet.IsExists ? operResult.State == TgEnumEntityState.IsExecuted : operResult.State == TgEnumEntityState.NotExecuted);
//		});
//	}

//	[Test]
//	public void Alter_filters_no_case_uid()
//	{
//		Assert.DoesNotThrow(() =>
//		{
//			TgEfContext efContext = CreateEfContext();
//			TgEfOperResult<TgEfFilterEntity> operResultGet = new TgEfFilterRepository(efContext).GetList(TgEnumTableTopRecords.All, isNoTracking: true);
//			TgEfOperResult<TgEfFilterEntity> operResult = efContext.AlterTableNoCaseUid<TgEfFilterEntity>();
//			Assert.That(operResultGet.IsExists ? operResult.State == TgEnumEntityState.IsExecuted : operResult.State == TgEnumEntityState.NotExecuted);
//		});
//	}

//	[Test]
//	public void Alter_messages_no_case_uid()
//	{
//		Assert.DoesNotThrow(() =>
//		{
//			TgEfContext efContext = CreateEfContext();
//			TgEfOperResult<TgEfMessageEntity> operResultGet = new TgEfMessageRepository(efContext).GetList(TgEnumTableTopRecords.All, isNoTracking: true);
//			TgEfOperResult<TgEfMessageEntity> operResult = efContext.AlterTableNoCaseUid<TgEfMessageEntity>();
//			Assert.That(operResultGet.IsExists ? operResult.State == TgEnumEntityState.IsExecuted : operResult.State == TgEnumEntityState.NotExecuted);
//		});
//	}

//	[Test]
//	public void Alter_proxies_no_case_uid()
//	{
//		Assert.DoesNotThrow(() =>
//		{
//			TgEfContext efContext = CreateEfContext();
//			TgEfOperResult<TgEfProxyEntity> operResultGet = new TgEfProxyRepository(efContext).GetList(TgEnumTableTopRecords.All, isNoTracking: true);
//			TgEfOperResult<TgEfProxyEntity> operResult = efContext.AlterTableNoCaseUid<TgEfProxyEntity>();
//			Assert.That(operResultGet.IsExists ? operResult.State == TgEnumEntityState.IsExecuted : operResult.State == TgEnumEntityState.NotExecuted);
//		});
//	}

//	[Test]
//	public void Alter_sources_no_case_uid()
//	{
//		Assert.DoesNotThrow(() =>
//		{
//			TgEfContext efContext = CreateEfContext();
//			TgEfOperResult<TgEfSourceEntity> operResultGet = new TgEfSourceRepository(efContext).GetList(TgEnumTableTopRecords.All, isNoTracking: true);
//			TgEfOperResult<TgEfSourceEntity> operResult = efContext.AlterTableNoCaseUid<TgEfSourceEntity>();
//			Assert.That(operResultGet.IsExists ? operResult.State == TgEnumEntityState.IsExecuted : operResult.State == TgEnumEntityState.NotExecuted);
//		});
//	}

//	[Test]
//	public void Alter_versions_no_case_uid()
//	{
//		Assert.DoesNotThrow(() =>
//		{
//			TgEfContext efContext = CreateEfContext();
//			TgEfOperResult<TgEfVersionEntity> operResultGet = new TgEfVersionRepository(efContext).GetList(TgEnumTableTopRecords.All, isNoTracking: true);
//			TgEfOperResult<TgEfVersionEntity> operResult = efContext.AlterTableNoCaseUid<TgEfVersionEntity>();
//			Assert.That(operResultGet.IsExists ? operResult.State == TgEnumEntityState.IsExecuted : operResult.State == TgEnumEntityState.NotExecuted);
//		});
//	}

//	#endregion
//}