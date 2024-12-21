// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Contacts;

public sealed class TgEfContactRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfContactEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfContactRepository)}";

	public override TgEfStorageResult<TgEfContactEntity> Get(TgEfContactEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfContactEntity> storageResult = base.Get(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfContactEntity? itemFind = isNoTracking
			? EfContext.Contacts
				.AsNoTracking()
				.SingleOrDefault(x => x.Id == item.Id)
			: EfContext.Contacts
				.AsTracking()
				.SingleOrDefault(x => x.Id == item.Id);
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfContactEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfContactEntity>> GetAsync(TgEfContactEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfContactEntity> storageResult = await base.GetAsync(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfContactEntity? itemFind = isNoTracking
			? await EfContext.Contacts.AsNoTracking()
				.Where(x => x.Id == item.Id)
				.SingleOrDefaultAsync()
			: await EfContext.Contacts.AsTracking()
				.Where(x => x.Id == item.Id)
				.SingleOrDefaultAsync();
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfContactEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfStorageResult<TgEfContactEntity> GetFirst(bool isNoTracking)
	{
		TgEfContactEntity? item = isNoTracking
			? EfContext.Contacts.AsNoTracking().FirstOrDefault()
			: EfContext.Contacts.AsTracking().FirstOrDefault();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfContactEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfContactEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfContactEntity? item = isNoTracking
			? await EfContext.Contacts.AsNoTracking().FirstOrDefaultAsync()
			: await EfContext.Contacts.AsTracking().FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfContactEntity>(TgEnumEntityState.IsExists, item);
	}

	public async Task<TgEfStorageResult<TgEfContactDto>> GetListDtoAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfContactDto> items;
		var query = isNoTracking ? EfContext.Contacts.AsNoTracking() : EfContext.Contacts.AsTracking();
		items = take > 0
			? await query
				.Skip(skip).Take(take)
				.Select(SelectDto()).ToListAsync()
			: (IList<TgEfContactDto>)await query
				.Select(SelectDto()).ToListAsync();
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

	public override TgEfStorageResult<TgEfContactEntity> GetList(int take, int skip, bool isNoTracking)
	{
		IList<TgEfContactEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Contacts.AsNoTracking().Skip(skip).Take(take).ToList()
				: [.. EfContext.Contacts.AsTracking().Skip(skip).Take(take)];
		}
		else
		{
			items = isNoTracking
				? EfContext.Contacts.AsNoTracking().ToList()
				: [.. EfContext.Contacts.AsTracking()];
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfContactEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfContactEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? await EfContext.Contacts.AsNoTracking().Skip(skip).Take(take).ToListAsync()
				: await EfContext.Contacts.AsTracking().Skip(skip).Take(take).ToListAsync();
		}
		else
		{
			items = isNoTracking
				? await EfContext.Contacts.AsNoTracking().ToListAsync()
				: await EfContext.Contacts.AsTracking().ToListAsync();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override TgEfStorageResult<TgEfContactEntity> GetList(int take, int skip, Expression<Func<TgEfContactEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfContactEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Contacts.AsNoTracking().Where(where).Skip(skip).Take(take).ToList()
				: [.. EfContext.Contacts.AsTracking().Where(where).Skip(skip).Take(take)];
		}
		else
		{
			items = isNoTracking
				? EfContext.Contacts.AsNoTracking().Where(where).ToList()
				: [.. EfContext.Contacts.AsTracking().Where(where)];
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfContactEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfContactEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfContactEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? await EfContext.Contacts.AsNoTracking().Where(where).Skip(skip).Take(take).ToListAsync()
				: await EfContext.Contacts.AsTracking().Where(where).Skip(skip).Take(take).ToListAsync();
		}
		else
		{
			items = isNoTracking
				? await EfContext.Contacts.AsNoTracking().Where(where).ToListAsync()
				: await EfContext.Contacts.AsTracking().Where(where).ToListAsync();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Contacts.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Contacts.AsNoTracking().CountAsync();

	public override int GetCount(Expression<Func<TgEfContactEntity, bool>> where) =>
		EfContext.Contacts.AsNoTracking().Where(where).Count();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfContactEntity, bool>> where) =>
		await EfContext.Contacts.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfStorageResult<TgEfContactEntity> DeleteAll()
	{
		TgEfStorageResult<TgEfContactEntity> storageResult = GetList(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfContactEntity item in storageResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfStorageResult<TgEfContactEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfContactEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfContactEntity item in storageResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}