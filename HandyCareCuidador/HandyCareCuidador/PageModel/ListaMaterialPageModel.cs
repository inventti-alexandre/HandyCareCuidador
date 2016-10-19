using System;
using System.Collections.ObjectModel;
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
    public class ListaMaterialPageModel : FreshBasePageModel
    {
        private Material _selectedMaterial;

        public bool deleteVisible;

        //IMaterialRestService _restService;
        //private ICuidadorPacienteRestService _cuidadorPacienteRestService;
        public Paciente oPaciente { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }

        public PageModelHelper oHorario { get; set; }
        public ObservableCollection<Material> Materiais { get; set; }

        public Command AddMaterial
        {
            get
            {
                return new Command(async () =>
                {
                    deleteVisible = false;
                    var material = new Material();
                    var x = new Tuple<Material, CuidadorPaciente>(material, CuidadorPaciente);
                    await CoreMethods.PushPageModel<MaterialPageModel>(x);
                });
            }
        }

        public Material SelectedMaterial
        {
            get { return _selectedMaterial; }
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
                return new Command<Material>(async material =>
                {
                    deleteVisible = true;
                    RaisePropertyChanged("IsVisible");
                    var x = new Tuple<Material, CuidadorPaciente>(material, CuidadorPaciente);
                    await CoreMethods.PushPageModel<MaterialPageModel>(x);
                    material = null;
                });
            }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            oPaciente = new Paciente();
            oHorario = new PageModelHelper {ActivityRunning = true, Visualizar = false};
            var x = initData as Tuple<Paciente, CuidadorPaciente>;
            oPaciente = x.Item1;
            CuidadorPaciente=new CuidadorPaciente();
            CuidadorPaciente = x.Item2;
            await GetMateriais();
        }

        public async Task GetMateriais()
        {
            try
            {
                await Task.Run(async () =>
                {
                    var pacresult =
                        new ObservableCollection<CuidadorPaciente>(
                                await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync(true))
                            .Where(e => e.PacId == oPaciente.Id)
                            .AsEnumerable();
                    var result =
                        new ObservableCollection<Material>(
                                await CuidadorRestService.DefaultManager.RefreshMaterialAsync(true))
                            .Where(e => pacresult.Select(m => m.Id)
                                .Contains(e.MatPacId))
                            .AsEnumerable();
                    Materiais = new ObservableCollection<Material>(result);
                    if (Materiais.Count == 0)
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
            oHorario.Visualizar = false;
            var newMaterial = returndData as Material;
            if (!Materiais.Contains(newMaterial))
                Materiais.Add(newMaterial);
            else
                await GetMateriais();
        }
    }
}