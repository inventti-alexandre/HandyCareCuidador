using System;
using System.Threading;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Android.Content;
using Android.Provider;
using Android.Support.V4.OS;
using HandyCareCuidador.Data;
using HandyCareCuidador.Droid.Services;
using HandyCareCuidador.Message;
using HandyCareCuidador.Model;
using HandyCareCuidador.Services;
using Java.IO;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Permissions;
using Xamarin.Forms;

namespace HandyCareCuidador.Droid
{
    //Microsoft.WindowsAzure.Mobile;Microsoft.WindowsAzure.Mobile.Ext
    [Activity(Label = "Handy Care - Cuidador", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light",/* MainLauncher = true,*/ ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, App.IAuthenticate
    {
        public event EventHandler<ServiceConnectedEventArgs> AfazerServiceConnected = delegate { };

        private static readonly File FotoFile = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(
                                Android.OS.Environment.DirectoryPictures), DateTime.Now.ToString() + "handycare.jpg");
        private static readonly File VideoFile = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(
                        Android.OS.Environment.DirectoryMovies), DateTime.Now.ToString() + "handycare.mp4");

        // declarations
        protected readonly string logTag = "App";
        protected static AfazerServiceConnection afazerServiceConnection;
        private MobileServiceUser user;

        public AfazerService AfazerService
        {
            get
            {
                if (afazerServiceConnection.Binder == null)
                {
                    throw new Java.Lang.Exception("Fodeu");
                }
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
                    message = $"you are now signed-in as {user.UserId}.";
                    success = true;
                    var a = new Thread(() =>
                    {
                        ThreadPool.QueueUserWorkItem(async o => await StartAfazerService());
                    });
                    a.Start();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();
            return success;
        }

        public async Task StartAfazerService()
        {
            afazerServiceConnection=new AfazerServiceConnection(null);
            afazerServiceConnection.ServiceConnected += (object sender, ServiceConnectedEventArgs e) =>
            {
                this.AfazerServiceConnected(this, e);
            };
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
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            CurrentPlatform.Init();
            App.Init((App.IAuthenticate)this);
            LoadApplication(new App());
            var app = Xamarin.Forms.Application.Current as App;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            var app = Xamarin.Forms.Application.Current as App;
            app?.ShowImage(FotoFile.Path);
        }
    }
}

