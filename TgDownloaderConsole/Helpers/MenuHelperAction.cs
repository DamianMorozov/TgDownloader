// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// See https://aka.ms/new-console-template for more information

namespace TgDownloaderConsole.Helpers;

internal partial class MenuHelper
{
    #region Public and private methods

    public void RunAction(Action action)
    {
        if (!CheckTgSettings())
        {
            TgLog.Warning(TgLocale.TgMustSetSettings);
            Console.ReadKey();
            return;
        }

        AnsiConsole.Status()
            .AutoRefresh(true)
            .Spinner(Spinner.Known.Star)
            .SpinnerStyle(Style.Parse("green"))
            .Start("Thinking...", statusContext =>
            {
                StatusContext = statusContext;
                Stopwatch sw = new();
                sw.Start();
                //statusContext.Status($"{GetStatus(sw,
                //    TgClient.TgDownload.MessageCurrentId, TgClient.TgDownload.MessageCount)} | Process job");
                //statusContext.Refresh();
                action();
                sw.Stop();
                statusContext.Status($"{GetStatus(sw,
                    TgClient.TgDownload.MessageCurrentId, TgClient.TgDownload.MessageCount)}");
                statusContext.Refresh();
            });
        StatusContext = null;
        TgLog.Line(TgLocale.TypeAnyKeyForReturn);
        Console.ReadKey();
    }

    #endregion
}