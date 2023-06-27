// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Common;

/// <summary>
/// Base class for TgViewModel.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public partial class TgViewBase : ObservableObject
{
	#region Public and private fields, properties, constructor

	protected bool IsInitialized;
	public TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
	public TgSqlContextManagerHelper ContextManager => TgSqlContextManagerHelper.Instance;
	public TgClientHelper TgClient { get; set; } = TgClientHelper.Instance;
	public EnumToBooleanConverter EnumToBooleanConverter { get; set; }
	public bool IsLoad { get; set; }
	public bool IsClientReady { get; set; }
	public bool IsFileSession { get; set; }
	public TgExceptionModel Exception { get; set; }
	public string StateMessage { get; set; }
	public string ExceptionMessage { get; set; }

	public TgViewBase()
	{
		EnumToBooleanConverter = new();
		Exception = new();
		TgClient.UpdateState = UpdateState;
		TgClient.UpdateException = UpdateException;
		StateMessage = string.Empty;
		ExceptionMessage = string.Empty;
		
		if (TgClientUtils.TgClient.IsReady)
		{
			TgClient = TgClientUtils.TgClient;
			IsClientReady = TgClient.IsReady;
		}
	}

	#endregion

	#region Public and private methods

	public override string ToString() => $"{TgLocale} | {ContextManager}";

	/// <summary>
	/// Update state message.
	/// </summary>
	/// <param name="message"></param>
	public void UpdateState(string message)
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			StateMessage = message;
		});
	}

	/// <summary>
	/// Update exception message.
	/// </summary>
	/// <param name="message"></param>
	public void UpdateException(string message)
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			ExceptionMessage = message;
		});
	}

	public bool CheckClientReady()
	{
		if (!TgClient.IsReady)
		{
			UpdateState("Client is not ready!");
			return false;
		}
		return true;
	}

	#endregion
}