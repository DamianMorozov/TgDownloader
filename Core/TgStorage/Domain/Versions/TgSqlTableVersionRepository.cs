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

    #region Public and private methods

    public TgSqlTableVersionModel CreateNew(bool isCreateSession)
    {
        TgSqlTableVersionModel item = isCreateSession 
            ? new(TgSqlUtils.CreateUnitOfWork()) : new();
        item.Version = short.MaxValue;
        item.Description = "New version";
        return item;
    }

    public override async Task<bool> DeleteAsync(TgSqlTableVersionModel item)
    {
        TgSqlTableVersionModel itemFind = await GetAsync(item.Version);
        return itemFind.IsExists && await base.DeleteAsync(itemFind);
    }

    public async Task<bool> DeleteNewAsync() => await DeleteAsync(await GetNewAsync());

    public async Task<bool> SaveAsync(TgSqlTableVersionModel item, bool isGetByUid = false)
    {
        TgSqlTableVersionModel itemFind = isGetByUid ? await GetAsync(item.Uid) : await GetAsync(item);
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

    public override async Task<TgSqlTableVersionModel> GetAsync(Guid uid) =>
        await TgSqlUtils.CreateUnitOfWork()
            .GetObjectByKeyAsync<TgSqlTableVersionModel>(uid) 
        ?? CreateNew(true);

    public override async Task<TgSqlTableVersionModel> GetAsync(TgSqlTableVersionModel item) =>
        await GetAsync(item.Version);

    public async Task<TgSqlTableVersionModel> GetAsync(short version) =>
        await TgSqlUtils.CreateUnitOfWork()
        .FindObjectAsync<TgSqlTableVersionModel>(CriteriaOperator.Parse(
            $"{nameof(TgSqlTableVersionModel.Version)}={version}")) 
        ?? CreateNew(true);

    public async Task<TgSqlTableVersionModel> GetItemLastAsync() => 
        await TgSqlUtils.CreateUnitOfWork()
            .Query<TgSqlTableVersionModel>()
            .Select(i => i)
            .OrderByDescending(item => item.Version)
            .FirstOrDefaultAsync() 
        ?? CreateNew(true);

    public async Task<TgSqlTableVersionModel> GetNewAsync()
    {
        TgSqlTableVersionModel itemNew = CreateNew(true);
        return await new UnitOfWork()
                   .Query<TgSqlTableVersionModel>()
                   .Select(i => i)
                   .FirstOrDefaultAsync(i => Equals(i.Version, itemNew.Version) && Equals(i.Description, itemNew.Description))
               ?? itemNew;
    }

    public override async Task<TgSqlTableVersionModel> GetFirstAsync() => 
        await base.GetFirstAsync() ?? CreateNew(true);

    public short LastVersion => 18;

    #endregion
}