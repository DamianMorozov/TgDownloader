// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgAdvancedView.xaml
/// </summary>
public partial class TgSourcesPage
{
	#region Public and private fields, properties, constructor

	public TgSourcesPage()
	{
		TgDesktopUtils.TgSourcesVm.AddUpdateUi(TgEnumUpdateType.Page, UpdatePage);
		TgDesktopUtils.TgSourcesVm.AddUpdateUi(TgEnumUpdateType.Window, UpdateMainWindow);
		TgDesktopUtils.TgSourcesVm.AddUpdateUi(TgEnumUpdateType.Application, UpdateApplication);
		TgDesktopUtils.TgSourcesVm.OnNavigatedTo();
		InitializeComponent();
	}

	#endregion
}