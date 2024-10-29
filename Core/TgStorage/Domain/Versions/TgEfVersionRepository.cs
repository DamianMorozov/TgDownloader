// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Versions;

public sealed class TgEfVersionRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfVersionEntity>(efContext), ITgEfVersionRepository
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfVersionRepository)}";

	public override TgEfStorageResult<TgEfVersionEntity> Get(TgEfVersionEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfVersionEntity> storageResult = base.Get(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		TgEfVersionEntity? itemFind = isNoTracking
			? EfContext.Versions.AsNoTracking()
				.Where(x => x.Version == item.Version)
				.SingleOrDefault()
			: EfContext.Versions.AsTracking()
				.Where(x => x.Version == item.Version)
				.SingleOrDefault();
		return itemFind is not null
			? new TgEfStorageResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfVersionEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfVersionEntity>> GetAsync(TgEfVersionEntity item, bool isNoTracking)
	{
		TgEfStorageResult<TgEfVersionEntity> storageResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (storageResult.IsExists)
			return storageResult;
		TgEfVersionEntity? itemFind = isNoTracking
			? await EfContext.Versions.AsNoTracking()
				.Where(x => x.Version == item.Version)
				.SingleOrDefaultAsync()
				.ConfigureAwait(false)
			: await EfContext.Versions.AsTracking()
				.Where(x => x.Version == item.Version)
				.SingleOrDefaultAsync()
				.ConfigureAwait(false);
		return itemFind is not null
			? new TgEfStorageResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfVersionEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfStorageResult<TgEfVersionEntity> GetFirst(bool isNoTracking)
	{
		TgEfVersionEntity? item = isNoTracking
			? EfContext.Versions.AsNoTracking().FirstOrDefault()
			: EfContext.Versions.AsTracking().FirstOrDefault();
		return item is null
			? new TgEfStorageResult<TgEfVersionEntity>(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfVersionEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfVersionEntity? item = isNoTracking
			? await EfContext.Versions.AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Versions.AsTracking().FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfStorageResult<TgEfVersionEntity>(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfStorageResult<TgEfVersionEntity> GetList(int take, int skip, bool isNoTracking)
	{
		IList<TgEfVersionEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().Skip(skip).Take(take).ToList()
				: EfContext.Versions.AsTracking().Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().ToList()
				: EfContext.Versions.AsTracking().ToList();
		}

		return new TgEfStorageResult<TgEfVersionEntity>(
			items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfVersionEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfVersionEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().Skip(skip).Take(take).ToList()
				: EfContext.Versions.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().ToList()
				: EfContext.Versions.AsTracking().ToList();
		}

		return new TgEfStorageResult<TgEfVersionEntity>(
			items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override TgEfStorageResult<TgEfVersionEntity> GetList(int take, int skip, Expression<Func<TgEfVersionEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfVersionEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList()
				: EfContext.Versions.AsTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking()
					.Where(where)
					.ToList()
				: EfContext.Versions.AsTracking()
					.Where(where)
					.ToList();
		}

		return new TgEfStorageResult<TgEfVersionEntity>(
			items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfVersionEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfVersionEntity, bool>> where, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfVersionEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking()
					.Where(where)
					.Skip(skip).Take(take).ToList()
				: EfContext.Versions.Take(take)
					.Where(where)
					.ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking()
					.Where(where)
					.ToList()
				: EfContext.Versions.AsTracking()
					.Where(where)
					.ToList();
		}

		return new TgEfStorageResult<TgEfVersionEntity>(
			items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Versions.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() =>
		await EfContext.Versions.AsNoTracking().CountAsync().ConfigureAwait(false);

	public override int GetCount(Expression<Func<TgEfVersionEntity, bool>> where) => 
		EfContext.Versions.AsNoTracking().Where(where).Count();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfVersionEntity, bool>> where) =>
		await EfContext.Versions.AsNoTracking().Where(where).CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfStorageResult<TgEfVersionEntity> DeleteAll()
	{
		TgEfStorageResult<TgEfVersionEntity> storageResult = GetList(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TgEfVersionEntity item in storageResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}

		return new TgEfStorageResult<TgEfVersionEntity>(storageResult.IsExists
			? TgEnumEntityState.IsDeleted
			: TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfStorageResult<TgEfVersionEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TgEfVersionEntity> storageResult =
			await GetListAsync(0, 0, isNoTracking: false).ConfigureAwait(false);
		if (storageResult.IsExists)
		{
			foreach (TgEfVersionEntity item in storageResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}

		return new TgEfStorageResult<TgEfVersionEntity>(storageResult.IsExists
			? TgEnumEntityState.IsDeleted
			: TgEnumEntityState.NotDeleted);
	}

	#endregion

	#region Public and private methods - ITgEfVersionRepository

	public short LastVersion => 24;

	public TgEfVersionEntity GetLastVersion()
	{
		TgEfVersionEntity versionLast = new();
		if (EfContext.IsTableExists(TgEfConstants.TableVersions))
		{
			short defaultVersion = new TgEfVersionEntity().Version;
			List<TgEfVersionEntity> versions = GetList(TgEnumTableTopRecords.All, 0, isNoTracking: true).Items
				.Where(x => x.Version != defaultVersion).OrderBy(x => x.Version).ToList();
			if (versions.Any())
				versionLast = versions[^1];
		}
		return versionLast;
	}

	public void FillTableVersions()
	{
		DeleteNew();
		bool isLast = false;
		while (!isLast)
		{
			TgEfVersionEntity versionLast = GetLastVersion();
			if (Equals(versionLast.Version, new TgEfVersionEntity().Version))
				versionLast.Version = 0;
			switch (versionLast.Version)
			{
				case 0:
					Save(new() { Version = 1, Description = "Added versions table" });
					break;
				case 1:
					Save(new() { Version = 2, Description = "Added apps table" });
					break;
				case 2:
					Save(new() { Version = 3, Description = "Added documents table" });
					break;
				case 3:
					Save(new() { Version = 4, Description = "Added filters table" });
					break;
				case 4:
					Save(new() { Version = 5, Description = "Added messages table" });
					break;
				case 5:
					Save(new() { Version = 6, Description = "Added proxies table" });
					break;
				case 6:
					Save(new() { Version = 7, Description = "Added sources table" });
					break;
				case 7:
					Save(new() { Version = 8, Description = "Added source settings table" });
					break;
				case 8:
					Save(new() { Version = 9, Description = "Upgrade versions table" });
					break;
				case 9:
					Save(new() { Version = 10, Description = "Upgrade apps table" });
					break;
				case 10:
					Save(new() { Version = 11, Description = "Upgrade storage on XPO framework" });
					break;
				case 11:
					Save(new() { Version = 12, Description = "Upgrade apps table" });
					break;
				case 12:
					Save(new() { Version = 13, Description = "Upgrade documents table" });
					break;
				case 13:
					Save(new() { Version = 14, Description = "Upgrade filters table" });
					break;
				case 14:
					Save(new() { Version = 15, Description = "Upgrade messages table" });
					break;
				case 15:
					Save(new() { Version = 16, Description = "Upgrade proxies table" });
					break;
				case 16:
					Save(new() { Version = 17, Description = "Upgrade sources table" });
					break;
				case 17:
					Save(new() { Version = 18, Description = "Updating the UID field in the apps table" });
					break;
				case 18:
					Save(new() { Version = 19, Description = "Updating the UID field in the documents table" });
					break;
				case 19:
					Save(new() { Version = 20, Description = "Updating the UID field in the filters table" });
					break;
				case 20:
					Save(new() { Version = 21, Description = "Updating the UID field in the messages table" });
					break;
				case 21:
					Save(new() { Version = 22, Description = "Updating the UID field in the proxies table" });
					break;
				case 22:
					Save(new() { Version = 23, Description = "Updating the UID field in the sources table" });
					break;
				case 23:
					Save(new() { Version = 24, Description = "Updating the UID field in the versions table" });
					break;
			}
			if (versionLast.Version >= LastVersion)
				isLast = true;
		}
	}

	public async Task FillTableVersionsAsync()
	{
		await DeleteNewAsync();
		bool isLast = false;
		while (!isLast)
		{
			TgEfVersionEntity versionLast = GetLastVersion();
			if (Equals(versionLast.Version, new TgEfVersionEntity().Version))
				versionLast.Version = 0;
			switch (versionLast.Version)
			{
				case 0:
					await SaveAsync(new() { Version = 1, Description = "Added versions table" });
					break;
				case 1:
					await SaveAsync(new() { Version = 2, Description = "Added apps table" });
					break;
				case 2:
					await SaveAsync(new() { Version = 3, Description = "Added documents table" });
					break;
				case 3:
					await SaveAsync(new() { Version = 4, Description = "Added filters table" });
					break;
				case 4:
					await SaveAsync(new() { Version = 5, Description = "Added messages table" });
					break;
				case 5:
					await SaveAsync(new() { Version = 6, Description = "Added proxies table" });
					break;
				case 6:
					await SaveAsync(new() { Version = 7, Description = "Added sources table" });
					break;
				case 7:
					await SaveAsync(new() { Version = 8, Description = "Added source settings table" });
					break;
				case 8:
					await SaveAsync(new() { Version = 9, Description = "Upgrade versions table" });
					break;
				case 9:
					await SaveAsync(new() { Version = 10, Description = "Upgrade apps table" });
					break;
				case 10:
					await SaveAsync(new() { Version = 11, Description = "Upgrade storage on XPO framework" });
					break;
				case 11:
					await SaveAsync(new() { Version = 12, Description = "Upgrade apps table" });
					break;
				case 12:
					await SaveAsync(new() { Version = 13, Description = "Upgrade documents table" });
					break;
				case 13:
					await SaveAsync(new() { Version = 14, Description = "Upgrade filters table" });
					break;
				case 14:
					await SaveAsync(new() { Version = 15, Description = "Upgrade messages table" });
					break;
				case 15:
					await SaveAsync(new() { Version = 16, Description = "Upgrade proxies table" });
					break;
				case 16:
					await SaveAsync(new() { Version = 17, Description = "Upgrade sources table" });
					break;
				case 17:
					await SaveAsync(new() { Version = 18, Description = "Updating the UID field in the apps table" });
					break;
				case 18:
					await SaveAsync(new() { Version = 19, Description = "Updating the UID field in the documents table" });
					break;
				case 19:
					await SaveAsync(new() { Version = 20, Description = "Updating the UID field in the filters table" });
					break;
				case 20:
					await SaveAsync(new() { Version = 21, Description = "Updating the UID field in the messages table" });
					break;
				case 21:
					await SaveAsync(new() { Version = 22, Description = "Updating the UID field in the proxies table" });
					break;
				case 22:
					await SaveAsync(new() { Version = 23, Description = "Updating the UID field in the sources table" });
					break;
				case 23:
					await SaveAsync(new() { Version = 24, Description = "Updating the UID field in the versions table" });
					break;
			}
			if (versionLast.Version >= LastVersion)
				isLast = true;
		}
	}

	#endregion
}