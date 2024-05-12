// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Filters;

public sealed class TgEfFilterRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfFilterEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfFilterRepository)}";

	public override TgEfOperResult<TgEfFilterEntity> Get(TgEfFilterEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfFilterEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExists)
			return operResult;
		TgEfFilterEntity? itemFind = isNoTracking
			? EfContext.Filters.AsNoTracking()
				.SingleOrDefault(x => x.FilterType == item.FilterType && x.Name == item.Name)
			: EfContext.Filters.AsTracking()
				.SingleOrDefault(x => x.FilterType == item.FilterType && x.Name == item.Name);
		return itemFind is not null
			? new TgEfOperResult<TgEfFilterEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfFilterEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfOperResult<TgEfFilterEntity>> GetAsync(TgEfFilterEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfFilterEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExists)
			return operResult;
		TgEfFilterEntity? itemFind = isNoTracking
			? await EfContext.Filters.AsNoTracking()
				.Where(x => x.FilterType == item.FilterType && x.Name == item.Name)
				.SingleOrDefaultAsync()
				.ConfigureAwait(false)
			: await EfContext.Filters.AsTracking()
				.Where(x => x.FilterType == item.FilterType && x.Name == item.Name)
				.SingleOrDefaultAsync()
				.ConfigureAwait(false);
		return itemFind is not null
			? new TgEfOperResult<TgEfFilterEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfFilterEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfOperResult<TgEfFilterEntity> GetFirst(bool isNoTracking)
	{
		TgEfFilterEntity? item = isNoTracking
			? EfContext.Filters.AsNoTracking()
				.FirstOrDefault()
			: EfContext.Filters.AsTracking()
				.FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfFilterEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfFilterEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfOperResult<TgEfFilterEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfFilterEntity? item = isNoTracking
			? await EfContext.Filters.AsNoTracking()
				.FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Filters.AsTracking()
				.FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfFilterEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfFilterEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfOperResult<TgEfFilterEntity> GetList(int take, int skip, bool isNoTracking)
	{
		IList<TgEfFilterEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Filters.AsNoTracking()
					.Skip(skip).Take(take).ToList()
				: EfContext.Filters.AsTracking()
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Filters.AsNoTracking()
					.ToList()
				: EfContext.Filters.AsTracking()
					.ToList();
		}
		return new TgEfOperResult<TgEfFilterEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfFilterEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfFilterEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Filters.AsNoTracking().Skip(skip).Take(take).ToList()
				: EfContext.Filters.AsTracking().Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Filters.AsNoTracking().ToList()
				: EfContext.Filters.AsTracking().ToList();
		}
		return new TgEfOperResult<TgEfFilterEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override TgEfOperResult<TgEfFilterEntity> GetList(int take, int skip, Expression<Func<TgEfFilterEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfFilterEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Filters.AsNoTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList()
				: EfContext.Filters.AsTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Filters.AsNoTracking()
					.Where(where)
					.ToList()
				: EfContext.Filters.AsTracking()
					.Where(where)
					.ToList();
		}
		return new TgEfOperResult<TgEfFilterEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfFilterEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfFilterEntity, bool>> where, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfFilterEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Filters.AsNoTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList()
				: EfContext.Filters.AsTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Filters.AsNoTracking()
					.Where(where)
					.ToList()
				: EfContext.Filters.AsTracking()
					.Where(where)
					.ToList();
		}
		return new TgEfOperResult<TgEfFilterEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Filters.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Filters.AsNoTracking().CountAsync().ConfigureAwait(false);

	public override int GetCount(Expression<Func<TgEfFilterEntity, bool>> where) => 
		EfContext.Filters.AsNoTracking().Where(where).Count();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfFilterEntity, bool>> where) => 
		await EfContext.Filters.AsNoTracking().Where(where).CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfOperResult<TgEfFilterEntity> DeleteAll()
	{
		TgEfOperResult<TgEfFilterEntity> operResult = GetList(0, 0, isNoTracking: false);
		if (operResult.IsExists)
		{
			foreach (TgEfFilterEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfOperResult<TgEfFilterEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfFilterEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfFilterEntity> operResult = await GetListAsync(0, 0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExists)
		{
			foreach (TgEfFilterEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfOperResult<TgEfFilterEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}