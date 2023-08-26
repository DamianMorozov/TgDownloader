// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgDesktopUtils = TgDownloaderWinDesktop.Utils.TgDesktopUtils;

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgAdvancedView.xaml
/// </summary>
public partial class TgProxiesPage
{
	#region Public and private fields, properties, constructor

	public TgProxiesPage()
	{
		TgDesktopUtils.TgProxiesVm.AddUpdateUi(TgEnumUpdateType.Page, UpdatePage);
		TgDesktopUtils.TgProxiesVm.AddUpdateUi(TgEnumUpdateType.Window, UpdateMainWindow);
		TgDesktopUtils.TgProxiesVm.AddUpdateUi(TgEnumUpdateType.Application, UpdateApplication);
		TgDesktopUtils.TgProxiesVm.OnNavigatedTo();
        TgDesktopUtils.TgProxiesVm.OnLoadProxiesFromStorageAsync().ConfigureAwait(false);
		InitializeComponent();
	}

	#endregion
}