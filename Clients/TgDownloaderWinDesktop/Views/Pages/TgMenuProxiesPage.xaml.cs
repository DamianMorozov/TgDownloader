// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgAdvancedView.xaml
/// </summary>
public partial class TgMenuProxiesPage : INotifyPropertyChanged
{
	#region Public and private fields, properties, constructor

	public TgMenuProxiesViewModel ViewModel { get; set; }

	public TgMenuProxiesPage(TgMenuProxiesViewModel viewModel) : base(viewModel)
	{
		ViewModel = viewModel;
		InitializeComponent();
	}

	#endregion

	#region Public and private methods

	private void TgMenuProxiesPage_OnLoaded(object sender, RoutedEventArgs e)
	{
		ViewModel.Load();
	}

	private void ButtonSourceLoad_OnClick(object sender, RoutedEventArgs e)
	{
		if (sender is not Button button) return;
		if (button.Tag is not Guid proxyUid) return;

		ViewModel.IsLoad = true;

		TgSqlTableProxyModel proxy = ViewModel.Proxies.First(item => item.Uid.Equals(proxyUid));
		int index = ViewModel.Proxies.IndexOf(proxy);
		ViewModel.Proxies[index] = ViewModel.ContextManager.ContextTableProxies.GetItem(proxyUid);

		ViewModel.IsLoad = false;
	}

	private void ButtonSourceReloadAll_OnClick(object sender, RoutedEventArgs e)
	{
		ViewModel.IsLoad = true;

		for (int index = 0; index < ViewModel.Proxies.Count; index++)
		{
			ViewModel.Proxies[index] = 
				ViewModel.ContextManager.ContextTableProxies.GetItem(ViewModel.Proxies[index].Uid);
		}

		ViewModel.IsLoad = false;
	}

	#endregion
}