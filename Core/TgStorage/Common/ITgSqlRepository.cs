// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

public interface ITgSqlRepository<T> where T : ITgSqlTable, new()
{
    public string TableName { get; }

    public bool Delete(T item);
    public bool DeleteNew();
    public bool Save(T item, bool isGetByUid = false);
    public T Get(T item);
    public T GetNew();
    public T GetFirst();
    public IEnumerable<T> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All);
    public IEnumerable<T> GetEnumerable(int count);
}