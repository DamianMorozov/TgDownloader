// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Sources;

public sealed class TgEfSourceRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfSourceEntity>(efContext)
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfSourceRepository)}";

	public override TgEfOperResult<TgEfSourceEntity> Get(TgEfSourceEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfSourceEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExists)
			return operResult;
		TgEfSourceEntity? itemFind = isNoTracking
			? EfContext.Sources.AsNoTracking()
				.SingleOrDefault(x => x.Id == item.Id)
			: EfContext.Sources.AsTracking()
				.SingleOrDefault(x => x.Id == item.Id);
		return itemFind is not null
			? new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfOperResult<TgEfSourceEntity>> GetAsync(TgEfSourceEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfSourceEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExists)
			return operResult;
		TgEfSourceEntity? itemFind = isNoTracking
			? await EfContext.Sources.AsNoTracking()
				.Where(x => x.Id == item.Id)
				.SingleOrDefaultAsync()
				.ConfigureAwait(false)
			: await EfContext.Sources.AsTracking()
				.Where(x => x.Id == item.Id)
				.SingleOrDefaultAsync()
				.ConfigureAwait(false);
		return itemFind is not null
			? new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfOperResult<TgEfSourceEntity> GetFirst(bool isNoTracking)
	{
		TgEfSourceEntity? item = isNoTracking
			? EfContext.Sources.AsNoTracking().FirstOrDefault()
			: EfContext.Sources.AsTracking().FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfOperResult<TgEfSourceEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfSourceEntity? item = isNoTracking
			? await EfContext.Sources.AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Sources.AsTracking().FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfSourceEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfOperResult<TgEfSourceEntity> GetList(int take, int skip, bool isNoTracking)
	{
		IList<TgEfSourceEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking().Skip(skip).Take(take).ToList()
				: EfContext.Sources.AsTracking().Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking().ToList()
				: EfContext.Sources.AsTracking().ToList();
		}
		return new TgEfOperResult<TgEfSourceEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfSourceEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfSourceEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking().Skip(skip).Take(take).ToList()
				: EfContext.Sources.AsTracking().Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking().ToList()
				: EfContext.Sources.AsTracking().ToList();
		}
		return new TgEfOperResult<TgEfSourceEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override TgEfOperResult<TgEfSourceEntity> GetList(int take, int skip, Expression<Func<TgEfSourceEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfSourceEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList()
				: EfContext.Sources.AsTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking()
					.Where(where)
					.ToList()
				: EfContext.Sources.AsTracking()
					.Where(where)
					.ToList();
		}
		return new TgEfOperResult<TgEfSourceEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfSourceEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfSourceEntity, bool>> where, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfSourceEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList()
				: EfContext.Sources.AsTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Sources.AsNoTracking()
					.Where(where)
					.ToList()
				: EfContext.Sources.AsTracking()
					.Where(where)
					.ToList();
		}
		return new TgEfOperResult<TgEfSourceEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Sources.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Sources.AsNoTracking().CountAsync().ConfigureAwait(false);

	public override int GetCount(Expression<Func<TgEfSourceEntity, bool>> where) => 
		EfContext.Sources.AsNoTracking().Where(where).Count();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfSourceEntity, bool>> where) => 
		await EfContext.Sources.AsNoTracking().Where(where).CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfOperResult<TgEfSourceEntity> DeleteAll()
	{
		TgEfOperResult<TgEfSourceEntity> operResult = GetList(0, 0, isNoTracking: false);
		if (operResult.IsExists)
		{
			foreach (TgEfSourceEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfOperResult<TgEfSourceEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfSourceEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfSourceEntity> operResult = await GetListAsync(0, 0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExists)
		{
			foreach (TgEfSourceEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfOperResult<TgEfSourceEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}