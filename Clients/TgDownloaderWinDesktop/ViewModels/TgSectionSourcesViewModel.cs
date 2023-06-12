// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using TgStorage.Common;

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgSectionSourcesViewModel : TgBaseViewModel
{
	#region Public and private fields, properties, constructor

	public TgSectionSourcesViewModel()
	{
			ContextCache.Load(TgSqlTableName.Sources);
	}

	#endregion

	#region Public and private methods

	[RelayCommand]
	internal void OnMenuSourceOpen(string id)
	{
		
	}

	#endregion
}