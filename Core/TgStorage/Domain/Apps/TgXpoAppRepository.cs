// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Data.Filtering;

namespace TgStorage.Domain.Apps;

[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgXpoAppRepository(TgXpoContext xpoContext) : TgXpoRepositoryBase<TgXpoAppEntity>(xpoContext), ITgXpoRepository<TgXpoAppEntity>
{
	#region Public and private methods

	public override string ToDebugString() => nameof(TgXpoAppRepository);

	public override async Task<TgXpoOperResult<TgXpoAppEntity>> GetAsync(TgXpoAppEntity item)
	{
		TgXpoOperResult<TgXpoAppEntity> operResult = await base.GetAsync(item);
		if (operResult.IsExist)
			return operResult;
		TgXpoAppEntity? itemFind = await xpoContext.CreateUnitOfWork()
			.FindObjectAsync<TgXpoAppEntity>(CriteriaOperator.Parse(
			$"{nameof(TgXpoAppEntity.ApiHash)}='{item.ApiHash}'"));
		return itemFind is not null
			? new TgXpoOperResult<TgXpoAppEntity>(TgEnumEntityState.IsExist, itemFind)
			: new TgXpoOperResult<TgXpoAppEntity>(TgEnumEntityState.NotExist, item);
	}

	#endregion
}