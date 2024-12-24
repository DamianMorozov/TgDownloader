// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Filters;

public sealed class TgEfFilterRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfFilterEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfFilterRepository)}";

	public override IQueryable<TgEfFilterEntity> GetQuery(bool isNoTracking) =>
		isNoTracking ? EfContext.Filters.AsNoTracking() : EfContext.Filters.AsTracking();

	public override async Task<TgEfStorageResult<TgEfFilterEntity>> GetAsync(TgEfFilterEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfFilterEntity> storageResult = await base.GetAsync(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfFilterEntity? itemFind = await GetQuery(isNoTracking).SingleOrDefaultAsync(x => x.FilterType == item.FilterType && x.Name == item.Name);
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfFilterEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfFilterEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfFilterEntity? item = await GetQuery(isNoTracking).FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfFilterEntity>(TgEnumEntityState.IsExists, item);
	}

	public async Task<TgEfStorageResult<TgEfFilterDto>> GetListDtoAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfFilterDto> items;
		items = take > 0
			? await GetQuery(isNoTracking).Skip(skip).Take(take).Select(SelectDto()).ToListAsync()
			: await GetQuery(isNoTracking).Select(SelectDto()).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	private static Expression<Func<TgEfFilterEntity, TgEfFilterDto>> SelectDto() => item => new TgEfFilterDto
	{
		Uid = item.Uid,
		IsEnabled = item.IsEnabled,
		FilterType = item.GetStringForFilterType(),
		Name = item.Name,
		Mask = item.Mask,
		Size = $"{item.Size} {item.SizeType}",
	};

	public override async Task<TgEfStorageResult<TgEfFilterEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfFilterEntity> items = take > 0 
			? await GetQuery(isNoTracking).Skip(skip).Take(take).ToListAsync() 
			: await GetQuery(isNoTracking).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfFilterEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfFilterEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfFilterEntity> items = take > 0
			? await GetQuery(isNoTracking).Where(where).Skip(skip).Take(take).ToListAsync()
			: await GetQuery(isNoTracking).Where(where).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<int> GetCountAsync() => await EfContext.Filters.AsNoTracking().CountAsync();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfFilterEntity, bool>> where) => 
		await EfContext.Filters.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Delete

	public override async Task<TgEfStorageResult<TgEfFilterEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfFilterEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfFilterEntity item in storageResult.Items)
			{
				await DeleteAsync(item);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}