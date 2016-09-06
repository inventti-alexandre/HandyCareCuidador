﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class AcionarContatoEmergencia : ContentPage
    {
        public AcionarContatoEmergencia()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var lstFamiliares = this.FindByName<ListView>("lstFamiliares");
            lstFamiliares?.ClearValue(ListView.SelectedItemProperty);
        }

    }
}
