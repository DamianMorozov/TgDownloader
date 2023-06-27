// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgSettingsPage.xaml
/// </summary>
public partial class TgSettingsPage : TgPageBase
{
	public TgSettingsViewModel ViewModel { get; set; }

	public TgSettingsPage(TgSettingsViewModel viewModel)
	{
		ViewModel = viewModel;
		ViewModel.OnNavigatedTo();
		InitializeComponent();
	}
}