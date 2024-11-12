// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Models;

public sealed class LocalSettingsOptions
{
    public string? ApplicationDataFolder { get; set; }
    public string? LocalSettingsFile { get; set; }
    public string? EfStorage { get; set; }
    public string? TgSession { get; set; }
}
