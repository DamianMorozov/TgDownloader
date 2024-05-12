// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Contracts;

public interface ITgEfRepository<T> where T : TgEfEntityBase, ITgDbEntity, new()
{
	#region Public and private fields, properties, constructor

	public TgEfContext EfContext { get; }

	#endregion

	#region Public and private methods - Read

	public TgEfOperResult<T> Get(T item, bool isNoTracking);
	public Task<TgEfOperResult<T>> GetAsync(T item, bool isNoTracking);
	public TgEfOperResult<T> GetNew(bool isNoTracking);
	public Task<TgEfOperResult<T>> GetNewAsync(bool isNoTracking);
	public TgEfOperResult<T> GetFirst(bool isNoTracking);
	public Task<TgEfOperResult<T>> GetFirstAsync(bool isNoTracking);
	public TgEfOperResult<T> GetList(TgEnumTableTopRecords topRecords, int skip, bool isNoTracking);
	public Task<TgEfOperResult<T>> GetListAsync(TgEnumTableTopRecords topRecords, int skip, bool isNoTracking);
	public TgEfOperResult<T> GetList(int take, int skip, bool isNoTracking);
	public Task<TgEfOperResult<T>> GetListAsync(int take, int skip, bool isNoTracking);
	public TgEfOperResult<T> GetList(TgEnumTableTopRecords topRecords, int skip, Expression<Func<T, bool>> where, bool isNoTracking);
	public Task<TgEfOperResult<T>> GetListAsync(TgEnumTableTopRecords topRecords, int skip, Expression<Func<T, bool>> where, bool isNoTracking);
	public TgEfOperResult<T> GetList(int take, int skip, Expression<Func<T, bool>> where, bool isNoTracking);
	public Task<TgEfOperResult<T>> GetListAsync(int take, int skip, Expression<Func<T, bool>> where, bool isNoTracking);
	public int GetCount();
	public Task<int> GetCountAsync();
	public int GetCount(Expression<Func<T, bool>> where);
	public Task<int> GetCountAsync(Expression<Func<T, bool>> where);

	#endregion

	#region Public and private methods - Write

	public TgEfOperResult<T> Save(T item);
	public Task<TgEfOperResult<T>> SaveAsync(T item);
	public TgEfOperResult<T> SaveList(List<T> items);
	public Task<TgEfOperResult<T>> SaveListAsync(List<T> items);
	public TgEfOperResult<T> SaveWithoutTransaction(T item);
	public Task<TgEfOperResult<T>> SaveWithoutTransactionAsync(T item);
	public TgEfOperResult<T> SaveOrRecreate(T item, string tableName);
	public Task<TgEfOperResult<T>> SaveOrRecreateAsync(T item, string tableName);
	public TgEfOperResult<T> CreateNew();
	public Task<TgEfOperResult<T>> CreateNewAsync();

	#endregion

	#region Public and private methods - Remove

	public TgEfOperResult<T> Delete(T item, bool isSkipFind);
	public Task<TgEfOperResult<T>> DeleteAsync(T item, bool isSkipFind);
	public TgEfOperResult<T> DeleteNew();
	public Task<TgEfOperResult<T>> DeleteNewAsync();
	public TgEfOperResult<T> DeleteAll();
	public Task<TgEfOperResult<T>> DeleteAllAsync();

	#endregion
}