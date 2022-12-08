// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Diagnostics;

namespace TgDownloaderCore.Locales;

public class LocaleInfoHelper
{
    #region Design pattern "Lazy Singleton"

    private static LocaleInfoHelper _instance;
    public static LocaleInfoHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields and properties

    public string AppName => "[green]Name[/]";
    public string AppStatus => "[green]Status[/]";
    public string AppTitle => "TG-Downloader";
    public string AppValue => "[green]Value[/]";
    public string AppVersion => "[green]App version[/]";
    public string FileWrite(string file) => $"[green]File '{file}' is open for write.[/]";
    public string SourceFileInfo(string file) => $"[green]Source file: '{file}'.[/]";
    public string MenuMain => @"
Exit
Setup TG
Download files from TG Channel
            ".TrimStart('\r', ' ', '\n', '\t').TrimEnd('\r', ' ', '\n', '\t');
    public string DestDirectory => "[green]Enter destination directory:[/]";
    public string MenuDownloadFiles => "Download files from TG Channel";
    public string MenuReturn => "[green]Type any key to return into the main menu.[/]";
    public string MenuSetupTg => "Setup TG";
    public string MenuSwitchNumber => "[green]Switch menu number:[/]";
    public string MoveUpDown => "[grey](Move up and down to switch select)[/]";
    public string StatusException => "[green]Exception:[/]";
    public string StatusFinish(Stopwatch sw) => $"[green]Job is finished. Elapsed time: {sw.Elapsed}.[/]";
    public string StatusInnerException => "[green]Inner exception:[/]";
    public string StatusStart => "[green]Job is started.[/]";

    #endregion
}