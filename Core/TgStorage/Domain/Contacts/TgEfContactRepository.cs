// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Contacts;

public sealed class TgEfContactRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfContactEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfContactRepository)}";

	public override IQueryable<TgEfContactEntity> GetQuery(bool isNoTracking) =>
		isNoTracking ? EfContext.Contacts.AsNoTracking() : EfContext.Contacts.AsTracking();

	public override async Task<TgEfStorageResult<TgEfContactEntity>> GetAsync(TgEfContactEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfContactEntity> storageResult = await base.GetAsync(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfContactEntity? itemFind = await GetQuery(isNoTracking).SingleOrDefaultAsync(x => x.Id == item.Id);
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfContactEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfContactEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfContactEntity? item = await GetQuery(isNoTracking).FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfContactEntity>(TgEnumEntityState.IsExists, item);
	}

	public async Task<TgEfStorageResult<TgEfContactDto>> GetListDtoAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfContactDto> items;
		items = take > 0
			? await GetQuery(isNoTracking).Skip(skip).Take(take).Select(SelectDto()).ToListAsync()
			: await GetQuery(isNoTracking).Select(SelectDto()).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	private static Expression<Func<TgEfContactEntity, TgEfContactDto>> SelectDto() => item => new TgEfContactDto
	{
		Uid = item.Uid,
		Id = item.Id,
		UserName = item.UserName ?? string.Empty,
		DtChanged = $"{item.DtChanged:yyyy-MM-dd}",
		IsContactActive = item.IsActive,
		IsBot = item.IsBot,
		FirstName = item.FirstName ?? string.Empty,
		LastName = item.LastName ?? string.Empty,
		Phone = item.PhoneNumber ?? string.Empty,
		Status = item.Status ?? string.Empty,
	}; 

	public override async Task<TgEfStorageResult<TgEfContactEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfContactEntity> items = take > 0 
			? await GetQuery(isNoTracking).Skip(skip).Take(take).ToListAsync() 
			: await GetQuery(isNoTracking).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfContactEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfContactEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfContactEntity> items = take > 0
			? await GetQuery(isNoTracking).Where(where).Skip(skip).Take(take).ToListAsync()
			: await GetQuery(isNoTracking).Where(where).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<int> GetCountAsync() => await EfContext.Contacts.AsNoTracking().CountAsync();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfContactEntity, bool>> where) =>
		await EfContext.Contacts.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Delete

	public override async Task<TgEfStorageResult<TgEfContactEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfContactEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfContactEntity item in storageResult.Items)
			{
				await DeleteAsync(item);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}