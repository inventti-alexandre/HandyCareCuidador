using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class VideoPage : ContentPage
    {
        public VideoPage()
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