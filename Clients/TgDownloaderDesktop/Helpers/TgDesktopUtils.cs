﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Helpers;

/// <summary> Desktop utils </summary>
public static class TgDesktopUtils
{
    #region Public and private fields, properties, constructor

    public static TgClientHelper TgClient => TgClientHelper.Instance;
	//public static TgClientViewModel TgClientVm { get; } = App.GetService<TgClientViewModel>();
	public static string BaseDirectory = AppContext.BaseDirectory;
	public static string LocalFolder = ApplicationData.Current.LocalFolder.Path;
	public static string InstalledLocation = Package.Current.InstalledLocation.Path;
	public static string TgDownloaderLogName => "TgDownloader.log";

	#endregion

	#region Public and private methods

	//public static void RunAction(TgPageViewModelBase viewModel, Action action, bool isUpdateLoad)
	//{
	//    void Job()
	//    {
	//        if (isUpdateLoad)
	//            viewModel.IsLoad = true;
	//        TgClientViewModel.Exception.Clear();
	//        action();
	//    }

	//    void JobFinally()
	//    {
	//        viewModel.IsLoad = false;
	//    }

	//    try
	//    {
	//     App.MainWindow.DispatcherQueue.TryEnqueue(Job);
	//    }
	//    catch (Exception ex)
	//    {
	//     App.MainWindow.DispatcherQueue.TryEnqueue(() => { TgClientViewModel.Exception.Set(ex); });
	//    }
	//    finally
	//    {
	//        if (isUpdateLoad)
	//        {
	//         App.MainWindow.DispatcherQueue.TryEnqueue(JobFinally);
	//        }
	//    }
	//}

	//public static async Task RunActionAsync(TgPageViewModelBase viewModel, Action action, bool isUpdateLoad)
	//{
	//    await Task.Delay(1);

	//    void Job()
	//    {
	//        if (isUpdateLoad)
	//            viewModel.IsLoad = true;
	//        TgClientVm.Exception.Clear();
	//        action();
	//    }

	//    void JobFinally()
	//    {
	//        viewModel.IsLoad = false;
	//    }

	//    try
	//    {
	//        if (viewModel.Dispatcher.CheckAccess())
	//        {
	//            Job();
	//        }
	//        else
	//        {
	//            viewModel.Dispatcher.Invoke(() =>
	//            {
	//                Job();
	//            });
	//        }
	//    }
	//    catch (Exception ex)
	//    {
	//        if (viewModel.Dispatcher.CheckAccess())
	//            TgClientVm.Exception.Set(ex);
	//        else
	//            viewModel.Dispatcher.Invoke(() => { TgClientVm.Exception.Set(ex); });
	//    }
	//    finally
	//    {
	//        if (isUpdateLoad)
	//        {
	//            if (viewModel.Dispatcher.CheckAccess())
	//                JobFinally();
	//            else
	//                viewModel.Dispatcher.Invoke(() =>
	//                {
	//                    JobFinally();
	//                });
	//        }
	//    }
	//}

	//public static async Task RunAction2Async(TgPageViewModelBase viewModel, Action action, bool isUpdateLoad)
	//{
	//    await Task.Delay(1);

	//    async Task Job()
	//    {
	//        await Task.Delay(1);
	//        if (isUpdateLoad)
	//            viewModel.IsLoad = true;
	//        TgClientVm.Exception.Clear();
	//        action();
	//    }

	//    async Task JobFinally()
	//    {
	//        await Task.Delay(1);
	//        viewModel.IsLoad = false;
	//    }

	//    try
	//    {
	//        if (viewModel.Dispatcher.CheckAccess())
	//        {
	//            Job();
	//        }
	//        else
	//        {
	//            viewModel.Dispatcher.InvokeAsync(async () =>
	//            {
	//                await Job();
	//            });
	//        }
	//    }
	//    catch (Exception ex)
	//    {
	//        if (viewModel.Dispatcher.CheckAccess())
	//            TgClientVm.Exception.Set(ex);
	//        else
	//            viewModel.Dispatcher.InvokeAsync(async () => { TgClientVm.Exception.Set(ex); });
	//    }
	//    finally
	//    {
	//        if (isUpdateLoad)
	//        {
	//            if (viewModel.Dispatcher.CheckAccess())
	//                JobFinally();
	//            else
	//                viewModel.Dispatcher.InvokeAsync(async () =>
	//                {
	//                    await JobFinally();
	//                });
	//        }
	//    }
	//}

