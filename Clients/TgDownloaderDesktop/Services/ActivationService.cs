// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Services;

public sealed class ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers,
	ITgSettingsService settingsService) : IActivationService
{
	private UIElement? _shell;

	public async Task ActivateAsync(object activationArgs)
	{
		// Execute tasks before activation
		await settingsService.LoadAsync();
		// Set the MainWindow Content
		if (App.MainWindow.Content == null)
		{
			_shell = App.GetService<ShellPage>();
			App.MainWindow.Content = _shell ?? new Frame();
		}
		// Handle activation via ActivationHandlers
		await HandleActivationAsync(activationArgs);
		// Activate the MainWindow
		App.MainWindow.Activate();
		// Execute tasks after activation
		await StartupAsync();
	}

	private async Task HandleActivationAsync(object activationArgs)
	{
		var activationHandler = activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));
		if (activationHandler != null)
		{
			await activationHandler.HandleAsync(activationArgs);
		}
		if (defaultHandler.CanHandle(activationArgs))
		{
			await defaultHandler.HandleAsync(activationArgs);
		}
	}

	public async Task StartupAsync()
	{
		TgAsyncUtils.SetAppType(TgEnumAppType.Desktop);
		// Register TgEfContext as the DbContext for EF Core
		TgFileUtils.AppStorage = App.GetService<ITgSettingsService>().AppStorage;
		await TgEfUtils.CreateAndUpdateDbAsync();
	}
}
