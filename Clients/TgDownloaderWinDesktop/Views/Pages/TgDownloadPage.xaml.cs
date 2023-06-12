// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgDownloadView.xaml
/// </summary>
public partial class TgDownloadPage : INotifyPropertyChanged
{
	public TgDownloadViewModel ViewModel { get; set; }

	public TgDownloadPage(TgDownloadViewModel viewModel) : base(viewModel)
	{
		ViewModel = viewModel;
		InitializeComponent();
	}
}