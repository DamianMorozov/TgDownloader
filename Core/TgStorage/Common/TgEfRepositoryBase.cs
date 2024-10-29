// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

public abstract class TgEfRepositoryBase<TEntity>(TgEfContext efContext) : TgCommonBase, ITgEfRepository<TEntity>
	where TEntity : ITgDbFillEntity<TEntity>, new()
{
	#region Public and private fields, properties, constructor

	public TgEfContext EfContext { get; } = efContext;

	#endregion

	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfRepositoryBase<TEntity>)}";

	private TgEfStorageResult<TEntity> UseOverrideMethod() => throw new NotImplementedException(TgLocaleHelper.Instance.UseOverrideMethod);

	private async Task<TgEfStorageResult<TEntity>> UseOverrideMethodAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		throw new NotImplementedException(TgLocaleHelper.Instance.UseOverrideMethod);
	}

	private TgEfStorageResult<TEntity> Get(Guid uid)
	{
		//TEntity? item = EfContext.Find<TEntity>(uid);
		TEntity? item = default;
		switch (typeof(TEntity))
		{
			case var cls when cls == typeof(TgEfAppEntity):
				TgEfAppEntity? app = EfContext.Apps.Find(uid);
				if (app is TEntity appEntity)
					item = appEntity;
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				TgEfDocumentEntity? document = EfContext.Documents.Find(uid);
				if (document is TEntity documentEntity)
					item = documentEntity;
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				TgEfFilterEntity? filter = EfContext.Filters.Find(uid);
				if (filter is TEntity filterEntity)
					item = filterEntity;
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				TgEfMessageEntity? message = EfContext.Messages.Find(uid);
				if (message is TEntity messageEntity)
					item = messageEntity;
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				TgEfProxyEntity? proxy = EfContext.Proxies.Find(uid);
				if (proxy is TEntity proxyEntity)
					item = proxyEntity;
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				TgEfSourceEntity? source = EfContext.Sources.Find(uid);
				if (source is TEntity sourceEntity)
					item = sourceEntity;
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				TgEfVersionEntity? version = EfContext.Versions.Find(uid);
				if (version is TEntity versionEntity)
					item = versionEntity;
				break;
		}
		return item is not null
			? new(TgEnumEntityState.IsExists, item)
			: new TgEfStorageResult<TEntity>(TgEnumEntityState.NotExists);
	}

	private async Task<TgEfStorageResult<TEntity>> GetAsync(Guid uid)
	{
		//TEntity? item = await EfContext.FindAsync<TEntity>(uid).ConfigureAwait(false);
		TEntity? item = default;
		switch (typeof(TEntity))
		{
			case var cls when cls == typeof(TgEfAppEntity):
				TgEfAppEntity? app = await EfContext.Apps.FindAsync(uid).ConfigureAwait(false);
				if (app is TEntity appEntity)
					item = appEntity;
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				TgEfDocumentEntity? document = await EfContext.Documents.FindAsync(uid).ConfigureAwait(false);
				if (document is TEntity documentEntity)
					item = documentEntity;
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				TgEfFilterEntity? filter = await EfContext.Filters.FindAsync(uid).ConfigureAwait(false);
				if (filter is TEntity filterEntity)
					item = filterEntity;
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				TgEfMessageEntity? message = await EfContext.Messages.FindAsync(uid).ConfigureAwait(false);
				if (message is TEntity messageEntity)
					item = messageEntity;
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				TgEfProxyEntity? proxy = await EfContext.Proxies.FindAsync(uid).ConfigureAwait(false);
				if (proxy is TEntity proxyEntity)
					item = proxyEntity;
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				TgEfSourceEntity? source = await EfContext.Sources.FindAsync(uid).ConfigureAwait(false);
				if (source is TEntity sourceEntity)
					item = sourceEntity;
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				TgEfVersionEntity? version = await EfContext.Versions.FindAsync(uid).ConfigureAwait(false);
				if (version is TEntity versionEntity)
					item = versionEntity;
				break;
		}
		return item is not null
			? new(TgEnumEntityState.IsExists, item)
			: new TgEfStorageResult<TEntity>(TgEnumEntityState.NotExists);
	}

	#endregion

	#region Public and private methods - Read

	public virtual TgEfStorageResult<TEntity> Get(TEntity item, bool isNoTracking) => Get(item.Uid);

	public virtual async Task<TgEfStorageResult<TEntity>> GetAsync(TEntity item, bool isNoTracking) => await GetAsync(item.Uid).ConfigureAwait(false);

	public virtual TgEfStorageResult<TEntity> GetNew(bool isNoTracking) => Get(new(), isNoTracking);

	public virtual async Task<TgEfStorageResult<TEntity>> GetNewAsync(bool isNoTracking) => await GetAsync(new(), isNoTracking).ConfigureAwait(false);

	public virtual TgEfStorageResult<TEntity> GetFirst(bool isNoTracking) => UseOverrideMethod();

	public virtual async Task<TgEfStorageResult<TEntity>> GetFirstAsync(bool isNoTracking) => await UseOverrideMethodAsync().ConfigureAwait(false);

	public virtual TgEfStorageResult<TEntity> GetList(TgEnumTableTopRecords topRecords, int skip, bool isNoTracking) =>
		topRecords switch
		{
			TgEnumTableTopRecords.Top1 => GetList(1, skip, isNoTracking),
			TgEnumTableTopRecords.Top20 => GetList(20, skip, isNoTracking),
			TgEnumTableTopRecords.Top100 => GetList(200, skip, isNoTracking),
			TgEnumTableTopRecords.Top1000 => GetList(1_000, skip, isNoTracking),
			TgEnumTableTopRecords.Top10000 => GetList(10_000, skip, isNoTracking),
			TgEnumTableTopRecords.Top100000 => GetList(100_000, skip, isNoTracking),
			TgEnumTableTopRecords.Top1000000 => GetList(1_000_000, skip, isNoTracking),
			_ => GetList(0, skip, isNoTracking),
		};

	public virtual async Task<TgEfStorageResult<TEntity>> GetListAsync(TgEnumTableTopRecords topRecords, int skip, bool isNoTracking) =>
		topRecords switch
		{
			TgEnumTableTopRecords.Top1 => await GetListAsync(1, skip, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top20 => await GetListAsync(20, skip, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top100 => await GetListAsync(200, skip, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top1000 => await GetListAsync(1_000, skip, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top10000 => await GetListAsync(10_000, skip, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top100000 => await GetListAsync(100_000, skip, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top1000000 => await GetListAsync(1_000_000, skip, isNoTracking).ConfigureAwait(false),
			_ => await GetListAsync(0, skip, isNoTracking).ConfigureAwait(false),
		};

	public virtual TgEfStorageResult<TEntity> GetList(int take, int skip, bool isNoTracking) => UseOverrideMethod();

	public virtual async Task<TgEfStorageResult<TEntity>> GetListAsync(int take, int skip, bool isNoTracking) => await UseOverrideMethodAsync().ConfigureAwait(false);

	public virtual TgEfStorageResult<TEntity> GetList(TgEnumTableTopRecords topRecords, int skip, Expression<Func<TEntity, bool>> where, bool isNoTracking) =>
		topRecords switch
		{
			TgEnumTableTopRecords.Top1 => GetList(1, skip, where, isNoTracking),
			TgEnumTableTopRecords.Top20 => GetList(20, skip, where, isNoTracking),
			TgEnumTableTopRecords.Top100 => GetList(200, skip, where, isNoTracking),
			TgEnumTableTopRecords.Top1000 => GetList(1_000, skip, where, isNoTracking),
			TgEnumTableTopRecords.Top10000 => GetList(10_000, skip, where, isNoTracking),
			TgEnumTableTopRecords.Top100000 => GetList(100_000, skip, where, isNoTracking),
			TgEnumTableTopRecords.Top1000000 => GetList(1_000_000, skip, where, isNoTracking),
			_ => GetList(0, skip, where, isNoTracking),
		};

	public virtual async Task<TgEfStorageResult<TEntity>> GetListAsync(TgEnumTableTopRecords topRecords, int skip, Expression<Func<TEntity, bool>> where, bool isNoTracking) =>
		topRecords switch
		{
			TgEnumTableTopRecords.Top1 => await GetListAsync(1, skip, where, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top20 => await GetListAsync(20, skip, where, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top100 => await GetListAsync(200, skip, where, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top1000 => await GetListAsync(1_000, skip, where, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top10000 => await GetListAsync(10_000, skip, where, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top100000 => await GetListAsync(100_000, skip, where, isNoTracking).ConfigureAwait(false),
			TgEnumTableTopRecords.Top1000000 => await GetListAsync(1_000_000, skip, where, isNoTracking).ConfigureAwait(false),
			_ => await GetListAsync(0, skip, where, isNoTracking).ConfigureAwait(false),
		};

	public virtual TgEfStorageResult<TEntity> GetList(int take, int skip, Expression<Func<TEntity, bool>> where, bool isNoTracking) => UseOverrideMethod();

	public virtual async Task<TgEfStorageResult<TEntity>> GetListAsync(int take, int skip, Expression<Func<TEntity, bool>> where, bool isNoTracking) =>
		await UseOverrideMethodAsync().ConfigureAwait(false);

	public virtual int GetCount() => 0;

	public virtual async Task<int> GetCountAsync() => await Task.FromResult(0);

	public virtual int GetCount(Expression<Func<TEntity, bool>> where) => 0;

	public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> where) => await Task.FromResult(0);

	#endregion

	#region Public and private methods - Write

	public virtual TgEfStorageResult<TEntity> Save(TEntity item)
	{
		using IDbContextTransaction transaction = EfContext.Database.BeginTransaction();
		TgEfStorageResult<TEntity> storageResult = new(TgEnumEntityState.Unknown, item);
		try
		{
			storageResult = Get(item, isNoTracking: false);
			// Create.
			if (!storageResult.IsExists)
			{
				EfContext.Add(storageResult.Item);
			}
			// Update.
			else
			{
				storageResult.Item.Fill(item, isUidCopy: false);
			}
			// Validate.
			FluentValidation.Results.ValidationResult validationResult = TgEfUtils.GetEfValid(storageResult.Item);
			if (!validationResult.IsValid)
				storageResult.Item.Default();
			TgEfUtils.Normilize(storageResult.Item);
			//item = default;
			EfContext.SaveChanges();
			transaction.Commit();
			storageResult.State = TgEnumEntityState.IsSaved;
		}
		catch (Exception)
		{
			transaction.Rollback();
			storageResult.Item.Default();
			throw;
		}
		return storageResult;
	}

	public virtual async Task<TgEfStorageResult<TEntity>> SaveAsync(TEntity? item)
	{
		IDbContextTransaction transaction = await EfContext.Database.BeginTransactionAsync().ConfigureAwait(false);
		await using (transaction.ConfigureAwait(false))
		{
			TgEfStorageResult<TEntity> storageResult = new(TgEnumEntityState.Unknown, item);
			if (item is null) return storageResult;
			try
			{
				storageResult = await GetAsync(item, isNoTracking: false).ConfigureAwait(false);
				// Create.
				if (!storageResult.IsExists)
				{
					await EfContext.AddAsync(storageResult.Item).ConfigureAwait(false);
				}
				// Update.
				else
				{
					storageResult.Item.Fill(item, false);
				}
				// Validate.
				FluentValidation.Results.ValidationResult validationResult = TgEfUtils.GetEfValid(storageResult.Item);
				if (!validationResult.IsValid)
					storageResult.Item.Default();
				TgEfUtils.Normilize(storageResult.Item);
				item = default;
				await EfContext.SaveChangesAsync().ConfigureAwait(false);
				await transaction.CommitAsync().ConfigureAwait(false);
				storageResult.State = TgEnumEntityState.IsSaved;
			}
			catch (Exception)
			{
				await transaction.RollbackAsync().ConfigureAwait(false);
				storageResult.Item.Default();
				throw;
			}
			return storageResult;
		}
	}

	public virtual TgEfStorageResult<TEntity> SaveList(List<TEntity> items)
	{
		using IDbContextTransaction transaction = EfContext.Database.BeginTransaction();
		TgEfStorageResult<TEntity> storageResult = new(TgEnumEntityState.Unknown, items);
		try
		{
			List<TEntity> uniqueItems = items.Distinct().ToList();
			foreach (TEntity item in uniqueItems)
			{
				TgEfUtils.Normilize(item);
				EfContext.Add(item);
			}
			EfContext.SaveChanges();
			transaction.Commit();
			storageResult.State = TgEnumEntityState.IsSaved;
		}
		catch (Exception)
		{
			transaction.Rollback();
			storageResult.Item.Default();
			throw;
		}
		return storageResult;
	}

	public virtual async Task<TgEfStorageResult<TEntity>> SaveListAsync(List<TEntity> items)
	{
		IDbContextTransaction transaction = await EfContext.Database.BeginTransactionAsync().ConfigureAwait(false);
		await using (transaction.ConfigureAwait(false))
		{
			TgEfStorageResult<TEntity> storageResult = new(TgEnumEntityState.Unknown, items);
			try
			{
				List<TEntity> uniqueItems = items.Distinct().ToList();
				foreach (TEntity item in uniqueItems)
				{
					TgEfUtils.Normilize(item);
					EfContext.Add(item);
				}
				await EfContext.SaveChangesAsync().ConfigureAwait(false);
				await transaction.CommitAsync().ConfigureAwait(false);
				storageResult.State = TgEnumEntityState.IsSaved;
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync().ConfigureAwait(false);
				storageResult.Item.Default();
#if DEBUG
				Debug.WriteLine(ex);
#endif
				throw;
			}
			return storageResult;
		}
	}

	public virtual TgEfStorageResult<TEntity> SaveWithoutTransaction(TEntity item)
	{
		TgEfStorageResult<TEntity> storageResult = new(TgEnumEntityState.Unknown, item);
		try
		{
			storageResult = Get(item, isNoTracking: false);
			// Create.
			if (!storageResult.IsExists)
			{
				EfContext.Add(storageResult.Item);
				EfContext.SaveChanges();
			}
			// Update.
			else
			{
				storageResult.Item.Fill(item, false);
				FluentValidation.Results.ValidationResult validationResult = TgEfUtils.GetEfValid(storageResult.Item);
				if (!validationResult.IsValid)
					storageResult.Item.Default();
				EfContext.SaveChanges();
			}
			storageResult.State = TgEnumEntityState.IsSaved;
		}
		catch (Exception)
		{
			storageResult.Item.Default();
			throw;
		}
		return storageResult;
	}

	public virtual async Task<TgEfStorageResult<TEntity>> SaveWithoutTransactionAsync(TEntity item)
	{
		TgEfStorageResult<TEntity> storageResult = new(TgEnumEntityState.Unknown, item);
		try
		{
			storageResult = await GetAsync(item, isNoTracking: false).ConfigureAwait(false);
			// Create.
			if (!storageResult.IsExists)
			{
				await EfContext.AddAsync(storageResult.Item).ConfigureAwait(false);
				await EfContext.SaveChangesAsync().ConfigureAwait(false);
			}
			// Update.
			else
			{
				storageResult.Item.Fill(item, false);
				FluentValidation.Results.ValidationResult validationResult = TgEfUtils.GetEfValid(storageResult.Item);
				if (!validationResult.IsValid)
					storageResult.Item.Default();
				await EfContext.SaveChangesAsync().ConfigureAwait(false);
			}
			storageResult.State = TgEnumEntityState.IsSaved;
		}
		catch (Exception)
		{
			storageResult.Item.Default();
			throw;
		}
		return storageResult;
	}

	public virtual TgEfStorageResult<TEntity> SaveOrRecreate(TEntity item, string tableName)
	{
		TEntity itemBackup = new();
		itemBackup.Fill(item, true);
		try
		{
			return Save(item);
		}
		catch (Exception)
		{
			item = new();
			item.Fill(itemBackup, true);
			Delete(item, isSkipFind: false);
			return Save(itemBackup);
		}
	}

	public virtual async Task<TgEfStorageResult<TEntity>> SaveOrRecreateAsync(TEntity item, string tableName)
	{
		try
		{
			return await SaveAsync(item);
		}
		catch (Exception)
		{
			TEntity itemBackup = item;
			await DeleteAsync(item, isSkipFind: true);
			return await SaveAsync(itemBackup);
		}
	}

	public virtual TgEfStorageResult<TEntity> CreateNew() => Save(new());

	public virtual async Task<TgEfStorageResult<TEntity>> CreateNewAsync() => await SaveAsync(new()).ConfigureAwait(false);

	#endregion

	#region Public and private methods - Remove

	public virtual TgEfStorageResult<TEntity> Delete(TEntity item, bool isSkipFind)
	{
		IDbContextTransaction transaction = EfContext.Database.BeginTransaction();
		using (transaction)
		{
			TgEfStorageResult<TEntity> storageResult = default!;
			try
			{
				if (!isSkipFind)
				{
					storageResult = Get(item, isNoTracking: false);
					if (!storageResult.IsExists)
						return storageResult;
				}
				else
				{
					storageResult = new(TgEnumEntityState.IsExists, item);
				}
				EfContext.Remove(storageResult.Item);
				EfContext.SaveChanges();
				transaction.Commit();
				return new(TgEnumEntityState.IsDeleted);
			}
			catch (Exception)
			{
				transaction.Rollback();
				storageResult.Item.Default();
				throw;
			}
		}
	}

	public virtual async Task<TgEfStorageResult<TEntity>> DeleteAsync(TEntity item, bool isSkipFind)
	{
		IDbContextTransaction transaction = await EfContext.Database.BeginTransactionAsync().ConfigureAwait(false);
		await using (transaction.ConfigureAwait(false))
		{
			TgEfStorageResult<TEntity> storageResult = default!;
			try
			{
				if (!isSkipFind)
				{
					storageResult = await GetAsync(item, isNoTracking: false).ConfigureAwait(false);
					if (!storageResult.IsExists)
						return storageResult;
				}
				else
				{
					storageResult = new(TgEnumEntityState.IsExists, item);
				}
				EfContext.Remove(storageResult.Item);
				await EfContext.SaveChangesAsync().ConfigureAwait(false);
				await transaction.CommitAsync().ConfigureAwait(false);
				return new(TgEnumEntityState.IsDeleted);
			}
			catch (Exception)
			{
				await transaction.RollbackAsync().ConfigureAwait(false);
				storageResult.Item.Default();
				throw;
			}
		}
	}

	public virtual TgEfStorageResult<TEntity> DeleteNew()
	{
		TgEfStorageResult<TEntity> storageResult = GetNew(isNoTracking: false);
		return storageResult.IsExists
			? Delete(storageResult.Item, isSkipFind: true)
			: new(TgEnumEntityState.NotDeleted);
	}

	public virtual async Task<TgEfStorageResult<TEntity>> DeleteNewAsync()
	{
		TgEfStorageResult<TEntity> storageResult = await GetNewAsync(isNoTracking: false).ConfigureAwait(false);
		return storageResult.IsExists
			? await DeleteAsync(storageResult.Item, isSkipFind: true).ConfigureAwait(false)
			: new(TgEnumEntityState.NotDeleted);
	}

	public virtual TgEfStorageResult<TEntity> DeleteAll()
	{
		TgEfStorageResult<TEntity> storageResult = GetList(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TEntity item in storageResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public virtual async Task<TgEfStorageResult<TEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false).ConfigureAwait(false);
		if (storageResult.IsExists)
		{
			foreach (TEntity item in storageResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}