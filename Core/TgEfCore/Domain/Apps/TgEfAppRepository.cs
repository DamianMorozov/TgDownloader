// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Apps;

public sealed class TgEfAppRepository(TgEfContext context) : TgEfRepositoryBase<TgEfAppEntity>(context), ITgEfRepository<TgEfAppEntity>
{
    #region Public and private methods


    public TgEfAppEntity CreateNew()
    {
	    TgEfAppEntity item = new()
	    {
		    PhoneNumber = "+00000000000"
	    };
	    CreateNew(item);
	    return item;
    }

    public IEnumerable<TgEfAppEntity> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All) =>
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

    public IEnumerable<TgEfAppEntity> GetEnumerable(int count)
    {
	    IEnumerable<TgEfAppEntity> result = count > 0
		    ? Context.Apps.AsNoTracking().Select(x => x).Take(count)
		    : Context.Apps.AsNoTracking().Select(x => x);
	    foreach (TgEfAppEntity item in result)
	    {
		    Context.Entry(item).Reference(x => x.Proxy).Load();
	    }
	    return result;
    }

    public TgEfAppEntity GetSingle(Guid uid) => Context.Apps.AsNoTracking().Single(x => x.Uid.ToString() == uid.ToString());

    public async Task<TgEfAppEntity> GetSingleAsync(Guid uid) => 
        await Context.Apps.AsNoTracking().SingleAsync(x => x.Uid.ToString() == uid.ToString());

    public int GetCount() => Context.Apps.AsNoTracking().Count();

    #endregion
}