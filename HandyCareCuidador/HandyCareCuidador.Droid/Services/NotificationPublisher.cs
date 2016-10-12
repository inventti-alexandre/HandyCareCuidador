using Android.App;
using Android.Content;

namespace HandyCareCuidador.Droid.Services
{
    [BroadcastReceiver]
    public class NotificationPublisher : BroadcastReceiver
    {
        private string _description;
        private long _time;
        private string _title;

        public override void OnReceive(Context context, Intent intent)
        {
            //
            var notificationManager = (NotificationManager) context.GetSystemService(Context.NotificationService);

            //
            var resultIntent = new Intent(context, typeof(AfazerService));
            /*TaskStackBuilder stackBuilder = TaskStackBuilder.Create(context);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(AfazerService)));
            stackBuilder.AddNextIntent(resultIntent);*/

            //
            //PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);
            //
            var builder = new Notification.Builder(context);
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