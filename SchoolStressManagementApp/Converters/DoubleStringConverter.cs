using System.Globalization;

namespace SchoolStressManagementApp.Converters;

public class DoubleStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString() ?? "0";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string? strValue = value as string;
        if (string.IsNullOrWhiteSpace(strValue))
            return null;
        if (double.TryParse(value as string, out double result))
            return result;
        return null;
    }
}