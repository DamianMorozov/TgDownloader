// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Filters;

[DebuggerDisplay("{ToDebugString()}")]
[DoNotNotify]
public sealed class TgSqlTableFilterRepository : TgSqlRepositoryBase<TgSqlTableFilterModel>,
    ITgSqlCreateRepository<TgSqlTableFilterModel>, ITgSqlRepository<TgSqlTableFilterModel>
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlTableFilterRepository _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlTableFilterRepository Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public string TableName => TgSqlConstants.TableFilters;

    #endregion

    #region Public and private methods

    public TgSqlTableFilterModel CreateNew(bool isCreateSession) => isCreateSession 
        ? new(TgSqlUtils.CreateUnitOfWork())
            { IsEnabled = true, FilterType = TgEnumFilterType.SingleName, Name = "Any", Mask = "*", SizeType = TgEnumFileSizeType.Bytes }
        : new() 
            { IsEnabled = true, FilterType = TgEnumFilterType.SingleName, Name = "Any", Mask = "*", SizeType = TgEnumFileSizeType.Bytes };

    public override bool Delete(TgSqlTableFilterModel item)
    {
        TgSqlTableFilterModel itemFind = Get(item.FilterType, item.Name);
        return itemFind.IsExists && base.Delete(itemFind);
    }

    public bool DeleteNew() => Delete(GetNew());

    public bool Save(TgSqlTableFilterModel item, bool isGetByUid = false)
    {
        TgSqlTableFilterModel itemFind = isGetByUid ? Get(item.Uid) :  Get(item);
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

    public override TgSqlTableFilterModel Get(Guid uid) =>
        TgSqlUtils.CreateUnitOfWork().GetObjectByKey<TgSqlTableFilterModel>(uid) ?? CreateNew(true);

    public override TgSqlTableFilterModel Get(TgSqlTableFilterModel item) => 
        Get(item.FilterType, item.Name);

    public TgSqlTableFilterModel Get(TgEnumFilterType filterType, string name) => TgSqlUtils.CreateUnitOfWork()
        .FindObject<TgSqlTableFilterModel>(CriteriaOperator.Parse(
            $"{nameof(TgSqlTableFilterModel.FilterType)}='{filterType}' AND " +
            $"{nameof(TgSqlTableFilterModel.Name)}='{name}'")) ?? CreateNew(true); 

    public TgSqlTableFilterModel GetNew()
    {
        TgSqlTableFilterModel itemNew = CreateNew(true);
        return new UnitOfWork()
            .Query<TgSqlTableFilterModel>().Select(i => i)
            .FirstOrDefault(i => Equals(i.IsEnabled, itemNew.IsEnabled) &&
                Equals(i.FilterType, itemNew.FilterType) && Equals(i.Name, itemNew.Name) &&
                Equals(i.Mask, itemNew.Mask) && Equals(i.SizeType, itemNew.SizeType)) 
            ?? itemNew;
    }

    public IEnumerable<TgSqlTableFilterModel> GetEnumerableEnabled() => TgSqlUtils.CreateUnitOfWork()
        .Query<TgSqlTableFilterModel>()
        .Select(i => i)
        .Where(item => item.IsEnabled);

    public override TgSqlTableFilterModel GetFirst() => base.GetFirst() ?? CreateNew(true);

    #endregion
}