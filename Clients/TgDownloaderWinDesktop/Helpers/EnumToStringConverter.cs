// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Helpers;

public sealed class EnumToStringConverter : IValueConverter
{
	public EnumToStringConverter()
	{
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is not string enumString)
			throw new ArgumentException($"Exception \tpublic {nameof(EnumToStringConverter)}()\r\n");

		if (!Enum.IsDefined(typeof(Wpf.Ui.Appearance.ThemeType), value))
			throw new ArgumentException($"Exception \tpublic {nameof(EnumToStringConverter)}()\r\n");

		object enumValue = Enum.Parse(typeof(TgEnumProxyType), enumString);

		return enumValue.Equals(value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is not string enumString)
			throw new ArgumentException($"Exception {nameof(EnumToStringConverter)}");

		return Enum.Parse(typeof(Wpf.Ui.Appearance.ThemeType), enumString);
	}
}