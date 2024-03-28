// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Common;

public interface ITgEfRepository<TEntity> where TEntity : ITgEfEntity, new()
{
	public TEntity CreateNew(bool isSave);
	public TEntity GetFirst();
	public Task<TEntity> GetFirstAsync();
	public IEnumerable<TEntity> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All);
	public IEnumerable<TEntity> GetEnumerable(int count);
	public TEntity GetSingle(Guid uid);
	public Task<TEntity> GetSingleAsync(Guid uid);
    public int GetCount();
    public void DeleteAllItems();
    public Task DeleteAllItemsAsync();
}