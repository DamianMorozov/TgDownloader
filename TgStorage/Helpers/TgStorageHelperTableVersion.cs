// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Models.Versions;

namespace TgStorage.Helpers;

public partial class TgStorageHelper
{
    #region Public and private methods

    public SqlTableVersionModel NewEmptyVersion() =>
        new() { Uid = Guid.Empty, Version = 0, Description = string.Empty };

    private void AddItemVersion<T>(T item, UnitOfWork uow) where T : ISqlTable, new()
    {
        if (item is not SqlTableVersionModel version) return;
        version = new(uow)
        {
            DtCreated = DateTime.Now,
            DtChanged = DateTime.Now,
            Version = version.Version,
            Description = version.Description,
        };
        if (IsValidXpLite(version))
            uow.CommitChanges();
    }

    private void UpdateItemVersion(SqlTableVersionModel item)
    {
        SqlTableVersionModel? itemDb = GetItemNullable<SqlTableVersionModel>(item.Uid);
        if (itemDb is not { }) return;
        itemDb.DtChanged = DateTime.Now;
        itemDb.Version = item.Version;
        itemDb.Description = item.Description;
        if (IsValidXpLite(itemDb))
        {
            itemDb.Session.Save(itemDb);
            itemDb.Session.CommitTransaction();
        }
    }

    public SqlTableVersionModel? GetItemNullableVersion(ushort version) =>
        new UnitOfWork()
            .Query<SqlTableVersionModel>()
            .Select(item => item)
            .FirstOrDefault(item => Equals(item.Version, version));

    public SqlTableVersionModel GetItemVersion(ushort version) =>
        GetItemNullableVersion(version) ?? new();

    public List<SqlTableVersionModel> GetVersionsList() =>
        new UnitOfWork()
            .Query<SqlTableVersionModel>()
            .Select(item => item)
            .ToList();

    #endregion
}