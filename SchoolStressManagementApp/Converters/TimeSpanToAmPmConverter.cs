using System.Globalization;

namespace SchoolStressManagementApp.Converters;

public class TimeSpanToAmPmConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TimeSpan ts)
        {
            // Convert TimeSpan to a DateTime to use AM/PM formatting
            return DateTime.Today.Add(ts).ToString("hh:mm tt");
        }
        return "__:__ __";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
