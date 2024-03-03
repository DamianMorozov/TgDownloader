// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace TgDownloaderBlazor.Pages;

public sealed partial class Index
{
	#region Public and private fields, properties, constructor

	private int currentCount = 0;

	#endregion

	#region Public and private methods

	private void IncrementCount()
	{
		currentCount++;
	}

	private async Task ButtonIncrement(MouseEventArgs arg)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1));
		currentCount++;
	}

	#endregion
}