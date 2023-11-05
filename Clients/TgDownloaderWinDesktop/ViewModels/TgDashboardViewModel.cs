// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgDashboardViewModel : TgPageViewModelBase, INavigationAware
{
	#region Public and private methods

	public void OnNavigatedTo()
	{
        _ = Task.Run(InitializeViewModelAsync).ConfigureAwait(true);
    }

	public void OnNavigatedFrom() { }

    #endregion

    #region Public and private methods

    // SettingsDefaultCommand
    [RelayCommand]
	public async Task OnSettingsDefaultAsync()
	{
        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            TgAppSettings.DefaultXmlSettings();
        }, false).ConfigureAwait(false);
	}

    // SettingsSaveCommand
    [RelayCommand]
	public async Task OnSettingsSaveAsync()
	{
        await TgDesktopUtils.RunActionAsync(this, () =>
        {
            TgAppSettings.StoreXmlSettingsUnsafe();
            TgAppSettings.LoadXmlSettings();
        }, false).ConfigureAwait(false);
	}

	#endregion
}