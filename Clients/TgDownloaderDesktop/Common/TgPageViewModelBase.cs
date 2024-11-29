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
	private bool _isLoad;
	[ObservableProperty]
	private string _stateConnectDt = string.Empty;
	[ObservableProperty]
	private string _stateConnectMsg = string.Empty;
	[ObservableProperty]
	private string _stateExceptionDt = string.Empty;
	[ObservableProperty]
	private string _stateExceptionMsg = string.Empty;
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
		IsLoad = true;
		if (parameter is XamlRoot xamlRoot)
			XamlRootVm = xamlRoot;
	}

	/// <summary> Update state client message </summary>
	public virtual void UpdateStateConnect(string message)
	{
		App.MainWindow.DispatcherQueue.TryEnqueue(() =>
		{
			StateConnectDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
			StateConnectMsg = message;
		});
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
	public virtual void UpdateStateException(string filePath, int lineNumber, string memberName, string message)
	{
		App.MainWindow.DispatcherQueue.TryEnqueue(() =>
		{
			StateExceptionDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
			StateExceptionMsg = $"Line {lineNumber} | Member {memberName} | {message}";
		});
	}

	/// <summary> Update exception message </summary>
	public virtual void UpdateStateExceptionShort(string message)
	{
		App.MainWindow.DispatcherQueue.TryEnqueue(() =>
		{
			StateExceptionDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
			StateExceptionMsg = message;
		});
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
		await Task.CompletedTask;
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
		await Task.CompletedTask;
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
		await Task.CompletedTask;
	}

	#endregion
}
