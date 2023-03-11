// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgCore.Enums;
using TgStorage.Models.Filters;

namespace TgStorage.Helpers;

public partial class TgStorageHelper
{
	#region Public and private methods

	public SqlTableFilterModel NewEmptyFilter() =>
		new() { Uid = Guid.Empty, FilterType = FilterType.None, Name = string.Empty, Mask = string.Empty };

	private void AddFilter<T>(T item, UnitOfWork uow) where T : ISqlTable, new()
	{
		if (item is not SqlTableFilterModel filter) return;
		filter = new(uow)
		{
			IsEnabled = filter.IsEnabled,
			DtCreated = DateTime.Now,
			DtChanged = DateTime.Now,
			FilterType = filter.FilterType,
			Name = filter.Name,
			Mask = string.IsNullOrEmpty(filter.Mask) && (Equals(filter.FilterType, FilterType.MinSize) || Equals(filter.FilterType, FilterType.MaxSize)) ? "*" : filter.Mask,
			Size = filter.Size,
			SizeType = filter.SizeType,
		};
		if (IsValidXpLite(filter))
			uow.CommitChanges();
	}

	public SqlTableFilterModel GetDefaultFilter()
	{
		SqlTableFilterModel filter = NewEmptyFilter();
		filter.IsEnabled = true;
		filter.FilterType = FilterType.SingleName;
		filter.Name = "Any";
		filter.Mask = "*";
		filter.SizeType = FileSizeType.Bytes;
		return filter;
	}

	public void AddDefaultFilter() => AddItem(GetDefaultFilter());

	private void UpdateFilter<T>(T item) where T : ISqlTable, new()
	{
		if (item is not SqlTableFilterModel filter) return;
		SqlTableFilterModel? itemDb = GetItemNullable<SqlTableFilterModel>(item.Uid);
		if (itemDb is not { }) return;
		itemDb.IsEnabled = filter.IsEnabled;
		itemDb.DtCreated = filter.DtCreated;
		itemDb.DtChanged = DateTime.Now;
		itemDb.FilterType = filter.FilterType;
		itemDb.Name = filter.Name;
		itemDb.Mask = filter.Mask;
		itemDb.Size = filter.Size;
		itemDb.SizeType = filter.SizeType;
		if (IsValidXpLite(itemDb))
		{
			itemDb.Session.Save(itemDb);
			itemDb.Session.CommitTransaction();
		}
	}

	public List<SqlTableFilterModel> GetFiltersList() =>
		new UnitOfWork()
			.Query<SqlTableFilterModel>()
			.Select(item => item)
			.ToList();

	public List<SqlTableFilterModel> GetFiltersEnabledList() =>
		new UnitOfWork()
			.Query<SqlTableFilterModel>()
			.Select(item => item)
			.Where(item => item.IsEnabled)
			.ToList();

	public void DeleteFilter<T>(T item) where T : ISqlTable, new()
	{
		if (item is not SqlTableFilterModel filter) return;
		SqlTableFilterModel? itemDb = GetItemNullable<SqlTableFilterModel>(item.Uid);
		if (itemDb is not { }) return;
		itemDb.Session.Delete(itemDb);
		itemDb.Session.CommitTransaction();
	}

	public void DeleteDefaultFilter()
	{
		SqlTableFilterModel defaultFilter = GetDefaultFilter();
		List<SqlTableFilterModel> filters = GetFiltersList();
		SqlTableFilterModel? filter = filters.Find(item => Equals(item.Name, defaultFilter.Name));
		if (filter is not null && filter.IsExists)
			DeleteFilter(filter);
	}

	public void DeleteAllFilters()
	{
		List<SqlTableFilterModel> filters = GetFiltersList();
		foreach (SqlTableFilterModel filter in filters)
		{
			if (filter is not null && filter.IsExists)
				DeleteFilter(filter);
		}
	}

	#endregion
}