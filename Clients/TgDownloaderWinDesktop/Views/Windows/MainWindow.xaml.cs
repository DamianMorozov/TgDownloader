// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;

namespace TgDownloaderWinDesktop.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INavigationWindow
{
	public MainWindowViewModel ViewModel { get; init; }

	public MainWindow(MainWindowViewModel viewModel, IPageService pageService, INavigationService navigationService)
	{
		ViewModel = viewModel;
		DataContext = this;

		InitializeComponent();
		SetPageService(pageService);

		navigationService.SetNavigationControl(RootNavigation);
	}

	#region INavigationWindow methods

	public Frame GetFrame() => RootFrame;

	public INavigation GetNavigation() => RootNavigation;

	public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

	public void SetPageService(IPageService pageService) => RootNavigation.PageService = pageService;

	public void ShowWindow() => Show();

	public void CloseWindow() => Close();

	#endregion INavigationWindow methods

	/// <summary>
	/// Raises the closed event.
	/// </summary>
	protected override void OnClosed(EventArgs e)
	{
		base.OnClosed(e);
		// Make sure that closing this window will begin the process of closing the application.
		Application.Current.Shutdown();
	}
}