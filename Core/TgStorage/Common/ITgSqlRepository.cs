// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

public interface ITgSqlRepository<T> where T : ITgSqlTable, new()
{
    public Task<bool> DeleteAsync(T item);
    public Task<bool> DeleteNewAsync();
    public Task<bool> SaveAsync(T item, bool isGetByUid = false);
    public Task<T> GetAsync(T item);
    public Task<T> GetNewAsync();
    public Task<T> GetFirstAsync();
    public IEnumerable<T> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All);
    public IEnumerable<T> GetEnumerable(int count);
}