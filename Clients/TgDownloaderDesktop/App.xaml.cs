// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
	#region Public and private fields, properties, constructor

	// The .NET Generic Host provides dependency injection, configuration, logging, and other services.
	// https://docs.microsoft.com/dotnet/core/extensions/generic-host
	// https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
	// https://docs.microsoft.com/dotnet/core/extensions/configuration
	// https://docs.microsoft.com/dotnet/core/extensions/logging
	public IHost Host { get; }

	public static T GetService<T>() where T : class
	{
		if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
		{
			throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
		}
		return service;
	}

	public static WindowEx MainWindow { get; } = new MainWindow();

	public static UIElement? AppTitlebar { get; set; }

	public App()
	{
		InitializeComponent();
		Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().UseContentRoot(AppContext.BaseDirectory)
			.ConfigureServices((context, services) =>
			{
				// Default Activation Handler
				services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();
				// Other Activation Handlers
				services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();
				// Services
				services.AddSingleton<IAppNotificationService, AppNotificationService>();
				services.AddSingleton<ITgSettingsService, TgSettingsService>();
				services.AddTransient<IWebViewService, WebViewService>();
				services.AddTransient<INavigationViewService, NavigationViewService>();
				services.AddSingleton<IActivationService, ActivationService>();
				services.AddSingleton<IPageService, PageService>();
				services.AddSingleton<INavigationService, NavigationService>();
				// Core Services
				services.AddSingleton<ISampleDataService, SampleDataService>();
				services.AddSingleton<IFileService, FileService>();
				// Views and ViewModels
				services.AddTransient<TgSettingsViewModel>();
				services.AddTransient<TgSettingsPage>();
				services.AddTransient<DataGridViewModel>();
				services.AddTransient<DataGridPage>();
				services.AddTransient<ContentGridDetailViewModel>();
				services.AddTransient<ContentGridDetailPage>();
				services.AddTransient<ContentGridViewModel>();
				services.AddTransient<ContentGridPage>();
				services.AddTransient<ListDetailsViewModel>();
				services.AddTransient<ListDetailsPage>();
				services.AddTransient<WebViewViewModel>();
				services.AddTransient<WebViewPage>();
				services.AddTransient<TgMainViewModel>();
				services.AddTransient<TgMainPage>();
				services.AddTransient<ShellViewModel>();
				services.AddTransient<ShellPage>();
				services.AddTransient<TgClientViewModel>();
				services.AddTransient<TgClientPage>();
				services.AddTransient<TgLoadDataViewModel>();
				services.AddTransient<TgLoadDataPage>();
				services.AddTransient<TgContactsViewModel>();
				services.AddTransient<TgContactsPage>();
				services.AddTransient<TgSourcesViewModel>();
				services.AddTransient<TgSourcesPage>();
				services.AddTransient<TgFiltersViewModel>();
				services.AddTransient<TgFiltersPage>();
				// Configuration
				services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
			})
			.Build();
		App.GetService<IAppNotificationService>().Initialize();
		UnhandledException += App_UnhandledException;
	}

	~App()
	{
		Host.Dispose();
	}

	#endregion

	#region Public and private methods

	private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
	{
		// TODO: Log and handle exceptions as appropriate.
		// https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
		TgDesktopUtils.FileLog(e.Exception);
		// Set a handled exception to prevent the application from terminating
		e.Handled = true;
	}

	protected override async void OnLaunched(LaunchActivatedEventArgs args)
	{
		base.OnLaunched(args);
		App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));
		await App.GetService<IActivationService>().ActivateAsync(args);
	}

	#endregion
}
