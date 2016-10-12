using System;
using Android.OS;

namespace HandyCareCuidador.Droid.Services
{
    public class ServiceConnectedEventArgs : EventArgs
    {
        public IBinder Binder { get; set; }
    }
}