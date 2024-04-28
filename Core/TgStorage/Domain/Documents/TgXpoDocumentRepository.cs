//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//using DevExpress.Data.Filtering;

//namespace TgStorage.Domain.Documents;

//[DebuggerDisplay("{ToDebugString()}")]
//[DoNotNotify]
//public sealed class TgXpoDocumentRepository(TgXpoContext xpoContext) : TgXpoRepositoryBase<TgXpoDocumentEntity>(xpoContext), ITgXpoRepository<TgXpoDocumentEntity>
//{
//	#region Public and private methods

//	public override string ToDebugString() => nameof(TgXpoDocumentRepository);

//	public override async Task<TgXpoOperResult<TgXpoDocumentEntity>> GetAsync(TgXpoDocumentEntity item)
//	{
//		TgXpoOperResult<TgXpoDocumentEntity> operResult = await base.GetAsync(item);
//		if (operResult.IsExist)
//			return operResult;
//		TgXpoDocumentEntity? itemFind = await xpoContext.CreateUnitOfWork()
//			.FindObjectAsync<TgXpoDocumentEntity>(CriteriaOperator.Parse(
//				$"{nameof(TgXpoDocumentEntity.SourceId)}={item.SourceId} AND {nameof(TgXpoDocumentEntity.Id)}={item.Id} AND " +
//				$"{nameof(TgXpoDocumentEntity.MessageId)}={item.MessageId}"));
//		return itemFind is not null
//			? new TgXpoOperResult<TgXpoDocumentEntity>(TgEnumEntityState.IsExist, itemFind) 
//			: new TgXpoOperResult<TgXpoDocumentEntity>(TgEnumEntityState.NotExist, item);
//	}

//	#endregion
//}