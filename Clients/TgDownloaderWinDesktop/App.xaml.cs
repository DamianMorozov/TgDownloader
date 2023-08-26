// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
	private TgSqlContextManagerHelper ContextManager => TgSqlContextManagerHelper.Instance;

	// The.NET Generic Host provides dependency injection, configuration, logging, and other services.
	// https://docs.microsoft.com/dotnet/core/extensions/generic-host
	// https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
	// https://docs.microsoft.com/dotnet/core/extensions/configuration
	// https://docs.microsoft.com/dotnet/core/extensions/logging
	private static readonly IHost Host = Microsoft.Extensions.Hosting.Host
		.CreateDefaultBuilder()
		.ConfigureAppConfiguration(configurationBinder =>
        {
            string? path = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
			if (!string.IsNullOrEmpty(path))
                configurationBinder.SetBasePath(path);
        })
		.ConfigureServices((context, services) =>
		{
			// App Host
			services.AddHostedService<ApplicationHostService>();
			// Page resolver service
			services.AddSingleton<IPageService, PageService>();
			// Theme manipulation
			services.AddSingleton<IThemeService, ThemeService>();
			// TaskBar manipulation
			services.AddSingleton<ITaskBarService, TaskBarService>();
			// Service containing navigation, same as INavigationWindow... but without window
			services.AddSingleton<INavigationService, NavigationService>();
			// Main window with navigation
			services.AddScoped<INavigationWindow, MainWindow>();
			services.AddScoped<TgMainWindowViewModel>();
			// Views and ViewModels
			services.AddTransient<TgDashboardPage>();
			services.AddTransient<TgDashboardViewModel>();
			services.AddTransient<TgSettingsPage>();
			services.AddTransient<TgSettingsViewModel>();
			services.AddTransient<TgSourcesPage>();
			services.AddTransient<TgSourcesViewModel>();
			services.AddTransient<TgClientPage>();
			services.AddTransient<TgClientViewModel>();
			services.AddTransient<TgProxiesPage>();
			services.AddTransient<TgProxiesViewModel>();
			// Configuration
			services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
		}).Build();

	/// <summary>
	/// Gets registered service.
	/// </summary>
	/// <typeparam name="T">Type of the service to get.</typeparam>
	/// <returns>Instance of the service or <see langword="null"/>.</returns>
	public static T GetService<T>() where T : class, new() => 
        Host.Services.GetService(typeof(T)) as T ?? new T();

    /// <summary>
	/// Occurs when the application is loading.
	/// </summary>
	private async void OnStartup(object sender, StartupEventArgs e)
	{
		TgAsyncUtils.SetAppType(TgEnumAppType.Async);
		ContextManager.CreateOrConnectDb(true);
		await Host.StartAsync().ConfigureAwait(false);
		TgDesktopUtils.SetClientActions();
	}

	/// <summary>
	/// Occurs when the application is closing.
	/// </summary>
	private async void OnExit(object sender, ExitEventArgs e)
	{
		await Host.StopAsync().ConfigureAwait(true);
		Host.Dispose();
	}

	/// <summary>
	/// Occurs when an exception is thrown by an application but not handled.
	/// </summary>
	private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		// For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
	}
}