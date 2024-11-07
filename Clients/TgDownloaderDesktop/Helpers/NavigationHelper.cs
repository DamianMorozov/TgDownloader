// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Helpers;

// Helper class to set the navigation target for a NavigationViewItem.
//
// Usage in XAML:
// <NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavigationHelper.NavigateTo="AppName.ViewModels.MainViewModel" />
//
// Usage in code:
// NavigationHelper.SetNavigateTo(navigationViewItem, typeof(MainViewModel).FullName);
public class NavigationHelper
{
	public static string GetNavigateTo(NavigationViewItem item) => (string)item.GetValue(NavigateToProperty);

	public static void SetNavigateTo(NavigationViewItem item, string value) => item.SetValue(NavigateToProperty, value);

	public static readonly DependencyProperty NavigateToProperty =
		DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavigationHelper), new PropertyMetadata(null));
}
