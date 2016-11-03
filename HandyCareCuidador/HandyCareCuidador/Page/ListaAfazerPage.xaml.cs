using System.Globalization;
using Syncfusion.SfCalendar.XForms;
using Syncfusion.SfSchedule.XForms;
using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class ListaAfazerPage : ContentPage
    {
        public ListaAfazerPage()
        {
            InitializeComponent();
            Calendar.Locale = new CultureInfo("pt-BR");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            lstAfazer.ClearValue(ListView.SelectedItemProperty);
        }
    }
}