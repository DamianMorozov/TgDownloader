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

	public TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;
	protected bool IsInitialized;
	public TgSqlContextManagerHelper ContextManager => TgSqlContextManagerHelper.Instance;
	public EnumToBooleanConverter EnumToBooleanConverter { get; private set; }
	public bool IsFileSession { get; set; }
	public string StateClientMsg { get; set; }
	public string StateClientDt { get; set; }
	public string StateSourceMsg { get; set; }
	public string StateSourceDt { get; set; }
	public string StateExceptionMsg { get; set; }
	public string StateExceptionDt { get; set; }
	public TgExceptionModel Exception { get; set; }
	public Action<Action> UpdateUserControl { get; private set; }
	public Action<Action> UpdatePage { get; private set; }
	public Action<Action> UpdateWindow { get; private set; }
	public Action<Action> UpdateMainWindow { get; private set; }
	public Action<Action> UpdateApplication { get; private set; }

	public TgPageViewModelBase()
	{
		EnumToBooleanConverter = new();
		Exception = new();

		StateClientMsg = string.Empty;
		StateClientDt = string.Empty;
		StateSourceMsg = string.Empty;
		StateSourceDt = string.Empty;
		StateExceptionMsg = string.Empty;
		StateExceptionDt = string.Empty;

		UpdateUserControl = _ => { };
		UpdatePage = _ => { };
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
	public virtual void UpdateStateClient(string message)
	{
			TgDesktopUtils.RunAction(() =>
			{
			StateClientDt = TgDataFormatUtils.DtFormat(DateTime.Now);
			StateClientMsg = message;
			});
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
		TgDesktopUtils.RunAction(() =>
		{
StateExceptionDt = TgDataFormatUtils.DtFormat(DateTime.Now);
		StateExceptionMsg = $"Line {lineNumber} | Member {memberName} | {message}";
		});
	}

	/// <summary>
	/// Update state source message.
	/// </summary>
	/// <param name="sourceId"></param>
	/// <param name="messageId"></param>
	/// <param name="message"></param>
	public virtual void UpdateStateSource(long sourceId, int messageId, string message)
	{
		TgDesktopUtils.RunAction(() =>
				{
StateSourceDt = TgDataFormatUtils.DtFormat(DateTime.Now);
		StateSourceMsg = $"{sourceId} | {messageId} | {message}";
				});
	}

	public bool CheckClientReady()
	{
		if (!TgDesktopUtils.TgClient.CheckClientIsReady())
		{
			TgDesktopUtils.TgClient.UpdateStateClient("Client is not ready!");
			return false;
		}
		return true;
	}

	public void AddUpdateUi(TgEnumUpdateType updateType, Action<Action> updateUi)
{
	switch (updateType)
	{
		case TgEnumUpdateType.Default:
			break;
		case TgEnumUpdateType.UserControl:
			AddUpdateUserControl(updateUi);
			break;
		case TgEnumUpdateType.Page:
			AddUpdatePage(updateUi);
			break;
		case TgEnumUpdateType.Window:
			AddUpdateWindow(updateUi);
			break;
		case TgEnumUpdateType.MainWindow:
			AddUpdateWindow(updateUi);
			break;
		case TgEnumUpdateType.Application:
			AddUpdateApp(updateUi);
			break;
		default:
			throw new ArgumentOutOfRangeException(nameof(updateType), updateType, null);
	}
}

	private void AddUpdateUserControl(Action<Action> updateUi)
{
    if (UpdateUserControl.GetInvocationList().Length == 0)
    {
        UpdateUserControl = updateUi;
        return;
    }
    if (UpdateUserControl.GetInvocationList().Length > 0)
    {
        IEnumerable<string> namesExists = UpdateUserControl.GetInvocationList().Select(item => item.Method.Name);
        if (!namesExists.Contains(updateUi.Method.Name))
            UpdateUserControl += updateUi;
    }
}

	private void AddUpdatePage(Action<Action> updateUi)
{
    if (UpdatePage.GetInvocationList().Length == 0)
    {
        UpdatePage = updateUi;
        return;
    }
    if (UpdatePage.GetInvocationList().Length > 0)
    {
        IEnumerable<string> namesExists = UpdatePage.GetInvocationList().Select(item => item.Method.Name);
        if (!namesExists.Contains(updateUi.Method.Name))
            UpdatePage += updateUi;
    }
}

	private void AddUpdateWindow(Action<Action> updateUi)
{
    if (UpdateWindow.GetInvocationList().Length == 0)
    {
        UpdateWindow = updateUi;
        return;
    }
    if (UpdateWindow.GetInvocationList().Length > 0)
    {
        IEnumerable<string> namesExists = UpdateWindow.GetInvocationList().Select(item => item.Method.Name);
        if (!namesExists.Contains(updateUi.Method.Name))
            UpdateWindow += updateUi;
    }
}

	private void AddUpdateMainWindow(Action<Action> updateUi)
{
    if (UpdateMainWindow.GetInvocationList().Length == 0)
    {
        UpdateMainWindow = updateUi;
        return;
    }
    if (UpdateMainWindow.GetInvocationList().Length > 0)
    {
        IEnumerable<string> namesExists = UpdateMainWindow.GetInvocationList().Select(item => item.Method.Name);
        if (!namesExists.Contains(updateUi.Method.Name))
            UpdateMainWindow += updateUi;
    }
}

	private void AddUpdateApp(Action<Action> updateUi)
{
    if (UpdateApplication.GetInvocationList().Length == 0)
    {
        UpdateApplication = updateUi;
        return;
    }
    if (UpdateApplication.GetInvocationList().Length > 0)
    {
        IEnumerable<string> namesExists = UpdateApplication.GetInvocationList().Select(item => item.Method.Name);
        if (!namesExists.Contains(updateUi.Method.Name))
            UpdateApplication += updateUi;
    }
}

	#endregion
}