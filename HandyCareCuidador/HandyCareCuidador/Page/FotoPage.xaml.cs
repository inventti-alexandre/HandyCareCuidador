using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class FotoPage : ContentPage
    {
        public FotoPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var lstFamiliares = this.FindByName<ListView>("lstFamiliares");
            lstFamiliares?.ClearValue(ListView.SelectedItemProperty);
        }
    }
}