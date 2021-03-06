﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SendArchives.Converters
{
    class ConverterNullToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == DependencyProperty.UnsetValue)
            {
                return Visibility.Collapsed;
            }
            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
