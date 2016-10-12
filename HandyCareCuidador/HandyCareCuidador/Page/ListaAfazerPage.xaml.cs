using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class ListaAfazerPage : ContentPage
    {
        public ListaAfazerPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var lstAfazer = this.FindByName<ListView>("lstAfazer");
            var lstAfazerConcluido = this.FindByName<ListView>("lstAfazerConcluido");
            if (lstAfazer != null)
                lstAfazer.ClearValue(ListView.SelectedItemProperty);
            if (lstAfazerConcluido != null)
                lstAfazerConcluido.ClearValue(ListView.SelectedItemProperty);
        }
    }
}