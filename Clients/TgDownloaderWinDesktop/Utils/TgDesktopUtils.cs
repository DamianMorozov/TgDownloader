// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Utils;

/// <summary>
/// Desktop utils.
/// </summary>
public static class TgDesktopUtils
{
    #region Public and private fields, properties, constructor

    public static TgClientHelper TgClient => TgClientHelper.Instance;
    private static TgXpoContext XpoContext { get; } = new(TgEnumStorageType.Prod);
    public static TgClientViewModel TgClientVm { get; } = new();
    public static TgDashboardViewModel TgDashboardVm { get; } = new();
    public static TgFiltersViewModel TgFiltersVm { get; } = new();
    public static TgItemProxyViewModel TgItemProxyVm { get; } = new(XpoContext.ProxyRepository);
    public static TgItemSourceViewModel TgItemSourceVm { get; } = new();
	public static TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
	public static TgConstants TgConst => TgConstants.CreateInstance();
	public static TgProxiesViewModel TgProxiesVm { get; } = new();
    public static TgSettingsViewModel TgSettingsVm { get; } = new();
    public static TgSourcesViewModel TgSourcesVm { get; } = new();
    public static TgDownloadsViewModel TgDownloadsVm{ get; } = new();

    #endregion

    #region Public and private methods

    /// <summary>
    /// Set client state update.
    /// </summary>
    public static void SetupClient()
    {
        TgClient.SetupUpdateStateConnect(TgClientVm.UpdateStateConnectAsync);
        TgClient.SetupUpdateStateProxy(TgClientVm.UpdateStateProxyAsync);
        TgClient.SetupUpdateStateSource(TgClientVm.UpdateStateSourceAsync);
		TgClient.SetupUpdateStateMessage(TgClientVm.UpdateStateMessageAsync);
        TgClient.SetupUpdateStateException(TgClientVm.UpdateStateExceptionAsync);
        TgClient.SetupUpdateStateExceptionShort(TgClientVm.UpdateStateExceptionShortAsync);
        TgClient.SetupAfterClientConnect(TgClientVm.AfterClientConnectAsync);
        TgClient.SetupGetClientDesktopConfig(TgClientVm.ConfigClientDesktop);
    }

    #endregion

    #region Public and private methods

    public static void RunAction(TgPageViewModelBase viewModel, Action action, bool isUpdateLoad)
    {
        void Job()
        {
            if (isUpdateLoad)
                viewModel.IsLoad = true;
            TgClientVm.Exception.Clear();
            action();
        }

        void JobFinally()
        {
            viewModel.IsLoad = false;
        }

        try
        {
            if (viewModel.Dispatcher.CheckAccess())
            {
                Job();
            }
            else
            {
                viewModel.Dispatcher.Invoke(Job);
            }
        }
        catch (Exception ex)
        {
            if (viewModel.Dispatcher.CheckAccess())
                TgClientVm.Exception.Set(ex);
            else
                viewModel.Dispatcher.Invoke(() => { TgClientVm.Exception.Set(ex); });
        }
        finally
        {
            if (isUpdateLoad)
            {
                if (viewModel.Dispatcher.CheckAccess())
                    JobFinally();
                else
                    viewModel.Dispatcher.Invoke(JobFinally);
            }
        }
    }

    public static async Task RunActionAsync(TgPageViewModelBase viewModel, Action action, bool isUpdateLoad)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

        void Job()
        {
            if (isUpdateLoad)
                viewModel.IsLoad = true;
            TgClientVm.Exception.Clear();
            action();
        }

        void JobFinally()
        {
            viewModel.IsLoad = false;
        }

        try
        {
            if (viewModel.Dispatcher.CheckAccess())
            {
                Job();
            }
            else
            {
                viewModel.Dispatcher.Invoke(() =>
                {
                    Job();
                });
            }
        }
        catch (Exception ex)
        {
            if (viewModel.Dispatcher.CheckAccess())
                TgClientVm.Exception.Set(ex);
            else
                viewModel.Dispatcher.Invoke(() => { TgClientVm.Exception.Set(ex); });
        }
        finally
        {
            if (isUpdateLoad)
            {
                if (viewModel.Dispatcher.CheckAccess())
                    JobFinally();
                else
                    viewModel.Dispatcher.Invoke(() =>
                    {
                        JobFinally();
                    });
            }
        }
    }

    public static async Task RunAction2Async(TgPageViewModelBase viewModel, Action action, bool isUpdateLoad)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

        async Task Job()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            if (isUpdateLoad)
                viewModel.IsLoad = true;
            TgClientVm.Exception.Clear();
            action();
        }

        async Task JobFinally()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            viewModel.IsLoad = false;
        }

        try
        {
            if (viewModel.Dispatcher.CheckAccess())
            {
                Job();
            }
            else
            {
                viewModel.Dispatcher.InvokeAsync(async () =>
                {
                    await Job();
                });
            }
        }
        catch (Exception ex)
        {
            if (viewModel.Dispatcher.CheckAccess())
                TgClientVm.Exception.Set(ex);
            else
                viewModel.Dispatcher.InvokeAsync(async () => { TgClientVm.Exception.Set(ex); });
        }
        finally
        {
            if (isUpdateLoad)
            {
                if (viewModel.Dispatcher.CheckAccess())
                    JobFinally();
                else
                    viewModel.Dispatcher.InvokeAsync(async () =>
                    {
                        await JobFinally();
                    });
            }
        }
    }

    public static async Task RunFuncAsync(TgPageViewModelBase viewModel, Func<Task> action, bool isUpdateLoad)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

        async Task Job()
        {
            if (isUpdateLoad)
                viewModel.IsLoad = true;
            TgClientVm.Exception.Clear();
            await action();
        }

        void JobFinally()
        {
            viewModel.IsLoad = false;
        }

        try
        {
            if (viewModel.Dispatcher.CheckAccess())
            {
                await Job();
            }
            else
            {
                await viewModel.Dispatcher.InvokeAsync(async () =>
                {
                    await Job();
                });
            }
        }
        catch (Exception ex)
        {
            if (viewModel.Dispatcher.CheckAccess())
                TgClientVm.Exception.Set(ex);
            else
                await viewModel.Dispatcher.InvokeAsync(() => TgClientVm.Exception.Set(ex));
        }
        finally
        {
            if (isUpdateLoad)
            {
                if (viewModel.Dispatcher.CheckAccess())
                    JobFinally();
                else
                    await viewModel.Dispatcher.InvokeAsync(JobFinally);
            }
        }
    }

    #endregion
}