// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgMenuProxiesViewModel : TgViewBase, INavigationAware
{
	#region Public and private fields, properties, constructor

	public ObservableCollection<TgMvvmProxyModel> MvvmProxies { get; set; }

	public TgMenuProxiesViewModel()
	{
		MvvmProxies = new();
	}

	#endregion

	#region Public and private methods

	public void OnNavigatedTo()
	{
		if (!IsInitialized)
			InitializeViewModel();
	}

	public void OnNavigatedFrom()
	{
		//
	}

	private void InitializeViewModel()
	{
		IsInitialized = true;
	}

	/// <summary>
	/// Load sources from Storage.
	/// </summary>
	public void LoadProxiesFromStorage()
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			SetOrderProxies(ContextManager.ContextTableProxies.GetList());
		});
	}

	/// <summary>
	/// Clear proxies.
	/// </summary>
	public void ClearProxies() => Application.Current.Dispatcher.Invoke(MvvmProxies.Clear);

	/// <summary>
	/// Sort proxies.
	/// </summary>
	private void SetOrderProxies(List<TgSqlTableProxyModel> proxies)
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			proxies = proxies.OrderBy(x => x.Port).ToList().OrderBy(x => x.HostName).ToList();
			ClearProxies();
			foreach (TgSqlTableProxyModel proxy in proxies) 
				MvvmProxies.Add(new(proxy, ConnectProxy));
		});
	}

	/// <summary>
	/// Connect proxy.
	/// </summary>
	/// <param name="mvvmProxy"></param>
	public void ConnectProxy(TgMvvmProxyModel mvvmProxy)
	{
		// Checks.
		if (!CheckClientReady()) return;
	}

	#endregion

	#region Public and private methods - RelayCommand

	[RelayCommand]
	public async Task OnLoadProxiesFromStorageAsync()
	{
		IsLoad = true;
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		LoadProxiesFromStorage();
		IsLoad = false;
	}

	#endregion
}