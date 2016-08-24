using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HandyCareCuidador;
using HandyCareCuidador.Data;
using Microsoft.WindowsAzure.MobileServices;

namespace HandyCareCuidador.UWP
{
    public sealed partial class MainPage:HandyCareCuidador.App.IAuthenticate
    {
        private MobileServiceUser user;
        public MainPage()
        {
            this.InitializeComponent();
           // Xamarin.FormsMaps.Init("tXakENkxNjW1qFQiJRLW~yQ_ShKb5OpjWdZgk71L9Vw~An7iTGagwyp_wS_UaGiToWHo55PM90MaAVUKrceSfmxHHmKb4ZYZm8i88jeTh-hs");
            HandyCareCuidador.App.Init(this);
            LoadApplication(new HandyCareCuidador.App());
        }

        public async Task<bool> Authenticate(MobileServiceAuthenticationProvider provider)
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook logcin using a server-managed flow.
                user = await CuidadorRestService.DefaultManager.CurrentClient.LoginAsync(provider);
                //await
                //    CuidadorRestService.DefaultManager.CurrentClient.LoginAsync(
                //        MobileServiceAuthenticationProvider.Google);
                if (user != null)
                {/*SALVAR userid no banco. Quando o usuário autenticar-se, deverá realizar uma pesquisa no banco para verificar se esse Id já consta lá.
                    Se sim, exibir a página principal. Caso contrário, exibir a tela de cadastro de cuidador*/
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
