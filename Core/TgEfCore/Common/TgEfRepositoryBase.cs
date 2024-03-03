// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgEfCore.Common;

[Table(TgSqlConstants.TableApps)]
public abstract class TgEfRepositoryBase<TEntity>(TgEfContext context) : TgCommonBase where TEntity : ITgSqlTable, new()
{
    #region Public and private fields, properties, constructor

    protected TgEfContext Context { get; } = context;

	#endregion

	#region Public and private methods

	public TEntity CreateNew(TEntity item)
	{
		using IDbContextTransaction transaction = Context.Database.BeginTransaction();
		try
		{
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

	#endregion
}