using System;
using System.Globalization;
using Xamarin.Forms;

namespace HandyCareCuidador
{
    public class NullableValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? string.Empty : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int result;
            if (int.TryParse(value.ToString(), out result))
            {
                int? resulto = result;
                return resulto;
            }
            return null;
        }

        #endregion
    }
}

//!string.IsNullOrWhiteSpace(value.ToString()) && 