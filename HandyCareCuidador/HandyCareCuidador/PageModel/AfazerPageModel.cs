using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using PropertyChanged;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class AfazerPageModel : FreshBasePageModel
    {
        public Afazer Afazer { get; set; }
        public bool NewItem { get; set; }
        public Material oMaterial { get; set; }
        public Medicamento oMedicamento { get; set; }
        public Paciente Paciente { get; set; }
        public PageModelHelper oHorario { get; set; }
        public MaterialUtilizado oMaterialUtilizado { get; set; }
        public Dictionary<string, Color> colorDict;
        public ObservableCollection<ColorList> ListaCores { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        public MedicamentoAdministrado oMedicamentoAdministrado { get; set; }
        public ConclusaoAfazer AfazerConcluido { get; set; }
        public MotivoNaoConclusaoTarefa MotivoNaoConclusaoTarefa { get; set; }
        public ObservableCollection<Material> Materiais { get; set; }
        public ObservableCollection<Material> MateriaisEscolhidos { get; set; }
        public ObservableCollection<Medicamento> Medicamentos { get; set; }
        public ObservableCollection<MaterialUtilizado> MateriaisUtilizados { get; set; }
        public ColorList SelectedColor { get; set; }
        public Color Color { get; set; }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (NewItem)
                        Afazer.Id = Guid.NewGuid().ToString();
                    oHorario.Visualizar = false;
                    var a = Color;
                    oHorario.ActivityRunning = true;
                    Afazer.AfaPaciente = CuidadorPaciente.Id;
                    Afazer.AfaHorarioPrevisto = oHorario.Data.Date + oHorario.Horario;
                    Afazer.AfaHorarioPrevistoTermino= oHorario.DataTermino.Date + oHorario.HorarioTermino;
                    var rgbString = $"#{(int) (SelectedColor.Color.R*255):X2}{(int) (SelectedColor.Color.G*255):X2}{(int) (SelectedColor.Color.B*255):X2}";
                    Afazer.AfaCor = rgbString;
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
                    UserDialogs.Instance.ShowSuccess("Afazer registrado com sucesso", 4000);
                });
            }
        }

        public Command ConcluirCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var result = await CoreMethods.DisplayActionSheet("O afazer foi finalizado corretamente?", 
                        "Cancelar", null, "Sim","Não");
                    switch (result)
                    {
                        case "Sim":
                        {
                                oHorario.Visualizar = false;
                                oHorario.ActivityRunning = true;
                                AfazerConcluido.Id = Guid.NewGuid().ToString();
                                AfazerConcluido.ConConcluido = false;
                                AfazerConcluido.ConHorarioConcluido = DateTime.Now;
                                AfazerConcluido.ConAfazer = Afazer.Id;
                                await CuidadorRestService.DefaultManager.SaveConclusaoAfazerAsync(AfazerConcluido, true);
                                await CoreMethods.PopPageModel(Afazer);
                                UserDialogs.Instance.ShowSuccess("Afazer concluído com sucesso", 4000);

                            }
                            break;
                        case "Não":
                        {
                                var resulto = await UserDialogs.Instance.PromptAsync(new PromptConfig()

                                                           .SetTitle("Informe o motivo da não realização da tarefa")
                                                           .SetPlaceholder("Descreva o que aconteceu")
                                                           .SetMaxLength(255));
                                MotivoNaoConclusaoTarefa.Id= Guid.NewGuid().ToString();
                                oHorario.Visualizar = false;
                                oHorario.ActivityRunning = true;
                                AfazerConcluido.Id = Guid.NewGuid().ToString();
                                AfazerConcluido.ConConcluido = false;
                                AfazerConcluido.ConHorarioConcluido = DateTime.Now;
                                AfazerConcluido.ConAfazer = Afazer.Id;
                                MotivoNaoConclusaoTarefa.MoAfazer = AfazerConcluido.Id;
                                MotivoNaoConclusaoTarefa.MoExplicacao=resulto.Text;
                                await CuidadorRestService.DefaultManager.SaveConclusaoAfazerAsync(AfazerConcluido, true);
                                await CuidadorRestService.DefaultManager.SaveMotivoNaoConclusaoTarefa(MotivoNaoConclusaoTarefa, true);

                                await CoreMethods.PopPageModel(Afazer);
                                UserDialogs.Instance.ShowSuccess("Afazer concluído com sucesso", 4000);

                            }
                            break;
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
                    oHorario.Visualizar = false;
                    oHorario.ActivityRunning = true;
                    await CuidadorRestService.DefaultManager.DeleteAfazerAsync(Afazer);
                    await CoreMethods.PopPageModel(Afazer);
                });
            }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            Color=new Color();
            SelectedColor=new ColorList();
            oHorario = new PageModelHelper
            {
                HabilitarMaterial = false,
                HabilitarMedicamento = false,
                deleteVisible = false
            };
            MotivoNaoConclusaoTarefa=new MotivoNaoConclusaoTarefa();
            var x = initData as Tuple<Afazer, Paciente, CuidadorPaciente,DateTime>;
            Afazer = new Afazer();
            Paciente = new Paciente();
            CuidadorPaciente = new CuidadorPaciente();
            if (x != null)
            {
                if (x.Item1 != null)
                {
                    Afazer = x.Item1;
                    NewItem = false;
                    oHorario.deleteVisible = true;
                }
                else
                {
                    NewItem = true;
                }
                Paciente = x.Item2;
                CuidadorPaciente = x.Item3;
                oHorario.Data = x.Item4;
                oHorario.DataTermino = x.Item4.AddDays(1);
            }
            Materiais =
                new ObservableCollection<Material>(await CuidadorRestService.DefaultManager.RefreshMaterialAsync());
            Medicamentos =
                new ObservableCollection<Medicamento>(await CuidadorRestService.DefaultManager.RefreshMedicamentoAsync());
            MateriaisUtilizados =
                new ObservableCollection<MaterialUtilizado>(
                    await CuidadorRestService.DefaultManager.RefreshMaterialUtilizadoAsync(Afazer?.Id));
            AfazerConcluido = new ConclusaoAfazer();

            ListaCores = new ObservableCollection<ColorList>
            {
                new ColorList
                {
                    Cor = "Padrão",
                    Color = Color.Default
                },
                new ColorList
                {
                                        Cor = "Laranja escuro",
                    Color =  Color.FromHex("#FF5722")

                },
                                new ColorList
                {
                    Cor = "Cinza azulado",
                    Color = Color.FromHex("#607D8B")

                },                new ColorList
                {
                                        Cor = "Ciano",
                    Color = Color.FromHex("#00BCD4")

                },                new ColorList
                {
                                        Cor = "Roxo escuro",
                    Color = Color.FromHex("#673AB7")

                },                new ColorList
                {
                                        Cor = "Cinza",
                    Color = Color.FromHex("#9E9E9E")

                },                new ColorList
                {
                                        Cor = "Azul claro",
                    Color = Color.FromHex("#02A8F3")

                },                new ColorList
                {
                                        Cor = "Vermelho",
                    Color = Color.FromHex("#D32F2F")

                },
            };
        //    colorDict = new Dictionary<string, Color>
        //{
        //    { "Default", Color.Default },                  { "Amber", Color.FromHex("#FFC107") },
        //    { "Black", Color.FromHex("#212121") },         { "Blue", Color.FromHex("#2196F3") },
        //    { "Blue Grey", Color.FromHex("#607D8B") },     { "Brown", Color.FromHex("#795548") },
        //    { "Cyan", Color.FromHex("#00BCD4") },          { "Dark Orange", Color.FromHex("#FF5722") },
        //    { "Dark Purple", Color.FromHex("#673AB7") },   { "Green", Color.FromHex("#4CAF50") },
        //    { "Grey", Color.FromHex("#9E9E9E") },          { "Indigo", Color.FromHex("#3F51B5") },
        //    { "Light Blue", Color.FromHex("#02A8F3") },    { "Light Green", Color.FromHex("#8AC249") },
        //    { "Lime", Color.FromHex("#CDDC39") },          { "Orange", Color.FromHex("#FF9800") },
        //    { "Pink", Color.FromHex("#E91E63") },          { "Purple", Color.FromHex("#94499D") },
        //    { "Red", Color.FromHex("#D32F2F") },           { "Teal", Color.FromHex("#009587") },
        //    { "White", Color.FromHex("#FFFFFF") },         { "Yellow", Color.FromHex("#FFEB3B") },
        //};
        }
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            if (Afazer?.Id == null)
            {
                //oHorario.deleteVisible = false;
                oHorario.Data = DateTime.Now;
                oHorario.Horario = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                oHorario.DataTermino = DateTime.Now;
                oHorario.HorarioTermino = new TimeSpan(DateTime.Now.Hour+1, DateTime.Now.Minute, DateTime.Now.Second);
            }
            else
            {
                oHorario.Data = Afazer.AfaHorarioPrevisto;
                oHorario.Horario = Afazer.AfaHorarioPrevisto.TimeOfDay;
                //oHorario.deleteVisible = true;
            }
        }
    }
}