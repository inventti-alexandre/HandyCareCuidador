using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
            //btnMapa.Clicked += OnButtonActivated;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var x = new ImageSourceConverter();
            //x.ConvertFromInvariantString();
            btnAfazer.Text = "Afazeres";
            btnMedicamento.Text = "Medicamento";
            btnPaciente.Text = "Paciente";
            btnContato.Text = "Acionar contatos";
            btnFoto.Text = "Foto";
            btnInfo.Text = "Suas informações";
            btnMaterial.Text = "Material";
            btnVideo.Text = "Vídeo";
            btnMapa.Text = "Mapa";

            btnAfazer.Image = (FileImageSource) ImageSource.FromFile("calendar.png");
            btnMedicamento.Image = (FileImageSource) ImageSource.FromFile("pills.png");
            btnPaciente.Image = (FileImageSource) ImageSource.FromFile("patient.png");
            btnContato.Image = (FileImageSource) ImageSource.FromFile("smartphone.png");
            btnFoto.Image = (FileImageSource) ImageSource.FromFile("photo.png");
            btnInfo.Image = (FileImageSource) ImageSource.FromFile("info.png");
            btnMaterial.Image = (FileImageSource) ImageSource.FromFile("glove.png");
            btnVideo.Image = (FileImageSource) ImageSource.FromFile("video.png");
            btnMapa.Image = (FileImageSource) ImageSource.FromFile("map.png");
        }

        //{

        //private void OnButtonActivated(object sender, EventArgs args)
        //    var button = (Button) sender;
        //    var a = new MapPage();
        //    button.Navigation.PushModalAsync(a);
        //}
    }
}