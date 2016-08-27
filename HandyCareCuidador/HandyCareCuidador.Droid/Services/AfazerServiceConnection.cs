using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace HandyCareCuidador.Droid.Services
{
    public class AfazerServiceConnection:Java.Lang.Object, IServiceConnection
    {
        public event EventHandler<ServiceConnectedEventArgs> ServiceConnected = delegate { };
        protected HorarioAfazerBinder binder;
        public HorarioAfazerBinder Binder
        {
            get { return this.binder; }
            set { this.binder = value; }
        }
        public AfazerServiceConnection(HorarioAfazerBinder binder)
        {
            if (binder != null)
            {
                this.binder = binder;
            }
        }
        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            var serviceBinder = service as HorarioAfazerBinder;
            if (serviceBinder != null)
            {
                this.binder = serviceBinder;
                this.binder.IsBound = true;
                Log.Debug("Service Connection", "OnServiceConnected Called");
                this.ServiceConnected(this, new ServiceConnectedEventArgs() {Binder=service});
                serviceBinder.Service.VerificaHorarios();
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            throw new NotImplementedException();
        }
    }
}