// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderCore.Locales;

internal class LocaleHelper
{
    #region Design pattern "Lazy Singleton"

    private static LocaleHelper _instance;
    internal static LocaleHelper Instance => LazyInitializer.EnsureInitialized(ref _instance);

    #endregion

    #region Public and private fields and properties

    public LocaleInfoHelper Info { get; } = LocaleInfoHelper.Instance;
    public LocaleQuestionHelper Question { get; } = LocaleQuestionHelper.Instance;
    public LocaleWarningHelper Warning { get; } = LocaleWarningHelper.Instance;

    #endregion
}