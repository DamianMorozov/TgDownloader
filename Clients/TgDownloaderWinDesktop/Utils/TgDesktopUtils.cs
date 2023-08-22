// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Utils;

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
	public static void SetClientActions()
	{
		TgClient.UpdateStateClient = TgClientVm.UpdateStateClient;
		TgClient.UpdateStateException = TgClientVm.UpdateStateException;
		TgClient.UpdateStateSource = TgClientVm.UpdateStateSource;
		TgClient.AfterClientConnect = TgClientVm.AfterClientConnect;
		TgClient.GetClientDesktopConfig = TgClientVm.GetDesktopConfig;
	}

	#endregion

	#region Public and private methods

	public static void RunAction(TgViewModelBase viewModel, Action action)
	{
		TgAsyncUtils.ExecTaskAsync(async () =>
		{
			try
			{
				TgDispatcherUtils.DispatcherUpdateApplication(() =>
				{
					viewModel.IsLoad = true;
				});
				await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

				TgDispatcherUtils.DispatcherUpdateApplication(action);
			}
			catch (Exception ex)
			{
				TgDispatcherUtils.DispatcherUpdateApplication(() =>
				{
					viewModel.IsLoad = true;
					TgClientVm.Exception.Set(ex);
				});
			}
			finally
			{
				await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
				TgDispatcherUtils.DispatcherUpdateApplication(() => { viewModel.IsLoad = false; });
			}
		}, false);
	}
	
public static void RunAction(Action action)
	{
		TgAsyncUtils.ExecTaskAsync(async () =>
		{
			try
			{
				TgDispatcherUtils.DispatcherUpdateApplication(() =>
				{
					TgClientVm.Exception.Clear();
				});
				await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
				TgDispatcherUtils.DispatcherUpdateApplication(action);
			}
			catch (Exception ex)
			{
				TgClientVm.Exception.Set(ex);
			}
		}, false);
	}
	
	#endregion
}