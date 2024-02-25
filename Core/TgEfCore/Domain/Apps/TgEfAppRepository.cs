// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Apps;

[Table(TgSqlConstants.TableApps)]
public sealed class TgEfAppRepository(TgEfContext context) : TgEfRepositoryBase(context),
    ITgSqlCreateRepository<TgEfAppEntity>, ITgEfRepository<TgEfAppEntity>
{
    #region Public and private methods


    public TgEfAppEntity CreateNew(bool isCreateSession)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(TgEfAppEntity item)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteNewAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveAsync(TgEfAppEntity item, bool isGetByUid = false)
    {
        throw new NotImplementedException();
    }

    public Task<TgEfAppEntity> GetAsync(TgEfAppEntity item)
    {
        throw new NotImplementedException();
    }

    public Task<TgEfAppEntity> GetNewAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TgEfAppEntity> GetFirstAsync()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TgEfAppEntity> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All) =>
        topRecords switch
        {
            TgSqlEnumTableTopRecords.Top200 => GetEnumerable(200),
            TgSqlEnumTableTopRecords.Top1000 => GetEnumerable(1_000),
            TgSqlEnumTableTopRecords.Top10000 => GetEnumerable(10_000),
            TgSqlEnumTableTopRecords.Top100000 => GetEnumerable(100_000),
            TgSqlEnumTableTopRecords.Top1000000 => GetEnumerable(1_000_000),
            _ => GetEnumerable(0),
        };

    public IEnumerable<TgEfAppEntity> GetEnumerable(int count) =>
        count > 0 ? Context.Apps.AsNoTracking().Select(x => x).Take(count) : Context.Apps.AsNoTracking().Select(x => x);

    public TgEfAppEntity GetSingle(Guid uid) => Context.Apps.AsNoTracking().Single(x => x.Uid.ToString() == uid.ToString());

    public async Task<TgEfAppEntity> GetSingleAsync(Guid uid) => await Context.Apps.AsNoTracking().SingleAsync(x => x.Uid.ToString() == uid.ToString());

    #endregion
}