// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

/// <summary>
/// SQL table helper base.
/// </summary>
/// <typeparam name="T"></typeparam>
[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public abstract class TgSqlRepositoryBase<T> : TgCommonBase where T : ITgSqlTable, new()
{
    #region Public and private methods

    public virtual async Task<bool> DeleteAsync(T item) =>
        !item.IsNotExists && await TgSqlUtils.TryDeleteAsync(item);

    public virtual async Task<bool> DeleteAllItemsAsync()
    {
        IEnumerable<T> items = GetEnumerable();
        bool result = true;
        foreach (T item in items)
        {
            if (item.IsExists)
                if (!await DeleteAsync(item))
                    result = false;
        }
        return result;
    }

    public virtual async Task<T> GetAsync(Guid uid) =>
        await TgSqlUtils.CreateUnitOfWork()
        .GetObjectByKeyAsync<T>(uid);

    public virtual async Task<T> GetAsync(T item)
    {
        if (item is not XPLiteObject xpLite) return new();
        return await TgSqlUtils.CreateUnitOfWork()
            .GetObjectByKeyAsync<T>(xpLite.Session.GetKeyValue(item));
    }

    public virtual IEnumerable<T> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All) =>
        topRecords switch
        {
	        TgSqlEnumTableTopRecords.Top20 => GetEnumerable(20),
			TgSqlEnumTableTopRecords.Top200 => GetEnumerable(200),
            TgSqlEnumTableTopRecords.Top1000 => GetEnumerable(1_000),
            TgSqlEnumTableTopRecords.Top10000 => GetEnumerable(10_000),
            TgSqlEnumTableTopRecords.Top100000 => GetEnumerable(100_000),
            TgSqlEnumTableTopRecords.Top1000000 => GetEnumerable(1_000_000),
            _ => GetEnumerable(0),
        };

    public virtual IEnumerable<T> GetEnumerable(int count) =>
        count > 0 ? TgSqlUtils.CreateUnitOfWork().Query<T>().Select(i => i).Take(count) : TgSqlUtils.CreateUnitOfWork().Query<T>().Select(i => i);

    public virtual async Task<T> GetFirstAsync() => 
        await TgSqlUtils.CreateUnitOfWork()
            .Query<T>()
            .FirstOrDefaultAsync();

    #endregion
}