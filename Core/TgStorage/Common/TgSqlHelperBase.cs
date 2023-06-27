// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

/// <summary>
/// SQL table helper base.
/// </summary>
/// <typeparam name="T"></typeparam>
[DoNotNotify]
public class TgSqlHelperBase<T> where T : TgSqlTableBase, new()
{
    #region Public and private methods

    public virtual string TableName => throw new NotImplementedException("Use override method!");
    
    protected readonly object Locker = new();

	public virtual T NewItem() => throw new NotImplementedException("Use override method!");

    public virtual T NewItem(Session session) => throw new NotImplementedException("Use override method!");

    public virtual T GetNewItem() => throw new NotImplementedException("Use override method!");

    public virtual T GetItemLast() => NewItem();

    public virtual T GetItemFirstOrDefault() => new UnitOfWork().Query<T>().Select(item => item).FirstOrDefault() ?? NewItem();

    public virtual T GetItem(Guid uid) => new UnitOfWork().Query<T>().Select(item => item).FirstOrDefault(item => Equals(item.Uid, uid)) ?? NewItem();

    public virtual T GetItem(T item) => GetItem(item.Uid);

    public virtual T GetCurrentItem() => throw new NotImplementedException("Use override method!");

    public virtual List<T> GetList(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All) => topRecords switch
    {
        TgSqlEnumTableTopRecords.Top200 => new UnitOfWork().Query<T>().Select(item => item).Take(200).ToList(),
        TgSqlEnumTableTopRecords.Top1000 => new UnitOfWork().Query<T>().Select(item => item).Take(1_000).ToList(),
        TgSqlEnumTableTopRecords.Top10000 => new UnitOfWork().Query<T>().Select(item => item).Take(10_000).ToList(),
        TgSqlEnumTableTopRecords.Top100000 => new UnitOfWork().Query<T>().Select(item => item).Take(100_000).ToList(),
        TgSqlEnumTableTopRecords.Top1000000 => new UnitOfWork().Query<T>().Select(item => item).Take(1_000_000).ToList(),
        _ => new UnitOfWork().Query<T>().Select(item => item).ToList(),
    };

    public virtual bool AddItem(T item) => throw new NotImplementedException("Use override method!");

    public virtual bool AddOrUpdateItem(T item) => throw new NotImplementedException("Use override method!");

    public virtual ValidationResult GetValidXpLite(T item) => throw new NotImplementedException("Use override method!");

    public virtual bool UpdateItem(T itemSource, T itemDest) => throw new NotImplementedException("Use override method!");

    public virtual bool DeleteItem(T item)
    {
        T itemDb = GetItem(item.Uid);
        if (itemDb.IsNotExists) return false;
        itemDb.Session.Delete(itemDb);
        itemDb.Session.CommitTransactionAsync();
        return true;
    }

    public virtual bool DeleteAllItems()
    {
        List<T> items = GetList();
        if (!items.Any()) return false;
        foreach (T? item in from T item in items where item.IsExists select item)
        {
            DeleteItem(item);
        }
        return true;
    }

    #endregion
}