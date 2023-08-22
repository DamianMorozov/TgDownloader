// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Utils;

/// <summary>
/// WPF extensions.
/// </summary>
public static class TgDispatcherUtils
{
	#region Public and private fields, properties, constructor

	public static DispatcherPriority Priority { get; set; } = DispatcherPriority.Normal;

	#endregion

	#region Public and private methods

	public static void DispatcherUpdateMainWindow() => DispatcherUpdateMainWindow(Priority);

	public static void DispatcherUpdateMainWindow(DispatcherPriority priority)
	{
		if (!Application.Current.Dispatcher.CheckAccess())
			Application.Current.Dispatcher.BeginInvoke(() =>
			{
				if (Application.Current.MainWindow is not null)
					Application.Current.MainWindow.UpdateLayout();
			}, priority);
		else
		{
			if (Application.Current.MainWindow is not null)
				Application.Current.MainWindow.UpdateLayout();
		}
	}
	
	public static void DispatcherUpdateMainWindow(Action action) => DispatcherUpdateMainWindow(action, Priority);

	public static void DispatcherUpdateMainWindow(Action action, DispatcherPriority priority)
	{
		if (!Application.Current.Dispatcher.CheckAccess())
			Application.Current.Dispatcher.BeginInvoke(() =>
			{
				//TgAsyncUtils.ExecAction(action);
				action();
				if (Application.Current.MainWindow is not null)
					Application.Current.MainWindow.UpdateLayout();
			}, priority);
		else
		{
			//TgAsyncUtils.ExecAction(action);
			action();
			if (Application.Current.MainWindow is not null)
				Application.Current.MainWindow.UpdateLayout();
		}
	}
	
	public static void DispatcherUpdateApplication(Action action) => DispatcherUpdateApplication(action, Priority);

	public static void DispatcherUpdateApplication(Action action, DispatcherPriority priority)
	{
		if (!Application.Current.Dispatcher.CheckAccess())
			Application.Current.Dispatcher.BeginInvoke(() =>
			{
				//TgAsyncUtils.ExecAction(action);
				action();
			}, priority);
		else
		{
			//TgAsyncUtils.ExecAction(action);
			action();
		}
	}

	#endregion
}