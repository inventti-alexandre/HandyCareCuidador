using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using HandyCareCuidador.Data;
using HandyCareCuidador.Droid.Services;
using Java.IO;
using Java.Lang;
using Microsoft.WindowsAzure.MobileServices;
using Octane.Xam.VideoPlayer.Android;
using Plugin.Permissions;
using Syncfusion.SfCalendar.XForms.Droid;
using Syncfusion.SfSchedule.XForms;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Environment = Android.OS.Environment;
using Exception = Java.Lang.Exception;
using Thread = System.Threading.Thread;

namespace HandyCareCuidador.Droid
{
    //Microsoft.WindowsAzure.Mobile;Microsoft.WindowsAzure.Mobile.Ext
    [Activity(Label = "Handy Care - Cuidador", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light",
         /* MainLauncher = true,*/ ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity, App.IAuthenticate
    {
        private static readonly File FotoFile = new File(Environment.GetExternalStoragePublicDirectory(
            Environment.DirectoryPictures), DateTime.Now + "handycare.jpg");

        private static readonly File VideoFile = new File(Environment.GetExternalStoragePublicDirectory(
            Environment.DirectoryMovies), DateTime.Now + "handycare.mp4");

        protected static AfazerServiceConnection afazerServiceConnection;

        // declarations
        protected readonly string logTag = "App";
        private MobileServiceUser user;

        public AfazerService AfazerService
        {
            get
            {
                if (afazerServiceConnection.Binder == null)
                    throw new Exception("Fodeu");
                return afazerServiceConnection.Binder.Service;
            }
        }

        public async Task<bool> Authenticate(MobileServiceAuthenticationProvider provider)
        {
            var success = false;
            var message = string.Empty;
            try
            {
                user = await CuidadorRestService.DefaultManager.CurrentClient.LoginAsync(this,
                    provider);
                if (user != null)
                {
                    message = "Login realizado com sucesso"; /*$"you are now signed-in as {user.UserId}.";*/
                    success = true;
                    var a = new Thread(() => { ThreadPool.QueueUserWorkItem(async o => await StartAfazerService()); });
                    a.Start();
                }
            }
            catch (System.Exception ex)
            {
                message = ex.Message;
            }
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Status de login");
            builder.Create().Show();
            return success;
        }

        public event EventHandler<ServiceConnectedEventArgs> AfazerServiceConnected = delegate { };

        public async Task StartAfazerService()
        {
            afazerServiceConnection = new AfazerServiceConnection(null);
            afazerServiceConnection.ServiceConnected += (sender, e) => { AfazerServiceConnected(this, e); };
            await Task.Run(() =>
            {
                StartService(new Intent(this, typeof(AfazerService)));
                var afazerServiceIntent = new Intent(this, typeof(AfazerService));
                BindService(afazerServiceIntent, afazerServiceConnection, Bind.AutoCreate);
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            FormsMaps.Init(this, bundle);
            CurrentPlatform.Init();
            App.Init(this);
            new SfScheduleRenderer();
            new SfCalendarRenderer();
            UserDialogs.Init(() => this);
            FormsVideoPlayer.Init();
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            var app = Xamarin.Forms.Application.Current as App;
            app?.ShowImage(FotoFile.Path);
        }
    }
}