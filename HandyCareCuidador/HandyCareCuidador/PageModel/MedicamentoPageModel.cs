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
    public class MedicamentoPageModel : FreshBasePageModel
    {
        //IMedicamentoRestService _restService;
        public bool deleteVisible = true;
        public bool alterar = false;
        public Medicamento Medicamento { get; set; }
        public HorarioViewModel oHorario { get; set; }
        public MedicamentoPageModel(IMedicamentoRestService restService)
        {
            //_restService = FreshIOC.Container.Resolve<IMedicamentoRestService>();
        }
        public override void Init(object initData)
        {
            base.Init(initData);
            Medicamento = initData as Medicamento;
            oHorario = new HorarioViewModel();
            if (Medicamento == null)
            {
                Medicamento = new Medicamento();
                deleteVisible = false;
                alterar = false;
            }
            else
            {
                alterar = false;
            }
            RaisePropertyChanged("IsVisible");
        }
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            oHorario = new HorarioViewModel();
            if (Medicamento == null)
            {
                Medicamento = new Medicamento();
            }
            else
            {
                oHorario.QuantidadeF = Medicamento.MedQuantidade;
            }
        }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    Medicamento.MedQuantidade = Convert.ToSingle(oHorario.Quantidade);
                    await CuidadorRestService.DefaultManager.SaveMedicamentoAsync(Medicamento, alterar);
                    await CoreMethods.PopPageModel(Medicamento);
                });
            }
        }
        public Command DeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CuidadorRestService.DefaultManager.DeleteMedicamentoAsync(Medicamento);
                    await CoreMethods.PopPageModel(Medicamento);
                });
            }
        }
        //    protected override void ViewIsAppearing(object sender, EventArgs e)
        //    {
        //        base.ViewIsAppearing(sender, e);
        //        if (Medicamento == null)
        //            deleteVisible = false;
        //        else
        //            deleteVisible = true;
        //        RaisePropertyChanged("IsVisible");
        //    }

        //}
    }
}