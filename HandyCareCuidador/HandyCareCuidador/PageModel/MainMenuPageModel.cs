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
using HandyCareCuidador.Page;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    public class MainMenuPageModel : FreshBasePageModel
    {
        //private IPacienteRestService _pacienteRestService;
        //private ICuidadorPacienteRestService _cuidadorPacienteRestService;
        //private ICuidadorRestService _cuidadorRestService;
        private string Id { get; set; }
        private Paciente _selectedPaciente;
        public ObservableCollection<Paciente> Pacientes { get; set; }
        public HorarioViewModel oHorario { get; set; }
        public  Paciente oPaciente { get; set; }
        public ObservableCollection<CuidadorPaciente> CuidadorPacientes { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }

        public MainMenuPageModel()
        {
            //_pacienteRestService = FreshIOC.Container.Resolve<IPacienteRestService>();
            //_cuidadorRestService = FreshIOC.Container.Resolve<ICuidadorRestService>();
            //_cuidadorPacienteRestService = FreshIOC.Container.Resolve<ICuidadorPacienteRestService>();
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            Id = initData as string;
        }

        protected override async void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            oPaciente = new Paciente();
            oHorario = new HorarioViewModel { ActivityRunning = true, Visualizar = false };
            await GetPacientes();
        }

        private async Task GetPacientes()
        {
            try
            {
                await Task.Run(async () =>
                {
                    //CuidadorRestService.DefaultManager.RefreshPacienteAsync();
                    var result = new ObservableCollection<Paciente>(await CuidadorRestService.DefaultManager.RefreshPacienteAsync());
                    CuidadorPaciente = new ObservableCollection<CuidadorPaciente>(await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync()).FirstOrDefault(e=>e.CuiId==Id);
                    if (result.Count > 0)
                    {
                        var pacientes =
                            result.Where(e => CuidadorPaciente.PacId.Contains(e.Id)).AsEnumerable();
                        Pacientes=new ObservableCollection<Paciente>(pacientes);
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
        public Command ShowAfazeres
        {
            get
            {
                return new Command(async () => {
                    if (SelectedPaciente != null)
                    {
                        await CoreMethods.PushPageModel<ListaAfazerPageModel>(SelectedPaciente);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                                                       "Selecione um paciente","OK");
                    }
                });
            }
        }
        public Command ShowPaciente
        {
            get
            {
                return new Command(async () => {
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
        public Command ShowMateriais
        {
            get
            {
                return new Command(async () => {
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
            get
            {
                return _selectedPaciente;
            }
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
                return new Command(async () => {//ENVIAR ID DO PACIENTE
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
    }
}
