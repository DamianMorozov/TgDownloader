// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
    #region Public and private methods

    public bool CheckTgSettingsWithWarning(TgDownloadSettingsModel tgDownloadSettings)
    {
        bool result = TgClient is { IsReady: true } && tgDownloadSettings.SourceVm.IsReady;
        if (!result)
        {
            ClientConnect(tgDownloadSettings, true);
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


	public void RunActionProgress(TgDownloadSettingsModel tgDownloadSettings, Func<TgDownloadSettingsModel, Task> action,
        bool isSkipCheckTgSettings, bool isScanCount)
    {
        if (!isSkipCheckTgSettings && !CheckTgSettingsWithWarning(tgDownloadSettings))
            return;

		AnsiConsole.Progress()
            .AutoRefresh(false)
            .AutoClear(true)
            .HideCompleted(true)
            .Columns(GetProgressColumns())
			.StartAsync(async context =>
			{
				Stopwatch sw = Stopwatch.StartNew();
				Stopwatch swFileRefresh = Stopwatch.StartNew();
				// ProgressTask.
				ProgressTask progressTaskFile = context.AddTask("Downloading file", new ProgressTaskSettings
				{
					AutoStart = true,
					MaxValue = 0
				});
				ProgressTask progressTaskSource = context.AddTask("Downloading source", new ProgressTaskSettings
				{
					AutoStart = true,
					MaxValue = tgDownloadSettings.SourceVm.SourceLastId
				});
				// Update console title.
				async Task UpdateConsoleTitleAsync(string title)
				{
					await Task.Delay(TimeSpan.FromMilliseconds(1));
					Console.Title = string.IsNullOrEmpty(title) ? $"{TgLocale.AppTitleConsoleShort}" : $"{TgLocale.AppTitleConsoleShort} {title}";
				}
				// Update source.
				async Task UpdateStateSourceAsync(long sourceId, int messageId, string message)
                {
	                await Task.Delay(TimeSpan.FromMilliseconds(1));
	                if (string.IsNullOrEmpty(message)) return;
					progressTaskSource.Description = $"Read the message {tgDownloadSettings.SourceVm.SourceFirstId} from {tgDownloadSettings.SourceVm.SourceLastId}";
					progressTaskSource.Value = tgDownloadSettings.SourceVm.SourceFirstId;
					context.Refresh();
				}
				// Update download file state.
				async Task UpdateStateFileAsync(long sourceId, int messageId, string fileName, long fileSize, long transmitted, long fileSpeed, bool isFileNewDownload)
                {
	                await Task.Delay(TimeSpan.FromMilliseconds(1));
	                progressTaskSource.Description = $"Read the message {tgDownloadSettings.SourceVm.SourceFirstId} from {tgDownloadSettings.SourceVm.SourceLastId}";
	                progressTaskSource.Value = tgDownloadSettings.SourceVm.SourceFirstId;
					// Download job.
					if (!string.IsNullOrEmpty(fileName) && !isFileNewDownload && tgDownloadSettings.SourceVm.SourceId.Equals(sourceId))
	                {
						// ProgressTask.
						progressTaskFile.Description = fileName;
		                progressTaskFile.Value = transmitted;
						if (!progressTaskFile.MaxValue.Equals(fileSize))
							progressTaskFile.MaxValue = fileSize;
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
						// ProgressTask.
						progressTaskFile.Value = 0;
						progressTaskFile.MaxValue = fileSize;
						// Download status reset.
						tgDownloadSettings.SourceVm.SourceFirstId = messageId;
						tgDownloadSettings.SourceVm.CurrentFileName = string.Empty;
						tgDownloadSettings.SourceVm.CurrentFileSize = 0;
						tgDownloadSettings.SourceVm.CurrentFileTransmitted = 0;
						tgDownloadSettings.SourceVm.CurrentFileSpeed = 0;
	                }
					// State.
					if (!swFileRefresh.IsRunning)
						swFileRefresh.Start();
					else if (swFileRefresh.Elapsed > TimeSpan.FromMilliseconds(1_000))
					{
						swFileRefresh.Reset();
						context.Refresh();
					}
				}
				// Setup.
				TgClient.SetupUpdateTitle(UpdateConsoleTitleAsync);
                TgClient.SetupUpdateStateSource(UpdateStateSourceAsync);
                TgClient.SetupUpdateStateFile(UpdateStateFileAsync);
				// Action.
				await action(tgDownloadSettings);
				sw.Stop();
				progressTaskFile.StopTask();
				progressTaskSource.StopTask();
                UpdateStateSourceAsync(0, 0, 
                    isScanCount
                        ? $"{GetStatus(sw, tgDownloadSettings.SourceVm.SourceScanCount, tgDownloadSettings.SourceVm.SourceScanCurrent)}"
                        : $"{GetStatus(sw, tgDownloadSettings.SourceVm.SourceFirstId, tgDownloadSettings.SourceVm.SourceLastId)}")
                    .GetAwaiter().GetResult();
            });

		//TgLog.MarkupLine(TgLocale.TypeAnyKeyForReturn);
		//Console.ReadKey();
		TgLog.MarkupLine(TgLocale.WaitDownloadComplete);
		while (!tgDownloadSettings.SourceVm.IsComplete)
		{
			Console.ReadKey();
			TgLog.MarkupLine(TgLocale.WaitDownloadComplete);
		}
	}

	public void RunActionStatus(TgDownloadSettingsModel tgDownloadSettings, Func<TgDownloadSettingsModel, Task> action,
		bool isSkipCheckTgSettings, bool isScanCount)
	{
		if (!isSkipCheckTgSettings && !CheckTgSettingsWithWarning(tgDownloadSettings))
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
					await Task.Delay(TimeSpan.FromMilliseconds(1));
					Console.Title = string.IsNullOrEmpty(title) ? $"{TgLocale.AppTitleConsoleShort}" : $"{TgLocale.AppTitleConsoleShort} {title}";
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
					await Task.Delay(TimeSpan.FromMilliseconds(1));
					if (string.IsNullOrEmpty(message))
						return;
					statusContext.Status(TgLog.GetMarkupString(isScanCount
						? $"{GetStatus(tgDownloadSettings.SourceVm.SourceScanCount, messageId)} | {message} | " +
							$"Progress {tgDownloadSettings.SourceVm.ProgressPercentString}"
						: GetFileStatus(message)));
					statusContext.Refresh();
				}
				// Update message.
				async Task UpdateStateMessageAsync(string message)
				{
					await Task.Delay(TimeSpan.FromMilliseconds(1));
					if (string.IsNullOrEmpty(message))
						return;
					statusContext.Status(TgLog.GetMarkupString(message));
					statusContext.Refresh();
				}
				// Update download file state.
				async Task UpdateStateFileAsync(long sourceId, int messageId, string fileName, long fileSize, long transmitted, long fileSpeed, bool isFileNewDownload)
				{
					await Task.Delay(TimeSpan.FromMilliseconds(1));
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
				Stopwatch sw = Stopwatch.StartNew();
				action(tgDownloadSettings).GetAwaiter().GetResult();
				sw.Stop();
				// Update state source.
				UpdateStateSourceAsync(0, 0,
						isScanCount
							? $"{GetStatus(sw, tgDownloadSettings.SourceVm.SourceScanCount, tgDownloadSettings.SourceVm.SourceScanCurrent)}"
							: $"{GetStatus(sw, tgDownloadSettings.SourceVm.SourceFirstId, tgDownloadSettings.SourceVm.SourceLastId)}")
					.GetAwaiter().GetResult();
			});
		
		//TgLog.MarkupLine(TgLocale.TypeAnyKeyForReturn);
		//Console.ReadKey();
		TgLog.MarkupLine(TgLocale.WaitDownloadComplete);
		while (!tgDownloadSettings.SourceVm.IsComplete)
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