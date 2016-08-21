using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Android.Support.V7.App;
using Android.Content.PM;
using System.Threading;
using System.Timers;

namespace HandyCareCuidador.Droid
{
    [Activity(Label = "Handy Care", Theme = "@style/Theme.Splash" /*"@android:style/Theme.Holo.Light.NoActionBar.Fullscreen"*/, MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.SplashLayout);
            ThreadPool.QueueUserWorkItem(async o => await StartApp());
        }

        private async Task StartApp()
        {
            await Task.Run(() =>
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                RunOnUiThread(()=> {
                    var progressBar = FindViewById<ProgressBar>(Resource.Id.progressBarSplash);
                    progressBar.Indeterminate = true;
                    progressBar.Visibility = ViewStates.Visible;
                    StartActivity(new Intent(this, typeof(MainActivity)));
                    });
                watch.Stop();
                Thread.Sleep(Convert.ToInt32(watch.ElapsedMilliseconds));
            });
        }
    }
}