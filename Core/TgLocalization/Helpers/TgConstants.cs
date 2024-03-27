// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgLocalization.Helpers;

public sealed class TgConstants
{
	public static TgConstants CreateInstance()
	{
		return new TgConstants();
	}

    public static string AppTitle => "TgDownloader";
    public static string AppTitleBlazor => "TgDownloader-Blazor";
    public static string AppTitleConsoleShort => "TGDC";
    public static string AppTitleWinDesktop => "TgDownloader-Desktop";
    public static string LinkDockerHub => "https://hub.docker.com/r/damianmorozov/tgdownloader-blazor";
    public static string LinkDockerHubTitle => "DockerHub";
    public static string LinkGitHub => "https://github.com/DamianMorozov/TgDownloader";
    public static string LinkGitHubTitle => "GitHub";
	public static string LinkTelegramAppsTitle => "Telegram";
	public static string LinkTelegramApps => "https://my.telegram.org/apps/";
}