	//public static async Task RunFuncAsync(TgPageViewModelBase viewModel, Func<Task> action, bool isUpdateLoad)
 //   {
 //       async Task Job()
 //       {
 //           if (isUpdateLoad)
 //               viewModel.IsLoad = true;
 //           //TgClientViewModel.Exception.Clear();
 //           await action();
 //       }

 //       void JobFinally()
 //       {
 //           viewModel.IsLoad = false;
 //       }

 //       try
 //       {
	//        App.MainWindow.DispatcherQueue.TryEnqueue(async () => await Job());
 //       }
 //       catch (Exception ex)
 //       {
	//        //App.MainWindow.DispatcherQueue.TryEnqueue(() => TgClientViewModel.Exception.Set(ex));
 //       }
 //       finally
 //       {
 //           if (isUpdateLoad)
 //           {
	//            App.MainWindow.DispatcherQueue.TryEnqueue(JobFinally);
 //           }
 //           await Task.CompletedTask;
 //       }
 //   }

	private static void FileLogCore(string message)
    {
	    var storageFolder = StorageFolder.GetFolderFromPathAsync(LocalFolder).GetAwaiter().GetResult();
	    var storageFile = storageFolder.CreateFileAsync(TgDownloaderLogName, CreationCollisionOption.OpenIfExists).GetAwaiter().GetResult();
	    var logMessage = $"[{DateTime.Now}] {message}";
	    using var stream = storageFile.OpenStreamForWriteAsync().GetAwaiter().GetResult();
	    stream.Seek(0, SeekOrigin.End);
	    using var writer = new StreamWriter(stream);
	    writer.WriteLine(logMessage);
    }

	private static void AppendCallerInfo(this StringBuilder sb, string filePath, int lineNumber, string memberName)
	{
		sb.AppendLine($"[{DateTime.Now}] {nameof(filePath)}: {Path.GetFileName(filePath)}");
		sb.AppendLine($"[{DateTime.Now}] {nameof(lineNumber)}: {lineNumber}");
		sb.AppendLine($"[{DateTime.Now}] {nameof(memberName)}: {memberName}");
	}

