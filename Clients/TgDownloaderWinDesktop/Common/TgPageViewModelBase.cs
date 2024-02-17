// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Common;

/// <summary>
/// Base class for TgViewModel.
/// </summary>
[DebuggerDisplay("{ToDebugString()}")]
public partial class TgPageViewModelBase : TgViewModelBase
{
    #region Public and private fields, properties, constructor

    public TgSqlContextManagerHelper ContextManager => TgSqlContextManagerHelper.Instance;
    protected bool IsInitialized;
    public Action<Action> UpdateApplication { get; private set; }
    public Action<Action> UpdateMainWindow { get; private set; }
    public Action<Action> UpdateUserControl { get; private set; }
    public Action<Action> UpdateWindow { get; private set; }
    public bool IsFileSession { get; set; }
    public EnumToBooleanConverter EnumToBooleanConverter { get; private set; }
    public string StateConnectDt { get; set; }
    public string StateConnectMsg { get; set; }
    public string StateExceptionDt { get; set; }
    public string StateExceptionMsg { get; set; }
    public string StateProxyDt { get; set; }
    public string StateProxyMsg { get; set; }
    public string StateSourceDt { get; set; }
    public string StateSourceMsg { get; set; }
    public TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
    public TgExceptionModel Exception { get; set; }
    public Dispatcher Dispatcher { get; set; }

    public TgPageViewModelBase()
    {
        EnumToBooleanConverter = new();
        Exception = new();

        StateConnectDt = string.Empty;
        StateConnectMsg = string.Empty;
        StateExceptionDt = string.Empty;
        StateExceptionMsg = string.Empty;
        StateProxyDt = string.Empty;
        StateProxyMsg = string.Empty;
        StateSourceDt = string.Empty;
        StateSourceMsg = string.Empty;
        Dispatcher = Dispatcher.CurrentDispatcher;

        UpdateUserControl = _ => { };
        UpdateWindow = _ => { };
        UpdateMainWindow = _ => { };
        UpdateApplication = _ => { };
    }

    #endregion

    #region Public and private methods

    public override string ToString() => $"{TgDesktopUtils.TgLocale} | {ContextManager}";

    protected virtual void InitializeViewModel()
    {
        IsInitialized = true;
    }

    protected virtual async Task InitializeViewModelAsync()
    {
        IsInitialized = true;
    }

    /// <summary>
    /// Update state client message.
    /// </summary>
    /// <param name="message"></param>
    public virtual async Task UpdateStateConnectAsync(string message)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
	        await Task.Delay(TimeSpan.FromMilliseconds(1));
			StateConnectDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
            StateConnectMsg = message;
        }, false);
    }

    /// <summary>
    /// Update state client message.
    /// </summary>
    /// <param name="message"></param>
    public virtual async Task UpdateStateProxyAsync(string message)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
	        await Task.Delay(TimeSpan.FromMilliseconds(1));
			StateProxyDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
            StateProxyMsg = message;
        }, false);
    }

    /// <summary>
    /// Update exception message.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <param name="memberName"></param>
    /// <param name="message"></param>
    public virtual async Task UpdateStateExceptionAsync(string filePath, int lineNumber, string memberName, string message)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
	        await Task.Delay(TimeSpan.FromMilliseconds(1));
			StateExceptionDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
            StateExceptionMsg = $"Line {lineNumber} | Member {memberName} | {message}";
        }, false);
    }

    /// <summary>
    /// Update exception message.
    /// </summary>
    /// <param name="message"></param>
    public virtual async Task UpdateStateExceptionShortAsync(string message)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
	        await Task.Delay(TimeSpan.FromMilliseconds(1));
			StateExceptionDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
            StateExceptionMsg = message;
        }, false);
    }

    /// <summary>
    /// Update state source message.
    /// </summary>
    /// <param name="sourceId"></param>
    /// <param name="messageId"></param>
    /// <param name="message"></param>
    public virtual async Task UpdateStateSourceAsync(long sourceId, int messageId, string message)
    {
	    if (sourceId == 0 && messageId == 0)
	    {
		    await UpdateStateMessageAsync(message);
            return;
	    }
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
	        await Task.Delay(TimeSpan.FromMilliseconds(1));
			StateSourceDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
            StateSourceMsg = $"{sourceId} | {messageId} | {message}";
        }, false);
    }

    /// <summary>
    /// Update state message.
    /// </summary>
    /// <param name="message"></param>
    public virtual async Task UpdateStateMessageAsync(string message)
    {
        await TgDesktopUtils.RunFuncAsync(this, async () =>
        {
	        await Task.Delay(TimeSpan.FromMilliseconds(1));
			StateSourceDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
            StateSourceMsg = message;
        }, false);
    }

    public bool CheckClientReady() => TgDesktopUtils.TgClient.CheckClientIsReady();

    #endregion
}