// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;
using TgCore.Enums;
using TgStorage.Models.Proxies;

namespace TgStorage.Helpers;

public partial class TgStorageHelper
{
    #region Public and private methods

    public SqlTableProxyModel NewEmptyProxy() =>
        new() { Uid = Guid.Empty, HostName = "Test", Port = 404, Type = ProxyType.Http, UserName = "Test", Password = "Test" };

    private void AddItemProxy<T>(T item, UnitOfWork uow) where T : ISqlTable, new()
    {
        if (item is not SqlTableProxyModel proxy) return;
        proxy = new(uow)
        {
            DtCreated = DateTime.Now,
            DtChanged = DateTime.Now,
            Type = proxy.Type,
            HostName = proxy.HostName,
            Port = proxy.Port,
            Secret = proxy.Secret,
        };
        if (IsValidXpLite(proxy))
            uow.CommitChanges();
    }

    private void UpdateItemProxy(SqlTableProxyModel item)
    {
        SqlTableProxyModel? itemDb = GetItemNullable<SqlTableProxyModel>(item.Uid);
        if (itemDb is not { }) return;
        itemDb.DtChanged = DateTime.Now;
        itemDb.Type = item.Type;
        itemDb.HostName = item.HostName;
        itemDb.Port = item.Port;
        itemDb.UserName = item.UserName;
        itemDb.Secret = item.Secret;
        if (IsValidXpLite(itemDb))
        {
            itemDb.Session.Save(itemDb);
            itemDb.Session.CommitTransaction();
        }
    }

    public SqlTableProxyModel? GetItemNullableProxy(ProxyType proxyType, string hostName, ushort port) =>
        new UnitOfWork()
            .Query<SqlTableProxyModel>()
            .Select(item => item)
            .FirstOrDefault(item =>
                Equals(item.Type, proxyType) &&
                           Equals(item.HostName, hostName) &&
                Equals(item.Port, port));

    public SqlTableProxyModel GetItemProxy(ProxyType proxyType, string hostName, ushort port) =>
        GetItemNullableProxy(proxyType, hostName, port) ?? new();

    #endregion
}