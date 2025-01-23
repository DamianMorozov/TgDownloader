// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktopWPF.Views.Pages;

/// <summary>
/// Interaction logic for TgSettingsPage.xaml
/// </summary>
public partial class TgSettingsPage
{
	public TgSettingsPage()
	{
		TgDesktopUtils.TgSettingsVm.OnNavigatedTo();
		InitializeComponent();
	}
}