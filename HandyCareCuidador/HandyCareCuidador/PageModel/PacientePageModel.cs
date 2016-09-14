using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class PacientePageModel : FreshBasePageModel
    {
        //IPacienteRestService _restService;
        //private IMotivoCuidadoRestService _motivoCuidadoRestService;
        //private ICuidadorPacienteRestService _cuidadorPacienteRestService;
        //private IPeriodoTratamentoRestService _periodoTratamentoRestService;
        public bool deleteVisible = true;
        public bool novoItem = true;

        public Paciente Paciente { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        public MotivoCuidado MotivoCuidado { get; set; }
        public Cuidador Cuidador { get; set; }
        public PeriodoTratamento PeriodoTratamento { get; set; }
        public HorarioViewModel oHorario { get; set; }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!oHorario.VisualizarTermino)
                        PeriodoTratamento.PerTermino = null;
                    Paciente.Id = Guid.NewGuid().ToString();
                    CuidadorPaciente.Id = Guid.NewGuid().ToString();
                    CuidadorPaciente.PacId = Paciente.Id;
                    CuidadorPaciente.CuiId = Cuidador.Id;
                    PeriodoTratamento.Id= Guid.NewGuid().ToString();
                    CuidadorPaciente.CuiPeriodoTratamento = PeriodoTratamento.Id;
                    await CuidadorRestService.DefaultManager.SavePacienteAsync(Paciente, oHorario.NovoPaciente);
                    await CuidadorRestService.DefaultManager.SavePeriodoTratamentoAsync(PeriodoTratamento, oHorario.NovoPaciente);
                    await CuidadorRestService.DefaultManager.SaveCuidadorPacienteAsync(CuidadorPaciente, oHorario.NovoPaciente);
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

        public override async void Init(object initData)
        {
            base.Init(initData);
            Cuidador=new Cuidador();
            Paciente = new Paciente();
            PeriodoTratamento = new PeriodoTratamento
            {
                PerInicio = DateTime.Now,
                PerTermino = DateTime.Now
            };
            MotivoCuidado = new MotivoCuidado();
            oHorario = new HorarioViewModel {ActivityRunning = true, Visualizar = false, VisualizarTermino = false};
            var x = initData as Tuple<Paciente, bool,Cuidador>;
            if (x != null)
            {
                oHorario.NovoPaciente = x.Item2;
                oHorario.DadoPaciente = false;
                Cuidador = x.Item3;
            }
            if (!oHorario.NovoPaciente)
            {
                await GetInfo();
                if (x?.Item1 != null)
                {
                    Paciente = x.Item1;
                }
                oHorario.DadoPaciente = true;
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
                    var pacresult = new ObservableCollection<CuidadorPaciente>(
                        await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync())
                        .FirstOrDefault(e => e.PacId == Paciente.Id);
                    PeriodoTratamento = new ObservableCollection<PeriodoTratamento>(
                        await CuidadorRestService.DefaultManager.RefreshPeriodoTratamentoAsync())
                        .FirstOrDefault(e => e.Id == pacresult.CuiPeriodoTratamento);
                    if (PeriodoTratamento.PerTermino != null)
                    {
                        oHorario.Data = PeriodoTratamento.PerTermino.Value;
                        oHorario.VisualizarTermino = true;
                    }
                    var selection =
                        new ObservableCollection<MotivoCuidado>(
                            await CuidadorRestService.DefaultManager.RefreshMotivoCuidadoAsync());
                    MotivoCuidado = selection.FirstOrDefault(e => e.Id.Contains(Paciente.PacMotivoCuidado));
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
    }
}