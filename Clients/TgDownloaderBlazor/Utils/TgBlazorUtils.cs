// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Utils;

public static class TgBlazorUtils
{
	#region Public and private methods

	public static async Task RunFuncAsync(Func<Task> action)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

		try
		{
			await action();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
	}

	public static async Task RunFuncAsync(Func<Task> action, Action actionFinally)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

		try
		{
			await action();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
		finally
		{
			actionFinally();
		}
	}

	public static async Task RunActionAsync(Action action)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

		try
		{
			action();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
	}

	public static async Task RunActionAsync(Action action, Action actionFinally)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

		try
		{
			action();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
		finally
		{
			actionFinally();
		}
	}

	#endregion
}