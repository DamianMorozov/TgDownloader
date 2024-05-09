// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Versions;

public sealed class TgEfVersionRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfVersionEntity>(efContext), ITgEfVersionRepository
{
	#region Public and private methods

	public override TgEfOperResult<TgEfVersionEntity> Get(TgEfVersionEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfVersionEntity> operResult = base.Get(item, isNoTracking);
		if (operResult.IsExists)
			return operResult;
		TgEfVersionEntity? itemFind = isNoTracking
			? EfContext.Versions.AsNoTracking()
				.SingleOrDefault(x => x.Version == item.Version)
			: EfContext.Versions.AsTracking()
				.SingleOrDefault(x => x.Version == item.Version);
		return itemFind is not null
			? new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfOperResult<TgEfVersionEntity>> GetAsync(TgEfVersionEntity item, bool isNoTracking)
	{
		TgEfOperResult<TgEfVersionEntity> operResult = await base.GetAsync(item, isNoTracking).ConfigureAwait(false);
		if (operResult.IsExists)
			return operResult;
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
			? new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, itemFind)
			: new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfOperResult<TgEfVersionEntity> GetFirst(bool isNoTracking)
	{
		TgEfVersionEntity? item = isNoTracking
			? EfContext.Versions.AsNoTracking().FirstOrDefault()
			: EfContext.Versions.AsTracking().FirstOrDefault();
		return item is null
			? new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfOperResult<TgEfVersionEntity>> GetFirstAsync(bool isNoTracking)
	{
		TgEfVersionEntity? item = isNoTracking
			? await EfContext.Versions.AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false)
			: await EfContext.Versions.AsTracking().FirstOrDefaultAsync().ConfigureAwait(false);
		return item is null
			? new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.NotExists)
			: new TgEfOperResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfOperResult<TgEfVersionEntity> GetList(TgEnumTableTopRecords topRecords, bool isNoTracking) => 
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

	private TgEfOperResult<TgEfVersionEntity> GetList(int count, bool isNoTracking)
	{
		IList<TgEfVersionEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().Take(count).AsEnumerable().ToList()
				: EfContext.Versions.AsTracking().Take(count).AsEnumerable().ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().AsEnumerable().ToList()
				: EfContext.Versions.AsTracking().AsEnumerable().ToList();
		}

		return new TgEfOperResult<TgEfVersionEntity>(
			items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfOperResult<TgEfVersionEntity>> GetListAsync(TgEnumTableTopRecords topRecords,
		bool isNoTracking) => topRecords switch
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

	private async Task<TgEfOperResult<TgEfVersionEntity>> GetListAsync(int count, bool isNoTracking)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IList<TgEfVersionEntity> items;
		if (count > 0)
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().Take(count).AsEnumerable().ToList()
				: EfContext.Versions.Take(count).AsEnumerable().ToList();
		}
		else
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().AsEnumerable().ToList()
				: EfContext.Versions.AsTracking().AsEnumerable().ToList();
		}

		return new TgEfOperResult<TgEfVersionEntity>(
			items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Versions.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() =>
		await EfContext.Versions.AsNoTracking().CountAsync().ConfigureAwait(false);

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfOperResult<TgEfVersionEntity> DeleteAll()
	{
		TgEfOperResult<TgEfVersionEntity> operResult = GetList(0, isNoTracking: false);
		if (operResult.IsExists)
		{
			foreach (TgEfVersionEntity item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}

		return new TgEfOperResult<TgEfVersionEntity>(operResult.IsExists
			? TgEnumEntityState.IsDeleted
			: TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfOperResult<TgEfVersionEntity>> DeleteAllAsync()
	{
		TgEfOperResult<TgEfVersionEntity> operResult =
			await GetListAsync(0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExists)
		{
			foreach (TgEfVersionEntity item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}

		return new TgEfOperResult<TgEfVersionEntity>(operResult.IsExists
			? TgEnumEntityState.IsDeleted
			: TgEnumEntityState.NotDeleted);
	}

	#endregion

	#region Public and private methods - Contract

	public short LastVersion => 24;

	public TgEfVersionEntity GetLastVersion()
	{
		TgEfVersionEntity versionLast = new();
		if (EfContext.IsTableExists(TgEfConstants.TableVersions))
		{
			short defaultVersion = new TgEfVersionEntity().Version;
			List<TgEfVersionEntity> versions = GetList(TgEnumTableTopRecords.All, isNoTracking: true).Items
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