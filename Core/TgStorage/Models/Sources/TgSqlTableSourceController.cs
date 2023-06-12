// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Sources;

public sealed class TgSqlTableSourceController : TgSqlHelperBase<TgSqlTableSourceModel>
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlTableSourceController _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlTableSourceController Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public override string TableName => TgSqlConstants.TableSources;
    private readonly object _locker = new();

    #endregion

    #region Public and private methods

    public override TgSqlTableSourceModel NewItem() => new();

    public override TgSqlTableSourceModel NewItem(Session session) => new(session);

    public TgSqlTableSourceModel GetItem(long id) =>
        new UnitOfWork()
            .Query<TgSqlTableSourceModel>()
            .Select(item => item)
            .FirstOrDefault(item => Equals(item.Id, id)) ?? NewItem();

    public override TgSqlTableSourceModel GetNewItem() => new UnitOfWork().Query<TgSqlTableSourceModel>().Select(item => item)
        .FirstOrDefault(item => Equals(item.Id, NewItem().Id) && Equals(item.UserName, NewItem().UserName) &&
        Equals(item.Title, NewItem().Title) && Equals(item.About, NewItem().About) && Equals(item.Count, NewItem().Count) &&
        Equals(item.Directory, NewItem().Directory) && Equals(item.FirstId, NewItem().FirstId) && Equals(item.IsAutoUpdate, NewItem().IsAutoUpdate)) ?? NewItem();

    public override bool AddItem(TgSqlTableSourceModel item)
    {
        using UnitOfWork uow = new();
        TgSqlTableSourceModel itemNew = new(uow)
        {
            UserName = string.IsNullOrEmpty(item.UserName) ? "" : item.UserName,
            DtChanged = item.DtChanged > DateTime.MinValue ? item.DtChanged : DateTime.Now,
            Id = item.Id,
            Title = item.Title,
            About = item.About,
            Count = item.Count,
            Directory = item.Directory,
            FirstId = item.FirstId,
            IsAutoUpdate = item.IsAutoUpdate,
        };
		if (GetValidXpLite(itemNew).IsValid)
        {
            uow.CommitChanges();
            return true;
        }
        return false;
    }

    public override ValidationResult GetValidXpLite(TgSqlTableSourceModel item) => 
	    new TgSqlTableSourceValidator().Validate(item);

    public override bool UpdateItem(TgSqlTableSourceModel itemSource, TgSqlTableSourceModel itemDest)
    {
        itemDest.DtChanged = itemSource.DtChanged > DateTime.MinValue ? itemSource.DtChanged : DateTime.Now;
        itemDest.Id = itemSource.Id;
        if (itemSource.FirstId > itemDest.FirstId)
            itemDest.FirstId = itemSource.FirstId;
        if (itemSource.IsAutoUpdate)
            itemDest.IsAutoUpdate = itemSource.IsAutoUpdate;
        if (!string.IsNullOrEmpty(itemSource.UserName))
            itemDest.UserName = itemSource.UserName;
        if (!string.IsNullOrEmpty(itemSource.Title))
            itemDest.Title = itemSource.Title;
        if (!string.IsNullOrEmpty(itemSource.About))
            itemDest.About = itemSource.About;
        if (itemSource.Count > 0)
            itemDest.Count = itemSource.Count;
        if (!string.IsNullOrEmpty(itemSource.Directory))
            itemDest.Directory = itemSource.Directory;
        if (GetValidXpLite(itemDest).IsValid)
        {
            itemDest.Session.Save(itemDest);
            itemDest.Session.CommitTransaction();
            return true;
        }
        return false;
    }

    public override bool AddOrUpdateItem(TgSqlTableSourceModel item)
    {
	    lock (_locker)
	    {
		    // Try find item.
	        TgSqlTableSourceModel itemDest = GetItem(item.Id);
	        // Add item.
	        if (!itemDest.IsExists)
	            return AddItem(item);
	        // Update item.
	        return UpdateItem(item, itemDest);
	    }
    }

    public override bool DeleteItem(TgSqlTableSourceModel item)
    {
        TgSqlTableSourceModel itemDb = GetItem(item.Id);
        return base.DeleteItem(itemDb);
    }

    #endregion
}