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
}