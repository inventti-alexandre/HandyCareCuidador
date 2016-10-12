using System;
using System.Threading;
using System.Threading.Tasks;
using HandyCareCuidador.Model;
using HandyCareCuidador.Services;
using Xamarin.Forms;

namespace HandyCareCuidador
{
    public class AlertaHorario
    {
        private Paciente Paciente { get; set; }
        private CuidadorPaciente CuidadorPaciente { get; set; }

        public async Task RunCounter(CancellationToken token)
        {
            while (true)
            {
                await Task.Run(async () =>
                {
                    await App.GetAfazeres(false);
                    foreach (var item in App.Afazeres)
                        if (Math.Abs((item.AfaHorarioPrevisto - DateTime.Now).TotalMinutes) < 1)
                            DependencyService.Get<ILocalNotifications>().SendLocalNotification(
                                "Handy Care",
                                "Fazer " + item.AfaObservacao + " em " + item.AfaHorarioPrevisto,
                                item.AfaHorarioPrevisto.Ticks);
                }, token);
                await Task.Delay(35000);
            }
        }
    }
}