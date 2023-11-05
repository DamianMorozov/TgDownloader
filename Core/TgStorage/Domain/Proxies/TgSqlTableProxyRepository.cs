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

    #region Public and private methods

    public TgSqlTableProxyModel CreateNew(bool isCreateSession)
    {
        TgSqlTableProxyModel proxy = isCreateSession
            ? new(TgSqlUtils.CreateUnitOfWork()) : new();
        proxy.Type = TgEnumProxyType.None;
        proxy.HostName = "No proxy";
        proxy.Port = 404;
        proxy.UserName = "No user";
        proxy.Password = "No password";
        proxy.Secret = string.Empty;
        return proxy;
    }

    public override async Task<bool> DeleteAsync(TgSqlTableProxyModel item)
    {
        TgSqlTableProxyModel itemFind = await GetAsync(item.Type, item.HostName, item.Port);
        return itemFind.IsExists && await base.DeleteAsync(itemFind);
    }

    public async Task<bool> DeleteNewAsync() => await DeleteAsync(await GetNewAsync());

    public async Task<bool> SaveAsync(TgSqlTableProxyModel item, bool isGetByUid = false)
    {
        TgSqlTableProxyModel itemFind = isGetByUid ? await GetAsync(item.Uid) : await GetAsync(item);
        if (itemFind.IsNotExists)
        {
            itemFind.Fill(item);
            ValidationResult validationResult = TgSqlUtils.GetValidXpLite(itemFind);
            return validationResult.IsValid && await TgSqlUtils.TryInsertAsync(itemFind);
        }
        else
        {
            itemFind.Fill(item, itemFind.Uid);
            ValidationResult validationResult = TgSqlUtils.GetValidXpLite(itemFind);
            return validationResult.IsValid && await TgSqlUtils.TryUpdateAsync(itemFind);
        }
    }

    public override async Task<TgSqlTableProxyModel> GetAsync(Guid uid) =>
        await TgSqlUtils.CreateUnitOfWork()
            .GetObjectByKeyAsync<TgSqlTableProxyModel>(uid) 
        ?? CreateNew(true);

    public override async Task<TgSqlTableProxyModel> GetAsync(TgSqlTableProxyModel item) =>
        await GetAsync(item.Type, item.HostName, item.Port);

    public async Task<TgSqlTableProxyModel> GetAsync(TgEnumProxyType proxyType, string hostName, ushort port) =>
        await TgSqlUtils.CreateUnitOfWork()
            .FindObjectAsync<TgSqlTableProxyModel>(CriteriaOperator.Parse(
            $"{nameof(TgSqlTableProxyModel.Type)}='{proxyType}' AND {nameof(TgSqlTableProxyModel.HostName)}='{hostName}' AND " +
            $"{nameof(TgSqlTableProxyModel.Port)}={port}")) 
        ?? CreateNew(true);

    public async Task<TgSqlTableProxyModel> GetNewAsync()
    {
        TgSqlTableProxyModel itemNew = CreateNew(true);
        return await new UnitOfWork()
                   .Query<TgSqlTableProxyModel>().Select(i => i)
                   .FirstOrDefaultAsync(i => Equals(i.Type, itemNew.Type) &&
                                             Equals(i.HostName, itemNew.HostName) && Equals(i.Port, itemNew.Port) &&
                                             Equals(i.UserName, itemNew.UserName) && Equals(i.Password, itemNew.Password))
               ?? itemNew;
    }

    public override async Task<TgSqlTableProxyModel> GetFirstAsync() =>
        await base.GetFirstAsync() ?? CreateNew(true);

    public IReadOnlyList<TgEnumProxyType> GetProxyTypes() => 
        Enum.GetValues(typeof(TgEnumProxyType)).Cast<TgEnumProxyType>().ToList();

    #endregion
}