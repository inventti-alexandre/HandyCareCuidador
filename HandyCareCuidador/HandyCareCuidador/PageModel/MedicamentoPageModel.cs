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
    public class MedicamentoPageModel : FreshBasePageModel
    {
        public bool alterar;
        //IMedicamentoRestService _restService;
        public bool deleteVisible = true;

        public Medicamento Medicamento { get; set; }
        public Paciente Paciente { get; set; }
        public HorarioViewModel oHorario { get; set; }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    Medicamento.MedQuantidade = Convert.ToSingle(oHorario.Quantidade);
                    Medicamento.MedPacId = Paciente.Id;
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

        public override void Init(object initData)
        {
            base.Init(initData);
            var x = initData as Tuple<Medicamento, Paciente>;
            Medicamento = new Medicamento();
            Paciente = new Paciente();
            if (x != null)
            {
                Medicamento = x.Item1;
                Paciente = x.Item2;
            }
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

        //    }
        //        RaisePropertyChanged("IsVisible");
        //            deleteVisible = true;
        //        else
        //            deleteVisible = false;
        //        if (Medicamento == null)
        //        base.ViewIsAppearing(sender, e);
        //    {
        //    protected override void ViewIsAppearing(object sender, EventArgs e)

        //}
    }
}