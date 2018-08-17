using SendArchives.Files;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace SendArchives.Converters
{
    class ConverterCollectionFilesToTotalSize : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == DependencyProperty.UnsetValue || (int)values[1] == 0)
            {
                return "0";
            }
            var a = ((ObservableCollection<FileSpecification>)values[0]).Sum(s => s.Size);
            return String.Format("{0:N}", a / 1048576.0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
