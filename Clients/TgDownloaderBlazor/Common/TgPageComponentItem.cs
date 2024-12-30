// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgInfrastructure.Contracts;

namespace TgDownloaderBlazor.Common;

public abstract class TgPageComponentItem<TEntity> : TgPageComponentBase where TEntity : ITgDbEntity, new()
{
	#region Public and private fields, properties, constructor

    [Parameter] public string Uid { get; set; } = string.Empty;
	protected TEntity Item { get; set; } = new();
	protected string ItemSummaryString => Item.ToDebugString();

	#endregion

	#region Public and private methods

	//

	#endregion
}