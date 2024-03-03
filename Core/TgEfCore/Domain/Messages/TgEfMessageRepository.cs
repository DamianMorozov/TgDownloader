// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Messages;

public sealed class TgEfMessageRepository(TgEfContext context) : TgEfRepositoryBase<TgEfMessageEntity>(context), ITgEfRepository<TgEfMessageEntity>
{
    #region Public and private methods


    public TgEfMessageEntity CreateNew()
    {
	    TgEfMessageEntity item = new()
	    {
		    Id = 0,
		    SourceId = 0,
		    DtCreated = DateTime.Now,
		    Type = TgEnumMessageType.Message,
		    Size = 0,
		    Message = string.Empty
	    };
	    CreateNew(item);
	    return item;
    }

    public IEnumerable<TgEfMessageEntity> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All) =>
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

    public IEnumerable<TgEfMessageEntity> GetEnumerable(int count)
    {
	    IEnumerable<TgEfMessageEntity> result = count > 0
		    ? Context.Messages.AsNoTracking().Select(x => x).Take(count)
		    : Context.Messages.AsNoTracking().Select(x => x);
	    return result;
    }

    public TgEfMessageEntity GetSingle(Guid uid) => Context.Messages.AsNoTracking().Single(x => x.Uid.ToString() == uid.ToString());

    public async Task<TgEfMessageEntity> GetSingleAsync(Guid uid) => 
        await Context.Messages.AsNoTracking().SingleAsync(x => x.Uid.ToString() == uid.ToString());

    public int GetCount() => Context.Messages.AsNoTracking().Count();

    #endregion
}