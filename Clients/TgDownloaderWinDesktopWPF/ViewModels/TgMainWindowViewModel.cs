// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktopWPF.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgMainWindowViewModel : TgPageViewModelBase
{
	public ObservableCollection<INavigationControl> NavigationItems { get; set; } = [];

	public ObservableCollection<INavigationControl> NavigationFooter { get; set; } = [];

	public ObservableCollection<MenuItem> TrayMenuItems { get; set; } = [];

	public TgMainWindowViewModel(INavigationService navigationService) : base()
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
        NavigationItems =
		[
			new NavigationItem
            {
                Content = "Home",
                PageTag =  "page_home",
                Icon = SymbolRegular.Home24,
                PageType = typeof(TgDashboardPage)
            },
            new NavigationItem
            {
                Content = TgLocaleHelper.Instance.MenuMainConnection,
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
                Content = TgLocaleHelper.Instance.MenuMainSources,
                PageTag = "page_sources",
                Icon = SymbolRegular.DataBarHorizontal24,
                PageType = typeof(TgSourcesPage),
            },
            //new NavigationItem
            //{
            //    Content = TgLocaleHelper.Instance.MenuMainDownloads,
            //    PageTag = "page_downloads",
            //    Icon = SymbolRegular.DataBarHorizontal24,
            //    PageType = typeof(TgDownloadsPage),
            //},
        ];

        NavigationFooter =
		[
			new NavigationItem
            {
                Content = TgLocaleHelper.Instance.MenuMainSettings,
                PageTag = "page_settings",
                Icon = SymbolRegular.Settings24,
                PageType = typeof(TgSettingsPage)
            }
        ];

        TrayMenuItems =
		[
			new () { Header = "Home", Tag = "page_home" },
            new() { Header = "Client", Tag = "page_client" },
            new() { Header = "Proxies", Tag = "page_proxies" },
            new() { Header = "Sources", Tag = "page_sources" },
            new() { Header = "Downloads", Tag = "page_downloads" },
            new() { Header = "Settings", Tag = "page_settings" },
        ];

        IsInitialized = true;
	}
}
