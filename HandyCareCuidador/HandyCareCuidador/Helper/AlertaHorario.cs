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
        private bool tocou=false;
        public async Task RunCounter(CancellationToken token)
        {
            while (true)
            {
                await Task.Run(async () =>
                {
                    await App.GetAfazeres(false);
                    foreach (var item in App.Afazeres)
                    {
                        if ((Math.Abs((item.AfaHorarioPrevisto - DateTime.Now).TotalMinutes) < 1) &&(tocou==false))
                        {
                            DependencyService.Get<ILocalNotifications>().SendLocalNotification(
            "Handy Care - Cuidador",
            "Fazer " + item.AfaObservacao + " de " + item.AfaHorarioPrevisto +" às " + item.AfaHorarioPrevistoTermino,
            item.AfaHorarioPrevisto.Ticks);
                            tocou = true;
                        }
                    }
                }, token);
                tocou = false;
                await Task.Delay(35000);
            }
        }
    }
}