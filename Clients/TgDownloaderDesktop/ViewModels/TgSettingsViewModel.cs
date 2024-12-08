// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public partial class TgSettingsViewModel : TgPageViewModelBase
{
	#region Public and private fields, properties, constructor

	public IRelayCommand SettingsDefaultCommand { get; }
	public IRelayCommand SettingsSaveCommand { get; }

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
		await SettingsSaveCoreAsync();
	}

	private async Task SettingsSaveAsync()
	{
		await ContentDialogAsync(SettingsSaveCoreAsync, TgResourceExtensions.AskSettingsSave());
	}

	private async Task SettingsSaveCoreAsync()
	{
		await SettingsService.SaveAsync();
		await ContentDialogAsync(async () => { await Windows.ApplicationModel.Core.CoreApplication.RequestRestartAsync(string.Empty); },
			TgResourceExtensions.AskRestartApp(), ContentDialogButton.Primary);
	}

	#endregion
}
