// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class TgSettingsPage : Page
{
	#region Public and private fields, properties, constructor

	public TgSettingsViewModel ViewModel { get; }

	public TgSettingsPage()
	{
		ViewModel = App.GetService<TgSettingsViewModel>();
		InitializeComponent();
		Loaded += OnLoaded;
	}

	#endregion

	#region Public and private methods

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		ViewModel.OnLoaded(XamlRoot);
		ComboBoxAppThemes.SelectionChanged += Selector_OnSelectionChanged;
	}

	private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		await ViewModel.SwitchThemeAsync();
	}

	#endregion
}
