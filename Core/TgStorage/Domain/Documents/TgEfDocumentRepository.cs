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
			: EfContext.Documents
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
				.DefaultIfEmpty()
				.SingleOrDefaultAsync(x => x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
				.ConfigureAwait(false)
			: await EfContext.Documents
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.SingleOrDefaultAsync(x => x.SourceId == item.SourceId && x.Id == item.Id && x.MessageId == item.MessageId)
				.ConfigureAwait(false);
		return itemFind is not null
			? new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfOperResult<TgEfDocumentEntity> GetFirst(bool isNoTracking)
	{
		TgEfDocumentEntity? item = isNoTracking
			? EfContext.Documents.AsTracking().Include(x => x.Source).FirstOrDefault()
			: EfContext.Documents.Include(x => x.Source).FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfOperResult<TgEfDocumentEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfDocumentEntity? item = isNoTracking
			? await EfContext.Documents.AsTracking().Include(x => x.Source).FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Documents.Include(x => x.Source).FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfDocumentEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfOperResult<TgEfDocumentEntity> GetEnumerable(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
		topRecords switch
		{
			TgEnumTableTopRecords.Top1 => GetEnumerable(1, isNoTracking),
			TgEnumTableTopRecords.Top20 => GetEnumerable(20, isNoTracking),
			TgEnumTableTopRecords.Top100 => GetEnumerable(200, isNoTracking),
			TgEnumTableTopRecords.Top1000 => GetEnumerable(1_000, isNoTracking),
			TgEnumTableTopRecords.Top10000 => GetEnumerable(10_000, isNoTracking),
			TgEnumTableTopRecords.Top100000 => GetEnumerable(100_000, isNoTracking),
			TgEnumTableTopRecords.Top1000000 => GetEnumerable(1_000_000, isNoTracking),
			_ => GetEnumerable(0, isNoTracking),
		};

	public override TgEfOperResult<TgEfDocumentEntity> GetEnumerable(int count, bool isNoTracking)
	{
		IEnumerable<TgEfDocumentEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking().Include(x => x.Source).Take(count).AsEnumerable()
				: EfContext.Documents.Include(x => x.Source).Take(count).AsEnumerable();
		}
		else
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking().Include(x => x.Source).AsEnumerable()
				: EfContext.Documents.Include(x => x.Source).AsEnumerable();
		}
		return new TgEfOperResult<TgEfDocumentEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfDocumentEntity>> GetEnumerableAsync(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
		topRecords switch
		{
			TgEnumTableTopRecords.Top1 => await GetEnumerableAsync(1, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top20 => await GetEnumerableAsync(20, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top100 => await GetEnumerableAsync(200, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top1000 => await GetEnumerableAsync(1_000, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top10000 => await GetEnumerableAsync(10_000, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top100000 => await GetEnumerableAsync(100_000, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top1000000 => await GetEnumerableAsync(1_000_000, isNoTracking).ConfigureAwait(false),
			_ => await GetEnumerableAsync(0, isNoTracking).ConfigureAwait(false),
		};

	public override async Task<TgEfOperResult<TgEfDocumentEntity>> GetEnumerableAsync(int count, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IEnumerable<TgEfDocumentEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking().Include(x => x.Source).Take(count).AsEnumerable()
				: EfContext.Documents.Include(x => x.Source).Take(count).AsEnumerable();
		}
		else
		{
			items = isNoTracking
				? EfContext.Documents.AsNoTracking().Include(x => x.Source).AsEnumerable()
				: EfContext.Documents.Include(x => x.Source).AsEnumerable();
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
		TgEfOperResult<TgEfDocumentEntity> operResult = GetEnumerable(0, isNoTracking: false);
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
		TgEfOperResult<TgEfDocumentEntity> operResult = await GetEnumerableAsync(0, isNoTracking: false).ConfigureAwait(false);
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