// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgItemSourceViewModel : TgViewBase, INavigationAware
{
	#region Public and private fields, properties, constructor

	public TgItemSourceViewModel()
	{
		//
	}

	#endregion

	#region Public and private methods

	public void OnNavigatedTo()
	{
		if (!IsInitialized)
			InitializeViewModel();
	}

	public void OnNavigatedFrom()
	{
		//
	}

	private void InitializeViewModel()
	{
		IsInitialized = true;
	}

	[RelayCommand]
	internal void OnMenuSourceOpen(string id)
	{
		//
	}

	#endregion
}