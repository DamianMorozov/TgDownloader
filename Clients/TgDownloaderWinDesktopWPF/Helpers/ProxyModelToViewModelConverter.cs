//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
//namespace TgDownloaderWinDesktopWPF.Helpers;

//public sealed class ProxyModelToViewModelConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        if (value is not TgEfProxyEntity proxy)
//            throw new ArgumentException($"{TgDesktopUtils.TgLocale.Exception} at {nameof(ProxyModelToViewModelConverter)}");

//        return new TgEfProxyViewModel(proxy);
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        if (parameter is not TgEfProxyViewModel proxyVm)
//            throw new ArgumentException($"{TgDesktopUtils.TgLocale.Exception} at {nameof(ProxyModelToViewModelConverter)}");

//        return proxyVm.Proxy;
//    }
//}
