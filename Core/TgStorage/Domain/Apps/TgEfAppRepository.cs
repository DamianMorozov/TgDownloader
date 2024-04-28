// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Apps;

public sealed class TgEfAppRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfAppEntity>(efContext)
{
	#region Public and private methods

	public override TgEfOperResult<TgEfAppEntity> Get(TgEfAppEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfAppEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExists)
			return operResult;
		TgEfAppEntity? itemFind = isNoTracking
			? EfContext.Apps.AsNoTracking()
				.Include(x => x.Proxy)
				.DefaultIfEmpty()
				.SingleOrDefault(x => x.ApiHash == item.ApiHash)
			: EfContext.Apps
				.Include(x => x.Proxy)
				.DefaultIfEmpty()
				.SingleOrDefault(x => x.ApiHash == item.ApiHash);
		return itemFind is not null
			? new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfOperResult<TgEfAppEntity>> GetAsync(TgEfAppEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfAppEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExists)
			return operResult;
		TgEfAppEntity? itemFind = isNoTracking
			? await EfContext.Apps.AsNoTracking()
				.Include(x => x.Proxy)
				.DefaultIfEmpty()
				.SingleOrDefaultAsync(x => x.ApiHash == item.ApiHash)
				.ConfigureAwait(false)
			: await EfContext.Apps
				.Include(x => x.Proxy)
				.DefaultIfEmpty()
				.SingleOrDefaultAsync(x => x.ApiHash == item.ApiHash)
				.ConfigureAwait(false);
		return itemFind is not null
			? new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfOperResult<TgEfAppEntity> GetFirst(bool isNoTracking)
	{
		TgEfAppEntity? item = isNoTracking
			? EfContext.Apps.AsTracking().Include(x => x.Proxy).FirstOrDefault()
			: EfContext.Apps.Include(x => x.Proxy).FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfOperResult<TgEfAppEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfAppEntity? item = isNoTracking
			? await EfContext.Apps.AsTracking().Include(x => x.Proxy).FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Apps.Include(x => x.Proxy).FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfOperResult<TgEfAppEntity> GetEnumerable(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
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

	public override TgEfOperResult<TgEfAppEntity> GetEnumerable(int count, bool isNoTracking)
	{
		IEnumerable<TgEfAppEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Apps.AsNoTracking().Include(x => x.Proxy).Take(count).AsEnumerable()
				: EfContext.Apps.Include(x => x.Proxy).Take(count).AsEnumerable();
		}
		else
		{
			items = isNoTracking
				? EfContext.Apps.AsNoTracking().Include(x => x.Proxy).AsEnumerable()
				: EfContext.Apps.Include(x => x.Proxy).AsEnumerable();
		}
		return new TgEfOperResult<TgEfAppEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfAppEntity>> GetEnumerableAsync(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
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

	public override async Task<TgEfOperResult<TgEfAppEntity>> GetEnumerableAsync(int count, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IEnumerable<TgEfAppEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Apps.AsNoTracking().Include(x => x.Proxy).Take(count).AsEnumerable()
				: EfContext.Apps.Include(x => x.Proxy).Take(count).AsEnumerable();
		}
		else
		{
			items = isNoTracking
				? EfContext.Apps.AsNoTracking().Include(x => x.Proxy).AsEnumerable()
				: EfContext.Apps.Include(x => x.Proxy).AsEnumerable();
		}
		return new TgEfOperResult<TgEfAppEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Apps.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Apps.AsNoTracking().CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfOperResult<TgEfAppEntity> DeleteAll()
	{
		TgEfOperResult<TgEfAppEntity> operResult = GetEnumerable(0, isNoTracking: false);
		if (operResult.IsExists)
		{
			foreach (TgEfAppEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfOperResult<TgEfAppEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfAppEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfAppEntity> operResult = await GetEnumerableAsync(0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExists)
		{
			foreach (TgEfAppEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfOperResult<TgEfAppEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}