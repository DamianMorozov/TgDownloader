// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderCore.Locales;

public class LocaleWarningHelper
{
    #region Design pattern "Lazy Singleton"

    private static LocaleWarningHelper _instance;
    public static LocaleWarningHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields and properties

    public string DeleteDestFile => "[red]Deleted and create out file.[/]";
    public string DirIsNotExists => "[red]Directory is not exists![/]";
    public string DirNotFound(string dir) => $"[red]Directory '{dir}' is not found![/]";
    public string FileNotFound(string file) => $"[red]File '{file}' is not found![/]";
    public string HaveException => "[red]Exception![/]";
    public string Message(string message) => $"[red]x {message}[/]";
    public string NotValidMenuItem => "[red]That's not a valid menu item![/]";
    public string ValueLess(string value) => $"[red]Value must be more than {value}[/]";
    public string ValueMore(string value) => $"[red]Value must be less than {value}[/]";
    public string TgSettingsNotSet => "[red]<not set>[/]";

    #endregion
}