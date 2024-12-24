// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Contracts;

public interface ITgEfRepository<TEntity> where TEntity : ITgDbFillEntity<TEntity>, new()
{
	#region Public and private fields, properties, constructor

	public TgEfContext EfContext { get; }

	#endregion

	#region Public and private methods

	public Task<IDbContextTransaction> BeginTransactionAsync();
	public Task CommitTransactionAsync();
	public Task RollbackTransactionAsync();
	public IQueryable<TEntity> GetQuery(bool isNoTracking);

	#endregion

	#region Public and private methods - Read

	public Task<TgEfStorageResult<TEntity>> GetAsync(TEntity item, bool isNoTracking);
	public Task<TgEfStorageResult<TEntity>> GetNewAsync(bool isNoTracking);
	public Task<TgEfStorageResult<TEntity>> GetFirstAsync(bool isNoTracking);
	public Task<TEntity> GetFirstItemAsync(bool isNoTracking);
	public Task<TgEfStorageResult<TEntity>> GetListAsync(TgEnumTableTopRecords topRecords, int skip, bool isNoTracking);
	public Task<TgEfStorageResult<TEntity>> GetListAsync(int take, int skip, bool isNoTracking);
	public Task<TgEfStorageResult<TEntity>> GetListAsync(TgEnumTableTopRecords topRecords, int skip, Expression<Func<TEntity, bool>> where, bool isNoTracking);
	public Task<TgEfStorageResult<TEntity>> GetListAsync(int take, int skip, Expression<Func<TEntity, bool>> where, bool isNoTracking);
	public Task<int> GetCountAsync();
	public Task<int> GetCountAsync(Expression<Func<TEntity, bool>> where);

	#endregion

	#region Public and private methods - Write

	public Task<TgEfStorageResult<TEntity>> SaveAsync(TEntity item);
	public Task<TgEfStorageResult<TEntity>> SaveListAsync(List<TEntity> items);
	public Task<TgEfStorageResult<TEntity>> SaveWithoutTransactionAsync(TEntity item);
	public Task<TgEfStorageResult<TEntity>> SaveOrRecreateAsync(TEntity item, string tableName);
	public Task<TgEfStorageResult<TEntity>> CreateNewAsync();

	#endregion

	#region Public and private methods - Remove

	public Task<TgEfStorageResult<TEntity>> DeleteAsync(TEntity item);
	public Task<TgEfStorageResult<TEntity>> DeleteNewAsync();
	public Task<TgEfStorageResult<TEntity>> DeleteAllAsync();

	#endregion
}