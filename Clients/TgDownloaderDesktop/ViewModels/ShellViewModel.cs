// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
	[ObservableProperty]
	private bool _isBackEnabled;
	[ObservableProperty]
	private object? _selected;
	public INavigationService NavigationService { get; }
	public INavigationViewService NavigationViewService { get; }

	public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
	{
		NavigationService = navigationService;
		NavigationService.Navigated += OnNavigated;
		NavigationViewService = navigationViewService;
	}

	private void OnNavigated(object sender, NavigationEventArgs e)
	{
		IsBackEnabled = NavigationService.CanGoBack;
		if (e.SourcePageType == typeof(TgSettingsPage))
		{
			Selected = NavigationViewService.SettingsItem;
			return;
		}
		var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
		if (selectedItem != null)
		{
			Selected = selectedItem;
		}
	}
}
