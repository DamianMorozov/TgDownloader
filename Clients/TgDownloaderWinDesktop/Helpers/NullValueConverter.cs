// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Helpers;

public sealed class NullValueConverter : IValueConverter
{
    private TgLocaleHelper TgLocale { get; } = TgLocaleHelper.Instance;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value ?? DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	    => throw new NotImplementedException(TgLocale.UseOverrideMethod);
}