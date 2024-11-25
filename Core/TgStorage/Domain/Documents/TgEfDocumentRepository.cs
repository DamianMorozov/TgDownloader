// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Documents;

public sealed class TgEfDocumentRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfDocumentEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfDocumentRepository)}";

	public override TgEfStorageResult<TgEfDocumentEntity> Get(TgEfDocumentEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfDocumentEntity> storageResult = base.Get(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfDocumentEntity? itemFind = isNoTracking
			? EfContext.Documents.AsNoTracking()
				.Where(x => x != null && x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
				.Include(x => x.Source)
				//.DefaultIfEmpty()
				.SingleOrDefault()
			: EfContext.Documents.AsTracking()
				.Where(x => x != null && x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
				.Include(x => x.Source)
				//.DefaultIfEmpty()
				.SingleOrDefault();
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfDocumentEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> GetAsync(TgEfDocumentEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfDocumentEntity> storageResult = await base.GetAsync(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfDocumentEntity? itemFind = isNoTracking
			? await EfContext.Documents.AsNoTracking()
				.Where(x => x != null && x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
				.Include(x => x.Source)
				.SingleOrDefaultAsync()
			: await EfContext.Documents.AsTracking()
				.Where(x => x != null && x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
				.Include(x => x.Source)
				.SingleOrDefaultAsync();
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfDocumentEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfStorageResult<TgEfDocumentEntity> GetFirst(bool isNoTracking)
	{
		TgEfDocumentEntity? item = isNoTracking
			? EfContext.Documents.AsNoTracking()
				.Include(x => x.Source)
				//.DefaultIfEmpty()
				.FirstOrDefault()
			: EfContext.Documents.AsTracking()
				.Include(x => x.Source)
				//.DefaultIfEmpty()
				.FirstOrDefault();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfDocumentEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfDocumentEntity? item = isNoTracking
			? await EfContext.Documents.AsNoTracking()
				.Include(x => x.Source)
				.FirstOrDefaultAsync()
			: await EfContext.Documents.AsTracking()
				.Include(x => x.Source)
				.FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfDocumentEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfStorageResult<TgEfDocumentEntity> GetList(int take, int skip, bool isNoTracking)
	{
		IList<TgEfDocumentEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking()
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList()
				: EfContext.Documents.AsTracking()
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking()
					.Include(x => x.Source)
					.ToList()
				: EfContext.Documents.AsTracking()
					.Include(x => x.Source)
					.ToList();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		await Task.Delay(1);
		IList<TgEfDocumentEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking()
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList()
				: EfContext.Documents.AsTracking()
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking()
					.Include(x => x.Source)
					.ToList()
				: EfContext.Documents.AsTracking()
					.Include(x => x.Source)
					.ToList();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override TgEfStorageResult<TgEfDocumentEntity> GetList(int take, int skip, Expression<Func<TgEfDocumentEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfDocumentEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking()
					.Where(where)
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList()
				: EfContext.Documents.AsTracking()
					.Where(where)
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking()
					.Where(where)
					.Include(x => x.Source)
					.ToList()
				: EfContext.Documents.AsTracking()
					.Where(where)
					.Include(x => x.Source)
					.ToList();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfDocumentEntity, bool>> where, bool isNoTracking)
	{
		await Task.Delay(1);
		IList<TgEfDocumentEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking()
					.Where(where)
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList()
				: EfContext.Documents.AsTracking()
					.Where(where)
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking()
					.Where(where)
					.Include(x => x.Source)
					.ToList()
				: EfContext.Documents.AsTracking()
					.Where(where)
					.Include(x => x.Source)
					.ToList();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Documents.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Documents.AsNoTracking().CountAsync();

	public override int GetCount(Expression<Func<TgEfDocumentEntity, bool>> where) => 
		EfContext.Documents.AsNoTracking().Where(where).Count();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfDocumentEntity, bool>> where) => 
		await EfContext.Documents.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfStorageResult<TgEfDocumentEntity> DeleteAll()
	{
		TgEfStorageResult<TgEfDocumentEntity> storageResult = GetList(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfDocumentEntity item in storageResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfDocumentEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfDocumentEntity item in storageResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}