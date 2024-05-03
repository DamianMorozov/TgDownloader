// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

/// <summary> EF operation result </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgEfOperResult<T> where T : TgEfEntityBase, ITgDbEntity, new()
{
	#region Public and private fields, properties, constructor

	public TgEnumEntityState State { get; set; }

	public T Item { get; set; }

	public IEnumerable<T> Items { get; set; }

	public bool IsExists => State is TgEnumEntityState.IsExists or TgEnumEntityState.IsSaved;

	public TgEfOperResult()
	{
		State = TgEnumEntityState.Unknown;
		Item = new();
		Items = [];
	}

	public TgEfOperResult(TgEnumEntityState state)
	{
		State = state;
		Item = new T();
		Items = [];
	}

	public TgEfOperResult(TgEnumEntityState state, T item)
	{
		State = state;
		Item = item;
		Items = [];
	}

	public TgEfOperResult(TgEnumEntityState state, IEnumerable<T> items)
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
	~TgEfOperResult() => Dispose(false);

	/// <summary> Throw exception if disposed </summary>
	private void CheckIfDisposed()
	{
		if (_disposed)
			throw new ObjectDisposedException($"{nameof(TgEfContext)}: {TgLocaleHelper.Instance.ObjectHasBeenDisposedOff}!");
	}

	/// <summary> Release managed resources </summary>
	private void ReleaseManagedResources()
	{
		//
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

	#region Public and private methods

	public string ToDebugString() => $"{State} | {Item.ToDebugString()} | {Items.Count()}";

	#endregion
}