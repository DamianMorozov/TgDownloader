// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Messages;

public sealed class TgEfMessageRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfMessageEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfMessageRepository)}";

	public override TgEfStorageResult<TgEfMessageEntity> Get(TgEfMessageEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfMessageEntity> storageResult = base.Get(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfMessageEntity? itemFind = isNoTracking
			? EfContext.Messages.AsNoTracking()
				.Where(x => x.SourceId == item.SourceId && x.Id == item.Id)
				.Include(x => x.Source)
				//.DefaultIfEmpty()
				.SingleOrDefault()
			: EfContext.Messages
				.Where(x => x.SourceId == item.SourceId && x.Id == item.Id)
				.Include(x => x.Source)
				//.DefaultIfEmpty()
				.SingleOrDefault();
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfMessageEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfMessageEntity>> GetAsync(TgEfMessageEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfMessageEntity> storageResult = await base.GetAsync(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfMessageEntity? itemFind = isNoTracking
			? await EfContext.Messages.AsNoTracking()
				.Where(x => x.SourceId == item.SourceId && x.Id == item.Id)
				.Include(x => x.Source)
				.SingleOrDefaultAsync()
			: await EfContext.Messages
				.Where(x => x.SourceId == item.SourceId && x.Id == item.Id)
				.Include(x => x.Source)
				.SingleOrDefaultAsync();
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfMessageEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfStorageResult<TgEfMessageEntity> GetFirst(bool isNoTracking)
	{
		TgEfMessageEntity? item = isNoTracking
			? EfContext.Messages.AsTracking()
				.Include(x => x.Source)
				//.DefaultIfEmpty()
				.FirstOrDefault()
			: EfContext.Messages
				.Include(x => x.Source)
				//.DefaultIfEmpty()
				.FirstOrDefault();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfMessageEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfMessageEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfMessageEntity? item = isNoTracking
			? await EfContext.Messages.AsTracking()
				.Include(x => x.Source)
				.FirstOrDefaultAsync()
			: await EfContext.Messages
				.Include(x => x.Source)
				.FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfMessageEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfStorageResult<TgEfMessageEntity> GetList(int take, int skip, bool isNoTracking)
	{
		IList<TgEfMessageEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking().Include(x => x.Source).Skip(skip).Take(take).ToList()
				: EfContext.Messages.Include(x => x.Source).Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking().Include(x => x.Source).ToList()
				: EfContext.Messages.Include(x => x.Source).ToList();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfMessageEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfMessageEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? await EfContext.Messages.AsNoTracking().Include(x => x.Source).Skip(skip).Take(take).ToListAsync()
				: await EfContext.Messages.Include(x => x.Source).Skip(skip).Take(take).ToListAsync();
		}
		else
		{
			items = isNoTracking
				? await EfContext.Messages.AsNoTracking().Include(x => x.Source).ToListAsync()
				: await EfContext.Messages.Include(x => x.Source).ToListAsync();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override TgEfStorageResult<TgEfMessageEntity> GetList(int take, int skip, Expression<Func<TgEfMessageEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfMessageEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking().Where(where).Include(x => x.Source).Skip(skip).Take(take).ToList()
				: EfContext.Messages.Where(where).Include(x => x.Source).Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking().Where(where).Include(x => x.Source).ToList()
				: EfContext.Messages.Where(where).Include(x => x.Source).ToList();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfMessageEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfMessageEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfMessageEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? await EfContext.Messages.AsNoTracking().Where(where).Include(x => x.Source).Skip(skip).Take(take).ToListAsync()
				: await EfContext.Messages.Where(where).Include(x => x.Source).Skip(skip).Take(take).ToListAsync();
		}
		else
		{
			items = isNoTracking
				? await EfContext.Messages.AsNoTracking().Where(where).Include(x => x.Source).ToListAsync()
				: await EfContext.Messages.Where(where).Include(x => x.Source).ToListAsync();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Messages.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Messages.AsNoTracking().CountAsync();

	public override int GetCount(Expression<Func<TgEfMessageEntity, bool>> where) => 
		EfContext.Messages.AsNoTracking().Where(where).Count();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfMessageEntity, bool>> where) => 
		await EfContext.Messages.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfStorageResult<TgEfMessageEntity> DeleteAll()
	{
		TgEfStorageResult<TgEfMessageEntity> storageResult = GetList(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfMessageEntity item in storageResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfStorageResult<TgEfMessageEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfMessageEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfMessageEntity item in storageResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}