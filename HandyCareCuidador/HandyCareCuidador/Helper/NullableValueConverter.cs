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
            float result;
            if (!float.TryParse(value.ToString(), out result)) return null;
            float? resulto = result;
            return resulto;
        }

        #endregion
    }
}

//!string.IsNullOrWhiteSpace(value.ToString()) && 