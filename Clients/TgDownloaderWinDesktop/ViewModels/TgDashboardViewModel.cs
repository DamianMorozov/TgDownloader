// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgCore.Helpers;

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgDashboardViewModel : TgBaseViewModel
{
	#region Public and private fields, properties, constructor

	public TgAppSettingsHelper AppSettings { get; private set; } = TgAppSettingsHelper.Instance;

	#endregion
}