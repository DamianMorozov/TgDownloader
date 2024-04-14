// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Microsoft.EntityFrameworkCore.Storage;

namespace TgStorage.Common;

public abstract class TgEfRepositoryBase<T>(TgEfContext efContext) : TgCommonBase, ITgEfRepository<T> where T : TgEfEntityBase, ITgDbEntity, new()
{
	#region Public and private fields, properties, constructor

	protected TgEfContext EfContext { get; } = efContext;

	#endregion

	#region Public and private methods

	private TgEfOperResult<T> UseOverrideMethod() => throw new NotImplementedException(TgLocale.UseOverrideMethod);

	private async Task<TgEfOperResult<T>> UseOverrideMethodAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		throw new NotImplementedException(TgLocale.UseOverrideMethod);
	}

	private TgEfOperResult<T> Get(Guid uid)
	{
		T? item = efContext.Find<T>(uid);
		return item is not null
			? new TgEfOperResult<T>(TgEnumEntityState.IsExist, item)
			: new TgEfOperResult<T>(TgEnumEntityState.NotExist);
	}

	private async Task<TgEfOperResult<T>> GetAsync(Guid uid)
	{
		T? item = await efContext.FindAsync<T>(uid).ConfigureAwait(false);
		return item is not null
			? new TgEfOperResult<T>(TgEnumEntityState.IsExist, item)
			: new TgEfOperResult<T>(TgEnumEntityState.NotExist);
	}

	#endregion

	#region Public and private methods - Read

	public virtual TgEfOperResult<T> Get(T item, bool isNoTracking) => Get(item.Uid);

	public virtual async Task<TgEfOperResult<T>> GetAsync(T item, bool isNoTracking) => await GetAsync(item.Uid).ConfigureAwait(false);

	public virtual TgEfOperResult<T> GetNew(bool isNoTracking) => Get(new T().Uid);

	public virtual async Task<TgEfOperResult<T>> GetNewAsync(bool isNoTracking) => await GetAsync(new T().Uid).ConfigureAwait(false);

	public virtual TgEfOperResult<T> GetFirst(bool isNoTracking) => UseOverrideMethod();

	public virtual async Task<TgEfOperResult<T>> GetFirstAsync(bool isNoTracking) => await UseOverrideMethodAsync().ConfigureAwait(false);

	public virtual TgEfOperResult<T> GetEnumerable(TgEnumTableTopRecords topRecords, bool isNoTracking) => UseOverrideMethod();

	public virtual TgEfOperResult<T> GetEnumerable(int count, bool isNoTracking) => UseOverrideMethod();

	public virtual async Task<TgEfOperResult<T>> GetEnumerableAsync(TgEnumTableTopRecords topRecords, bool isNoTracking) =>
		await UseOverrideMethodAsync().ConfigureAwait(false);

	public virtual async Task<TgEfOperResult<T>> GetEnumerableAsync(int count, bool isNoTracking) => 
		await UseOverrideMethodAsync().ConfigureAwait(false);

	public virtual int GetCount() => throw new NotImplementedException(TgLocale.UseOverrideMethod);

	public virtual async Task<int> GetCountAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		throw new NotImplementedException(TgLocale.UseOverrideMethod);
	}

	#endregion

	#region Public and private methods - Write

	public virtual TgEfOperResult<T> Save(T item)
	{
		using IDbContextTransaction transaction = EfContext.Database.BeginTransaction();
		TgEfOperResult<T> operResult = new TgEfOperResult<T>(TgEnumEntityState.Unknown, item);
		try
		{
			operResult = Get(item, isNoTracking: false);
			// Create.
			if (operResult.NotExist)
			{
				EfContext.Add(operResult.Item);
				EfContext.SaveChanges();
			}
			// Update.
			else
			{
				operResult.Item.Fill(item);
				FluentValidation.Results.ValidationResult validationResult = TgStorageUtils.GetEfValid(operResult.Item);
				if (!validationResult.IsValid)
					operResult.Item.Default();
				EfContext.SaveChanges();
			}
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
			TgEfOperResult<T> operResult = new TgEfOperResult<T>(TgEnumEntityState.Unknown, item);
			try
			{
				operResult = await GetAsync(item, isNoTracking: false).ConfigureAwait(false);
				// Create.
				if (operResult.NotExist)
				{
					await EfContext.AddAsync(operResult.Item).ConfigureAwait(false);
					await EfContext.SaveChangesAsync().ConfigureAwait(false);
				}
				// Update.
				else
				{
					operResult.Item.Fill(item);
					FluentValidation.Results.ValidationResult validationResult = TgStorageUtils.GetEfValid(operResult.Item);
					if (!validationResult.IsValid)
						operResult.Item.Default();
					await EfContext.SaveChangesAsync().ConfigureAwait(false);
				}
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

	public virtual TgEfOperResult<T> CreateNew() => Save(new T());

	public virtual async Task<TgEfOperResult<T>> CreateNewAsync() => await SaveAsync(new T()).ConfigureAwait(false);

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
					if (operResult.NotExist)
						return operResult;
				}
				else
				{
					operResult = new TgEfOperResult<T>(TgEnumEntityState.IsExist, item);
				}
				EfContext.Remove(operResult.Item);
				transaction.Commit();
				return new TgEfOperResult<T>(TgEnumEntityState.IsDeleted);
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
					if (operResult.NotExist)
						return operResult;
				}
				else
				{
					operResult = new TgEfOperResult<T>(TgEnumEntityState.IsExist, item);
				}
				EfContext.Remove(operResult.Item);
				await transaction.CommitAsync().ConfigureAwait(false);
				return new TgEfOperResult<T>(TgEnumEntityState.IsDeleted);
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
		return operResult.IsExist
			? Delete(operResult.Item, isSkipFind: true)
			: new TgEfOperResult<T>(TgEnumEntityState.NotDeleted);
	}

	public virtual async Task<TgEfOperResult<T>> DeleteNewAsync()
	{
		TgEfOperResult<T> operResult = await GetNewAsync(isNoTracking: false).ConfigureAwait(false);
		return operResult.IsExist
			? await DeleteAsync(operResult.Item, isSkipFind: true).ConfigureAwait(false)
			: new TgEfOperResult<T>(TgEnumEntityState.NotDeleted);
	}

	public virtual TgEfOperResult<T> DeleteAll()
	{
		TgEfOperResult<T> operResult = GetEnumerable(0, isNoTracking: false);
		if (operResult.IsExist)
		{
			foreach (T item in operResult.Items)
			{
				Delete(item, isSkipFind: true);
			}
		}
		return new TgEfOperResult<T>(operResult.IsExist ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	public virtual async Task<TgEfOperResult<T>> DeleteAllAsync()
	{
		TgEfOperResult<T> operResult = await GetEnumerableAsync(0, isNoTracking: false).ConfigureAwait(false);
		if (operResult.IsExist)
		{
			foreach (T item in operResult.Items)
			{
				await DeleteAsync(item, isSkipFind: true).ConfigureAwait(false);
			}
		}
		return new TgEfOperResult<T>(operResult.IsExist ? TgEnumEntityState.IsDeleted : TgEnumEntityState.NotDeleted);
	}

	#endregion
}