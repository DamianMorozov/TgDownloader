// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
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
	[ObservableProperty]
	private string _licenseDescription = string.Empty;
	[ObservableProperty]
	private string _donateUsdtTrc20 = "TBTDRWnMBw7acfpkhAXjSQNSNHQGFR662Y";
	[ObservableProperty]
	private string _donateUsdtTon = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	private string _donateToncoin = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	private string _donateBitcoin = "1FJayytWUK6vkxK2nUcD2TJskk3g9ZnmfW";
	[ObservableProperty]
	private string _donateNotcoin = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	private string _donateDogs = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	private string _donateHmstr = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	private string _donateX = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	private string _donateCatizen = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";
	[ObservableProperty]
	private string _donateMajor = "UQBkjSs3XPmraI_sS4Mf05SMd1y44DahNhwPg9ySp3V-M3N6";

	public TgMainViewModel(ITgSettingsService settingsService) : base(settingsService)
	{
		VersionDescription = GetVersionDescription();
		AppVersionTitle =
			$"{TgConstants.AppTitleDesktop} " +
			$"v{TgCommonUtils.GetTrimVersion(Assembly.GetExecutingAssembly().GetName().Version)}";
		AppVersionShort = $"v{TgCommonUtils.GetTrimVersion(Assembly.GetExecutingAssembly().GetName().Version)}";
		AppVersionFull = $"{TgResourceExtensions.GetAppVersion()}: {AppVersionShort}";
		LicenseDescription = LicenseManager.CurrentLicense.Description;
	}

	#endregion

	#region Public and private methods

	private static string GetVersionDescription()
	{
		Version version;
		if (TgRuntimeHelper.IsMSIX)
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
