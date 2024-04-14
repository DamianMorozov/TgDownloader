// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Data.Filtering;

namespace TgStorage.Domain.Filters;

[DebuggerDisplay("{ToDebugString()}")]
[DoNotNotify]
public sealed class TgXpoFilterRepository(TgXpoContext xpoContext) : TgXpoRepositoryBase<TgXpoFilterEntity>(xpoContext), ITgXpoRepository<TgXpoFilterEntity>
{
	#region Public and private methods

	public override string ToDebugString() => nameof(TgXpoFilterRepository);

	public override async Task<TgXpoOperResult<TgXpoFilterEntity>> GetAsync(TgXpoFilterEntity item)
	{
		TgXpoOperResult<TgXpoFilterEntity> operResult = await base.GetAsync(item);
		if (operResult.IsExist)
			return operResult;
		TgXpoFilterEntity? itemFind = await xpoContext.CreateUnitOfWork()
			.FindObjectAsync<TgXpoFilterEntity>(CriteriaOperator.Parse(
			$"{nameof(TgXpoFilterEntity.FilterType)}='{item.FilterType}' AND " +
			$"{nameof(TgXpoFilterEntity.Name)}='{item.Name}'"));
		return itemFind is not null
			? new TgXpoOperResult<TgXpoFilterEntity>(TgEnumEntityState.IsExist, itemFind)
			: new TgXpoOperResult<TgXpoFilterEntity>(TgEnumEntityState.NotExist, item);
	}

	public Task<TgXpoOperResult<TgXpoFilterEntity>> GetEnumerableEnabledAsync() => 
		GetEnumerableAsync(item => item.IsEnabled);

	#endregion
}