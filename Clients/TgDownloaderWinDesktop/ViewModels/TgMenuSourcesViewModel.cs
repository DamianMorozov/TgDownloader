// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Collections.Generic;

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgMenuSourcesViewModel : TgBaseViewModel
{
	#region Public and private fields, properties, constructor

	public ObservableCollection<TgSqlTableSourceModel> Sources { get; set; }

	public TgMenuSourcesViewModel()
	{
		Sources = new();
	}

	#endregion

	#region Public and private methods

	public void Load()
	{
		Sources.Clear();
		List<TgSqlTableSourceModel> list = new();
		foreach (TgSqlTableSourceModel source in ContextManager.ContextTableSources.GetList())
		{
			list.Add(source);
		}
		list = list.OrderBy(x => x.Title).ToList().OrderBy(x => x.UserName).ToList();
		foreach (TgSqlTableSourceModel source in list)
		{
			Sources.Add(source);
		}
	}

	#endregion
}