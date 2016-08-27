using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HandyCareCuidador.Droid.Services
{
    [BroadcastReceiver]
    public class NotificationPublisher : BroadcastReceiver
    {
        private string _title;
        private string _description;
        private long _time;
        public NotificationPublisher()
        {

        }
        public override void OnReceive(Context context, Intent intent)
        {
            //
            NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);

            //
            Intent resultIntent = new Intent(context, typeof(AfazerService));
            /*TaskStackBuilder stackBuilder = TaskStackBuilder.Create(context);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(AfazerService)));
            stackBuilder.AddNextIntent(resultIntent);*/            

            //
            //PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);
            //
            Notification.Builder builder = new Notification.Builder(context);
            builder.SetAutoCancel(true);
            //builder.SetContentIntent(resultPendingIntent);
            builder.SetContentTitle(_title);
            builder.SetContentText(_description);
            builder.SetDefaults(NotificationDefaults.All);
            builder.SetSmallIcon(Resource.Drawable.icon);

            //
            notificationManager.Notify(100, builder.Build());
        }
    }
}