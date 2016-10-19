using System;
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

        public Material Material { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        public PageModelHelper oHorario { get; set; }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    oHorario.Visualizar = false;
                    oHorario.ActivityRunning = true;
                    Material.MatQuantidade = Convert.ToSingle(oHorario.Quantidade);
                    Material.MatPacId = CuidadorPaciente.Id;
                    await CuidadorRestService.DefaultManager.SaveMaterialAsync(Material, novoItem);
                    await CoreMethods.PopPageModel(Material);
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
                    await CuidadorRestService.DefaultManager.DeleteMaterialAsync(Material);
                    await CoreMethods.PopPageModel(Material);
                });
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            var x = initData as Tuple<Material, CuidadorPaciente>;
            Material = new Material();
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
                oHorario.deleteVisible = true;
                novoItem = false;
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
        }
    }
}