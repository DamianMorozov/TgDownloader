// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Apps;

[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgSqlTableAppRepository : TgSqlRepositoryBase<TgSqlTableAppModel>,
    ITgSqlCreateRepository<TgSqlTableAppModel>, ITgSqlRepository<TgSqlTableAppModel>
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlTableAppRepository _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlTableAppRepository Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private methods

    public TgSqlTableAppModel CreateNew(bool isCreateSession)
    {
        TgSqlTableAppModel item = isCreateSession
            ? new(TgSqlUtils.CreateUnitOfWork()) : new();
        item.PhoneNumber = "+00000000000";
        return item;
    }

    public override async Task<bool> DeleteAsync(TgSqlTableAppModel item)
    {
        TgSqlTableAppModel itemFind = await GetAsync(item);
        return itemFind.IsExists && await base.DeleteAsync(itemFind);
    }

    public async Task<bool> DeleteNewAsync() => await DeleteAsync(await GetNewAsync());

    public async Task<bool> SaveAsync(TgSqlTableAppModel item, bool isGetByUid = false)
    {
        TgSqlTableAppModel itemFind = await GetAsync(item);
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

    public override async Task<TgSqlTableAppModel> GetAsync(TgSqlTableAppModel item) =>
        await GetByHashAsync(item.ApiHash);

    public async Task<TgSqlTableAppModel> GetByHashAsync(Guid apiHash) =>
        await TgSqlUtils.CreateUnitOfWork()
        .FindObjectAsync<TgSqlTableAppModel>(CriteriaOperator.Parse(
            $"{nameof(TgSqlTableAppModel.ApiHash)}='{apiHash}'")) 
        ?? CreateNew(true);

    public async Task<TgSqlTableAppModel> GetNewAsync()
    {
        TgSqlTableAppModel itemNew = CreateNew(true);
        return await new UnitOfWork()
                   .Query<TgSqlTableAppModel>()
                   .Select(i => i)
                   .FirstOrDefaultAsync(i => Equals(i.ApiHash, itemNew.ApiHash) && Equals(i.PhoneNumber, itemNew.PhoneNumber))
               ?? itemNew;
    }

    public override async Task<TgSqlTableAppModel> GetFirstAsync() => 
        await base.GetFirstAsync() ?? CreateNew(true);

    public async Task<Guid> GetFirstProxyUidAsync()
    {
        TgSqlTableAppModel app = await GetFirstAsync();
        return app.ProxyUid;
    }

    public async Task<TgSqlTableProxyModel> GetCurrentProxyAsync()
    {
        var proxyUid = await GetFirstProxyUidAsync();
        return await TgSqlTableProxyRepository.Instance.GetAsync(proxyUid) ?? await TgSqlTableProxyRepository.Instance.GetNewAsync();
    }


    #endregion
}