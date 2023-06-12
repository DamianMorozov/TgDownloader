// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Globalization;
using System.Windows.Data;

namespace TgDownloaderWinDesktop.Helpers;

public sealed class EnumToBooleanConverter : IValueConverter
{
	public EnumToBooleanConverter()
	{
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is not string enumString)
			throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");

		if (!Enum.IsDefined(typeof(Wpf.Ui.Appearance.ThemeType), value))
			throw new ArgumentException("ExceptionEnumToBooleanConverterValueMustBeAnEnum");

		object enumValue = Enum.Parse(typeof(Wpf.Ui.Appearance.ThemeType), enumString);

		return enumValue.Equals(value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is not string enumString)
			throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");

		return Enum.Parse(typeof(Wpf.Ui.Appearance.ThemeType), enumString);
	}
}