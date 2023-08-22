// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgDashboardViewModel : TgPageViewModelBase, INavigationAware
{
	#region Public and private methods

	public void OnNavigatedTo()
	{
		if (!IsInitialized)
			InitializeViewModel();
	}

	public void OnNavigatedFrom()
	{
		//
	}

	#endregion

	#region Public and private methods

	[RelayCommand]
	public async Task OnSettingsDefaultAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgDesktopUtils.RunAction(this, () =>
		{
				TgAppSettings.DefaultXmlSettings();
		});
	}

[RelayCommand]
	public async Task OnSettingsSaveAsync()
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		TgDesktopUtils.RunAction(this, () =>
		{
				TgAppSettings.StoreXmlSettingsUnsafe();
		TgAppSettings.LoadXmlSettings();
		});
	}

	#endregion
}