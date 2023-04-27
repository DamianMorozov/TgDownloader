// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Core;

/// <summary>
/// SQL table helper base.
/// </summary>
/// <typeparam name="T"></typeparam>
public class TgSqlHelperBase<T> where T : TgSqlTableBase, new()
{
    #region Public and private methods

    public virtual string TableName => throw new NotImplementedException("Use override method!");

    public virtual T NewItem() => throw new NotImplementedException("Use override method!");

    public virtual T NewItem(Session session) => throw new NotImplementedException("Use override method!");

    public virtual T GetNewItem() => throw new NotImplementedException("Use override method!");

    public virtual T GetItemLast() => NewItem();

    public virtual T GetItemFirstOrDefault() => new UnitOfWork().Query<T>().Select(item => item).FirstOrDefault() ?? NewItem();

    public virtual T GetItem(Guid uid) => new UnitOfWork().Query<T>().Select(item => item).FirstOrDefault(item => Equals(item.Uid, uid)) ?? NewItem();

    public virtual T GetItem(T item) => GetItem(item.Uid);

    public virtual T GetCurrentItem() => throw new NotImplementedException("Use override method!");

    public virtual List<T> GetList(bool isTop200) =>
        !isTop200 ? new UnitOfWork().Query<T>().Select(item => item).ToList() : new UnitOfWork().Query<T>().Select(item => item).Take(200).ToList();

    public virtual bool AddItem(T item) => throw new NotImplementedException("Use override method!");

    public virtual bool AddOrUpdateItem(T item) => throw new NotImplementedException("Use override method!");

    public virtual bool IsValidXpLite(T item) => throw new NotImplementedException("Use override method!");

    public virtual bool UpdateItem(T itemSource, T itemDest) => throw new NotImplementedException("Use override method!");

    public virtual bool DeleteItem(T item)
    {
        T itemDb = GetItem(item.Uid);
        if (itemDb.IsNotExists) return false;
        itemDb.Session.Delete(itemDb);
        itemDb.Session.CommitTransaction();
        return true;
    }

    public virtual bool DeleteAllItems()
    {
        List<T> items = GetList(false);
        if (!items.Any()) return false;
		foreach (var item in from T item in items where item.IsExists select item)
		{
			DeleteItem(item);
		}
		return true;
    }

    #endregion
}