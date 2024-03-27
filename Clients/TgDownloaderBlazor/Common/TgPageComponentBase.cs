// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderBlazor.Common;

public abstract class TgPageComponentBase : ComponentBase
{
    #region Public and private fields, properties, constructor

    [Inject] protected NavigationManager UriHelper { get; set; } = default!;
    [Inject] protected DialogService DialogService { get; set; } = default!;
    [Inject] protected TooltipService TooltipService { get; set; } = default!;
    [Inject] protected ContextMenuService ContextMenuService { get; set; } = default!;
    [Inject] protected NotificationService NotificationService { get; set; } = default!;
    [Inject] protected IDbContextFactory<TgEfContext> DbFactory { get; set; } = default!;
	[Inject] protected TgJsService JsService { get; set; } = default!;

    protected TgLocaleHelper TgLocale => TgLocaleHelper.Instance;
    protected TgAppSettingsHelper AppSettings => TgAppSettingsHelper.Instance;
    protected static TgClientHelper TgClient => TgClientHelper.Instance;
    public bool IsBlazorLoading { get; set; } = true;

    #endregion
}