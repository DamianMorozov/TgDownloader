// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using ValidationException = FluentValidation.ValidationException;

namespace TgStorage.Common;

public abstract class TgEfRepositoryBase<TEntity>(TgEfContext efContext) : TgCommonBase, ITgEfRepository<TEntity>
	where TEntity : ITgDbFillEntity<TEntity>, new()
{
	#region Public and private fields, properties, constructor

	public TgEfContext EfContext { get; } = efContext;
	private static readonly SemaphoreSlim TransactionSemaphore = new(1, 1);


	#endregion

	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfRepositoryBase<TEntity>)}";

	public virtual IQueryable<TEntity> GetQuery(bool isNoTracking) => throw new NotImplementedException(TgLocaleHelper.Instance.UseOverrideMethod);

	private static TgEfStorageResult<TEntity> UseOverrideMethod() => throw new NotImplementedException(TgLocaleHelper.Instance.UseOverrideMethod);

	private static async Task<TgEfStorageResult<TEntity>> UseOverrideMethodAsync()
	{
		await Task.CompletedTask;
		throw new NotImplementedException(TgLocaleHelper.Instance.UseOverrideMethod);
	}

	public async Task<IDbContextTransaction> BeginTransactionAsync() => await EfContext.Database.BeginTransactionAsync();

	public async Task CommitTransactionAsync() => await EfContext.Database.CommitTransactionAsync();

	public async Task RollbackTransactionAsync() => await EfContext.Database.RollbackTransactionAsync();

	#endregion

	#region Public and private methods - Read

	public virtual async Task<TgEfStorageResult<TEntity>> GetAsync(TEntity item, bool isNoTracking)
	{
		var uid = item?.Uid ?? Guid.Empty;
		TEntity? result = default;
		switch (typeof(TEntity))
		{
			case var cls when cls == typeof(TgEfAppEntity):
				TgEfAppEntity? app = await EfContext.Apps.FindAsync(uid);
				if (app is TEntity appEntity)
					result = appEntity;
				break;
			case var cls when cls == typeof(TgEfDocumentEntity):
				TgEfDocumentEntity? document = await EfContext.Documents.FindAsync(uid);
				if (document is TEntity documentEntity)
					result = documentEntity;
				break;
			case var cls when cls == typeof(TgEfContactEntity):
				TgEfContactEntity? contact = await EfContext.Contacts.FindAsync(uid);
				if (contact is TEntity contactEntity)
					result = contactEntity;
				break;
			case var cls when cls == typeof(TgEfFilterEntity):
				TgEfFilterEntity? filter = await EfContext.Filters.FindAsync(uid);
				if (filter is TEntity filterEntity)
					result = filterEntity;
				break;
			case var cls when cls == typeof(TgEfMessageEntity):
				TgEfMessageEntity? message = await EfContext.Messages.FindAsync(uid);
				if (message is TEntity messageEntity)
					result = messageEntity;
				break;
			case var cls when cls == typeof(TgEfProxyEntity):
				TgEfProxyEntity? proxy = await EfContext.Proxies.FindAsync(uid);
				if (proxy is TEntity proxyEntity)
					result = proxyEntity;
				break;
			case var cls when cls == typeof(TgEfSourceEntity):
				TgEfSourceEntity? source = await EfContext.Sources.FindAsync(uid);
				if (source is TEntity sourceEntity)
					result = sourceEntity;
				break;
			case var cls when cls == typeof(TgEfStoryEntity):
				TgEfStoryEntity? story = await EfContext.Stories.FindAsync(uid);
				if (story is TEntity storyEntity)
					result = storyEntity;
				break;
			case var cls when cls == typeof(TgEfVersionEntity):
				TgEfVersionEntity? version = await EfContext.Versions.FindAsync(uid);
				if (version is TEntity versionEntity)
					result = versionEntity;
				break;
		}
		return result is not null
			? new(TgEnumEntityState.IsExists, result)
			: new TgEfStorageResult<TEntity>(TgEnumEntityState.NotExists);
	}

	public virtual async Task<TgEfStorageResult<TEntity>> GetNewAsync(bool isNoTracking) => await GetAsync(new(), isNoTracking);

	public virtual async Task<TgEfStorageResult<TEntity>> GetFirstAsync(bool isNoTracking) => await TgEfRepositoryBase<TEntity>.UseOverrideMethodAsync();
	
	public virtual async Task<TEntity> GetFirstItemAsync(bool isNoTracking) => (await GetFirstAsync(isNoTracking)).Item;

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

	public virtual async Task<TgEfStorageResult<TEntity>> GetListAsync(int take, int skip, bool isNoTracking) => await TgEfRepositoryBase<TEntity>.UseOverrideMethodAsync();

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

	public virtual async Task<TgEfStorageResult<TEntity>> GetListAsync(int take, int skip, Expression<Func<TEntity, bool>> where, bool isNoTracking) =>
		await TgEfRepositoryBase<TEntity>.UseOverrideMethodAsync();

	public virtual async Task<int> GetCountAsync() => await Task.FromResult(0);

	public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> where) => await Task.FromResult(0);

	#endregion

	#region Public and private methods - Write

	public virtual async Task<TgEfStorageResult<TEntity>> SaveAsync(TEntity? item)
	{
		await TransactionSemaphore.WaitAsync();
		IDbContextTransaction transaction = await EfContext.Database.BeginTransactionAsync();
		TgEfStorageResult<TEntity>? storageResult = null;
		await using (transaction)
		{
			if (item is null) return new(TgEnumEntityState.Unknown, item);
			try
			{
				storageResult = await GetAsync(item, isNoTracking: false);
				// Create
				if (!storageResult.IsExists)
				{
					await EfContext.AddAsync(storageResult.Item);
				}
				// Update
				else
				{
					storageResult.Item.Fill(item, isUidCopy: false);
				}
				// Validate
				FluentValidation.Results.ValidationResult validationResult = TgEfUtils.GetEfValid(storageResult.Item);
				if (!validationResult.IsValid)
					throw new ValidationException(validationResult.Errors);
				TgEfUtils.Normilize(storageResult.Item);
				await EfContext.SaveChangesAsync();
				await transaction.CommitAsync();
				storageResult.State = TgEnumEntityState.IsSaved;
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
#if DEBUG
				Debug.WriteLine(ex, TgConstants.LogTypeStorage);
#endif
				throw;
			}
			finally
			{
				TransactionSemaphore.Release();
			}
			return storageResult;
		}
	}


	public virtual async Task<TgEfStorageResult<TEntity>> SaveListAsync(List<TEntity> items)
	{
		await TransactionSemaphore.WaitAsync();
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
				Debug.WriteLine(ex, TgConstants.LogTypeStorage);
#endif
				throw;
			}
			finally
			{
				TransactionSemaphore.Release();
			}
			return storageResult;
		}
	}

	public virtual async Task<TgEfStorageResult<TEntity>> SaveWithoutTransactionAsync(TEntity item)
	{
		await TransactionSemaphore.WaitAsync();
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
			Debug.WriteLine(ex, TgConstants.LogTypeStorage);
#endif
			throw;
		}
		finally
		{
			TransactionSemaphore.Release();
		}
		return storageResult;
	}

	public virtual async Task<TgEfStorageResult<TEntity>> SaveOrRecreateAsync(TEntity item, string tableName)
	{
		await TransactionSemaphore.WaitAsync();
		try
		{
			return await SaveAsync(item);
		}
		catch (Exception ex)
		{
			TEntity itemBackup = item;
			await DeleteAsync(item);
#if DEBUG
			Debug.WriteLine(ex, TgConstants.LogTypeStorage);
#endif
			try
			{
				return await SaveAsync(itemBackup);
			}
			catch (Exception)
			{
				throw;
			}
		}
		finally
		{
			TransactionSemaphore.Release();
		}
	}

	public virtual async Task<TgEfStorageResult<TEntity>> CreateNewAsync() => await SaveAsync(new());

	#endregion

	#region Public and private methods - Remove

	public virtual async Task<TgEfStorageResult<TEntity>> DeleteAsync(TEntity item)
	{
		await TransactionSemaphore.WaitAsync();
		IDbContextTransaction transaction = await EfContext.Database.BeginTransactionAsync();
		await using (transaction)
		{
			try
			{
				TgEfStorageResult<TEntity> storageResult;
				storageResult = await GetAsync(item, isNoTracking: false);
				if (!storageResult.IsExists)
					return storageResult;
				EfContext.Remove(storageResult.Item);
				await EfContext.SaveChangesAsync();
				await transaction.CommitAsync();
				return new(TgEnumEntityState.IsDeleted);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
#if DEBUG
				Debug.WriteLine(ex, TgConstants.LogTypeStorage);
#endif
				throw;
			}
			finally
			{
				TransactionSemaphore.Release();
			}
		}
	}

	public virtual async Task<TgEfStorageResult<TEntity>> DeleteNewAsync()
	{
		TgEfStorageResult<TEntity> storageResult = await GetNewAsync(isNoTracking: false);
		return storageResult.IsExists
			? await DeleteAsync(storageResult.Item)
			: new(TgEnumEntityState.NotDeleted);
	}

	public virtual async Task<TgEfStorageResult<TEntity>> DeleteAllAsync()
	{
		TgEfStorageResult<TEntity> storageResult = await GetListAsync(0, 0, isNoTracking: false);
		if (storageResult.IsExists)
		{
			foreach (TEntity item in storageResult.Items)
			{
				await DeleteAsync(item);
			}
		}
		return new(storageResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}