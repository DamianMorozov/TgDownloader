// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Common;

public class TgUserControlViewBase : UserControl
{
	#region Public and private methods

	protected void UpdateUserControl(Action action) => this.DispatcherUpdateUserControl(action);
	protected void UpdateMainWindow(Action action) => TgDispatcherUtils.DispatcherUpdateMainWindow(action);
	protected void UpdateApplication(Action action) => TgDispatcherUtils.DispatcherUpdateApplication(action);

	#endregion
}