// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Documents;

[DebuggerDisplay("{ToDebugString()}")]
[DoNotNotify]
public sealed class TgSqlTableDocumentRepository : TgSqlRepositoryBase<TgSqlTableDocumentModel>, 
    ITgSqlCreateRepository<TgSqlTableDocumentModel>, ITgSqlRepository<TgSqlTableDocumentModel>
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlTableDocumentRepository _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlTableDocumentRepository Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields, properties, constructor

    public string TableName => TgSqlConstants.TableDocuments;

    #endregion

    #region Public and private methods

    public TgSqlTableDocumentModel CreateNew(bool isCreateSession) => isCreateSession 
        ? new(TgSqlUtils.CreateUnitOfWork()) 
        : new();

    public override bool Delete(TgSqlTableDocumentModel item)
    {
        TgSqlTableDocumentModel itemFind = Get(item.SourceId, item.Id, item.MessageId);
        return itemFind.IsExists && base.Delete(itemFind);
    }

    public bool DeleteNew() => Delete(GetNew());

    public bool Save(TgSqlTableDocumentModel item, bool isGetByUid = false)
    {
        TgSqlTableDocumentModel itemFind = isGetByUid ? Get(item.Uid) : Get(item);
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

    public void Save(long id, long sourceId, long messageId, string fileName, long fileSize, long accessHash) =>
        Save(new(TgSqlUtils.CreateUnitOfWork())
        {
            Id = id,
            SourceId = sourceId,
            MessageId = messageId,
            FileName = fileName,
            FileSize = fileSize,
            AccessHash = accessHash
        });

    public override TgSqlTableDocumentModel Get(Guid uid) =>
        TgSqlUtils.CreateUnitOfWork().GetObjectByKey<TgSqlTableDocumentModel>(uid) ?? CreateNew(true);
    
    public override TgSqlTableDocumentModel Get(TgSqlTableDocumentModel item) =>
        Get(item.SourceId, item.Id, item.MessageId);

    public TgSqlTableDocumentModel Get(long sourceId, long id, long messageId) => TgSqlUtils.CreateUnitOfWork()
        .FindObject<TgSqlTableDocumentModel>(CriteriaOperator.Parse(
            $"{nameof(TgSqlTableDocumentModel.SourceId)}={sourceId} AND {nameof(TgSqlTableDocumentModel.Id)}={id} AND " +
            $"{nameof(TgSqlTableDocumentModel.MessageId)}={messageId}")) ?? CreateNew(true); 
    
    public TgSqlTableDocumentModel GetNew()
    {
        TgSqlTableDocumentModel itemNew = CreateNew(true);
        return new UnitOfWork()
            .Query<TgSqlTableDocumentModel>().Select(i => i)
            .FirstOrDefault(i => Equals(i.Id, itemNew.Id) && Equals(i.SourceId, itemNew.SourceId) && Equals(i.MessageId, itemNew.MessageId)) 
            ?? itemNew;
    }

    public override TgSqlTableDocumentModel GetFirst() => base.GetFirst() ?? CreateNew(true);

    #endregion
}