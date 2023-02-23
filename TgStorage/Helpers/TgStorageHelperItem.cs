// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Models;
using TgStorage.Models.Apps;
using TgStorage.Models.Proxies;
using TgStorage.Models.Versions;

namespace TgStorage.Helpers;

public partial class TgStorageHelper : IHelper
{
    #region Public and private methods

    public T? GetItemNullable<T>() where T : ISqlTable =>
        new UnitOfWork().Query<T>().Select(item => item).FirstOrDefault();

    public T GetItemFirstOrDefault<T>() where T : ISqlTable, new() => GetItemNullable<T>() ?? new T();

    public T? GetItemNullable<T>(Guid uid) where T : ISqlTable =>
        new UnitOfWork().Query<T>().Select(item => item).FirstOrDefault(item => Equals(item.Uid, uid));

    public T GetItem<T>(Guid uid) where T : ISqlTable, new() => GetItemNullable<T>(uid) ?? new T();

    public T GetItem<T>(T item) where T : ISqlTable, new() => GetItemNullable<T>(item.Uid) ?? new T();

    public SqlTableXpLiteBase NewEmpty<T>() where T : ISqlTable
    {
        return typeof(T) switch
        {
            var cls when cls == typeof(SqlTableAppModel) => NewEmptyApp(),
            var cls when cls == typeof(SqlTableProxyModel) => NewEmptyProxy(),
            _ => new()
        };
    }

    public void AddOrUpdateItem<T>(T item) where T : ISqlTable, new()
    {
        // Try find item.
        T itemDb = GetItem(item);
        // Add item.
        if (!itemDb.IsExists)
            AddItem(item);
        // Update item.
        else
            UpdateItem(item);
    }

    private void AddItem<T>(T item) where T : ISqlTable, new()
    {
        using UnitOfWork uow = new();
        switch (typeof(T))
        {
            case var cls when cls == typeof(SqlTableAppModel):
                AddItemApp(item, uow);
                break;
            case var cls when cls == typeof(SqlTableProxyModel):
                AddItemProxy(item, uow);
                break;
            case var cls when cls == typeof(SqlTableVersionModel):
                AddItemVersion(item, uow);
                break;
        }
    }

    private void UpdateItem<T>(T item) where T : ISqlTable, new()
    {
        switch (typeof(T))
        {
            case var cls when cls == typeof(SqlTableAppModel):
                if (item is SqlTableAppModel app)
                    UpdateItemApp(app);
                break;
            case var cls when cls == typeof(SqlTableProxyModel):
                if (item is SqlTableProxyModel proxy)
                    UpdateItemProxy(proxy);
                break;
            case var cls when cls == typeof(SqlTableVersionModel):
                if (item is SqlTableVersionModel version)
                    UpdateItemVersion(version);
                break;
        }
    }

    #endregion
}