// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Proxies;

public sealed class TgEfProxyRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfProxyEntity>(efContext), ITgEfProxyRepository
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfProxyRepository)}";

	public override TgEfStorageResult<TgEfProxyEntity> Get(TgEfProxyEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfProxyEntity> storageResult = base.Get(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfProxyEntity? itemFind = isNoTracking
			? EfContext.Proxies.AsNoTracking()
				.Where(x => x.Type == item.Type && x.HostName == item.HostName && x.Port == item.Port)
				.SingleOrDefault()
			: EfContext.Proxies.AsTracking()
				.Where(x => x.Type == item.Type && x.HostName == item.HostName && x.Port == item.Port)
				.SingleOrDefault();
		return itemFind is not null
			? new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfProxyEntity>> GetAsync(TgEfProxyEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfProxyEntity> storageResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (storageResult.IsExists)
			return storageResult;
		TgEfProxyEntity? itemFind = isNoTracking
			? await EfContext.Proxies.AsNoTracking()
				.Where(x => x.Type == item.Type && x.HostName == item.HostName && x.Port == item.Port)
				.SingleOrDefaultAsync()
				.ConfigureAwait(false)
			: await EfContext.Proxies.AsTracking()
				.Where(x => x.Type == item.Type && x.HostName == item.HostName && x.Port == item.Port)
				.SingleOrDefaultAsync()
				.ConfigureAwait(false);
		return itemFind is not null
			? new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfStorageResult<TgEfProxyEntity> GetFirst(bool isNoTracking)
	{
		TgEfProxyEntity? item = isNoTracking
			? EfContext.Proxies.AsNoTracking().FirstOrDefault()
			: EfContext.Proxies.AsTracking().FirstOrDefault();
		return item is null
			? new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfProxyEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfProxyEntity? item = isNoTracking
			? await EfContext.Proxies.AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Proxies.AsTracking().FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfStorageResult<TgEfProxyEntity> GetList(int take, int skip, bool isNoTracking)
	{
		IList<TgEfProxyEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking().Skip(skip).Take(take).ToList()
				: EfContext.Proxies.AsTracking().Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking().ToList()
				: EfContext.Proxies.AsTracking().ToList();
		}
		return new TgEfStorageResult<TgEfProxyEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfProxyEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfProxyEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking().Skip(skip).Take(take).ToList()
				: EfContext.Proxies.AsTracking().Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking().ToList()
				: EfContext.Proxies.AsTracking().ToList();
		}
		return new TgEfStorageResult<TgEfProxyEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override TgEfStorageResult<TgEfProxyEntity> GetList(int take, int skip, Expression<Func<TgEfProxyEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfProxyEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList()
				: EfContext.Proxies.AsTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking()
					.Where(where)
					.ToList()
				: EfContext.Proxies.AsTracking()
					.Where(where)
					.ToList();
		}
		return new TgEfStorageResult<TgEfProxyEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfProxyEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfProxyEntity, bool>> where, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfProxyEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList()
				: EfContext.Proxies.AsTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking()
					.Where(where)
					.ToList()
				: EfContext.Proxies.AsTracking()
					.Where(where)
					.ToList();
		}
		return new TgEfStorageResult<TgEfProxyEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Proxies.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Proxies.AsNoTracking().CountAsync().ConfigureAwait(false);

	public override int GetCount(Expression<Func<TgEfProxyEntity, bool>> where) => 
		EfContext.Proxies.AsNoTracking().Where(where).Count();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfProxyEntity, bool>> where) => 
		await EfContext.Proxies.AsNoTracking().Where(where).CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfStorageResult<TgEfProxyEntity> DeleteAll()
	{
		TgEfStorageResult<TgEfProxyEntity> storageResult = GetList(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfProxyEntity item in storageResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfStorageResult<TgEfProxyEntity>(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfStorageResult<TgEfProxyEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfProxyEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false).ConfigureAwait(false);
		if (storageResult.IsExists)
		{
			foreach (TgEfProxyEntity item in storageResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfStorageResult<TgEfProxyEntity>(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion

	#region Public and private methods - ITgEfProxyRepository

	public TgEfStorageResult<TgEfProxyEntity> GetCurrentProxy(TgEfStorageResult<TgEfAppEntity> storageResult)
	{
		if (!storageResult.IsExists)
			return new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.NotExists);

		TgEfStorageResult<TgEfProxyEntity> storageResultProxy = Get(
			new TgEfProxyEntity { Uid = storageResult.Item.ProxyUid ?? Guid.Empty }, isNoTracking: false);
		return storageResultProxy.IsExists ? storageResultProxy : new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.NotExists);
	}

	public async Task<TgEfStorageResult<TgEfProxyEntity>> GetCurrentProxyAsync(TgEfStorageResult<TgEfAppEntity> storageResult)
	{
		if (!storageResult.IsExists)
			return new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.NotExists);

		TgEfStorageResult<TgEfProxyEntity> storageResultProxy = await GetAsync(
			new TgEfProxyEntity { Uid = storageResult.Item.ProxyUid ?? Guid.Empty }, isNoTracking: false);
		return storageResultProxy.IsExists ? storageResultProxy : new TgEfStorageResult<TgEfProxyEntity>(TgEnumEntityState.NotExists);
	}

	public Guid GetCurrentProxyUid(TgEfStorageResult<TgEfAppEntity> storageResult) => GetCurrentProxy(storageResult).Item.Uid;

	public async Task<Guid> GetCurrentProxyUidAsync(TgEfStorageResult<TgEfAppEntity> storageResult) => (await GetCurrentProxyAsync(storageResult)).Item.Uid;

	#endregion
}