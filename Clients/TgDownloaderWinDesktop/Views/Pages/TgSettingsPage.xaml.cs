// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgSettingsPage.xaml
/// </summary>
public partial class TgSettingsPage : INotifyPropertyChanged
{
	public TgSettingsViewModel ViewModel { get; set; }

	public TgSettingsPage(TgSettingsViewModel viewModel) : base(viewModel)
	{
		ViewModel = viewModel;
		InitializeComponent();

		//radioButtonLight.SetBinding(Label.IsVisibleProperty,
		//	new Binding(
		//		nameof(ViewModel.SelectedPlatform),
		//		converter: new EnumToBooleanConverter(),
		//		converterParameter: MyDevicePlatform.Tizen));
		
		//textBlockMessage.SetBinding(TextBlock.TextProperty,
		//	new Binding(nameof(ViewModel.Message)) { Mode = BindingMode.OneWay, Source = ViewModel });
		//textBlockMessage.SetBinding(VisibilityProperty,
		//	new Binding(nameof(ViewModel.MessageVisibility)) { Mode = BindingMode.OneWay, Source = ViewModel });
		//textBlockMessage.FontSize = ViewModel.FontSizeMessage;
		//ScrollViewer.Content = textBlockMessage;
	}
}