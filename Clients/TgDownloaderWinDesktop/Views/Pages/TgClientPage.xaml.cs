// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Pages;

/// <summary>
/// Interaction logic for TgClientVm.xaml
/// </summary>
public partial class TgClientPage
{
	#region Public and private fields, properties, constructor

	public TgClientPage()
	{
		TgDesktopUtils.TgClientVm.AddUpdateUi(TgEnumUpdateType.Page, UpdatePage);
		TgDesktopUtils.TgClientVm.AddUpdateUi(TgEnumUpdateType.Window, UpdateMainWindow);
		TgDesktopUtils.TgClientVm.AddUpdateUi(TgEnumUpdateType.Application, UpdateApplication);
		TgDesktopUtils.TgClientVm.OnNavigatedTo();
		InitializeComponent();
	}

	#endregion
}