// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Documents;

public sealed class TgEfDocumentRepository(TgEfContext context) : TgEfRepositoryBase<TgEfDocumentEntity>(context), ITgEfRepository<TgEfDocumentEntity>
{
    #region Public and private methods


    public TgEfDocumentEntity CreateNew(bool isSave)
    {
	    TgEfDocumentEntity item = new();
        if (isSave)
			CreateNew(item);
	    return item;
    }

    public TgEfDocumentEntity GetFirst() => Context.Documents.FirstOrDefault() ?? CreateNew(false);

    public async Task<TgEfDocumentEntity> GetFirstAsync() => await Context.Documents.FirstOrDefaultAsync() ?? CreateNew(false);
    
    public IEnumerable<TgEfDocumentEntity> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All) =>
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

    public IEnumerable<TgEfDocumentEntity> GetEnumerable(int count)
    {
	    IEnumerable<TgEfDocumentEntity> result = count > 0
		    ? Context.Documents.AsNoTracking().Select(x => x).Take(count)
		    : Context.Documents.AsNoTracking().Select(x => x);
	    return result;
    }

    public TgEfDocumentEntity GetSingle(Guid uid) => Context.Documents.AsNoTracking().Single(x => x.Uid.ToString() == uid.ToString());

    public async Task<TgEfDocumentEntity> GetSingleAsync(Guid uid) => 
        await Context.Documents.AsNoTracking().SingleAsync(x => x.Uid.ToString() == uid.ToString());

    public int GetCount() => Context.Documents.AsNoTracking().Count();

    public void DeleteAllItems() => Context.Documents.ExecuteDelete();

    public async Task DeleteAllItemsAsync() => await Context.Documents.ExecuteDeleteAsync();

	#endregion
}