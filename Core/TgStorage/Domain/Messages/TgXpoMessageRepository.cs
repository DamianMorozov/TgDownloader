//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//using DevExpress.Data.Filtering;

//namespace TgStorage.Domain.Messages;

//[DebuggerDisplay("{ToDebugString()}")]
//[DoNotNotify]
//public sealed class TgXpoMessageRepository(TgXpoContext xpoContext) : TgXpoRepositoryBase<TgXpoMessageEntity>(xpoContext), ITgXpoRepository<TgXpoMessageEntity>
//{
//	#region Public and private methods

//	public override string ToDebugString() => nameof(TgXpoMessageRepository);

//	public override async Task<TgXpoOperResult<TgXpoMessageEntity>> GetAsync(TgXpoMessageEntity item)
//	{
//		TgXpoOperResult<TgXpoMessageEntity> operResult = await base.GetAsync(item);
//		if (operResult.IsExist)
//			return operResult;
//		TgXpoMessageEntity? itemFind = await xpoContext.CreateUnitOfWork()
//			.FindObjectAsync<TgXpoMessageEntity>(CriteriaOperator.Parse(
//				$"{nameof(TgXpoMessageEntity.SourceId)}={item.SourceId} AND " +
//				$"{nameof(TgXpoMessageEntity.Id)}={item.Id}"));
//		return itemFind is not null
//			? new TgXpoOperResult<TgXpoMessageEntity>(TgEnumEntityState.IsExist, itemFind)
//			: new TgXpoOperResult<TgXpoMessageEntity>(TgEnumEntityState.NotExist, item);
//	}

//	public override Task<TgXpoOperResult<TgXpoMessageEntity>> GetNewAsync() => 
//		GetAsync(new TgXpoMessageEntity { DtCreated = DateTime.Now });

//	public async Task<bool> GetExistsAsync(TgXpoMessageEntity item) => (await GetAsync(item)).IsExist;

//	#endregion
//}