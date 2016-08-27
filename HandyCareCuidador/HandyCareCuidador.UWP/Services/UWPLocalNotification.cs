using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCareCuidador.Services;
using HandyCareCuidador.UWP.Services;
[assembly: Xamarin.Forms.Dependency(typeof(UWPLocalNotification))]

namespace HandyCareCuidador.UWP.Services
{
    class UWPLocalNotification:ILocalNotifications
    {
        public void SendLocalNotification(string title, string description, long time)
        {
            throw new NotImplementedException();
        }
    }
}
