// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Pages;

public sealed partial class Header : RadzenHeader
{
    #region Public and private fields, properties, constructor

    [Inject] private NotificationService NotificationService { get; set; } = default!;
    [Inject] private TgJsService JsService { get; set; } = default!;

	#endregion

	#region Public and private methods

	private async Task ShowAppInfo()
	{
        await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
        NotificationService.Notify(new NotificationMessage
        {
            Severity = NotificationSeverity.Info,
            Summary = TgLocaleHelper.Instance.AppInfo,
            Detail = TgAppUtils.AppVersionFull
        });
    }

    #endregion
}