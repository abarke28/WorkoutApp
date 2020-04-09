using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using WorkoutApp.Model;

namespace WorkoutApp.ViewModel.Converters
{
    public class EnumToRadioBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string parameterString)) return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false) return DependencyProperty.UnsetValue;

            object parameterValue = Enum.Parse(value.GetType(), parameterString);

            return parameterValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string parameterString)) return DependencyProperty.UnsetValue;

            return Enum.Parse(targetType, parameterString);
        }
    }
}