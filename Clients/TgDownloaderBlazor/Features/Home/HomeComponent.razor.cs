// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Features.Home;

public sealed partial class HomeComponent : TgPageComponent
{
    #region Public and private fields, properties, constructor

    public static string AppVersionTitle { get; set; } = string.Empty;
    public static string AppVersionShort { get; set; } = string.Empty;
    public static string AppVersionFull { get; set; } = string.Empty;

    public HomeComponent()
    {
        AppVersionTitle = $"{TgLocale.AppTitleBlazor} " +
                          $"v{TgCommonUtils.GetTrimVersion(Assembly.GetExecutingAssembly().GetName().Version)}";
        AppVersionShort = $"v{TgCommonUtils.GetTrimVersion(Assembly.GetExecutingAssembly().GetName().Version)}";
        AppVersionFull = $"{TgLocale.AppVersion}: {AppVersionShort}";

    }

    #endregion
}