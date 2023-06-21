// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
	private TgSqlContextManagerHelper ContextManager { get; set; } = TgSqlContextManagerHelper.Instance;

	// The.NET Generic Host provides dependency injection, configuration, logging, and other services.
	// https://docs.microsoft.com/dotnet/core/extensions/generic-host
	// https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
	// https://docs.microsoft.com/dotnet/core/extensions/configuration
	// https://docs.microsoft.com/dotnet/core/extensions/logging
	private static readonly IHost Host = Microsoft.Extensions.Hosting.Host
		.CreateDefaultBuilder()
		.ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
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
			services.AddScoped<INavigationWindow, Views.Windows.MainWindow>();
			services.AddScoped<MainWindowViewModel>();
			// Views and ViewModels
			services.AddTransient<TgMenuDashboardPage>();
			services.AddTransient<TgMenuDashboardViewModel>();
			services.AddTransient<TgSettingsPage>();
			services.AddTransient<TgSettingsViewModel>();
			services.AddTransient<TgMenuSourcesPage>();
			services.AddTransient<TgMenuSourcesViewModel>();
			services.AddTransient<TgMenuClientPage>();
			services.AddTransient<TgMenuClientViewModel>();
			services.AddTransient<TgMenuProxiesPage>();
			services.AddTransient<TgMenuProxiesViewModel>();
			// Configuration
			services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
		}).Build();

	/// <summary>
	/// Gets registered service.
	/// </summary>
	/// <typeparam name="T">Type of the service to get.</typeparam>
	/// <returns>Instance of the service or <see langword="null"/>.</returns>
	public static T GetService<T>() where T : class
	{
		return Host.Services.GetService(typeof(T)) as T;
	}

	/// <summary>
	/// Occurs when the application is loading.
	/// </summary>
	private async void OnStartup(object sender, StartupEventArgs e)
	{
		await Host.StartAsync();
		ContextManager.CreateOrConnectDb(true);
	}

	/// <summary>
	/// Occurs when the application is closing.
	/// </summary>
	private async void OnExit(object sender, ExitEventArgs e)
	{
		await Host.StopAsync();

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