using System;
using System.Collections.ObjectModel;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class AfazerPageModel : FreshBasePageModel
    {
        //readonly IAfazerRestService _restService;
        //readonly IMaterialRestService _materialRestService;
        //readonly IConclusaoAfazerRestService _concluirRestService;
        //readonly IMaterialUtilizadoRestService _materialUtilizadoRestService;
        //readonly IMedicamentoRestService _medicamentoRestService;
        //private readonly IMedicamentoAdministradoRestService _medicamentoAdministradoRestService;
        public Afazer Afazer { get; set; }
        public bool NewItem { get; set; }
        public Material oMaterial { get; set; }
        public Medicamento oMedicamento { get; set; }
        public HorarioViewModel oHorario { get; set; }
        public MaterialUtilizado oMaterialUtilizado { get; set; }
        public MedicamentoAdministrado oMedicamentoAdministrado { get; set; }
        public ConclusaoAfazer AfazerConcluido { get; set; }
        public ObservableCollection<Material> Materiais { get; set; }
        public ObservableCollection<Material> MateriaisEscolhidos { get; set; }
        public ObservableCollection<Medicamento> Medicamentos { get; set; }
        public ObservableCollection<MaterialUtilizado> MateriaisUtilizados { get; set; }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (NewItem)
                    {
                        Afazer.Id = Guid.NewGuid().ToString();
                    }
                    Afazer.AfaHorarioPrevisto = oHorario.Data.Date + oHorario.Horario;
                    await CuidadorRestService.DefaultManager.SaveAfazerAsync(Afazer, true);
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
                        await CuidadorRestService.DefaultManager.SaveMaterialAsync(oMaterial, false);
                        await CuidadorRestService.DefaultManager.SaveMaterialUtilizadoAsync(oMaterialUtilizado, true);
                    }
                    if (oMedicamento != null)
                    {
                        oMedicamentoAdministrado = new MedicamentoAdministrado
                        {
                            MedAfazer = Afazer.Id,
                            MedAdministrado = oMedicamento.Id,
                            MemQuantidadeAdministrada = Convert.ToInt32(oHorario.Quantidade),
                            Id = Guid.NewGuid().ToString()
                        };
                        oMedicamento.MedQuantidade -= oMedicamentoAdministrado.MemQuantidadeAdministrada;
                        await CuidadorRestService.DefaultManager.SaveMedicamentoAsync(oMedicamento, false);
                        await
                            CuidadorRestService.DefaultManager.SaveMedicamentoAdministradoAsync(
                                oMedicamentoAdministrado, true);
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
                    await CuidadorRestService.DefaultManager.SaveConclusaoAfazerAsync(AfazerConcluido, true);
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
                    await CuidadorRestService.DefaultManager.DeleteAfazerAsync(Afazer);
                    await CoreMethods.PopPageModel(Afazer);
                });
            }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            Afazer = initData as Afazer;
            Materiais =
                new ObservableCollection<Material>(await CuidadorRestService.DefaultManager.RefreshMaterialAsync());
            Medicamentos =
                new ObservableCollection<Medicamento>(await CuidadorRestService.DefaultManager.RefreshMedicamentoAsync());
            MateriaisUtilizados =
                new ObservableCollection<MaterialUtilizado>(
                    await CuidadorRestService.DefaultManager.RefreshMaterialUtilizadoAsync(Afazer?.Id));
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
    }
}