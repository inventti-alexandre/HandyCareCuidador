using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class PacientePageModel : FreshBasePageModel
    {
        public Paciente Paciente { get; set; }
        public ObservableCollection<string> GrupoSanguineo { get; set; }
        public ObservableCollection<string> Fator { get; set; }
        public string SelectedGrupo { get; set; }
        public string SelectedFator { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        public MotivoCuidado MotivoCuidado { get; set; }
        public Cuidador Cuidador { get; set; }
        public TipoTratamento TipoTratamento { get; set; }
        public PeriodoTratamento PeriodoTratamento { get; set; }
        public PageModelHelper oHorario { get; set; }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (oHorario.VisualizarTermino == false)
                            PeriodoTratamento.PerTermino = null;
                        else
                            PeriodoTratamento.PerTermino = oHorario.Data;
                        oHorario.Visualizar = false;
                        oHorario.ActivityRunning = true;
                        Paciente.Id = Guid.NewGuid().ToString();
                        Paciente.PacTipoSanguineo = SelectedGrupo + SelectedFator;
                        CuidadorPaciente = new CuidadorPaciente {Id = Guid.NewGuid().ToString()};
                        MotivoCuidado.Id = Guid.NewGuid().ToString();
                        CuidadorPaciente.PacId = Paciente.Id;
                        CuidadorPaciente.CuiId = Cuidador.Id;
                        TipoTratamento.Id = Guid.NewGuid().ToString();
                        TipoTratamento.TipCuidado = MotivoCuidado.Id;
                        PeriodoTratamento.Id = Guid.NewGuid().ToString();
                        CuidadorPaciente.CuiPeriodoTratamento = PeriodoTratamento.Id;
                        Paciente.PacMotivoCuidado = MotivoCuidado.Id;
                        await Task.Run(async () =>
                        {
                            await
                                CuidadorRestService.DefaultManager.SaveMotivoCuidadoAsync(MotivoCuidado,
                                    oHorario.NovoPaciente);
                            await
                                CuidadorRestService.DefaultManager.SavePeriodoTratamentoAsync(PeriodoTratamento,
                                    oHorario.NovoPaciente);
                            await CuidadorRestService.DefaultManager.SavePacienteAsync(Paciente, oHorario.NovoPaciente);
                            await
                                CuidadorRestService.DefaultManager.SaveTipoTratamentoAsync(TipoTratamento,
                                    oHorario.NovoPaciente);
                            await
                                CuidadorRestService.DefaultManager.SaveCuidadorPacienteAsync(CuidadorPaciente,
                                    oHorario.NovoPaciente);
                        });
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                    finally
                    {
                        await CoreMethods.PopPageModel(Paciente);
                        UserDialogs.Instance.ShowSuccess("Paciente cadastrado com sucesso", 4000);
                    }
                });
            }
        }

        public Command DeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CuidadorRestService.DefaultManager.DeletePacienteAsync(Paciente);
                    await CoreMethods.PopPageModel(Paciente);
                });
            }
        }
        public Command EditarCommand
        {
            get
            {
                return new Command(() =>
                {
                    oHorario.NovoPaciente = !oHorario.NovoPaciente;
                });
            }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            GrupoSanguineo = new ObservableCollection<string> {"AB", "A", "B", "O"};
            Fator = new ObservableCollection<string> {"+", "-"};
            var x = initData as Tuple<Paciente, bool, Cuidador>;
            Cuidador = new Cuidador();
            Paciente = new Paciente();
            PeriodoTratamento = new PeriodoTratamento
            {
                PerInicio = DateTime.Now,
                PerTermino = DateTime.Now
            };
            TipoTratamento = new TipoTratamento();
            MotivoCuidado = new MotivoCuidado();
            oHorario = new PageModelHelper {ActivityRunning = true, Visualizar = false, NovoPaciente = false};
            if (x != null)
            {
                oHorario.NovoPaciente = x.Item2;
                oHorario.DadoPaciente = false;
                Cuidador = x.Item3;
            }
            if (oHorario.NovoPaciente == false)
            {
                if (x?.Item1 != null)
                {
                    Paciente = x.Item1;
                    await GetInfo();
                }
                oHorario.DadoPaciente = true;
            }
            oHorario.ActivityRunning = false;
            oHorario.Visualizar = true;
        }

        private async Task GetInfo()
        {
            try
            {
                var pacresult = new ObservableCollection<CuidadorPaciente>(
                        await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync())
                    .FirstOrDefault(e => e.PacId == Paciente.Id);
                if (pacresult.Id != null)
                {
                    PeriodoTratamento = new ObservableCollection<PeriodoTratamento>(
                        await CuidadorRestService.DefaultManager.RefreshPeriodoTratamentoAsync())
                        .FirstOrDefault(e => e.Id == pacresult.CuiPeriodoTratamento);
                }
                if (PeriodoTratamento.PerTermino != null)
                    {
                        oHorario.Data = PeriodoTratamento.PerTermino.Value;
                        oHorario.VisualizarTermino = true;
                    }
                MotivoCuidado =
                        new ObservableCollection<MotivoCuidado>(
                            await CuidadorRestService.DefaultManager.RefreshMotivoCuidadoAsync())
                            .FirstOrDefault(e => e.Id == Paciente.PacMotivoCuidado);
                    var p = MotivoCuidado;
                    TipoTratamento = new ObservableCollection<TipoTratamento>(
                            await CuidadorRestService.DefaultManager.RefreshTipoTratamentoAsync())
                        .FirstOrDefault(e => e.TipCuidado==MotivoCuidado.Id);
                    var k = TipoTratamento;
                var a = Paciente.PacTipoSanguineo.IndexOf("+", StringComparison.Ordinal);
                SelectedGrupo = Paciente.PacTipoSanguineo.Split('+')[0];
                SelectedFator = Paciente.PacTipoSanguineo.Substring(a);
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
        }
    }
}