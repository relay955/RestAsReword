using System.Globalization;
using System.Windows.Data;

namespace RestAsReward.utils;

public abstract class SimpleConverter<T> : IValueConverter {

    public abstract object Convert(T value);
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is not T tValue) return null;
        return Convert(tValue);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
}
