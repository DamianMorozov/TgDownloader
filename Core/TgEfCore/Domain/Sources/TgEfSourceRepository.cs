// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Sources;

public sealed class TgEfSourceRepository(TgEfContext context) : TgEfRepositoryBase<TgEfSourceEntity>(context), ITgEfRepository<TgEfSourceEntity>
{
    #region Public and private methods

    public TgEfSourceEntity CreateNew()
    {
	    TgEfSourceEntity item = new()
	    {
		    Id = 1,
		    Count = 1,
		    About = "Test",
		    Directory = TgFileUtils.GetDefaultDirectory(),
		    DtChanged = DateTime.Now,
		    FirstId = 1,
		    IsAutoUpdate = false,
		    Title = "Test",
		    UserName = "Test",
	    };
	    CreateNew(item);
        return item;
    }

	public IEnumerable<TgEfSourceEntity> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All) =>
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

    public IEnumerable<TgEfSourceEntity> GetEnumerable(int count) =>
        count > 0 ? Context.Sources.AsNoTracking().Select(x => x).Take(count) : Context.Sources.AsNoTracking().Select(x => x);

    public TgEfSourceEntity GetSingle(Guid uid) => Context.Sources.AsNoTracking().Single(x => x.Uid.ToString() == uid.ToString());

    public async Task<TgEfSourceEntity> GetSingleAsync(Guid uid) => 
        await Context.Sources.AsNoTracking().SingleAsync(x => x.Uid.ToString() == uid.ToString());

    public int GetCount() => Context.Sources.AsNoTracking().Count();

    #endregion
}