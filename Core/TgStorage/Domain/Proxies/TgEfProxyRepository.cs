// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Microsoft.EntityFrameworkCore;

namespace TgStorage.Domain.Proxies;

public sealed class TgEfProxyRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfProxyEntity>(efContext), ITgEfRepository<TgEfProxyEntity>
{
	#region Public and private methods

	public override TgEfOperResult<TgEfProxyEntity> Get(TgEfProxyEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfProxyEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExist)
			return operResult;
		TgEfProxyEntity? itemFind = isNoTracking
			? efContext.Proxies.AsNoTracking().SingleOrDefault(
				x => x.Type == item.Type && x.HostName == item.HostName && x.Port == item.Port)
			: efContext.Proxies.SingleOrDefault(
				x => x.Type == item.Type && x.HostName == item.HostName && x.Port == item.Port);
		return itemFind is not null
			? new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.IsExist, itemFind)
			: new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExist, item);
	}

	public override async Task<TgEfOperResult<TgEfProxyEntity>> GetAsync(TgEfProxyEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfProxyEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExist)
			return operResult;
		TgEfProxyEntity? itemFind = isNoTracking
			? await efContext.Proxies.AsNoTracking().SingleOrDefaultAsync(
				x => x.Type == item.Type && x.HostName == item.HostName && x.Port == item.Port).ConfigureAwait(false)
			: await efContext.Proxies.SingleOrDefaultAsync(
				x => x.Type == item.Type && x.HostName == item.HostName && x.Port == item.Port).ConfigureAwait(false);
		return itemFind is not null
			? new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.IsExist, itemFind)
			: new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExist, item);
	}

	public override TgEfOperResult<TgEfProxyEntity> GetFirst(bool isNoTracking)
	{
		TgEfProxyEntity? item = isNoTracking
			? EfContext.Proxies.AsTracking().FirstOrDefault()
			: EfContext.Proxies.FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExist)
			: new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.IsExist, item);
	}

	public override async Task<TgEfOperResult<TgEfProxyEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfProxyEntity? item = isNoTracking
			? await EfContext.Proxies.AsTracking().FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Proxies.FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.NotExist)
			: new TgEfOperResult<TgEfProxyEntity>(TgEnumEntityState.IsExist, item);
	}

	public override TgEfOperResult<TgEfProxyEntity> GetEnumerable(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
		topRecords switch
		{
			TgEnumTableTopRecords.Top1 => GetEnumerable(1, isNoTracking),
			TgEnumTableTopRecords.Top20 => GetEnumerable(20, isNoTracking),
			TgEnumTableTopRecords.Top100 => GetEnumerable(200, isNoTracking),
			TgEnumTableTopRecords.Top1000 => GetEnumerable(1_000, isNoTracking),
			TgEnumTableTopRecords.Top10000 => GetEnumerable(10_000, isNoTracking),
			TgEnumTableTopRecords.Top100000 => GetEnumerable(100_000, isNoTracking),
			TgEnumTableTopRecords.Top1000000 => GetEnumerable(1_000_000, isNoTracking),
			_ => GetEnumerable(0, isNoTracking),
		};

	public override TgEfOperResult<TgEfProxyEntity> GetEnumerable(int count, bool isNoTracking)
	{
		IEnumerable<TgEfProxyEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking().Take(count).AsEnumerable()
				: EfContext.Proxies.Take(count).AsEnumerable();
		}
		else
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking().AsEnumerable()
				: EfContext.Proxies.AsEnumerable();
		}
		return new TgEfOperResult<TgEfProxyEntity>(items.Any() ? TgEnumEntityState.IsExist : TgEnumEntityState.NotExist, items);
	}

	public override async Task<TgEfOperResult<TgEfProxyEntity>> GetEnumerableAsync(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
		topRecords switch
		{
			TgEnumTableTopRecords.Top1 => await GetEnumerableAsync(1, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top20 => await GetEnumerableAsync(20, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top100 => await GetEnumerableAsync(200, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top1000 => await GetEnumerableAsync(1_000, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top10000 => await GetEnumerableAsync(10_000, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top100000 => await GetEnumerableAsync(100_000, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top1000000 => await GetEnumerableAsync(1_000_000, isNoTracking).ConfigureAwait(false),
			_ => await GetEnumerableAsync(0, isNoTracking).ConfigureAwait(false),
		};

	public override async Task<TgEfOperResult<TgEfProxyEntity>> GetEnumerableAsync(int count, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IEnumerable<TgEfProxyEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking().Take(count).AsEnumerable()
				: EfContext.Proxies.Take(count).AsEnumerable();
		}
		else
		{
			items = isNoTracking
				? EfContext.Proxies.AsNoTracking().AsEnumerable()
				: EfContext.Proxies.AsEnumerable();
		}
		return new TgEfOperResult<TgEfProxyEntity>(items.Any() ? TgEnumEntityState.IsExist : TgEnumEntityState.NotExist, items);
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
		TgEfOperResult<TgEfProxyEntity> operResult = GetEnumerable(0, isNoTracking: false);
		if (operResult.IsExist)
		{
			foreach (TgEfProxyEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfOperResult<TgEfProxyEntity>(operResult.IsExist ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfProxyEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfProxyEntity> operResult = await GetEnumerableAsync(0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExist)
		{
			foreach (TgEfProxyEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfOperResult<TgEfProxyEntity>(operResult.IsExist ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}