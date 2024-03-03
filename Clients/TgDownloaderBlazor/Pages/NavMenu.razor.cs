namespace TgDownloaderBlazor.Pages;

public sealed partial class NavMenu: TgPageComponentBase
{
    #region Public and private fields, properties, constructor

    private bool _collapseNavMenu = true;

    private string? NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

    #endregion

    #region Public and private methods

    private void ToggleNavMenu()
    {
        _collapseNavMenu = !_collapseNavMenu;
    }

    #endregion
}