// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Models.Apps;

namespace TgStorage.Helpers;

public partial class TgStorageHelper
{
    #region Public and private methods

    public SqlTableAppModel NewEmptyApp() =>
        new() { Uid = Guid.Empty, ApiHash = Guid.Empty.ToString().Replace("-", ""), PhoneNumber = "+12345678999"};

    private void AddItemApp<T>(T item, UnitOfWork uow) where T : ISqlTable, new()
    {
        if (item is not SqlTableAppModel itemDb) return;
        itemDb = new (uow)
        {
            DtCreated = DateTime.Now,
            DtChanged = DateTime.Now,
            ApiHash = itemDb.ApiHash,
            PhoneNumber = itemDb.PhoneNumber,
            ProxyUid = itemDb.ProxyUid,
            DbVersion = itemDb.GetLastDbVersion(),
        };
        if (IsValidXpLite(itemDb))
            uow.CommitChanges();
    }

    private void UpdateItemApp(SqlTableAppModel item)
    {
        SqlTableAppModel? itemDb = GetItemNullable<SqlTableAppModel>(item.Uid);
        if (itemDb is not { }) return;
            itemDb.DtChanged = DateTime.Now;
        itemDb.ApiHash = item.ApiHash;
        itemDb.PhoneNumber = item.PhoneNumber;
        itemDb.ProxyUid = item.ProxyUid;
        itemDb.DbVersion = item.GetLastDbVersion();
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

    public SqlTableAppModel? GetItemNullableApp() =>
        new UnitOfWork()
            .Query<SqlTableAppModel>()
            .Select(item => item)
            .FirstOrDefault(item => !Equals(item.ApiHash, Guid.Empty.ToString().Replace("-", "")));

    public SqlTableAppModel GetItemApp() =>
        GetItemNullableApp() ?? new();

    #endregion
}