using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Model;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class FotoPageModel:FreshBasePageModel
    {
        public Foto Foto { get; set; }
        public Cuidador Cuidador { get; set; }
        public Paciente Paciente { get; set; }
        private Foto _selectedFamiliar;
        public ObservableCollection<Familiar> Familiares { get; set; }
        public override void Init(object initData)
        {
            base.Init(initData);
            Cuidador=new Cuidador();
            Paciente=new Paciente();
            var tupla = initData as Tuple<Cuidador,Paciente>;
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
                .Where(e=>e.PacId==Paciente.Id);
                var selection = new ObservableCollection<Familiar>(await CuidadorRestService.DefaultManager.RefreshFamiliarAsync(true))
                .Where(e=> oi.Select(a=>a.FamId)
                .Contains(e.Id)).AsEnumerable();
                Familiares = new ObservableCollection<Familiar>(selection);
            });
        }

        public Foto SelectedFamiliar
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

                    //await CoreMethods.DisplayAlert("File Location", file.Path, "OK");
                    image.Source = ImageSource.FromStream(() =>
                    {
                        Foto = new Foto
                        {
                            FotoDados = Helper.HelperClass.ReadFully(file.GetStream()),
                            FotCuidador = Cuidador.Id
                        };
                        var stream = file.GetStream();
                        file.Dispose();
                        Task.Run(async () =>
                        {
                            var tupla = new Tuple<Foto,Familiar>(Foto,familiar);
                            await CoreMethods.PushPageModel<EnviarFotoPageModel>(tupla);
                            //await CuidadorRestService.DefaultManager.SaveFotoAsync(Foto,true);
                        });
                        return stream;
                    });
                });
            }
        }


    }
}
