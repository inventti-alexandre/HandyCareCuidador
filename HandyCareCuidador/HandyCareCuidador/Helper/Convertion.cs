using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.SfCalendar.XForms;
using Xamarin.Forms;

namespace HandyCareCuidador.Helper
{
    public class Convertion : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as InlineToggledEventArgs;
            return eventArgs.selectedAppointment;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
