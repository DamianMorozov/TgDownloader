// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Versions;

[DebuggerDisplay("{ToDebugString()}")]
[DoNotNotify]
public sealed class TgSqlTableVersionRepository : TgSqlRepositoryBase<TgSqlTableVersionModel>,
    ITgSqlCreateRepository<TgSqlTableVersionModel>, ITgSqlRepository<TgSqlTableVersionModel>
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlTableVersionRepository _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlTableVersionRepository Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public string TableName => TgSqlConstants.TableVersions;

    #endregion

    #region Public and private methods

    public TgSqlTableVersionModel CreateNew(bool isCreateSession) => isCreateSession
        ? new(TgSqlUtils.CreateUnitOfWork())
            { Version = short.MaxValue, Description = "New version" }
        : new()
            { Version = short.MaxValue, Description = "New version" };

    public override bool Delete(TgSqlTableVersionModel item)
    {
        TgSqlTableVersionModel itemFind = Get(item.Version);
        return itemFind.IsExists && base.Delete(itemFind);
    }

    public bool DeleteNew() => Delete(GetNew());

    public bool Save(TgSqlTableVersionModel item)
    {
        TgSqlTableVersionModel itemFind = Get(item);
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

    public override TgSqlTableVersionModel Get(TgSqlTableVersionModel item) =>
        Get(item.Version);

    public TgSqlTableVersionModel Get(short version) => TgSqlUtils.CreateUnitOfWork()
        .FindObject<TgSqlTableVersionModel>(CriteriaOperator.Parse(
            $"{nameof(TgSqlTableVersionModel.Version)}={version}")) ?? CreateNew(true);

    public TgSqlTableVersionModel GetItemLast() => TgSqlUtils.CreateUnitOfWork()
        .Query<TgSqlTableVersionModel>()
        .Select(i => i)
        .OrderByDescending(item => item.Version)
        .FirstOrDefault() 
        ?? CreateNew(true);

    public TgSqlTableVersionModel GetNew()
    {
        TgSqlTableVersionModel itemNew = CreateNew(true);
        return new UnitOfWork()
        .Query<TgSqlTableVersionModel>()
        .Select(i => i)
        .FirstOrDefault(i => Equals(i.Version, itemNew.Version) && Equals(i.Description, itemNew.Description)) 
        ?? itemNew;
    }

    public override TgSqlTableVersionModel GetFirst() => base.GetFirst() ?? CreateNew(true);

    public short LastVersion => 18;

    #endregion
}