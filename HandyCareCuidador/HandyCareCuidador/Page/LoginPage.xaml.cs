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
            btnFacebook.Image = (FileImageSource)ImageSource.FromFile("facebook.png");
            btnGoogle.Image = (FileImageSource)ImageSource.FromFile("googleplus.png");
            //btnMicrosoft.Image = (FileImageSource)ImageSource.FromFile("microsoft.png");
            btnMicrosoftAD.Image = (FileImageSource)ImageSource.FromFile("microsoft.png");
            btnTwitter.Image = (FileImageSource)ImageSource.FromFile("twitter.png");

        }

    }
}