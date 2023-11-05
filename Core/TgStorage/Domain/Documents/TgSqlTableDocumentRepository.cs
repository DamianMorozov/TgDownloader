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

    #region Public and private methods

    public TgSqlTableDocumentModel CreateNew(bool isCreateSession) => isCreateSession 
        ? new(TgSqlUtils.CreateUnitOfWork()) 
        : new();

    public override async Task<bool> DeleteAsync(TgSqlTableDocumentModel item)
    {
        TgSqlTableDocumentModel itemFind = await GetAsync(item.SourceId, item.Id, item.MessageId);
        return itemFind.IsExists && await base.DeleteAsync(itemFind);
    }

    public async Task<bool> DeleteNewAsync() => await DeleteAsync(await GetNewAsync());

    public async Task<bool> SaveAsync(TgSqlTableDocumentModel item, bool isGetByUid = false)
    {
        TgSqlTableDocumentModel itemFind = isGetByUid ? await GetAsync(item.Uid) : await GetAsync(item);
        if (itemFind.IsNotExists)
        {
            itemFind.Fill(item);
            ValidationResult validationResult = TgSqlUtils.GetValidXpLite(itemFind);
            return validationResult.IsValid && await TgSqlUtils.TryInsertAsync(itemFind);
        }
        else
        {
            itemFind.Fill(item, itemFind.Uid);
            ValidationResult validationResult = TgSqlUtils.GetValidXpLite(itemFind);
            return validationResult.IsValid && await TgSqlUtils.TryUpdateAsync(itemFind);
        }
    }

    public async Task SaveAsync(long id, long sourceId, long messageId, string fileName, long fileSize, long accessHash) =>
        await SaveAsync(new(TgSqlUtils.CreateUnitOfWork())
        {
            Id = id,
            SourceId = sourceId,
            MessageId = messageId,
            FileName = fileName,
            FileSize = fileSize,
            AccessHash = accessHash
        });

    public override async Task<TgSqlTableDocumentModel> GetAsync(Guid uid) =>
        await TgSqlUtils.CreateUnitOfWork()
            .GetObjectByKeyAsync<TgSqlTableDocumentModel>(uid) ?? CreateNew(true);
    
    public override async Task<TgSqlTableDocumentModel> GetAsync(TgSqlTableDocumentModel item) =>
        await GetAsync(item.SourceId, item.Id, item.MessageId);

    public async Task<TgSqlTableDocumentModel> GetAsync(long sourceId, long id, long messageId) =>
        await TgSqlUtils.CreateUnitOfWork()
        .FindObjectAsync<TgSqlTableDocumentModel>(CriteriaOperator.Parse(
            $"{nameof(TgSqlTableDocumentModel.SourceId)}={sourceId} AND {nameof(TgSqlTableDocumentModel.Id)}={id} AND " +
            $"{nameof(TgSqlTableDocumentModel.MessageId)}={messageId}")) 
        ?? CreateNew(true);

    public async Task<TgSqlTableDocumentModel> GetNewAsync()
    {
        TgSqlTableDocumentModel itemNew = CreateNew(true);
        return await new UnitOfWork()
                   .Query<TgSqlTableDocumentModel>().Select(i => i)
                   .FirstOrDefaultAsync(i => Equals(i.Id, itemNew.Id) && Equals(i.SourceId, itemNew.SourceId) && Equals(i.MessageId, itemNew.MessageId))
               ?? itemNew;
    }

    public override async Task<TgSqlTableDocumentModel> GetFirstAsync() =>
        await base.GetFirstAsync() ?? CreateNew(true);

    #endregion
}