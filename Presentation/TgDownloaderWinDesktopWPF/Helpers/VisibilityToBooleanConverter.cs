// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktopWPF.Helpers;

public sealed class VisibilityToBooleanConverter : IValueConverter
{
	public VisibilityToBooleanConverter()
	{
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is not string enumString)
			throw new ArgumentException($"{TgDesktopUtils.TgLocale.Exception} at {nameof(VisibilityToBooleanConverter)}");

		if (!Enum.IsDefined(typeof(Visibility), value))
			throw new ArgumentException($"{TgDesktopUtils.TgLocale.Exception} at {nameof(VisibilityToBooleanConverter)}");

		object enumValue = Enum.Parse(typeof(Visibility), enumString);

		return enumValue.Equals(value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is not string enumString)
			throw new ArgumentException($"{TgDesktopUtils.TgLocale.Exception} at {nameof(VisibilityToBooleanConverter)}");

		return Enum.Parse(typeof(Visibility), enumString);
	}
}