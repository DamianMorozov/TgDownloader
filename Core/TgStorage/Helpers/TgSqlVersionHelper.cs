// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Helpers;

public sealed class TgSqlVersionHelper : TgSqlHelperBase<TgSqlTableVersionModel>
{
	#region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	private static TgSqlVersionHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public static TgSqlVersionHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

	#endregion

	#region Public and private fields, properties, constructor

	public override string TableName => TgSqlConstants.TableVersions;

	#endregion

	#region Public and private methods

	public override TgSqlTableVersionModel NewItem() => new() { Version = short.MaxValue, Description = "New version" };

	public override TgSqlTableVersionModel NewItem(Session session) => new(session) { Version = short.MaxValue, Description = "New version" };

	public TgSqlTableVersionModel GetItem(short version) =>
		new UnitOfWork()
			.Query<TgSqlTableVersionModel>()
			.Select(item => item)
			.OrderBy(item => item.Version)
			.FirstOrDefault(item => Equals(item.Version, version)) ?? NewItem();

	public override TgSqlTableVersionModel GetItemLast() =>
		new UnitOfWork()
			.Query<TgSqlTableVersionModel>()
			.Select(item => item)
			.OrderByDescending(item => item.Version)
			.FirstOrDefault() ?? NewItem();

	public override TgSqlTableVersionModel GetNewItem() => new UnitOfWork().Query<TgSqlTableVersionModel>().Select(item => item)
		.FirstOrDefault(item => Equals(item.Version, NewItem().Version) && Equals(item.Description, NewItem().Description)) ?? NewItem();

	public short VersionLast => 18;

	public override bool AddItem(TgSqlTableVersionModel item)
	{
		using UnitOfWork uow = new();
		TgSqlTableVersionModel itemNew = new(uow)
		{
			Version = item.Version,
			Description = item.Description,
		};
		if (IsValidXpLite(itemNew))
		{
			uow.CommitChanges();
			return true;
		}
		return false;
	}

	public override bool IsValidXpLite(TgSqlTableVersionModel item) => new TgSqlTableVersionValidator().Validate(item).IsValid;

	public override bool UpdateItem(TgSqlTableVersionModel itemSource, TgSqlTableVersionModel itemDest)
	{
		itemDest.Version = itemSource.Version;
		itemDest.Description = itemSource.Description;
		if (IsValidXpLite(itemDest))
		{
			itemDest.Session.Save(itemDest);
			itemDest.Session.CommitTransaction();
			return true;
		}
		return false;
	}

	public override bool AddOrUpdateItem(TgSqlTableVersionModel item)
	{
		// Try find item.
		TgSqlTableVersionModel itemDest = GetItem(item.Version);
		// Add item.
		if (!itemDest.IsExists)
			return AddItem(item);
		// Update item.
		return UpdateItem(item, itemDest);
	}

	public override bool DeleteItem(TgSqlTableVersionModel item)
	{
		TgSqlTableVersionModel itemDb = GetItem(item.Version);
		return base.DeleteItem(itemDb);
	}

	#endregion
}