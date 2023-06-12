// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgAppViewModel : TgBaseViewModel
{
	#region Public and private fields, properties, constructor

	public TgAppViewModel()
	{
		//
	}

	#endregion

	#region Public and private methods

	[RelayCommand]
	internal void OnMenuAppReset()
	{
		//
	}

	[RelayCommand]
	internal void OnMenuAppSetFileSession()
	{
		//
	}

	[RelayCommand]
	internal void OnMenuAppSetFileStorage()
	{
		//
	}

	[RelayCommand]
	internal void OnMenuAppSetUseProxy()
	{
		//
	}

	#endregion
}