﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    public class MainMenuPageModel : FreshBasePageModel
    {
        private Paciente _selectedPaciente;
        public Image image;
        private Cuidador Cuidador { get; set; }
        public ObservableCollection<Paciente> Pacientes { get; set; }
        public PageModelHelper oHorario { get; set; }
        public Paciente oPaciente { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        public ObservableCollection<CuidadorPaciente> CuidadoresPacientes { get; set; }

        public Command ShowFoto
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        var tupla = new Tuple<Cuidador, Paciente>(Cuidador, SelectedPaciente);
                        await CoreMethods.PushPageModel<FotoPageModel>(tupla);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Command ShowVideo
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        var tupla = new Tuple<Cuidador, Paciente>(Cuidador, SelectedPaciente);
                        await CoreMethods.PushPageModel<VideoPageModel>(tupla);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Command ShowCuidador
        {
            get
            {
                return new Command(async () =>
                {
                    var x = new Tuple<Cuidador, App>(Cuidador, null);
                    await CoreMethods.PushPageModel<CuidadorPageModel>(x);
                });
            }
        }

        public Command ShowAfazeres
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        CuidadorPaciente = CuidadoresPacientes.FirstOrDefault(e => e.PacId == SelectedPaciente.Id);
                        var x = new Tuple<Paciente, CuidadorPaciente>(SelectedPaciente, CuidadorPaciente);
                        await CoreMethods.PushPageModel<ListaAfazerPageModel>(x);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Command ShowPaciente
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        var x = new Tuple<Paciente, bool, Cuidador>(SelectedPaciente, false, Cuidador);
                        await CoreMethods.PushPageModel<PacientePageModel>(x);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Command AddPaciente
        {
            get
            {
                return new Command(async () =>
                {
                    var x = new Tuple<Paciente, bool, Cuidador>(SelectedPaciente, true, Cuidador);
                    await CoreMethods.PushPageModel<PacientePageModel>(x);
                });
            }
        }

        public Command AlertarContatos
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        var tupla = new Tuple<Cuidador, Paciente>(Cuidador, SelectedPaciente);
                        await CoreMethods.PushPageModel<AcionarContatoEmergencia>(tupla);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Command ShowMateriais
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        CuidadorPaciente = CuidadoresPacientes.FirstOrDefault(e => e.PacId == SelectedPaciente.Id);
                        var x = new Tuple<Paciente,CuidadorPaciente>(SelectedPaciente,CuidadorPaciente);
                        await CoreMethods.PushPageModel<ListaMaterialPageModel>(x);
                    }
                    else
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                });
            }
        }

        public Paciente SelectedPaciente
        {
            get { return _selectedPaciente; }
            set
            {
                _selectedPaciente = value;
                if (value != null)
                {
                    //ShowMedicamentos.Execute(value);
                    //SelectedPaciente = null;
                }
            }
        }

        public Command ShowMedicamentos
        {
            get
            {
                return new Command(async () =>
                {
//ENVIAR ID DO PACIENTE
                    if (SelectedPaciente != null)
                    {
                        CuidadorPaciente = CuidadoresPacientes.FirstOrDefault(e => e.PacId == SelectedPaciente.Id);
                        var x = new Tuple<Paciente, CuidadorPaciente>(SelectedPaciente, CuidadorPaciente);
                        await CoreMethods.PushPageModel<ListaMedicamentoPageModel>(x);
                    }
                    else
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                });
            }
        }

        public Command ShowMapa
        {
            get { return new Command(async () => { await CoreMethods.PushPageModel<MapPageModel>(); }); }
        }

        public void ShowImage(string filepath)
        {
            image.Source = ImageSource.FromFile(filepath);
        }

        public override void Init(object initData)
        {
            try
            {
                base.Init(initData);
                Cuidador = new Cuidador();
                Cuidador = initData as Cuidador;
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Ih, rapaz {0}", e.Message);
                throw;
            }
        }

        protected override async void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            oPaciente = new Paciente();
            oHorario = new PageModelHelper
            {
                ActivityRunning = true,
                Visualizar = false,
                BoasVindas = "Olá, " + Cuidador.CuiNomeCompleto
            };
            await GetPacientes();
            //MedImage = new Image {Source = ImageSource.FromFile("pills.png")};
        }

        private async Task GetPacientes()
        {
            try
            {
                await Task.Run(async () =>
                {
                    //CuidadorRestService.DefaultManager.RefreshPacienteAsync();
                    //CuidadorPaciente = new CuidadorPaciente();
                    //CuidadorPaciente =
                    //    new ObservableCollection<CuidadorPaciente>(
                    //        await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync()).FirstOrDefault(
                    //            e => e.CuiId == Cuidador.Id);
                    var cuidadoresPacientes = new ObservableCollection<CuidadorPaciente>(
                        await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync()).Where(
                        e => e.CuiId == Cuidador.Id).AsEnumerable();
                    CuidadoresPacientes = new ObservableCollection<CuidadorPaciente>(cuidadoresPacientes);
                    if (CuidadoresPacientes.Any())
                    {
                        var result =
                            new ObservableCollection<Paciente>(
                                CuidadorRestService.DefaultManager.RefreshPacienteAsync().Result);
                        var resulto =
                            result.Where(e => CuidadoresPacientes.Select(m => m.PacId).Contains(e.Id)).AsEnumerable();
                        Pacientes = new ObservableCollection<Paciente>(resulto);
                    }
                    oHorario.ActivityRunning = false;
                    oHorario.Visualizar = true;
                });
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }
    }
}