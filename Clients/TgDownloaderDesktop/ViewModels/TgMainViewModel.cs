﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

public partial class TgMainViewModel : TgPageViewModelBase
{
	#region Public and private fields, properties, constructor

	[ObservableProperty]
	private string _versionDescription = string.Empty;
	[ObservableProperty]
	private string _appVersionTitle = string.Empty;
	[ObservableProperty]
	private string _appVersionShort = string.Empty;
	[ObservableProperty]
	private string _appVersionFull = string.Empty;

	public TgMainViewModel()
	{
		VersionDescription = GetVersionDescription();
		AppVersionTitle =
			$"{TgConstants.AppTitleDesktop} " +
			$"v{TgCommonUtils.GetTrimVersion(Assembly.GetExecutingAssembly().GetName().Version)}";
		AppVersionShort = $"v{TgCommonUtils.GetTrimVersion(Assembly.GetExecutingAssembly().GetName().Version)}";
		AppVersionFull = $"{TgDesktopUtils.TgLocale.AppVersion}: {AppVersionShort}";

	}

	#endregion

	#region Public and private methods

	private static string GetVersionDescription()
	{
		Version version;
		if (RuntimeHelper.IsMSIX)
		{
			var packageVersion = Package.Current.Id.Version;
			version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
		}
		else
		{
			version = Assembly.GetExecutingAssembly().GetName().Version!;
		}
		return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
	}

	#endregion
}
