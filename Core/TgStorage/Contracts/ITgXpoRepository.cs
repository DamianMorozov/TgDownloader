//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//using DevExpress.Xpo;

//namespace TgStorage.Contracts;

//public interface ITgXpoRepository<T> where T : XPLiteObject, ITgDbEntity, new()
//{
//	#region Public and private methods - Read

//	public Task<TgXpoOperResult<T>> GetAsync(Guid uid);
//	public Task<TgXpoOperResult<T>> GetAsync(T item);
//	public Task<TgXpoOperResult<T>> GetNewAsync();
//	public Task<TgXpoOperResult<T>> GetFirstAsync();
//	public Task<TgXpoOperResult<T>> GetEnumerableAsync(Expression<Func<T, bool>> predicate);
//	public Task<TgXpoOperResult<T>> GetEnumerableAsync(TgEnumTableTopRecords topRecords = TgEnumTableTopRecords.All,
//		Expression<Func<T, bool>>? predicate = null);

//	#endregion

//	#region Public and private methods - Write

//	public Task<TgXpoOperResult<T>> CreateNewAsync();
//	public Task<TgXpoOperResult<T>> SaveAsync(T item);

//	#endregion

//	#region Public and private methods - Remove

//	public Task<TgXpoOperResult<T>> DeleteAsync(T item, bool isSkipFind);
//	public Task<TgXpoOperResult<T>> DeleteNewAsync();
//	public Task<TgXpoOperResult<T>> DeleteAllAsync();

//	#endregion
//}