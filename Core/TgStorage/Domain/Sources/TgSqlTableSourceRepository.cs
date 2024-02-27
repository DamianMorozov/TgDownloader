// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Sources;

[DebuggerDisplay("{ToDebugString()}")]
[DoNotNotify]
public sealed class TgSqlTableSourceRepository : TgSqlRepositoryBase<TgSqlTableSourceModel>,
    ITgSqlCreateRepository<TgSqlTableSourceModel>, ITgSqlRepository<TgSqlTableSourceModel>
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlTableSourceRepository _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlTableSourceRepository Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private methods

    public TgSqlTableSourceModel CreateNew(bool isCreateSession)
    {
        TgSqlTableSourceModel item = isCreateSession
            ? new(TgSqlUtils.CreateUnitOfWork()) : new();
        item.Id = 1;
        item.Count = 1;
        item.About = "Test";
        item.Directory = TgFileUtils.GetDefaultDirectory();
        item.DtChanged = DateTime.Now;
        item.FirstId = 1;
        item.IsAutoUpdate = false;
        item.Title = "Test";
        item.UserName = "Test";
        return item;
    }

    public TgSqlTableSourceModel CreateNew(long id) => new() { Id = id };

    public override async Task<bool> DeleteAsync(TgSqlTableSourceModel item)
    {
        TgSqlTableSourceModel itemFind = await GetAsync(item.Id);
        return itemFind.IsExists && await base.DeleteAsync(itemFind);
    }

    public async Task<bool> DeleteNewAsync() => await DeleteAsync(await GetNewAsync());

    public async Task<bool> SaveAsync(TgSqlTableSourceModel item, bool isGetByUid = false)
    {
        TgSqlTableSourceModel itemFind = isGetByUid ? await GetAsync(item.Uid) : await GetAsync(item);
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

    public async Task<bool> SaveByViewModelAsync(TgSqlTableSourceViewModel itemVm) =>
        await SaveAsync(new TgSqlTableSourceModel()
        {
            Id = itemVm.SourceId,
            UserName = itemVm.SourceUserName,
            Title = itemVm.SourceTitle,
            About = itemVm.SourceAbout,
            Count = itemVm.SourceLastId,
            Directory = itemVm.SourceDirectory,
            FirstId = itemVm.SourceFirstId,
            IsAutoUpdate = itemVm.IsAutoUpdate
        });

    public override async Task<TgSqlTableSourceModel> GetAsync(Guid uid) =>
        await TgSqlUtils.CreateUnitOfWork()
            .GetObjectByKeyAsync<TgSqlTableSourceModel>(uid) 
        ?? CreateNew(true);

    public override async Task<TgSqlTableSourceModel> GetAsync(TgSqlTableSourceModel item) =>
        await GetAsync(item.Id);

    public async Task<TgSqlTableSourceModel> GetAsync(long id) =>
        await TgSqlUtils.CreateUnitOfWork()
            .FindObjectAsync<TgSqlTableSourceModel>(CriteriaOperator.Parse($"{nameof(TgSqlTableSourceModel.Id)}={id}"))
        ?? CreateNew(true);

    public async Task<TgSqlTableSourceModel> GetNewAsync()
    {
        TgSqlTableSourceModel itemNew = CreateNew(true);
        return await new UnitOfWork()
                   .Query<TgSqlTableSourceModel>().Select(i => i)
                   .FirstOrDefaultAsync(i => Equals(i.Id, itemNew.Id)
                                             && Equals(i.UserName, itemNew.UserName) && Equals(i.Title, itemNew.Title) &&
                                             Equals(i.About, itemNew.About) && Equals(i.Count, itemNew.Count) &&
                                             Equals(i.Directory, itemNew.Directory) && Equals(i.FirstId, itemNew.FirstId) &&
                                             Equals(i.IsAutoUpdate, itemNew.IsAutoUpdate))
               ?? itemNew;
    }

    public override async Task<TgSqlTableSourceModel> GetFirstAsync() =>
        await base.GetFirstAsync() ?? CreateNew(true);

    #endregion
}