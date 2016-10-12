using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class FotoPageModel : FreshBasePageModel
    {
        private Familiar _selectedFamiliar;
        public Foto Foto { get; set; }
        public Cuidador Cuidador { get; set; }
        public Paciente Paciente { get; set; }
        public PageModelHelper oHorario { get; set; }
        public ObservableCollection<Parentesco> Parentescos { get; set; }
        public ObservableCollection<Familiar> Familiares { get; set; }

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
                    var result =
                        await CoreMethods.DisplayActionSheet("Forma de fotografia", "Cancelar", null, "Galeria",
                            "Tirar foto");


                    if (result == "Tirar foto")
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
                            Name = DateTime.Now.ToString() + "HandyCareFoto.jpg",
                            CompressionQuality = 10,
                            PhotoSize = PhotoSize.Medium,
                            SaveToAlbum = true
                        });
                        if (file == null)
                            return;
                        await CoreMethods.DisplayAlert("File Location", file.Path, "OK");
                        Foto = new Foto
                        {
                            FotoDados = HelperClass.ReadFully(file.GetStream()),
                            FotCuidador = Cuidador.Id
                        };
                        image.Source = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            file.Dispose();
                            return stream;
                        });
                        var tupla = new Tuple<Foto, Familiar, Image>(Foto, x, image);
                        await CoreMethods.PushPageModel<EnviarFotoPageModel>(tupla);
                    }
                    else if (result == "Galeria")
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
                        Foto = new Foto
                        {
                            FotoDados = HelperClass.ReadFully(file.GetStream()),
                            FotCuidador = Cuidador.Id
                        };
                        image.Source = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            file.Dispose();
                            return stream;
                        });
                        var tupla = new Tuple<Foto, Familiar, Image>(Foto, x, image);
                        await CoreMethods.PushPageModel<EnviarFotoPageModel>(tupla);
                    }
                });
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            oHorario = new PageModelHelper {ActivityRunning = true};
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
                var oi = new ObservableCollection<PacienteFamiliar>(
                        await CuidadorRestService.DefaultManager.RefreshPacienteFamiliarAsync(true))
                    .Where(e => e.PacId == Paciente.Id);
                var selection =
                    new ObservableCollection<Familiar>(
                            await CuidadorRestService.DefaultManager.RefreshFamiliarAsync(true))
                        .Where(e => oi.Select(a => a.FamId)
                            .Contains(e.Id)).AsEnumerable();
                var x =
                    new ObservableCollection<Parentesco>(
                            await CuidadorRestService.DefaultManager.RefreshParentescoAsync(true))
                        .Where(e => selection.Select(a => a.FamParentesco)
                            .Contains(e.Id)).AsEnumerable();
                foreach (var z in selection)
                    foreach (var b in x)
                        if (z.FamParentesco == b.Id)
                            z.FamDescriParentesco = b.ParDescricao;

                Parentescos = new ObservableCollection<Parentesco>(x);
                Familiares = new ObservableCollection<Familiar>(selection);
                oHorario.ActivityRunning = false;
            });
        }
    }
}