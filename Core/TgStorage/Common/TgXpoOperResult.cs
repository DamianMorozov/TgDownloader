// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using DevExpress.Xpo;

namespace TgStorage.Common;

/// <summary>
/// XPO operation result.
/// </summary>
public class TgXpoOperResult<T> where T : XPLiteObject, ITgDbEntity, new()
{
	#region Public and private fields, properties, constructor

	public TgEnumEntityState State { get; set; }

	public T Item { get; set; }

	public IEnumerable<T> Items { get; set; }

	public bool IsExist => State is TgEnumEntityState.IsExist or TgEnumEntityState.IsSaved;

	public bool NotExist => !IsExist || State == TgEnumEntityState.IsDeleted;

	public TgXpoOperResult()
	{
		State = TgEnumEntityState.Unknown;
		Item = new();
		Items = [];
	}

	public TgXpoOperResult(TgEnumEntityState state)
	{
		State = state;
		Item = new T();
		Items = [];
	}

	public TgXpoOperResult(TgEnumEntityState state, Session session)
	{
		State = state;
		Item = (T)Activator.CreateInstance(typeof(T), session);
		Items = [];
	}

	public TgXpoOperResult(TgEnumEntityState state, T item)
	{
		State = state;
		Item = item;
		Items = [];
	}

	public TgXpoOperResult(TgEnumEntityState state, IEnumerable<T> items)
	{
		State = state;
		Item = new();
		Items = items;
	}

	#endregion

	#region Public and private methods - IDisposable

	/// <summary> Locker object </summary>
	private readonly object _locker = new();
	/// <summary> To detect redundant calls </summary>
	private bool _disposed;

	/// <summary> Finalizer </summary>
	~TgXpoOperResult() => Dispose(false);

	/// <summary> Throw exception if disposed </summary>
	private void CheckIfDisposed()
	{
		if (_disposed)
			throw new ObjectDisposedException($"{nameof(TgEfContext)}: object has been disposed off!");
	}

	/// <summary> Release managed resources </summary>
	private void ReleaseManagedResources()
	{
		if (Item.Session is not null)
		{
			if (Item.Session.IsConnected)
				Item.Session.Disconnect();
			Item.Session.Dispose();
		}
		if (Items.Any())
		{
			foreach (T item in Items)
			{
				if (item.Session.IsConnected)
					item.Session.Disconnect();
				item.Session.Dispose();
			}
		}
	}

	/// <summary> Release unmanaged resources </summary>
	private void ReleaseUnmanagedResources()
	{
		//
	}

	/// <summary> Dispose pattern </summary>
	public void Dispose()
	{
		Dispose(true);
		// Suppress finalization.
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (_disposed)
			return;
		lock (_locker)
		{
			// Release managed resources.
			if (disposing)
				ReleaseManagedResources();
			// Release unmanaged resources.
			ReleaseUnmanagedResources();
			// Flag.
			_disposed = true;
		}
	}

	/// <summary> Dispose async pattern | await using </summary>
	public ValueTask DisposeAsync()
	{
		// Release unmanaged resources.
		Dispose(false);
		// Suppress finalization.
		GC.SuppressFinalize(this);
		// Result.
		return ValueTask.CompletedTask;
	}

	#endregion
}