// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgMenuDashboardPage.xaml
/// </summary>
public partial class TgMenuDashboardPage : INotifyPropertyChanged
{
	#region Public and private fields, properties, constructor

	public TgMenuDashboardViewModel ViewModel { get; set; }

	public TgMenuDashboardPage(TgMenuDashboardViewModel viewModel) : base(viewModel)
	{
		ViewModel = viewModel;
		InitializeComponent();
	}

	#endregion

	#region Public and private methods

	private void ButtonSettingsSave_OnClick(object sender, RoutedEventArgs e)
	{
		ViewModel.TgAppSettings.StoreXmlSettingsUnsafe();
		ViewModel.TgAppSettings.LoadXmlSettings();
	}

	private void ButtonSettingsDefault_OnClick(object sender, RoutedEventArgs e)
	{
		ViewModel.TgAppSettings.DefaultXmlSettings();
	}

	#endregion
}