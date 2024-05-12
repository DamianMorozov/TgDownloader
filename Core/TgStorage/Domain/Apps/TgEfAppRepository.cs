// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Apps;

public sealed class TgEfAppRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfAppEntity>(efContext), ITgEfAppRepository
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfAppRepository)}";

	public override TgEfOperResult<TgEfAppEntity> Get(TgEfAppEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfAppEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExists)
			return operResult;
		TgEfAppEntity? itemFind = isNoTracking
			? EfContext.Apps.AsNoTracking()
				.Include(x => x.Proxy)
				.Where(x => x.ApiHash == item.ApiHash)
				.DefaultIfEmpty()
				.SingleOrDefault()
			: EfContext.Apps.AsTracking()
				.Include(x => x.Proxy)
				.Where(x => x.ApiHash == item.ApiHash)
				.DefaultIfEmpty()
				.SingleOrDefault();
		return itemFind is not null
			? new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfOperResult<TgEfAppEntity>> GetAsync(TgEfAppEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfAppEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExists)
			return operResult;
		TgEfAppEntity? itemFind = isNoTracking
			? await EfContext.Apps.AsNoTracking()
				.Include(x => x.Proxy)
				.Where(x => x.ApiHash == item.ApiHash)
				.DefaultIfEmpty()
				.SingleOrDefaultAsync()
				.ConfigureAwait(false)
			: await EfContext.Apps.AsTracking()
				.Include(x => x.Proxy)
				.Where(x => x.ApiHash == item.ApiHash)
				.DefaultIfEmpty()
				.SingleOrDefaultAsync()
				.ConfigureAwait(false);
		return itemFind is not null
			? new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfOperResult<TgEfAppEntity> GetFirst(bool isNoTracking)
	{
		TgEfAppEntity? item = isNoTracking
			? EfContext.Apps.AsNoTracking()
				.Include(x => x.Proxy)
				.DefaultIfEmpty()
				.FirstOrDefault()
			: EfContext.Apps.AsTracking()
				.Include(x => x.Proxy)
				.DefaultIfEmpty()
				.FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfOperResult<TgEfAppEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfAppEntity? item = isNoTracking
			? await EfContext.Apps.AsNoTracking()
				.Include(x => x.Proxy)
				.DefaultIfEmpty()
				.FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Apps.AsTracking()
				.Include(x => x.Proxy)
				.DefaultIfEmpty()
				.FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfOperResult<TgEfAppEntity> GetList(int take, int skip, bool isNoTracking)
	{
		IList<TgEfAppEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Apps.AsNoTracking()
					.Include(x => x.Proxy)
					.Skip(skip).Take(take).ToList()
				: EfContext.Apps.AsTracking()
					.Include(x => x.Proxy)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Apps.AsNoTracking()
					.Include(x => x.Proxy)
					.ToList()
				: EfContext.Apps.AsTracking()
					.Include(x => x.Proxy)
					.ToList();
		}
		return new TgEfOperResult<TgEfAppEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfAppEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfAppEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Apps.AsNoTracking()
					.Include(x => x.Proxy)
					.Skip(skip).Take(take).ToList()
				: EfContext.Apps.AsTracking()
					.Include(x => x.Proxy)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Apps.AsNoTracking()
					.Include(x => x.Proxy)
					.ToList()
				: EfContext.Apps.AsTracking()
					.Include(x => x.Proxy)
					.ToList();
		}
		return new TgEfOperResult<TgEfAppEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override TgEfOperResult<TgEfAppEntity> GetList(int take, int skip, Expression<Func<TgEfAppEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfAppEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Apps.AsNoTracking()
					.Where(where)
					.Include(x => x.Proxy)
					.Skip(skip).Take(take).ToList()
				: EfContext.Apps.AsTracking()
					.Where(where)
					.Include(x => x.Proxy)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Apps.AsNoTracking()
					.Where(where)
					.Include(x => x.Proxy)
					.ToList()
				: EfContext.Apps.AsTracking()
					.Where(where)
					.Include(x => x.Proxy)
					.ToList();
		}
		return new TgEfOperResult<TgEfAppEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfAppEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfAppEntity, bool>> where, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfAppEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Apps.AsNoTracking()
					.Where(where)
					.Include(x => x.Proxy)
					.Skip(skip).Take(take).ToList()
				: EfContext.Apps.AsTracking()
					.Where(where)
					.Include(x => x.Proxy)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Apps.AsNoTracking()
					.Where(where)
					.Include(x => x.Proxy)
					.ToList()
				: EfContext.Apps.AsTracking()
					.Where(where)
					.Include(x => x.Proxy)
					.ToList();
		}
		return new TgEfOperResult<TgEfAppEntity>(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Apps.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() => await EfContext.Apps.AsNoTracking().CountAsync().ConfigureAwait(false);

	public override int GetCount(Expression<Func<TgEfAppEntity, bool>> where) => 
		EfContext.Apps.AsNoTracking().Where(where).Count();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfAppEntity, bool>> where) => 
		await EfContext.Apps.AsNoTracking().Where(where).CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfOperResult<TgEfAppEntity> DeleteAll()
	{
		TgEfOperResult<TgEfAppEntity> operResult = GetList(0, 0, isNoTracking: false);
		if (operResult.IsExists)
		{
			foreach (TgEfAppEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfOperResult<TgEfAppEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfAppEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfAppEntity> operResult = await GetListAsync(0, 0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExists)
		{
			foreach (TgEfAppEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfOperResult<TgEfAppEntity>(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion

	#region Public and private methods - ITgEfAppRepository

	public TgEfOperResult<TgEfAppEntity> GetCurrentApp()
	{
		TgEfAppEntity? item =
			EfContext.Apps.AsTracking()
				.Where(x => x.Uid != Guid.Empty)
				.Include(x => x.Proxy)
				.DefaultIfEmpty()
				.FirstOrDefault();
		return item is not null
			? new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.IsExists, item)
			: new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.NotExists, new TgEfAppEntity());
	}

	public async Task<TgEfOperResult<TgEfAppEntity>> GetCurrentAppAsync()
	{
		TgEfAppEntity? item = await
			EfContext.Apps.AsTracking()
				.Where(x => x.Uid != Guid.Empty)
				.Include(x => x.Proxy)
				.DefaultIfEmpty()
				.FirstOrDefaultAsync();
		return item is not null
			? new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.IsExists, item)
			: new TgEfOperResult<TgEfAppEntity>(TgEnumEntityState.NotExists, new TgEfAppEntity());
	}

	public Guid GetCurrentAppUid() => GetCurrentApp().Item.Uid;

	public async Task<Guid> GetCurrentAppUidAsync() => (await GetCurrentAppAsync()).Item.Uid;

	#endregion
}