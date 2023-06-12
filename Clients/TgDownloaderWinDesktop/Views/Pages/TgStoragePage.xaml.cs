// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgSectionSourcesView.xaml
/// </summary>
public partial class TgStoragePage : INotifyPropertyChanged
{
	public TgStorageViewModel ViewModel { get; init; }

	public TgStoragePage(TgStorageViewModel viewModel) : base(viewModel)
	{
		ViewModel = viewModel;
		InitializeComponent();
	}
}