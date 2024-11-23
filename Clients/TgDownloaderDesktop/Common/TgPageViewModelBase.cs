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
	
	private string _appEfStorage = default!;
	public string AppEfStorage
	{
		get => _appEfStorage;
		set
		{
			if (SetProperty(ref _appEfStorage, value))
			{
				SettingsService.SetAppEfStorageAsync(AppEfStorage).ConfigureAwait(false);
			}
			IsExistsEfStorage = File.Exists(value);
		}
	}
	[ObservableProperty]
	private bool _isExistsEfStorage;
	private string _appTgSession = default!;
	public string AppTgSession
	{
		get => _appTgSession;
		set
		{
			if (SetProperty(ref _appTgSession, value))
			{
				SettingsService.SetAppTgSessionAsync(AppTgSession).ConfigureAwait(false);
			}
			IsExistsTgSession = File.Exists(value);
			OnPropertyChanged();
		}
	}
	[ObservableProperty]
	private bool _isExistsTgSession;

	public bool IsLoad { get; set; }
	public string StateConnectDt { get; set; } = string.Empty;
	public string StateConnectMsg { get; set; } = string.Empty;
	public string StateExceptionDt { get; set; } = string.Empty;
	public string StateExceptionMsg { get; set; } = string.Empty;
	public string StateProxyDt { get; set; } = string.Empty;
	public string StateProxyMsg { get; set; } = string.Empty;
	public string StateSourceDt { get; set; } = string.Empty;
	public string StateSourceMsg { get; set; } = string.Empty;
	public TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
	public static TgExceptionModel Exception { get; set; } = new();
	[ObservableProperty]
	private XamlRoot? _xamlRootVm;

	public TgPageViewModelBase(ITgSettingsService settingsService)
	{
		SettingsService = settingsService;
	}

	#endregion

	#region Public and private methods

	public virtual string ToDebugString() => $"{TgCommonUtils.GetIsLoad(IsLoad)}";

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

	#endregion
}
