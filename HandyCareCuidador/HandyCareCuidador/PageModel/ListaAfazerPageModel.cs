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
    public class ListaAfazerPageModel : FreshBasePageModel
    {
        //private readonly IConclusaoAfazerRestService _conclusaoRestService;

        private bool _finalizarAfazer;
        //private readonly IAfazerRestService _restService;
        //private readonly ICuidadorPacienteRestService _cuidadorPacienteRestService;

        private Afazer _selectedAfazer;
        private Paciente _selectedPaciente;
        //private Afazer _selectedAfazerConcluido;
        public bool AfazerConcluido;
        public Task check;
        public bool deleteVisible;

        public PageModelHelper oHorario { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        //public Paciente oPaciente { get; set; }
        public ObservableCollection<Afazer> Afazeres { get; set; }
        public ObservableCollection<Afazer> ConcluidosAfazeres { get; set; }

        public ObservableCollection<ConclusaoAfazer> AfazeresConcluidos { get; set; }
        public Afazer AfazerSelecionado { get; set; }

        public Command AddAfazer
        {
            get
            {
                return new Command(async () =>
                {
                    deleteVisible = false;
                    var x = new Tuple<Afazer, Paciente, CuidadorPaciente>(null, oPaciente, CuidadorPaciente);
                    await CoreMethods.PushPageModel<AfazerPageModel>(x);
                });
            }
        }

        public Afazer SelectedAfazer
        {
            get { return _selectedAfazer; }
            set
            {
                _selectedAfazer = value;
                if (value != null)
                {
                    AfazerSelected.Execute(value);
                    SelectedAfazer = null;
                }
            }
        }

        public Command<Afazer> AfazerSelected
        {
            get
            {
                return new Command<Afazer>(async afazer =>
                {
                    deleteVisible = true;
                    var a = AfazerSelecionado;
                    RaisePropertyChanged("IsVisible");
                    var x = new Tuple<Afazer, Paciente>(afazer, oPaciente);
                    await CoreMethods.PushPageModel<AfazerPageModel>(x);
                    afazer = null;
                });
            }
        }

        public bool FinalizarAfazer
        {
            get { return _finalizarAfazer; }
            set
            {
                _finalizarAfazer = value;
                var a = AfazerSelecionado;
                AfazerConcluido = true;
                AfazerFinalizado.Execute(value);
            }
        }

        public Command<Afazer> AfazerFinalizado
        {
            get
            {
                return new Command<Afazer>(async afazer =>
                {
                    var a = afazer;
                    await CoreMethods.PushPageModel<AfazerPageModel>(afazer);
                    afazer = null;
                });
            }
        }

        public Paciente oPaciente
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

        public Command VisualizarConcluidos
        {
            get
            {
                return
                    new Command(
                        async () => { await CoreMethods.PushPageModel<ListaAfazerConcluidoPageModel>(oPaciente); });
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            oPaciente = new Paciente();
            CuidadorPaciente = new CuidadorPaciente();
            var x = initData as Tuple<Paciente, CuidadorPaciente>;
            if (x != null)
            {
                oPaciente = x.Item1;
                CuidadorPaciente = x.Item2;
            }
        }

        protected override async void ViewIsAppearing(object sender, EventArgs e)
        {
            AfazerSelecionado = new Afazer();
            oHorario = new PageModelHelper {ActivityRunning = true, Visualizar = false};
            await GetAfazeresConcluidos();
            await GetAfazeres();
            oHorario.ActivityRunning = false;
        }

        public async Task GetAfazeresConcluidos()
        {
            try
            {
                await Task.Run(async () =>
                {
                    AfazeresConcluidos =
                        new ObservableCollection<ConclusaoAfazer>(
                            await CuidadorRestService.DefaultManager.RefreshConclusaoAfazerAsync(true));
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetAfazeres()
        {
            //INSERIR PACID EM MATERIAL E MEDICAMENTO
            try
            {
                await Task.Run(async () =>
                {
                    oHorario.ActivityRunning = true;
                    oHorario.FinalizarAfazer = false;
                    var selection =
                        new ObservableCollection<Afazer>(
                            await CuidadorRestService.DefaultManager.RefreshAfazerAsync(true));
                    if ((selection.Count > 0) && (AfazeresConcluidos.Count > 0))
                    {
                        var pacresult =
                            new ObservableCollection<CuidadorPaciente>(
                                    await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync(true))
                                .Where(e => e.PacId == oPaciente.Id)
                                .AsEnumerable();
                        var result = selection.Where(e => !AfazeresConcluidos.Select(m => m.ConAfazer)
                                .Contains(e.Id))
                            .Where(e => pacresult.Select(m => m.Id).Contains(e.AfaPaciente))
                            .AsEnumerable();
                        Afazeres = new ObservableCollection<Afazer>(result);
                        App.Afazeres = new ObservableCollection<Afazer>(result);
                    }
                    else
                    {
                        Afazeres = new ObservableCollection<Afazer>(selection);
                    }
                    oHorario.ActivityRunning = false;
                    if (Afazeres.Count == 0)
                        oHorario.Visualizar = true;
                });
            }
            catch (ArgumentNullException e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
        }

        public override void ReverseInit(object returndData)
        {
            base.ReverseInit(returndData);
            var newAfazer = returndData as Afazer;
            if (!Afazeres.Contains(newAfazer))
            {
                Afazeres.Add(newAfazer);
            }
            //if (AfazerConcluido)
            //{
            //    Task.Run(async () => await GetAfazeresConcluidos());
            //    Task.Run(async () => await GetAfazeres());
            //}
        }

        public void OnItemToggled(object sender, ToggledEventArgs e)
        {
            var toggledSwitch = (ListSwitch) sender;
        }
    }
}