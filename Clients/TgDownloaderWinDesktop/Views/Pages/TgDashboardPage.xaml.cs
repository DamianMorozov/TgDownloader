// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgDashboardPage.xaml
/// </summary>
public partial class TgDashboardPage : INotifyPropertyChanged
{
	public TgDashboardViewModel ViewModel { get; set; }

	public TgDashboardPage(TgDashboardViewModel viewModel) : base(viewModel)
	{
		ViewModel = viewModel;
		InitializeComponent();
	}

	private void ButtonSettingsReload_OnClick(object sender, RoutedEventArgs e)
	{
		_ = Task.Run(async () =>
		{
			await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
			ViewModel.IsReload = true;
		}).ConfigureAwait(true);

		ViewModel.AppSettings.LoadXmlSettings();

		_ = Task.Run(async () =>
		{
			await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
			ViewModel.IsReload = false;
		}).ConfigureAwait(true);
	}

	private void ButtonSettingsSave_OnClick(object sender, RoutedEventArgs e)
	{
		_ = Task.Run(async () =>
		{
			await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
			ViewModel.IsReload = true;
		}).ConfigureAwait(true);

		ViewModel.AppSettings.StoreXmlSettingsUnsafe();
		ViewModel.AppSettings.LoadXmlSettings();

		_ = Task.Run(async () =>
		{
			await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
			ViewModel.IsReload = false;
		}).ConfigureAwait(true);
	}

	private void ButtonSettingsDefault_OnClick(object sender, RoutedEventArgs e)
	{
		_ = Task.Run(async () =>
		{
			await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
			ViewModel.IsReload = true;
		}).ConfigureAwait(true);

		ViewModel.AppSettings.DefaultXmlSettings();

		_ = Task.Run(async () =>
		{
			await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
			ViewModel.IsReload = false;
		}).ConfigureAwait(true);
	}
}