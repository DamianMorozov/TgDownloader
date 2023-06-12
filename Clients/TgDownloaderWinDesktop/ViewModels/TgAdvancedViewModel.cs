// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgAdvancedViewModel : TgBaseViewModel
{
	#region Public and private fields, properties, constructor

	public TgAdvancedViewModel()
	{
		//
	}

	#endregion

	#region Public and private methods

	[RelayCommand]
	internal void OnMenuScanChats()
	{
		//
	}

	[RelayCommand]
	internal void OnMenuScanDialogs()
	{
		//
	}

	[RelayCommand]
	internal void OnMenuViewSources()
	{
		//await Shell.Current.GoToAsync(nameof(TgSectionSourcesPage)).ConfigureAwait(false);
	}

	[RelayCommand]
	internal void OnMenuDownloadAuto()
	{
		//
	}

	#endregion
}