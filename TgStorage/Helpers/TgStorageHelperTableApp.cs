// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;
using TgStorage.Models.Apps;

namespace TgStorage.Helpers;

public partial class TgStorageHelper
{
    #region Public and private methods

    public SqlTableAppModel NewEmptyApp() =>
        new() { Uid = Guid.Empty, ApiHash = Guid.Empty.ToString().Replace("-", ""), PhoneNumber = "+12345678999"};

    private void AddItemApp<T>(T item, UnitOfWork uow) where T : ISqlTable, new()
    {
        if (item is not SqlTableAppModel app) return;
        app = new (uow)
        {
            DtCreated = DateTime.Now,
            DtChanged = DateTime.Now,
            ApiHash = app.ApiHash,
            PhoneNumber = app.PhoneNumber,
            IsUseProxy = app.IsUseProxy,
            ProxyUid = app.ProxyUid,
            DbVersion = 10,
        };
        if (IsValidXpLite(app))
            uow.CommitChanges();
    }

    private void UpdateItemApp(SqlTableAppModel item)
    {
        SqlTableAppModel? itemDb = GetItemNullable<SqlTableAppModel>(item.Uid);
        if (itemDb is not { }) return;
            itemDb.DtChanged = DateTime.Now;
        itemDb.ApiHash = item.ApiHash;
        itemDb.PhoneNumber = item.PhoneNumber;
        itemDb.IsUseProxy = item.IsUseProxy;
        itemDb.ProxyUid = item.ProxyUid;
        itemDb.DbVersion = 10;
        if (IsValidXpLite(itemDb))
        {
            itemDb.Session.Save(itemDb);
            itemDb.Session.CommitTransaction();
        }
    }

    public SqlTableAppModel? GetItemNullableApp(string apiHash) =>
        new UnitOfWork()
            .Query<SqlTableAppModel>()
            .Select(item => item)
            .FirstOrDefault(item => Equals(item.ApiHash, apiHash));

    public SqlTableAppModel GetItemApp(string apiHash) =>
        GetItemNullableApp(apiHash) ?? new();

    #endregion
}