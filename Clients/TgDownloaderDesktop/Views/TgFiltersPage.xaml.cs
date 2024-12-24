// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Views;

public partial class TgFiltersPage
{
	#region Public and private fields, properties, constructor

	public TgFiltersViewModel ViewModel { get; }

	public TgFiltersPage()
	{
		ViewModel = App.GetService<TgFiltersViewModel>();
		InitializeComponent();
	}

	#endregion

	#region Public and private methods

	protected override async void OnNavigatedTo(NavigationEventArgs e)
	{
		base.OnNavigatedTo(e);
		await ViewModel.OnNavigatedToAsync(e);
		ViewModel.OnLoaded(XamlRoot);
	}

	#endregion
}
