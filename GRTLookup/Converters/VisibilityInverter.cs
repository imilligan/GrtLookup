using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace GRTLookup.Converters
{
    public class VisibilityInverter : IValueConverter
    {

        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is Boolean))
            {
                throw new ArgumentException("Value must be a boolean");
            }
            else
            {
                if ((Boolean)value)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is Visibility))
            {
                throw new ArgumentException("Value must be a Visibility");
            }
            else
            {
                if (((Visibility)(value)) == Visibility.Visible)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        #endregion
    }
}
