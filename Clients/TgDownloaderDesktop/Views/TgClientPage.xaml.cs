// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Views;

public partial class TgClientPage
{
	#region Public and private fields, properties, constructor

	public TgClientViewModel ViewModel { get; }

	public TgClientPage()
	{
		ViewModel = App.GetService<TgClientViewModel>();
		InitializeComponent();
		Loaded += OnLoaded;
	}

	#endregion

	#region Public and private methods

	protected override async void OnNavigatedTo(NavigationEventArgs e)
	{
		base.OnNavigatedTo(e);
		await ViewModel.OnNavigatedToAsync(e);
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		ViewModel.OnLoaded(XamlRoot);
	}

	#endregion
}
