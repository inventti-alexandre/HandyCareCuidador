using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    public class MainMenuPageModel : FreshBasePageModel
    {
        private Paciente _selectedPaciente;
        //public event Action TakePicture = () => { };

        //private IPacienteRestService _pacienteRestService;
        //private ICuidadorPacienteRestService _cuidadorPacienteRestService;
        //private ICuidadorRestService _cuidadorRestService;
        private Cuidador Cuidador { get; set; }
        private Foto Foto { get; set; }
        public ObservableCollection<Paciente> Pacientes { get; set; }
        public HorarioViewModel oHorario { get; set; }
        public Paciente oPaciente { get; set; }
        public ObservableCollection<CuidadorPaciente> CuidadorPacientes { get; set; }
        public CuidadorPaciente CuidadorPaciente { get; set; }
        public Image image;
        public void ShowImage(string filepath)
        {
            image.Source = ImageSource.FromFile(filepath);
        }
        public Command TirarFoto
        {
            get
            {
                return new Command(async () =>
                {
                    var image = new Image();
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await CoreMethods.DisplayAlert("No Camera", ":( No camera available.", "OK");
                        return;
                    }

                    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "Handy Care",
                        Name = DateTime.Now.ToString() + "HandyCareFoto.jpg",
                        CompressionQuality = 70,
                        PhotoSize = PhotoSize.Small,
                        SaveToAlbum = true
                    });

                    if (file == null)
                        return;

                    await CoreMethods.DisplayAlert("File Location", file.Path, "OK");
                    image.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        //file.Dispose();
                        return stream;
                    });
                    if (await CoreMethods.DisplayActionSheet("Deseja enviar a foto para um familiar", "Não", null, "Ok") == "Ok")
                    {
                        Foto = new Foto();
                        Foto.FotoDados = Helper.HelperClass.ReadFully(file.GetStream());
                        //Foto.FotoDados
                    }
                    //var app = Xamarin.Forms.Application.Current as App;
                    //app?.PictureEventHandler();
                });
            }
        }
        public Command GravarVideo
        {
            get
            {
                return new Command(async () =>
                {
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
                    {
                        await CoreMethods.DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                        return;
                    }

                    var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                    {
                        Name = DateTime.Now.ToString()+"HandyCareVideo.mp4",
                        Directory = "Handy Care",
                        DesiredLength = new TimeSpan(0,0,0,10),
                        Quality = VideoQuality.Medium
                    });

                    if (file == null)
                        return;

                    await CoreMethods.DisplayAlert("Video Recorded", "Location: " + file.Path, "OK");

                    file.Dispose();
                    //var app = Xamarin.Forms.Application.Current as App;
                    //app?.VideoEventHandler();
                });
            }
        }
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


        public Command ShowCuidador
        {
            get
            {
                return new Command(async () =>
                {
                        await CoreMethods.PushPageModel<CuidadorPageModel>(Cuidador);
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
                        await CoreMethods.PushPageModel<ListaAfazerPageModel>(SelectedPaciente);
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
                        await CoreMethods.PushPageModel<PacientePageModel>(SelectedPaciente);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
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
                        //ENVIAR PUSH NOTIFICATION PARA OS FAMILIARES
                        Device.OpenUri(new Uri("tel:0"));
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
                        await CoreMethods.PushPageModel<ListaMaterialPageModel>(SelectedPaciente);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
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
                        await CoreMethods.PushPageModel<ListaMedicamentoPageModel>(SelectedPaciente);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }
        public Command ShowMapa
        {
            get
            {
                return new Command(async () =>
                {
                        await CoreMethods.PushPageModel<MapPageModel>();
                });
            }
        }

        public override void Init(object initData)
        {
            try
            {
                base.Init(initData);
                Cuidador=new Cuidador();
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
            oHorario = new HorarioViewModel {ActivityRunning = true, Visualizar = false};
            await GetPacientes();
        }

        private async Task GetPacientes()
        {
            try
            {
                await Task.Run(async () =>
                {
                    //CuidadorRestService.DefaultManager.RefreshPacienteAsync();
                    CuidadorPaciente = new CuidadorPaciente();
                    var result =
                        new ObservableCollection<Paciente>(
                            await CuidadorRestService.DefaultManager.RefreshPacienteAsync());
                    CuidadorPaciente =
                        new ObservableCollection<CuidadorPaciente>(
                            await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync()).FirstOrDefault(
                                e => e.CuiId == Cuidador.Id);
                    if (result.Count > 0)
                    {
                        var pacientes =
                            result.Where(e => CuidadorPaciente.PacId.Contains(e.Id)).AsEnumerable();
                        Pacientes= new ObservableCollection<Paciente>();
                        Pacientes = new ObservableCollection<Paciente>(pacientes);
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