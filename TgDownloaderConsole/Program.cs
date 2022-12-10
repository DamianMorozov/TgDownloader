// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// See https://aka.ms/new-console-template for more information

LocaleHelper locale = LocaleHelper.Instance;
LogHelper log = LogHelper.Instance;
MenuHelper menu = MenuHelper.Instance;
TgClientHelper tgClient = TgClientHelper.Instance;

SetLog();

do
{
    try
    {
        menu.ShowTable("");
        menu.MenuItem = TgMenu.Return;
        string mainMenu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title(locale.Info.MenuSwitchNumber)
            .PageSize(6)
            .MoreChoicesText(locale.Info.MoveUpDown)
            .AddChoices(locale.Info.MenuMain));
        switch (mainMenu)
        {
            case "Exit":
                menu.MenuItem = TgMenu.Exit;
                break;
            case "Connect":
                ConnectTgClient();
                break;
            case "Settings":
                SetupTgSettings();
                break;
            case "Download files":
                menu.MenuItem = TgMenu.DownloadFiles;
                RunMenuJob(DownloadFiles);
                break;
        }
    }
    catch (Exception ex)
    {
        log.MarkupLineStamp(locale.Info.StatusException + log.GetMarkupString(ex.Message));
        if (ex.InnerException is not null)
            log.MarkupLineStamp(locale.Info.StatusInnerException + log.GetMarkupString(ex.InnerException.Message));
        menu.MenuItem = TgMenu.Exit;
    }
} while (menu.MenuItem is not TgMenu.Exit);

void SetLog()
{
    Console.OutputEncoding = Encoding.UTF8;
    log.SetMarkupLineStamp(AnsiConsole.MarkupLine);
    log.SetAskString(AnsiConsole.Ask<string>);
    log.SetAskInt(AnsiConsole.Ask<int>);
    log.SetAskBool(AnsiConsole.Ask<bool>);
}

void SetupTgSettings()
{
    menu.ShowTable(locale.Info.MenuSetupTg);
    menu.SetOptionsSetupTg();

    switch (menu.MenuItem)
    {
        case TgMenu.SetTgSourceUsername:
            tgClient.TgSettings.SetSourceUserName();
            break;
        case TgMenu.SetTgDestDirectory:
            tgClient.TgSettings.SetDestDirectory();
            break;
        case TgMenu.SetTgMessageStartId:
            tgClient.TgSettings.SetMessageStartId();
            break;
        case TgMenu.SetTgMessageCount:
            tgClient.TgSettings.SetMessageCount();
            break;
    }
}

void RunMenuJob(Action action)
{
    menu.ShowTable(locale.Info.MenuDownloadFiles);
    log.MarkupLineStamp(locale.Info.StatusStart);

    AnsiConsole.Status()
        .AutoRefresh(true)
        .Spinner(Spinner.Known.Aesthetic)
        .SpinnerStyle(Style.Parse("green"))
        .Start("Thinking...", statusContext =>
        {
            Stopwatch sw = new();
            sw.Start();
            statusContext.Status($"{menu.GetStatusInfo(sw)} | Process job.");
            statusContext.Refresh();
            action();
            sw.Stop();
            statusContext.Status($"{menu.GetStatusInfo(sw)} | Job is complete.");
            statusContext.Refresh();
        });
    log.MarkupLineStamp(locale.Info.MenuReturn);
    Console.ReadKey();
}

void ConnectTgClient()
{
    tgClient.Connect();
    tgClient.CollectAllChats().GetAwaiter().GetResult();
    tgClient.PrintChatsInfo(tgClient.ListSmallGroups, "small groups", true);
    tgClient.PrintChannelsInfo(tgClient.ListGroups, "groups", true);
    tgClient.PrintChannelsInfo(tgClient.ListChannels, "channels", true);
    log.MarkupLineStamp(locale.Info.AnyKey);
    Console.ReadKey();
}

void DownloadFiles()
{
    if (!menu.CheckTgSettings()) return;
    Channel channel = tgClient.PrepareCollectMessages();
    menu.ShowTable(locale.Info.MenuDownloadFiles);
    tgClient.CollectMessages(channel).GetAwaiter().GetResult();
}
