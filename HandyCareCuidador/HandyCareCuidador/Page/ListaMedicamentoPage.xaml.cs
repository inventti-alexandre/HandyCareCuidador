﻿using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class ListaMedicamentoPage : ContentPage
    {
        public ListaMedicamentoPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var lstMedicamento = this.FindByName<ListView>("lstMedicamento");

            if (lstMedicamento != null)
                lstMedicamento.ClearValue(ListView.SelectedItemProperty);
        }
    }
}