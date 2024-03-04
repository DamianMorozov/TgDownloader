// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Pages;

public sealed partial class Index
{
	#region Public and private fields, properties, constructor

	public TgAppSettingsHelper TgAppSettings => TgAppSettingsHelper.Instance;

	#endregion

	#region Public and private methods

	private async Task ResetToDefault(MouseEventArgs arg)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1));
		TgAppSettings.DefaultXmlSettings();
	}

	private async Task SaveToXml(MouseEventArgs arg)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1));
		TgAppSettings.StoreXmlSettingsUnsafe();
		TgAppSettings.LoadXmlSettings();
	}

	#endregion
}