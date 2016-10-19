﻿using System;
using System.Collections.ObjectModel;
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
    public class MedicamentoPageModel : FreshBasePageModel
    {
        public bool alterar;
        //IMedicamentoRestService _restService;
        public bool deleteVisible = true;

        public Medicamento Medicamento { get; set; }

        public ObservableCollection<ViaAdministracaoMedicamento> Vias { get; set; }
        //public ViaAdministracaoMedicamento ViaAdministracaoMedicamento { get; set; }
        //public FormaApresentacaoMedicamento FormaApresentacaoMedicamento { get; set; }
        public ObservableCollection<FormaApresentacaoMedicamento> Formas { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        public Image MedImage { get; set; }
        public PageModelHelper oHorario { get; set; }
        public ViaAdministracaoMedicamento oViaAdministracaoMedicamento { get; set; }

        public FormaApresentacaoMedicamento oFormaApresentacaoMedicamento { get; set; }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    oHorario.Visualizar = false;
                    oHorario.ActivityRunning = true;
                    Medicamento.MedQuantidade = Convert.ToSingle(oHorario.Quantidade);
                    Medicamento.MedPacId = CuidadorPaciente.Id;
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
                    oHorario.Visualizar = false;
                    oHorario.ActivityRunning = true;
                    await CuidadorRestService.DefaultManager.DeleteMedicamentoAsync(Medicamento);
                    await CoreMethods.PopPageModel(Medicamento);
                });
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            oHorario = new PageModelHelper
            {
                QuantidadeF = null
            };

            var x = initData as Tuple<Medicamento, CuidadorPaciente>;
            Medicamento = new Medicamento();
            CuidadorPaciente = new CuidadorPaciente();
            GetInfoMateriais();
            //FormaApresentacaoMedicamento = new FormaApresentacaoMedicamento();
            //ViaAdministracaoMedicamento = new ViaAdministracaoMedicamento();
            if (x != null)
            {
                Medicamento = x.Item1;
                CuidadorPaciente = x.Item2;
                oHorario.QuantidadeF = Medicamento.MedQuantidade;
            }
            if (Medicamento.Id == null)
            {
                Medicamento = new Medicamento();
                oHorario.deleteVisible = false;
                alterar = false;
            }
            else
            {
                oHorario.deleteVisible = true;
                alterar = false;
            }
        }

        private void GetInfoMateriais()
        {
            Task.Run(async () =>
            {
                Vias =
                    new ObservableCollection<ViaAdministracaoMedicamento>(
                        await CuidadorRestService.DefaultManager.RefreshViaAdministracaoMedicamentoAsync());
                var x = Vias.Count;
                Formas =
                    new ObservableCollection<FormaApresentacaoMedicamento>(
                        await CuidadorRestService.DefaultManager.RefreshFormaApresentacaoMedicamentoAsync());
                var y = Formas.Count;
            });
            oHorario.ActivityRunning = false;
            oHorario.Visualizar = true;
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
        }
    }
}