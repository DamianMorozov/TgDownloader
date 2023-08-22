// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgFiltersVm.xaml
/// </summary>
public partial class TgFiltersPage
{
	#region Public and private fields, properties, constructor

	public TgFiltersPage()
	{
		TgDesktopUtils.TgFiltersVm.AddUpdateUi(TgEnumUpdateType.Page, UpdatePage);
		TgDesktopUtils.TgFiltersVm.AddUpdateUi(TgEnumUpdateType.Window, UpdateMainWindow);
		TgDesktopUtils.TgFiltersVm.AddUpdateUi(TgEnumUpdateType.Application, UpdateApplication);
		TgDesktopUtils.TgFiltersVm.OnNavigatedTo();
		InitializeComponent();
	}

	#endregion
}