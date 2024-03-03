// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Proxies;

public sealed class TgEfProxyRepository(TgEfContext context) : TgEfRepositoryBase<TgEfProxyEntity>(context), ITgEfRepository<TgEfProxyEntity>
{
    #region Public and private methods

    public TgEfProxyEntity CreateNew()
    {
	    TgEfProxyEntity item = new()
	    {
		    Type = TgEnumProxyType.None,
		    HostName = "No proxy",
		    Port = 404,
		    UserName = "No user",
		    Password = "No password",
		    Secret = string.Empty
	    };
	    CreateNew(item);
	    return item;
    }

    public IEnumerable<TgEfProxyEntity> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All) =>
        topRecords switch
        {
	        TgSqlEnumTableTopRecords.Top20 => GetEnumerable(20),
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

    public async Task<TgEfProxyEntity> GetSingleAsync(Guid uid) => 
        await Context.Proxies.AsNoTracking().SingleAsync(x => x.Uid.ToString() == uid.ToString());

    public int GetCount() => Context.Proxies.AsNoTracking().Count();

    #endregion
}