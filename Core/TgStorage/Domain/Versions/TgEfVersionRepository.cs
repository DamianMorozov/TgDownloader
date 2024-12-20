﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Versions;

public sealed class TgEfVersionRepository(TgEfContext efContext) : TgEfRepositoryBase<TgEfVersionEntity>(efContext), ITgEfVersionRepository
{
	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfVersionRepository)}";

	public override TgEfStorageResult<TgEfVersionEntity> Get(TgEfVersionEntity item, bool isNoTracking)
	{
		var storageResult = base.Get(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		var itemFind = isNoTracking
			? EfContext.Versions
				.AsNoTracking()
				.SingleOrDefault(x => x.Version == item.Version)
			: EfContext.Versions
				.AsTracking()
				.SingleOrDefault(x => x.Version == item.Version);
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfVersionEntity>(TgEnumEntityState.NotExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfVersionEntity>> GetAsync(TgEfVersionEntity item, bool isNoTracking)
	{
		var storageResult = await base.GetAsync(item, isNoTracking);
		if (storageResult.IsExists)
			return storageResult;
		var itemFind = isNoTracking
			? await EfContext.Versions.AsNoTracking()
				.Where(x => x.Version == item.Version)
				.SingleOrDefaultAsync()
			: await EfContext.Versions.AsTracking()
				.Where(x => x.Version == item.Version)
				.SingleOrDefaultAsync();
		return itemFind is not null
			? new(TgEnumEntityState.IsExists, itemFind)
			: new TgEfStorageResult<TgEfVersionEntity>(TgEnumEntityState.NotExists, item);
	}

	public override TgEfStorageResult<TgEfVersionEntity> GetFirst(bool isNoTracking)
	{
		var item = isNoTracking
			? EfContext.Versions.AsNoTracking().FirstOrDefault()
			: EfContext.Versions.AsTracking().FirstOrDefault();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, item);
	}

	public override async Task<TgEfStorageResult<TgEfVersionEntity>> GetFirstAsync(bool isNoTracking)
	{
		var item = isNoTracking
			? await EfContext.Versions.AsNoTracking().FirstOrDefaultAsync()
			: await EfContext.Versions.AsTracking().FirstOrDefaultAsync();
		return item is null
			? new(TgEnumEntityState.NotExists)
			: new TgEfStorageResult<TgEfVersionEntity>(TgEnumEntityState.IsExists, item);
	}

	public override TgEfStorageResult<TgEfVersionEntity> GetList(int take, int skip, bool isNoTracking)
	{
		IList<TgEfVersionEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().Skip(skip).Take(take).ToList()
				: [.. EfContext.Versions.AsTracking().Skip(skip).Take(take)];
		}
		else
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking().ToList()
				: [.. EfContext.Versions.AsTracking()];
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfVersionEntity>> GetListAsync(int take, int skip, bool isNoTracking)
	{
		IList<TgEfVersionEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? await EfContext.Versions.AsNoTracking().Skip(skip).Take(take).ToListAsync()
				: await EfContext.Versions.Skip(skip).Take(take).ToListAsync();
		}
		else
		{
			items = isNoTracking
				? await EfContext.Versions.AsNoTracking().ToListAsync()
				: await EfContext.Versions.AsTracking().ToListAsync();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
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
				: [.. EfContext.Versions.AsTracking()
					.Where(where)
					.Skip(skip).Take(take)];
		}
		else
		{
			items = isNoTracking
				? EfContext.Versions.AsNoTracking()
					.Where(where)
					.ToList()
				: [.. EfContext.Versions.AsTracking().Where(where)];
		}

		return new(
			items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override async Task<TgEfStorageResult<TgEfVersionEntity>> GetListAsync(int take, int skip, Expression<Func<TgEfVersionEntity, bool>> where, bool isNoTracking)
	{
		IList<TgEfVersionEntity> items;
		if (take > 0)
		{
			items = isNoTracking
				? await EfContext.Versions.AsNoTracking().Where(where).Skip(skip).Take(take).ToListAsync()
				: await EfContext.Versions.Take(take).Where(where).ToListAsync();
		}
		else
		{
			items = isNoTracking
				? await EfContext.Versions.AsNoTracking().Where(where).ToListAsync()
				: await EfContext.Versions.AsTracking().Where(where).ToListAsync();
		}
		return new(items.Any() ? TgEnumEntityState.IsExists : TgEnumEntityState.NotExists, items);
	}

	public override int GetCount() => EfContext.Versions.AsNoTracking().Count();

	public override async Task<int> GetCountAsync() =>
		await EfContext.Versions.AsNoTracking().CountAsync();

	public override int GetCount(Expression<Func<TgEfVersionEntity, bool>> where) => 
		EfContext.Versions.AsNoTracking().Where(where).Count();

	public override async Task<int> GetCountAsync(Expression<Func<TgEfVersionEntity, bool>> where) =>
		await EfContext.Versions.AsNoTracking().Where(where).CountAsync();

	#endregion

	#region Public and private methods - Write

	//

	#endregion

	#region Public and private methods - Delete

	public override TgEfStorageResult<TgEfVersionEntity> DeleteAll()
	{
		var storageResult = GetList(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (var item in storageResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists
			? TgEnumEntityState.IsDeleted
			: TgEnumEntityState.NotDeleted);
	}

	public override async Task<TgEfStorageResult<TgEfVersionEntity>> DeleteAllAsync()
	{
		var storageResult =
			await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (var item in storageResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists
			? TgEnumEntityState.IsDeleted
			: TgEnumEntityState.NotDeleted);
	}

	#endregion

	#region Public and private methods - ITgEfVersionRepository

	public short LastVersion => 28;

	public async Task<TgEfVersionEntity> GetLastVersionAsync()
	{
		TgEfVersionEntity versionLast = new();
		if (EfContext.IsTableExists(TgEfConstants.TableVersions))
		{
			var defaultVersion = new TgEfVersionEntity().Version;
			var versions = (await GetListAsync(TgEnumTableTopRecords.All, 0, isNoTracking: true)).Items
				.Where(x => x.Version != defaultVersion).OrderBy(x => x.Version).ToList();
			if (versions.Any())
				versionLast = versions[^1];
		}
		return versionLast;
	}

	public async Task FillTableVersionsAsync()
	{
		await DeleteNewAsync();
		var isLast = false;
		while (!isLast)
		{
			var versionLast = await GetLastVersionAsync();
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
					await SaveAsync(new() { Version = 8, Description = "Updated sources table" });
					break;
				case 8:
					await SaveAsync(new() { Version = 9, Description = "Updated versions table" });
					break;
				case 9:
					await SaveAsync(new() { Version = 10, Description = "Updated apps table" });
					break;
				case 10:
					await SaveAsync(new() { Version = 11, Description = "Upgraded storage to XPO framework" });
					break;
				case 11:
					await SaveAsync(new() { Version = 12, Description = "Updated apps table" });
					break;
				case 12:
					await SaveAsync(new() { Version = 13, Description = "Updated documents table" });
					break;
				case 13:
					await SaveAsync(new() { Version = 14, Description = "Updated filters table" });
					break;
				case 14:
					await SaveAsync(new() { Version = 15, Description = "Updated messages table" });
					break;
				case 15:
					await SaveAsync(new() { Version = 16, Description = "Updated proxies table" });
					break;
				case 16:
					await SaveAsync(new() { Version = 17, Description = "Updated sources table" });
					break;
				case 17:
					await SaveAsync(new() { Version = 18, Description = "Updated the UID field in the apps table" });
					break;
				case 18:
					await SaveAsync(new() { Version = 19, Description = "Updated the UID field in the documents table" });
					break;
				case 19:
					await SaveAsync(new() { Version = 20, Description = "Updated the UID field in the filters table" });
					break;
				case 20:
					await SaveAsync(new() { Version = 21, Description = "Updated the UID field in the messages table" });
					break;
				case 21:
					await SaveAsync(new() { Version = 22, Description = "Updated the UID field in the proxies table" });
					break;
				case 22:
					await SaveAsync(new() { Version = 23, Description = "Updated the UID field in the sources table" });
					break;
				case 23:
					await SaveAsync(new() { Version = 24, Description = "Updated the UID field in the versions table" });
					break;
				case 24:
					await SaveAsync(new() { Version = 25, Description = "Migrated storage to EF Core" });
					break;
				case 25:
					await SaveAsync(new() { Version = 26, Description = "Updated apps table" });
					break;
				case 26:
					await SaveAsync(new() { Version = 27, Description = "Added contacts table" });
					break;
				case 27:
					await SaveAsync(new() { Version = 28, Description = "Added stories table" });
					break;
			}
			if (versionLast.Version >= LastVersion)
				isLast = true;
		}
	}

	#endregion
}