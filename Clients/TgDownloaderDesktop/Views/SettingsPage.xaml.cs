// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
	public SettingsViewModel ViewModel
	{
		get;
	}

	public SettingsPage()
	{
		ViewModel = App.GetService<SettingsViewModel>();
		InitializeComponent();
	}
}
