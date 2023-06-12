// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Models;

public sealed class AppConfig
{
	public string ConfigurationsFolder { get; set; } = string.Empty;

	public string AppPropertiesFileName { get; set; } = string.Empty;
}