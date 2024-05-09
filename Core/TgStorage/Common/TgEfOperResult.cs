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

	#region Public and private methods

	public string ToDebugString() => $"{State} | {Item.ToDebugString()} | {Items.Count()}";

	#endregion
}