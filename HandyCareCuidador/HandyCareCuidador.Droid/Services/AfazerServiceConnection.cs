using System;
using Android.Content;
using Android.OS;
using Android.Util;
using Object = Java.Lang.Object;

namespace HandyCareCuidador.Droid.Services
{
    public class AfazerServiceConnection : Object, IServiceConnection
    {
        protected HorarioAfazerBinder binder;

        public AfazerServiceConnection(HorarioAfazerBinder binder)
        {
            if (binder != null)
                this.binder = binder;
        }

        public HorarioAfazerBinder Binder
        {
            get { return binder; }
            set { binder = value; }
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            var serviceBinder = service as HorarioAfazerBinder;
            if (serviceBinder != null)
            {
                binder = serviceBinder;
                binder.IsBound = true;
                Log.Debug("Service Connection", "OnServiceConnected Called");
                ServiceConnected(this, new ServiceConnectedEventArgs {Binder = service});
                serviceBinder.Service.VerificaHorarios();
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<ServiceConnectedEventArgs> ServiceConnected = delegate { };
    }
}