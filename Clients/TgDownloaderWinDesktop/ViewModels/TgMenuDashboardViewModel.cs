// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgMenuDashboardViewModel : TgBaseViewModel
{
	#region Public and private fields, properties, constructor

	public TgAppSettingsHelper TgAppSettings { get; private set; } = TgAppSettingsHelper.Instance;

	#endregion
}