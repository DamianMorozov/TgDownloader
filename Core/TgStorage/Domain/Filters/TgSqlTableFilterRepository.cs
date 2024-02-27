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

    #region Public and private methods

    public TgSqlTableFilterModel CreateNew(bool isCreateSession)
    {
        TgSqlTableFilterModel item = isCreateSession
            ? new(TgSqlUtils.CreateUnitOfWork()) : new();
        item.IsEnabled = true;
        item.FilterType = TgEnumFilterType.SingleName;
        item.Name = "Any";
        item.Mask = "*";
        item.SizeType = TgEnumFileSizeType.Bytes;
        return item;
    }

    public override async Task<bool> DeleteAsync(TgSqlTableFilterModel item)
    {
        TgSqlTableFilterModel itemFind = await GetAsync(item.FilterType, item.Name);
        return itemFind.IsExists && await base.DeleteAsync(itemFind);
    }

    public async Task<bool> DeleteNewAsync() => await DeleteAsync(await GetNewAsync());

    public async Task<bool> SaveAsync(TgSqlTableFilterModel item, bool isGetByUid = false)
    {
        TgSqlTableFilterModel itemFind = isGetByUid ? await GetAsync(item.Uid) : await GetAsync(item);
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

    public override async Task<TgSqlTableFilterModel> GetAsync(Guid uid) =>
        await TgSqlUtils.CreateUnitOfWork()
            .GetObjectByKeyAsync<TgSqlTableFilterModel>(uid) 
        ?? CreateNew(true);

    public override async Task<TgSqlTableFilterModel> GetAsync(TgSqlTableFilterModel item) =>
        await GetAsync(item.FilterType, item.Name);

    public async Task<TgSqlTableFilterModel> GetAsync(TgEnumFilterType filterType, string name) =>
        await TgSqlUtils.CreateUnitOfWork()
            .FindObjectAsync<TgSqlTableFilterModel>(CriteriaOperator.Parse(
            $"{nameof(TgSqlTableFilterModel.FilterType)}='{filterType}' AND " +
            $"{nameof(TgSqlTableFilterModel.Name)}='{name}'")) 
        ?? CreateNew(true);

    public async Task<TgSqlTableFilterModel> GetNewAsync()
    {
        TgSqlTableFilterModel itemNew = CreateNew(true);
        return await new UnitOfWork()
                   .Query<TgSqlTableFilterModel>().Select(i => i)
                   .FirstOrDefaultAsync(i => Equals(i.IsEnabled, itemNew.IsEnabled) &&
                                             Equals(i.FilterType, itemNew.FilterType) && Equals(i.Name, itemNew.Name) &&
                                             Equals(i.Mask, itemNew.Mask) && Equals(i.SizeType, itemNew.SizeType))
               ?? itemNew;
    }

    public IEnumerable<TgSqlTableFilterModel> GetEnumerableEnabled() => TgSqlUtils.CreateUnitOfWork()
        .Query<TgSqlTableFilterModel>()
        .Select(i => i)
        .Where(item => item.IsEnabled);

    public override async Task<TgSqlTableFilterModel> GetFirstAsync() =>
        await base.GetFirstAsync() ?? CreateNew(true);

    #endregion
}