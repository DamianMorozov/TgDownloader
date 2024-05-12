// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Documents;

public sealed class TgEfDocumentRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfDocumentEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfDocumentRepository)}";

	public override TgEfOperResult<TgEfDocumentEntity> Get(TgEfDocumentEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfDocumentEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExists)
			return operResult;
		TgEfDocumentEntity? itemFind = isNoTracking
			? EfContext.Documents.AsNoTracking()
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.SingleOrDefault(x => x != null && x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
			: EfContext.Documents.AsTracking()
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.SingleOrDefault(x => x != null && x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId);
		return itemFind is not null
			? new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfOperResult<TgEfDocumentEntity>> GetAsync(TgEfDocumentEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfDocumentEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExists)
			return operResult;
		TgEfDocumentEntity? itemFind = isNoTracking
			? await EfContext.Documents.AsNoTracking()
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.Where(x => x != null && x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
				.DefaultIfEmpty()
				.SingleOrDefaultAsync()
				.ConfigureAwait(false)
			: await EfContext.Documents.AsTracking()
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.Where(x => x != null && x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
				.DefaultIfEmpty()
				.SingleOrDefaultAsync()
				.ConfigureAwait(false);
		return itemFind is not null
			? new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfOperResult<TgEfDocumentEntity> GetFirst(bool isNoTracking)
	{
		TgEfDocumentEntity? item = isNoTracking
			? EfContext.Documents.AsNoTracking()
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.FirstOrDefault()
			: EfContext.Documents.AsTracking()
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfOperResult<TgEfDocumentEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfDocumentEntity? item = isNoTracking
			? await EfContext.Documents.AsNoTracking()
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Documents.AsTracking()
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfOperResult<TgEfDocumentEntity> GetList(int take, int skip, bool isNoTracking)
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
		return new TgEfOperResult<TgEfDocumentEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfDocumentEntity>> GetListAsync(int take, int skip, bool isNoTracking)
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
		return new TgEfOperResult<TgEfDocumentEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override TgEfOperResult<TgEfDocumentEntity> GetList(int take, int skip, Expression<Func<TgEfDocumentEntity, bool>> where, bool isNoTracking)
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
		return new TgEfOperResult<TgEfDocumentEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfDocumentEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfDocumentEntity, bool>> where, bool isNoTracking)
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
		return new TgEfOperResult<TgEfDocumentEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
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

	public override TgEfOperResult<TgEfDocumentEntity> DeleteAll()
	{
		TgEfOperResult<TgEfDocumentEntity> operResult = GetList(0, 0, isNoTracking: false);
		if (operResult.IsExists)
		{
			foreach (TgEfDocumentEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfOperResult<TgEfDocumentEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfDocumentEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfDocumentEntity> operResult = await GetListAsync(0, 0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExists)
		{
			foreach (TgEfDocumentEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfOperResult<TgEfDocumentEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}