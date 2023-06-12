// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Collections.Generic;
using System.Windows.Media;

namespace TgDownloaderWinDesktop.ViewModels;

[DebuggerDisplay("{ToString()}")]
public sealed partial class TgDataViewModel : TgBaseViewModel
{
	private bool _isInitialized = false;

	public IEnumerable<DataColor> Colors { get; set; } = new List<DataColor>();

	public override void OnNavigatedTo()
	{
		if (!_isInitialized)
			InitializeViewModel();
	}

	private void InitializeViewModel()
	{
		Random random = new();
		List<DataColor> colorCollection = new();

		for (int i = 0; i < 8192; i++)
			colorCollection.Add(new()
			{
				Color = new SolidColorBrush(Color.FromArgb(
					(byte)200,
					(byte)random.Next(0, 250),
					(byte)random.Next(0, 250),
					(byte)random.Next(0, 250)))
			});

		Colors = colorCollection;

		_isInitialized = true;
	}
}