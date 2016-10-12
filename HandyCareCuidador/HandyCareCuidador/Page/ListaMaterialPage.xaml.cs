using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class ListaMaterialPage : ContentPage
    {
        public ListaMaterialPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var lstMaterial = this.FindByName<ListView>("lstMaterial");

            if (lstMaterial != null)
                lstMaterial.ClearValue(ListView.SelectedItemProperty);
        }
    }
}