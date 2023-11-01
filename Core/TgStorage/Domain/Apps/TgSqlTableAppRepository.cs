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

    #region Public and private fields, properties, constructor

    public string TableName => TgSqlConstants.TableApps;

    #endregion

    #region Public and private methods

    public TgSqlTableAppModel CreateNew(bool isCreateSession) => isCreateSession 
        ? new(TgSqlUtils.CreateUnitOfWork()) { PhoneNumber = "+00000000000" }
        : new () { PhoneNumber = "+00000000000" };

    public override bool Delete(TgSqlTableAppModel item)
    {
        TgSqlTableAppModel itemFind = Get(item);
        return itemFind.IsExists && base.Delete(itemFind);
    }

    public bool DeleteNew() => Delete(GetNew());

    public bool Save(TgSqlTableAppModel item, bool isGetByUid = false)
    {
        TgSqlTableAppModel itemFind = Get(item);
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

    public override TgSqlTableAppModel Get(TgSqlTableAppModel item) => Get(item.ApiHash);

    public override TgSqlTableAppModel Get(Guid apiHash) => TgSqlUtils.CreateUnitOfWork()
        .FindObject<TgSqlTableAppModel>(CriteriaOperator.Parse(
            $"{nameof(TgSqlTableAppModel.ApiHash)}='{apiHash}'")) ?? CreateNew(true);

    public TgSqlTableAppModel GetNew()
    {
        TgSqlTableAppModel itemNew = CreateNew(true);
        return new UnitOfWork()
           .Query<TgSqlTableAppModel>()
           .Select(i => i)
           .FirstOrDefault(i => Equals(i.ApiHash, itemNew.ApiHash) && Equals(i.PhoneNumber, itemNew.PhoneNumber)) 
            ?? itemNew;
    }

    public override TgSqlTableAppModel GetFirst() => base.GetFirst() ?? CreateNew(true);

    public Guid GetFirstProxyUid => GetFirst().ProxyUid;

    public TgSqlTableProxyModel GetCurrentProxy() =>
        TgSqlTableProxyRepository.Instance.Get(GetFirstProxyUid) ?? TgSqlTableProxyRepository.Instance.GetNew();

    #endregion
}