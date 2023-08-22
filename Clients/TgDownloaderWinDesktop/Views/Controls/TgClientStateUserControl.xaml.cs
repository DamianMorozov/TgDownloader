// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Controls;

/// <summary>
/// UserControl Client state.
/// </summary>
public sealed partial class TgClientStateUserControl : TgUserControlViewBase
{
	#region Public and private fields, properties, constructor

	public TgClientStateUserControl()
	{
		TgDesktopUtils.TgClientVm.AddUpdateUi(TgEnumUpdateType.UserControl, UpdateUserControl);
		TgDesktopUtils.TgProxiesVm.AddUpdateUi(TgEnumUpdateType.Window, UpdateMainWindow);
		TgDesktopUtils.TgProxiesVm.AddUpdateUi(TgEnumUpdateType.Application, UpdateApplication);
		TgDesktopUtils.TgClientVm.OnNavigatedTo();
		InitializeComponent();
	}

	#endregion
}