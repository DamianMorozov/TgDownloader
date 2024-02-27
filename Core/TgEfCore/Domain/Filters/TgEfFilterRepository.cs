// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Domain.Filters;

[Table(TgSqlConstants.TableApps)]
public sealed class TgEfFilterRepository(TgEfContext context) : TgEfRepositoryBase(context),
    ITgEfRepository<TgEfFilterEntity>
{
    #region Public and private methods

    public TgEfFilterEntity CreateNew()
    {
        using IDbContextTransaction transaction = Context.Database.BeginTransaction();
        try
        {
            TgEfFilterEntity item = new()
            {
                IsEnabled = true,
                FilterType = TgEnumFilterType.SingleName,
                Name = "Any",
                Mask = "*",
                SizeType = TgEnumFileSizeType.Bytes
            };
            Context.Add(item);
            Context.SaveChanges();
            transaction.Commit();
            return item;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public Task<bool> DeleteAsync(TgEfFilterEntity item)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteNewAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveAsync(TgEfFilterEntity item, bool isGetByUid = false)
    {
        throw new NotImplementedException();
    }

    public Task<TgEfFilterEntity> GetAsync(TgEfFilterEntity item)
    {
        throw new NotImplementedException();
    }

    public Task<TgEfFilterEntity> GetNewAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TgEfFilterEntity> GetFirstAsync()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TgEfFilterEntity> GetEnumerable(TgSqlEnumTableTopRecords topRecords = TgSqlEnumTableTopRecords.All) =>
        topRecords switch
        {
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

    #endregion
}