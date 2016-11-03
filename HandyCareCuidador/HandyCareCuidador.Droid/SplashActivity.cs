using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace HandyCareCuidador.Droid
{
    [Activity(Label = "Handy Care - Cuidador", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SplashLayout);
            ThreadPool.QueueUserWorkItem(async o => await StartApp());
        }

        private async Task StartApp()
        {
            await Task.Run(() =>
            {
                var watch = Stopwatch.StartNew();
                RunOnUiThread(() => { StartActivity(new Intent(this, typeof(MainActivity))); });
                watch.Stop();
                Thread.Sleep(Convert.ToInt32(watch.ElapsedMilliseconds));
            });
        }
    }
}