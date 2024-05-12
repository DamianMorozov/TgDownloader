// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Messages;

public sealed class TgEfMessageRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfMessageEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfMessageRepository)}";

	public override TgEfOperResult<TgEfMessageEntity> Get(TgEfMessageEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfMessageEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExists)
			return operResult;
		TgEfMessageEntity? itemFind = isNoTracking
			? EfContext.Messages.AsNoTracking()
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.SingleOrDefault(x => x.SourceId == item.SourceId && x.Id == item.Id)
			: EfContext.Messages
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.SingleOrDefault(x => x.SourceId == item.SourceId && x.Id == item.Id);
		return itemFind is not null
			? new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfOperResult<TgEfMessageEntity>> GetAsync(TgEfMessageEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfMessageEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExists)
			return operResult;
		TgEfMessageEntity? itemFind = isNoTracking
			? await EfContext.Messages.AsNoTracking()
				.Include(x => x.Source)
				.Where(x => x.SourceId == item.SourceId && x.Id == item.Id)
				.DefaultIfEmpty()
				.SingleOrDefaultAsync()
				.ConfigureAwait(false)
			: await EfContext.Messages
				.Include(x => x.Source)
				.Where(x => x.SourceId == item.SourceId && x.Id == item.Id)
				.DefaultIfEmpty()
				.SingleOrDefaultAsync()
				.ConfigureAwait(false);
		return itemFind is not null
			? new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfOperResult<TgEfMessageEntity> GetFirst(bool isNoTracking)
	{
		TgEfMessageEntity? item = isNoTracking
			? EfContext.Messages.AsTracking()
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.FirstOrDefault()
			: EfContext.Messages
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfOperResult<TgEfMessageEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfMessageEntity? item = isNoTracking
			? await EfContext.Messages.AsTracking()
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Messages
				.Include(x => x.Source)
				.DefaultIfEmpty()
				.FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfMessageEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfOperResult<TgEfMessageEntity> GetList(int take, int skip, bool isNoTracking)
	{
		IList<TgEfMessageEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking()
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList()
				: EfContext.Messages
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking()
					.Include(x => x.Source)
					.ToList()
				: EfContext.Messages
					.Include(x => x.Source)
					.ToList();
		}
		return new TgEfOperResult<TgEfMessageEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfMessageEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfMessageEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking()
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList()
				: EfContext.Messages
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking()
					.Include(x => x.Source)
					.ToList()
				: EfContext.Messages
					.Include(x => x.Source)
					.ToList();
		}
		return new TgEfOperResult<TgEfMessageEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override TgEfOperResult<TgEfMessageEntity> GetList(int take, int skip, Expression<Func<TgEfMessageEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfMessageEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking()
					.Where(where)
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList()
				: EfContext.Messages
					.Where(where)
					.Include(x => x.Source)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking()
					.Where(where)
					.Include(x => x.Source)
					.ToList()
				: EfContext.Messages
					.Where(where)
					.Include(x => x.Source)
					.ToList();
		}
		return new TgEfOperResult<TgEfMessageEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfMessageEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfMessageEntity, bool>> where, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfMessageEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking()
					.Where(where)
					.Include(x => x.Source)
					.Skip(skip).Take(take)
					.ToList()
				: EfContext.Messages
					.Where(where)
					.Include(x => x.Source)
					.Skip(skip).Take(take)
					.ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Messages.AsNoTracking()
					.Where(where)
					.Include(x => x.Source)
					.ToList()
				: EfContext.Messages
					.Where(where)
					.Include(x => x.Source)
					.ToList();
		}
		return new TgEfOperResult<TgEfMessageEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Messages.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Messages.AsNoTracking().CountAsync().ConfigureAwait(false);

	public override int GetCount(Expression<Func<TgEfMessageEntity, bool>> where) => 
		EfContext.Messages.AsNoTracking().Where(where).Count();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfMessageEntity, bool>> where) => 
		await EfContext.Messages.AsNoTracking().Where(where).CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfOperResult<TgEfMessageEntity> DeleteAll()
	{
		TgEfOperResult<TgEfMessageEntity> operResult = GetList(0, 0, isNoTracking: false);
		if (operResult.IsExists)
		{
			foreach (TgEfMessageEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfOperResult<TgEfMessageEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfMessageEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfMessageEntity> operResult = await GetListAsync(0, 0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExists)
		{
			foreach (TgEfMessageEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfOperResult<TgEfMessageEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}