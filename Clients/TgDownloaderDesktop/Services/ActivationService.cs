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
		await LoadAsync();
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

	private async Task LoadAsync()
	{
		await settingsService.LoadAsync();
		await Task.CompletedTask;
	}

	private async Task StartupAsync()
	{
		// Register TgEfContext as the DbContext for EF Core
		await TgEfUtils.CreateAndUpdateDbAsync();
		TgAsyncUtils.SetAppType(TgEnumAppType.Desktop);
		await Task.CompletedTask;
	}
}
