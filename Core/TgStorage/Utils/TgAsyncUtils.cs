// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgStorage.Utils;

/// <summary>
/// Async utilities.
/// </summary>
public static class TgAsyncUtils
{
	#region Public and private fields, properties, constructor

	public static TgEnumAppType AppType { get; private set; } = TgEnumAppType.Default;

	#endregion

	#region Public and private methods

	public static void SetAppType(TgEnumAppType appType) => AppType = appType;

	public static T GetTaskResult<T>(Func<Task<T>> job, TgEnumAppType appType = TgEnumAppType.Default)
	{
		if (appType == TgEnumAppType.Default)
			appType = AppType;
		switch (appType)
		{
			case TgEnumAppType.Async:
				Task<T> task = Task.Run(async () =>
				{
					await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
					return await job().ConfigureAwait(false);
				});
				task.ConfigureAwait(false);
				return task.GetAwaiter().GetResult();
			case TgEnumAppType.Sync:
				return job().ConfigureAwait(true).GetAwaiter().GetResult();
			default:
				throw new ArgumentException(null, nameof(appType));
		}
	}

	public static void ExecTaskAsync(Func<Task> job, bool isWait, TgEnumAppType appType = TgEnumAppType.Default)
	{
		if (appType == TgEnumAppType.Default)
			appType = AppType;
		switch (appType)
		{
			case TgEnumAppType.Async:
				Task task = Task.Run(async () =>
				{
					await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
					await job().ConfigureAwait(false);
				});
				task.ConfigureAwait(false);
				if (isWait)
					task.GetAwaiter().GetResult();
				break;
			case TgEnumAppType.Sync:
				job().ConfigureAwait(true);
				break;
			default:
				throw new ArgumentException(null, nameof(appType));
		}
	}

	public static void ExecTask(Task job, bool isWait, TgEnumAppType appType = TgEnumAppType.Default)
	{
		if (appType == TgEnumAppType.Default)
			appType = AppType;
		switch (appType)
		{
			case TgEnumAppType.Async:
				Task task = Task.Run(async () =>
				{
					await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
					await job.ConfigureAwait(false);
				});
				task.ConfigureAwait(false);
				if (isWait)
					task.GetAwaiter().GetResult();
				break;
			case TgEnumAppType.Sync:
				job.ConfigureAwait(true);
				break;
			default:
				throw new ArgumentException(null, nameof(appType));
		}
	}

	public static void ExecAction(Action action, TgEnumAppType appType = TgEnumAppType.Default)
	{
		if (appType == TgEnumAppType.Default)
			appType = AppType;
		switch (appType)
		{
			case TgEnumAppType.Async:
				Task task = Task.Run(async () =>
				{
					await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
					action();
				});
				task.ConfigureAwait(false);
				break;
			case TgEnumAppType.Sync:
				action();
				break;
			default:
				throw new ArgumentException(null, nameof(appType));
		}
	}

	public static void AddAction(ref Action actionMain, Action actionNew)
{
    if (actionMain.GetInvocationList().Length == 0)
    {
        actionMain = actionNew;
        return;
    }
    if (actionMain.GetInvocationList().Length > 0)
    {
        IEnumerable<string> namesExists = actionMain.GetInvocationList().Select(item => item.Method.Name);
        if (!namesExists.Contains(actionNew.Method.Name))
            actionMain += actionNew;
    }
}

	#endregion
}