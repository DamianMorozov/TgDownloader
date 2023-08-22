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

    public virtual bool Delete(T item) =>
        !item.IsNotExists && TgSqlUtils.TryDeleteAsync(item).Result;

    public virtual bool DeleteAllItems()
    {
        IEnumerable<T> items = GetEnumerable();
        bool result = true;
        foreach (T item in items)
        {
            if (item.IsExists)
                if (!Delete(item))
                    result = false;
        }
        return result;
    }

    public virtual T? Get(Guid uid) => TgSqlUtils.CreateUnitOfWork()
        .GetObjectByKey<T?>(uid);

    public virtual T? Get(T item)
    {
        if (item is not XPLiteObject xpLite) return default;
        return TgSqlUtils.CreateUnitOfWork()
            .GetObjectByKey<T?>(xpLite.Session.GetKeyValue(item));
    }

    public virtual IEnumerable<T> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All) =>
        topRecords switch
        {
            TgSqlEnumTableTopRecords.Top200 => GetEnumerable(200),
            TgSqlEnumTableTopRecords.Top1000 => GetEnumerable(1_000),
            TgSqlEnumTableTopRecords.Top10000 => GetEnumerable(10_000),
            TgSqlEnumTableTopRecords.Top100000 => GetEnumerable(100_000),
            TgSqlEnumTableTopRecords.Top1000000 => GetEnumerable(1_000_000),
            _ => TgSqlUtils.CreateUnitOfWork().Query<T>().Select(i => i),
        };

    public virtual IEnumerable<T> GetEnumerable(int count) =>
        TgSqlUtils.CreateUnitOfWork().Query<T>().Select(i => i).Take(count);

    public virtual T? GetFirst() => TgSqlUtils.CreateUnitOfWork()
        .Query<T>()
        .FirstOrDefault();

    #endregion
}