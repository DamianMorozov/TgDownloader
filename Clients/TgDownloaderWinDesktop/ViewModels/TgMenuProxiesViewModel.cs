// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgMenuProxiesViewModel : TgBaseViewModel
{
	#region Public and private fields, properties, constructor

	public ObservableCollection<TgSqlTableProxyModel> Proxies { get; set; }

	public TgMenuProxiesViewModel()
	{
		Proxies = new();
	}

	#endregion

	#region Public and private methods

	public void Load()
	{
		Proxies.Clear();
		foreach (TgSqlTableProxyModel proxy in ContextManager.ContextTableProxies.GetList())
		{
			Proxies.Add(proxy);
		}
	}

	#endregion
}