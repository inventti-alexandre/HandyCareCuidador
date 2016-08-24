using Android.OS;
using FreshMvvm;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]

    public class ListaMaterialPageModel:FreshBasePageModel
    {
        //IMaterialRestService _restService;
        //private ICuidadorPacienteRestService _cuidadorPacienteRestService;
        public Paciente oPaciente { get; set; }
        public HorarioViewModel oHorario { get; set; }

        public bool deleteVisible;
        public ListaMaterialPageModel()
        {
            //_restService = FreshIOC.Container.Resolve<IMaterialRestService>();
            //_cuidadorPacienteRestService = FreshIOC.Container.Resolve<ICuidadorPacienteRestService>();
        }
        public ObservableCollection<Material> Materiais { get; set; }

        public override async void Init(object initData)
        {
            base.Init(initData);
            oPaciente = new Paciente();
            oHorario = new HorarioViewModel { ActivityRunning = true, Visualizar = false };
            oPaciente = initData as Paciente;
            await GetMateriais();
        }
        public async Task GetMateriais()
        {
            try
            {
                await Task.Run(async () => {
                    var pacresult = new ObservableCollection<CuidadorPaciente>(await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync())
                    .Where(e => e.PacId == oPaciente.Id)
                    .AsEnumerable();
                    var result = new ObservableCollection<Material>(await CuidadorRestService.DefaultManager.RefreshMaterialAsync())
                    .Where(e => pacresult.Select(m => m.Id)
                    .Contains(e.MatPacId))
                    .AsEnumerable();
                    Materiais = new ObservableCollection<Material>(result);
                    if(Materiais.Count==0)
                        oHorario.Visualizar = true;
                });
                oHorario.ActivityRunning = false;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public override async void ReverseInit(object returndData)
        {
            base.ReverseInit(returndData);
            var newMaterial = returndData as Material;
            if (!Materiais.Contains(newMaterial))
            {
                Materiais.Add(newMaterial);
            }
            else
            {
                await GetMateriais();
            }
        }
        public Command AddMaterial
        {
            get
            {
                return new Command(async () =>
                {
                    deleteVisible = false;
                    await CoreMethods.PushPageModel<MaterialPageModel>();
                });
            }
        }
        Material _selectedMaterial;
        public Material SelectedMaterial
        {
            get
            {
                return _selectedMaterial;
            }
            set
            {
                _selectedMaterial = value;
                if (value != null)
                {
                    MaterialSelected.Execute(value);
                    SelectedMaterial = null;
                }
            }
        }
        public Command<Material> MaterialSelected
        {
            get
            {
                return new Command<Material>(async (material) =>
                {
                    deleteVisible = true;
                    RaisePropertyChanged("IsVisible");
                    await CoreMethods.PushPageModel<MaterialPageModel>(material);
                    material = null;
                });
            }
        }

    }
}
