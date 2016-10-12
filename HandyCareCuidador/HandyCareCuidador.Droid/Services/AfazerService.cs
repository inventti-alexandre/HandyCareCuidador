using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;

namespace HandyCareCuidador.Droid.Services
{
    [Service]
    public class AfazerService : Service
    {
        private IBinder _binder;
        private Context _context;
        private CancellationTokenSource _cts;

        public override IBinder OnBind(Intent intent)
        {
            _binder = new HorarioAfazerBinder(this);
            return _binder;
        }

        public void VerificaHorarios()
        {
            _cts = new CancellationTokenSource();

            var a = new Thread(() =>
            {
                Task.Run(async () =>
                {
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