using System;
using System.Collections.Generic;
using System.Text;
using HandyCareCuidador.iOS.Services;
using HandyCareCuidador.Services;

[assembly: Xamarin.Forms.Dependency(typeof(iOSLocalNotification))]

namespace HandyCareCuidador.iOS.Services
{
    public class iOSLocalNotification:ILocalNotifications
    {
        public void SendLocalNotification(string title, string description, long time)
        {
            throw new NotImplementedException();
        }
    }
}
