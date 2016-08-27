using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    public class MainMenuPageModel : FreshBasePageModel
    {
        private Paciente _selectedPaciente;

        //private IPacienteRestService _pacienteRestService;
        //private ICuidadorPacienteRestService _cuidadorPacienteRestService;
        //private ICuidadorRestService _cuidadorRestService;
        private Cuidador Cuidador { get; set; }
        public ObservableCollection<Paciente> Pacientes { get; set; }
        public HorarioViewModel oHorario { get; set; }
        public Paciente oPaciente { get; set; }
        public ObservableCollection<CuidadorPaciente> CuidadorPacientes { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }

        public Command ShowCuidador
        {
            get
            {
                return new Command(async () =>
                {
                        await CoreMethods.PushPageModel<CuidadorPageModel>(Cuidador);
                });
            }
        }
        public Command ShowAfazeres
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        await CoreMethods.PushPageModel<ListaAfazerPageModel>(SelectedPaciente);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Command ShowPaciente
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        await CoreMethods.PushPageModel<PacientePageModel>(SelectedPaciente);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Command AlertarContatos
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        //ENVIAR PUSH NOTIFICATION PARA OS FAMILIARES
                        Device.OpenUri(new Uri("tel:0"));
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Command ShowMateriais
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        await CoreMethods.PushPageModel<ListaMaterialPageModel>(SelectedPaciente);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Paciente SelectedPaciente
        {
            get { return _selectedPaciente; }
            set
            {
                _selectedPaciente = value;
                if (value != null)
                {
                    //ShowMedicamentos.Execute(value);
                    //SelectedPaciente = null;
                }
            }
        }

        public Command ShowMedicamentos
        {
            get
            {
                return new Command(async () =>
                {
//ENVIAR ID DO PACIENTE
                    if (SelectedPaciente != null)
                    {
                        await CoreMethods.PushPageModel<ListaMedicamentoPageModel>(SelectedPaciente);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }
        public Command ShowMapa
        {
            get
            {
                return new Command(async () =>
                {
                        await CoreMethods.PushPageModel<MapPageModel>();
                });
            }
        }

        public override void Init(object initData)
        {
            try
            {
                base.Init(initData);
                Cuidador=new Cuidador();
                Cuidador = initData as Cuidador;

            }
            catch (NullReferenceException e)
            {
                    Debug.WriteLine("Ih, rapaz {0}", e.Message);
                throw;
            }
        }

        protected override async void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            oPaciente = new Paciente();
            oHorario = new HorarioViewModel {ActivityRunning = true, Visualizar = false};
            await GetPacientes();
        }

        private async Task GetPacientes()
        {
            try
            {
                await Task.Run(async () =>
                {
                    //CuidadorRestService.DefaultManager.RefreshPacienteAsync();
                    CuidadorPaciente = new CuidadorPaciente();
                    var result =
                        new ObservableCollection<Paciente>(
                            await CuidadorRestService.DefaultManager.RefreshPacienteAsync(true));
                    CuidadorPaciente =
                        new ObservableCollection<CuidadorPaciente>(
                            await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync(true)).FirstOrDefault(
                                e => e.CuiId == Cuidador.Id);
                    if (result.Count > 0)
                    {
                        var pacientes =
                            result.Where(e => CuidadorPaciente.PacId.Contains(e.Id)).AsEnumerable();
                        Pacientes= new ObservableCollection<Paciente>();
                        Pacientes = new ObservableCollection<Paciente>(pacientes);
                    }
                    oHorario.ActivityRunning = false;
                    oHorario.Visualizar = true;
                });
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }
    }
}