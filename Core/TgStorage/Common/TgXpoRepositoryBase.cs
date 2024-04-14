// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Data.Filtering;
using DevExpress.Xpo;

namespace TgStorage.Common;

/// <summary>
/// SQL table helper base.
/// </summary>
/// <typeparam name="T"></typeparam>
[DoNotNotify]
[DebuggerDisplay("{ToDebugString()}")]
public abstract class TgXpoRepositoryBase<T>(TgXpoContext xpoContext) : TgCommonBase where T : XPLiteObject, ITgDbEntity, new()
{
	#region Public and private fields, properties, constructor

	protected TgXpoContext XpoContext { get; } = xpoContext;

	#endregion

	#region Public and private methods - Read

	public async Task<TgXpoOperResult<T>> GetAsync(Guid uid, Session uow)
	{
		T? item = await uow.GetObjectByKeyAsync<T>(uid);
		if (item is null)
		{
			item = await uow
				.FindObjectAsync<T>(CriteriaOperator.Parse($"{nameof(ITgDbEntity.Uid)}='{uid:D}'"));
			if (item is not null)
				item.LetterCase = TgEnumLetterCase.LowerCase;
		}
		if (item is null)
		{
			item = await uow
				.FindObjectAsync<T>(CriteriaOperator.Parse($"{nameof(ITgDbEntity.Uid)}='{uid.ToString("D").ToUpper()}'"));
			if (item is not null)
				item.LetterCase = TgEnumLetterCase.UpperCase;
		}
		return item is not null
			? new TgXpoOperResult<T>(TgEnumEntityState.IsExist, item)
			: new TgXpoOperResult<T>(TgEnumEntityState.NotExist, uow);
	}

	public virtual Task<TgXpoOperResult<T>> GetNewAsync() => GetAsync(new T());

	public virtual async Task<TgXpoOperResult<T>> GetAsync(Guid uid)
	{
		using UnitOfWork uow = XpoContext.CreateUnitOfWork();
		return await GetAsync(uid, uow);
	}

	public virtual Task<TgXpoOperResult<T>> GetAsync(T item) => GetAsync(item.Uid);

	public Task<TgXpoOperResult<T>> GetEnumerableAsync(Expression<Func<T, bool>> predicate) =>
		GetEnumerableAsync(TgEnumTableTopRecords.All, predicate);

	public virtual async Task<TgXpoOperResult<T>> GetEnumerableAsync(TgEnumTableTopRecords topRecords = TgEnumTableTopRecords.All,
		Expression<Func<T, bool>>? predicate = null) =>
		topRecords switch
		{
			TgEnumTableTopRecords.Top1 => await GetEnumerableAsync(1, predicate),
			TgEnumTableTopRecords.Top20 => await GetEnumerableAsync(20, predicate),
			TgEnumTableTopRecords.Top100 => await GetEnumerableAsync(100, predicate),
			TgEnumTableTopRecords.Top1000 => await GetEnumerableAsync(1_000, predicate),
			TgEnumTableTopRecords.Top10000 => await GetEnumerableAsync(10_000, predicate),
			TgEnumTableTopRecords.Top100000 => await GetEnumerableAsync(100_000, predicate),
			TgEnumTableTopRecords.Top1000000 => await GetEnumerableAsync(1_000_000, predicate),
			_ => await GetEnumerableAsync(0, null),
		};

	private async Task<TgXpoOperResult<T>> GetEnumerableAsync(int count, Expression<Func<T, bool>>? predicate)
	{
		IEnumerable<T>? items;
		if (count > 0)
		{
			items = predicate is not null
				? await xpoContext.CreateUnitOfWork()
					.Query<T>()
					.Take(count)
					.Where(predicate)
					.ToListAsync()
				: await xpoContext.CreateUnitOfWork()
					.Query<T>()
					.Take(count)
					.ToListAsync();
		}
		else
		{
			items = predicate is not null
				? await xpoContext.CreateUnitOfWork()
					.Query<T>()
					.Where(predicate)
					.ToListAsync()
				: await xpoContext.CreateUnitOfWork()
					.Query<T>()
					.ToListAsync();
		}
		return items is not null && items.Any()
			? new TgXpoOperResult<T>(TgEnumEntityState.IsExist, items)
			: new TgXpoOperResult<T>(TgEnumEntityState.NotExist);
	}

	public virtual async Task<TgXpoOperResult<T>> GetFirstAsync()
	{
		T? item = await xpoContext.CreateUnitOfWork().Query<T>().FirstOrDefaultAsync();
		return item is not null ? new TgXpoOperResult<T>(TgEnumEntityState.IsExist, item)
			: await GetNewAsync();
	}

	#endregion

	#region Public and private methods - Write

	public virtual async Task<TgXpoOperResult<T>> CreateNewAsync()
	{
		TgXpoOperResult<T> operResult = await GetNewAsync();
		return operResult.IsExist
			? operResult
			: await XpoContext.TrySaveAsync(operResult.Item);
	}

	public virtual Task<TgXpoOperResult<T>> SaveAsync(T item) => XpoContext.TrySaveAsync(item);

	#endregion

	#region Public and private methods - Remove

	public virtual Task<TgXpoOperResult<T>> DeleteAsync(T item, bool isSkipFind) => XpoContext.TryDeleteAsync(item, isSkipFind);

	public virtual async Task<TgXpoOperResult<T>> DeleteNewAsync()
	{
		TgXpoOperResult<T> operResult = await GetNewAsync();
		return operResult.IsExist
			? await XpoContext.TryDeleteAsync(operResult.Item, true)
			: new TgXpoOperResult<T>(TgEnumEntityState.NotDeleted);
	}

	public virtual async Task<TgXpoOperResult<T>> DeleteAllAsync()
	{
		TgXpoOperResult<T> operResult = await GetEnumerableAsync();
		if (operResult.NotExist)
			return new TgXpoOperResult<T>(TgEnumEntityState.NotDeleted);

		TgEnumEntityState state = TgEnumEntityState.Unknown;
		foreach (T item in operResult.Items)
		{
			if ((await DeleteAsync(item, isSkipFind: false)).State == TgEnumEntityState.NotDeleted)
				state = TgEnumEntityState.NotDeleted;
		}
		return new TgXpoOperResult<T>(state);
	}

	#endregion
}