// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Filters;

public sealed class TgEfFilterRepository(TgEfContext context) : TgEfRepositoryBase<TgEfFilterEntity>(context), ITgEfRepository<TgEfFilterEntity>
{
    #region Public and private methods

    public TgEfFilterEntity CreateNew(bool isSave)
    {
	    TgEfFilterEntity item = new()
	    {
		    IsEnabled = true,
		    FilterType = TgEnumFilterType.SingleName,
		    Name = "Any",
		    Mask = "*",
		    SizeType = TgEnumFileSizeType.Bytes
	    };
        if (isSave)
			CreateNew(item);
	    return item;
    }

    public TgEfFilterEntity GetFirst() => Context.Filters.FirstOrDefault() ?? CreateNew(false);

    public async Task<TgEfFilterEntity> GetFirstAsync() => await Context.Filters.FirstOrDefaultAsync() ?? CreateNew(false);

    public IEnumerable<TgEfFilterEntity> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All) =>
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

    public IEnumerable<TgEfFilterEntity> GetEnumerable(int count) =>
        count > 0 ? Context.Filters.AsNoTracking().Select(x => x).Take(count) : Context.Filters.AsNoTracking().Select(x => x);

    public TgEfFilterEntity GetSingle(Guid uid) => Context.Filters.AsNoTracking().Single(x => x.Uid.ToString() == uid.ToString());

    public async Task<TgEfFilterEntity> GetSingleAsync(Guid uid) => await Context.Filters.AsNoTracking().SingleAsync(x => x.Uid.ToString() == uid.ToString());

    public int GetCount() => Context.Filters.AsNoTracking().Count();

	public void DeleteAllItems() => Context.Filters.ExecuteDelete();

	public async Task DeleteAllItemsAsync() => await Context.Filters.ExecuteDeleteAsync();

	#endregion
}