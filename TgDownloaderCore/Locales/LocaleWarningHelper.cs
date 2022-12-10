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

    public string DirIsNotExists => "Directory is not exists!";
    public string DirNotFound(string dir) => $"Directory '{dir}' is not found!";
    public string TgSettingsNotSet => "<not set>";

    #endregion
}