using System;
using System.Globalization;
using System.Windows.Data;

namespace SendArchives.Converters
{
    class ConverterFileToIconImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            System.Windows.Media.ImageSource icon;
            try
            {
                using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon((string)value))
                {
                    icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                              sysicon.Handle,
                              System.Windows.Int32Rect.Empty,
                              System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                }
                return icon;
            }
            catch
            {
                return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
