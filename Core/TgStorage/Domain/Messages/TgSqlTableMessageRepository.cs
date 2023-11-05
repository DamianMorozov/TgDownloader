// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Domain.Messages;

[DebuggerDisplay("{ToDebugString()}")]
[DoNotNotify]
public sealed class TgSqlTableMessageRepository : TgSqlRepositoryBase<TgSqlTableMessageModel>,
    ITgSqlCreateRepository<TgSqlTableMessageModel>, ITgSqlRepository<TgSqlTableMessageModel>
{
    #region Design pattern "Lazy Singleton"

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static TgSqlTableMessageRepository _instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TgSqlTableMessageRepository Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private methods

    public TgSqlTableMessageModel CreateNew(bool isCreateSession)
    {
        TgSqlTableMessageModel message = isCreateSession
            ? new(TgSqlUtils.CreateUnitOfWork()) : new();
        message.Id = 0;
        message.SourceId = 0;
        message.DtCreated = DateTime.Now;
        message.Type = TgEnumMessageType.Message;
        message.Size = 0;
        message.Message = string.Empty;

        return message;
    }

    public override async Task<bool> DeleteAsync(TgSqlTableMessageModel item)
    {
        TgSqlTableMessageModel itemFind = await GetAsync(item.SourceId, item.Id);
        return itemFind.IsExists && await base.DeleteAsync(itemFind);
    }

    public async Task<bool> DeleteNewAsync() => await DeleteAsync(await GetNewAsync());
    
    public async Task<bool> SaveAsync(TgSqlTableMessageModel item, bool isGetByUid = false)
    {
        TgSqlTableMessageModel itemFind = isGetByUid ? await GetAsync(item.Uid) : await GetAsync(item);
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

    public async Task SaveAsync(int id, long sourceId, DateTime dtCreate, TgEnumMessageType type,
        long size, string message) => await SaveAsync(new()
    {
        Id = id,
        SourceId = sourceId,
        DtCreated = dtCreate,
        Type = type,
        Size = size,
        Message = message
    });

    public override async Task<TgSqlTableMessageModel> GetAsync(Guid uid) =>
        TgSqlUtils.CreateUnitOfWork().GetObjectByKey<TgSqlTableMessageModel>(uid) ?? CreateNew(true);

    public override async Task<TgSqlTableMessageModel> GetAsync(TgSqlTableMessageModel item) =>
        await GetAsync(item.SourceId, item.Id);

    public async Task<TgSqlTableMessageModel> GetAsync(long sourceId, long id) =>
        await TgSqlUtils.CreateUnitOfWork()
        .FindObjectAsync<TgSqlTableMessageModel>(CriteriaOperator.Parse(
            $"{nameof(TgSqlTableMessageModel.SourceId)}={sourceId} AND " +
            $"{nameof(TgSqlTableMessageModel.Id)}={id}")) 
        ?? CreateNew(true);

    public async Task<bool> GetExistsAsync(long id, long sourceId) =>
        (await GetAsync(sourceId, id)).IsExists;

    public async Task<TgSqlTableMessageModel> GetNewAsync()
    {
        TgSqlTableMessageModel itemNew = CreateNew(true);
        return await new UnitOfWork()
                   .Query<TgSqlTableMessageModel>().Select(i => i)
                   .FirstOrDefaultAsync(i => Equals(i.Id, itemNew.Id) && Equals(i.SourceId, itemNew.SourceId) && Equals(i.Type, itemNew.Type) &&
                                             Equals(i.Size, itemNew.Size) && Equals(i.Message, itemNew.Message))
               ?? itemNew;
    }

    public override async Task<TgSqlTableMessageModel> GetFirstAsync() 
        => await base.GetFirstAsync() ?? CreateNew(true);

    #endregion
}