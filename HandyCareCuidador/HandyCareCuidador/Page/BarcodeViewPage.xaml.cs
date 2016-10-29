using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace HandyCareCuidador.Page
{
    public partial class BarcodeViewPage : ContentPage
    {
        public BarcodeViewPage()
        {
            InitializeComponent();
            Barcodigo.BarcodeOptions.Width = 600;
            Barcodigo.BarcodeOptions.Height = 600;
            Barcodigo.Aspect = Aspect.AspectFit;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            try
            {
                Barcodigo.RemoveBinding(ZXingBarcodeImageView.BarcodeValueProperty);
                Barcodigo.BarcodeValue = "0";
            }
            catch (ArgumentException)
            {
                Debug.WriteLine("Teste");
            }
        }
    }
}
