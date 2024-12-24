// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Documents;

public sealed class TgEfDocumentRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfDocumentEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfDocumentRepository)}";

	public override IQueryable<TgEfDocumentEntity> GetQuery(bool isReadOnly = true) =>
		isReadOnly ? EfContext.Documents.AsNoTracking() : EfContext.Documents.AsTracking();

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> GetAsync(TgEfDocumentEntity item, bool isReadOnly = true)
	{
		TgEfStorageResult<TgEfDocumentEntity> storageResult = await base.GetAsync(item, isReadOnly);
		if (storageResult.IsExists)
			return storageResult;
		TgEfDocumentEntity? itemFind = await GetQuery(isReadOnly)
				.Where(x => x != null && x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
				.Include(x => x.Source).SingleOrDefaultAsync();
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfDocumentEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> GetFirstAsync(bool isReadOnly = true)
	{
		TgEfDocumentEntity? item = await GetQuery(isReadOnly).Include(x => x.Source).FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfDocumentEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> GetListAsync(int take, int skip, bool isReadOnly = true)
	{
		IList<TgEfDocumentEntity> items = take > 0
			? await GetQuery(isReadOnly).Include(x => x.Source).Skip(skip).Take(take).ToListAsync()
			: await GetQuery(isReadOnly).Include(x => x.Source).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfDocumentEntity, bool>> where, bool isReadOnly = true)
	{
		IList<TgEfDocumentEntity> items = take > 0
			? await GetQuery(isReadOnly).Where(where).Include(x => x.Source).Skip(skip).Take(take).ToListAsync()
			: await GetQuery(isReadOnly).Where(where).Include(x => x.Source).ToListAsync();
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<int> GetCountAsync() => await EfContext.Documents.AsNoTracking().CountAsync();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfDocumentEntity, bool>> where) => 
		await EfContext.Documents.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Delete

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfDocumentEntity> storageResult = await GetListAsync(0, 0, isReadOnly: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfDocumentEntity item in storageResult.Items)
			{
				await DeleteAsync(item);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}