using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace WorkoutApp.ViewModel.Converters
{
    class BoolToVisibilityHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                true => "Visible",
                false => "Hidden",
                _ => "Hidden"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                "Visible" => true,
                "Collapsed" => false,
                "Hidden" => false,
                _ => false
            };
        }
    }
}