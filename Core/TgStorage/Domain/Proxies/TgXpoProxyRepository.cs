// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Data.Filtering;

namespace TgStorage.Domain.Proxies;

[DebuggerDisplay("{ToDebugString()}")]
[DoNotNotify]
public sealed class TgXpoProxyRepository(TgXpoContext xpoContext) : TgXpoRepositoryBase<TgXpoProxyEntity>(xpoContext), ITgXpoRepository<TgXpoProxyEntity>
{
	#region Public and private methods

	public override string ToDebugString() => nameof(TgXpoProxyRepository);

	public override async Task<TgXpoOperResult<TgXpoProxyEntity>> GetAsync(TgXpoProxyEntity item)
	{
		{
			TgXpoOperResult<TgXpoProxyEntity> operResult = await base.GetAsync(item);
			if (operResult.IsExist)
				return operResult;
			TgXpoProxyEntity? itemFind = await xpoContext.CreateUnitOfWork()
				.FindObjectAsync<TgXpoProxyEntity>(CriteriaOperator.Parse(
					$"{nameof(TgXpoProxyEntity.Type)}='{item.Type}' AND {nameof(TgXpoProxyEntity.HostName)}='{item.HostName}' AND " +
					$"{nameof(TgXpoProxyEntity.Port)}={item.Port}"));
			return itemFind is not null
				? new TgXpoOperResult<TgXpoProxyEntity>(TgEnumEntityState.IsExist, itemFind)
				: new TgXpoOperResult<TgXpoProxyEntity>(TgEnumEntityState.NotExist, item);
		}
	}

	public IReadOnlyList<TgEnumProxyType> GetProxyTypes() =>
		Enum.GetValues(typeof(TgEnumProxyType)).Cast<TgEnumProxyType>().ToList();

	#endregion
}