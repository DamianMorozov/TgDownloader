// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Common;

/// <summary> EF storage result </summary>
[DebuggerDisplay("{ToDebugString()}")]
public sealed class TgEfStorageResult<TEntity> where TEntity : ITgDbFillEntity<TEntity>, new()
{
	#region Public and private fields, properties, constructor

	public TgEnumEntityState State { get; set; }

	public TEntity Item { get; set; }

	public IEnumerable<TEntity> Items { get; set; }

	public bool IsExists => State is TgEnumEntityState.IsExists or TgEnumEntityState.IsSaved;

	public TgEfStorageResult()
	{
		State = TgEnumEntityState.Unknown;
		Item = new();
		Items = [];
	}

	public TgEfStorageResult(TgEnumEntityState state)
	{
		State = state;
		Item = new();
		Items = [];
	}

	public TgEfStorageResult(TgEnumEntityState state, TEntity? item)
	{
		State = state;
		Item = item ?? new();
		Items = [];
	}

	public TgEfStorageResult(TgEnumEntityState state, IEnumerable<TEntity>? items)
	{
		State = state;
		Item = new();
		Items = items ?? new List<TEntity>();
	}

	#endregion

	#region Public and private methods

	public string ToDebugString() => $"{State} | {Item.Uid} | {Items.Count()}";

	#endregion
}