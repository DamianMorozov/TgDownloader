// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktopWPF.Views.Pages;

/// <summary>
/// Interaction logic for TgAdvancedView.xaml
/// </summary>
public partial class TgDownloadsPage
{
	#region Public and private fields, properties, constructor

	public TgDownloadsPage()
	{
		TgDesktopUtils.TgDownloadsVm.OnNavigatedTo();
		InitializeComponent();
	}

	#endregion
}