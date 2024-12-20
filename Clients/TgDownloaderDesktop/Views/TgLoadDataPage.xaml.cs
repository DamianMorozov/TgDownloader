// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Views;

public partial class TgLoadDataPage
{
	#region Public and private fields, properties, constructor

	public TgLoadDataViewModel ViewModel { get; }

	public TgLoadDataPage()
	{
		ViewModel = App.GetService<TgLoadDataViewModel>();
		InitializeComponent();
	}

	#endregion

	#region Public and private methods

	protected override async void OnNavigatedTo(NavigationEventArgs e)
	{
		base.OnNavigatedTo(e);
		await ViewModel.OnNavigatedToAsync(e);
		ViewModel.OnLoaded(XamlRoot);
		ViewModel.IsPageLoad = true;
	}

	protected override void OnNavigatedFrom(NavigationEventArgs e)
	{
		base.OnNavigatedFrom(e);
		ViewModel.IsPageLoad = false;
	}

	#endregion
}
