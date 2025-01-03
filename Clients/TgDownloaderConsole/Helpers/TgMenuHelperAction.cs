// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
    #region Public and private methods

    public async Task<bool> CheckTgSettingsWithWarningAsync(TgDownloadSettingsViewModel tgDownloadSettings)
    {
        bool result = TgClient is { IsReady: true } && tgDownloadSettings.SourceVm.Dto.IsReady;
        if (!result)
        {
            await ClientConnectAsync(tgDownloadSettings, true);
            result = TgClient is { IsReady: true } && tgDownloadSettings.SourceVm.Dto.IsReady;
            if (!result)
            {
                TgLog.MarkupWarning(TgLocale.TgMustSetSettings);
                Console.ReadKey();
            }
        }
        return result;
    }

    private ProgressColumn[] GetProgressColumns() => new ProgressColumn[]
    {
	    new TaskDescriptionColumn { Alignment = Justify.Left },
	    new ProgressBarColumn { Width = 25 },
	    new PercentageColumn(),
	    new DownloadedColumn { Culture = CultureInfo.InvariantCulture },
	    new TransferSpeedColumn { Culture = CultureInfo.InvariantCulture }
    };

    private const string DownloadingFile = "Downloading file";

	public async Task RunTaskProgressAsync(TgDownloadSettingsViewModel tgDownloadSettings, Func<TgDownloadSettingsViewModel, Task> task,
        bool isSkipCheckTgSettings, bool isScanCount)
    {
        if (!isSkipCheckTgSettings && !await CheckTgSettingsWithWarningAsync(tgDownloadSettings))
            return;

		AnsiConsole.Progress()
            .AutoRefresh(false)
            .AutoClear(true)
            .HideCompleted(true)
            .Columns(GetProgressColumns())
			.Start(context =>
			{
				Stopwatch sw = Stopwatch.StartNew();
				Stopwatch swFileRefresh = Stopwatch.StartNew();
				// ProgressTask
				ProgressTask progressTaskSource = context.AddTask("Downloading source", new()
				{
					AutoStart = false,
					MaxValue = tgDownloadSettings.SourceVm.Dto.Count
				});
				ProgressTask[] progressTaskFiles = new ProgressTask[tgDownloadSettings.CountThreads];
				for (int i = 0; i < tgDownloadSettings.CountThreads; i++)
				{
					progressTaskFiles[i] = context.AddTask(DownloadingFile, new()
					{
						AutoStart = false,
						MaxValue = 0,
					});
					progressTaskFiles[i].Value = 0;
				}
				// Update console title
				async Task UpdateConsoleTitleAsync(string title)
				{
					Console.Title = string.IsNullOrEmpty(title) ? $"{TgConstants.AppTitleConsoleShort}" : $"{TgConstants.AppTitleConsoleShort} {title}";
					await Task.CompletedTask;
				}
				// Update source
				async Task UpdateStateSourceAsync(long sourceId, int messageId, string message)
				{
					if (string.IsNullOrEmpty(message)) return;
					progressTaskSource.Description = $"{message} {messageId} from {tgDownloadSettings.SourceVm.Dto.Count}";
					progressTaskSource.Value = messageId;
					context.Refresh();
					await Task.CompletedTask;
				}
				// Update source
				void UpdateStateSource(long sourceId, int messageId, string message) => 
					UpdateStateSourceAsync(sourceId, messageId, message).GetAwaiter().GetResult();
				// Update download file state
				async Task UpdateStateFileAsync(long sourceId, int messageId, string fileName, long fileSize, long transmitted, long fileSpeed, 
					bool isFileNewDownload, int threadNumber)
				{
					progressTaskSource.Description = $"Read the message {tgDownloadSettings.SourceVm.Dto.FirstId} from {tgDownloadSettings.SourceVm.Dto.Count}";
	                progressTaskSource.Value = tgDownloadSettings.SourceVm.Dto.FirstId;
					// Download job
					if (!string.IsNullOrEmpty(fileName) && !isFileNewDownload && tgDownloadSettings.SourceVm.Dto.Id.Equals(sourceId))
	                {
						if (!progressTaskFiles[threadNumber].IsStarted)
							progressTaskFiles[threadNumber].StartTask();
						progressTaskFiles[threadNumber].Description = fileName;
						progressTaskFiles[threadNumber].Value = transmitted;
						if (!progressTaskFiles[threadNumber].MaxValue.Equals(fileSize))
							progressTaskFiles[threadNumber].MaxValue = fileSize;
						tgDownloadSettings.SourceVm.Dto.FirstId = messageId;
						tgDownloadSettings.SourceVm.Dto.CurrentFileName = fileName;
						tgDownloadSettings.SourceVm.Dto.CurrentFileSize = fileSize;
						tgDownloadSettings.SourceVm.Dto.CurrentFileTransmitted = transmitted;
						tgDownloadSettings.SourceVm.Dto.CurrentFileSpeed = fileSpeed;
	                }
					// Download reset
					else
	                {
						progressTaskFiles[threadNumber].Value = 0;
						progressTaskFiles[threadNumber].MaxValue = fileSize;
						progressTaskFiles[threadNumber].StopTask();
						tgDownloadSettings.SourceVm.Dto.FirstId = messageId;
						tgDownloadSettings.SourceVm.Dto.CurrentFileName = string.Empty;
						tgDownloadSettings.SourceVm.Dto.CurrentFileSize = 0;
						tgDownloadSettings.SourceVm.Dto.CurrentFileTransmitted = 0;
						tgDownloadSettings.SourceVm.Dto.CurrentFileSpeed = 0;
	                }
					// State
					if (!swFileRefresh.IsRunning)
						swFileRefresh.Start();
					else if (swFileRefresh.Elapsed > TimeSpan.FromMilliseconds(1_000))
					{
						swFileRefresh.Reset();
						context.Refresh();
					}
					await Task.CompletedTask;
				}
				// Setup
				TgClient.SetupUpdateTitle(UpdateConsoleTitleAsync);
                TgClient.SetupUpdateStateSource(UpdateStateSourceAsync);
                TgClient.SetupUpdateStateFile(UpdateStateFileAsync);
				// Task
				task(tgDownloadSettings).GetAwaiter().GetResult();
				sw.Stop();
				if (progressTaskSource.IsStarted)
					progressTaskSource.StopTask();
				foreach (var progressTaskFile in progressTaskFiles)
				{
					if (progressTaskFile.IsStarted)
						progressTaskFile.StopTask();
				}
                UpdateStateSource(0, 0, 
                    isScanCount
                        ? $"{GetStatus(sw, tgDownloadSettings.SourceScanCount, tgDownloadSettings.SourceScanCurrent)}"
                        : $"{GetStatus(sw, tgDownloadSettings.SourceVm.Dto.FirstId, tgDownloadSettings.SourceVm.Dto.Count)}");
            });

		TgLog.MarkupLine(TgLocale.WaitDownloadComplete);
		while (!tgDownloadSettings.SourceVm.Dto.IsComplete)
		{
			Console.ReadKey();
			TgLog.MarkupLine(TgLocale.WaitDownloadComplete);
		}
	}

	public async Task RunTaskStatusAsync(TgDownloadSettingsViewModel tgDownloadSettings, Func<TgDownloadSettingsViewModel, Task> task,
		bool isSkipCheckTgSettings, bool isScanCount, bool isWaitComplete)
	{
		if (!isSkipCheckTgSettings && !await CheckTgSettingsWithWarningAsync(tgDownloadSettings))
			return;
		AnsiConsole.Status()
			.AutoRefresh(false)
			.Spinner(Spinner.Known.Star)
			.SpinnerStyle(Style.Parse("green"))
			.Start("Thinking...", (statusContext) =>
			{
				statusContext.Spinner(Spinner.Known.Star);
				statusContext.SpinnerStyle(Style.Parse("green"));
				// Update console title
				async Task UpdateConsoleTitleAsync(string title)
				{
					Console.Title = string.IsNullOrEmpty(title) ? $"{TgConstants.AppTitleConsoleShort}" : $"{TgConstants.AppTitleConsoleShort} {title}";
					await Task.CompletedTask;
				}
				string GetFileStatus(string message = "") =>
					string.IsNullOrEmpty(message)
						? $"{GetStatus(tgDownloadSettings.SourceVm.Dto.Count, tgDownloadSettings.SourceVm.Dto.FirstId)} | " +
						  $"Progress {tgDownloadSettings.SourceVm.Dto.ProgressPercentString}"
						: $"{GetStatus(tgDownloadSettings.SourceVm.Dto.Count, tgDownloadSettings.SourceVm.Dto.FirstId)} | {message} | " +
						  $"Progress {tgDownloadSettings.SourceVm.Dto.ProgressPercentString}";
				// Update source
				async Task UpdateStateSourceAsync(long sourceId, int messageId, string message)
				{
					if (string.IsNullOrEmpty(message))
						return;
					statusContext.Status(TgLog.GetMarkupString(isScanCount
						? $"{GetStatus(tgDownloadSettings.SourceScanCount, messageId),15} | {message,40}"
						: GetFileStatus(message)));
					statusContext.Refresh();
					await Task.CompletedTask;
				}
				// Update contact
				async Task UpdateStateContactAsync(long id, string firstName, string lastName, string userName)
				{
					statusContext.Status(TgLog.GetMarkupString(
						$"{GetStatus(tgDownloadSettings.SourceScanCount, id),15} | {firstName,20} | {lastName,20} | {userName,20}"));
					statusContext.Refresh();
					await Task.CompletedTask;
				}
				// Update story
				async Task UpdateStateStoryAsync(long id, string caption)
				{
					statusContext.Status(TgLog.GetMarkupString(
						$"{GetStatus(tgDownloadSettings.SourceScanCount, id),15} | {caption,30}"));
					statusContext.Refresh();
					await Task.CompletedTask;
				}
				// Update message
				async Task UpdateStateMessageAsync(string message)
				{
					if (string.IsNullOrEmpty(message))
						return;
					statusContext.Status(TgLog.GetMarkupString(message));
					statusContext.Refresh();
					await Task.CompletedTask;
				}
				// Update download file state
				async Task UpdateStateFileAsync(long sourceId, int messageId, string fileName, long fileSize, long transmitted, long fileSpeed, 
					bool isFileNewDownload, int threadNumber)
				{
					// Download job
					if (!string.IsNullOrEmpty(fileName) && !isFileNewDownload && tgDownloadSettings.SourceVm.Dto.Id.Equals(sourceId))
					{
						// Download status job
						tgDownloadSettings.SourceVm.Dto.FirstId = messageId;
						tgDownloadSettings.SourceVm.Dto.CurrentFileName = fileName;
						tgDownloadSettings.SourceVm.Dto.CurrentFileSize = fileSize;
						tgDownloadSettings.SourceVm.Dto.CurrentFileTransmitted = transmitted;
						tgDownloadSettings.SourceVm.Dto.CurrentFileSpeed = fileSpeed;
					}
					// Download reset
					else
					{
						// Download status reset
						tgDownloadSettings.SourceVm.Dto.FirstId = messageId;
						tgDownloadSettings.SourceVm.Dto.CurrentFileName = string.Empty;
						tgDownloadSettings.SourceVm.Dto.CurrentFileSize = 0;
						tgDownloadSettings.SourceVm.Dto.CurrentFileTransmitted = 0;
						tgDownloadSettings.SourceVm.Dto.CurrentFileSpeed = 0;
					}
					// State
					statusContext.Status(TgLog.GetMarkupString($"{GetFileStatus()} | " +
						$"File {fileName} | " +
						$"Transmitted {tgDownloadSettings.SourceVm.Dto.CurrentFileProgressPercentString} | Speed " +
						$"{tgDownloadSettings.SourceVm.Dto.CurrentFileSpeedKBString}"));
					statusContext.Refresh();
					await Task.CompletedTask;
				}
				// Setup
				TgClient.SetupUpdateTitle(UpdateConsoleTitleAsync);
				TgClient.SetupUpdateStateSource(UpdateStateSourceAsync);
				TgClient.SetupUpdateStateContact(UpdateStateContactAsync);
				TgClient.SetupUpdateStateStory(UpdateStateStoryAsync);
				TgClient.SetupUpdateStateFile(UpdateStateFileAsync);
				TgClient.SetupUpdateStateMessage(UpdateStateMessageAsync);
				// Task
				var sw = Stopwatch.StartNew();
				task(tgDownloadSettings).GetAwaiter().GetResult();
				sw.Stop();
				// Update state source
				UpdateStateSourceAsync(0, 0, isScanCount
					? $"{GetStatus(sw, tgDownloadSettings.SourceScanCount, tgDownloadSettings.SourceScanCurrent)}"
					: $"{GetStatus(sw, tgDownloadSettings.SourceVm.Dto.FirstId, tgDownloadSettings.SourceVm.Dto.Count)}")
					.GetAwaiter().GetResult();
			});
		TgLog.MarkupLine(TgLocale.WaitDownloadComplete);
		while (isWaitComplete && !tgDownloadSettings.SourceVm.Dto.IsComplete)
		{
			Console.ReadKey();
			TgLog.MarkupLine(TgLocale.WaitDownloadComplete);
		}
	}

	private string GetStatus(Stopwatch sw, long count, long current) =>
        count is 0 && current is 0
            ? $"{TgLog.GetDtShortStamp()} | {sw.Elapsed} | "
            : $"{TgLog.GetDtShortStamp()} | {sw.Elapsed} | {TgCommonUtils.CalcSourceProgress(count, current):#00.00} % | {TgCommonUtils.GetLongString(current)} / {TgCommonUtils.GetLongString(count)}";

    private string GetStatus(long count, long current) =>
        count is 0 && current is 0
            ? TgLog.GetDtShortStamp()
            : $"{TgLog.GetDtShortStamp()} | {TgCommonUtils.CalcSourceProgress(count, current):#00.00} % | {TgCommonUtils.GetLongString(current)} / {TgCommonUtils.GetLongString(count)}";

    #endregion
}