// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Proxies;

public sealed class TgEfProxyRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfProxyEntity>(efContext), ITgEfProxyRepository
{
	#region Public and private methods

	public override TgEfOperResult<TgEfProxyEntity> Get(TgEfProxyEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfProxyEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExists)
			return operResult;
		TgEfProxyEntity? itemFind = isNoTracking
			? EfContext.Proxies.AsNoTracking()
				.SingleOrDefault(x => x.Type == item.Type && x.HostName == item.HostName && x.Port == item.Port)
			: EfContext.Proxies.AsTracking()
				.SingleOrDefault(x => x.Type == item.Type && x.HostName == item.HostName && x.Port == item.Port);
		return itemFind is not null
			? new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfOperResult<TgEfProxyEntity>> GetAsync(TgEfProxyEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfProxyEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExists)
			return operResult;
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
			? new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfOperResult<TgEfProxyEntity> GetFirst(bool isNoTracking)
	{
		TgEfProxyEntity? item = isNoTracking
			? EfContext.Proxies.AsNoTracking().FirstOrDefault()
			: EfContext.Proxies.AsTracking().FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfOperResult<TgEfProxyEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfProxyEntity? item = isNoTracking
			? await EfContext.Proxies.AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Proxies.AsTracking().FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfOperResult<TgEfProxyEntity> GetList(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
		topRecords switch
		{
			TgEnumTableTopRecords.Top1 => GetList(1, isNoTracking),
			TgEnumTableTopRecords.Top20 => GetList(20, isNoTracking),
			TgEnumTableTopRecords.Top100 => GetList(200, isNoTracking),
			TgEnumTableTopRecords.Top1000 => GetList(1_000, isNoTracking),
			TgEnumTableTopRecords.Top10000 => GetList(10_000, isNoTracking),
			TgEnumTableTopRecords.Top100000 => GetList(100_000, isNoTracking),
			TgEnumTableTopRecords.Top1000000 => GetList(1_000_000, isNoTracking),
			_ => GetList(0, isNoTracking),
		};

	private TgEfOperResult<TgEfProxyEntity> GetList(int count, bool isNoTracking)
	{
		IList<TgEfProxyEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking().Take(count).AsEnumerable().ToList()
				: EfContext.Proxies.AsTracking().Take(count).AsEnumerable().ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking().AsEnumerable().ToList()
				: EfContext.Proxies.AsTracking().AsEnumerable().ToList();
		}
		return new TgEfOperResult<TgEfProxyEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfProxyEntity>> GetListAsync(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
		topRecords switch
		{
			TgEnumTableTopRecords.Top1 => await GetListAsync(1, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top20 => await GetListAsync(20, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top100 => await GetListAsync(200, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top1000 => await GetListAsync(1_000, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top10000 => await GetListAsync(10_000, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top100000 => await GetListAsync(100_000, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top1000000 => await GetListAsync(1_000_000, isNoTracking).ConfigureAwait(false),
			_ => await GetListAsync(0, isNoTracking).ConfigureAwait(false),
		};

	private async Task<TgEfOperResult<TgEfProxyEntity>> GetListAsync(int count, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfProxyEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking().Take(count).AsEnumerable().ToList()
				: EfContext.Proxies.AsTracking().Take(count).AsEnumerable().ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking().AsEnumerable().ToList()
				: EfContext.Proxies.AsTracking().AsEnumerable().ToList();
		}
		return new TgEfOperResult<TgEfProxyEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Proxies.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Proxies.AsNoTracking().CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfOperResult<TgEfProxyEntity> DeleteAll()
	{
		TgEfOperResult<TgEfProxyEntity> operResult = GetList(0, isNoTracking: false);
		if (operResult.IsExists)
		{
			foreach (TgEfProxyEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfOperResult<TgEfProxyEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfProxyEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfProxyEntity> operResult = await GetListAsync(0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExists)
		{
			foreach (TgEfProxyEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfOperResult<TgEfProxyEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion

	#region Public and private methods

	public TgEfOperResult<TgEfProxyEntity> GetCurrentProxy(TgEfOperResult<TgEfAppEntity> operResult)
	{
		if (!operResult.IsExists)
			return new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExists);

		TgEfOperResult<TgEfProxyEntity> operResultProxy = Get(
			new TgEfProxyEntity { Uid = operResult.Item.ProxyUid ?? Guid.Empty }, isNoTracking: false);
		return operResultProxy.IsExists ? operResultProxy : new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExists);
	}

	public async Task<TgEfOperResult<TgEfProxyEntity>> GetCurrentProxyAsync(TgEfOperResult<TgEfAppEntity> operResult)
	{
		if (!operResult.IsExists)
			return new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExists);

		TgEfOperResult<TgEfProxyEntity> operResultProxy = await GetAsync(
			new TgEfProxyEntity { Uid = operResult.Item.ProxyUid ?? Guid.Empty }, isNoTracking: false);
		return operResultProxy.IsExists ? operResultProxy : new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExists);
	}

	public Guid GetCurrentProxyUid(TgEfOperResult<TgEfAppEntity> operResult) => GetCurrentProxy(operResult).Item.Uid;

	public async Task<Guid> GetCurrentProxyUidAsync(TgEfOperResult<TgEfAppEntity> operResult) => (await GetCurrentProxyAsync(operResult)).Item.Uid;

	#endregion
}