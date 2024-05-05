// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Contracts;

public interface ITgEfRepository<T> where T : TgEfEntityBase, ITgDbEntity, new()
{
	#region Public and private methods - Read

	public TgEfOperResult<T> Get(T item, bool isNoTracking);
	public Task<TgEfOperResult<T>> GetAsync(T item, bool isNoTracking);
	public TgEfOperResult<T> GetNew(bool isNoTracking);
	public Task<TgEfOperResult<T>> GetNewAsync(bool isNoTracking);
	public TgEfOperResult<T> GetFirst(bool isNoTracking);
	public Task<TgEfOperResult<T>> GetFirstAsync(bool isNoTracking);
	public TgEfOperResult<T> GetList(TgEnumTableTopRecords topRecords, bool isNoTracking);
	public Task<TgEfOperResult<T>> GetListAsync(TgEnumTableTopRecords topRecords, bool isNoTracking);
	public int GetCount();
	public Task<int> GetCountAsync();

	#endregion

	#region Public and private methods - Write

	public TgEfOperResult<T> Save(T item);
	public Task<TgEfOperResult<T>> SaveAsync(T item);
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