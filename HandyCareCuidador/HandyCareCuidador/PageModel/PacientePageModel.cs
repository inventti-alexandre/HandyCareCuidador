using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class PacientePageModel:FreshBasePageModel
    {
        //IPacienteRestService _restService;
        //private IMotivoCuidadoRestService _motivoCuidadoRestService;
        //private ICuidadorPacienteRestService _cuidadorPacienteRestService;
        //private IPeriodoTratamentoRestService _periodoTratamentoRestService;
        public bool deleteVisible = true;
        public bool novoItem = true;
        public Paciente Paciente { get; set; }
        public MotivoCuidado MotivoCuidado { get; set; }
        public PeriodoTratamento PeriodoTratamento { get; set; }
        public HorarioViewModel oHorario { get; set; }
        public PacientePageModel()
        {
            //_restService = FreshIOC.Container.Resolve<IPacienteRestService>();
            //_motivoCuidadoRestService = FreshIOC.Container.Resolve<IMotivoCuidadoRestService>();
            //_cuidadorPacienteRestService = FreshIOC.Container.Resolve<ICuidadorPacienteRestService>();
            //_periodoTratamentoRestService = FreshIOC.Container.Resolve<IPeriodoTratamentoRestService>();
        }
        public override async void Init(object initData)
        {
            base.Init(initData);
            Paciente = new Paciente();
            PeriodoTratamento = new PeriodoTratamento();
            MotivoCuidado = new MotivoCuidado();
            oHorario = new HorarioViewModel { ActivityRunning = true, Visualizar = false ,VisualizarTermino = false};
            Paciente = initData as Paciente;
            await GetInfo();
            if (Paciente == null)
            {
                deleteVisible = false;
                novoItem = true;
            }
            else
            {
                novoItem = false;
            }
            oHorario.ActivityRunning = false;
            oHorario.Visualizar = true;
        }

        private async Task GetInfo()
        {
            try
            {
                await Task.Run(async () =>
                {
                    var pacresult = new ObservableCollection<CuidadorPaciente>(await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync())
                        .FirstOrDefault(e => e.PacId == Paciente.Id);
                    PeriodoTratamento = new ObservableCollection<PeriodoTratamento>(await CuidadorRestService.DefaultManager.RefreshPeriodoTratamentoAsync())
                        .FirstOrDefault(e => e.Id == pacresult.CuiPeriodoTratamento);
                    if (PeriodoTratamento.PerTermino != null)
                    {
                        oHorario.Data = PeriodoTratamento.PerTermino.Value;
                        oHorario.VisualizarTermino = true;
                    }
                    var selection = new ObservableCollection<MotivoCuidado>(await CuidadorRestService.DefaultManager.RefreshMotivoCuidadoAsync());
                    MotivoCuidado = selection.FirstOrDefault(e => e.Id.Contains(Paciente.PacMotivoCuidado));
                    //MotivosCuidados = new ObservableCollection<MotivoCuidado>(result);

                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

    protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            //oHorario = new HorarioViewModel();
        }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    //Paciente.MatQuantidade = Convert.ToInt32(oHorario.Quantidade);
                    await CuidadorRestService.DefaultManager.SavePacienteAsync(Paciente, novoItem);
                    await CoreMethods.PopPageModel(Paciente);
                });
            }
        }
        public Command DeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CuidadorRestService.DefaultManager.DeletePacienteAsync(Paciente);
                    await CoreMethods.PopPageModel(Paciente);
                });
            }
        }
    }
}
