// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Microsoft.EntityFrameworkCore;

namespace TgStorage.Domain.Sources;

public sealed class TgEfSourceRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfSourceEntity>(efContext), ITgEfRepository<TgEfSourceEntity>
{
	#region Public and private methods

	public override TgEfOperResult<TgEfSourceEntity> Get(TgEfSourceEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfSourceEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExist)
			return operResult;
		TgEfSourceEntity? itemFind = isNoTracking
			? efContext.Sources.AsNoTracking().SingleOrDefault(
				x => x.Id == item.Id)
			: efContext.Sources.SingleOrDefault(
				x => x.Id == item.Id);
		return itemFind is not null
			? new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.IsExist, itemFind)
			: new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.NotExist, item);
	}

	public override async Task<TgEfOperResult<TgEfSourceEntity>> GetAsync(TgEfSourceEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfSourceEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExist)
			return operResult;
		TgEfSourceEntity? itemFind = isNoTracking
			? await efContext.Sources.AsNoTracking().SingleOrDefaultAsync(
				x => x.Id == item.Id).ConfigureAwait(false)
			: await efContext.Sources.SingleOrDefaultAsync(
				x => x.Id == item.Id).ConfigureAwait(false);
		return itemFind is not null
			? new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.IsExist, itemFind)
			: new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.NotExist, item);
	}

	public override TgEfOperResult<TgEfSourceEntity> GetFirst(bool isNoTracking)
	{
		TgEfSourceEntity? item = isNoTracking
			? EfContext.Sources.AsTracking().FirstOrDefault()
			: EfContext.Sources.FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.NotExist)
			: new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.IsExist, item);
	}

	public override async Task<TgEfOperResult<TgEfSourceEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfSourceEntity? item = isNoTracking
			? await EfContext.Sources.AsTracking().FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Sources.FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.NotExist)
			: new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.IsExist, item);
	}

	public override TgEfOperResult<TgEfSourceEntity> GetEnumerable(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
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

	public override TgEfOperResult<TgEfSourceEntity> GetEnumerable(int count, bool isNoTracking)
	{
		IEnumerable<TgEfSourceEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking().Take(count).AsEnumerable()
				: EfContext.Sources.Take(count).AsEnumerable();
		}
		else
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking().AsEnumerable()
				: EfContext.Sources.AsEnumerable();
		}
		return new TgEfOperResult<TgEfSourceEntity>(items.Any() ? TgEnumEntityState.IsExist : TgEnumEntityState.NotExist, items);
	}

	public override async Task<TgEfOperResult<TgEfSourceEntity>> GetEnumerableAsync(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
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

	public override async Task<TgEfOperResult<TgEfSourceEntity>> GetEnumerableAsync(int count, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IEnumerable<TgEfSourceEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking().Take(count).AsEnumerable()
				: EfContext.Sources.Take(count).AsEnumerable();
		}
		else
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking().AsEnumerable()
				: EfContext.Sources.AsEnumerable();
		}
		return new TgEfOperResult<TgEfSourceEntity>(items.Any() ? TgEnumEntityState.IsExist : TgEnumEntityState.NotExist, items);
	}

	public override int GetCount() => EfContext.Sources.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Sources.AsNoTracking().CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfOperResult<TgEfSourceEntity> DeleteAll()
	{
		TgEfOperResult<TgEfSourceEntity> operResult = GetEnumerable(0, isNoTracking: false);
		if (operResult.IsExist)
		{
			foreach (TgEfSourceEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfOperResult<TgEfSourceEntity>(operResult.IsExist ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfSourceEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfSourceEntity> operResult = await GetEnumerableAsync(0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExist)
		{
			foreach (TgEfSourceEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfOperResult<TgEfSourceEntity>(operResult.IsExist ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}