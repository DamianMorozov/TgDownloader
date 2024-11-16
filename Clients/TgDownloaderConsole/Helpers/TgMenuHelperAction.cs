// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
    #region Public and private methods

    public async Task<bool> CheckTgSettingsWithWarningAsync(TgDownloadSettingsViewModel tgDownloadSettings)
    {
        bool result = TgClient is { IsReady: true } && tgDownloadSettings.SourceVm.IsReady;
        if (!result)
        {
            await ClientConnectAsync(tgDownloadSettings, true);
            result = TgClient is { IsReady: true } && tgDownloadSettings.SourceVm.IsReady;
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

	public async Task RunActionProgressAsync(TgDownloadSettingsViewModel tgDownloadSettings, Action<TgDownloadSettingsViewModel> action,
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
					MaxValue = tgDownloadSettings.SourceVm.SourceLastId
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
					await Task.Delay(1);
					Console.Title = string.IsNullOrEmpty(title) ? $"{TgConstants.AppTitleConsoleShort}" : $"{TgConstants.AppTitleConsoleShort} {title}";
				}
				// Update source
				async Task UpdateStateSourceAsync(long sourceId, int messageId, string message)
				{
					await Task.Delay(1);
					if (string.IsNullOrEmpty(message)) return;
					progressTaskSource.Description = $"{message} {messageId} from {tgDownloadSettings.SourceVm.SourceLastId}";
					progressTaskSource.Value = messageId;
					context.Refresh();
				}
				// Update source
				void UpdateStateSource(long sourceId, int messageId, string message) => 
					UpdateStateSourceAsync(sourceId, messageId, message).GetAwaiter().GetResult();
				// Update download file state
				async Task UpdateStateFileAsync(long sourceId, int messageId, string fileName, long fileSize, long transmitted, long fileSpeed, 
					bool isFileNewDownload, int threadNumber)
				{
					await Task.Delay(1);
	                progressTaskSource.Description = $"Read the message {tgDownloadSettings.SourceVm.SourceFirstId} from {tgDownloadSettings.SourceVm.SourceLastId}";
	                progressTaskSource.Value = tgDownloadSettings.SourceVm.SourceFirstId;
					// Download job
					if (!string.IsNullOrEmpty(fileName) && !isFileNewDownload && tgDownloadSettings.SourceVm.SourceId.Equals(sourceId))
	                {
						if (!progressTaskFiles[threadNumber].IsStarted)
							progressTaskFiles[threadNumber].StartTask();
						progressTaskFiles[threadNumber].Description = fileName;
						progressTaskFiles[threadNumber].Value = transmitted;
						if (!progressTaskFiles[threadNumber].MaxValue.Equals(fileSize))
							progressTaskFiles[threadNumber].MaxValue = fileSize;
						tgDownloadSettings.SourceVm.SourceFirstId = messageId;
						tgDownloadSettings.SourceVm.CurrentFileName = fileName;
						tgDownloadSettings.SourceVm.CurrentFileSize = fileSize;
						tgDownloadSettings.SourceVm.CurrentFileTransmitted = transmitted;
						tgDownloadSettings.SourceVm.CurrentFileSpeed = fileSpeed;
	                }
					// Download reset
					else
	                {
						progressTaskFiles[threadNumber].Value = 0;
						progressTaskFiles[threadNumber].MaxValue = fileSize;
						progressTaskFiles[threadNumber].StopTask();
						tgDownloadSettings.SourceVm.SourceFirstId = messageId;
						tgDownloadSettings.SourceVm.CurrentFileName = string.Empty;
						tgDownloadSettings.SourceVm.CurrentFileSize = 0;
						tgDownloadSettings.SourceVm.CurrentFileTransmitted = 0;
						tgDownloadSettings.SourceVm.CurrentFileSpeed = 0;
	                }
					// State
					if (!swFileRefresh.IsRunning)
						swFileRefresh.Start();
					else if (swFileRefresh.Elapsed > TimeSpan.FromMilliseconds(1_000))
					{
						swFileRefresh.Reset();
						context.Refresh();
					}
				}
				// Setup
				TgClient.SetupUpdateTitle(UpdateConsoleTitleAsync);
                TgClient.SetupUpdateStateSource(UpdateStateSourceAsync);
                TgClient.SetupUpdateStateFile(UpdateStateFileAsync);
				// Action
				action(tgDownloadSettings);
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
                        ? $"{GetStatus(sw, tgDownloadSettings.SourceVm.SourceScanCount, tgDownloadSettings.SourceVm.SourceScanCurrent)}"
                        : $"{GetStatus(sw, tgDownloadSettings.SourceVm.SourceFirstId, tgDownloadSettings.SourceVm.SourceLastId)}");
            });

		TgLog.MarkupLine(TgLocale.WaitDownloadComplete);
		while (!tgDownloadSettings.SourceVm.IsComplete)
		{
			Console.ReadKey();
			TgLog.MarkupLine(TgLocale.WaitDownloadComplete);
		}
	}

	public async Task RunActionStatusAsync(TgDownloadSettingsViewModel tgDownloadSettings, Action<TgDownloadSettingsViewModel> action,
		bool isSkipCheckTgSettings, bool isScanCount, bool isWaitComplete)
	{
		if (!isSkipCheckTgSettings && !await CheckTgSettingsWithWarningAsync(tgDownloadSettings))
			return;
		AnsiConsole.Status()
			.AutoRefresh(false)
			.Spinner(Spinner.Known.Star)
			.SpinnerStyle(Style.Parse("green"))
			.Start("Thinking...", statusContext =>
			{
				statusContext.Spinner(Spinner.Known.Star);
				statusContext.SpinnerStyle(Style.Parse("green"));
				// Update console title.
				async Task UpdateConsoleTitleAsync(string title)
				{
					await Task.Delay(1);
					Console.Title = string.IsNullOrEmpty(title) ? $"{TgConstants.AppTitleConsoleShort}" : $"{TgConstants.AppTitleConsoleShort} {title}";
				}
				string GetFileStatus(string message = "") =>
					string.IsNullOrEmpty(message)
						? $"{GetStatus(tgDownloadSettings.SourceVm.SourceLastId, tgDownloadSettings.SourceVm.SourceFirstId)} | " +
						  $"Progress {tgDownloadSettings.SourceVm.ProgressPercentString}"
						: $"{GetStatus(tgDownloadSettings.SourceVm.SourceLastId, tgDownloadSettings.SourceVm.SourceFirstId)} | {message} | " +
						  $"Progress {tgDownloadSettings.SourceVm.ProgressPercentString}";
				// Update source.
				async Task UpdateStateSourceAsync(long sourceId, int messageId, string message)
				{
					await Task.Delay(1);
					if (string.IsNullOrEmpty(message))
						return;
					statusContext.Status(TgLog.GetMarkupString(isScanCount
						//? $"{GetStatus(tgDownloadSettings.SourceVm.SourceScanCount, messageId)} | {message} | Progress {tgDownloadSettings.SourceVm.ProgressPercentString}"
						? $"{GetStatus(tgDownloadSettings.SourceVm.SourceScanCount, messageId)} | {message}"
						: GetFileStatus(message)));
					statusContext.Refresh();
				}
				// Update message.
				async Task UpdateStateMessageAsync(string message)
				{
					await Task.Delay(1);
					if (string.IsNullOrEmpty(message))
						return;
					statusContext.Status(TgLog.GetMarkupString(message));
					statusContext.Refresh();
				}
				// Update download file state.
				async Task UpdateStateFileAsync(long sourceId, int messageId, string fileName, long fileSize, long transmitted, long fileSpeed, 
					bool isFileNewDownload, int threadNumber)
				{
					await Task.Delay(1);
					// Download job.
					if (!string.IsNullOrEmpty(fileName) && !isFileNewDownload && tgDownloadSettings.SourceVm.SourceId.Equals(sourceId))
					{
						// Download status job.
						tgDownloadSettings.SourceVm.SourceFirstId = messageId;
						tgDownloadSettings.SourceVm.CurrentFileName = fileName;
						tgDownloadSettings.SourceVm.CurrentFileSize = fileSize;
						tgDownloadSettings.SourceVm.CurrentFileTransmitted = transmitted;
						tgDownloadSettings.SourceVm.CurrentFileSpeed = fileSpeed;
					}
					// Download reset.
					else
					{
						// Download status reset.
						tgDownloadSettings.SourceVm.SourceFirstId = messageId;
						tgDownloadSettings.SourceVm.CurrentFileName = string.Empty;
						tgDownloadSettings.SourceVm.CurrentFileSize = 0;
						tgDownloadSettings.SourceVm.CurrentFileTransmitted = 0;
						tgDownloadSettings.SourceVm.CurrentFileSpeed = 0;
					}
					// State.
					statusContext.Status(TgLog.GetMarkupString($"{GetFileStatus()} | " +
						$"File {fileName} | " +
						$"Transmitted {tgDownloadSettings.SourceVm.CurrentFileProgressPercentString} | Speed {tgDownloadSettings.SourceVm.CurrentFileSpeedKBString}"));
					statusContext.Refresh();
				}
				// Setup.
				TgClient.SetupUpdateTitle(UpdateConsoleTitleAsync);
				TgClient.SetupUpdateStateSource(UpdateStateSourceAsync);
				TgClient.SetupUpdateStateFile(UpdateStateFileAsync);
				TgClient.SetupUpdateStateMessage(UpdateStateMessageAsync);
				// Action.
				var sw = Stopwatch.StartNew();
				action(tgDownloadSettings);
				sw.Stop();
				// Update state source.
				UpdateStateSourceAsync(0, 0, isScanCount
					? $"{GetStatus(sw, tgDownloadSettings.SourceVm.SourceScanCount, tgDownloadSettings.SourceVm.SourceScanCurrent)}"
					: $"{GetStatus(sw, tgDownloadSettings.SourceVm.SourceFirstId, tgDownloadSettings.SourceVm.SourceLastId)}")
					.GetAwaiter().GetResult();
			});
		
		//TgLog.MarkupLine(TgLocale.TypeAnyKeyForReturn);
		//Console.ReadKey();
		TgLog.MarkupLine(TgLocale.WaitDownloadComplete);
		while (isWaitComplete && !tgDownloadSettings.SourceVm.IsComplete)
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