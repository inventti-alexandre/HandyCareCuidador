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
    public class AcionarContatoEmergencia : FreshBasePageModel
    {
        private Familiar _selectedFamiliar;
        public Cuidador Cuidador { get; set; }
        public Paciente Paciente { get; set; }
        public PageModelHelper oHorario { get; set; }
        public ObservableCollection<Parentesco> Parentescos { get; set; }
        public ObservableCollection<Familiar> Familiares { get; set; }

        public Command LigarPM
        {
            get
            {
                return new Command(() =>
                {
#if DEBUG
                    Device.OpenUri(new Uri("tel:0"));
#else
                    Device.OpenUri(new Uri("tel:190"));
#endif
                });
            }
        }

        public Command LigarSAMU
        {
            get
            {
                return new Command(() =>
                {
#if DEBUG
                    Device.OpenUri(new Uri("tel:0"));
#else
                    Device.OpenUri(new Uri("tel:192"));
#endif
                });
            }
        }

        public Command LigarCBM
        {
            get
            {
                return new Command(() =>
                {
#if DEBUG
                    Device.OpenUri(new Uri("tel:0"));
#else
                    Device.OpenUri(new Uri("tel:193"));
#endif
                });
            }
        }

        public Familiar SelectedFamiliar
        {
            get { return _selectedFamiliar; }
            set
            {
                _selectedFamiliar = value;
                if (value != null)
                {
                    FamiliarSelected.Execute(value);
                    SelectedFamiliar = null;
                }
            }
        }

        public Command<Familiar> FamiliarSelected
        {
            get
            {
                return new Command<Familiar>(async familiar =>
                {
                    var tempfamiliar = familiar;
                    familiar = null;
                    var contatoemergencia =
                        new ObservableCollection<ContatoEmergencia>(
                                await CuidadorRestService.DefaultManager.RefreshContatoEmergenciaAsync(true))
                            .FirstOrDefault(e => e.Id == tempfamiliar.FamContatoEmergencia);
                    var contelefone =
                        new ObservableCollection<ConTelefone>(
                                await CuidadorRestService.DefaultManager.RefreshConTelefoneAsync(true))
                            .FirstOrDefault(e => e.Id == contatoemergencia.ConTelefone);
                    var concelular =
                        new ObservableCollection<ConCelular>(
                                await CuidadorRestService.DefaultManager.RefreshConCelularAsync(true))
                            .FirstOrDefault(e => e.Id == contatoemergencia.ConCelular);
                    if ((contelefone != null) && (concelular != null))
                    {
                        var result =
                            await
                                CoreMethods.DisplayActionSheet("Forma de ligação", "Cancelar", null, "Celular",
                                    "Telefone");
                        if (result == "Celular")
                            Device.OpenUri(new Uri("tel:" + concelular.ConNumCelular));
                        else if (result == "Telefone")
                            Device.OpenUri(new Uri("tel:" + contelefone.ConNumTelefone));
                    }
                });
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            oHorario = new PageModelHelper {ActivityRunning = true};
            Cuidador = new Cuidador();
            Paciente = new Paciente();
            var tupla = initData as Tuple<Cuidador, Paciente>;
            if (tupla != null)
            {
                Cuidador = tupla.Item1;
                Paciente = tupla.Item2;
            }
            GetFamiliares();
        }

        private void GetFamiliares()
        {
            Task.Run(async () =>
            {
                var oi = new ObservableCollection<PacienteFamiliar>(
                        await CuidadorRestService.DefaultManager.RefreshPacienteFamiliarAsync(true))
                    .Where(e => e.PacId == Paciente.Id);
                var selection =
                    new ObservableCollection<Familiar>(
                            await CuidadorRestService.DefaultManager.RefreshFamiliarAsync(true))
                        .Where(e => oi.Select(a => a.FamId)
                            .Contains(e.Id)).AsEnumerable();
                var x =
                    new ObservableCollection<Parentesco>(
                            await CuidadorRestService.DefaultManager.RefreshParentescoAsync(true))
                        .Where(e => selection.Select(a => a.FamParentesco)
                            .Contains(e.Id)).AsEnumerable();
                foreach (var z in selection)
                    foreach (var b in x)
                        if (z.FamParentesco == b.Id)
                            z.FamDescriParentesco = b.ParDescricao;

                Parentescos = new ObservableCollection<Parentesco>(x);
                Familiares = new ObservableCollection<Familiar>(selection);
                oHorario.ActivityRunning = false;
            });
        }
    }
}