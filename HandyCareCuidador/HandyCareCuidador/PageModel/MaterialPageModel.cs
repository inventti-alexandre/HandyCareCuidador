using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class MaterialPageModel : FreshBasePageModel
    {
        //IMaterialRestService _restService;
        public bool deleteVisible = true;
        public bool novoItem = true;
        public ObservableCollection<string> Unidades { get; set; }
        public string SelectedUnidade { get; set; }
        public Material Material { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        public PageModelHelper oHorario { get; set; }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        Material.MatUnidade = SelectedUnidade;
                        oHorario.Visualizar = false;
                        oHorario.ActivityRunning = true;
                        Material.MatQuantidade = Convert.ToSingle(oHorario.Quantidade);
                        Material.MatPacId = CuidadorPaciente.Id;
                        await CuidadorRestService.DefaultManager.SaveMaterialAsync(Material, novoItem);

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                    finally
                    {
                        await CoreMethods.PopPageModel(Material);
                        UserDialogs.Instance.ShowSuccess("Material cadastrado com sucesso", 4000);

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
                    try
                    {
                        oHorario.Visualizar = false;
                        oHorario.ActivityRunning = true;
                        await CuidadorRestService.DefaultManager.DeleteMaterialAsync(Material);

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                    finally
                    {
                        await CoreMethods.PopPageModel(Material);
                        UserDialogs.Instance.ShowSuccess("Material excluido com sucesso", 4000);

                    }
                });
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            var x = initData as Tuple<Material, CuidadorPaciente>;
            Material = new Material();
            Unidades = new ObservableCollection<string> { "Cx", "ml", "l", "kg", "g", "mg", "un" };
            CuidadorPaciente = new CuidadorPaciente();
            if (x != null)
            {
                Material = x.Item1;
                CuidadorPaciente = x.Item2;
            }
            oHorario = new PageModelHelper {Quantidade = null};
            if (Material.Id == null)
            {
                Material = new Material();
                oHorario.deleteVisible = false;
                novoItem = true;
            }
            else
            {
                SelectedUnidade = Material.MatUnidade;
                oHorario.deleteVisible = true;
                oHorario.Quantidade = Material.MatQuantidade;
                novoItem = false;
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
        }
    }
}