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

    #region Public and private fields, properties, constructor

    public string TableName => TgSqlConstants.TableMessages;

    #endregion

    #region Public and private methods

    public TgSqlTableMessageModel CreateNew(bool isCreateSession) => isCreateSession 
        ? new(TgSqlUtils.CreateUnitOfWork())
            { Id = 0, SourceId = 0, DtCreated = DateTime.Now, Type = TgEnumMessageType.Message, Size = 0, Message = string.Empty }
        : new()
            { Id = 0, SourceId = 0, DtCreated = DateTime.Now, Type = TgEnumMessageType.Message, Size = 0, Message = string.Empty };

    public override bool Delete(TgSqlTableMessageModel item)
    {
        TgSqlTableMessageModel itemFind = Get(item.SourceId, item.Id);
        return itemFind.IsExists && base.Delete(itemFind);
    }

    public bool DeleteNew() => Delete(GetNew());
    
    public bool Save(TgSqlTableMessageModel item)
    {
        TgSqlTableMessageModel itemFind = Get(item);
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

    public void Save(int id, long sourceId, DateTime dtCreate, TgEnumMessageType type,
        long size, string message) => Save(new()
    {
        Id = id,
        SourceId = sourceId,
        DtCreated = dtCreate,
        Type = type,
        Size = size,
        Message = message
    });

    public override TgSqlTableMessageModel Get(TgSqlTableMessageModel item) => 
        Get(item.SourceId, item.Id);

    public TgSqlTableMessageModel Get(long sourceId, long id) => TgSqlUtils.CreateUnitOfWork()
        .FindObject<TgSqlTableMessageModel>(CriteriaOperator.Parse(
            $"{nameof(TgSqlTableMessageModel.SourceId)}={sourceId} AND " +
            $"{nameof(TgSqlTableMessageModel.Id)}={id}")) ?? CreateNew(true);

    public bool GetExists(long id, long sourceId) => Get(sourceId, id).IsExists;

    public TgSqlTableMessageModel GetNew()
    {
        TgSqlTableMessageModel itemNew = CreateNew(true);
        return new UnitOfWork()
            .Query<TgSqlTableMessageModel>().Select(i => i)
            .FirstOrDefault(i => Equals(i.Id, itemNew.Id) && Equals(i.SourceId, itemNew.SourceId) && Equals(i.Type, itemNew.Type) &&
                Equals(i.Size, itemNew.Size) && Equals(i.Message, itemNew.Message)) 
            ?? itemNew;
    }

    public override TgSqlTableMessageModel GetFirst() => base.GetFirst() ?? CreateNew(true);

    #endregion
}