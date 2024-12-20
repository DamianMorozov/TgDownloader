// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Common;

/// <summary> Base class for TgViewModel </summary>
[DebuggerDisplay("{ToDebugString()}")]
public partial class TgPageViewModelBase : ObservableRecipient
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private ITgSettingsService _settingsService;
	[ObservableProperty]
	private TgLicenseManagerHelper _licenseManager = TgLicenseManagerHelper.Instance;
	[ObservableProperty]
	private TgExceptionViewModel _exception = new();
	[ObservableProperty]
	private bool _isPageLoad;
	[ObservableProperty]
	private string _connectionDt = string.Empty;
	[ObservableProperty]
	private string _connectionMsg = string.Empty;
	[ObservableProperty]
	private string _stateProxyDt = string.Empty;
	[ObservableProperty]
	private string _stateProxyMsg = string.Empty;
	[ObservableProperty]
	private string _stateSourceDt = string.Empty;
	[ObservableProperty]
	private string _stateSourceMsg = string.Empty;
	[ObservableProperty]
	private XamlRoot? _xamlRootVm;

	public TgPageViewModelBase(ITgSettingsService settingsService)
	{
		SettingsService = settingsService;
		LicenseManager.ActivateLicense(string.Empty, TgResourceExtensions.GetLicenseFreeDescription(),
			TgResourceExtensions.GetLicensePaidDescription(), TgResourceExtensions.GetLicensePremiumDescription());
	}

	#endregion

	#region Public and private methods

	public virtual string ToDebugString() => TgObjectUtils.ToDebugString(this);

	public virtual void OnLoaded(object parameter)
	{
		if (parameter is XamlRoot xamlRoot)
			XamlRootVm = xamlRoot;
	}

	public virtual async Task OnNavigatedToAsync(NavigationEventArgs e)
	{
		await Task.CompletedTask;
	}

	/// <summary> Update state client message </summary>
	public virtual void UpdateStateProxy(string message)
	{
		App.MainWindow.DispatcherQueue.TryEnqueue(() =>
		{
			StateProxyDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
			StateProxyMsg = message;
		});
	}

	/// <summary> Update exception message </summary>
	public virtual async Task UpdateExceptionAsync(Exception ex)
	{
		Exception = new(ex);
		await Task.CompletedTask;
	}

	/// <summary> Update state source message </summary>
	public virtual void UpdateStateSource(long sourceId, int messageId, string message)
	{
		if (sourceId == 0 && messageId == 0)
		{
			UpdateStateMessage(message);
			return;
		}
		App.MainWindow.DispatcherQueue.TryEnqueue(() =>
		{
			StateSourceDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
			StateSourceMsg = $"{sourceId} | {messageId} | {message}";
		});
	}

	/// <summary> Update state message </summary>
	public virtual void UpdateStateMessage(string message)
	{
		App.MainWindow.DispatcherQueue.TryEnqueue(() =>
		{
			StateSourceDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
			StateSourceMsg = message;
		});
	}

	protected async Task ContentDialogAsync(string title, ContentDialogButton defaultButton = ContentDialogButton.Close)
	{
		if (XamlRootVm is null) return;
		ContentDialog dialog = new()
		{
			XamlRoot = XamlRootVm,
			Title = title,
			PrimaryButtonText = TgResourceExtensions.GetYesButton(),
			CloseButtonText = TgResourceExtensions.GetCancelButton(),
			DefaultButton = defaultButton,
		};
		_ = await dialog.ShowAsync();
	}

	protected async Task ContentDialogAsync(Func<Task> task, string title, ContentDialogButton defaultButton = ContentDialogButton.Close)
	{
		if (XamlRootVm is null) return;
		ContentDialog dialog = new()
		{
			XamlRoot = XamlRootVm,
			Title = title,
			PrimaryButtonText = TgResourceExtensions.GetYesButton(),
			CloseButtonText = TgResourceExtensions.GetCancelButton(),
			DefaultButton = defaultButton,
			PrimaryButtonCommand = new AsyncRelayCommand(task)
		};
		_ = await dialog.ShowAsync();
	}

	protected async Task ContentDialogAsync(Action action, string title, ContentDialogButton defaultButton = ContentDialogButton.Close)
	{
		if (XamlRootVm is null) return;
		ContentDialog dialog = new()
		{
			XamlRoot = XamlRootVm,
			Title = title,
			PrimaryButtonText = TgResourceExtensions.GetYesButton(),
			CloseButtonText = TgResourceExtensions.GetCancelButton(),
			DefaultButton = defaultButton,
			PrimaryButtonCommand = new RelayCommand(action)
		};
		_ = await dialog.ShowAsync();
	}

	protected async Task LoadDataAsync(Func<Task> task)
	{
		try
		{
			IsPageLoad = true;
			await Task.Delay(100);
			await task();
		}
		finally
		{
			if (SettingsService.IsExistsAppStorage)
				IsPageLoad = false;
		}
	}

	#endregion
}
