using Android.App;
using Android.Content;
using HandyCareCuidador.Droid.Services;
using HandyCareCuidador.Services;
using Xamarin.Forms;
using Application = Android.App.Application;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;

[assembly: Dependency(typeof(AndroidLocalNotification))]

namespace HandyCareCuidador.Droid.Services
{
    public class AndroidLocalNotification : ILocalNotifications
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
            var resultIntent = new Intent(Application.Context, typeof(AfazerService));
            resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            var stackBuilder = TaskStackBuilder.Create(Application.Context);
            stackBuilder.AddNextIntent(resultIntent);
            var resultPendingIntent =
                stackBuilder.GetPendingIntent(0, (int) PendingIntentFlags.UpdateCurrent);
            builder.SetContentIntent(resultPendingIntent);

            var notificationManager =
                Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
            const int notificationId = 0;
            notificationManager?.Notify(notificationId, notification);
        }

        #endregion
    }
}