	public static void FileLog(Exception ex, string message = "", 
		[CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
	{
		var sb = new StringBuilder();
		sb.AppendCallerInfo(filePath, lineNumber, memberName);
		sb.AppendLine($"[{DateTime.Now}] {message}");
		sb.AppendLine($"[{DateTime.Now}] Exception: {ex.Message}");
		if (ex.InnerException is not null)
			sb.AppendLine($"[{DateTime.Now}] Exception: {ex.InnerException.Message}");
		sb.AppendLine($"[{DateTime.Now}] Stack Trace: {ex.StackTrace}");
		FileLogCore(sb.ToString());
	}

	public static void FileLog(string message,
		[CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
	{
		var sb = new StringBuilder();
		sb.AppendCallerInfo(filePath, lineNumber, memberName);
		sb.AppendLine($"[{DateTime.Now}] {message}");
		FileLogCore(sb.ToString());
	}

	private static async Task FileLogCoreAsync(string message)
    {
	    var storageFolder = await StorageFolder.GetFolderFromPathAsync(LocalFolder);
	    var storageFile = await storageFolder.CreateFileAsync(TgDownloaderLogName, CreationCollisionOption.OpenIfExists);
	    var logMessage = $"[{DateTime.Now}] {message}";
	    await using var stream = await storageFile.OpenStreamForWriteAsync();
	    stream.Seek(0, SeekOrigin.End);
	    await using var writer = new StreamWriter(stream);
	    await writer.WriteLineAsync(logMessage);
    }

	public static async Task FileLogAsync(Exception ex, string message = "", 
		[CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
	{
		var sb = new StringBuilder();
		sb.AppendCallerInfo(filePath, lineNumber, memberName);
		sb.AppendLine($"[{DateTime.Now}] {message}");
		sb.AppendLine($"[{DateTime.Now}] Exception: {ex.Message}");
		if (ex.InnerException is not null)
			sb.AppendLine($"[{DateTime.Now}] Exception: {ex.InnerException.Message}");
		sb.AppendLine($"[{DateTime.Now}] Stack Trace: {ex.StackTrace}");
		await FileLogCoreAsync(sb.ToString());
	}

	public static async Task FileLogAsync(string message,
		[CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
	{
		var sb = new StringBuilder();
		sb.AppendCallerInfo(filePath, lineNumber, memberName);
		sb.AppendLine($"[{DateTime.Now}] {message}");
		await FileLogCoreAsync(sb.ToString());
	}

	public static async Task<bool> CheckFileStorageExistsAsync(string fullPath)
	{
		var result = await CheckFileStorageExistsCoreAsync(fullPath);
		if (result)
			return true;
		return await CheckFileStorageExistsCoreAsync(fullPath);
	}

	private static async Task<bool> CheckFileStorageExistsCoreAsync(string fullPath)
	{
        try
        {
		    var folder = Path.GetDirectoryName(fullPath) ?? string.Empty;
		    if (string.IsNullOrEmpty(folder))
			    return false;
		    var fileName = Path.GetFileName(fullPath);
			var storageFolder = await StorageFolder.GetFolderFromPathAsync(folder);
            if (storageFolder is null)
                return false;
            var storageFile = await storageFolder.GetFileAsync(fileName);
            return storageFile.IsAvailable;
        }
        catch (Exception ex)
        {
			await FileLogAsync(ex);
        }
        return false;
	}

	public static async Task<bool> DeleteFileStorageExistsAsync(string fullPath)
	{
		var result = await DeleteFileStorageExistsCoreAsync(fullPath);
		if (result)
			return true;
		return await DeleteFileStorageExistsCoreAsync(fullPath);
	}

	public static async Task<bool> DeleteFileStorageExistsCoreAsync(string fullPath)
	{
		var folder = string.Empty;
		var fileName = string.Empty;
		try
		{
			folder = Path.GetDirectoryName(fullPath) ?? string.Empty;
			if (string.IsNullOrEmpty(folder))
				return false;
			fileName  = Path.GetFileName(fullPath);
			var storageFolder = await StorageFolder.GetFolderFromPathAsync(folder);
			if (storageFolder is null)
				return false;
			var storageFile = await storageFolder.GetFileAsync(fileName);
			if (storageFile.IsAvailable)
				await storageFile.DeleteAsync();
		}
		catch (Exception ex)
		{
#if DEBUG
			await FileLogAsync(ex, $"{Path.Combine(folder, fileName)}");
#endif
		}
		return false;
	}

	public static bool CheckFileStorageExists(string fullPath)
	{
		var result = CheckFileStorageCoreExists(fullPath);
		if (result) return true;
		return CheckFileStorageCoreExists(fullPath);
	}

	private static bool CheckFileStorageCoreExists(string fullPath)
	{
		var folder = string.Empty;
		var fileName = string.Empty;
		try
        {
			folder = Path.GetDirectoryName(fullPath) ?? string.Empty;
			if (string.IsNullOrEmpty(folder))
				return false;
			fileName  = Path.GetFileName(fullPath);
			var storageFolder = StorageFolder.GetFolderFromPathAsync(folder).GetAwaiter().GetResult();
            if (storageFolder is null)
                return false;
            var storageFile = storageFolder.GetFileAsync(fileName).GetAwaiter().GetResult();
            return storageFile.IsAvailable;
        }
        catch (Exception ex)
        {
#if DEBUG
	        FileLog(ex, $"{Path.Combine(folder, fileName)}");
#endif
		}
		return false;
	}

	#endregion
}