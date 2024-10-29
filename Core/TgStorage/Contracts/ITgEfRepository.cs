// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Contracts;

public interface ITgEfRepository<TEntity> where TEntity : ITgDbFillEntity<TEntity>, new()
{
	#region Public and private fields, properties, constructor

	public TgEfContext EfContext { get; }

	#endregion

	#region Public and private methods - Read

	public TgEfStorageResult<TEntity> Get(TEntity item, bool isNoTracking);
	public Task<TgEfStorageResult<TEntity>> GetAsync(TEntity item, bool isNoTracking);
	public TgEfStorageResult<TEntity> GetNew(bool isNoTracking);
	public Task<TgEfStorageResult<TEntity>> GetNewAsync(bool isNoTracking);
	public TgEfStorageResult<TEntity> GetFirst(bool isNoTracking);
	public Task<TgEfStorageResult<TEntity>> GetFirstAsync(bool isNoTracking);
	public TgEfStorageResult<TEntity> GetList(TgEnumTableTopRecords topRecords, int skip, bool isNoTracking);
	public Task<TgEfStorageResult<TEntity>> GetListAsync(TgEnumTableTopRecords topRecords, int skip, bool isNoTracking);
	public TgEfStorageResult<TEntity> GetList(int take, int skip, bool isNoTracking);
	public Task<TgEfStorageResult<TEntity>> GetListAsync(int take, int skip, bool isNoTracking);
	public TgEfStorageResult<TEntity> GetList(TgEnumTableTopRecords topRecords, int skip, Expression<Func<TEntity, bool>> where, bool isNoTracking);
	public Task<TgEfStorageResult<TEntity>> GetListAsync(TgEnumTableTopRecords topRecords, int skip, Expression<Func<TEntity, bool>> where, bool isNoTracking);
	public TgEfStorageResult<TEntity> GetList(int take, int skip, Expression<Func<TEntity, bool>> where, bool isNoTracking);
	public Task<TgEfStorageResult<TEntity>> GetListAsync(int take, int skip, Expression<Func<TEntity, bool>> where, bool isNoTracking);
	public int GetCount();
	public Task<int> GetCountAsync();
	public int GetCount(Expression<Func<TEntity, bool>> where);
	public Task<int> GetCountAsync(Expression<Func<TEntity, bool>> where);

	#endregion

	#region Public and private methods - Write

	public TgEfStorageResult<TEntity> Save(TEntity item);
	public Task<TgEfStorageResult<TEntity>> SaveAsync(TEntity item);
	public TgEfStorageResult<TEntity> SaveList(List<TEntity> items);
	public Task<TgEfStorageResult<TEntity>> SaveListAsync(List<TEntity> items);
	public TgEfStorageResult<TEntity> SaveWithoutTransaction(TEntity item);
	public Task<TgEfStorageResult<TEntity>> SaveWithoutTransactionAsync(TEntity item);
	public TgEfStorageResult<TEntity> SaveOrRecreate(TEntity item, string tableName);
	public Task<TgEfStorageResult<TEntity>> SaveOrRecreateAsync(TEntity item, string tableName);
	public TgEfStorageResult<TEntity> CreateNew();
	public Task<TgEfStorageResult<TEntity>> CreateNewAsync();

	#endregion

	#region Public and private methods - Remove

	public TgEfStorageResult<TEntity> Delete(TEntity item, bool isSkipFind);
	public Task<TgEfStorageResult<TEntity>> DeleteAsync(TEntity item, bool isSkipFind);
	public TgEfStorageResult<TEntity> DeleteNew();
	public Task<TgEfStorageResult<TEntity>> DeleteNewAsync();
	public TgEfStorageResult<TEntity> DeleteAll();
	public Task<TgEfStorageResult<TEntity>> DeleteAllAsync();

	#endregion
}