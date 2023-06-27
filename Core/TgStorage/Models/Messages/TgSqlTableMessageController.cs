// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Messages;

[DebuggerDisplay("{ToString()}")]
[DoNotNotify]
public sealed class TgSqlTableMessageController : TgSqlHelperBase<TgSqlTableMessageModel>
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlTableMessageController _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlTableMessageController Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public override string TableName => TgSqlConstants.TableMessages;

    #endregion

    #region Public and private methods

    public override TgSqlTableMessageModel NewItem() => new()
    { Id = 0, SourceId = 0, DtCreated = DateTime.Now, Type = TgEnumMessageType.Message, Size = 0, Message = string.Empty };

    public override TgSqlTableMessageModel NewItem(Session session) => new(session)
    { Id = 0, SourceId = 0, DtCreated = DateTime.Now, Type = TgEnumMessageType.Message, Size = 0, Message = string.Empty };

    public override TgSqlTableMessageModel GetNewItem() => new UnitOfWork().Query<TgSqlTableMessageModel>().Select(item => item)
        .FirstOrDefault(item => Equals(item.Id, NewItem().Id) && Equals(item.SourceId, NewItem().SourceId) &&
            Equals(item.Type, NewItem().Type) && Equals(item.Size, NewItem().Size) && Equals(item.Message, NewItem().Message)) ?? NewItem();

    public override bool AddItem(TgSqlTableMessageModel item)
    {
        using UnitOfWork uow = new();
        TgSqlTableMessageModel itemNew = new(uow)
        {
            Id = item.Id,
            SourceId = item.SourceId,
            DtCreated = item.DtCreated,
            Type = item.Type,
            Size = item.Size,
            Message = item.Message,
        };
        if (GetValidXpLite(itemNew).IsValid)
        {
            uow.CommitChangesAsync();
            return true;
        }
        return false;
    }

    public override ValidationResult GetValidXpLite(TgSqlTableMessageModel item) => 
	    new TgSqlTableMessageValidator().Validate(item);

    public override bool UpdateItem(TgSqlTableMessageModel itemSource, TgSqlTableMessageModel itemDest)
    {
        itemDest.Id = itemSource.Id;
        itemDest.SourceId = itemSource.SourceId;
        itemDest.DtCreated = itemSource.DtCreated;
        itemDest.Type = itemSource.Type;
        itemDest.Size = itemSource.Size;
        itemDest.Message = itemSource.Message;
        if (GetValidXpLite(itemDest).IsValid)
        {
            itemDest.Session.SaveAsync(itemDest);
            itemDest.Session.CommitTransactionAsync();
            return true;
        }
        return false;
    }

    public bool FindExistsMessage(long id, long sourceId) => GetItem(sourceId, id).IsExists;

	public TgSqlTableMessageModel GetItem(long sourceId, long id)
	{
		lock (Locker)
		{
			return new UnitOfWork()
				.Query<TgSqlTableMessageModel>()
				.Select(item => item)
				.FirstOrDefault(item => Equals(item.SourceId, sourceId) && Equals(item.Id, id)) ?? NewItem();
		}
	}

	public override bool AddOrUpdateItem(TgSqlTableMessageModel item)
    {
	    lock (Locker)
	    {
		    // Try find item.
		    TgSqlTableMessageModel itemDest = GetItem(item.SourceId, item.Id);
		    // Add item.
		    if (!itemDest.IsExists)
			    return AddItem(item);
		    // Update item.
		    return UpdateItem(item, itemDest);
	    }
    }

    public override bool DeleteItem(TgSqlTableMessageModel item)
    {
        TgSqlTableMessageModel itemDb = GetItem(item.SourceId, item.Id);
        return base.DeleteItem(itemDb);
    }

    public void StoreMessage(int id, long sourceId, DateTime dtCreate, TgEnumMessageType type, long size, string message)
    {
	    AddOrUpdateItem(new()
	    {
		    Id = id,
		    SourceId = sourceId,
		    DtCreated = dtCreate,
		    Type = type,
		    Size = size,
		    Message = message
	    });
	    TgSqlTableSourceModel source = TgSqlTableSourceController.Instance.GetItem(sourceId);
	    source.FirstId = id;
	    TgSqlTableSourceController.Instance.AddOrUpdateItem(source);
    }

	#endregion
}