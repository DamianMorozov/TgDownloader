// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Proxies;

[Table(TgSqlConstants.TableApps)]
public sealed class TgEfProxyRepository(TgEfContext context) : TgEfRepositoryBase(context),
    ITgSqlCreateRepository<TgEfProxyEntity>, ITgEfRepository<TgEfProxyEntity>
{
    #region Public and private methods

    public TgEfProxyEntity CreateNew(bool isCreateSession)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(TgEfProxyEntity item)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteNewAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveAsync(TgEfProxyEntity item, bool isGetByUid = false)
    {
        throw new NotImplementedException();
    }

    public Task<TgEfProxyEntity> GetAsync(TgEfProxyEntity item)
    {
        throw new NotImplementedException();
    }

    public Task<TgEfProxyEntity> GetNewAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TgEfProxyEntity> GetFirstAsync()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TgEfProxyEntity> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All) =>
        topRecords switch
        {
            TgSqlEnumTableTopRecords.Top200 => GetEnumerable(200),
            TgSqlEnumTableTopRecords.Top1000 => GetEnumerable(1_000),
            TgSqlEnumTableTopRecords.Top10000 => GetEnumerable(10_000),
            TgSqlEnumTableTopRecords.Top100000 => GetEnumerable(100_000),
            TgSqlEnumTableTopRecords.Top1000000 => GetEnumerable(1_000_000),
            _ => GetEnumerable(0),
        };

    public IEnumerable<TgEfProxyEntity> GetEnumerable(int count) =>
        count > 0 ? Context.Proxies.AsNoTracking().Select(x => x).Take(count) : Context.Proxies.AsNoTracking().Select(x => x);

    public TgEfProxyEntity GetSingle(Guid uid) => Context.Proxies.AsNoTracking().Single(x => x.Uid.ToString() == uid.ToString());

    public async Task<TgEfProxyEntity> GetSingleAsync(Guid uid) => await Context.Proxies.AsNoTracking().SingleAsync(x => x.Uid.ToString() == uid.ToString());

    #endregion
}