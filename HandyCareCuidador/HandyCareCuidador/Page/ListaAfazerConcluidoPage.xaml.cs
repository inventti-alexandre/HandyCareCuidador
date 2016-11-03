using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class ListaAfazerConcluidoPage : ContentPage
    {
        public ListaAfazerConcluidoPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            lstAfazerConcluido.ClearValue(ListView.SelectedItemProperty);
        }
    }
}