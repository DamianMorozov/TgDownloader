namespace TgDownloaderBlazor.Common;

public abstract class TgPageComponent : ComponentBase
{
    #region Public and private fields, properties, constructor

    protected virtual TgLocaleHelper TgLocale => TgLocaleHelper.Instance;

    #endregion
}