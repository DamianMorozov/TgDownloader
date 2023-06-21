// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Filters;

public sealed class TgSqlTableFilterController : TgSqlHelperBase<TgSqlTableFilterModel>
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlTableFilterController _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlTableFilterController Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public override string TableName => TgSqlConstants.TableFilters;

    #endregion

    #region Public and private methods

    public override TgSqlTableFilterModel NewItem() =>
        new() { IsEnabled = true, FilterType = TgEnumFilterType.SingleName, Name = "Any", Mask = "*", SizeType = TgEnumFileSizeType.Bytes };

    public override TgSqlTableFilterModel NewItem(Session session) =>
        new(session) { IsEnabled = true, FilterType = TgEnumFilterType.SingleName, Name = "Any", Mask = "*", SizeType = TgEnumFileSizeType.Bytes };

    public TgSqlTableFilterModel GetItem(TgEnumFilterType filterType, string name) =>
        new UnitOfWork()
            .Query<TgSqlTableFilterModel>()
            .Select(item => item)
            .FirstOrDefault(item => Equals(item.FilterType, filterType) && Equals(item.Name, name)) ?? NewItem();

    public override TgSqlTableFilterModel GetNewItem() => new UnitOfWork().Query<TgSqlTableFilterModel>().Select(item => item)
        .FirstOrDefault(item => Equals(item.IsEnabled, NewItem().IsEnabled) && Equals(item.FilterType, NewItem().FilterType) &&
            Equals(item.Name, NewItem().Name) && Equals(item.Mask, NewItem().Mask) && Equals(item.SizeType, NewItem().SizeType)) ?? NewItem();

    public override bool AddItem(TgSqlTableFilterModel item)
    {
        using UnitOfWork uow = new();
        TgSqlTableFilterModel itemNew = new(uow)
        {
            IsEnabled = item.IsEnabled,
            FilterType = item.FilterType,
            Name = item.Name,
            Mask = string.IsNullOrEmpty(item.Mask) && (Equals(item.FilterType, TgEnumFilterType.MinSize) || Equals(item.FilterType, TgEnumFilterType.MaxSize)) ? "*" : item.Mask,
            Size = item.Size,
            SizeType = item.SizeType,
        };
        if (GetValidXpLite(itemNew).IsValid)
        {
            uow.CommitChanges();
            return true;
        }
        return false;
    }

    public override ValidationResult GetValidXpLite(TgSqlTableFilterModel item) => 
	    new TgSqlTableFilterValidator().Validate(item);

    public override bool UpdateItem(TgSqlTableFilterModel itemSource, TgSqlTableFilterModel itemDest)
    {
        itemDest.IsEnabled = itemSource.IsEnabled;
        itemDest.FilterType = itemSource.FilterType;
        itemDest.Name = itemSource.Name;
        itemDest.Mask = itemSource.Mask;
        itemDest.Size = itemSource.Size;
        itemDest.SizeType = itemSource.SizeType;
        if (GetValidXpLite(itemDest).IsValid)
        {
            itemDest.Session.Save(itemDest);
            itemDest.Session.CommitTransaction();
            return true;
        }
        return false;
    }

    public override bool AddOrUpdateItem(TgSqlTableFilterModel item)
	{
		lock (Locker)
		{
			// Try find item.
			TgSqlTableFilterModel itemDest = GetItem(item.FilterType, item.Name);
	        // Add item.
	        if (!itemDest.IsExists)
	            return AddItem(item);
	        // Update item.
	        return UpdateItem(item, itemDest);
		}
    }

    public override bool DeleteItem(TgSqlTableFilterModel item)
    {
        TgSqlTableFilterModel itemDb = GetItem(item.FilterType, item.Name);
        return base.DeleteItem(itemDb);
    }

    public List<TgSqlTableFilterModel> GetListEnabled() =>
        new UnitOfWork()
            .Query<TgSqlTableFilterModel>()
            .Select(item => item)
            .Where(item => item.IsEnabled)
            .ToList();

    public bool DeleteDefaultItem()
    {
        TgSqlTableFilterModel defaultFilter = NewItem();
        List<TgSqlTableFilterModel> filters = GetList();
        TgSqlTableFilterModel? item = filters.Find(item => Equals(item.Name, defaultFilter.Name));
        if (item is not null && item.IsExists)
            return DeleteItem(item);
        return false;
    }

    #endregion
}