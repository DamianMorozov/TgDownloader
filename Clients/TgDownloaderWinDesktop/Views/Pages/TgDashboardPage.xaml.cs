// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgDashboardPage.xaml
/// </summary>
public partial class TgDashboardPage
{
	#region Public and private fields, properties, constructor

	public TgDashboardPage()
	{
		TgDesktopUtils.TgDashboardVm.AddUpdateUi(TgEnumUpdateType.Page, UpdatePage);
		TgDesktopUtils.TgDashboardVm.AddUpdateUi(TgEnumUpdateType.Window, UpdateMainWindow);
		TgDesktopUtils.TgDashboardVm.AddUpdateUi(TgEnumUpdateType.Application, UpdateApplication);
		TgDesktopUtils.TgDashboardVm.OnNavigatedTo();
		InitializeComponent();
	}

	#endregion
}