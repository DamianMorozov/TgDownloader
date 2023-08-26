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

    #region Public and private fields, properties, constructor

    public string TableName => TgSqlConstants.TableSources;

    #endregion

    #region Public and private methods

    public TgSqlTableSourceModel CreateNew(bool isCreateSession) => isCreateSession
        ? new(TgSqlUtils.CreateUnitOfWork())
        {
            Id = 1,
            Count = 1,
            About = "Test",
            Directory = "C:",
            DtChanged = DateTime.Now,
            FirstId = 1,
            IsAutoUpdate = false,
            Title = "Test",
            UserName = "Test"
        }
        : new()
        {
            Id = 1,
            Count = 1,
            About = "Test",
            Directory = "C:",
            DtChanged = DateTime.Now,
            FirstId = 1,
            IsAutoUpdate = false,
            Title = "Test",
            UserName = "Test"
        };

    public TgSqlTableSourceModel CreateNew(long id) => new() { Id = id };

    public override bool Delete(TgSqlTableSourceModel item)
    {
        TgSqlTableSourceModel itemFind = Get(item.Id);
        return itemFind.IsExists && base.Delete(itemFind);
    }

    public bool DeleteNew() => Delete(GetNew());

    public bool Save(TgSqlTableSourceModel item)
    {
        TgSqlTableSourceModel itemFind = Get(item);
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

    public bool SaveByViewModel(TgSqlTableSourceViewModel itemVm) =>
        Save(new TgSqlTableSourceModel()
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

    public override TgSqlTableSourceModel Get(TgSqlTableSourceModel item) =>
        Get(item.Id);

    public TgSqlTableSourceModel Get(long id) => TgSqlUtils.CreateUnitOfWork()
        .FindObject<TgSqlTableSourceModel>(CriteriaOperator.Parse(
            $"{nameof(TgSqlTableSourceModel.Id)}={id}")) ?? CreateNew(true); 

    public TgSqlTableSourceModel GetNew()
    {
        TgSqlTableSourceModel itemNew = CreateNew(true);
        return new UnitOfWork()
            .Query<TgSqlTableSourceModel>().Select(i => i)
            .FirstOrDefault(i => Equals(i.Id, itemNew.Id)
                && Equals(i.UserName, itemNew.UserName) && Equals(i.Title, itemNew.Title) &&
                Equals(i.About, itemNew.About) && Equals(i.Count, itemNew.Count) &&
                Equals(i.Directory, itemNew.Directory) && Equals(i.FirstId, itemNew.FirstId) &&
                Equals(i.IsAutoUpdate, itemNew.IsAutoUpdate)) 
            ?? itemNew;
    }

    public override TgSqlTableSourceModel GetFirst() => base.GetFirst() ?? CreateNew(true);

    #endregion
}