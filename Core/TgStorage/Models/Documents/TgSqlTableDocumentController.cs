// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Models.Documents;

public sealed class TgSqlTableDocumentController : TgSqlHelperBase<TgSqlTableDocumentModel>
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlTableDocumentController _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlTableDocumentController Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public override string TableName => TgSqlConstants.TableDocuments;

    #endregion

    #region Public and private methods

    public override TgSqlTableDocumentModel NewItem() => new();

    public override TgSqlTableDocumentModel NewItem(Session session) => new(session);

    public TgSqlTableDocumentModel GetItem(long sourceId, long id, long messageId) =>
        new UnitOfWork()
            .Query<TgSqlTableDocumentModel>()
            .Select(item => item)
            .FirstOrDefault(item => Equals(item.SourceId, sourceId) && Equals(item.Id, id) && Equals(item.MessageId, messageId)) ?? NewItem();

    public override TgSqlTableDocumentModel GetNewItem() => new UnitOfWork().Query<TgSqlTableDocumentModel>().Select(item => item)
        .FirstOrDefault(item => Equals(item.Id, NewItem().Id) && Equals(item.SourceId, NewItem().SourceId) && Equals(item.MessageId, NewItem().MessageId)) ?? NewItem();

    public override bool AddItem(TgSqlTableDocumentModel item)
    {
        using UnitOfWork uow = new();
        TgSqlTableDocumentModel itemNew = new(uow)
        {
            SourceId = item.SourceId,
            Id = item.Id,
            MessageId = item.MessageId,
            FileName = item.FileName,
            FileSize = item.FileSize,
            AccessHash = item.AccessHash,
        };
        if (GetValidXpLite(itemNew).IsValid)
        {
            uow.CommitChanges();
            return true;
        }
        return false;
    }

    public override ValidationResult GetValidXpLite(TgSqlTableDocumentModel item) => 
	    new TgSqlTableDocumentValidator().Validate(item);

    public override bool UpdateItem(TgSqlTableDocumentModel itemSource, TgSqlTableDocumentModel itemDest)
    {
        itemDest.SourceId = itemSource.SourceId;
        itemDest.Id = itemSource.Id;
        itemDest.MessageId = itemSource.MessageId;
        itemDest.FileName = itemSource.FileName;
        itemDest.FileSize = itemSource.FileSize;
        itemDest.AccessHash = itemSource.AccessHash;
        if (GetValidXpLite(itemDest).IsValid)
        {
            itemDest.Session.Save(itemDest);
            itemDest.Session.CommitTransaction();
            return true;
        }
        return false;
    }

    public override bool AddOrUpdateItem(TgSqlTableDocumentModel item)
    {
        // Try find item.
        TgSqlTableDocumentModel itemDest = GetItem(item.SourceId, item.Id, item.MessageId);
        // Add item.
        if (!itemDest.IsExists)
            return AddItem(item);
        // Update item.
        return UpdateItem(item, itemDest);
    }

    public override bool DeleteItem(TgSqlTableDocumentModel item)
    {
        TgSqlTableDocumentModel itemDb = GetItem(item.SourceId, item.Id, item.MessageId);
        return base.DeleteItem(itemDb);
    }

    #endregion
}