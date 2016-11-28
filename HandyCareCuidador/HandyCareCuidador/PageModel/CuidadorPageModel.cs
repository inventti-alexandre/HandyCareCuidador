using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class CuidadorPageModel : FreshBasePageModel
    {
        private TipoCuidador _tipoCuidador;
        private App app;
        private Geoname _selectedEstado;
        public Cuidador Cuidador { get; set; }
        public ContatoEmergencia ContatoEmergencia { get; set; }
        public ConCelular ConCelular { get; set; }
        public bool NovoCuidador { get; set; } = true;
        public ConEmail ConEmail { get; set; }
        public ConTelefone ConTelefone { get; set; }
        public Geoname Cidade { get; set; }
        public ObservableCollection<Geoname> ListaEstados { get; set; }
        public ObservableCollection<Geoname> ListaCidades { get; set; }

        public ObservableCollection<TipoCuidador> TiposCuidadores { get; set; }
        public ValidacaoCuidador ValidacaoCuidador { get; set; }
        public PageModelHelper oHorario { get; set; }
        public ImageSource CuidadorFoto { get; set; }
        public ImageSource Documento { get; set; }

        public TipoCuidador SelectedTipoCuidador
        {
            get { return _tipoCuidador; }
            set
            {
                _tipoCuidador = value;
                if (value != null)
                {
                    //ShowMedicamentos.Execute(value);
                    //SelectedPaciente = null;
                }
            }
        }

        public Command AlterarCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (NovoCuidador)
                        NovoCuidador = false;
                    else
                        NovoCuidador = true;
                });
            }
        }

        public Geoname SelectedEstado
        {
            get { return _selectedEstado; }
            set
            {
                _selectedEstado = value;
                if (value != null)
                {
                    EstadoSelected.Execute(value);
                }
            }
        }

        public Command<Geoname> EstadoSelected
        {
            get
            {
                return new Command<Geoname>(async estado =>
                {
                    var x = new HttpClient();
                    var b = await x.GetStringAsync("http://www.geonames.org/childrenJSON?geonameId=" + estado.geonameId);
                    var o = JsonConvert.DeserializeObject<RootObject>(b);
                    ListaCidades = new ObservableCollection<Geoname>(o.geonames);
                });
            }
        }

        public Command FotoDoc
        {
            get
            {
                return new Command(async () =>
                {
                    var result =
                        await CoreMethods.DisplayActionSheet("Forma de fotografia", "Cancelar", null, "Galeria",
                            "Tirar foto");

                    switch (result)
                    {
                        case "Tirar foto":
                        {
                            var image = new Image();
                            await CrossMedia.Current.Initialize();

                            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                            {
                                await CoreMethods.DisplayAlert("No Camera", ":( No camera available.", "OK");
                                return;
                            }

                            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                            {
                                Directory = "Handy Care Fotos",
                                Name = DateTime.Now + "HandyCareFoto.jpg",
                                CompressionQuality = 10,
                                PhotoSize = PhotoSize.Medium,
                                SaveToAlbum = true
                            });
                            if (file == null)
                                return;
                            await CoreMethods.DisplayAlert("File Location", file.Path, "OK");
                            ValidacaoCuidador.ValDocumento = HelperClass.ReadFully(file.GetStream());
                            Documento = ImageSource.FromStream(() =>
                            {
                                var stream = file.GetStream();
                                file.Dispose();
                                return stream;
                            });
                        }
                            break;
                        case "Galeria":
                        {
                            var image = new Image();

                            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                            {
                                CompressionQuality = 10,
                                PhotoSize = PhotoSize.Medium
                            });
                            if (file == null)
                                return;
                            await CoreMethods.DisplayAlert("File Location", file.Path, "OK");
                            ValidacaoCuidador.ValDocumento = HelperClass.ReadFully(file.GetStream());
                            image.Source = ImageSource.FromStream(() =>
                            {
                                var stream = file.GetStream();
                                file.Dispose();
                                return stream;
                            });
                        }
                            break;
                    }
                });
            }
        }

        public Command FotoCui
        {
            get
            {
                return new Command(async () =>
                {
                    var result =
                        await CoreMethods.DisplayActionSheet("Forma de fotografia", "Cancelar", null, "Galeria",
                            "Tirar foto");

                    switch (result)
                    {
                        case "Tirar foto":
                        {
                            var image = new Image();
                            await CrossMedia.Current.Initialize();

                            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                            {
                                await CoreMethods.DisplayAlert("No Camera", ":( No camera available.", "OK");
                                return;
                            }

                            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                            {
                                Directory = "Handy Care Fotos",
                                Name = DateTime.Now + "HandyCareFoto.jpg",
                                CompressionQuality = 10,
                                PhotoSize = PhotoSize.Medium,
                                SaveToAlbum = true
                            });
                            if (file == null)
                                return;
                            await CoreMethods.DisplayAlert("File Location", file.Path, "OK");
                            Cuidador.CuiFoto = HelperClass.ReadFully(file.GetStream());
                            CuidadorFoto = ImageSource.FromStream(() =>
                            {
                                var stream = file.GetStream();
                                file.Dispose();
                                return stream;
                            });
                        }
                            break;
                        case "Galeria":
                        {
                            var image = new Image();

                            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                            {
                                CompressionQuality = 10,
                                PhotoSize = PhotoSize.Medium
                            });
                            if (file == null)
                                return;
                            await CoreMethods.DisplayAlert("File Location", file.Path, "OK");
                            Cuidador.CuiFoto = HelperClass.ReadFully(file.GetStream());
                            CuidadorFoto = ImageSource.FromStream(() =>
                            {
                                var stream = file.GetStream();
                                file.Dispose();
                                return stream;
                            });
                        }
                            break;
                    }
                });
            }
        }

        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (Cidade.name != null)
                        {
                            Cuidador.CuiCidade = Cidade.name;
                            Cuidador.CuiEstado = SelectedEstado.name;
                        }

                        oHorario.Visualizar = false;
                        oHorario.ActivityRunning = true;
                        if (oHorario.NovoCuidador)
                        {
                            ValidacaoCuidador.Id = Guid.NewGuid().ToString();
                            ValidacaoCuidador.ValValidado = true;
                            Cuidador.CuiContatoEmergencia = ContatoEmergencia.Id;
                            ContatoEmergencia.ConEmail = ConEmail.Id;
                            ContatoEmergencia.ConCelular = ConCelular.Id;
                            ContatoEmergencia.ConTelefone = ConTelefone.Id;
                            ContatoEmergencia.ConTipo = "E15C9314-AD3A-47E1-A89F-BEEBB46460B1";

                            Cuidador.CuiValidacaoCuidador = ValidacaoCuidador.Id;
                            Cuidador.CuiTipoCuidador = SelectedTipoCuidador.Id;

                        }
                        ValidacaoCuidador.Deleted = false;
                        ConCelular.Deleted = false;
                        ConEmail.Deleted = false;
                        ConTelefone.Deleted = false;
                        ContatoEmergencia.Deleted = false;
                        Cuidador.Deleted = false;
                        await Task.Run(async () =>
                        {
                            await
                                CuidadorRestService.DefaultManager.SaveValidacaoCuidadorAsync(ValidacaoCuidador,
                                    oHorario.NovoCuidador);
                            await
                                CuidadorRestService.DefaultManager.SaveConCelularAsync(ConCelular, oHorario.NovoCuidador);
                            await CuidadorRestService.DefaultManager.SaveConEmailAsync(ConEmail, oHorario.NovoCuidador);
                            await
                                CuidadorRestService.DefaultManager.SaveConTelefoneAsync(ConTelefone,
                                    oHorario.NovoCuidador);
                            await
                                CuidadorRestService.DefaultManager.SaveContatoEmergenciaAsync(ContatoEmergencia,
                                    oHorario.NovoCuidador);
                            await CuidadorRestService.DefaultManager.SaveCuidadorAsync(Cuidador, oHorario.NovoCuidador);
                        });

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                    finally
                    {
                        if (oHorario.NovoCuidador)
                            app.AbrirMainMenu(Cuidador);
                        else
                            await CoreMethods.PopPageModel(Cuidador);
                    }
                    //                        await CoreMethods.PushPageModelWithNewNavigation<MainMenuPageModel>(Cuidador);
                });
            }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            Cuidador = new Cuidador();
            SelectedTipoCuidador = new TipoCuidador();
            ValidacaoCuidador = new ValidacaoCuidador();
            oHorario = new PageModelHelper
            {
                ActivityRunning = true,
                Visualizar = false,
                VisualizarTermino = false,
                NovoCuidador = false,
                NovoCadastro = false,
                CuidadorExibicao = true
            };
            var x = initData as Tuple<Cuidador, App>;
            if (x != null)
            {
                Cuidador = x.Item1;
                if (x.Item2 != null)
                    app = x.Item2;
            }
            oHorario.NovoCuidador = Cuidador?.Id == null;
            oHorario.NovoCadastro = Cuidador?.Id == null;
            oHorario.CuidadorExibicao = Cuidador?.Id != null;
            if (oHorario.NovoCuidador)
            {
                oHorario.BoasVindas = "Tirar foto";
                NovoCuidador = true;
                ConTelefone = new ConTelefone { Id = Guid.NewGuid().ToString() };
                ConCelular = new ConCelular { Id = Guid.NewGuid().ToString() };
                ConEmail = new ConEmail { Id = Guid.NewGuid().ToString() };
                ContatoEmergencia = new ContatoEmergencia { Id = Guid.NewGuid().ToString() };
            }
            else
            {
                NovoCuidador = false;
                oHorario.BoasVindas = "Alterar foto";
            }
            Cidade=new Geoname();
            //Cuidador = initData as Cuidador;
            await GetData();
            if (Cuidador?.CuiFoto != null)
            {
                CuidadorFoto = ImageSource.FromStream(() => new MemoryStream(Cuidador.CuiFoto));
            }
            var n = new HttpClient();
            var b = await n.GetStringAsync("http://www.geonames.org/childrenJSON?geonameId=3469034");
            var o = JsonConvert.DeserializeObject<RootObject>(b);
            ListaEstados = new ObservableCollection<Geoname>(o.geonames);

            oHorario.ActivityRunning = false;
            oHorario.Visualizar = true;
        }


        private async Task GetData()
        {
            try
            {
                await Task.Run(async () =>
                {
                    TiposCuidadores =
                        new ObservableCollection<TipoCuidador>(
                            await CuidadorRestService.DefaultManager.RefreshTipoCuidadorAsync());
                    var x = TiposCuidadores.Count;
                    //var x =
                    //    new ObservableCollection<TipoCuidador>(
                    //        await CuidadorRestService.DefaultManager.RefreshTipoCuidadorAsync());
                    if (!oHorario.NovoCuidador)
                    {
                        SelectedTipoCuidador = TiposCuidadores.FirstOrDefault(e => e.Id == Cuidador.CuiTipoCuidador);
                        ValidacaoCuidador = new ObservableCollection<ValidacaoCuidador>(
                                await CuidadorRestService.DefaultManager.RefreshValidacaoCuidadorAsync())
                            .FirstOrDefault(e => e.Id == Cuidador.CuiValidacaoCuidador);
                        if (ValidacaoCuidador?.ValDocumento != null)
                        {
                            Documento = ImageSource.FromStream(() => new MemoryStream(ValidacaoCuidador.ValDocumento));
                        }

                    }
                });
                if (oHorario.NovoCuidador == false)
                {
                    await Task.Run(async () =>
                    {
                        ContatoEmergencia = new ObservableCollection<ContatoEmergencia>(
                            await CuidadorRestService.DefaultManager.RefreshContatoEmergenciaAsync()).FirstOrDefault(
                            e => e.Id == Cuidador.CuiContatoEmergencia);
                        ConCelular = new ObservableCollection<ConCelular>(
                            await CuidadorRestService.DefaultManager.RefreshConCelularAsync()).FirstOrDefault(e => e.Id == ContatoEmergencia.ConCelular);
                        ConEmail = new ObservableCollection<ConEmail>(
    await CuidadorRestService.DefaultManager.RefreshConEmailAsync()).FirstOrDefault(e => e.Id == ContatoEmergencia.ConEmail);
                        ConTelefone = new ObservableCollection<ConTelefone>(
    await CuidadorRestService.DefaultManager.RefreshConTelefoneAsync()).FirstOrDefault(e => e.Id == ContatoEmergencia.ConTelefone);
                    });
                }

            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}