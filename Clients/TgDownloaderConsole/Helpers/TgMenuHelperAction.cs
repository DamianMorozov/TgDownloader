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

    public void RunAction(TgDownloadSettingsModel tgDownloadSettings, Action<TgDownloadSettingsModel> action,
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
                // Update Console Title
                void UpdateConsoleTitle(string title) => Console.Title = string.IsNullOrEmpty(title) 
                    ? $"{TgLocale.AppTitleConsoleShort}" : $"{TgLocale.AppTitleConsoleShort} {title}";
                // Update status.
                void UpdateStateMessage(string message)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        statusContext.Status(TgLog.GetMarkupString(message));
                        statusContext.Refresh();
                    }
                }
                void UpdateStateSource(long sourceId, int messageId, string message)
                {
                    if (isScanCount)
                        statusContext.Status(
                            TgLog.GetMarkupString($"{GetStatus(tgDownloadSettings.SourceVm.SourceScanCount,
                                messageId)} | {message}"));
                    else
                        statusContext.Status(
                            TgLog.GetMarkupString($"{GetStatus(tgDownloadSettings.SourceVm.SourceLastId,
                                tgDownloadSettings.SourceVm.SourceFirstId)} | {message}"));
                    statusContext.Refresh();
                }
                TgClient.SetupActions(UpdateConsoleTitle, UpdateStateMessage, UpdateStateSource);
                // Action.
                Stopwatch sw = new();
                sw.Start();
                action(tgDownloadSettings);
                sw.Stop();
                UpdateStateMessage(
                    isScanCount
                        ? $"{GetStatus(sw, tgDownloadSettings.SourceVm.SourceScanCount, tgDownloadSettings.SourceVm.SourceScanCurrent)}"
                        : $"{GetStatus(sw, tgDownloadSettings.SourceVm.SourceFirstId, tgDownloadSettings.SourceVm.SourceLastId)}");
            });
        TgLog.MarkupLine(TgLocale.TypeAnyKeyForReturn);
        Console.ReadKey();
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