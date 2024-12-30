// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Contacts;

/// <summary> Contact repository </summary>
public sealed class TgEfContactRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfContactEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfContactRepository)}";

	public override IQueryable<TgEfContactEntity> GetQuery(bool isReadOnly = true) =>
		isReadOnly ? EfContext.Contacts.AsNoTracking() : EfContext.Contacts.AsTracking();

	public override async Task<TgEfStorageResult<TgEfContactEntity>> GetAsync(TgEfContactEntity item, bool isReadOnly = true)
	{
		TgEfStorageResult<TgEfContactEntity> storageResult = await base.GetAsync(item, isReadOnly);
		if (storageResult.IsExists)
			return storageResult;
		TgEfContactEntity? itemFind = await GetQuery(isReadOnly).SingleOrDefaultAsync(x => x.Id == item.Id);
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfContactEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfContactEntity>> GetFirstAsync(bool isReadOnly = true)
	{
		TgEfContactEntity? item = await GetQuery(isReadOnly).FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfContactEntity>(TgEnumEntityState.IsExists, item);
	}

	public async Task<List<TgEfContactDto>> GetListDtosAsync(int take, int skip, bool isReadOnly = true)
	{
		var dtos = take > 0
			? await GetQuery(isReadOnly).Skip(skip).Take(take).Select(SelectDto()).ToListAsync()
			: await GetQuery(isReadOnly).Select(SelectDto()).ToListAsync();
		return dtos;
	}

	private static Expression<Func<TgEfContactEntity, TgEfContactDto>> SelectDto() => item => new TgEfContactDto().GetDto(item);

	public override async Task<TgEfStorageResult<TgEfContactEntity>> GetListAsync(int take, int skip, bool isReadOnly = true)
	{
		IList<TgEfContactEntity> items = take > 0 
			? await GetQuery(isReadOnly).Skip(skip).Take(take).ToListAsync() 
			: await GetQuery(isReadOnly).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfContactEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfContactEntity, bool>> where, bool isReadOnly = true)
	{
		IList<TgEfContactEntity> items = take > 0
			? await GetQuery(isReadOnly).Where(where).Skip(skip).Take(take).ToListAsync()
			: await GetQuery(isReadOnly).Where(where).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<int> GetCountAsync() => await EfContext.Contacts.AsNoTracking().CountAsync();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfContactEntity, bool>> where) =>
		await EfContext.Contacts.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Delete

	public override async Task<TgEfStorageResult<TgEfContactEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfContactEntity> storageResult = await GetListAsync(0, 0, isReadOnly: false);
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