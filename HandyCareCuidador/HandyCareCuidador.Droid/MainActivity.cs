using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

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
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            //Xamarin.FormsMaps.Init(this, bundle);
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            LoadApplication(new App());
        }
    }
}

