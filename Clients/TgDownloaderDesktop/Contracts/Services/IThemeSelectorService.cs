﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.Contracts.Services;

public interface IThemeSelectorService
{
	ElementTheme Theme
	{
		get;
	}

	Task InitializeAsync();

	Task SetThemeAsync(ElementTheme theme);

	Task SetRequestedThemeAsync();
}
