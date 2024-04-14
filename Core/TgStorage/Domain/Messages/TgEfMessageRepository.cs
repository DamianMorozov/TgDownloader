// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Microsoft.EntityFrameworkCore;

namespace TgStorage.Domain.Messages;

public sealed class TgEfMessageRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfMessageEntity>(efContext), ITgEfRepository<TgEfMessageEntity>
{
	#region Public and private methods

	public override TgEfOperResult<TgEfMessageEntity> Get(TgEfMessageEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfMessageEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExist)
			return operResult;
		TgEfMessageEntity? itemFind = isNoTracking
				? efContext.Messages.AsNoTracking().Include(x => x.Source).SingleOrDefault(x => x.SourceId == item.SourceId && x.Id == item.Id)
				: efContext.Messages.Include(x => x.Source).SingleOrDefault(x => x.SourceId == item.SourceId && x.Id == item.Id);
		return itemFind is not null
			? new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.IsExist, itemFind)
			: new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.NotExist, item);
	}

	public override async Task<TgEfOperResult<TgEfMessageEntity>> GetAsync(TgEfMessageEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfMessageEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExist)
			return operResult;
		TgEfMessageEntity? itemFind = isNoTracking
			? await efContext.Messages.AsNoTracking().Include(x => x.Source).SingleOrDefaultAsync(x => x.SourceId == item.SourceId && x.Id == item.Id).ConfigureAwait(false)
			: await efContext.Messages.Include(x => x.Source).SingleOrDefaultAsync(x => x.SourceId == item.SourceId && x.Id == item.Id).ConfigureAwait(false);
		return itemFind is not null
			? new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.IsExist, itemFind)
			: new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.NotExist, item);
	}

	public override TgEfOperResult<TgEfMessageEntity> GetFirst(bool isNoTracking)
	{
		TgEfMessageEntity? item = isNoTracking
			? EfContext.Messages.AsTracking().Include(x => x.Source).FirstOrDefault()
			: EfContext.Messages.Include(x => x.Source).FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.NotExist)
			: new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.IsExist, item);
	}

	public override async Task<TgEfOperResult<TgEfMessageEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfMessageEntity? item = isNoTracking
			? await EfContext.Messages.AsTracking().Include(x => x.Source).FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Messages.Include(x => x.Source).FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.NotExist)
			: new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.IsExist, item);
	}

	public override TgEfOperResult<TgEfMessageEntity> GetEnumerable(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
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

	public override TgEfOperResult<TgEfMessageEntity> GetEnumerable(int count, bool isNoTracking)
	{
		IEnumerable<TgEfMessageEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking().Include(x => x.Source).Take(count).AsEnumerable()
				: EfContext.Messages.Include(x => x.Source).Take(count).AsEnumerable();
		}
		else
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking().Include(x => x.Source).AsEnumerable()
				: EfContext.Messages.Include(x => x.Source).AsEnumerable();
		}
		return new TgEfOperResult<TgEfMessageEntity>(items.Any() ? TgEnumEntityState.IsExist : TgEnumEntityState.NotExist, items);
	}

	public override async Task<TgEfOperResult<TgEfMessageEntity>> GetEnumerableAsync(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
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

	public override async Task<TgEfOperResult<TgEfMessageEntity>> GetEnumerableAsync(int count, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IEnumerable<TgEfMessageEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking().Include(x => x.Source).Take(count).AsEnumerable()
				: EfContext.Messages.Include(x => x.Source).Take(count).AsEnumerable();
		}
		else
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking().Include(x => x.Source).AsEnumerable()
				: EfContext.Messages.Include(x => x.Source).AsEnumerable();
		}
		return new TgEfOperResult<TgEfMessageEntity>(items.Any() ? TgEnumEntityState.IsExist : TgEnumEntityState.NotExist, items);
	}

	public override int GetCount() => EfContext.Messages.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Messages.AsNoTracking().CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfOperResult<TgEfMessageEntity> DeleteAll()
	{
		TgEfOperResult<TgEfMessageEntity> operResult = GetEnumerable(0, isNoTracking: false);
		if (operResult.IsExist)
		{
			foreach (TgEfMessageEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfOperResult<TgEfMessageEntity>(operResult.IsExist ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfMessageEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfMessageEntity> operResult = await GetEnumerableAsync(0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExist)
		{
			foreach (TgEfMessageEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfOperResult<TgEfMessageEntity>(operResult.IsExist ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}