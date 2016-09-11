using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Logo.Source = Device.OnPlatform(null, ImageSource.FromFile("splash.png"),
                ImageSource.FromFile("splash.png"));
        }

    }
}