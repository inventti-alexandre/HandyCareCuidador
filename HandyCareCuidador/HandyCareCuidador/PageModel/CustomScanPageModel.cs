using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using PropertyChanged;
using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace HandyCareCuidador.PageModel
{

    [ImplementPropertyChanged]
    public class CustomScanPageModel:FreshBasePageModel
    {
        public Result Teste { get; set; }
        public Cuidador Cuidador { get; set; }
        public Paciente Paciente { get; set; }
        private Tuple<Paciente, bool, Cuidador> oi;
        public PageModelHelper PageModelHelper { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        public override void Init(object initData)
        {
            base.Init(initData);
            PageModelHelper = new PageModelHelper {Visualizar = true};
            Cuidador =new Cuidador();
            Paciente = new Paciente();
            Cuidador = initData as Cuidador;
            if (Cuidador != null) CuidadorPaciente = new CuidadorPaciente {CuiId = Cuidador.Id};
        }
        public Command<Result> ScanCommand
        {
            get
            {
                return new Command<Result>(scan =>
                {

                    PageModelHelper.Visualizar = false;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Task.Run(async () =>
                        {
                            Teste = scan;
                            Paciente = new ObservableCollection<Paciente>(
                                await CuidadorRestService.DefaultManager.RefreshPacienteAsync())
                                .FirstOrDefault(e => e.Id == Teste.Text);
                        });
                        oi = new Tuple<Paciente, bool, Cuidador>(Paciente, false, Cuidador);
                        await CoreMethods.PushPageModel<PacientePageModel>(oi);
                    });
                });
            }
        }

    }
}
