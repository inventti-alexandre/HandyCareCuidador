using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    public class MainMenuPageModel : FreshBasePageModel
    {
        private IPacienteRestService _pacienteRestService;
        private ICuidadorPacienteRestService _cuidadorPacienteRestService;
        public ObservableCollection<Paciente> Pacientes { get; set; }
        public  Paciente oPaciente { get; set; }
        public ObservableCollection<CuidadorPaciente> CuidadorPacientes { get; set; }
        public MainMenuPageModel()
        {
            _pacienteRestService = FreshIOC.Container.Resolve<IPacienteRestService>();
            _cuidadorPacienteRestService = FreshIOC.Container.Resolve<ICuidadorPacienteRestService>();
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
        }

        protected override async void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            oPaciente = new Paciente();
            await GetPacientes();

        }

        private async Task GetPacientes()
        {
            try
            {
                await Task.Run(async () =>
                {
                    CuidadorPacientes = new ObservableCollection<CuidadorPaciente>(await _cuidadorPacienteRestService.RefreshDataAsync());
                    var result = new ObservableCollection<Paciente>(await  _pacienteRestService.RefreshDataAsync());
                    if (result.Count > 0)
                    {
                        var pacientes =
                            result.Where(e => CuidadorPacientes.Select(m => m.PacId).Contains(e.Id)).AsEnumerable();
                        Pacientes=new ObservableCollection<Paciente>(pacientes);
                    }
                });

            }
            catch (Exception)
            {
                    
                throw;
            }
        }

        public Command ShowAfazeres
        {
            get
            {
                return new Command(async () => {
                                                   if (oPaciente.Id != null)
                                                   {
                        var afazeresTab = new FreshTabbedNavigationContainer("Afazeres");
                        afazeresTab.AddTab<ListaAfazerPageModel>("Afazeres", null);
                        afazeresTab.AddTab<ListaAfazerConcluidoPageModel>("Concluídos", null);
                        await CoreMethods.PushNewNavigationServiceModal(afazeresTab);
                    }
                                                   else
                                                   {
                                                       await
                                                           CoreMethods.DisplayAlert("Informação",
                                                               "Selecione um paciente","OK");
                                                   }
                });
            }
        }

        public Command ShowMateriais
        {
            get
            {
                return new Command(async () => {
                    await CoreMethods.PushPageModel<ListaMaterialPageModel>();
                    
                });
            }
        }
        public Command ShowMedicamentos
        {
            get
            {
                return new Command(async () => {
                    await CoreMethods.PushPageModel<ListaMedicamentoPageModel>();
                });
            }
        }

    }
}
