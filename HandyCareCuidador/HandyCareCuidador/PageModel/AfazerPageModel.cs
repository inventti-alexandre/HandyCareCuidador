using FreshMvvm;
using GalaSoft.MvvmLight;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class AfazerPageModel : FreshBasePageModel
    {
        readonly IAfazerRestService _restService;
        readonly IMaterialRestService _materialRestService;
        readonly IConclusaoAfazerRestService _concluirRestService;
        readonly IMaterialUtilizadoRestService _materialUtilizadoRestService;
        readonly IMedicamentoRestService _medicamentoRestService;
        private readonly IMedicamentoAdministradoRestService _medicamentoAdministradoRestService;
        public Afazer Afazer { get; set; }
        public bool NewItem { get; set; }
        public Material oMaterial { get; set; }
        public Medicamento oMedicamento { get; set; }
        public HorarioViewModel oHorario { get; set; }
        public MaterialUtilizado oMaterialUtilizado { get; set; }
        public MedicamentoAdministrado oMedicamentoAdministrado { get; set; }
        public ConclusaoAfazer AfazerConcluido{get;set;}
        public ObservableCollection<Material> Materiais { get; set; }
        public ObservableCollection<Material> MateriaisEscolhidos { get; set; }
        public ObservableCollection<Medicamento> Medicamentos { get; set; }
        public ObservableCollection<MaterialUtilizado> MateriaisUtilizados {get;set;}


        public AfazerPageModel()
        {
            _restService = FreshIOC.Container.Resolve<IAfazerRestService>();
            _concluirRestService = FreshIOC.Container.Resolve<IConclusaoAfazerRestService>();
            _materialRestService = FreshIOC.Container.Resolve<IMaterialRestService>();
            _medicamentoRestService = FreshIOC.Container.Resolve<IMedicamentoRestService>();
            _materialUtilizadoRestService = FreshIOC.Container.Resolve<IMaterialUtilizadoRestService>();
            _medicamentoAdministradoRestService = FreshIOC.Container.Resolve<IMedicamentoAdministradoRestService>();
        }
        public override async void Init(object initData)
        {
            base.Init(initData);
            Afazer = initData as Afazer;
            Materiais = new ObservableCollection<Material>(await _materialRestService.RefreshDataExistenteAsync());
            Medicamentos= new ObservableCollection<Medicamento>(await _medicamentoRestService.RefreshDataAsync());
            MateriaisUtilizados = new ObservableCollection<MaterialUtilizado>(await _materialUtilizadoRestService.RefreshDataAsync(Afazer?.Id));
            if (Afazer == null)
            {
                Afazer = new Afazer();
                NewItem = true;
            }
            else
            {
                NewItem = false;
                AfazerConcluido = new ConclusaoAfazer();
            }
        }
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            oHorario = new HorarioViewModel
            {
                HabilitarMaterial = false,
                HabilitarMedicamento = false
            };
            if (Afazer == null)
            {
                Afazer = new Afazer();
                oHorario.deleteVisible = false;
                oHorario.Data = DateTime.Now;
                oHorario.Horario = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            }
            else
            {
                oHorario.Data = Afazer.AfaHorarioPrevisto;
                oHorario.Horario = Afazer.AfaHorarioPrevisto.TimeOfDay;
                oHorario.deleteVisible = true;
            }
        }
        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if(NewItem)
                    {
                        Afazer.Id = Guid.NewGuid().ToString();
                    }
                    Afazer.AfaHorarioPrevisto = oHorario.Data.Date + oHorario.Horario;
                    await _restService.SaveAfazerAsync(Afazer, true);
                    if (oMaterial != null)
                    {
                        oMaterialUtilizado = new MaterialUtilizado
                        {
                            MatAfazer = Afazer.Id,
                            MatUtilizado = oMaterial.Id,
                            MatQuantidadeUtilizada = Convert.ToInt32(oHorario.Quantidade),
                            Id = Guid.NewGuid().ToString()
                        };
                        oMaterial.MatQuantidade -= oMaterialUtilizado.MatQuantidadeUtilizada;
                        oMaterial.MaterialUtilizado.Add(oMaterialUtilizado);
                        await _materialRestService.SaveMaterialAsync(oMaterial, false);
                        await _materialUtilizadoRestService.SaveMaterialUtilizadoAsync(oMaterialUtilizado, true);
                    }
                    if (oMedicamento != null)
                    {
                        oMedicamentoAdministrado = new MedicamentoAdministrado
                        {
                            MedAfazer = Afazer.Id,
                            MedAdministrado = oMedicamento.Id,
                            MemQuantidadeAdministrada = Convert.ToInt32(oHorario.Quantidade),
                            Id=Guid.NewGuid().ToString()
                        };
                        oMedicamento.MedQuantidade -= oMedicamentoAdministrado.MemQuantidadeAdministrada;
                        await _medicamentoRestService.SaveMedicamentoAsync(oMedicamento, false);
                        await _medicamentoAdministradoRestService.SaveMedicamentoAdministradoAsync(oMedicamentoAdministrado, true);
                    }
                    await CoreMethods.PopPageModel(Afazer);
                });
            }
        }
        public Command ConcluirCommand
        {
            get
            {
                return new Command(async () =>
                {
                    AfazerConcluido.ConConcluido = false;
                    AfazerConcluido.ConHorarioConcluido = DateTime.Now;
                    AfazerConcluido.ConAfazer = Afazer.Id;
                    await _concluirRestService.SaveConclusaoAfazerAsync(AfazerConcluido, true);
                    await CoreMethods.PopPageModel(Afazer);
                });
            }
        }

        public Command DeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await _restService.DeleteAfazerAsync(Afazer);
                    await CoreMethods.PopPageModel(Afazer);
                });
            }
        }
    }
}
