// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Helpers;

public sealed class TgSqlProxyHelper : TgSqlHelperBase<TgSqlTableProxyModel>
{
	#region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	private static TgSqlProxyHelper _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public static TgSqlProxyHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

	#endregion

	#region Public and private fields, properties, constructor

	public override string TableName => TgSqlConstants.TableProxies;

	#endregion

	#region Public and private methods

	public override TgSqlTableProxyModel NewItem() =>
		new() { Type = TgProxyType.None, HostName = "No proxy", Port = 404, UserName = "No user", Password = "No password",  Secret = string.Empty };

	public override TgSqlTableProxyModel NewItem(Session session) => new(session)
		{ Type = TgProxyType.None, HostName = "No proxy", Port = 404, UserName = "No user", Password = "No password",  Secret = string.Empty };

	public TgSqlTableProxyModel GetItem(TgProxyType proxyType, string hostName, ushort port) =>
		new UnitOfWork()
			.Query<TgSqlTableProxyModel>()
			.Select(item => item)
			.FirstOrDefault(item => Equals(item.Type, proxyType) && Equals(item.HostName, hostName) && Equals(item.Port, port)) ?? NewItem();

	public override TgSqlTableProxyModel GetNewItem() => new UnitOfWork().Query<TgSqlTableProxyModel>().Select(item => item)
		.FirstOrDefault(item => Equals(item.Type, NewItem().Type) && Equals(item.HostName, NewItem().HostName) &&
				Equals(item.Port, NewItem().Port) && Equals(item.UserName, NewItem().UserName) && Equals(item.Password, NewItem().Password)) ?? NewItem();

	public override bool AddItem(TgSqlTableProxyModel item)
	{
		using UnitOfWork uow = new();
		TgSqlTableProxyModel itemNew = new(uow)
		{
			Type = item.Type,
			HostName = item.HostName,
			Port = item.Port,
			UserName = item.UserName,
			Password = item.Password,
			Secret = item.Secret,
		};
		if (IsValidXpLite(itemNew))
		{
			uow.CommitChanges();
			return true;
		}
		return false;
	}

	public override bool IsValidXpLite(TgSqlTableProxyModel item) => new TgSqlTableProxyValidator().Validate(item).IsValid;

	public override bool UpdateItem(TgSqlTableProxyModel itemSource, TgSqlTableProxyModel itemDest)
	{
		itemDest.Type = itemSource.Type;
		itemDest.HostName = itemSource.HostName;
		itemDest.Port = itemSource.Port;
		itemDest.UserName = itemSource.UserName;
		itemDest.Password = itemSource.Password;
		itemDest.Secret = itemSource.Secret;
		if (IsValidXpLite(itemDest))
		{
			itemDest.Session.Save(itemDest);
			itemDest.Session.CommitTransaction();
			return true;
		}
		return false;
	}

	public override bool AddOrUpdateItem(TgSqlTableProxyModel item)
	{
		// Try find item.
		TgSqlTableProxyModel itemDest = GetItem(item.Type, item.HostName, item.Port);
		// Add item.
		if (!itemDest.IsExists)
			return AddItem(item);
		// Update item.
		return UpdateItem(item, itemDest);
	}

	public override bool DeleteItem(TgSqlTableProxyModel item)
	{
		TgSqlTableProxyModel itemDb = GetItem(item.Type, item.HostName, item.Port);
		return base.DeleteItem(itemDb);
	}

	#endregion
}