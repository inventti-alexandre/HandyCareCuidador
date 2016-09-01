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
    public class ListaMedicamentoPageModel : FreshBasePageModel
    {
        private Medicamento _selectedMedicamento;
        //private readonly IMedicamentoRestService _restService;
        //private readonly ICuidadorPacienteRestService _cuidadorPacienteRestService;
        public bool deleteVisible;

        public Paciente oPaciente { get; set; }
        public HorarioViewModel oHorario { get; set; }
        public ObservableCollection<Medicamento> Medicamentos { get; set; }

        public Command AddMedicamento
        {
            get
            {
                return new Command(async () =>
                {
                    deleteVisible = false;
                    await CoreMethods.PushPageModel<MedicamentoPageModel>();
                });
            }
        }

        public Medicamento SelectedMedicamento
        {
            get { return _selectedMedicamento; }
            set
            {
                _selectedMedicamento = value;
                if (value != null)
                {
                    MedicamentoSelected.Execute(value);
                    SelectedMedicamento = null;
                }
            }
        }

        public Command<Medicamento> MedicamentoSelected
        {
            get
            {
                return new Command<Medicamento>(async medicamento =>
                {
                    deleteVisible = true;
                    RaisePropertyChanged("IsVisible");
                    await CoreMethods.PushPageModel<MedicamentoPageModel>(medicamento);
                    medicamento = null;
                });
            }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            oPaciente = initData as Paciente;
            oHorario = new HorarioViewModel {ActivityRunning = true, Visualizar = false};
            await GetMedicamentos();
        }

        public async Task GetMedicamentos()
        {
            try
            {
                await Task.Run(async () =>
                {
                    var pacresult =
                        new ObservableCollection<CuidadorPaciente>(
                            await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync())
                            .Where(e => e.PacId == oPaciente.Id)
                            .AsEnumerable();
                    var x = pacresult.Count();
                    var result =
                        new ObservableCollection<Medicamento>(
                            await CuidadorRestService.DefaultManager.RefreshMedicamentoAsync())
                            .Where(e => pacresult.Select(m => m.Id)
                                .Contains(e.MedPacId))
                            .AsEnumerable();
                    Medicamentos = new ObservableCollection<Medicamento>(result);
                });
                oHorario.ActivityRunning = false;
                oHorario.Visualizar = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override void ReverseInit(object returndData)
        {
            base.ReverseInit(returndData);
            var newMedicamento = returndData as Medicamento;
            if (!Medicamentos.Contains(newMedicamento))
            {
                Medicamentos.Add(newMedicamento);
            }
        }
    }
}