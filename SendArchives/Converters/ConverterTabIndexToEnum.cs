using System;
using System.Globalization;
using System.Windows.Data;

namespace SendArchives.Converters
{
    public class ConverterTabIndexToEnum : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value!=null && value is Enumerations.TabMainWindow)
            {
                return (int)value;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Enumerations.TabMainWindow)value;
        }
    }
}
