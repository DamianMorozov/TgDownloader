// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Proxies;

[DebuggerDisplay("{ToDebugString()}")]
[DoNotNotify]
public sealed class TgSqlTableProxyRepository : TgSqlRepositoryBase<TgSqlTableProxyModel>,
    ITgSqlCreateRepository<TgSqlTableProxyModel>, ITgSqlRepository<TgSqlTableProxyModel>
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlTableProxyRepository _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlTableProxyRepository Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public string TableName => TgSqlConstants.TableProxies;

    #endregion

    #region Public and private methods

    public TgSqlTableProxyModel CreateNew(bool isCreateSession) => isCreateSession
        ? new(TgSqlUtils.CreateUnitOfWork())
        {
            Type = TgEnumProxyType.None,
            HostName = "No proxy",
            Port = 404,
            UserName = "No user",
            Password = "No password",
            Secret = string.Empty
        }
        : new()
        {
            Type = TgEnumProxyType.None,
            HostName = "No proxy",
            Port = 404,
            UserName = "No user",
            Password = "No password",
            Secret = string.Empty
        };

    public override bool Delete(TgSqlTableProxyModel item)
    {
        TgSqlTableProxyModel itemFind = Get(item.Type, item.HostName, item.Port);
        return itemFind.IsExists && base.Delete(itemFind);
    }

    public bool DeleteNew() => Delete(GetNew());

    public bool Save(TgSqlTableProxyModel item, bool isGetByUid = false)
    {
        TgSqlTableProxyModel itemFind = isGetByUid ? Get(item.Uid) : Get(item);
        if (itemFind.IsNotExists)
        {
            itemFind.Fill(item);
            ValidationResult validationResult = TgSqlUtils.GetValidXpLite(itemFind);
            return validationResult.IsValid && TgSqlUtils.TryInsertAsync(itemFind).Result;
        }
        else
        {
            itemFind.Fill(item, itemFind.Uid);
            ValidationResult validationResult = TgSqlUtils.GetValidXpLite(itemFind);
            return validationResult.IsValid && TgSqlUtils.TryUpdateAsync(itemFind).Result;
        }
    }

    public override TgSqlTableProxyModel Get(Guid uid) =>
        TgSqlUtils.CreateUnitOfWork().GetObjectByKey<TgSqlTableProxyModel>(uid) ?? CreateNew(true);

    public override TgSqlTableProxyModel Get(TgSqlTableProxyModel item) => 
        Get(item.Type, item.HostName, item.Port);

    public TgSqlTableProxyModel Get(TgEnumProxyType proxyType, string hostName, ushort port) => TgSqlUtils.CreateUnitOfWork()
        .FindObject<TgSqlTableProxyModel>(CriteriaOperator.Parse(
            $"{nameof(TgSqlTableProxyModel.Type)}='{proxyType}' AND {nameof(TgSqlTableProxyModel.HostName)}='{hostName}' AND " +
            $"{nameof(TgSqlTableProxyModel.Port)}={port}")) ?? CreateNew(true);

    public TgSqlTableProxyModel GetNew()
    {
        TgSqlTableProxyModel itemNew = CreateNew(true);
        return new UnitOfWork()
            .Query<TgSqlTableProxyModel>().Select(i => i)
            .FirstOrDefault(i => Equals(i.Type, itemNew.Type) &&
                Equals(i.HostName, itemNew.HostName) && Equals(i.Port, itemNew.Port) &&
                Equals(i.UserName, itemNew.UserName) && Equals(i.Password, itemNew.Password)) 
            ?? itemNew;
    }

    public override TgSqlTableProxyModel GetFirst() => base.GetFirst() ?? CreateNew(true);

    public IReadOnlyList<TgEnumProxyType> GetProxyTypes() => 
        Enum.GetValues(typeof(TgEnumProxyType)).Cast<TgEnumProxyType>().ToList();

    #endregion
}