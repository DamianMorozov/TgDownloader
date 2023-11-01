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
    public string StateMessageDt { get; set; }
    public string StateMessageMsg { get; set; }
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
        StateMessageDt = string.Empty;
        StateMessageMsg = string.Empty;
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

    /// <summary>
    /// Update state client message.
    /// </summary>
    /// <param name="message"></param>
    public virtual void UpdateStateConnect(string message)
    {
        TgDesktopUtils.RunAction(this, () =>
        {
            StateConnectDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
            StateConnectMsg = message;
        }, false);
    }

    /// <summary>
    /// Update state client message.
    /// </summary>
    /// <param name="message"></param>
    public virtual void UpdateStateProxy(string message)
    {
        TgDesktopUtils.RunAction(this, () =>
        {
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
    public virtual void UpdateStateException(string filePath, int lineNumber, string memberName, string message)
    {
        TgDesktopUtils.RunAction(this, () =>
        {
            StateExceptionDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
            StateExceptionMsg = $"Line {lineNumber} | Member {memberName} | {message}";
        }, false);
    }

    /// <summary>
    /// Update state source message.
    /// </summary>
    /// <param name="message"></param>
    public virtual void UpdateStateMessage(string message)
    {
        TgDesktopUtils.RunAction(this, () =>
        {
            StateMessageDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
            StateMessageMsg = message;
        }, false);
    }

    /// <summary>
    /// Update state source message.
    /// </summary>
    /// <param name="sourceId"></param>
    /// <param name="messageId"></param>
    /// <param name="message"></param>
    public virtual void UpdateStateSource(long sourceId, int messageId, string message)
    {
        TgDesktopUtils.RunAction(this, () =>
        {
            StateSourceDt = TgDataFormatUtils.GetDtFormat(DateTime.Now);
            StateSourceMsg = $"{sourceId} | {messageId} | {message}";
        }, false);
    }

    public bool CheckClientReady() => TgDesktopUtils.TgClient.CheckClientIsReady();

    //private void AddUpdateUserControl(Action<Action> updateUi)
    //{
    //    if (UpdateUserControl.GetInvocationList().Length == 0)
    //    {
    //        UpdateUserControl = updateUi;
    //        return;
    //    }
    //    if (UpdateUserControl.GetInvocationList().Length > 0)
    //    {
    //        IEnumerable<string> namesExists = UpdateUserControl.GetInvocationList().Select(item => item.Method.Name);
    //        if (!namesExists.Contains(updateUi.Method.Name))
    //            UpdateUserControl += updateUi;
    //    }
    //}

    //private void AddUpdatePage(Action<Action> updateUi)
    //{
    //    if (UpdatePage.GetInvocationList().Length == 0)
    //    {
    //        UpdatePage = updateUi;
    //        return;
    //    }
    //    if (UpdatePage.GetInvocationList().Length > 0)
    //    {
    //        IEnumerable<string> namesExists = UpdatePage.GetInvocationList().Select(item => item.Method.Name);
    //        if (!namesExists.Contains(updateUi.Method.Name))
    //            UpdatePage += updateUi;
    //    }
    //}

    //private void AddUpdateWindow(Action<Action> updateUi)
    //{
    //    if (UpdateWindow.GetInvocationList().Length == 0)
    //    {
    //        UpdateWindow = updateUi;
    //        return;
    //    }
    //    if (UpdateWindow.GetInvocationList().Length > 0)
    //    {
    //        IEnumerable<string> namesExists = UpdateWindow.GetInvocationList().Select(item => item.Method.Name);
    //        if (!namesExists.Contains(updateUi.Method.Name))
    //            UpdateWindow += updateUi;
    //    }
    //}

    //private void AddUpdateMainWindow(Action<Action> updateUi)
    //{
    //    if (UpdateMainWindow.GetInvocationList().Length == 0)
    //    {
    //        UpdateMainWindow = updateUi;
    //        return;
    //    }
    //    if (UpdateMainWindow.GetInvocationList().Length > 0)
    //    {
    //        IEnumerable<string> namesExists = UpdateMainWindow.GetInvocationList().Select(item => item.Method.Name);
    //        if (!namesExists.Contains(updateUi.Method.Name))
    //            UpdateMainWindow += updateUi;
    //    }
    //}

    //private void AddUpdateApp(Action<Action> updateUi)
    //{
    //    if (UpdateApplication.GetInvocationList().Length == 0)
    //    {
    //        UpdateApplication = updateUi;
    //        return;
    //    }
    //    if (UpdateApplication.GetInvocationList().Length > 0)
    //    {
    //        IEnumerable<string> namesExists = UpdateApplication.GetInvocationList().Select(item => item.Method.Name);
    //        if (!namesExists.Contains(updateUi.Method.Name))
    //            UpdateApplication += updateUi;
    //    }
    //}

    #endregion
}