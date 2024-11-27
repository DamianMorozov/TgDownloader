// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Filters;

public sealed class TgEfFilterRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfFilterEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfFilterRepository)}";

	public override TgEfStorageResult<TgEfFilterEntity> Get(TgEfFilterEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfFilterEntity> storageResult = base.Get(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfFilterEntity? itemFind = isNoTracking
			? EfContext.Filters
				.AsNoTracking()
				.SingleOrDefault(x => x.FilterType == item.FilterType && x.Name == item.Name)
			: EfContext.Filters
				.AsTracking()
				.SingleOrDefault(x => x.FilterType == item.FilterType && x.Name == item.Name);
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfFilterEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfFilterEntity>> GetAsync(TgEfFilterEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfFilterEntity> storageResult = await base.GetAsync(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfFilterEntity? itemFind = isNoTracking
			? await EfContext.Filters.AsNoTracking()
				.Where(x => x.FilterType == item.FilterType && x.Name == item.Name)
				.SingleOrDefaultAsync()
			: await EfContext.Filters.AsTracking()
				.Where(x => x.FilterType == item.FilterType && x.Name == item.Name)
				.SingleOrDefaultAsync();
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfFilterEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfStorageResult<TgEfFilterEntity> GetFirst(bool isNoTracking)
	{
		TgEfFilterEntity? item = isNoTracking
			? EfContext.Filters.AsNoTracking()
				.FirstOrDefault()
			: EfContext.Filters.AsTracking()
				.FirstOrDefault();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfFilterEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfFilterEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfFilterEntity? item = isNoTracking
			? await EfContext.Filters.AsNoTracking()
				.FirstOrDefaultAsync()
			: await EfContext.Filters.AsTracking()
				.FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfFilterEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfStorageResult<TgEfFilterEntity> GetList(int take, int skip, bool isNoTracking)
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
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfFilterEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfFilterEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? await EfContext.Filters.AsNoTracking().Skip(skip).Take(take).ToListAsync()
				: await EfContext.Filters.AsTracking().Skip(skip).Take(take).ToListAsync();
		}
		else
		{
			items = isNoTracking
				? await EfContext.Filters.AsNoTracking().ToListAsync()
				: await EfContext.Filters.AsTracking().ToListAsync();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override TgEfStorageResult<TgEfFilterEntity> GetList(int take, int skip, Expression<Func<TgEfFilterEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfFilterEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Filters.AsNoTracking().Where(where).Skip(skip).Take(take).ToList()
				: EfContext.Filters.AsTracking().Where(where).Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Filters.AsNoTracking().Where(where).ToList()
				: EfContext.Filters.AsTracking().Where(where).ToList();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfFilterEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfFilterEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfFilterEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? await EfContext.Filters.AsNoTracking().Where(where).Skip(skip).Take(take).ToListAsync()
				: await EfContext.Filters.AsTracking().Where(where).Skip(skip).Take(take).ToListAsync();
		}
		else
		{
			items = isNoTracking
				? await EfContext.Filters.AsNoTracking().Where(where).ToListAsync()
				: await EfContext.Filters.AsTracking().Where(where).ToListAsync();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Filters.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Filters.AsNoTracking().CountAsync();

	public override int GetCount(Expression<Func<TgEfFilterEntity, bool>> where) => 
		EfContext.Filters.AsNoTracking().Where(where).Count();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfFilterEntity, bool>> where) => 
		await EfContext.Filters.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfStorageResult<TgEfFilterEntity> DeleteAll()
	{
		TgEfStorageResult<TgEfFilterEntity> storageResult = GetList(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfFilterEntity item in storageResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfStorageResult<TgEfFilterEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfFilterEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfFilterEntity item in storageResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}