// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// See https://aka.ms/new-console-template for more information

LocaleHelper locale = LocaleHelper.Instance;
LogHelper log = LogHelper.Instance;
MenuHelper menu = MenuHelper.Instance;
TgClientHelper tgClient = TgClientHelper.Instance;
StatusContext globalStatusContext = null;

SetLog();

TgMenu menuMain = TgMenu.Exit;
do
{
    try
    {
        menu.ShowTable(locale.AppMainMenu);
        string menuAsk = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title(locale.MenuSwitchNumber)
            .PageSize(10)
            .MoreChoicesText(locale.MoveUpDown)
            .AddChoices(locale.MenuMain));
        switch (menuAsk)
        {
            case "Connect":
                menuMain = TgMenu.Connect;
                ConnectTgClient();
                break;
            case "Info":
                menuMain = TgMenu.Info;
                GetTgInfo();
                break;
            case "Settings":
                menuMain = TgMenu.Settings;
                SetupTgSettings();
                break;
            case "Download files":
                menuMain = TgMenu.DownloadFiles;
                RunMenuJob(DownloadFiles);
                break;
        }
    }
    catch (Exception ex)
    {
        log.MarkupLineStamp(locale.StatusException + log.GetMarkupString(ex.Message));
        if (ex.InnerException is not null)
            log.MarkupLineStamp(locale.StatusInnerException + log.GetMarkupString(ex.InnerException.Message));
        menuMain = TgMenu.Exit;
    }
} while (menuMain is not TgMenu.Exit);

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
    TgMenuSettings menuSettings;
    do
    {
        menu.ShowTable(locale.TgSettings);
        menuSettings = menu.SetOptionsSetupTg();
        switch (menuSettings)
        {
            case TgMenuSettings.SourceUsername:
                tgClient.TgSettings.SetSourceUserName();
                tgClient.PrepareCollectMessages();
                break;
            case TgMenuSettings.DestDirectory:
                tgClient.TgSettings.SetDestDirectory();
                break;
            case TgMenuSettings.MessageCurrentId:
                tgClient.TgSettings.SetMessageCurrentId();
                break;
        }
    } while (menuSettings is not TgMenuSettings.Return);
}

void RunMenuJob(Action<Stopwatch> action)
{
    menu.ShowTable(locale.MenuDownloadFiles);
    log.MarkupLineStamp(locale.StatusStart);
    if (!menu.CheckTgSettings())
    {
        log.MarkupLineStampWarning(locale.TgMustSetSettings);
        Console.ReadKey();
        return;
    }

    AnsiConsole.Status()
        .AutoRefresh(true)
        .Spinner(Spinner.Known.Ascii)
        .SpinnerStyle(Style.Parse("green"))
        .Start("Thinking...", statusContext =>
        {
            globalStatusContext = statusContext;
            Stopwatch sw = new();
            sw.Start();
            statusContext.Status($"{menu.GetStatus(sw, 
                tgClient.TgSettings.MessageCurrentId, tgClient.TgSettings.MessageCount)} | Process job");
            statusContext.Refresh();
            action(sw);
            sw.Stop();
            statusContext.Status($"{menu.GetStatus(sw,
                tgClient.TgSettings.MessageCurrentId, tgClient.TgSettings.MessageCount)} | Job is complete");
            statusContext.Refresh();
        });
    globalStatusContext = null;
    log.MarkupLineStamp(locale.MenuReturn);
    Console.ReadKey();
}

void ConnectTgClient()
{
    menu.ShowTable(locale.TgClientConnect);
    tgClient.Connect();
    tgClient.CollectAllChats().GetAwaiter().GetResult();
    log.MarkupLineStampInfo(locale.TgClientSetupComplete);
    Console.ReadKey();
}

void GetTgInfo()
{
    menu.ShowTable(locale.TgClientGetInfo);
    if (!tgClient.IsConnected)
    {
        log.MarkupLineStampWarning(locale.TgMustClientConnect);
        Console.ReadKey();
        return;
    }

    Dictionary<long, ChatBase> dicDialogs = tgClient.CollectAllDialogs();
    tgClient.PrintChatsInfo(dicDialogs, "dialogs", RefreshStatus);
    
    log.MarkupLineStampInfo(locale.TgGetInfoComplete);
    Console.ReadKey();
}

void DownloadFiles(Stopwatch sw)
{
    AnsiConsole.Clear();
    Channel channel = tgClient.PrepareCollectMessages();
    menu.ShowTable(locale.MenuDownloadFiles);
    tgClient.CollectMessages(channel, sw, RefreshStatus);
    tgClient.TgSettings.SetMessageCurrentIdDefault();
}

void RefreshStatus(string message)
{
    //AnsiConsole.Clear();
    if (globalStatusContext is null) return;
    globalStatusContext.Status(log.GetMarkupString($"{menu.GetStatus(tgClient.TgSettings.MessageCount, tgClient.TgSettings.MessageCurrentId)} | {message}"));
    globalStatusContext.Refresh();
}