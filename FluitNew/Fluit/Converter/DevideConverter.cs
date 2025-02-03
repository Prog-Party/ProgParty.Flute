using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Fluit.Converter
{
    public class DevideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val, param;
            if (value == null || !double.TryParse(value.ToString(), out val) ||
                parameter == null || !double.TryParse(parameter.ToString(), out param))
                return value;
            if (param == 0)
                return value;
            return val*param;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val, param;
            if (value == null || !double.TryParse(value.ToString(), out val) ||
                parameter == null || !double.TryParse(parameter.ToString(), out param))
                return value;
            if (param == 0)
                return value;
            return val / param;
        }
    }
}
