//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//using DevExpress.Data.Filtering;

//namespace TgStorage.Domain.Sources;

//[DebuggerDisplay("{ToDebugString()}")]
//[DoNotNotify]
//public sealed class TgXpoSourceRepository(TgXpoContext xpoContext) : TgXpoRepositoryBase<TgXpoSourceEntity>(xpoContext), ITgXpoRepository<TgXpoSourceEntity>
//{
//	#region Public and private methods

//	public override string ToDebugString() => nameof(TgXpoSourceRepository);

//	public override async Task<TgXpoOperResult<TgXpoSourceEntity>> GetAsync(TgXpoSourceEntity item)
//	{
//		TgXpoOperResult<TgXpoSourceEntity> operResult = await base.GetAsync(item);
//		if (operResult.IsExist)
//			return operResult;
//		TgXpoSourceEntity? itemFind = await xpoContext.CreateUnitOfWork()
//			.FindObjectAsync<TgXpoSourceEntity>(CriteriaOperator.Parse($"{nameof(TgXpoSourceEntity.Id)}={item.Id}"));
//		return itemFind is not null
//				? new TgXpoOperResult<TgXpoSourceEntity>(TgEnumEntityState.IsExist, itemFind)
//				: new TgXpoOperResult<TgXpoSourceEntity>(TgEnumEntityState.NotExist, item);
//	}

//	public override Task<TgXpoOperResult<TgXpoSourceEntity>> GetNewAsync() => 
//		GetAsync(new TgXpoSourceEntity { DtChanged = DateTime.Now, Directory = TgFileUtils.GetDefaultDirectory() });

//	#endregion
//}