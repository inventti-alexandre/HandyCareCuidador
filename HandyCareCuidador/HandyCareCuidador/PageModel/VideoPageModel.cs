using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class VideoPageModel : FreshBasePageModel
    {
        public Video Video { get; set; }
        public Cuidador Cuidador { get; set; }
        public Paciente Paciente { get; set; }
        public HorarioViewModel oHorario { get; set; }
        public ObservableCollection<Parentesco> Parentescos { get; set; }
        private Familiar _selectedFamiliar;
        public ObservableCollection<Familiar> Familiares { get; set; }
        public override void Init(object initData)
        {
            base.Init(initData);
            oHorario = new HorarioViewModel { ActivityRunning = true };
            Cuidador = new Cuidador();
            Paciente = new Paciente();
            var tupla = initData as Tuple<Cuidador, Paciente>;
            if (tupla != null)
            {
                Cuidador = tupla.Item1;
                Paciente = tupla.Item2;
            }
            GetFamiliares();
        }

        private void GetFamiliares()
        {
            Task.Run(async () =>
            {
                var oi = new ObservableCollection<PacienteFamiliar>(await CuidadorRestService.DefaultManager.RefreshPacienteFamiliarAsync(true))
                .Where(e => e.PacId == Paciente.Id);
                var selection = new ObservableCollection<Familiar>(await CuidadorRestService.DefaultManager.RefreshFamiliarAsync(true))
                .Where(e => oi.Select(a => a.FamId)
                .Contains(e.Id)).AsEnumerable();
                var x = new ObservableCollection<Parentesco>(await CuidadorRestService.DefaultManager.RefreshParentescoAsync(true))
                .Where(e => selection.Select(a => a.FamParentesco)
                .Contains(e.Id)).AsEnumerable();
                foreach (var z in selection)
                {
                    foreach (var b in x)
                    {
                        if (z.FamParentesco == b.Id)
                            z.FamDescriParentesco = b.ParDescricao;
                    }
                }
                Parentescos = new ObservableCollection<Parentesco>(x);
                Familiares = new ObservableCollection<Familiar>(selection);
                oHorario.ActivityRunning = false;
            });
        }

        public Familiar SelectedFamiliar
        {
            get { return _selectedFamiliar; }
            set
            {
                _selectedFamiliar = value;
                if (value != null)
                {
                    FamiliarSelected.Execute(value);
                    SelectedFamiliar = null;
                }
            }
        }
        public Command<Familiar> FamiliarSelected
        {
            get
            {
                return new Command<Familiar>(async familiar =>
                {
                    var x = familiar;
                    familiar = null;
                    var result = await CoreMethods.DisplayActionSheet("Forma de Videografia", "Cancelar", null, "Galeria",
                        "Gravar vídeo");

                    if (result == "Gravar vídeo")
                    {
                        var image = new Image();
                        await CrossMedia.Current.Initialize();

                        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                        {
                            await CoreMethods.DisplayAlert("No Camera", ":( No camera available.", "OK");
                            return;
                        }

                        var file = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions
                        {
                            Quality = VideoQuality.Low,
                            DesiredLength = new TimeSpan(0,0,10),
                            Name = DateTime.Now + "HandyCare.mp4",
                            Directory = "Handy Care Videos",
                            SaveToAlbum = true
                        });
                        if (file == null)
                            return;
                        await CoreMethods.DisplayAlert("File Location", file.Path, "OK");
                        Video = new Video
                        {
                            VidDados = Helper.HelperClass.ReadFully(file.GetStream()),
                            VidCuidador = Cuidador.Id,
                        };
                        image.Source = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            file.Dispose();
                            return stream;
                        });
                        var tupla = new Tuple<Video, Familiar, Image>(Video, x, image);
                        await CoreMethods.PushPageModel<EnviarVideoPageModel>(tupla);
                    }
                    else if (result == "Galeria")
                    {
                        var image = new Image();

                        var file = await CrossMedia.Current.PickVideoAsync();
                        if (file == null)
                            return;
                        await CoreMethods.DisplayAlert("File Location", file.Path, "OK");
                        Video = new Video
                        {
                            VidDados = Helper.HelperClass.ReadFully(file.GetStream()),
                            VidCuidador = Cuidador.Id,
                        };
                        image.Source = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            file.Dispose();
                            return stream;
                        });
                        var tupla = new Tuple<Video, Familiar, Image>(Video, x, image);
                        await CoreMethods.PushPageModel<EnviarVideoPageModel>(tupla);
                    }
                });
            }
        }


    }
}
