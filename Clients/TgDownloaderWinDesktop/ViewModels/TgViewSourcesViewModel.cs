// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgViewSourcesViewModel : TgBaseViewModel
{
	#region Public and private fields, properties, constructor

	public ObservableCollection<TgSqlTableSourceModel> Sources { get; set; }

	public TgViewSourcesViewModel()
	{
		Sources = new();
	}

	#endregion

	#region Public and private methods

	public void ReloadSources()
	{
		Sources.Clear();
		foreach (TgSqlTableSourceModel source in ContextManager.ContextTableSources.GetList())
		{
			Sources.Add(source);
		}
	}

	#endregion
}