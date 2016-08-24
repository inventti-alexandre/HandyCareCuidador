using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
            btnMapa.Clicked += OnButtonActivated;
        }
        void OnButtonActivated(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            var a = new MapPage();
            button.Navigation.PushModalAsync(a);
        }
    }
}
