// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Messages;

public sealed class TgEfMessageRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfMessageEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfMessageRepository)}";

	public override IQueryable<TgEfMessageEntity> GetQuery(bool isNoTracking) =>
		isNoTracking ? EfContext.Messages.AsNoTracking() : EfContext.Messages.AsTracking();

	public override async Task<TgEfStorageResult<TgEfMessageEntity>> GetAsync(TgEfMessageEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfMessageEntity> storageResult = await base.GetAsync(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfMessageEntity? itemFind = await GetQuery(isNoTracking)
			.Where(x => x.SourceId == item.SourceId && x.Id == item.Id)
			.Include(x => x.Source)
			.SingleOrDefaultAsync();
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfMessageEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfMessageEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfMessageEntity? item = await GetQuery(isNoTracking).Include(x => x.Source).FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfMessageEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfMessageEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfMessageEntity> items = take > 0
			? await GetQuery(isNoTracking).Include(x => x.Source).Skip(skip).Take(take).ToListAsync()
			: await GetQuery(isNoTracking).Include(x => x.Source).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfMessageEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfMessageEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfMessageEntity> items = take > 0
			? await GetQuery(isNoTracking).Where(where).Include(x => x.Source).Skip(skip).Take(take).ToListAsync()
			: await GetQuery(isNoTracking).Where(where).Include(x => x.Source).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<int> GetCountAsync() => await EfContext.Messages.AsNoTracking().CountAsync();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfMessageEntity, bool>> where) => 
		await EfContext.Messages.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Delete

	public override async Task<TgEfStorageResult<TgEfMessageEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfMessageEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfMessageEntity item in storageResult.Items)
			{
				await DeleteAsync(item);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}