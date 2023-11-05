// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgDownloaderWinDesktop.Helpers;

public sealed class VisibilityToBooleanConverter : IValueConverter
{
	public VisibilityToBooleanConverter()
	{
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is not string enumString)
			throw new ArgumentException($"Exception \tpublic {nameof(VisibilityToBooleanConverter)}()\r\n");

		if (!Enum.IsDefined(typeof(Visibility), value))
			throw new ArgumentException($"Exception \tpublic {nameof(VisibilityToBooleanConverter)}()\r\n");

		object enumValue = Enum.Parse(typeof(Visibility), enumString);

		return enumValue.Equals(value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is not string enumString)
			throw new ArgumentException($"Exception {nameof(VisibilityToBooleanConverter)}");

		return Enum.Parse(typeof(Visibility), enumString);
	}
}