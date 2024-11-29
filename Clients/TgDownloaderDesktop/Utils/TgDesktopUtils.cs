// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Utils;

/// <summary> Desktop utils </summary>
public static class TgDesktopUtils
{
    #region Public and private fields, properties, constructor

    public static TgClientHelper TgClient => TgClientHelper.Instance;
	//public static TgClientViewModel TgClientVm { get; } = App.GetService<TgClientViewModel>();
	//public static TgDashboardViewModel TgDashboardVm { get; } = new();
	//public static TgFiltersViewModel TgFiltersVm { get; } = new();
	//public static TgItemProxyViewModel TgItemProxyVm { get; } = new();
	//public static TgItemSourceViewModel TgItemSourceVm { get; } = new();
	//public static TgProxiesViewModel TgProxiesVm { get; } = new();
	//public static TgSettingsViewModel TgSettingsVm { get; } = new();
	//public static TgSourcesViewModel TgSourcesVm { get; } = new();
	//public static TgDownloadsViewModel TgDownloadsVm{ get; } = new();
	//public static string CurrentPath = Package.Current.InstalledLocation.Path;
	public static string CurrentPath = AppContext.BaseDirectory;

	#endregion

    #region Public and private methods

    //public static void RunAction(TgPageViewModelBase viewModel, Action action, bool isUpdateLoad)
    //{
    //    void Job()
    //    {
    //        if (isUpdateLoad)
    //            viewModel.IsLoad = true;
    //        TgClientViewModel.Exception.Clear();
    //        action();
    //    }

    //    void JobFinally()
    //    {
    //        viewModel.IsLoad = false;
    //    }

    //    try
    //    {
	   //     App.MainWindow.DispatcherQueue.TryEnqueue(Job);
    //    }
    //    catch (Exception ex)
    //    {
	   //     App.MainWindow.DispatcherQueue.TryEnqueue(() => { TgClientViewModel.Exception.Set(ex); });
    //    }
    //    finally
    //    {
    //        if (isUpdateLoad)
    //        {
	   //         App.MainWindow.DispatcherQueue.TryEnqueue(JobFinally);
    //        }
    //    }
    //}

    //public static async Task RunActionAsync(TgPageViewModelBase viewModel, Action action, bool isUpdateLoad)
    //{
    //    await Task.Delay(1);

    //    void Job()
    //    {
    //        if (isUpdateLoad)
    //            viewModel.IsLoad = true;
    //        TgClientVm.Exception.Clear();
    //        action();
    //    }

    //    void JobFinally()
    //    {
    //        viewModel.IsLoad = false;
    //    }

    //    try
    //    {
    //        if (viewModel.Dispatcher.CheckAccess())
    //        {
    //            Job();
    //        }
    //        else
    //        {
    //            viewModel.Dispatcher.Invoke(() =>
    //            {
    //                Job();
    //            });
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        if (viewModel.Dispatcher.CheckAccess())
    //            TgClientVm.Exception.Set(ex);
    //        else
    //            viewModel.Dispatcher.Invoke(() => { TgClientVm.Exception.Set(ex); });
    //    }
    //    finally
    //    {
    //        if (isUpdateLoad)
    //        {
    //            if (viewModel.Dispatcher.CheckAccess())
    //                JobFinally();
    //            else
    //                viewModel.Dispatcher.Invoke(() =>
    //                {
    //                    JobFinally();
    //                });
    //        }
    //    }
    //}

    //public static async Task RunAction2Async(TgPageViewModelBase viewModel, Action action, bool isUpdateLoad)
    //{
    //    await Task.Delay(1);

    //    async Task Job()
    //    {
    //        await Task.Delay(1);
    //        if (isUpdateLoad)
    //            viewModel.IsLoad = true;
    //        TgClientVm.Exception.Clear();
    //        action();
    //    }

    //    async Task JobFinally()
    //    {
    //        await Task.Delay(1);
    //        viewModel.IsLoad = false;
    //    }

    //    try
    //    {
    //        if (viewModel.Dispatcher.CheckAccess())
    //        {
    //            Job();
    //        }
    //        else
    //        {
    //            viewModel.Dispatcher.InvokeAsync(async () =>
    //            {
    //                await Job();
    //            });
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        if (viewModel.Dispatcher.CheckAccess())
    //            TgClientVm.Exception.Set(ex);
    //        else
    //            viewModel.Dispatcher.InvokeAsync(async () => { TgClientVm.Exception.Set(ex); });
    //    }
    //    finally
    //    {
    //        if (isUpdateLoad)
    //        {
    //            if (viewModel.Dispatcher.CheckAccess())
    //                JobFinally();
    //            else
    //                viewModel.Dispatcher.InvokeAsync(async () =>
    //                {
    //                    await JobFinally();
    //                });
    //        }
    //    }
    //}

    public static async Task RunFuncAsync(TgPageViewModelBase viewModel, Func<Task> action, bool isUpdateLoad)
    {
        async Task Job()
        {
            if (isUpdateLoad)
                viewModel.IsLoad = true;
            //TgClientViewModel.Exception.Clear();
            await action();
        }

        void JobFinally()
        {
            viewModel.IsLoad = false;
        }

        try
        {
	        App.MainWindow.DispatcherQueue.TryEnqueue(async () => await Job());
        }
        catch (Exception ex)
        {
	        //App.MainWindow.DispatcherQueue.TryEnqueue(() => TgClientViewModel.Exception.Set(ex));
        }
        finally
        {
            if (isUpdateLoad)
            {
	            App.MainWindow.DispatcherQueue.TryEnqueue(JobFinally);
            }
            await Task.CompletedTask;
        }
    }

    public static void FileLog(Exception ex)
    {
		var logPath = Path.Combine(CurrentPath, TgFileUtils.FileLog);
		File.AppendAllText(logPath, $"[{DateTime.Now}] Exception: {ex.Message}{Environment.NewLine}Stack Trace: {ex.StackTrace}{Environment.NewLine}");
	}

	public static void FileLog(string message)
    {
	    var logPath = Path.Combine(CurrentPath, TgFileUtils.FileLog);
	    File.AppendAllText(logPath, $"[{DateTime.Now}] {message}{Environment.NewLine}");
    }

	#endregion
}