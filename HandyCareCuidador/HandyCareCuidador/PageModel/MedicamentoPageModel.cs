using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class MedicamentoPageModel : FreshBasePageModel
    {
        public bool alterar;
        //IMedicamentoRestService _restService;
        public bool deleteVisible = true;
        private ViaAdministracaoMedicamento _oVia;
        private FormaApresentacaoMedicamento _oForma;
        public Medicamento Medicamento { get; set; }
        public ObservableCollection<string> Unidades { get; set; }
        public string SelectedUnidade { get; set; }

        public ObservableCollection<ViaAdministracaoMedicamento> Vias { get; set; }
        //public ViaAdministracaoMedicamento ViaAdministracaoMedicamento { get; set; }
        //public FormaApresentacaoMedicamento FormaApresentacaoMedicamento { get; set; }
        public ObservableCollection<FormaApresentacaoMedicamento> Formas { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        public Image MedImage { get; set; }
        public PageModelHelper oHorario { get; set; }

        public ViaAdministracaoMedicamento oViaAdministracaoMedicamento
        {
            get { return _oVia; }
            set
            {
                _oVia = value;
                if (value != null)
                {
                    //ShowMedicamentos.Execute(value);
                    //SelectedPaciente = null;
                }

            }
        }

        public FormaApresentacaoMedicamento oFormaApresentacaoMedicamento
        {
            get { return _oForma; }
            set
            {
                _oForma = value;
                if (value != null)
                {
                    //ShowMedicamentos.Execute(value);
                    //SelectedPaciente = null;
                }

            }
        }


        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    Medicamento.MedUnidade = SelectedUnidade;
                    oHorario.Visualizar = false;
                    oHorario.ActivityRunning = true;
                    Medicamento.MedQuantidade = Convert.ToSingle(oHorario.Quantidade);
                    Medicamento.MedPacId = CuidadorPaciente.Id;
                    await CuidadorRestService.DefaultManager.SaveMedicamentoAsync(Medicamento, alterar);
                    await CoreMethods.PopPageModel(Medicamento);
                    UserDialogs.Instance.ShowSuccess("Medicamento cadastrado com sucesso", 4000);

                });
            }
        }

        public Command DeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    oHorario.Visualizar = false;
                    oHorario.ActivityRunning = true;
                    await CuidadorRestService.DefaultManager.DeleteMedicamentoAsync(Medicamento);
                    await CoreMethods.PopPageModel(Medicamento);
                });
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            oHorario = new PageModelHelper
            {
                QuantidadeF = 0
            };
            Unidades = new ObservableCollection<string> { "Cx", "ml", "l", "kg", "g", "mg", "un" };

            oViaAdministracaoMedicamento = new ViaAdministracaoMedicamento();
            oFormaApresentacaoMedicamento=new FormaApresentacaoMedicamento();
            var x = initData as Tuple<Medicamento, CuidadorPaciente>;
            Medicamento = new Medicamento();
            CuidadorPaciente = new CuidadorPaciente();
            GetInfoMateriais();
            //FormaApresentacaoMedicamento = new FormaApresentacaoMedicamento();
            //ViaAdministracaoMedicamento = new ViaAdministracaoMedicamento();
            if (x != null)
            {
                Medicamento = x.Item1;
                CuidadorPaciente = x.Item2;
                oHorario.QuantidadeF = Medicamento.MedQuantidade;
            }
            if (Medicamento.Id == null)
            {
                Medicamento = new Medicamento();
                oHorario.deleteVisible = false;
                alterar = false;
            }
            else
            {
                oHorario.deleteVisible = true;
                alterar = false;
            }
        }

        private void GetInfoMateriais()
        {
            Task.Run(async () =>
            {
                Vias =
                    new ObservableCollection<ViaAdministracaoMedicamento>(
                        await CuidadorRestService.DefaultManager.RefreshViaAdministracaoMedicamentoAsync());
                Formas =
                    new ObservableCollection<FormaApresentacaoMedicamento>(
                        await CuidadorRestService.DefaultManager.RefreshFormaApresentacaoMedicamentoAsync());
                if (Medicamento.Id != null)
                {
                    oFormaApresentacaoMedicamento = Formas.FirstOrDefault(e=>e.Id==Medicamento.MedApresentacao);
                    oViaAdministracaoMedicamento = Vias.FirstOrDefault(e=>e.Id==Medicamento.MedViaAdministracao);
                    SelectedUnidade = Medicamento.MedUnidade;
                }
            });
            oHorario.ActivityRunning = false;
            oHorario.Visualizar = true;
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
        }
    }
}