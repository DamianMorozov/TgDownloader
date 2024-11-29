// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public partial class TgSettingsViewModel : TgPageViewModelBase
{
	#region Public and private fields, properties, constructor

	public ICommand SettingsDefaultCommand { get; }
	public ICommand SettingsSaveCommand { get; }

	public TgSettingsViewModel(ITgSettingsService settingsService) : base(settingsService)
	{
		SettingsDefaultCommand = new AsyncRelayCommand(SettingsDefaultAsync);
		SettingsSaveCommand = new AsyncRelayCommand(SettingsSaveAsync);
	}

	#endregion

	#region Public and private methods

	public async Task OnNavigatedToAsync(NavigationEventArgs navigationEventArgs) => await SettingsService.LoadAsync();

	private async Task SettingsDefaultAsync() => await ContentDialogAsync(SettingsDefaultCoreAsync, TgResourceExtensions.AskSettingsDefault());

	private async Task SettingsDefaultCoreAsync()
	{
		SettingsService.Default();
		await Task.CompletedTask;
	}

	private async Task SettingsSaveAsync()
	{
		await ContentDialogAsync(SettingsService.SaveAsync, TgResourceExtensions.AskSettingsSave());
#if DEBUG
		await ContentDialogAsync(TgResourceExtensions.AssertionRestartApp(), ContentDialogButton.Primary);
#else
		await ContentDialogAsync(async () => { await Windows.ApplicationModel.Core.CoreApplication.RequestRestartAsync(string.Empty); }, 
			TgResourceExtensions.AskRestartApp());
#endif
		await Task.CompletedTask;
	}

	#endregion
}
