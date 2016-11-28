using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using PropertyChanged;
using Syncfusion.SfCalendar.XForms;
using Syncfusion.SfSchedule.XForms;
using Xamarin.Forms;
using MonthViewSettings = Syncfusion.SfSchedule.XForms.MonthViewSettings;

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
        public DayViewSettings ConfigDias { get; set; }
        private IEnumerable<Afazer> result;
        public MonthViewSettings ConfigMeses { get; set; }
        public DayLabelSettings ConfigExibDias { get; set; }
        public DateTime InicioData { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        //public Paciente oPaciente { get; set; }
        public ObservableCollection<Afazer> Afazeres { get; set; }
        public CalendarEventCollection DataRealizacaoAfazeres { get; set; }
        public ScheduleAppointmentCollection DataAfazeres { get; set; }
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
                    var x = new Tuple<Afazer, Paciente, CuidadorPaciente, DateTime>(null, oPaciente, CuidadorPaciente,
                        InicioData);
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
                    var x = new Tuple<Afazer, Paciente, CuidadorPaciente>(afazer, oPaciente, CuidadorPaciente);
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
                    var a = afazer.AfaHorarioPrevisto;
                    await CoreMethods.PushPageModel<AfazerPageModel>(afazer);
                    afazer = null;
                });
            }
        }

        public Command<CalendarEventCollection> AfazeresCalendario
        {
            get
            {
                return new Command<CalendarEventCollection>(afazer =>
                {
                    Afazeres.Clear();
                    oHorario.DadoPaciente = false;
                    if (afazer.Count == 0)
                        InicioData = DateTime.Now;
                    foreach (var item in afazer)
                    {
                        InicioData = item.StartTime;
                        foreach (var item2 in App.Afazeres)
                        {
                            if ((item.StartTime == item2.AfaHorarioPrevisto) && (item.Subject == item2.AfaObservacao))
                            {
                                Afazeres.Add(new Afazer
                                {
                                    AfaHorarioPrevisto = item.StartTime,
                                    AfaObservacao = item.Subject,
                                    AfaPaciente = item2.AfaPaciente,
                                    Id = item2.Id,
                                    AfaHorarioPrevistoTermino = item.EndTime,
                                    AfaCor = item2.AfaCor
                                });
                            }

                        }
                        //Debug.WriteLine(item.StyleId);
                    }
                    oHorario.DadoPaciente = true;
                    var a = Afazeres.Count;
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
            InicioData = new DateTime();
            Afazeres = new ObservableCollection<Afazer>();
            oHorario = new PageModelHelper
            {
                ActivityRunning = true,
                Visualizar = false,
                DadoPaciente = true,
                CuidadorExibicao = false
            };
            DataAfazeres = new ScheduleAppointmentCollection();
            DataRealizacaoAfazeres = new CalendarEventCollection();
            ConfigExibDias = new DayLabelSettings();
            ConfigDias = new DayViewSettings {ShowAllDay = true,};
            await GetAfazeresConcluidos();
            await GetAfazeres();
            oHorario.ActivityRunning = false;
            oHorario.CuidadorExibicao = true;

        }

        public async Task GetAfazeresConcluidos()
        {
            try
            {
                await Task.Run(async () =>
                {
                    try
                    {
                        AfazeresConcluidos =
    new ObservableCollection<ConclusaoAfazer>(
        await CuidadorRestService.DefaultManager.RefreshConclusaoAfazerAsync(true));

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
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
                        result = selection.Where(e => !AfazeresConcluidos.Select(m => m.ConAfazer)
                                .Contains(e.Id))
                            .Where(e => pacresult.Select(m => m.Id).Contains(e.AfaPaciente))
                            .AsEnumerable();
                        foreach (var afazer in result)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DataRealizacaoAfazeres.Add(new CalendarInlineEvent
                                {
                                    StartTime = afazer.AfaHorarioPrevisto,
                                    EndTime = afazer.AfaHorarioPrevistoTermino,
                                    Subject = afazer.AfaObservacao,
                                    Color = Color.FromHex(afazer.AfaCor),
                                    StyleId = afazer.Id
                                });
                            });
                        }
                        App.Afazeres = new ObservableCollection<Afazer>(result);
                    }
                    else
                    {
                        App.Afazeres = new ObservableCollection<Afazer>(selection);
                        foreach (var afazer in selection)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DataRealizacaoAfazeres.Add(new CalendarInlineEvent
                                {
                                    StartTime = afazer.AfaHorarioPrevisto,
                                    EndTime = DateTime.Now,
                                    Subject = afazer.AfaObservacao,
                                    Color = Color.Aqua,
                                    AutomationId = afazer.Id,
                                    ClassId = afazer.AfaPaciente

                                });
                            });
                        }

                    }
                    oHorario.ActivityRunning = false;
                    oHorario.CuidadorExibicao = true;
                    if (selection.Count == 0)
                        oHorario.Visualizar = true;
                });
            }
            catch (ArgumentNullException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
        }

        public override void ReverseInit(object returndData)
        {
            base.ReverseInit(returndData);
            //await GetAfazeresConcluidos();
            //await GetAfazeres();

            //var newAfazer = returndData as Afazer;
            //if (!Afazeres.Contains(newAfazer))
            //{
            //    Afazeres.Add(newAfazer);
            //}
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