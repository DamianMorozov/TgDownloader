﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using AppInstance = Microsoft.Windows.AppLifecycle.AppInstance;
using DispatcherQueuePriority = Microsoft.UI.Dispatching.DispatcherQueuePriority;

namespace TgDownloaderDesktop.Activation;

public class AppNotificationActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
	private readonly INavigationService _navigationService;
	private readonly IAppNotificationService _notificationService;

	public AppNotificationActivationHandler(INavigationService navigationService, IAppNotificationService notificationService)
	{
		_navigationService = navigationService;
		_notificationService = notificationService;
	}

	protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
	{
		return AppInstance.GetCurrent().GetActivatedEventArgs()?.Kind == ExtendedActivationKind.AppNotification;
	}

	protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
	{
		// TODO: Handle notification activations.

		//// // Access the AppNotificationActivatedEventArgs.
		//// var activatedEventArgs = (AppNotificationActivatedEventArgs)AppInstance.GetCurrent().GetActivatedEventArgs().Data;

		//// // Navigate to a specific page based on the notification arguments.
		//// if (_notificationService.ParseArguments(activatedEventArgs.Argument)["action"] == "Settings")
		//// {
		////     // Queue navigation with low priority to allow the UI to initialize.
		////     App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
		////     {
		////         _navigationService.NavigateTo(typeof(TgSettingsViewModel).FullName!);
		////     });
		//// }

		App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
		{
			App.MainWindow.ShowMessageDialogAsync("TODO: Handle notification activations.", "Notification Activation");
		});

		await Task.CompletedTask;
	}
}