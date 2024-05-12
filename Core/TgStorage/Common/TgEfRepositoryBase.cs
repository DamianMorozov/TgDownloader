// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

public abstract class TgEfRepositoryBase<T>(TgEfContext efContext) : TgCommonBase, ITgEfRepository<T>
	where T : TgEfEntityBase, ITgDbEntity, new()
{
	#region Public and private fields, properties, constructor

	public TgEfContext EfContext { get; } = efContext;

	#endregion

	#region Public and private methods

	public override string ToDebugString() => $"{nameof(TgEfRepositoryBase<T>)}";

	private TgEfOperResult<T> UseOverrideMethod() => throw new NotImplementedException(TgLocaleHelper.Instance.UseOverrideMethod);

	private async Task<TgEfOperResult<T>> UseOverrideMethodAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		throw new NotImplementedException(TgLocaleHelper.Instance.UseOverrideMethod);
	}

	private TgEfOperResult<T> Get(Guid uid)
	{
		T? item = EfContext.Find<T>(uid);
		return item is not null
			? new(TgEnumEntityState.IsExists, item)
			: new TgEfOperResult<T>(TgEnumEntityState.NotExists);
	}

	private async Task<TgEfOperResult<T>> GetAsync(Guid uid)
	{
		T? item = await EfContext.FindAsync<T>(uid).ConfigureAwait(false);
		return item is not null
			? new(TgEnumEntityState.IsExists, item)
			: new TgEfOperResult<T>(TgEnumEntityState.NotExists);
	}

	#endregion

	#region Public and private methods - Read

	public virtual TgEfOperResult<T> Get(T item, bool isNoTracking) => Get(item.Uid);

	public virtual async Task<TgEfOperResult<T>> GetAsync(T item, bool isNoTracking) => await GetAsync(item.Uid).ConfigureAwait(false);

	public virtual TgEfOperResult<T> GetNew(bool isNoTracking) => Get(new(), isNoTracking);

	public virtual async Task<TgEfOperResult<T>> GetNewAsync(bool isNoTracking) => await GetAsync(new(), isNoTracking).ConfigureAwait(false);

	public virtual TgEfOperResult<T> GetFirst(bool isNoTracking) => UseOverrideMethod();

	public virtual async Task<TgEfOperResult<T>> GetFirstAsync(bool isNoTracking) => await UseOverrideMethodAsync().ConfigureAwait(false);

	public virtual TgEfOperResult<T> GetList(TgEnumTableTopRecords topRecords, int skip, bool isNoTracking) =>
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

	public virtual async Task<TgEfOperResult<T>> GetListAsync(TgEnumTableTopRecords topRecords, int skip, bool isNoTracking) =>
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

	public virtual TgEfOperResult<T> GetList(int take, int skip, bool isNoTracking) => UseOverrideMethod();

	public virtual async Task<TgEfOperResult<T>> GetListAsync(int take, int skip, bool isNoTracking) => await UseOverrideMethodAsync().ConfigureAwait(false);

	public virtual TgEfOperResult<T> GetList(TgEnumTableTopRecords topRecords, int skip, Expression<Func<T, bool>> where, bool isNoTracking) =>
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

	public virtual async Task<TgEfOperResult<T>> GetListAsync(TgEnumTableTopRecords topRecords, int skip, Expression<Func<T, bool>> where, bool isNoTracking) =>
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

	public virtual TgEfOperResult<T> GetList(int take, int skip, Expression<Func<T, bool>> where, bool isNoTracking) => UseOverrideMethod();

	public virtual async Task<TgEfOperResult<T>> GetListAsync(int take, int skip, Expression<Func<T, bool>> where, bool isNoTracking) =>
		await UseOverrideMethodAsync().ConfigureAwait(false);

	public virtual int GetCount() => 0;

	public virtual async Task<int> GetCountAsync() => await Task.FromResult(0);

	public virtual int GetCount(Expression<Func<T, bool>> where) => 0;

	public virtual async Task<int> GetCountAsync(Expression<Func<T, bool>> where) => await Task.FromResult(0);

	#endregion

	#region Public and private methods - Write

	public virtual TgEfOperResult<T> Save(T item)
	{
		using IDbContextTransaction transaction = EfContext.Database.BeginTransaction();
		TgEfOperResult<T> operResult = new(TgEnumEntityState.Unknown, item);
		try
		{
			operResult = Get(item, isNoTracking: false);
			// Create.
			if (!operResult.IsExists)
			{
				EfContext.Add(operResult.Item);
			}
			// Update.
			else
			{
				operResult.Item.Fill(item);
				FluentValidation.Results.ValidationResult validationResult = TgEfUtils.GetEfValid(operResult.Item);
				if (!validationResult.IsValid)
					operResult.Item.Default();
			}
			TgEfUtils.Normilize(item);
			EfContext.SaveChanges();
			transaction.Commit();
			operResult.State = TgEnumEntityState.IsSaved;
		}
		catch (Exception)
		{
			transaction.Rollback();
			operResult.Item.Default();
			throw;
		}
		return operResult;
	}

	public virtual async Task<TgEfOperResult<T>> SaveAsync(T item)
	{
		IDbContextTransaction transaction = await EfContext.Database.BeginTransactionAsync().ConfigureAwait(false);
		await using (transaction.ConfigureAwait(false))
		{
			TgEfOperResult<T> operResult = new(TgEnumEntityState.Unknown, item);
			try
			{
				operResult = await GetAsync(item, isNoTracking: false).ConfigureAwait(false);
				// Create.
				if (!operResult.IsExists)
				{
					await EfContext.AddAsync(operResult.Item).ConfigureAwait(false);
				}
				// Update.
				else
				{
					operResult.Item.Fill(item);
					FluentValidation.Results.ValidationResult validationResult = TgEfUtils.GetEfValid(operResult.Item);
					if (!validationResult.IsValid)
						operResult.Item.Default();
				}
				TgEfUtils.Normilize(item);
				await EfContext.SaveChangesAsync().ConfigureAwait(false);
				await transaction.CommitAsync().ConfigureAwait(false);
				operResult.State = TgEnumEntityState.IsSaved;
			}
			catch (Exception)
			{
				await transaction.RollbackAsync().ConfigureAwait(false);
				operResult.Item.Default();
				throw;
			}
			return operResult;
		}
	}

	public virtual TgEfOperResult<T> SaveList(List<T> items)
	{
		using IDbContextTransaction transaction = EfContext.Database.BeginTransaction();
		TgEfOperResult<T> operResult = new(TgEnumEntityState.Unknown, items);
		try
		{
			List<T> uniqueItems = items.Distinct().ToList();
			foreach (T item in uniqueItems)
			{
				TgEfUtils.Normilize(item);
				EfContext.Add(item);
			}
			EfContext.SaveChanges();
			transaction.Commit();
			operResult.State = TgEnumEntityState.IsSaved;
		}
		catch (Exception)
		{
			transaction.Rollback();
			operResult.Item.Default();
			throw;
		}
		return operResult;
	}

	public virtual async Task<TgEfOperResult<T>> SaveListAsync(List<T> items)
	{
		IDbContextTransaction transaction = await EfContext.Database.BeginTransactionAsync().ConfigureAwait(false);
		await using (transaction.ConfigureAwait(false))
		{
			TgEfOperResult<T> operResult = new(TgEnumEntityState.Unknown, items);
			try
			{
				List<T> uniqueItems = items.Distinct().ToList();
				foreach (T item in uniqueItems)
				{
					TgEfUtils.Normilize(item);
					EfContext.Add(item);
				}
				await EfContext.SaveChangesAsync().ConfigureAwait(false);
				await transaction.CommitAsync().ConfigureAwait(false);
				operResult.State = TgEnumEntityState.IsSaved;
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync().ConfigureAwait(false);
				operResult.Item.Default();
#if DEBUG
				Debug.WriteLine(ex);
#endif
				throw;
			}
			return operResult;
		}
	}

	public virtual TgEfOperResult<T> SaveWithoutTransaction(T item)
	{
		TgEfOperResult<T> operResult = new(TgEnumEntityState.Unknown, item);
		try
		{
			operResult = Get(item, isNoTracking: false);
			// Create.
			if (!operResult.IsExists)
			{
				EfContext.Add(operResult.Item);
				EfContext.SaveChanges();
			}
			// Update.
			else
			{
				operResult.Item.Fill(item);
				FluentValidation.Results.ValidationResult validationResult = TgEfUtils.GetEfValid(operResult.Item);
				if (!validationResult.IsValid)
					operResult.Item.Default();
				EfContext.SaveChanges();
			}
			operResult.State = TgEnumEntityState.IsSaved;
		}
		catch (Exception)
		{
			operResult.Item.Default();
			throw;
		}
		return operResult;
	}

	public virtual async Task<TgEfOperResult<T>> SaveWithoutTransactionAsync(T item)
	{
		TgEfOperResult<T> operResult = new(TgEnumEntityState.Unknown, item);
		try
		{
			operResult = await GetAsync(item, isNoTracking: false).ConfigureAwait(false);
			// Create.
			if (!operResult.IsExists)
			{
				await EfContext.AddAsync(operResult.Item).ConfigureAwait(false);
				await EfContext.SaveChangesAsync().ConfigureAwait(false);
			}
			// Update.
			else
			{
				operResult.Item.Fill(item);
				FluentValidation.Results.ValidationResult validationResult = TgEfUtils.GetEfValid(operResult.Item);
				if (!validationResult.IsValid)
					operResult.Item.Default();
				await EfContext.SaveChangesAsync().ConfigureAwait(false);
			}
			operResult.State = TgEnumEntityState.IsSaved;
		}
		catch (Exception)
		{
			operResult.Item.Default();
			throw;
		}
		return operResult;
	}

	public virtual TgEfOperResult<T> SaveOrRecreate(T item, string tableName)
	{
		T itemBackup = new();
		itemBackup.Backup(item);
		try
		{
			return Save(item);
		}
		catch (Exception)
		{
			item = new();
			item.Backup(itemBackup);
			Delete(item, isSkipFind: false);
			return Save(itemBackup);
		}
	}

	public virtual async Task<TgEfOperResult<T>> SaveOrRecreateAsync(T item, string tableName)
	{
		try
		{
			return await SaveAsync(item);
		}
		catch (Exception)
		{
			T itemBackup = item;
			await DeleteAsync(item, isSkipFind: true);
			return await SaveAsync(itemBackup);
		}
	}

	public virtual TgEfOperResult<T> CreateNew() => Save(new());

	public virtual async Task<TgEfOperResult<T>> CreateNewAsync() => await SaveAsync(new()).ConfigureAwait(false);

	#endregion

	#region Public and private methods - Remove

	public virtual TgEfOperResult<T> Delete(T item, bool isSkipFind)
	{
		IDbContextTransaction transaction = EfContext.Database.BeginTransaction();
		using (transaction)
		{
			TgEfOperResult<T> operResult = default!;
			try
			{
				if (!isSkipFind)
				{
					operResult = Get(item, isNoTracking: false);
					if (!operResult.IsExists)
						return operResult;
				}
				else
				{
					operResult = new(TgEnumEntityState.IsExists, item);
				}
				EfContext.Remove(operResult.Item);
				EfContext.SaveChanges();
				transaction.Commit();
				return new(TgEnumEntityState.IsDeleted);
			}
			catch (Exception)
			{
				transaction.Rollback();
				operResult.Item.Default();
				throw;
			}
		}
	}

	public virtual async Task<TgEfOperResult<T>> DeleteAsync(T item, bool isSkipFind)
	{
		IDbContextTransaction transaction = await EfContext.Database.BeginTransactionAsync().ConfigureAwait(false);
		await using (transaction.ConfigureAwait(false))
		{
			TgEfOperResult<T> operResult = default!;
			try
			{
				if (!isSkipFind)
				{
					operResult = await GetAsync(item, isNoTracking: false).ConfigureAwait(false);
					if (!operResult.IsExists)
						return operResult;
				}
				else
				{
					operResult = new(TgEnumEntityState.IsExists, item);
				}
				EfContext.Remove(operResult.Item);
				await EfContext.SaveChangesAsync().ConfigureAwait(false);
				await transaction.CommitAsync().ConfigureAwait(false);
				return new(TgEnumEntityState.IsDeleted);
			}
			catch (Exception)
			{
				await transaction.RollbackAsync().ConfigureAwait(false);
				operResult.Item.Default();
				throw;
			}
		}
	}

	public virtual TgEfOperResult<T> DeleteNew()
	{
		TgEfOperResult<T> operResult = GetNew(isNoTracking: false);
		return operResult.IsExists
			? Delete(operResult.Item, isSkipFind: true)
			: new(TgEnumEntityState.NotDeleted);
	}

	public virtual async Task<TgEfOperResult<T>> DeleteNewAsync()
	{
		TgEfOperResult<T> operResult = await GetNewAsync(isNoTracking: false).ConfigureAwait(false);
		return operResult.IsExists
			? await DeleteAsync(operResult.Item, isSkipFind: true).ConfigureAwait(false)
			: new(TgEnumEntityState.NotDeleted);
	}

	public virtual TgEfOperResult<T> DeleteAll()
	{
		TgEfOperResult<T> operResult = GetList(0, 0, isNoTracking: false);
		if (operResult.IsExists)
		{
			foreach (T item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public virtual async Task<TgEfOperResult<T>> DeleteAllAsync()
	{
		TgEfOperResult<T> operResult = await GetListAsync(0, 0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExists)
		{
			foreach (T item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new(operResult.IsExists ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}