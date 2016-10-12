using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class EnviarVideoPage : ContentPage
    {
        public EnviarVideoPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}