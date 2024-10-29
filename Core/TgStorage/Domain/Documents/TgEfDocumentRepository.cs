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
			? new TgEfStorageResult<TgEfDocumentEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfDocumentEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> GetAsync(TgEfDocumentEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfDocumentEntity> storageResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (storageResult.IsExists)
			return storageResult;
		TgEfDocumentEntity? itemFind = isNoTracking
			? await EfContext.Documents.AsNoTracking()
				.Where(x => x != null && x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
				.Include(x => x.Source)
				//.DefaultIfEmpty()
				//.DefaultIfEmpty()
				.SingleOrDefaultAsync()
				.ConfigureAwait(false)
			: await EfContext.Documents.AsTracking()
				.Where(x => x != null && x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
				.Include(x => x.Source)
				//.DefaultIfEmpty()
				//.DefaultIfEmpty()
				.SingleOrDefaultAsync()
				.ConfigureAwait(false);
		return itemFind is not null
			? new TgEfStorageResult<TgEfDocumentEntity>(TgEnumEntityState.IsExists, itemFind)
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
			? new TgEfStorageResult<TgEfDocumentEntity>(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfDocumentEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfDocumentEntity? item = isNoTracking
			? await EfContext.Documents.AsNoTracking()
				.Include(x => x.Source)
				//.DefaultIfEmpty()
				.FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Documents.AsTracking()
				.Include(x => x.Source)
				//.DefaultIfEmpty()
				.FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfStorageResult<TgEfDocumentEntity>(TgEnumEntityState.NotExists)
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
		return new TgEfStorageResult<TgEfDocumentEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
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
		return new TgEfStorageResult<TgEfDocumentEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
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
		return new TgEfStorageResult<TgEfDocumentEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfDocumentEntity, bool>> where, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
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
		return new TgEfStorageResult<TgEfDocumentEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Documents.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Documents.AsNoTracking().CountAsync().ConfigureAwait(false);

	public override int GetCount(Expression<Func<TgEfDocumentEntity, bool>> where) => 
		EfContext.Documents.AsNoTracking().Where(where).Count();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfDocumentEntity, bool>> where) => 
		await EfContext.Documents.AsNoTracking().Where(where).CountAsync().ConfigureAwait(false);

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
		return new TgEfStorageResult<TgEfDocumentEntity>(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfStorageResult<TgEfDocumentEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfDocumentEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false).ConfigureAwait(false);
		if (storageResult.IsExists)
		{
			foreach (TgEfDocumentEntity item in storageResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfStorageResult<TgEfDocumentEntity>(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}