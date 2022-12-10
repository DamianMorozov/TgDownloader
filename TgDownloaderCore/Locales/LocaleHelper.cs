// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderCore.Locales;

public class LocaleHelper
{
    #region Design pattern "Lazy Singleton"

    private static LocaleHelper _instance;
    public static LocaleHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields and properties

    public LocaleInfoHelper Info => LocaleInfoHelper.Instance;
    public LocaleQuestionHelper Question => LocaleQuestionHelper.Instance;
    public LocaleWarningHelper Warning => LocaleWarningHelper.Instance;

    #endregion

    #region Public and private methods

    public string InfoMessage(string message) => $"[green]i {message}[/]";
    public string WarningMessage(string message) => $"[red]i {message}[/]";

    #endregion
}