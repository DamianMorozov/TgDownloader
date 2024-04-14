// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Data.Filtering;
using DevExpress.Xpo;

namespace TgStorage.Domain.Versions;

[DebuggerDisplay("{ToDebugString()}")]
[DoNotNotify]
public sealed class TgXpoVersionRepository(TgXpoContext xpoContext) : TgXpoRepositoryBase<TgXpoVersionEntity>(xpoContext), ITgXpoRepository<TgXpoVersionEntity>
{
	#region Public and private methods

	public override string ToDebugString() => nameof(TgXpoVersionRepository);

	public override async Task<TgXpoOperResult<TgXpoVersionEntity>> GetAsync(TgXpoVersionEntity item)
	{
		TgXpoOperResult<TgXpoVersionEntity> operResult = await base.GetAsync(item);
		if (operResult.IsExist)
			return operResult;
		TgXpoVersionEntity? itemFind = await xpoContext.CreateUnitOfWork()
			.FindObjectAsync<TgXpoVersionEntity>(CriteriaOperator.Parse(
				$"{nameof(TgXpoVersionEntity.Version)}={item.Version}"));
		return itemFind is not null
			? new TgXpoOperResult<TgXpoVersionEntity>(TgEnumEntityState.IsExist, itemFind)
			: new TgXpoOperResult<TgXpoVersionEntity>(TgEnumEntityState.NotExist, item);
	}

	public async Task<TgXpoOperResult<TgXpoVersionEntity>> GetItemLastAsync()
	{
		TgXpoVersionEntity? item = await xpoContext.CreateUnitOfWork()
			       .Query<TgXpoVersionEntity>()
			       .Select(i => i)
			       .OrderByDescending(item => item.Version)
			       .FirstOrDefaultAsync();
		return item is null
			? new TgXpoOperResult<TgXpoVersionEntity>(TgEnumEntityState.NotExist)
			: new TgXpoOperResult<TgXpoVersionEntity>(TgEnumEntityState.IsExist, item);
	}

	#endregion
}