// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Common;

/// <summary>
/// Base class for TgPage.
/// </summary>
public partial class TgPageViewBase : UiPage
{
	#region Public and private methods

	protected void UpdatePage(Action action) => this.DispatcherUpdatePage(action);

	protected void UpdateMainWindow(Action action) => TgDispatcherUtils.DispatcherUpdateMainWindow(action);

	protected void UpdateApplication(Action action) => TgDispatcherUtils.DispatcherUpdateApplication(action);

	#endregion
}