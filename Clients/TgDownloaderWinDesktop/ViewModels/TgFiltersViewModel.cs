// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToDebugString()}")]
public sealed partial class TgFiltersViewModel : TgPageViewModelBase, INavigationAware
{
	#region Public and private fields, properties, constructor

	public TgFiltersViewModel()
	{
		//
	}

	#endregion

	#region Public and private methods

	public void OnNavigatedTo()
	{
        _ = Task.Run(InitializeViewModelAsync).ConfigureAwait(true);
    }

	public void OnNavigatedFrom() { }

	#endregion
}