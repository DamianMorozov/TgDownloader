// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Sources;

public sealed class TgEfSourceRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfSourceEntity>(efContext)
{
	#region Public and private methods

	public override TgEfOperResult<TgEfSourceEntity> Get(TgEfSourceEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfSourceEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExists)
			return operResult;
		TgEfSourceEntity? itemFind = isNoTracking
			? EfContext.Sources.AsNoTracking()
				.SingleOrDefault(x => x.Id == item.Id)
			: EfContext.Sources.AsTracking()
				.SingleOrDefault(x => x.Id == item.Id);
		return itemFind is not null
			? new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfOperResult<TgEfSourceEntity>> GetAsync(TgEfSourceEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfSourceEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExists)
			return operResult;
		TgEfSourceEntity? itemFind = isNoTracking
			? await EfContext.Sources.AsNoTracking()
				.SingleOrDefaultAsync(x => x.Id == item.Id)
				.ConfigureAwait(false)
			: await EfContext.Sources.AsTracking()
				.SingleOrDefaultAsync(x => x.Id == item.Id)
				.ConfigureAwait(false);
		return itemFind is not null
			? new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfOperResult<TgEfSourceEntity> GetFirst(bool isNoTracking)
	{
		TgEfSourceEntity? item = isNoTracking
			? EfContext.Sources.AsNoTracking().FirstOrDefault()
			: EfContext.Sources.AsTracking().FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfOperResult<TgEfSourceEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfSourceEntity? item = isNoTracking
			? await EfContext.Sources.AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Sources.AsTracking().FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfOperResult<TgEfSourceEntity> GetList(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
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

	private TgEfOperResult<TgEfSourceEntity> GetList(int count, bool isNoTracking)
	{
		IList<TgEfSourceEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking().Take(count).AsEnumerable().ToList()
				: EfContext.Sources.AsTracking().Take(count).AsEnumerable().ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking().AsEnumerable().ToList()
				: EfContext.Sources.AsTracking().AsEnumerable().ToList();
		}
		return new TgEfOperResult<TgEfSourceEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfSourceEntity>> GetListAsync(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
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

	private async Task<TgEfOperResult<TgEfSourceEntity>> GetListAsync(int count, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfSourceEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking().Take(count).AsEnumerable().ToList()
				: EfContext.Sources.AsTracking().Take(count).AsEnumerable().ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking().AsEnumerable().ToList()
				: EfContext.Sources.AsTracking().AsEnumerable().ToList();
		}
		return new TgEfOperResult<TgEfSourceEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
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
		TgEfOperResult<TgEfSourceEntity> operResult = GetList(0, isNoTracking: false);
		if (operResult.IsExists)
		{
			foreach (TgEfSourceEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfOperResult<TgEfSourceEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfSourceEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfSourceEntity> operResult = await GetListAsync(0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExists)
		{
			foreach (TgEfSourceEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfOperResult<TgEfSourceEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}