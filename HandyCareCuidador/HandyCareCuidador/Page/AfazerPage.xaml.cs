using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class AfazerPage : ContentPage
    {
        public bool deleteVisible;

        public AfazerPage()
        {
            try

            {
                InitializeComponent();
                Hora.Format = String.Format("{0:HH:m:s tt");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /* protected override bool OnBackButtonPressed()
         {
             lstAfazer.SelectedItem = null;
             return base.OnBackButtonPressed();
         }*/
    }
}