using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using HandyCareCuidador.Data;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin;

namespace HandyCareCuidador.UWP
{
    public sealed partial class MainPage : HandyCareCuidador.App.IAuthenticate
    {
        private MobileServiceUser user;

        public MainPage()
        {
            InitializeComponent();
            FormsMaps.Init(
                "tXakENkxNjW1qFQiJRLW~yQ_ShKb5OpjWdZgk71L9Vw~An7iTGagwyp_wS_UaGiToWHo55PM90MaAVUKrceSfmxHHmKb4ZYZm8i88jeTh-hs");
            HandyCareCuidador.App.Init(this);
            LoadApplication(new HandyCareCuidador.App());
        }

        public async Task<bool> Authenticate(MobileServiceAuthenticationProvider provider)
        {
            var success = false;
            var message = string.Empty;
            try
            {
                user = await CuidadorRestService.DefaultManager.CurrentClient.LoginAsync(provider);
                if (user != null)
                {
                    message = $"you are now signed-in as {user.UserId}.";
                    success = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            // Display the success or failure message.
            await new MessageDialog(message, "Sign-in result").ShowAsync();

            return success;
        }
    }
}