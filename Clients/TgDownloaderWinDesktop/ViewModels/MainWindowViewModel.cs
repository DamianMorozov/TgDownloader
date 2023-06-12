// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class MainWindowViewModel : TgBaseViewModel
{
	private bool _isInitialized = false;

	public string ApplicationTitle { get; set; } = string.Empty;

	public ObservableCollection<INavigationControl> NavigationItems { get; set; } = new();

	public ObservableCollection<INavigationControl> NavigationFooter { get; set; } = new();

	public ObservableCollection<MenuItem> TrayMenuItems { get; set; } = new();

	public MainWindowViewModel(INavigationService navigationService)
	{
		if (!_isInitialized)
			InitializeViewModel();
	}

	private void InitializeViewModel()
	{
		ApplicationTitle = "Tg-Downloader-WinDesktop";

		//NavigationItem navigationItemAdvanced = new();
		//NavigationItem navigationItemAutoDownload = new()
		//{
		//	Content = TgLocaleHelper.Instance.MenuAutoDownload,
		//	PageTag = nameof(TgAdvancedPage),
		//	Icon = SymbolRegular.DataBarHorizontal24,
		//	PageType = typeof(TgAdvancedPage),
		//};
		//NavigationItem navigationItemAutoViewEvents = new()
		//{
		//	Content = TgLocaleHelper.Instance.MenuAutoViewEvents,
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
					PageTag = nameof(TgDashboardPage),
					Icon = SymbolRegular.Home24,
					PageType = typeof(TgDashboardPage)
				},
				new NavigationItem
				{
					Content = TgLocaleHelper.Instance.TableSources,
					PageTag = nameof(TgViewSourcesPage),
					Icon = SymbolRegular.DataBarHorizontal24,
					PageType = typeof(TgViewSourcesPage),
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
					Tag = "tray_home"
				},
				new()
				{
					Header = "Dashboard",
					Tag = "tray_dashboard"
				},
			};

		_isInitialized = true;
	}
}
