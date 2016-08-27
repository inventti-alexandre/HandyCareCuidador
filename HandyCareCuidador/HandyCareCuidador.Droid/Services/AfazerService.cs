using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HandyCareCuidador.Services;
using Xamarin.Forms;
using Application = Android.App.Application;

namespace HandyCareCuidador.Droid.Services
{
    [Service]
    public class AfazerService:Service
    {
        CancellationTokenSource _cts;
        private Context _context;
        public AfazerService()
        {

        }

        private IBinder _binder;
        public override IBinder OnBind(Intent intent)
        {
            _binder=new HorarioAfazerBinder(this);
            return _binder;
        }

        public void VerificaHorarios()
        {
            _cts = new CancellationTokenSource();

            var a = new Thread(() =>
            {
                Task.Run(async () => {
                    try
                    {
                        var counter = new AlertaHorario();
                        await counter.RunCounter(_cts.Token);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }, _cts.Token);
            });
            a.Start();
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
 

            return StartCommandResult.Sticky;
        }
    }
}