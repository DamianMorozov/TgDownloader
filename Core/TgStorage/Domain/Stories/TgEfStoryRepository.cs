// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Stories;

public sealed class TgEfStoryRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfStoryEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfStoryRepository)}";

	public override TgEfStorageResult<TgEfStoryEntity> Get(TgEfStoryEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfStoryEntity> storageResult = base.Get(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfStoryEntity? itemFind = isNoTracking
			? EfContext.Stories
				.AsNoTracking()
				.SingleOrDefault(x => x.Id == item.Id)
			: EfContext.Stories
				.AsTracking()
				.SingleOrDefault(x => x.Id == item.Id);
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfStoryEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfStoryEntity>> GetAsync(TgEfStoryEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfStoryEntity> storageResult = await base.GetAsync(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfStoryEntity? itemFind = isNoTracking
			? await EfContext.Stories.AsNoTracking()
				.Where(x => x.Id == item.Id)
				.SingleOrDefaultAsync()
			: await EfContext.Stories.AsTracking()
				.Where(x => x.Id == item.Id)
				.SingleOrDefaultAsync();
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfStoryEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfStorageResult<TgEfStoryEntity> GetFirst(bool isNoTracking)
	{
		TgEfStoryEntity? item = isNoTracking
			? EfContext.Stories.AsNoTracking().FirstOrDefault()
			: EfContext.Stories.AsTracking().FirstOrDefault();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfStoryEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfStoryEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfStoryEntity? item = isNoTracking
			? await EfContext.Stories.AsNoTracking().FirstOrDefaultAsync()
			: await EfContext.Stories.AsTracking().FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfStoryEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfStorageResult<TgEfStoryEntity> GetList(int take, int skip, bool isNoTracking)
	{
		IList<TgEfStoryEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Stories.AsNoTracking().Skip(skip).Take(take).ToList()
				: [.. EfContext.Stories.AsTracking().Skip(skip).Take(take)];
		}
		else
		{
			items = isNoTracking
				? EfContext.Stories.AsNoTracking().ToList()
				: [.. EfContext.Stories.AsTracking()];
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfStoryEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfStoryEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? await EfContext.Stories.AsNoTracking().Skip(skip).Take(take).ToListAsync()
				: await EfContext.Stories.AsTracking().Skip(skip).Take(take).ToListAsync();
		}
		else
		{
			items = isNoTracking
				? await EfContext.Stories.AsNoTracking().ToListAsync()
				: await EfContext.Stories.AsTracking().ToListAsync();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override TgEfStorageResult<TgEfStoryEntity> GetList(int take, int skip, Expression<Func<TgEfStoryEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfStoryEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Stories.AsNoTracking().Where(where).Skip(skip).Take(take).ToList()
				: [.. EfContext.Stories.AsTracking().Where(where).Skip(skip).Take(take)];
		}
		else
		{
			items = isNoTracking
				? EfContext.Stories.AsNoTracking().Where(where).ToList()
				: [.. EfContext.Stories.AsTracking().Where(where)];
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfStoryEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfStoryEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfStoryEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? await EfContext.Stories.AsNoTracking().Where(where).Skip(skip).Take(take).ToListAsync()
				: await EfContext.Stories.AsTracking().Where(where).Skip(skip).Take(take).ToListAsync();
		}
		else
		{
			items = isNoTracking
				? await EfContext.Stories.AsNoTracking().Where(where).ToListAsync()
				: await EfContext.Stories.AsTracking().Where(where).ToListAsync();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Stories.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Stories.AsNoTracking().CountAsync();

	public override int GetCount(Expression<Func<TgEfStoryEntity, bool>> where) =>
		EfContext.Stories.AsNoTracking().Where(where).Count();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfStoryEntity, bool>> where) =>
		await EfContext.Stories.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfStorageResult<TgEfStoryEntity> DeleteAll()
	{
		TgEfStorageResult<TgEfStoryEntity> storageResult = GetList(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfStoryEntity item in storageResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfStorageResult<TgEfStoryEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfStoryEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfStoryEntity item in storageResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}