using FreshMvvm;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCareCuidador.Data;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class MaterialPageModel:FreshBasePageModel
    {
        //IMaterialRestService _restService;
        public bool deleteVisible = true;
        public bool novoItem = true;
        public Material Material { get; set; }
        public HorarioViewModel oHorario { get; set; }
        public MaterialPageModel(IMaterialRestService restService)
        {
            //_restService = FreshIOC.Container.Resolve<IMaterialRestService>();
        }
        public override void Init(object initData)
        {
            base.Init(initData);
            Material = initData as Material;
            oHorario = new HorarioViewModel {Quantidade = null};
            if (Material == null)
            {
                Material = new Material();
                deleteVisible = false;
                novoItem = true;
            }
            else
            {
                novoItem = false;
            }
        }
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            //oHorario = new HorarioViewModel();
            if (Material == null)
            {
                Material = new Material();
                oHorario.Quantidade = null;
            }
            else
            {
                oHorario.Quantidade = Material.MatQuantidade;
            }
        }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    Material.MatQuantidade = Convert.ToInt32(oHorario.Quantidade);
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
                    await CuidadorRestService.DefaultManager.DeleteMaterialAsync(Material);
                    await CoreMethods.PopPageModel(Material);
                });
            }
        }
    }
}
