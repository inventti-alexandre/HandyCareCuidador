using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class AcionarContatoEmergenciaPage : ContentPage
    {
        public AcionarContatoEmergenciaPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            lstFamiliares?.ClearValue(ListView.SelectedItemProperty);
        }
    }
}