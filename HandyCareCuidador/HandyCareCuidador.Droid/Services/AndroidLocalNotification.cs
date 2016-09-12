using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using HandyCareCuidador.Droid.Services;
using HandyCareCuidador.Services;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidLocalNotification))]

namespace HandyCareCuidador.Droid.Services
{
    public class AndroidLocalNotification:ILocalNotifications
    {
        //private NotificationManager notificationManager;
        #region ILocalNotifications implementation

        public void SendLocalNotification(string title, string description, long time)
        {
            var builder = new Notification.Builder(Application.Context)
                .SetContentTitle(title)
                .SetContentText(description)
                .SetWhen(time)
                .SetDefaults(NotificationDefaults.All)
                .SetSmallIcon(Resource.Drawable.icon);
            var notification = builder.Build();
            Intent resultIntent = new Intent(Application.Context, typeof(AfazerService));
            resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(Application.Context);
            stackBuilder.AddNextIntent(resultIntent);
            var resultPendingIntent =
                stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);
            builder.SetContentIntent(resultPendingIntent);

            var notificationManager =
                Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
            const int notificationId = 0;
            notificationManager?.Notify(notificationId, notification);

        }
        #endregion
    }
}