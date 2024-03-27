// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Pages;

public sealed partial class Index : TgPageComponentBase
{
	#region Public and private fields, properties, constructor

	//

	#endregion

	#region Public and private methods

	protected override async Task OnInitializedAsync()
    {
		await base.OnInitializedAsync();
		if (!IsBlazorLoading)
			return;
		
		await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
		IsBlazorLoading = false;
	}

	private async Task ResetToDefault(MouseEventArgs arg)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1));
		AppSettings.DefaultXmlSettings();
	}

	private async Task SaveToXml(MouseEventArgs arg)
	{
		await Task.Delay(TimeSpan.FromMilliseconds(1));
		AppSettings.StoreXmlSettingsUnsafe();
		AppSettings.LoadXmlSettings();
	}

	#endregion
}