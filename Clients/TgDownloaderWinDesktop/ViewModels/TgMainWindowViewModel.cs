// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgMainWindowViewModel : TgPageViewModelBase
{
	public ObservableCollection<INavigationControl> NavigationItems { get; set; } = new();

	public ObservableCollection<INavigationControl> NavigationFooter { get; set; } = new();

	public ObservableCollection<MenuItem> TrayMenuItems { get; set; } = new();

	public TgMainWindowViewModel(INavigationService navigationService)
	{
		if (!IsInitialized)
			InitializeViewModel();
	}

	protected override void InitializeViewModel()
	{
		base.InitializeViewModel();

		//NavigationItem navigationItemAdvanced = new();
		//NavigationItem navigationItemAutoDownload = new()
		//{
		//	Content = TgLocaleHelper.Instance.MenuAutoDownload,
		//	PageTag = nameof(TgAdvancedPage),
		//	Icon = SymbolRegular.DataBarHorizontal24,
		//	PageType = typeof(TgAdvancedPage),
		//};
		//MenuItem menuItem1 = new() {Header = "Menu item 1"};
		//MenuItem menuItem2 = new() {Header = "Menu item 2"};
		//System.Windows.Controls.ContextMenu contextMenuAdvanced = new();
		//contextMenuAdvanced.Items.Add(navigationItemAutoDownload);
		//contextMenuAdvanced.Items.Add(navigationItemAutoViewEvents);
		//Label labelAdvanced = new() { Content = "Button 1", CommandBindings = { new CommandBinding() }};
		//navigationItemAdvanced.Content = contextMenuAdvanced;
		NavigationItems = new()
		{
				new NavigationItem
				{
					Content = "Home",
					PageTag =  "page_home",
					Icon = SymbolRegular.Home24,
					PageType = typeof(TgDashboardPage)
				},
				new NavigationItem
				{
					Content = TgLocaleHelper.Instance.MenuMainClient,
					PageTag =  "page_client",
					Icon = SymbolRegular.DataBarHorizontal24,
					PageType = typeof(TgClientPage),
				},
				new NavigationItem
				{
					Content = TgLocaleHelper.Instance.MenuMainProxies,
					PageTag = "page_proxies",
					Icon = SymbolRegular.DataBarHorizontal24,
					PageType = typeof(TgProxiesPage),
				},
				new NavigationItem
				{
					Content = TgLocaleHelper.Instance.TableSources,
					PageTag = "page_sources",
					Icon = SymbolRegular.DataBarHorizontal24,
					PageType = typeof(TgSourcesPage),
				},
			};

		NavigationFooter = new()
		{
				new NavigationItem
				{
					Content = "Settings",
				PageTag = nameof(TgSettingsPage),
					Icon = SymbolRegular.Settings24,
					PageType = typeof(TgSettingsPage)
				}
			};

		TrayMenuItems = new()
		{
				new()
				{
					Header = "Home",
					Tag = "page_home"
				},
				new()
				{
					Header = "Client",
					Tag = "page_client"
				},
				new()
				{
					Header = "Proxies",
					Tag = "page_proxies"
				},
				new()
				{
					Header = "Sources",
					Tag = "page_sources"
				},
			};

		IsInitialized = true;
	}
}
