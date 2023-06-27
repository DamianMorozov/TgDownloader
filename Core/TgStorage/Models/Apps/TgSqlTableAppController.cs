// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Apps;

[DebuggerDisplay("{ToString()}")]
[DoNotNotify]
public sealed class TgSqlTableAppController : TgSqlHelperBase<TgSqlTableAppModel>
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlTableAppController _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlTableAppController Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public override string TableName => TgSqlConstants.TableApps;

    #endregion

    #region Public and private methods

    public override TgSqlTableAppModel NewItem() => new() { PhoneNumber = "+00000000000" };

    public override TgSqlTableAppModel NewItem(Session session) => new(session) { PhoneNumber = "+00000000000" };

    public TgSqlTableAppModel GetItemByApiHash(Guid apiHash) =>
        new UnitOfWork()
            .Query<TgSqlTableAppModel>()
            .Select(item => item)
            .FirstOrDefault(item => Equals(item.ApiHash, apiHash)) ?? NewItem();

    public override TgSqlTableAppModel GetNewItem() => new UnitOfWork().Query<TgSqlTableAppModel>().Select(item => item)
        .FirstOrDefault(item => Equals(item.ApiHash, NewItem().ApiHash) && Equals(item.PhoneNumber, NewItem().PhoneNumber)) ?? NewItem();

    public override TgSqlTableAppModel GetCurrentItem()
	{
		lock (Locker)
		{
			return new UnitOfWork()
				.Query<TgSqlTableAppModel>()
				.Select(item => item)
				.FirstOrDefault(item => !Equals(item.ApiHash, Guid.Empty)) ?? NewItem();
		}
    }

    public Guid GetCurrentProxyUid => GetCurrentItem().ProxyUid;

    public TgSqlTableProxyModel GetCurrentProxy() => TgSqlTableProxyController.Instance.GetItem(GetCurrentProxyUid);

    public override bool AddItem(TgSqlTableAppModel item)
    {
        using UnitOfWork uow = new();
        TgSqlTableAppModel itemNew = new(uow)
        {
            ApiHash = item.ApiHash,
            ApiId = item.ApiId,
            PhoneNumber = item.PhoneNumber,
            ProxyUid = item.ProxyUid,
        };
        if (GetValidXpLite(itemNew).IsValid)
        {
            uow.CommitChangesAsync();
            return true;
        }
        return false;
    }

    public bool AddItem(TgSqlTableAppModel item, Guid proxyUid)
    {
        TgSqlTableProxyModel proxy = TgSqlTableProxyController.Instance.GetItem(proxyUid);
        if (!proxy.IsExists) throw new ArgumentException(nameof(proxy));
        using UnitOfWork uow = new();
        TgSqlTableAppModel itemNew = new(uow)
        {
            ApiHash = item.ApiHash,
            ApiId = item.ApiId,
            PhoneNumber = item.PhoneNumber,
            ProxyUid = proxyUid
        };
        if (GetValidXpLite(itemNew).IsValid)
        {
            uow.CommitChangesAsync();
            return true;
        }
        return false;
    }

    public override ValidationResult GetValidXpLite(TgSqlTableAppModel item) => 
	    new TgSqlTableAppValidator().Validate(item);

    public override bool UpdateItem(TgSqlTableAppModel itemSource, TgSqlTableAppModel itemDest)
    {
        itemDest.ApiHash = itemSource.ApiHash;
        itemDest.ApiId = itemSource.ApiId;
        itemDest.PhoneNumber = itemSource.PhoneNumber;
        itemDest.ProxyUid = itemSource.ProxyUid;
        if (GetValidXpLite(itemDest).IsValid)
        {
            itemDest.Session.SaveAsync(itemDest);
            itemDest.Session.CommitTransactionAsync();
            return true;
        }
        return false;
    }

    public bool UpdateItem(TgSqlTableAppModel itemSource, TgSqlTableAppModel itemDest, Guid proxyUid)
    {
        itemDest.ApiHash = itemSource.ApiHash;
        itemDest.ApiId = itemSource.ApiId;
        itemDest.PhoneNumber = itemSource.PhoneNumber;
        itemDest.ProxyUid = proxyUid;
        if (GetValidXpLite(itemDest).IsValid)
        {
            itemDest.Session.SaveAsync(itemDest);
            itemDest.Session.CommitTransactionAsync();
            return true;
        }
        return false;
    }

    public override bool AddOrUpdateItem(TgSqlTableAppModel item)
	{
		lock (Locker)
		{
			// Try find item.
			TgSqlTableAppModel itemDest = GetItemByApiHash(item.ApiHash);
	        // Add item.
	        if (!itemDest.IsExists)
	            return AddItem(item);
	        // Update item.
	        return UpdateItem(item, itemDest);
		}
    }

    public bool AddOrUpdateItem(TgSqlTableAppModel item, Guid proxyUid)
    {
        // Try find item.
        TgSqlTableAppModel itemDest = GetItemByApiHash(item.ApiHash);
        // Add item.
        if (!itemDest.IsExists)
            return AddItem(item, proxyUid);
        // Update item.
        return UpdateItem(item, itemDest, proxyUid);
    }

    public override bool DeleteItem(TgSqlTableAppModel item)
    {
        TgSqlTableAppModel itemDb = GetItemByApiHash(item.ApiHash);
        return base.DeleteItem(itemDb);
    }

    #endregion
}