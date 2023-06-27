// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgFiltersView.xaml
/// </summary>
public partial class TgMenuFiltersPage
{
	#region Public and private fields, properties, constructor

	public TgFiltersViewModel ViewModel { get; set; }

	public TgMenuFiltersPage(TgFiltersViewModel viewModel)
	{
		ViewModel = viewModel;
		ViewModel.OnNavigatedTo();
		InitializeComponent();
	}

	#endregion
}