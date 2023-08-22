// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Views.Controls;

/// <summary>
/// UserControl Client state.
/// </summary>
public sealed partial class TgExceptionsUserControl : TgUserControlViewBase
{
	#region Public and private fields, properties, constructor

	public TgExceptionsUserControl()
	{
		TgDesktopUtils.TgClientVm.AddUpdateUi(TgEnumUpdateType.UserControl, UpdateUserControl);
		TgDesktopUtils.TgClientVm.AddUpdateUi(TgEnumUpdateType.Window, UpdateMainWindow);
		TgDesktopUtils.TgClientVm.AddUpdateUi(TgEnumUpdateType.Application, UpdateApplication);
		TgDesktopUtils.TgClientVm.OnNavigatedTo();
		InitializeComponent();
	}

	#endregion
}