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
        public Paciente Paciente { get; set; }
        public HorarioViewModel oHorario { get; set; }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    oHorario.Visualizar = false;
                    oHorario.ActivityRunning = true;
                    Material.MatQuantidade = Convert.ToSingle(oHorario.Quantidade);
                    Material.MatPacId = Paciente.Id;
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
            var x = initData as Tuple<Material, Paciente>;
            Material=new Material();
            Paciente=new Paciente();
            if (x != null)
            {
                Material = x.Item1;
                Paciente = x.Item2;
            }
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
    }
}