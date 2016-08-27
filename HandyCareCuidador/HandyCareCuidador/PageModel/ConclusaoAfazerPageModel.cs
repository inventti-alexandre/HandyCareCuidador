﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using PropertyChanged;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class ConclusaoAfazerPageModel : FreshBasePageModel
    {
        //IAfazerRestService _restService;
        //IMaterialRestService _materialRestService;
        //IConclusaoAfazerRestService _concluirRestService;
        //IMaterialUtilizadoRestService _materialUtilizadoRestService;
        //IMedicamentoRestService _medicamentoRestService;
        //private IMedicamentoAdministradoRestService _medicamentoAdministradoRestService;
        public Afazer Afazer { get; set; }
        public bool newItem { get; set; }
        public Material oMaterial { get; set; }
        public Medicamento oMedicamento { get; set; }
        public HorarioViewModel oHorario { get; set; }
        public MaterialUtilizado oMaterialUtilizado { get; set; }
        public ConclusaoAfazer AfazerConcluido { get; set; }
        public ObservableCollection<Material> Materiais { get; set; }
        public ObservableCollection<Material> MateriaisEscolhidos { get; set; }
        public ObservableCollection<Medicamento> Medicamentos { get; set; }
        public ObservableCollection<MedicamentoAdministrado> MedicamentosAdministrados { get; set; }
        public ObservableCollection<MaterialUtilizado> MateriaisUtilizados { get; set; }

        public override async void Init(object initData)
        {
            base.Init(initData);
            var detalheAfazer = initData as Tuple<Afazer, ConclusaoAfazer>;
            MateriaisUtilizados =
                new ObservableCollection<MaterialUtilizado>(
                    await CuidadorRestService.DefaultManager.RefreshMaterialUtilizadoAsync(detalheAfazer?.Item1.Id));
            MedicamentosAdministrados =
                new ObservableCollection<MedicamentoAdministrado>(
                    await
                        CuidadorRestService.DefaultManager.RefreshMedicamentoAdministradoAsync(detalheAfazer?.Item1.Id));
            Materiais =
                (ObservableCollection<Material>)
                    new ObservableCollection<Material>(
                        await CuidadorRestService.DefaultManager.RefreshMaterialExistenteAsync())
                        .Where(m => MateriaisUtilizados.Select(n => n.MatUtilizado).Contains(m.Id));
            Medicamentos =
                (ObservableCollection<Medicamento>)
                    new ObservableCollection<Medicamento>(
                        await CuidadorRestService.DefaultManager.RefreshMedicamentoAsync())
                        .Where(m => MedicamentosAdministrados.Select(n => n.MedAdministrado).Contains(m.Id));
            if (Afazer == null)
            {
                Afazer = new Afazer();
                newItem = true;
            }
            else
            {
                newItem = false;
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