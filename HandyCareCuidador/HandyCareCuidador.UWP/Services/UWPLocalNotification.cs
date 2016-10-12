using System;
using HandyCareCuidador.Services;
using HandyCareCuidador.UWP.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(UWPLocalNotification))]

namespace HandyCareCuidador.UWP.Services
{
    internal class UWPLocalNotification : ILocalNotifications
    {
        public void SendLocalNotification(string title, string description, long time)
        {
            throw new NotImplementedException();
        }
    }
}