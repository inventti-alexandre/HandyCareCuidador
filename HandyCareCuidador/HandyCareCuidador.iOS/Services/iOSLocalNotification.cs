using System;
using HandyCareCuidador.iOS.Services;
using HandyCareCuidador.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSLocalNotification))]

namespace HandyCareCuidador.iOS.Services
{
    public class iOSLocalNotification : ILocalNotifications
    {
        public void SendLocalNotification(string title, string description, long time)
        {
            throw new NotImplementedException();
        }
    }
}