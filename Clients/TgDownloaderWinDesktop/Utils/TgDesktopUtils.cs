// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
namespace TgDownloaderWinDesktop.Utils;

/// <summary>
/// Desktop utils.
/// </summary>
public static class TgDesktopUtils
{
    #region Public and private fields, properties, constructor

    public static TgClientHelper TgClient => TgClientHelper.Instance;
    public static TgClientViewModel TgClientVm { get; } = new();
    public static TgDashboardViewModel TgDashboardVm { get; } = new();
    public static TgFiltersViewModel TgFiltersVm { get; } = new();
    public static TgItemSourceViewModel TgItemSourceVm { get; } = new();
    public static TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
    public static TgProxiesViewModel TgProxiesVm { get; } = new();
    public static TgSettingsViewModel TgSettingsVm { get; } = new();
    public static TgSourcesViewModel TgSourcesVm { get; } = new();

    #endregion

    #region Public and private methods

    /// <summary>
    /// Set client state update.
    /// </summary>
    public static void SetupClient()
    {
        TgClient.SetupActions(TgClientVm.UpdateStateConnect, TgClientVm.UpdateStateProxy,
            TgClientVm.UpdateStateMessage, TgClientVm.UpdateStateSource, TgClientVm.UpdateStateException,
            TgClientVm.AfterClientConnect, TgClientVm.GetClientDesktopConfig);
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

    public static async Task RunFuncAsync(TgPageViewModelBase viewModel, Func<Task> func, bool isUpdateLoad)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

        try
        {
            if (viewModel.Dispatcher.CheckAccess())
            {
                if (isUpdateLoad)
                    viewModel.IsLoad = true;
                TgClientVm.Exception.Clear();
                await func();
            }
            else
            {
                viewModel.Dispatcher.InvokeAsync(() =>
                {
                    if (isUpdateLoad)
                        viewModel.IsLoad = true;
                    TgClientVm.Exception.Clear();
                    func();
                });
            }
        }
        catch (Exception ex)
        {
            if (viewModel.Dispatcher.CheckAccess())
                TgClientVm.Exception.Set(ex);
            else
                viewModel.Dispatcher.InvokeAsync(() => { TgClientVm.Exception.Set(ex); });
        }
        finally
        {
            if (isUpdateLoad)
            {
                if (viewModel.Dispatcher.CheckAccess())
                    viewModel.IsLoad = false;
                else
                    viewModel.Dispatcher.InvokeAsync(() => { viewModel.IsLoad = false; });
            }
        }
    }

    #endregion
}