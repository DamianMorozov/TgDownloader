// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using ValidationException = FluentValidation.ValidationException;

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
		await Task.Delay(TimeSpan.FromMilliseconds(1));
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
		//TEntity? item = await EfContext.FindAsync<TEntity>(uid);
		TEntity? item = default;
		switch (typeof(TEntity))
		{
			case var cls when cls == typeof(TgEfAppEntity):
				TgEfAppEntity? app = await EfContext.Apps.FindAsync(uid);
				if (app is TEntity appEntity)
					item = appEntity;
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				TgEfDocumentEntity? document = await EfContext.Documents.FindAsync(uid);
				if (document is TEntity documentEntity)
					item = documentEntity;
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				TgEfFilterEntity? filter = await EfContext.Filters.FindAsync(uid);
				if (filter is TEntity filterEntity)
					item = filterEntity;
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				TgEfMessageEntity? message = await EfContext.Messages.FindAsync(uid);
				if (message is TEntity messageEntity)
					item = messageEntity;
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				TgEfProxyEntity? proxy = await EfContext.Proxies.FindAsync(uid);
				if (proxy is TEntity proxyEntity)
					item = proxyEntity;
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				TgEfSourceEntity? source = await EfContext.Sources.FindAsync(uid);
				if (source is TEntity sourceEntity)
					item = sourceEntity;
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				TgEfVersionEntity? version = await EfContext.Versions.FindAsync(uid);
				if (version is TEntity versionEntity)
					item = versionEntity;
				break;
		}
		return item is not null
			? new(TgEnumEntityState.IsExists, item)
			: new TgEfStorageResult<TEntity>(TgEnumEntityState.NotExists);
	}

	public async Task<IDbContextTransaction> BeginTransactionAsync() => await EfContext.Database.BeginTransactionAsync();

	public async Task CommitTransactionAsync() => await EfContext.Database.CommitTransactionAsync();

	public async Task RollbackTransactionAsync() => await EfContext.Database.RollbackTransactionAsync();

	#endregion

	#region Public and private methods - Read

	public virtual TgEfStorageResult<TEntity> Get(TEntity item, bool isNoTracking) => Get(item.Uid);

	public virtual async Task<TgEfStorageResult<TEntity>> GetAsync(TEntity item, bool isNoTracking) => await GetAsync(item.Uid);

	public virtual TgEfStorageResult<TEntity> GetNew(bool isNoTracking) => Get(new(), isNoTracking);

	public virtual async Task<TgEfStorageResult<TEntity>> GetNewAsync(bool isNoTracking) => await GetAsync(new(), isNoTracking);

	public virtual TgEfStorageResult<TEntity> GetFirst(bool isNoTracking) => UseOverrideMethod();

	public virtual async Task<TgEfStorageResult<TEntity>> GetFirstAsync(bool isNoTracking) => await UseOverrideMethodAsync();

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
			TgEnumTableTopRecords.Top1 => await GetListAsync(1, skip, isNoTracking),
			TgEnumTableTopRecords.Top20 => await GetListAsync(20, skip, isNoTracking),
			TgEnumTableTopRecords.Top100 => await GetListAsync(200, skip, isNoTracking),
			TgEnumTableTopRecords.Top1000 => await GetListAsync(1_000, skip, isNoTracking),
			TgEnumTableTopRecords.Top10000 => await GetListAsync(10_000, skip, isNoTracking),
			TgEnumTableTopRecords.Top100000 => await GetListAsync(100_000, skip, isNoTracking),
			TgEnumTableTopRecords.Top1000000 => await GetListAsync(1_000_000, skip, isNoTracking),
			_ => await GetListAsync(0, skip, isNoTracking),
		};

	public virtual TgEfStorageResult<TEntity> GetList(int take, int skip, bool isNoTracking) => UseOverrideMethod();

	public virtual async Task<TgEfStorageResult<TEntity>> GetListAsync(int take, int skip, bool isNoTracking) => await UseOverrideMethodAsync();

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
			TgEnumTableTopRecords.Top1 => await GetListAsync(1, skip, where, isNoTracking),
			TgEnumTableTopRecords.Top20 => await GetListAsync(20, skip, where, isNoTracking),
			TgEnumTableTopRecords.Top100 => await GetListAsync(200, skip, where, isNoTracking),
			TgEnumTableTopRecords.Top1000 => await GetListAsync(1_000, skip, where, isNoTracking),
			TgEnumTableTopRecords.Top10000 => await GetListAsync(10_000, skip, where, isNoTracking),
			TgEnumTableTopRecords.Top100000 => await GetListAsync(100_000, skip, where, isNoTracking),
			TgEnumTableTopRecords.Top1000000 => await GetListAsync(1_000_000, skip, where, isNoTracking),
			_ => await GetListAsync(0, skip, where, isNoTracking),
		};

	public virtual TgEfStorageResult<TEntity> GetList(int take, int skip, Expression<Func<TEntity, bool>> where, bool isNoTracking) => UseOverrideMethod();

	public virtual async Task<TgEfStorageResult<TEntity>> GetListAsync(int take, int skip, Expression<Func<TEntity, bool>> where, bool isNoTracking) =>
		await UseOverrideMethodAsync();

	public virtual int GetCount() => 0;

	public virtual async Task<int> GetCountAsync() => await Task.FromResult(0);

	public virtual int GetCount(Expression<Func<TEntity, bool>> where) => 0;

	public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> where) => await Task.FromResult(0);

	#endregion

	#region Public and private methods - Write

	public virtual TgEfStorageResult<TEntity> Save(TEntity item)
	{
		using IDbContextTransaction transaction = EfContext.Database.BeginTransaction();
		TgEfStorageResult<TEntity> storageResult;
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
				throw new ValidationException(validationResult.Errors);
			TgEfUtils.Normilize(storageResult.Item);
			//item = default;
			EfContext.SaveChanges();
			transaction.Commit();
			storageResult.State = TgEnumEntityState.IsSaved;
		}
		catch (Exception ex)
		{
			transaction.Rollback();
#if DEBUG
			Debug.WriteLine(ex);
#endif
			throw;
		}
		return storageResult;
	}

	private static readonly SemaphoreSlim _transactionSemaphore = new SemaphoreSlim(1, 1);

	public virtual async Task<TgEfStorageResult<TEntity>> SaveAsync(TEntity? item)
	{
		await _transactionSemaphore.WaitAsync();
		IDbContextTransaction transaction = await EfContext.Database.BeginTransactionAsync();
		await using (transaction)
		{
			TgEfStorageResult<TEntity> storageResult = new(TgEnumEntityState.Unknown, item);
			if (item is null) return storageResult;
			try
			{
				storageResult = await GetAsync(item, isNoTracking: false);
				// Create.
				if (!storageResult.IsExists)
				{
					await EfContext.AddAsync(storageResult.Item);
				}
				// Update.
				else
				{
					storageResult.Item.Fill(item, false);
				}
				// Validate.
				FluentValidation.Results.ValidationResult validationResult = TgEfUtils.GetEfValid(storageResult.Item);
				if (!validationResult.IsValid)
					throw new ValidationException(validationResult.Errors);
				TgEfUtils.Normilize(storageResult.Item);
				//item = default;
				await EfContext.SaveChangesAsync();
				await transaction.CommitAsync();
				storageResult.State = TgEnumEntityState.IsSaved;
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
#if DEBUG
				Debug.WriteLine(ex);
#endif
				throw;
			}
			finally
			{
				_transactionSemaphore.Release();
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
		catch (Exception ex)
		{
			transaction.Rollback();
#if DEBUG
			Debug.WriteLine(ex);
#endif
			throw;
		}
		finally
		{
			_transactionSemaphore.Release();
		}
		return storageResult;
	}

	public virtual async Task<TgEfStorageResult<TEntity>> SaveListAsync(List<TEntity> items)
	{
		await _transactionSemaphore.WaitAsync();
		IDbContextTransaction transaction = await EfContext.Database.BeginTransactionAsync();
		await using (transaction)
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
				await EfContext.SaveChangesAsync();
				await transaction.CommitAsync();
				storageResult.State = TgEnumEntityState.IsSaved;
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
#if DEBUG
				Debug.WriteLine(ex);
#endif
				throw;
			}
			finally
			{
				_transactionSemaphore.Release();
			}
			return storageResult;
		}
	}

	public virtual TgEfStorageResult<TEntity> SaveWithoutTransaction(TEntity item)
	{
		TgEfStorageResult<TEntity> storageResult;
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
					throw new ValidationException(validationResult.Errors);
				EfContext.SaveChanges();
			}
			storageResult.State = TgEnumEntityState.IsSaved;
		}
		catch (Exception ex)
		{
#if DEBUG
			Debug.WriteLine(ex);
#endif
			throw;
		}
		return storageResult;
	}

	public virtual async Task<TgEfStorageResult<TEntity>> SaveWithoutTransactionAsync(TEntity item)
	{
		await _transactionSemaphore.WaitAsync();
		TgEfStorageResult<TEntity> storageResult;
		try
		{
			storageResult = await GetAsync(item, isNoTracking: false);
			// Create.
			if (!storageResult.IsExists)
			{
				await EfContext.AddAsync(storageResult.Item);
				await EfContext.SaveChangesAsync();
			}
			// Update.
			else
			{
				storageResult.Item.Fill(item, false);
				FluentValidation.Results.ValidationResult validationResult = TgEfUtils.GetEfValid(storageResult.Item);
				if (!validationResult.IsValid)
					throw new ValidationException(validationResult.Errors);
				await EfContext.SaveChangesAsync();
			}
			storageResult.State = TgEnumEntityState.IsSaved;
		}
		catch (Exception ex)
		{
#if DEBUG
			Debug.WriteLine(ex);
#endif
			throw;
		}
		finally
		{
			_transactionSemaphore.Release();
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
		catch (Exception ex)
		{
			item = new();
			item.Fill(itemBackup, true);
			Delete(item, isSkipFind: false);
			return Save(itemBackup);
#if DEBUG
			Debug.WriteLine(ex);
#endif
			throw;
		}
	}

	public virtual async Task<TgEfStorageResult<TEntity>> SaveOrRecreateAsync(TEntity item, string tableName)
	{
		await _transactionSemaphore.WaitAsync();
		try
		{
			return await SaveAsync(item);
		}
		catch (Exception ex)
		{
			TEntity itemBackup = item;
			await DeleteAsync(item, isSkipFind: true);
			return await SaveAsync(itemBackup);
#if DEBUG
			Debug.WriteLine(ex);
#endif
			throw;
		}
		finally
		{
			_transactionSemaphore.Release();
		}
	}

	public virtual TgEfStorageResult<TEntity> CreateNew() => Save(new());

	public virtual async Task<TgEfStorageResult<TEntity>> CreateNewAsync() => await SaveAsync(new());

	#endregion

	#region Public and private methods - Remove

	public virtual TgEfStorageResult<TEntity> Delete(TEntity item, bool isSkipFind)
	{
		IDbContextTransaction transaction = EfContext.Database.BeginTransaction();
		using (transaction)
		{
			try
			{
				TgEfStorageResult<TEntity> storageResult;
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
			catch (Exception ex)
			{
				transaction.Rollback();
#if DEBUG
				Debug.WriteLine(ex);
#endif
				throw;
			}
		}
	}

	public virtual async Task<TgEfStorageResult<TEntity>> DeleteAsync(TEntity item, bool isSkipFind)
	{
		await _transactionSemaphore.WaitAsync();
		IDbContextTransaction transaction = await EfContext.Database.BeginTransactionAsync();
		await using (transaction)
		{
			try
			{
				TgEfStorageResult<TEntity> storageResult;
				if (!isSkipFind)
				{
					storageResult = await GetAsync(item, isNoTracking: false);
					if (!storageResult.IsExists)
						return storageResult;
				}
				else
				{
					storageResult = new(TgEnumEntityState.IsExists, item);
				}
				EfContext.Remove(storageResult.Item);
				await EfContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return new(TgEnumEntityState.IsDeleted);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				throw;
#if DEBUG
				Debug.WriteLine(ex);
#endif
				throw;
			}
			finally
			{
				_transactionSemaphore.Release();
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
		TgEfStorageResult<TEntity> storageResult = await GetNewAsync(isNoTracking: false);
		return storageResult.IsExists
			? await DeleteAsync(storageResult.Item, isSkipFind: true)
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
		TgEfStorageResult<TEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TEntity item in storageResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}