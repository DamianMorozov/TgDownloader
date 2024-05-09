// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Documents;

public sealed class TgEfDocumentRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfDocumentEntity>(efContext)
{
	#region Public and private methods

	public override TgEfOperResult<TgEfDocumentEntity> Get(TgEfDocumentEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfDocumentEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExists)
			return operResult;
		TgEfDocumentEntity? itemFind = isNoTracking
			? EfContext.Documents.AsNoTracking()
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.SingleOrDefault(x => x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
			: EfContext.Documents.AsTracking()
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.SingleOrDefault(x => x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId);
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
				.Where(x => x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
				.DefaultIfEmpty()
				.SingleOrDefaultAsync()
				.ConfigureAwait(false)
			: await EfContext.Documents.AsTracking()
				.Include(x => x.Source)
				.Where(x => x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
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

	public override TgEfOperResult<TgEfDocumentEntity> GetList(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
		topRecords switch
		{
			TgEnumTableTopRecords.Top1 => GetList(1, isNoTracking),
			TgEnumTableTopRecords.Top20 => GetList(20, isNoTracking),
			TgEnumTableTopRecords.Top100 => GetList(200, isNoTracking),
			TgEnumTableTopRecords.Top1000 => GetList(1_000, isNoTracking),
			TgEnumTableTopRecords.Top10000 => GetList(10_000, isNoTracking),
			TgEnumTableTopRecords.Top100000 => GetList(100_000, isNoTracking),
			TgEnumTableTopRecords.Top1000000 => GetList(1_000_000, isNoTracking),
			_ => GetList(0, isNoTracking),
		};

	private TgEfOperResult<TgEfDocumentEntity> GetList(int count, bool isNoTracking)
	{
		IList<TgEfDocumentEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking()
					.Include(x => x.Source)
					.Take(count).AsEnumerable().ToList()
				: EfContext.Documents.AsTracking()
					.Include(x => x.Source)
					.Take(count).AsEnumerable().ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking()
					.Include(x => x.Source)
					.AsEnumerable().ToList()
				: EfContext.Documents.AsTracking()
					.Include(x => x.Source)
					.AsEnumerable().ToList();
		}
		return new TgEfOperResult<TgEfDocumentEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfDocumentEntity>> GetListAsync(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
		topRecords switch
		{
			TgEnumTableTopRecords.Top1 => await GetListAsync(1, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top20 => await GetListAsync(20, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top100 => await GetListAsync(200, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top1000 => await GetListAsync(1_000, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top10000 => await GetListAsync(10_000, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top100000 => await GetListAsync(100_000, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top1000000 => await GetListAsync(1_000_000, isNoTracking).ConfigureAwait(false),
			_ => await GetListAsync(0, isNoTracking).ConfigureAwait(false),
		};

	private async Task<TgEfOperResult<TgEfDocumentEntity>> GetListAsync(int count, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IEnumerable<TgEfDocumentEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking()
					.Include(x => x.Source)
					.Take(count).AsEnumerable()
				: EfContext.Documents.AsTracking()
					.Include(x => x.Source)
					.Take(count).AsEnumerable();
		}
		else
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking()
					.Include(x => x.Source)
					.AsEnumerable()
				: EfContext.Documents.AsTracking()
					.Include(x => x.Source)
					.AsEnumerable();
		}
		return new TgEfOperResult<TgEfDocumentEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Documents.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Documents.AsNoTracking().CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfOperResult<TgEfDocumentEntity> DeleteAll()
	{
		TgEfOperResult<TgEfDocumentEntity> operResult = GetList(0, isNoTracking: false);
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
		TgEfOperResult<TgEfDocumentEntity> operResult = await GetListAsync(0, isNoTracking: false).ConfigureAwait(false);
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