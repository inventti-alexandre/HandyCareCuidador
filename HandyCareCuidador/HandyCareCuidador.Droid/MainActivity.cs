using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using HandyCareCuidador.Data;
using Microsoft.WindowsAzure.MobileServices;

namespace HandyCareCuidador.Droid
{
    public class ThemeSelector:Activity
    {
        public  string ThemeVersion()
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                return "@android:style/Theme.Material.Light";
            }
            else
            {
                return "@android:style/Theme.Holo.Light";
            }
        }
    }
    //Microsoft.WindowsAzure.Mobile;Microsoft.WindowsAzure.Mobile.Ext
    [Activity(Label = "Handy Care - Cuidador", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light",/* MainLauncher = true,*/ ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, App.IAuthenticate
    {
        private MobileServiceUser user;
        public async Task<bool> Authenticate(MobileServiceAuthenticationProvider provider)
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook logcin using a server-managed flow.
                user = await CuidadorRestService.DefaultManager.CurrentClient.LoginAsync(this,
                    provider);
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
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

            return success;
        }

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            CurrentPlatform.Init();
            App.Init((App.IAuthenticate)this);
            LoadApplication(new App());
        }
    }
}

