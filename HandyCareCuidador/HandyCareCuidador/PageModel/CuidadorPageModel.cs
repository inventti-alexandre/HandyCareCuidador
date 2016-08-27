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
    public class CuidadorPageModel : FreshBasePageModel
    {
        public Cuidador Cuidador { get; set; }
        public TipoCuidador TipoCuidador { get; set; }
        public ValidacaoCuidador ValidacaoCuidador { get; set; }
        public HorarioViewModel oHorario { get; set; }
        private bool novoItem;
        public override async void Init(object initData)
        {
            base.Init(initData);
            Cuidador = new Cuidador();
            TipoCuidador = new TipoCuidador();
            ValidacaoCuidador = new ValidacaoCuidador();
            oHorario = new HorarioViewModel {ActivityRunning = true, Visualizar = false, VisualizarTermino = false};
            Cuidador = initData as Cuidador;
            await GetData();
            //await GetInfo();
            novoItem = Cuidador == null;
            oHorario.ActivityRunning = false;
            oHorario.Visualizar = true;
        }
        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    //Paciente.MatQuantidade = Convert.ToInt32(oHorario.Quantidade);
                    await CuidadorRestService.DefaultManager.SaveCuidadorAsync(Cuidador, novoItem);
                    await CoreMethods.PopPageModel(Cuidador);
                });
            }
        }


        private async Task GetData()
        {
            TipoCuidador = new ObservableCollection<TipoCuidador>(await CuidadorRestService.DefaultManager.RefreshTipoCuidadorAsync())
                .FirstOrDefault(e => e.Id == Cuidador.CuiTipoCuidador);
            ValidacaoCuidador= new ObservableCollection<ValidacaoCuidador>(await CuidadorRestService.DefaultManager.RefreshValidacaoCuidadorAsync())
                .FirstOrDefault(e => e.Id == Cuidador.CuiValidacaoCuidador);
        }
    }
}