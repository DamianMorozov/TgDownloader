namespace TgDownloaderWinDesktop.Helpers;

public sealed class ProxyModelToViewModelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TgSqlTableProxyModel proxy)
            throw new ArgumentException("Exception ProxyModelToViewModelConverter");

        return new TgSqlTableProxyViewModel(proxy);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is not TgSqlTableProxyViewModel proxyVm)
            throw new ArgumentException("Exception ProxyModelToViewModelConverter");

        return proxyVm.Proxy;
    }
}
