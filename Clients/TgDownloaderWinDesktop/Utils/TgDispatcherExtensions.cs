// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Utils;

/// <summary>
/// WPF extensions.
/// </summary>
public static class TgDispatcherExtensions
{
	#region Public and private fields, properties, constructor

	public static DispatcherPriority Priority { get; set; } = DispatcherPriority.Normal;

	#endregion

	#region Public and private methods

	public static void DispatcherUpdateGrid(this Grid grid) => DispatcherUpdateGrid(grid, Priority);
	
	public static void DispatcherUpdateGrid(this Grid grid, DispatcherPriority priority)
	{
		if (!grid.Dispatcher.CheckAccess())
			grid.Dispatcher.BeginInvoke(() =>
			{
				grid.UpdateLayout();
			}, priority);
		else
		{
			grid.UpdateLayout();
		}
	}

	public static void DispatcherUpdateGrid(this Grid grid, Action action) => DispatcherUpdateGrid(grid, action, Priority);
	
	public static void DispatcherUpdateGrid(this Grid grid, Action action, DispatcherPriority priority)
	{
		if (!grid.Dispatcher.CheckAccess())
			grid.Dispatcher.BeginInvoke(() =>
			{
				//TgAsyncUtils.ExecAction(action);
				action();
				grid.UpdateLayout();
			}, priority);
		else
		{
			//TgAsyncUtils.ExecAction(action);
			action();
			grid.UpdateLayout();
		}
	}

	
	public static void DispatcherUpdateUserControl(this UserControl userControl) => DispatcherUpdateUserControl(userControl, Priority);
	
	public static void DispatcherUpdateUserControl(this UserControl userControl, DispatcherPriority priority)
	{
		if (!userControl.Dispatcher.CheckAccess())
			userControl.Dispatcher.BeginInvoke(() =>
			{
				userControl.UpdateLayout();
			}, priority);
		else
		{
			userControl.UpdateLayout();
		}
	}

	public static void DispatcherUpdateUserControl(this UserControl userControl, Action action) => DispatcherUpdateUserControl(userControl, action, Priority);

	public static void DispatcherUpdateUserControl(this UserControl userControl, Action action, DispatcherPriority priority)
	{
		if (!userControl.Dispatcher.CheckAccess())
			userControl.Dispatcher.BeginInvoke(() =>
			{
				//TgAsyncUtils.ExecAction(action);
				action();
				userControl.UpdateLayout();
			}, priority);
		else
		{
			//TgAsyncUtils.ExecAction(action);
			action();
			userControl.UpdateLayout();
		}
	}

	
	public static void DispatcherUpdatePage(this UiPage uiPage) => DispatcherUpdatePage(uiPage, Priority);

	public static void DispatcherUpdatePage(this UiPage uiPage, DispatcherPriority priority)
	{
		if (!uiPage.Dispatcher.CheckAccess())
			uiPage.Dispatcher.BeginInvoke(() =>
			{
				uiPage.UpdateLayout();
			}, priority);
		else
		{
			uiPage.UpdateLayout();
		}
	}

	public static void DispatcherUpdatePage(this UiPage uiPage, Action action) => DispatcherUpdatePage(uiPage, action, Priority);

	public static void DispatcherUpdatePage(this UiPage uiPage, Action action, DispatcherPriority priority)
	{
		if (!uiPage.Dispatcher.CheckAccess())
			uiPage.Dispatcher.BeginInvoke(() =>
			{
				//TgAsyncUtils.ExecAction(action);
				action();
				uiPage.UpdateLayout();
			}, priority);
		else
		{
			//TgAsyncUtils.ExecAction(action);
			action();
			uiPage.UpdateLayout();
		}
	}

	
	public static void DispatcherUpdateWindow(this Window window) => DispatcherUpdateWindow(window, Priority);

	public static void DispatcherUpdateWindow(this Window window, DispatcherPriority priority)
	{
		if (!window.Dispatcher.CheckAccess())
			window.Dispatcher.BeginInvoke(() =>
			{
				window.UpdateLayout();
			}, priority);
		else
		{
			window.UpdateLayout();
		}
	}

	public static void DispatcherUpdateWindow(this Window window, Action action) => DispatcherUpdateWindow(window, action, Priority);

	public static void DispatcherUpdateWindow(this Window window, Action action, DispatcherPriority priority)
	{
		if (!window.Dispatcher.CheckAccess())
			window.Dispatcher.BeginInvoke(() =>
			{
				//TgAsyncUtils.ExecAction(action);
				action();
				window.UpdateLayout();
			}, priority);
		else
		{
			//TgAsyncUtils.ExecAction(action);
			action();
			window.UpdateLayout();
		}
	}

	#endregion
}