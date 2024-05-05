// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Versions;

public sealed class TgEfVersionRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfVersionEntity>(efContext)
{
	#region Public and private methods

	public override TgEfOperResult<TgEfVersionEntity> Get(TgEfVersionEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfVersionEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExists)
			return operResult;
		TgEfVersionEntity? itemFind = isNoTracking
			? EfContext.Versions.AsNoTracking()
				.SingleOrDefault(x => x.Version == item.Version)
			: EfContext.Versions.AsTracking()
				.SingleOrDefault(x => x.Version == item.Version);
		return itemFind is not null
			? new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfOperResult<TgEfVersionEntity>> GetAsync(TgEfVersionEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfVersionEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExists)
			return operResult;
		TgEfVersionEntity? itemFind = isNoTracking
			? await EfContext.Versions.AsNoTracking()
				.SingleOrDefaultAsync(x => x.Version == item.Version)
				.ConfigureAwait(false)
			: await EfContext.Versions.AsTracking()
				.SingleOrDefaultAsync(x => x.Version == item.Version)
				.ConfigureAwait(false);
		return itemFind is not null
			? new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfOperResult<TgEfVersionEntity> GetFirst(bool isNoTracking)
	{
		TgEfVersionEntity? item = isNoTracking
			? EfContext.Versions.AsNoTracking().FirstOrDefault()
			: EfContext.Versions.AsTracking().FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfOperResult<TgEfVersionEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfVersionEntity? item = isNoTracking
			? await EfContext.Versions.AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Versions.AsTracking().FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfOperResult<TgEfVersionEntity>GetList(TgEnumTableTopRecords topRecords, bool isNoTracking) => 
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

	private TgEfOperResult<TgEfVersionEntity> GetList(int count, bool isNoTracking)
	{
		IList<TgEfVersionEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().Take(count).AsEnumerable().ToList()
				: EfContext.Versions.AsTracking().Take(count).AsEnumerable().ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().AsEnumerable().ToList()
				: EfContext.Versions.AsTracking().AsEnumerable().ToList();
		}

		return new TgEfOperResult<TgEfVersionEntity>(
			items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfVersionEntity>> GetListAsync(TgEnumTableTopRecords topRecords,
		bool isNoTracking) => topRecords switch
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

	private async Task<TgEfOperResult<TgEfVersionEntity>> GetListAsync(int count, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfVersionEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().Take(count).AsEnumerable().ToList()
				: EfContext.Versions.Take(count).AsEnumerable().ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().AsEnumerable().ToList()
				: EfContext.Versions.AsTracking().AsEnumerable().ToList();
		}

		return new TgEfOperResult<TgEfVersionEntity>(
			items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Versions.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() =>
		await EfContext.Versions.AsNoTracking().CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfOperResult<TgEfVersionEntity> DeleteAll()
	{
		TgEfOperResult<TgEfVersionEntity> operResult = GetList(0, isNoTracking: false);
		if (operResult.IsExists)
		{
			foreach (TgEfVersionEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}

		return new TgEfOperResult<TgEfVersionEntity>(operResult.IsExists
			? TgEnumEntityState.IsDeleted
			: TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfVersionEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfVersionEntity> operResult =
			await GetListAsync(0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExists)
		{
			foreach (TgEfVersionEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}

		return new TgEfOperResult<TgEfVersionEntity>(operResult.IsExists
			? TgEnumEntityState.IsDeleted
			: TgEnumEntityState.NotDeleted);
	}

	#endregion
}