// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Microsoft.EntityFrameworkCore;

namespace TgStorage.Domain.Versions;

public sealed class TgEfVersionRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfVersionEntity>(efContext), ITgEfRepository<TgEfVersionEntity>
{
	#region Public and private methods

	public override TgEfOperResult<TgEfVersionEntity> Get(TgEfVersionEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfVersionEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExist)
			return operResult;
		TgEfVersionEntity? itemFind = isNoTracking
			? efContext.Versions.AsNoTracking().SingleOrDefault(
				x => x.Version == item.Version)
			: efContext.Versions.SingleOrDefault(
				x => x.Version == item.Version);
		return itemFind is not null
			? new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.IsExist, itemFind)
			: new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.NotExist, item);
	}

	public override async Task<TgEfOperResult<TgEfVersionEntity>> GetAsync(TgEfVersionEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfVersionEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExist)
			return operResult;
		TgEfVersionEntity? itemFind = isNoTracking
			? await efContext.Versions.AsNoTracking().SingleOrDefaultAsync(
				x => x.Version == item.Version).ConfigureAwait(false)
			: await efContext.Versions.SingleOrDefaultAsync(
				x => x.Version == item.Version).ConfigureAwait(false);
		return itemFind is not null
			? new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.IsExist, itemFind)
			: new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.NotExist, item);
	}

	public override TgEfOperResult<TgEfVersionEntity> GetFirst(bool isNoTracking)
	{
		TgEfVersionEntity? item = isNoTracking
			? EfContext.Versions.AsTracking().FirstOrDefault()
			: EfContext.Versions.FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.NotExist)
			: new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.IsExist, item);
	}

	public override async Task<TgEfOperResult<TgEfVersionEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfVersionEntity? item = isNoTracking
			? await EfContext.Versions.AsTracking().FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Versions.FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.NotExist)
			: new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.IsExist, item);
	}

	public override TgEfOperResult<TgEfVersionEntity> GetEnumerable(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
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

	public override TgEfOperResult<TgEfVersionEntity> GetEnumerable(int count, bool isNoTracking)
	{
		IEnumerable<TgEfVersionEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().Take(count).AsEnumerable()
				: EfContext.Versions.Take(count).AsEnumerable();
		}
		else
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().AsEnumerable()
				: EfContext.Versions.AsEnumerable();
		}
		return new TgEfOperResult<TgEfVersionEntity>(items.Any() ? TgEnumEntityState.IsExist : TgEnumEntityState.NotExist, items);
	}

	public override async Task<TgEfOperResult<TgEfVersionEntity>> GetEnumerableAsync(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
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

	public override async Task<TgEfOperResult<TgEfVersionEntity>> GetEnumerableAsync(int count, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IEnumerable<TgEfVersionEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().Take(count).AsEnumerable()
				: EfContext.Versions.Take(count).AsEnumerable();
		}
		else
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().AsEnumerable()
				: EfContext.Versions.AsEnumerable();
		}
		return new TgEfOperResult<TgEfVersionEntity>(items.Any() ? TgEnumEntityState.IsExist : TgEnumEntityState.NotExist, items);
	}

	public override int GetCount() => EfContext.Versions.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Versions.AsNoTracking().CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfOperResult<TgEfVersionEntity> DeleteAll()
	{
		TgEfOperResult<TgEfVersionEntity> operResult = GetEnumerable(0, isNoTracking: false);
		if (operResult.IsExist)
		{
			foreach (TgEfVersionEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfOperResult<TgEfVersionEntity>(operResult.IsExist ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfVersionEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfVersionEntity> operResult = await GetEnumerableAsync(0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExist)
		{
			foreach (TgEfVersionEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfOperResult<TgEfVersionEntity>(operResult.IsExist ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}