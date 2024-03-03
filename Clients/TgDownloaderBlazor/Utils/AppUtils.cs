// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Utils;

public static class AppUtils
{

    public static string AppVersionTitle { get; set; }
    public static string AppVersionShort { get; set; }
    public static string AppVersionFull { get; set; }

    static AppUtils()
    {
        AppVersionTitle = 
            $"{TgLocaleHelper.Instance.AppTitleBlazor} v{TgCommonUtils.GetTrimVersion(Assembly.GetExecutingAssembly().GetName().Version)}";
        AppVersionShort = $"v{TgCommonUtils.GetTrimVersion(Assembly.GetExecutingAssembly().GetName().Version)}";
        AppVersionFull = $"{TgLocaleHelper.Instance.AppVersion}: {AppVersionShort}";
    }
}
