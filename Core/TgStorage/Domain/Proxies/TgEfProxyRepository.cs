// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Proxies;

public sealed class TgEfProxyRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfProxyEntity>(efContext), ITgEfProxyRepository
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfProxyRepository)}";

	public override IQueryable<TgEfProxyEntity> GetQuery(bool isNoTracking) =>
		isNoTracking ? EfContext.Proxies.AsNoTracking() : EfContext.Proxies.AsTracking();

	public override async Task<TgEfStorageResult<TgEfProxyEntity>> GetAsync(TgEfProxyEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfProxyEntity> storageResult = await base.GetAsync(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfProxyEntity? itemFind = await GetQuery(isNoTracking)
			.SingleOrDefaultAsync(x => x.Type == item.Type && x.HostName == item.HostName && x.Port == item.Port);
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfProxyEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfProxyEntity? item = await GetQuery(isNoTracking).FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfProxyEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfProxyEntity> items = take > 0 
			? await GetQuery(isNoTracking).Skip(skip).Take(take).ToListAsync() 
			: await GetQuery(isNoTracking).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfProxyEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfProxyEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfProxyEntity> items = take > 0
			? await GetQuery(isNoTracking).Where(where).Skip(skip).Take(take).ToListAsync()
			: (IList<TgEfProxyEntity>)await GetQuery(isNoTracking).Where(where).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<int> GetCountAsync() => await EfContext.Proxies.AsNoTracking().CountAsync();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfProxyEntity, bool>> where) => 
		await EfContext.Proxies.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Delete

	public override async Task<TgEfStorageResult<TgEfProxyEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfProxyEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfProxyEntity item in storageResult.Items)
			{
				await DeleteAsync(item);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion

	#region Public and private methods - ITgEfProxyRepository

	public async Task<TgEfStorageResult<TgEfProxyEntity>> GetCurrentProxyAsync(TgEfStorageResult<TgEfAppEntity> storageResult)
	{
		if (!storageResult.IsExists)
			return new(TgEnumEntityState.NotExists);

		TgEfStorageResult<TgEfProxyEntity> storageResultProxy = await GetAsync(
			new() { Uid = storageResult.Item.ProxyUid ?? Guid.Empty }, isNoTracking: false);
		return storageResultProxy.IsExists ? storageResultProxy : new(TgEnumEntityState.NotExists);
	}

	public async Task<Guid> GetCurrentProxyUidAsync(TgEfStorageResult<TgEfAppEntity> storageResult) => (await GetCurrentProxyAsync(storageResult)).Item.Uid;

	#endregion
}