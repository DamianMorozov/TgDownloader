// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Common;

public abstract class TgPageComponentEnumerable<TEntity> : TgPageComponentBase where TEntity : TgEfEntityBase, new()
{
	#region Public and private fields, properties, constructor

    protected RadzenDataGrid<TEntity> Grid { get; set; } = new();
	protected string SearchString { get; set; } = string.Empty;
	protected IEnumerable<TEntity> Items { get; set; } = new List<TEntity>();
	protected int ItemsCount { get; set; }
    protected string ItemsSummaryString => $"{TgLocale.FoundRows}: {ItemsCount}";

	#endregion

	#region Public and private methods

    //

    #endregion
}