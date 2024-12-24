// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Stories;

public sealed class TgEfStoryRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfStoryEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfStoryRepository)}";

	public override IQueryable<TgEfStoryEntity> GetQuery(bool isNoTracking) =>
		isNoTracking ? EfContext.Stories.AsNoTracking() : EfContext.Stories.AsTracking();

	public override async Task<TgEfStorageResult<TgEfStoryEntity>> GetAsync(TgEfStoryEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfStoryEntity> storageResult = await base.GetAsync(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfStoryEntity? itemFind = await GetQuery(isNoTracking).SingleOrDefaultAsync(x => x.Id == item.Id);
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfStoryEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfStoryEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfStoryEntity? item = await GetQuery(isNoTracking).FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfStoryEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfStoryEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfStoryEntity> items;
		items = take > 0 
			? await GetQuery(isNoTracking).Skip(skip).Take(take).ToListAsync() 
			: await GetQuery(isNoTracking).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfStoryEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfStoryEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfStoryEntity> items = take > 0
			? await GetQuery(isNoTracking).Where(where).Skip(skip).Take(take).ToListAsync()
			: await GetQuery(isNoTracking).Where(where).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<int> GetCountAsync() => await EfContext.Stories.AsNoTracking().CountAsync();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfStoryEntity, bool>> where) =>
		await EfContext.Stories.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Delete

	public override async Task<TgEfStorageResult<TgEfStoryEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfStoryEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfStoryEntity item in storageResult.Items)
			{
				await DeleteAsync(item);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}