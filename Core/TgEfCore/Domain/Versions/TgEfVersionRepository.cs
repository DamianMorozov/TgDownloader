// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Versions;

public sealed class TgEfVersionRepository(TgEfContext context) : TgEfRepositoryBase<TgEfVersionEntity>(context), ITgEfRepository<TgEfVersionEntity>
{
    #region Public and private methods

    public TgEfVersionEntity CreateNew()
    {
	    TgEfVersionEntity item = new()
	    {
		    Version = short.MaxValue,
		    Description = "New version",
	    };
	    CreateNew(item);
        return item;
    }

    public TgEfVersionEntity GetFirst() => Context.Versions.FirstOrDefault() ?? CreateNew();

    public async Task<TgEfVersionEntity> GetFirstAsync() => await Context.Versions.FirstOrDefaultAsync() ?? CreateNew();

	public IEnumerable<TgEfVersionEntity> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All) =>
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

    public IEnumerable<TgEfVersionEntity> GetEnumerable(int count) =>
        count > 0 ? Context.Versions.AsNoTracking().Select(x => x).Take(count) : Context.Versions.AsNoTracking().Select(x => x);

    public TgEfVersionEntity GetSingle(Guid uid) => Context.Versions.AsNoTracking().Single(x => x.Uid.ToString() == uid.ToString());

    public async Task<TgEfVersionEntity> GetSingleAsync(Guid uid) => 
        await Context.Versions.AsNoTracking().SingleAsync(x => x.Uid.ToString() == uid.ToString());

    public int GetCount() => Context.Versions.AsNoTracking().Count();

    public void DeleteAllItems() => Context.Versions.ExecuteDelete();

    public async Task DeleteAllItemsAsync() => await Context.Versions.ExecuteDeleteAsync();

	#endregion
}