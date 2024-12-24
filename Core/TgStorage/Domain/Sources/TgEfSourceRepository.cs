// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Sources;

public sealed class TgEfSourceRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfSourceEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfSourceRepository)}";

	public override IQueryable<TgEfSourceEntity> GetQuery(bool isNoTracking) =>
		isNoTracking? EfContext.Sources.AsNoTracking() : EfContext.Sources.AsTracking();

	public override async Task<TgEfStorageResult<TgEfSourceEntity>> GetAsync(TgEfSourceEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfSourceEntity> storageResult = await base.GetAsync(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfSourceEntity? itemFind = await GetQuery(isNoTracking).SingleOrDefaultAsync(x => x.Id == item.Id);
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfSourceEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfSourceEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfSourceEntity? item = await GetQuery(isNoTracking).FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfSourceEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfSourceEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfSourceEntity> items;
		items = take > 0 
			? await GetQuery(isNoTracking).Skip(skip).Take(take).ToListAsync() 
			: await GetQuery(isNoTracking).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public async Task<TgEfStorageResult<TgEfSourceDto>> GetListDtoAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfSourceDto> items;
		items = take > 0
			? await GetQuery(isNoTracking).Skip(skip).Take(take).Select(SelectDto()).ToListAsync()
			: await GetQuery(isNoTracking).Select(SelectDto()).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	private static Expression<Func<TgEfSourceEntity, TgEfSourceDto>> SelectDto() => item => new TgEfSourceDto
	{
		Uid = item.Uid,
		Id = item.Id,
		UserName = item.UserName ?? string.Empty,
		DtChanged = $"{item.DtChanged:yyyy-MM-dd}",
		IsSourceActive = item.IsActive,
		IsAutoUpdate = item.IsAutoUpdate,
		Title = item.Title ?? string.Empty,
		FirstId = item.FirstId,
		Count = item.Count,
	};

	public override async Task<TgEfStorageResult<TgEfSourceEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfSourceEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfSourceEntity> items;
		var query = isNoTracking ? EfContext.Sources.AsNoTracking() : EfContext.Sources.AsTracking();
		items = take > 0 
			? await query.Where(where).Skip(skip).Take(take).ToListAsync() 
			: await query.Where(where).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<int> GetCountAsync() => await EfContext.Sources.AsNoTracking().CountAsync();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfSourceEntity, bool>> where) => 
		await EfContext.Sources.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Delete

	public override async Task<TgEfStorageResult<TgEfSourceEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfSourceEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfSourceEntity item in storageResult.Items)
			{
				await DeleteAsync(item);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}