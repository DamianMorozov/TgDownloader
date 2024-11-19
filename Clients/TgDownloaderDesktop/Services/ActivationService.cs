// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Services;

public sealed class ActivationService : IActivationService
{
	private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
	private readonly IEnumerable<IActivationHandler> _activationHandlers;
	private readonly ITgSettingsService _settingsService;
	private UIElement? _shell = null;

	public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers, 
		ITgSettingsService settingsService)
	{
		_defaultHandler = defaultHandler;
		_activationHandlers = activationHandlers;
		_settingsService = settingsService;
	}

	public async Task ActivateAsync(object activationArgs)
	{
		// Execute tasks before activation.
		await InitializeAsync();
		// Set the MainWindow Content.
		if (App.MainWindow.Content == null)
		{
			_shell = App.GetService<ShellPage>();
			App.MainWindow.Content = _shell ?? new Frame();
		}
		// Handle activation via ActivationHandlers.
		await HandleActivationAsync(activationArgs);
		// Activate the MainWindow.
		App.MainWindow.Activate();
		// Execute tasks after activation.
		await StartupAsync();
	}

	private async Task HandleActivationAsync(object activationArgs)
	{
		var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));
		if (activationHandler != null)
		{
			await activationHandler.HandleAsync(activationArgs);
		}
		if (_defaultHandler.CanHandle(activationArgs))
		{
			await _defaultHandler.HandleAsync(activationArgs);
		}
	}

	private async Task InitializeAsync()
	{
		await _settingsService.LoadAsync();
		await Task.CompletedTask;
	}

	private async Task StartupAsync()
	{
		// Register TgEfContext as the DbContext for EF Core
		await TgEfUtils.CreateAndUpdateDbAsync();

		TgAsyncUtils.SetAppType(TgEnumAppType.Desktop);
		var tgClientVm = App.GetService<TgClientViewModel>();
		TgClientHelper.Instance.SetupGetClientDesktopConfig(tgClientVm.ConfigClientDesktop);

		await _settingsService.SetRequestedThemeAsync();
		await Task.CompletedTask;
	}
}
