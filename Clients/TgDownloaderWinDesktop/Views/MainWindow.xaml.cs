// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public sealed partial class MainWindow : INavigationWindow
{
    #region Public and private fields, properties, constructor

    public TgMainWindowViewModel ViewModel { get; init; }

    public MainWindow(TgMainWindowViewModel viewModel, IPageService pageService, INavigationService navigationService)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
        SetPageService(pageService);
        navigationService.SetNavigationControl(RootNavigation);
    }

    #endregion

    #region Public and private methods

    public Frame GetFrame() => RootFrame;

    public INavigation GetNavigation() => RootNavigation;

    public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

    public void SetPageService(IPageService pageService) => RootNavigation.PageService = pageService;

    public void ShowWindow() => Show();

    public void CloseWindow() => Close();

    /// <summary>
    /// Raises the closed event.
    /// </summary>
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        // Clear tray.
        TgTitleBar.Tray = null;
        // Make sure that closing this window will begin the process of closing the application.
        Application.Current.Shutdown();
    }

    #endregion
}